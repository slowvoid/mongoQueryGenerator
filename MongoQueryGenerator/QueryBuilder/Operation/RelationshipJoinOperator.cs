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
        /// Relationship startpoint
        /// </summary>
        public Entity SourceEntity { get; set; }
        /// <summary>
        /// Relationship endpoint
        /// </summary>
        public List<Entity> TargetEntities { get; set; }
        /// <summary>
        /// Relationship connecting both entities
        /// </summary>
        public Relationship Relationship { get; set; }
        #endregion

        #region Private Data
        
        #endregion

        #region Methods
        public override void Run( ref AlgebraOperatorResult LastResult )
        {
            // Retrieve mapping rules for Source Entity and Relationship
            MapRule SourceRule = ModelMap.Rules.First( Rule => Rule.Source.Name == SourceEntity.Name );
            MapRule RelationshipRule = ModelMap.Rules.FirstOrDefault( Rule => Rule.Source.Name == Relationship.Name );

            // Check if the relationship has attributes
            bool RelationshipHasAttributes = Relationship.Attributes.Count > 0;

            List<MongoDBOperator> OperationsToExecute = new List<MongoDBOperator>();

            string joinedAttributeName = $"data_{Relationship.Name}";

            if ( Relationship.Cardinality == RelationshipCardinality.OneToOne )
            {
                // Keep list of operators to run
                List<MongoDBOperator> OneToOneOperatorsToExecute = new List<MongoDBOperator>();
                // Store info regarding whether relationship attributes (if any) were already mapped
                bool HasRelationshipBeenProcessed = false;
                string RelationshipAttributesField = $"data_{Relationship.Name}Attributes";

                // Work on each entity
                foreach ( Entity TargetEntity in TargetEntities )
                {
                    if ( TargetEntity is ComputedEntity )
                    {
                        // TODO:
                    }
                    else
                    {
                        // Retrieve map rules from the target entity
                        MapRule TargetRule = ModelMap.Rules.First( Rule => Rule.Source.Name == TargetEntity.Name );

                        // Get relationship data
                        RelationshipConnection RelationshipData = Relationship.GetRelation( SourceEntity, TargetEntity );

                        if ( RelationshipData == null )
                        {
                            throw new ImpossibleOperationException( $"Entity {TargetEntity.Name} is not reachable through {Relationship.Name}" );
                        }

                        // Check if the source entity and target entity shares the collection
                        // which means one is embedded in the other
                        if ( SourceRule.Target.Name == TargetRule.Target.Name )
                        {
                            // First we add the fields to a data_Target attribue
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
                                    TargetFields.Add( $"\"data_{TargetEntity.Name}.{TargetEntity.Name}_{Attribute.Name}\"", (JSString)$"\"${RuleValue}\"" );
                                }
                            }

                            // Remove attributes mapped to data_Entity
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
                            OneToOneOperatorsToExecute.AddRange( new MongoDBOperator[] { AddOp, ProjectOp } );
                        }
                        else
                        {
                            // Entity is not embedded
                            string LookupTargetAs = $"data_{TargetEntity.Name}Join";
                            // Fetch it
                            LookupOperator LookupOp = new LookupOperator
                            {
                                From = TargetRule.Target.Name,
                                ForeignField = TargetRule.Rules.First( Rule => Rule.Key == RelationshipData.TargetAttribute.Name ).Value,
                                LocalField = SourceRule.Rules.First( Rule => Rule.Key == RelationshipData.SourceAttribute.Name ).Value,
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
                            if ( Relationship.Attributes.Count > 0 && RelationshipRule.Target.Name == TargetRule.Target.Name )
                            {
                                // Map attributes to relationship object
                                foreach ( DataAttribute Attribute in Relationship.Attributes )
                                {
                                    string RuleValue = RelationshipRule.Rules.First( Rule => Rule.Key == Attribute.Name ).Value;
                                    TargetFields.Add( $"\"{RelationshipAttributesField}.{Relationship.Name}_{Attribute.Name}\"", (JSString)$"\"${LookupTargetAs}.{RuleValue}\"" );
                                }

                                // Mark relationship attributes as processed
                                HasRelationshipBeenProcessed = true;
                            }

                            AddFieldsOperator AddOp = new AddFieldsOperator( TargetFields );

                            // Remove unwanted attributes
                            ProjectOperator ProjectOp = ProjectOperator.HideAttributesOperator( new string[] { LookupTargetAs } );

                            OneToOneOperatorsToExecute.AddRange( new MongoDBOperator[] { LookupOp, UnwindOp, AddOp, ProjectOp } );
                        }
                    }
                }

                // Additional fields to remove
                List<string> FieldsToRemove = new List<string>();

                // Entities taken care of, check if the relationship has attributes
                // and it's attributes aren't processed yet
                if ( Relationship.Attributes.Count > 0 && !HasRelationshipBeenProcessed )
                {
                    Dictionary<string, JSCode> RelationshipAttributes = new Dictionary<string, JSCode>();
                    // Add Relationship attributes to a single object
                    foreach ( DataAttribute Attribute in Relationship.Attributes )
                    {
                        string RuleValue = RelationshipRule.Rules.First( Rule => Rule.Key == Attribute.Name ).Value;
                        RelationshipAttributes.Add( $"\"{RelationshipAttributesField}.{Relationship.Name}_{Attribute.Name}\"", (JSString)$"\"${RuleValue}\"" );
                        // Add to removal list (source relationship attribute)
                        FieldsToRemove.Add( RuleValue );
                    }

                    AddFieldsOperator AddOp = new AddFieldsOperator( RelationshipAttributes );

                    OneToOneOperatorsToExecute.Add( AddOp );
                }

                // Create a single object containing relationship/joined data
                MergeObjectsOperator MergeOp = new MergeObjectsOperator();

                foreach ( Entity Target in TargetEntities )
                {
                    MergeOp.Objects.Add( $"$data_{Target.Name}" );
                    FieldsToRemove.Add( $"data_{Target.Name}" );
                }

                // Add relationship data
                MergeOp.Objects.Add( $"${RelationshipAttributesField}" );

                // Also add relationship attribute object to remove list
                FieldsToRemove.Add( $"{RelationshipAttributesField}" );

                // Build operation to hide all entity/relationship data attributes
                Dictionary<string, JSCode> BuildJoinData = new Dictionary<string, JSCode>();
                BuildJoinData.Add( $"data_{Relationship.Name}", new JSArray( new List<object> { MergeOp.ToJSCode() } ) );

                AddFieldsOperator BuildOp = new AddFieldsOperator( BuildJoinData );

                // Remove extra fields
                ProjectOperator RemoveOp = ProjectOperator.HideAttributesOperator( FieldsToRemove );

                // Add to execution list
                OneToOneOperatorsToExecute.AddRange( new MongoDBOperator[] { BuildOp, RemoveOp } );

                // Send to main execution queue
                OperationsToExecute.AddRange( OneToOneOperatorsToExecute.ToArray() );
            }
            else if ( Relationship.Cardinality == RelationshipCardinality.OneToMany )
            {
                // Go through all target entities
                foreach ( Entity TargetEntity in TargetEntities )
                {
                    if ( TargetEntity is ComputedEntity )
                    {
                        // Computed entities require better handling
                    }
                    else
                    {
                        // Retrieve mapping rule for target
                        MapRule TargetRule = ModelMap.Rules.FirstOrDefault( R => R.Source.Name == TargetEntity.Name );
                        if ( TargetRule == null )
                        {
                            throw new ImpossibleOperationException( $"Entity {TargetEntity.Name} has no valid mapping." );
                        }

                        if ( !Relationship.HasRelation( SourceEntity, TargetEntity ) )
                        {
                            throw new ImpossibleOperationException( $"Entities {SourceEntity.Name} and {TargetEntity.Name} are not related through {Relationship.Name}" );
                        }

                        RelationshipConnection RelationshipData = Relationship.GetRelation( SourceEntity, TargetEntity );

                        // Are source and target mapped to the same collection
                        if ( SourceRule.Target.Name == TargetRule.Target.Name )
                        {
                            /* Target entity is embedded in the source entity
                               This also means that the relationship attributes (if any)
                               are mapped to the same collection.

                               We just need to move them to a more appropriate place
                            */

                            // Instead of using AddAtributes
                            // We're using a single project operation reshape de document
                            Dictionary<string, JSCode> MapParams = new Dictionary<string, JSCode>();
                            string RootAttribute = string.Empty;
                            string MapAttributeAs = string.Empty;
                            // Iterate target entity attributes
                            foreach ( DataAttribute Attribute in TargetEntity.Attributes )
                            {
                                string RuleValue = TargetRule.Rules.First( Rule => Rule.Key == Attribute.Name ).Value;
                                string[] RulePath = RuleValue.Split( new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries );

                                if ( string.IsNullOrWhiteSpace( RootAttribute ) )
                                {
                                    RootAttribute = $"{RulePath.First()}";
                                    MapAttributeAs = $"data_{RootAttribute}";
                                }

                                MapParams.Add( $"\"{TargetEntity.Name}_{Attribute.Name}\"", new JSString( $"\"$${MapAttributeAs}.{string.Join( ".", RulePath.Skip( 1 ) )}\"" ) );
                            }

                            // Do the same thing to the relationship attributes
                            foreach ( DataAttribute Attribute in Relationship.Attributes )
                            {
                                string RuleValue = RelationshipRule.Rules.First( Rule => Rule.Key == Attribute.Name ).Value;
                                string[] RulePath = RuleValue.Split( new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries );

                                MapParams.Add( $"\"{Relationship.Name}_{Attribute.Name}\"", new JSString( $"\"$${MapAttributeAs}.{string.Join( ".", RulePath.Skip( 1 ) )}\"" ) );
                            }

                            // Setup project operation
                            MapExpr AttributeMap = new MapExpr( RootAttribute, MapAttributeAs, MapParams );
                            Dictionary<string, ProjectExpression> ProjectFields = new Dictionary<string, ProjectExpression>();
                            ProjectFields.Add( $"data_{Relationship.Name}", AttributeMap );


                            // Keep source entity attributes
                            foreach ( DataAttribute Attribute in SourceEntity.Attributes )
                            {
                                string RuleValue = SourceRule.Rules.First( Rule => Rule.Key == Attribute.Name ).Value;
                                ProjectFields.Add( $"\"{RuleValue}\"", new BooleanExpr( true ) );
                            }

                            ProjectOperator ProjectOp = new ProjectOperator( ProjectFields );


                            OperationsToExecute.Add( ProjectOp );
                        }
                        else
                        {
                            /* 
                             * In this case, the target entity is mapped to its own collection
                             * Which also means that if the relationship has attributes
                             * it must have been mapped to target collection
                             */

                            // Run a custom pipeline to rename the joined entity attributes
                            List<MongoDBOperator> CustomPipeline = new List<MongoDBOperator>();

                            // First thing to do in the pipeline is to match the joined entity with the source entity
                            Dictionary<string, string> PipelineVariables = new Dictionary<string, string>();
                            RelationshipConnection SourceConnection = Relationship.Relations.First();
                            string SourceRef = SourceRule.Rules.First( R => R.Key == SourceConnection.SourceAttribute.Name ).Value;
                            string SourceVar = $"source_{SourceConnection.SourceAttribute.Name}";
                            PipelineVariables.Add( SourceVar, $"${SourceRef}" );

                            string SourceEntityAttribute = TargetRule.Rules.First( R => R.Key == SourceConnection.TargetAttribute.Name ).Value;
                            EqExpr MatchSourceEq = new EqExpr( $"${SourceEntityAttribute}", $"$${SourceVar}" );
                            Match MatchSourceOp = new Match( new Expr( MatchSourceEq ) );

                            // Rename attributes
                            Dictionary<string, ProjectExpression> RenameAttributes = new Dictionary<string, ProjectExpression>();
                            foreach ( DataAttribute Attribute in TargetEntity.Attributes )
                            {
                                string AttributeMappedTo = TargetRule.Rules.First( R => R.Key == Attribute.Name ).Value;
                                if ( AttributeMappedTo != null )
                                {
                                    RenameAttributes.Add( $"{TargetEntity.Name}_{Attribute.Name}", new ValueExpr( $"\"${AttributeMappedTo}\"" ) );
                                }
                            }

                            // Force remove _id
                            RenameAttributes.Add( "_id", new BooleanExpr( false ) );

                            ProjectOperator RenameOp = new ProjectOperator( RenameAttributes );

                            CustomPipeline.AddRange( new List<MongoDBOperator> { MatchSourceOp, RenameOp } );

                            LookupOperator RelationshipLookup = new LookupOperator
                            {
                                From = TargetRule.Target.Name,
                                Let = PipelineVariables,
                                Pipeline = CustomPipeline,
                                As = $"data_{Relationship.Name}"
                            };
                          

                            OperationsToExecute.Add( RelationshipLookup );
                        }
                    }
                }
            }
            else if ( Relationship.Cardinality == RelationshipCardinality.ManyToMany )
            {
                // On Many to Many relationships we need to run custom pipelines to join entities
                // and multiple entities must be in the same pipeline as the relationship
                List<MongoDBOperator> CustomPipeline = new List<MongoDBOperator>();

                // The first operation for each relationship is to match the relationship document to the source entity
                Dictionary<string, string> PipelineVariables = new Dictionary<string, string>();
                RelationshipConnection SourceConnection = Relationship.Relations.First();
                string SourceRef = SourceRule.Rules.First( R => R.Key == SourceConnection.SourceAttribute.Name ).Value;
                string SourceVar = $"source_{SourceConnection.SourceAttribute.Name}";

                PipelineVariables.Add( SourceVar, $"${SourceRef}" );

                string RelationshipSourceRef = RelationshipRule.Rules.First( R => R.Key == SourceConnection.RefSourceAtrribute.Name ).Value;
                EqExpr MatchSourceEq = new EqExpr( $"${RelationshipSourceRef}", $"$${SourceVar}" );

                Match MatchSourceOp = new Match( new Expr( MatchSourceEq ) );

                CustomPipeline.Add( MatchSourceOp );

                foreach ( Entity TargetEntity in TargetEntities )
                {
                    if ( TargetEntity is ComputedEntity )
                    {
                    }
                    else
                    {
                        // Get Target rule
                        MapRule TargetRule = ModelMap.Rules.FirstOrDefault( R => R.Source.Name == TargetEntity.Name );
                        if ( TargetRule == null )
                        {
                            throw new ImpossibleOperationException( $"Entity {TargetEntity.Name} has no valid mapping." );
                        }

                        if ( !Relationship.HasRelation( SourceEntity, TargetEntity ) )
                        {
                            throw new ImpossibleOperationException( $"Entities {SourceEntity.Name} and {TargetEntity.Name} are not related through {Relationship.Name}" );
                        }

                        RelationshipConnection RelationshipData = Relationship.GetRelation( SourceEntity, TargetEntity );

                        // Many to many relationships so far we're only considering that entities are mapped to distinct collections
                        // and the relationship linking them has it's own collection

                        // Build the operations for the custom pipeline

                        // Foreach target entity, we must do a lookup, unwind, addfields and project operations
                        // Lookup target
                        string TargetLookupAs = $"data_{TargetEntity.Name}"; 

                        LookupOperator LookupTargetOp = new LookupOperator
                        {
                            From = TargetRule.Target.Name,
                            ForeignField = TargetRule.Rules.First( R => R.Key == RelationshipData.TargetAttribute.Name ).Value,
                            LocalField = RelationshipRule.Rules.First( R => R.Key == RelationshipData.RefTargetAttribute.Name ).Value,
                            As = TargetLookupAs
                        };

                        // Unwind
                        UnwindOperator UnwindTarget = new UnwindOperator( TargetLookupAs );


                        // Add fields
                        Dictionary<string, JSCode> FieldsToAdd = new Dictionary<string, JSCode>();

                        foreach ( DataAttribute Attribute in TargetEntity.Attributes )
                        {
                            string AttributeMappedTo = TargetRule.Rules.First( R => R.Key == Attribute.Name ).Value;
                            FieldsToAdd.Add( $"{TargetEntity.Name}_{Attribute.Name}", (JSString)$"\"${TargetLookupAs}.{AttributeMappedTo}\"" );
                        }

                        AddFieldsOperator AddFieldsOp = new AddFieldsOperator( FieldsToAdd );

                        // Project - remove joined data extra data
                        Dictionary<string, ProjectExpression> ProjectExpressions = new Dictionary<string, ProjectExpression>
                        {
                            { TargetLookupAs, new BooleanExpr( false ) }
                        };
                        ProjectOperator ProjectOp = new ProjectOperator( ProjectExpressions );

                        CustomPipeline.AddRange( new MongoDBOperator[] { LookupTargetOp, UnwindTarget, AddFieldsOp, ProjectOp } );
                    }
                }

                // Also rename relationship attributes
                Dictionary<string, JSCode> RelationshipAttributesToAdd = new Dictionary<string, JSCode>();
                Dictionary<string, ProjectExpression> RelationshipAttributesToRemove = new Dictionary<string, ProjectExpression>();

                foreach ( DataAttribute Attribute in Relationship.Attributes )
                {
                    string AttributeMap = RelationshipRule.Rules.First( R => R.Key == Attribute.Name ).Value;
                    RelationshipAttributesToAdd.Add( $"{Relationship.Name}_{Attribute.Name}", (JSString)$"\"${AttributeMap}\"" );
                    RelationshipAttributesToRemove.Add( AttributeMap, new BooleanExpr( false ) );
                }

                AddFieldsOperator RAddFields = new AddFieldsOperator( RelationshipAttributesToAdd );
                ProjectOperator RRemoveOp = new ProjectOperator( RelationshipAttributesToRemove );

                CustomPipeline.AddRange( new MongoDBOperator[] { RAddFields, RRemoveOp } );

                // Add Lookup for relationship
                LookupOperator RelationshipLookup = new LookupOperator
                {
                    From = RelationshipRule.Target.Name,
                    Let = PipelineVariables,
                    Pipeline = CustomPipeline,
                    As = $"data_{Relationship.Name}"
                };

                OperationsToExecute.Add( RelationshipLookup );
            }

            // Assign operation list
            LastResult.Commands.AddRange( OperationsToExecute );
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Initialize a new instance of Join Operation class
        /// </summary>
        /// <param name="SourceEntity">Source entity</param>
        /// <param name="Relationship">Join through this relationship</param>
        /// <param name="TargetEntities">Target entities</param>
        /// <param name="ModelMap">Map rules between ER and Mongo</param>
        public RelationshipJoinOperator( Entity SourceEntity, Relationship Relationship, List<Entity> TargetEntities, ModelMapping ModelMap ) : base( ModelMap )
        {
            this.SourceEntity = SourceEntity;
            this.TargetEntities = TargetEntities;
            this.Relationship = Relationship;
        }
        #endregion
    }
}
