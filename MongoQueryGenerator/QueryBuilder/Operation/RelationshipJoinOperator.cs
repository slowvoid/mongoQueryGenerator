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

                // List of operations to be executed by this relationship
                List<MongoDBOperator> RelationshipOperations = new List<MongoDBOperator>();
                // Store info regarding wheter the relationship attributes (if any) were already mapped
                bool HasRelationshipBeenProcessed = false;
                string RelationshipAttributesField = $"data_{TargetData.Relationship.Name}";

                // Processing may vary based on relationship cardinality
                if ( TargetData.Relationship.Cardinality == RelationshipCardinality.OneToOne )
                {
                    // Store all fields that should be merged under data_Relationship
                    List<string> FieldsToMerge = new List<string>();

                    // Process each entity to be joined
                    foreach ( Entity TargetEntity in TargetData.Targets )
                    {
                        // If the target is a ComputedEntity
                        // Run another RJOINOperator
                        if ( TargetEntity is ComputedEntity TargetAsCE )
                        {
                            LookupOperator LookupOp = LookupComputedEntity( SourceRule, TargetData, TargetEntity, TargetAsCE );
                            UnwindOperator UnwindLookup = new UnwindOperator( LookupOp.As );

                            OperationsToExecute.AddRange( new MongoDBOperator[] { LookupOp, UnwindLookup } );

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

                    // Remove these fields
                    List<string> FieldsToRemove = new List<string>();

                    // Process entity attributes (if any)
                    if ( TargetData.Relationship.Attributes.Count > 0 && !HasRelationshipBeenProcessed )
                    {
                        Dictionary<string, JSCode> RelationshipAttributes = new Dictionary<string, JSCode>();
                        // Add relationship attributes to a single object
                        foreach ( DataAttribute Attribute in TargetData.Relationship.Attributes )
                        {
                            string RuleValue = RelationshipRule.Rules.First( Rule => Rule.Key == Attribute.Name ).Value;
                            RelationshipAttributes.Add( $"\"{RelationshipAttributesField}.{TargetData.Relationship.Name}_{Attribute.Name}\"",
                                (JSString)$"\"${RuleValue}\"" );
                            // Add to removal list
                            FieldsToRemove.Add( RuleValue );
                        }

                        AddFieldsOperator AddOp = new AddFieldsOperator( RelationshipAttributes );
                        RelationshipOperations.Add( AddOp );
                    }

                    // Merge Fields
                    List<string> MergeObjects = new List<string>();

                    foreach ( Entity Target in TargetData.Targets )
                    { 
                        MergeObjects.Add( $"$data_{Target.Name}" );
                        FieldsToRemove.Add( $"data_{Target.Name}" );
                    }

                    foreach ( string Field in FieldsToMerge )
                    {
                        MergeObjects.Add( $"${Field}" );
                        FieldsToRemove.Add( Field );
                    }

                    // Add relationship data
                    MergeObjects.Add( $"${RelationshipAttributesField}" );
                    MergeObjectsOperator MergeOp = new MergeObjectsOperator( MergeObjects );

                    // Build operation to hide all entity/relationship data attributes
                    Dictionary<string, JSCode> BuildJoinData = new Dictionary<string, JSCode>();
                    BuildJoinData.Add( $"data_{TargetData.Relationship.Name}", new JSArray( new List<object> { MergeOp.ToJSCode() } ) );

                    AddFieldsOperator BuildOp = new AddFieldsOperator( BuildJoinData );
                    ProjectOperator HideFields = ProjectOperator.HideAttributesOperator( FieldsToRemove );

                    RelationshipOperations.AddRange( new MongoDBOperator[] { BuildOp, HideFields } );
                    OperationsToExecute.AddRange( RelationshipOperations.ToArray() );
                }
                else if ( TargetData.Relationship.Cardinality == RelationshipCardinality.OneToMany )
                {
                    // Store fields to be concatenated
                    // Poor mapping of relationships may lead to weird results
                    List<string> ConcatArrays = new List<string>();

                    foreach ( Entity TargetEntity in TargetData.Targets )
                    {
                        if ( TargetEntity is ComputedEntity TargetAsCE )
                        {
                            LookupOperator LookupCE = LookupComputedEntity( SourceRule, TargetData, TargetEntity, TargetAsCE );
                            RelationshipOperations.Add( LookupCE );
                            ConcatArrays.Add( LookupCE.As );
                        }
                        else
                        {
                            // Retrieve mapping rule for target
                            MapRule TargetRule = ModelMap.Rules.FirstOrDefault( R => R.Source.Name == TargetEntity.Name );
                            if ( TargetRule == null )
                            {
                                throw new ImpossibleOperationException( $"Entity {TargetEntity.Name} has no valid mapping." );
                            }

                            if ( !TargetData.Relationship.HasRelation( SourceEntity, TargetEntity ) )
                            {
                                throw new ImpossibleOperationException( $"Entities {SourceEntity.Name} and {TargetEntity.Name} are not related through {TargetData.Relationship.Name}" );
                            }

                            RelationshipConnection RelationshipData = TargetData.Relationship.GetRelation( SourceEntity, TargetEntity );

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
                                foreach ( DataAttribute Attribute in TargetData.Relationship.Attributes )
                                {
                                    string RuleValue = RelationshipRule.Rules.First( Rule => Rule.Key == Attribute.Name ).Value;
                                    string[] RulePath = RuleValue.Split( new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries );

                                    MapParams.Add( $"\"{TargetData.Relationship.Name}_{Attribute.Name}\"", new JSString( $"\"$${MapAttributeAs}.{string.Join( ".", RulePath.Skip( 1 ) )}\"" ) );
                                }

                                // Setup project operation
                                MapExpr AttributeMap = new MapExpr( RootAttribute, MapAttributeAs, MapParams );
                                Dictionary<string, ProjectExpression> ProjectFields = new Dictionary<string, ProjectExpression>();
                                ProjectFields.Add( $"data_{TargetData.Relationship.Name}", AttributeMap );


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

                                // Fetch target entity
                                LookupOperator TargetLookupOp = new LookupOperator
                                {
                                    From = TargetRule.Target.Name,
                                    ForeignField = TargetRule.Rules.First( Rule => Rule.Key == RelationshipData.TargetAttribute.Name ).Value,
                                    LocalField = SourceRule.Rules.First( Rule => Rule.Key == RelationshipData.SourceAttribute.Name ).Value,
                                    As = $"data_{TargetEntity.Name}"
                                };

                                // Run a project with map to rename joined attributes
                                Dictionary<string, JSCode> MapParams = new Dictionary<string, JSCode>();
                                string RootAttribute = $"data_{TargetEntity.Name}";
                                string MapAttributeAs = $"data_{RootAttribute}";

                                foreach ( DataAttribute Attribute in TargetEntity.Attributes )
                                {
                                    string RuleValue = TargetRule.Rules.First( Rule => Rule.Key == Attribute.Name ).Value;

                                    MapParams.Add( $"\"{TargetEntity.Name}_{Attribute.Name}\"", new JSString( $"\"$${MapAttributeAs}.{RuleValue}\"" ) );
                                }

                                // Do the same thing to the relationship attributes
                                foreach ( DataAttribute Attribute in TargetData.Relationship.Attributes )
                                {
                                    string RuleValue = RelationshipRule.Rules.First( Rule => Rule.Key == Attribute.Name ).Value;

                                    MapParams.Add( $"\"{TargetData.Relationship.Name}_{Attribute.Name}\"", new JSString( $"\"$${MapAttributeAs}.{RuleValue}\"" ) );
                                }

                                // Setup project operation
                                MapExpr AttributeMap = new MapExpr( RootAttribute, MapAttributeAs, MapParams );
                                Dictionary<string, ProjectExpression> ProjectFields = new Dictionary<string, ProjectExpression>();
                                ProjectFields.Add( $"data_{TargetData.Relationship.Name}", AttributeMap );


                                // Keep source entity attributes
                                foreach ( DataAttribute Attribute in SourceEntity.Attributes )
                                {
                                    string RuleValue = SourceRule.Rules.First( Rule => Rule.Key == Attribute.Name ).Value;
                                    ProjectFields.Add( $"\"{RuleValue}\"", new BooleanExpr( true ) );
                                }

                                ProjectOperator ProjectOp = new ProjectOperator( ProjectFields );


                                OperationsToExecute.AddRange( new MongoDBOperator[] { TargetLookupOp, ProjectOp } );
                            }
                        }
                    }

                    // Check if there are arrays to concat
                    if ( ConcatArrays.Count > 0 )
                    {
                        // Create concat expression
                        ConcatArrayExpr ConcatExpr = new ConcatArrayExpr( ConcatArrays );
                        // Add concatenation result as a new field
                        Dictionary<string, JSCode> ConcatenatedFields = new Dictionary<string, JSCode>();
                        ConcatenatedFields.Add( RelationshipAttributesField, ConcatExpr.ToJSCode() );
                        AddFieldsOperator AddConcatOp = new AddFieldsOperator( ConcatenatedFields );
                        // Remove fields
                        ProjectOperator RemoveConcatSourceOp = ProjectOperator.HideAttributesOperator( ConcatArrays );

                        RelationshipOperations.AddRange( new MongoDBOperator[] { AddConcatOp, RemoveConcatSourceOp } );
                    }

                    OperationsToExecute.AddRange( RelationshipOperations.ToArray() );
                }
                else if ( TargetData.Relationship.Cardinality == RelationshipCardinality.ManyToMany )
                {
                    // On Many to Many relationships we need to run custom pipelines to join entities
                    // and multiple entities must be in the same pipeline as the relationship
                    List<MongoDBOperator> CustomPipeline = new List<MongoDBOperator>();

                    // The first operation for each relationship is to match the relationship document to the source entity
                    Dictionary<string, string> PipelineVariables = new Dictionary<string, string>();
                    RelationshipConnection SourceConnection = TargetData.Relationship.Relations.First();
                    string SourceRef = SourceRule.Rules.First( R => R.Key == SourceConnection.SourceAttribute.Name ).Value;
                    string SourceVar = $"source_{SourceConnection.SourceAttribute.Name}";

                    PipelineVariables.Add( SourceVar, $"${SourceRef}" );

                    string RelationshipSourceRef = RelationshipRule.Rules.First( R => R.Key == SourceConnection.RefSourceAtrribute.Name ).Value;
                    EqExpr MatchSourceEq = new EqExpr( $"${RelationshipSourceRef}", $"$${SourceVar}" );

                    MatchOperator MatchSourceOp = new MatchOperator( new Expr( MatchSourceEq ) );

                    CustomPipeline.Add( MatchSourceOp );

                    // Store fields that should be merged with the root attribute
                    List<string> FieldsToMergeWithRoot = new List<string>();

                    foreach ( Entity TargetEntity in TargetData.Targets )
                    {
                        if ( TargetEntity is ComputedEntity TargetAsCE )
                        {
                            LookupOperator LookupCE = LookupComputedEntity( SourceRule, TargetData, TargetEntity, TargetAsCE );
                            CustomPipeline.Add( LookupCE );

                            // Unwind joined entity
                            UnwindOperator UnwindOp = new UnwindOperator( LookupCE.As );
                            CustomPipeline.Add( UnwindOp );

                            // Add to merge list
                            FieldsToMergeWithRoot.Add( LookupCE.As );
                        }
                        else
                        {
                            // Get Target rule
                            MapRule TargetRule = ModelMap.Rules.FirstOrDefault( R => R.Source.Name == TargetEntity.Name );
                            if ( TargetRule == null )
                            {
                                throw new ImpossibleOperationException( $"Entity {TargetEntity.Name} has no valid mapping." );
                            }

                            if ( !TargetData.Relationship.HasRelation( SourceEntity, TargetEntity ) )
                            {
                                throw new ImpossibleOperationException( $"Entities {SourceEntity.Name} and {TargetEntity.Name} are not related through {TargetData.Relationship.Name}" );
                            }

                            RelationshipConnection RelationshipData = TargetData.Relationship.GetRelation( SourceEntity, TargetEntity );

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

                    foreach ( DataAttribute Attribute in TargetData.Relationship.Attributes )
                    {
                        string AttributeMap = RelationshipRule.Rules.First( R => R.Key == Attribute.Name ).Value;
                        RelationshipAttributesToAdd.Add( $"{TargetData.Relationship.Name}_{Attribute.Name}", (JSString)$"\"${AttributeMap}\"" );
                        RelationshipAttributesToRemove.Add( AttributeMap, new BooleanExpr( false ) );
                    }

                    AddFieldsOperator RAddFields = new AddFieldsOperator( RelationshipAttributesToAdd );
                    ProjectOperator RRemoveOp = new ProjectOperator( RelationshipAttributesToRemove );

                    CustomPipeline.AddRange( new MongoDBOperator[] { RAddFields, RRemoveOp } );

                    // Merge fields with root (if any)
                    if ( FieldsToMergeWithRoot.Count > 0 )
                    {
                        // Add $$ROOT to merge
                        FieldsToMergeWithRoot.Add( "$$ROOT" );
                        MergeObjectsOperator MergeWithRoot = new MergeObjectsOperator( FieldsToMergeWithRoot );
                        ReplaceRootOperator ReplaceRootOp = new ReplaceRootOperator( MergeWithRoot.ToJSCode() );
                        CustomPipeline.Add( ReplaceRootOp );

                        // Hide merged attributes to avoid duplicates
                        ProjectOperator HideMerged = ProjectOperator.HideAttributesOperator( FieldsToMergeWithRoot.Where( Field => Field != "$$ROOT" ) );
                        CustomPipeline.Add( HideMerged );
                    }

                    // Add Lookup for relationship
                    LookupOperator RelationshipLookup = new LookupOperator
                    {
                        From = RelationshipRule.Target.Name,
                        Let = PipelineVariables,
                        Pipeline = CustomPipeline,
                        As = $"data_{TargetData.Relationship.Name}"
                    };

                    OperationsToExecute.Add( RelationshipLookup );
                }
            }

            // Return operations
            return new AlgebraOperatorResult( OperationsToExecute );
        }
        /// <summary>
        /// Process a computed entity
        /// </summary>
        /// <param name="SourceRule"></param>
        /// <param name="TargetData"></param>
        /// <param name="TargetEntity"></param>
        /// <param name="TargetAsCE"></param>
        /// <returns></returns>
        private LookupOperator LookupComputedEntity( MapRule SourceRule, RelationshipJoinArguments TargetData, Entity TargetEntity, ComputedEntity TargetAsCE )
        {
            // Retrieve the CE source entity before calling another RJOIN instance
            // Retrieve CE-SE rule
            MapRule CESERule = ModelMap.Rules.First( Rule => Rule.Source.Name == TargetAsCE.SourceEntity.Name && Rule.IsMain );

            // Stop if rule not found
            if ( CESERule == null )
            {
                throw new RuleNotFoundException( $"Entity [{TargetAsCE.SourceEntity.Name}] is not mapped." );
            }

            // Left side entities cannot be embedded
            if ( SourceRule.Target.Name == CESERule.Target.Name )
            {
                throw new InvalidMapException( $"Entities that are the left operand of a join operation cannot be embedded into another entity. [{TargetAsCE.SourceEntity.Name}]" );
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

            // Check if the source relationship is many to many
            if ( TargetData.Relationship.Cardinality == RelationshipCardinality.ManyToMany )
            {
                MapRule RelationshipRule = ModelMap.Rules.First( Rule => Rule.Source.Name == TargetData.Relationship.Name );
                SourceIdentifierValue = RelationshipRule.Rules.First( Rule => Rule.Key == ConnRules.RefTargetAttribute.Name ).Value;
            }

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
                TargetAsCE.SourceEntity,
                new List<RelationshipJoinArguments> { newArgs },
                ModelMap );

            CEPipeline.AddRange( TargetCEOperator.Run().Commands );

            // Rename entity attributes (avoids possible merge conflicts)
            Dictionary<string, JSCode> AttributesToRename = new Dictionary<string, JSCode>();
            List<string> AttributesToHide = new List<string>();
            foreach ( DataAttribute Attribute in TargetAsCE.SourceEntity.Attributes )
            {
                string RuleValue = CESERule.Rules.First( Rule => Rule.Key == Attribute.Name ).Value;
                if ( RuleValue == null )
                {
                    continue;
                }

                AttributesToRename.Add( $"{TargetAsCE.SourceEntity.Name}_{Attribute.Name}", (JSString)$"\"${RuleValue}\"" );
                AttributesToHide.Add( RuleValue );
            }

            AddFieldsOperator RenameOp = new AddFieldsOperator( AttributesToRename );
            ProjectOperator HideOp = ProjectOperator.HideAttributesOperator( AttributesToHide );

            CEPipeline.AddRange( new MongoDBOperator[] { RenameOp, HideOp } );

            // Create lookup
            LookupOperator LookupOp = new LookupOperator
            {
                From = CESERule.Target.Name,
                Let = CEPipelineVars,
                Pipeline = CEPipeline,
                As = $"data_{TargetEntity.Name}"
            };
            return LookupOp;
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
