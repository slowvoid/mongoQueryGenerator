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
        public QueryableEntity SourceEntity { get; set; }
        /// <summary>
        /// Relationship that connects the source and target entities
        /// </summary>
        public Relationship Relationship { get; set; }
        /// <summary>
        /// Target entities
        /// </summary>
        public List<QueryableEntity> TargetEntities { get; set; }
        #endregion

        #region Methods
        /// <summary>
        /// Run the algorithm
        /// </summary>
        /// <param name="LastResult"></param>
        /// <returns></returns>
        public override AlgebraOperatorResult Run()
        {
            // Store Operations
            List<MongoDBOperator> OperationsToExecute = new List<MongoDBOperator>();

            // Retrieve source entity rules (left side entities must have a main mapping)
            MapRule SourceRule = ModelMap.Rules.FirstOrDefault( Rule => Rule.Source.Name == SourceEntity.GetName() && Rule.IsMain );

            // Check if it exists
            if ( SourceRule == null )
            {
                throw new MissingMappingException( $"Mapping for entity {SourceEntity.GetName()} not found." );
            }

            // Fetch rules for relationship
            MapRule RelationshipRule = ModelMap.Rules.FirstOrDefault( Rule => Rule.Source.Name == Relationship.Name );

            // Check if the relationship has a mapping and if it is mapped to it's own collection
            if ( RelationshipRule != null && RelationshipRule.IsMain )
            {
                // In this case we basically have a N:M relationship using an intermediate collection
                // this means a custom lookup pipeline is required

                // Store all pipeline operators
                List<MongoDBOperator> PipelineOperators = new List<MongoDBOperator>();

                // Store pipeline variables
                Dictionary<string, string> PipelineVariables = new Dictionary<string, string>();

                // Find map to source identifier
                DataAttribute sourceId = SourceEntity.Element.Attributes.First( A => A.IsIdentifier );
                // TODO: Check if we can add a PrimaryKey attribute to entity

                string primaryKeyMap = SourceRule.Rules.First( R => R.Key == sourceId.Name ).Value;
                string sourcePrimaryKey = $"source_{sourceId.Name}";

                // Check if the map was found
                if ( string.IsNullOrWhiteSpace( primaryKeyMap ) )
                {
                    throw new RuleNotFoundException( $"No map found for identifier attribute {sourceId.Name}" );
                }

                PipelineVariables.Add( sourcePrimaryKey, $"${sourcePrimaryKey}" );

                // Find the sourcePrimaryKey reference in the middle collection
                MapRule SourceRefRule = ModelMap.Rules.First( R => R.Target.Name == RelationshipRule.Target.Name
                    && R.Source.Name == SourceEntity.GetName()
                    && !R.IsMain );

                string sourcePrimaryKeyRef = SourceRefRule.Rules.First( R => R.Key == sourceId.Name ).Value;
                // If the rule is not found, then the connection is incomplete
                if ( string.IsNullOrWhiteSpace( sourcePrimaryKeyRef ) )
                {
                    throw new RuleNotFoundException( $"Map for {sourceId.Name} was not found on {Relationship.Name} mapping" );
                }

                // First, match relationship to source entity
                MatchOperator MatchSourceOp = MatchOperator.CreateLookupMatch( sourcePrimaryKey, sourcePrimaryKeyRef );
                // Add to list
                PipelineOperators.Add( MatchSourceOp );

                // Store fields to merge with root attribute
                List<string> FieldsToMergeWithRoot = new List<string>();

                // Iterate all targets
                foreach ( QueryableEntity Target in TargetEntities )
                {
                    // Check if the target is a computed entity
                    if ( Target.Element is ComputedEntity )
                    {
                        // TODO:
                    }
                    else
                    {
                        // Check if the target is related to the source entity through the given relationship
                        if ( !Relationship.AreRelated( (Entity)SourceEntity.Element, (Entity)Target.Element ) )
                        {
                            throw new NotRelatedException( $"Entities {SourceEntity.GetName()} and {Target.GetName()} are not related through {Relationship.Name}" );
                        }

                        // Fetch target mapping
                        MapRule TargetRule = ModelMap.Rules.FirstOrDefault( Rule => Rule.Source.Name == Target.GetName() );

                        // Check if TargetRule was found
                        if ( TargetRule == null )
                        {
                            throw new RuleNotFoundException( $"No rules found for entity {Target.GetName()}" );
                        }

                        // Target entity can either be mapped to the same collection as the relationship or be on it's own collection
                        // otherwise it is not valid
                        if ( !TargetRule.IsMain && TargetRule.Target.Name != RelationshipRule.Target.Name )
                        {
                            // Try to fetch a main rule for the target
                            MapRule MainTargetRule = ModelMap.Rules.FirstOrDefault( Rule => Rule.Source.Name == Target.GetName() && Rule.IsMain );

                            // If not found, throw error
                            if ( MainTargetRule == null )
                            {
                                throw new ImpossibleOperationException( $"Entity {Target.GetName()} cannot be mapped to another entity for this operation" );
                            }

                            // Found main rule, use this one instead
                            TargetRule = MainTargetRule;
                        }

                        // Build operations
                        string TargetLookupAs = $"data_{Target.GetName()}";

                        // Target primary key
                        DataAttribute targetPrimaryKey = Target.Element.Attributes.First( A => A.IsIdentifier );
                        // Find target primary key map
                        MapRule TargetRefRule = ModelMap.Rules.First( R => R.Target.Name == RelationshipRule.Target.Name
                            && R.Source.Name == Target.GetName()
                            && !R.IsMain );

                        LookupOperator LookupTargetOp = new LookupOperator
                        {
                            From = TargetRule.Target.Name,
                            ForeignField = TargetRule.Rules.First( R => R.Key == targetPrimaryKey.Name ).Value,
                            LocalField = TargetRefRule.Rules.First( R => R.Key == targetPrimaryKey.Name ).Value,
                            As = TargetLookupAs
                        };

                        // Unwind data
                        UnwindOperator UnwindTarget = new UnwindOperator( TargetLookupAs );

                        // Add Fields
                        Dictionary<string, JSCode> FieldsToAdd = new Dictionary<string, JSCode>();

                        foreach ( DataAttribute Attribute in Target.Element.Attributes )
                        {
                            string AttributeMappedTo = TargetRule.Rules.First( R => R.Key == Attribute.Name ).Value;
                            FieldsToAdd.Add( $"{Target.GetName()}_{Attribute.Name}", (JSString)$"\"${TargetLookupAs}.{AttributeMappedTo}\"" );
                        }

                        AddFieldsOperator AddFieldsOp = new AddFieldsOperator( FieldsToAdd );

                        // Project - remove joined data extras
                        ProjectOperator ProjectOp = ProjectOperator.HideAttributesOperator( new string[] { TargetLookupAs } );

                        // Add to pipeline list
                        PipelineOperators.AddRange( new MongoDBOperator[] { LookupTargetOp, UnwindTarget, AddFieldsOp, ProjectOp } );
                    }
                }

                // Also rename relationship attributes
                Dictionary<string, JSCode> RelationshipAttributesToAdd = new Dictionary<string, JSCode>();
                List<string> RelationshipAttributesToRemove = new List<string>();

                foreach ( DataAttribute Attribute in Relationship.Attributes )
                {
                    string AttributeMap = RelationshipRule.Rules.First( R => R.Key == Attribute.Name ).Value;
                    RelationshipAttributesToAdd.Add( $"{Relationship.Name}_{Attribute.Name}", (JSString)$"\"${AttributeMap}\"" );
                    RelationshipAttributesToRemove.Add( AttributeMap );
                }

                AddFieldsOperator AddRelationshipFieldsOp = new AddFieldsOperator( RelationshipAttributesToAdd );
                ProjectOperator RemoveRelationshipFieldsOp = ProjectOperator.HideAttributesOperator( RelationshipAttributesToRemove );

                // Add to pipeline
                PipelineOperators.AddRange( new MongoDBOperator[] { AddRelationshipFieldsOp, RemoveRelationshipFieldsOp } );

                // Merge fields with root (if any)
                if ( FieldsToMergeWithRoot.Count > 0 )
                {
                    // Add $$ROOT to merge
                    FieldsToMergeWithRoot.Add( "$$ROOT" );
                    MergeObjectsOperator MergeWithRoot = new MergeObjectsOperator( FieldsToMergeWithRoot );
                    ReplaceRootOperator ReplaceRootOp = new ReplaceRootOperator( MergeWithRoot.ToJSCode() );
                    PipelineOperators.Add( ReplaceRootOp );

                    // Hide merged attributes to avoid duplicates
                    ProjectOperator HideMerged = ProjectOperator.HideAttributesOperator( FieldsToMergeWithRoot.Where( Field => Field != "$$ROOT" ) );
                    PipelineOperators.Add( HideMerged );
                }

                // Add lookup operator for relationship
                LookupOperator RelationshipLookupOp = new LookupOperator
                {
                    From = RelationshipRule.Target.Name,
                    Let = PipelineVariables,
                    Pipeline = PipelineOperators,
                    As = $"data_{Relationship.Name}"
                };

                // Add to execution list
                OperationsToExecute.Add( RelationshipLookupOp );
            }
            else
            {
                // This means that either the relationship has no attributes
                // or that it is embedded into another collection

                // Store fields to merge under data_Relationship
                List<string> FieldsToMerge = new List<string>();

                // Iterate targets
                foreach ( QueryableEntity Target in TargetEntities )
                {
                    // Check if the target is a computed entity
                    if ( Target.Element is ComputedEntity )
                    {
                        // TODO:
                    }
                    else
                    {
                        // Check if target is related to source through relationship
                        if ( !Relationship.AreRelated( (Entity)SourceEntity.Element, (Entity)Target.Element ) )
                        {
                            throw new NotRelatedException( $"Entities {SourceEntity.GetName()} and {Target.GetName()} are not related through {Relationship.Name}" );
                        }

                        // Fetch rule
                        MapRule TargetRule = ModelMap.Rules.FirstOrDefault( R => R.Source.Name == Target.GetName() );

                        if ( TargetRule == null )
                        {
                            // Try finding a main mapping
                            MapRule NewTargetRule = ModelMap.Rules.FirstOrDefault( R => R.Source.Name == Target.GetName() && R.IsMain );

                            if ( NewTargetRule == null )
                            {
                                throw new RuleNotFoundException( $"No mapping was found for {Target.GetName()}" );
                            }

                            TargetRule = NewTargetRule;
                        }

                        // Check if the target and source are embedded
                        if ( SourceRule.Target.Name == TargetRule.Target.Name )
                        {
                            // Target is embedded, check the cardinality
                            // Assuming that Cardinality is One means that the target is embedded in an object or scattered in the source collection
                            // Assuming that Cardinality is Many means that the target is embedded in an array
                            RelationshipEnd TargetEnd = Relationship.Ends.First( E => E.TargetEntity.Name == Target.GetName() );

                            if ( TargetEnd.Cardinality == RelationshipCardinality.One )
                            {
                                // Attributes to add
                                Dictionary<string, JSCode> AttributesToAdd = new Dictionary<string, JSCode>();
                                List<string> AttributesToRemove = new List<string>();
                                List<string> RootAttributes = new List<string>();
                                // Fetch attributes and add them to an object under data_Relationship
                                foreach ( DataAttribute Attribute in Target.Element.Attributes )
                                {
                                    string AttributeMap = TargetRule.Rules.First( R => R.Key == Attribute.Name ).Value;
                                    AttributesToAdd.Add( $"data_{Target.GetName()}.{Target.GetName()}_{Attribute.Name}", (JSString)$"\"${AttributeMap}\"" );

                                    // Check if it is within an object
                                    string[] AttributeMapPath = AttributeMap.Split( new char[] { '.' } );

                                    if ( AttributeMapPath.Length > 1 )
                                    {
                                        RootAttributes.Add( AttributeMapPath.First() );
                                    }
                                    else
                                    {
                                        AttributesToRemove.Add( AttributeMap );
                                    }
                                }

                                FieldsToMerge.Add( $"data_{Target.GetName()}" );

                                // Add fields
                                AddFieldsOperator AddTargetFieldsOp = new AddFieldsOperator( AttributesToAdd );

                                // Remove old attributes
                                AttributesToRemove.AddRange( RootAttributes.Distinct() );
                                ProjectOperator RemoveTargetFields = ProjectOperator.HideAttributesOperator( AttributesToRemove );

                                OperationsToExecute.AddRange( new MongoDBOperator[] { AddTargetFieldsOp, RemoveTargetFields } );
                            }
                            else if ( TargetEnd.Cardinality == RelationshipCardinality.Many)
                            {

                            }
                        }
                        else
                        {

                        }
                    }
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
        private LookupOperator LookupComputedEntity( MapRule SourceRule, RelationshipJoinArgument TargetData, Entity TargetEntity, ComputedEntity TargetAsCE )
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Computes the virtual map after executing this instance
        /// </summary>
        /// <returns></returns>
        public override VirtualMap ComputeVirtualMap( VirtualMap ExistingVirtualMap = null )
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Compute the virtual map of a ComputedEntity
        /// </summary>
        /// <param name="RootAttribute"></param>
        /// <param name="TargetEntity"></param>
        private List<VirtualRule> ComputeCEVirtualMap( string RootAttribute, ComputedEntity TargetEntity )
        {
            // Rule list to return
            List<VirtualRule> RuleList = new List<VirtualRule>();
            // Shorten path to entity
            Entity CurrentEntity = (Entity)TargetEntity.SourceEntity.Element;
            // Create rule for the base entity
            VirtualRule VirtualEntityRule = new VirtualRule( TargetEntity.SourceEntity.Element, TargetEntity.SourceEntity.Alias );
            // Iterate it's attributes
            foreach ( DataAttribute Attribute in CurrentEntity.Attributes )
            {
                VirtualEntityRule.AddRule( Attribute.Name, $"{RootAttribute}.{CurrentEntity.Name}_{Attribute.Name}" );
            }
            // Add to list
            RuleList.Add( VirtualEntityRule );

            // Set new root attribute for relationship and joining entities
            string NewRootAttribute = $"{RootAttribute}.data_{TargetEntity.Relationship.Name}";
            // Process relationship
            VirtualRule RelationshipVirtualRule = new VirtualRule( TargetEntity.Relationship );
            foreach ( DataAttribute Attribute in TargetEntity.Relationship.Attributes )
            {
                RelationshipVirtualRule.AddRule( Attribute.Name, $"{NewRootAttribute}.{TargetEntity.Relationship.Name}_{Attribute.Name}" );
            }
            // Add to list
            RuleList.Add( RelationshipVirtualRule );

            // Process additional entities
            foreach ( QueryableEntity Target in TargetEntity.TargetEntities )
            {
                if ( Target.Element is ComputedEntity TargetAsCE )
                {
                    RuleList.AddRange( ComputeCEVirtualMap( NewRootAttribute, TargetAsCE ) );
                }
                else
                {
                    // Rules
                    VirtualRule TargetEntityVirtualRule = new VirtualRule( Target.Element, Target.Alias );
                    // Process attributes
                    foreach ( DataAttribute Attribute in Target.Element.Attributes )
                    {
                        TargetEntityVirtualRule.AddRule( Attribute.Name, $"{NewRootAttribute}.{Target.Element.Name}_{Attribute.Name}" );
                    }
                    // Add to list
                    RuleList.Add( TargetEntityVirtualRule );
                }
            }

            return RuleList;
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Initialize a new instance of RelationshipJoinOperator
        /// </summary>
        /// <param name="SourceEntity"></param>
        /// <param name="Map"></param>
        public RelationshipJoinOperator( QueryableEntity SourceEntity, ModelMapping Map ) : base( Map )
        {
            this.SourceEntity = SourceEntity;
        }
        #endregion
    }
}
