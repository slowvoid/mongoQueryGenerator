using QueryBuilder.ER;
using QueryBuilder.Javascript;
using QueryBuilder.Map;
using QueryBuilder.Mongo.Aggregation.Operators;
using QueryBuilder.Mongo.Expressions;
using QueryBuilder.Operation.Arguments;
using QueryBuilder.Operation.Exceptions;
using QueryBuilder.Shared;
using System;
using System.Collections.Generic;
using System.Linq;

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

        public string RelationshipAlias { get; set; }
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

            // Check if the Source entity and the relationship are connected
            if ( !Relationship.ContainsElement( (Entity)SourceEntity.Element ) )
            {
                throw new NotRelatedException( $"Entity {SourceEntity.GetName()} is not related to relationship {Relationship.Name}" );
            }

            // Retrieve main rule for relationship
            MapRule MainRelationshipRule = ModelMap.FindMainRule( Relationship );

            // Check if it is present
            if ( MainRelationshipRule != null )
            {
                // Ok, relationship has a main mapping, it usually means a N:M(:P:...:Z) relationship
                // this requires a custom lookup pipeline
                List<MongoDBOperator> PipelineOperators = new List<MongoDBOperator>();

                // Find Source Main rule
                MapRule MainSourceRule = ModelMap.FindMainRule( SourceEntity.Element );

                if ( MainSourceRule == null )
                {
                    throw new InvalidMapException( $"Left side entities must have a main mapping [{SourceEntity.GetName()}" );
                }

                // In this case we should find a rule for Source Entity that targets the relationship collection
                MapRule SourceRule = ModelMap.FindRule( SourceEntity.Element, MainRelationshipRule.Target );

                // Check if the rule is present
                if ( SourceRule == null )
                {
                    throw new InvalidMapException( $"Missing rules for {SourceEntity.GetName()} that has the same target as the relationship {Relationship.Name}" );
                }

                // Source match attribute
                string SourceMatchAttribute = $"match_{SourceEntity.GetName()}";

                // Setup pipeline variables
                Dictionary<string, string> PipelineVariables = new Dictionary<string, string>();
                PipelineVariables.Add( SourceMatchAttribute, $"${MainSourceRule.GetRuleValueForAttribute( SourceEntity.Element.GetIdentifierAttribute() )}" );

                // Create a Match Operator for the pipeline
                string whatever = SourceRule.GetRuleValueForAttribute( SourceEntity.Element.GetIdentifierAttribute() );

                DataAttribute Identifier = SourceEntity.Element.GetIdentifierAttribute();

                MatchOperator MatchSourceOp = MatchOperator.CreateLookupMatch( SourceRule.GetRuleValueForAttribute( SourceEntity.Element.GetIdentifierAttribute() ), SourceMatchAttribute );

                PipelineOperators.Add( MatchSourceOp );

                // Iterate target entities
                foreach ( QueryableEntity Target in TargetEntities )
                {
                    // Check if the target is related to the source through the relationship
                    if ( !Relationship.AreRelated( (Entity)SourceEntity.Element, (Entity)Target.Element ) )
                    {
                        throw new NotRelatedException( $"Entities {SourceEntity.GetName()} and {Target.GetName()} are not related through {Relationship.Name}" );
                    }

                    // Check if the target has a main mapping
                    MapRule MainTargetRule = ModelMap.FindMainRule( Target.Element );

                    if ( MainTargetRule == null )
                    {
                        throw new MissingMappingException( $"Target entity {Target.GetName()} must have a main mapping" );
                    }
                }

                // Process Relationship attributes (if any)

                // Build Lookup Operator
                LookupOperator LookupOp = new LookupOperator()
                {
                    From = MainRelationshipRule.Target.Name,
                    Let = PipelineVariables,
                    Pipeline = PipelineOperators,
                    As = $"data_{Relationship.Name}"
                };

                OperationsToExecute.Add( LookupOp );
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
        public RelationshipJoinOperator( QueryableEntity SourceEntity, Relationship Relationship, List<QueryableEntity> TargetEntities, ModelMapping Map ) : base( Map )
        {
            this.SourceEntity = SourceEntity;
            this.Relationship = Relationship;
            this.TargetEntities = TargetEntities;
        }

        public RelationshipJoinOperator( QueryableEntity SourceEntity, Relationship Relationship, string RelationshipAlias, List<QueryableEntity> TargetEntities, ModelMapping Map ) : base( Map )
        {
            this.SourceEntity = SourceEntity;
            this.Relationship = Relationship;
            this.TargetEntities = TargetEntities;
            this.RelationshipAlias = RelationshipAlias;
        }
        #endregion
    }
}
