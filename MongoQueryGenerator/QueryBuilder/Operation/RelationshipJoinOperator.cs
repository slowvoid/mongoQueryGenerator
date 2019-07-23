using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using QueryBuilder.ER;
using QueryBuilder.Javascript;
using QueryBuilder.Map;
using QueryBuilder.Mongo;
using QueryBuilder.Mongo.Aggregation.Operators;
using QueryBuilder.Mongo.Expressions;
using QueryBuilder.Operation.Arguments;
using QueryBuilder.Operation.Exceptions;
using QueryBuilder.Query;
using QueryBuilder.Shared;

namespace QueryBuilder.Operation
{
    /// <summary>
    /// Represents a JOIN operation
    /// </summary>
    public class RelationshipJoinOperator : AlgebraOperator
    {
        #region Properties
        /// <summary>
        /// Source entity
        /// </summary>
        public Entity SourceEntity { get; set; }
        /// <summary>
        /// What is to be joined
        /// </summary>
        public List<RelationshipJoinArguments> Targets { get; set; }
        #endregion

        #region Methods
        /// <summary>
        /// Run the algorithm
        /// </summary>
        /// <param name="LastResult"></param>
        /// <returns></returns>
        public override AlgebraOperatorResult Run()
        {
            // Locate rules for source entity
            MapRule SourceRule = ModelMap.Rules.First( Rule => Rule.Source.Name == SourceEntity.Name && Rule.IsMain );
            // If not found, throw error
            if ( SourceRule == null )
            {
                throw new InvalidOperationException( $"Entity {SourceEntity.Name} does not have a main map." );
            }

            // Store all operations that must be executed before returning
            List<MongoDBOperator> OperationsToExecute = new List<MongoDBOperator>();

            // Process each joined pair of relationship + computed entity
            foreach ( RelationshipJoinArguments TargetData in Targets )
            {
                // Retrieve relationship rules (if any)
                MapRule RelationshipRule = ModelMap.Rules.FirstOrDefault( Rule => Rule.Source.Name == TargetData.Relationship.Name );

                // Processing may vary based on relationship cardinality
                if ( TargetData.Relationship.Cardinality == RelationshipCardinality.OneToOne )
                {
                    // List of operations to be executed by this relationship
                    List<MongoDBOperator> RelationshipOperations = new List<MongoDBOperator>();

                    // Store info regarding wheter the relationship attributes (if any) were already mapped
                    bool HasRelationshipBeenProcessed = false;
                    string RelationshipAttributesField = $"data_{TargetData.Relationship.Name}";
                    // Store all fields that should be merged under data_Relationship
                    List<string> FieldsToMerge = new List<string>();

                    // Process each entity to be joined
                    foreach ( Entity TargetEntity in TargetData.Targets )
                    {
                        // If the target is a ComputedEntity
                        // Run another RJOINOperator
                        if ( TargetEntity is ComputedEntity TargetAsCE )
                        {
                            // Retrieve the CE source entity before calling another RJOIN instance
                            // Retreive CE-SE rule
                            MapRule CESERule = ModelMap.Rules.First( Rule => Rule.Source.Name == TargetAsCE.SourceEntity.Name && Rule.IsMain );
                            // Stop if rule not found
                            if ( CESERule == null )
                            {
                                throw new RuleNotFoundException( $"Entity [{TargetAsCE.SourceEntity.Name}] is not mapped." );
                            }

                            // Left side entities cannot be embedded
                            if ( SourceRule.Target.Name == CESERule.Target.Name )
                            {
                                throw new InvalidMapException( $"Entities that are the left operand of a join operation cannot be embedded into another entity. [{TargetAsCE.SourceEntity.Name}" );
                            }

                            // Check if the entities are related
                            if ( !TargetData.Relationship.HasRelation( SourceEntity, TargetAsCE.SourceEntity ) )
                            {
                                throw new NotRelatedException( $"Entity {SourceEntity.Name} is not related to {TargetAsCE.SourceEntity.Name} through {TargetData.Relationship.Name}" );
                            }

                            // Retrieve connection rules for this entity
                            RelationshipConnection ConnRules = TargetData.Relationship.GetRelation( SourceEntity, TargetAsCE.SourceEntity );

                            // Start a lookup operation with custom pipeline
                            List<MongoDBOperator> CEPipeline = new List<MongoDBOperator>();

                            // Pipeline variables
                            Dictionary<string, string> CEPipelineVars = new Dictionary<string, string>();
                            string SourceIdentifier = $"{SourceEntity.Name.ToLower()}_identifier";
                            string SourceIdentifierValue = SourceRule.Rules.First( Rule => Rule.Key == ConnRules.SourceAttribute.Name ).Value;

                            string TargetRuleValue = CESERule.Rules.First( Rule => Rule.Key == ConnRules.TargetAttribute.Name ).Value;

                            CEPipelineVars.Add( SourceIdentifier, $"${SourceIdentifierValue}" );

                            // Match source with incoming
                            Expr MatchExpr = new Expr( new EqExpr( $"$${SourceIdentifier}", $"${TargetRuleValue}" ) );
                            MatchOperator MatchOp = new MatchOperator( MatchExpr );

                            CEPipeline.Add( MatchOp );

                            // Setup a new Argument instance
                            RelationshipJoinArguments newArgs = new RelationshipJoinArguments(
                                TargetAsCE.Relationship,
                                TargetAsCE.TargetEntities );

                            RelationshipJoinOperator TargetCEOperator = new RelationshipJoinOperator(
                                SourceEntity,
                                new List<RelationshipJoinArguments> { newArgs },
                                ModelMap );

                            CEPipeline.AddRange( TargetCEOperator.Run().Commands );

                            // Create lookup
                            LookupOperator LookupOp = new LookupOperator
                            {
                                From = CESERule.Target.Name,
                                Let = CEPipelineVars,
                                Pipeline = CEPipeline,
                                As = $"data_{TargetEntity.Name}"
                            };

                            OperationsToExecute.Add( LookupOp );

                            // Add Field to merge list
                            FieldsToMerge.Add( LookupOp.As );
                        }
                        else
                        {
                            MapRule TargetRule = ModelMap.Rules.First( Rule => Rule.Source.Name == TargetEntity.Name );

                            if ( !TargetData.Relationship.HasRelation( SourceEntity, TargetEntity ) )
                            {
                                throw new NotRelatedException( $"Entities [{SourceEntity.Name}] and [{TargetEntity.Name}] are not related through {TargetData.Relationship.Name}" );
                            }

                            RelationshipConnection ConnRules = TargetData.Relationship.GetRelation( SourceEntity, TargetEntity );
                            // Throw error if rule is not found
                            if ( TargetRule == null )
                            {
                                throw new RuleNotFoundException( $"Entity [{TargetEntity.Name}] is not mapped." );
                            }

                            // Check if the target is embedded into the source
                            if ( SourceRule.Target.Name == TargetRule.Target.Name )
                            {
                                // Add target fields to a single attribute
                                Dictionary<string, JSCode> TargetFields = new Dictionary<string, JSCode>();
                                List<string> RootAttributes = new List<string>();

                                foreach ( DataAttribute Attribute in TargetEntity.Attributes )
                                {
                                    // Find root attribute (if any)
                                    string RuleValue = TargetRule.Rules.FirstOrDefault( Rule => Rule.Key == Attribute.Name ).Value;

                                    if ( RuleValue != null )
                                    {
                                        string[] RulePath = RuleValue.Split( new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries );
                                        if ( RulePath.Length > 1 )
                                        {
                                            // Using dot notation, first element is the root attribute
                                            RootAttributes.Add( RulePath.First() );
                                        }
                                        else
                                        {
                                            RootAttributes.Add( RuleValue ); // Attribute is first level
                                        }

                                        // Add new field
                                        // Using dot notation tells MongoDB that it is an object
                                        TargetFields.Add( $"\"data_{TargetEntity.Name}.{TargetEntity.Name}_{Attribute.Name}\"",
                                            (JSString)$"\"${RuleValue}\"" );
                                    }
                                }

                                // Remove attribute mapped to data_Entity
                                Dictionary<string, ProjectExpression> RemoveAttributes = new Dictionary<string, ProjectExpression>();
                                // Lookup data from the RootAttributes list
                                foreach ( string Attribute in RootAttributes.Distinct() )
                                {
                                    RemoveAttributes.Add( Attribute, new BooleanExpr( false ) );
                                }

                                // Setup MongoDB operators
                                AddFieldsOperator AddOp = new AddFieldsOperator( TargetFields );
                                ProjectOperator ProjectOp = new ProjectOperator( RemoveAttributes );

                                // Add to execution list
                                RelationshipOperations.AddRange( new MongoDBOperator[] { AddOp, ProjectOp } );
                            }
                            else
                            {
                                // Entity is not embedded
                                string LookupTargetAs = $"data_{TargetEntity.Name}Join";
                                // Fetch it
                                LookupOperator LookupOp = new LookupOperator
                                {
                                    From = TargetRule.Target.Name,
                                    ForeignField = TargetRule.Rules.First( Rule => Rule.Key == ConnRules.TargetAttribute.Name ).Value,
                                    LocalField = SourceRule.Rules.First( Rule => Rule.Key == ConnRules.SourceAttribute.Name ).Value,
                                    As = LookupTargetAs
                                };

                                // Unwind data (becomes a single object)
                                UnwindOperator UnwindOp = new UnwindOperator( LookupTargetAs, false );

                                // Build a new field to match algebra definition
                                Dictionary<string, JSCode> TargetFields = new Dictionary<string, JSCode>();

                                foreach ( DataAttribute Attribute in TargetEntity.Attributes )
                                {
                                    string RuleValue = TargetRule.Rules.First( Rule => Rule.Key == Attribute.Name ).Value;
                                    TargetFields.Add( $"\"data_{TargetEntity.Name}.{TargetEntity.Name}_{Attribute.Name}\"", (JSString)$"\"${LookupTargetAs}.{RuleValue}\"" );
                                }

                                // Check if relationship attributes are mapped to this entity
                                if ( TargetData.Relationship.Attributes.Count > 0 && RelationshipRule.Target.Name == TargetRule.Target.Name )
                                {
                                    // Map attributes to relationship object
                                    foreach ( DataAttribute Attribute in TargetData.Relationship.Attributes )
                                    {
                                        string RuleValue = RelationshipRule.Rules.First( Rule => Rule.Key == Attribute.Name ).Value;
                                        TargetFields.Add( $"\"{RelationshipAttributesField}.{TargetData.Relationship.Name}_{Attribute.Name}\"", (JSString)$"\"${LookupTargetAs}.{RuleValue}\"" );
                                    }

                                    // Mark relationship attributes as processed
                                    HasRelationshipBeenProcessed = true;
                                }

                                AddFieldsOperator AddOp = new AddFieldsOperator( TargetFields );

                                // Remove unwanted attributes
                                ProjectOperator ProjectOp = ProjectOperator.HideAttributesOperator( new string[] { LookupTargetAs } );

                                RelationshipOperations.AddRange( new MongoDBOperator[] { LookupOp, UnwindOp, AddOp, ProjectOp } );
                            }
                        }
                    }

                    // Process entity attributes (if any)
                    if ( TargetData.Relationship.Attributes.Count > 0 )
                    {
                        // Process
                    }

                    // Remove these fields
                    List<string> FieldsToRemove = new List<string>();
                    // Merge Fields
                    MergeObjectsOperator MergeOp = new MergeObjectsOperator();
                    foreach ( string Field in FieldsToMerge )
                    {
                        MergeOp.Objects.Add( $"${Field}" );
                        FieldsToRemove.Add( Field );
                    }
                    
                    // Build operation to hide all entity/relationship data attributes
                    Dictionary<string, JSCode> BuildJoinData = new Dictionary<string, JSCode>();
                    BuildJoinData.Add( $"data_{TargetData.Relationship.Name}", new JSArray( new List<object> { MergeOp.ToJSCode() } ) );

                    AddFieldsOperator BuildOp = new AddFieldsOperator( BuildJoinData );
                    ProjectOperator HideFields = ProjectOperator.HideAttributesOperator( FieldsToRemove );

                    OperationsToExecute.AddRange( new List<MongoDBOperator> { BuildOp, HideFields } );
                }
                else if ( TargetData.Relationship.Cardinality == RelationshipCardinality.OneToMany )
                { }
                else if ( TargetData.Relationship.Cardinality == RelationshipCardinality.ManyToMany )
                { }
            }

            // Return operations
            return new AlgebraOperatorResult( OperationsToExecute );
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Initialize a new instance of RelationshipJoinOperator
        /// </summary>
        /// <param name="SourceEntity"></param>
        /// <param name="Targets"></param>
        /// <param name="Map"></param>
        public RelationshipJoinOperator( Entity SourceEntity, List<RelationshipJoinArguments> Targets, ModelMapping Map ) : base( Map )
        {
            this.SourceEntity = SourceEntity;
            this.Targets = Targets;
        }
        #endregion
    }
}
