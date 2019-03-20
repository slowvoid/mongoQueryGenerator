using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using QueryBuilder.ER;
using QueryBuilder.Map;
using QueryBuilder.Mongo;
using QueryBuilder.Mongo.Aggregation.Operators;
using QueryBuilder.Mongo.Expressions;
using QueryBuilder.Operation.Exceptions;
using QueryBuilder.Query;

namespace QueryBuilder.Operation
{
    /// <summary>
    /// Represents a JOIN operation
    /// </summary>
    public class JoinOperation : BaseOperation
    {
        #region Properties
        /// <summary>
        /// Relationship startpoint
        /// </summary>
        public Entity SourceEntity { get; set; }
        /// <summary>
        /// Relationship endpoint
        /// </summary>
        public Entity TargetEntity { get; set; }
        /// <summary>
        /// Relationship connecting both entities
        /// </summary>
        public Relationship Relationship { get; set; }
        #endregion

        #region Private Data
        
        #endregion

        #region Methods
        public override OperationResult Run( OperationResult LastResult )
        {
            // Get Map rule
            MapRule SourceRule = ModelMap.Rules.Find( R => R.Source == SourceEntity );
            MapRule TargetRule = ModelMap.Rules.Find( R => R.Source == TargetEntity );
            MapRule RelationshipRule = ModelMap.Rules.Find( R => R.Source == Relationship );

            // Check if the entities are mapped to different collections
            bool EntitiesShareTarget = SourceRule.Target == TargetRule.Target;
            // Also check if the relationship has attributes and shares a mapping rule with either entities
            bool RelationshipHasAttributes = Relationship.Attributes.Count > 0;
            bool RelationshipSharesTarget = true;
            if ( RelationshipRule != null )
            {
                RelationshipSharesTarget = ( new Collection[] { SourceRule.Target, TargetRule.Target } ).Contains( RelationshipRule.Target );
            }

            // Store operations to be performed
            List<BaseOperator> OperationsToRun = new List<BaseOperator>();

            if ( EntitiesShareTarget )
            {
                // Entities are mapped to the same collection
                // This means a simpler join
                // TODO: Replace with a more proper aggregate stage or find command
                LookupOperator LookupOp = new LookupOperator {
                    From = TargetRule.Target.Name,
                    ForeignField = TargetRule.Rules.First( K => K.Key == Relationship.TargetAttribute.Name ).Value,
                    LocalField = SourceRule.Rules.First( K => K.Key == Relationship.SourceAttribute.Name ).Value,
                    As = Relationship.Name
                };
            }
            else
            {
                if ( RelationshipHasAttributes && !RelationshipSharesTarget )
                {
                    // Relationship has attributes
                    // But does not share a collection with either entities
                    // So we assume it is a Many to Many relationship and try to join the target collection
                    // through the relationship collection
                    LookupOperator RelationshipLookup = new LookupOperator {
                        From = RelationshipRule.Target.Name,
                        ForeignField = RelationshipRule.Rules.First( K => K.Key == Relationship.RefToSourceAttribute.Name ).Value,
                        LocalField = RelationshipRule.Rules.First( K => K.Key == Relationship.SourceAttribute.Name ).Value,
                        As = Relationship.Name
                    };

                    // Now we've created a new attribute with the relationship data (all ocurrences)
                    // We should create a new document for each ocurrence and bring it to the same level as the other attributes
                    Unwind UnwindRelationship = new Unwind {
                        Field = RelationshipLookup.As
                    };

                    // Now join target entity
                    LookupOperator TargetLookup = new LookupOperator {
                        From = TargetRule.Target.Name,
                        ForeignField = TargetRule.Rules.First( K => K.Key == Relationship.TargetAttribute.Name ).Value,
                        LocalField = string.Format( "{0}.{1}", Relationship.Name, RelationshipRule.Rules.First( K => K.Key == Relationship.RefToTargetAttribute.Name ).Value ),
                        As = string.Format( "{0}_{1}", Relationship.Name, TargetEntity.Name )
                    };

                    // Unwind target entity data
                    Unwind UnwindTarget = new Unwind {
                        Field = TargetLookup.As
                    };

                    // Bring the relationship data to the same level
                    MergeObjects MergeData = new MergeObjects();
                    MergeData.Objects.AddRange( new List<string> {
                        string.Format( "${0}", RelationshipLookup.As ),
                        string.Format( "${0}", TargetLookup.As ),
                        "$$ROOT"
                    });
                    // Merges relationship data into the root document
                    ReplaceRoot ReplaceData = new ReplaceRoot {
                        NewRoot = MergeData.ToJavaScript()
                    };

                    // Add to list of operations
                    OperationsToRun.AddRange( new List<BaseOperator> {
                        RelationshipLookup,
                        UnwindRelationship,
                        TargetLookup,
                        UnwindTarget,
                        ReplaceData
                    } );
                }
                else
                {
                    // Relationship shares a collection with either collection
                    // So just lookup the target
                    LookupOperator Op = new LookupOperator {
                        // Set target entity
                        From = TargetRule.Target.Name,
                        // Locate mapping for the source / target attributes
                        ForeignField = TargetRule.Rules.First( K => K.Key == Relationship.TargetAttribute.Name ).Value,
                        LocalField = SourceRule.Rules.First( K => K.Key == Relationship.SourceAttribute.Name ).Value,

                        // As field (For now use relationship name)
                        As = Relationship.Name
                    };

                    // Also ask the database to perform an unwind operation
                    Unwind unwindOp = new Unwind {
                        Field = Op.As
                    };

                    // Bring data to the same level
                    MergeObjects MergeData = new MergeObjects();
                    MergeData.Objects.AddRange( new List<string> {
                        string.Format( "${0}", Op.As ),
                        "$$ROOT"
                    } );

                    ReplaceRoot ReplaceData = new ReplaceRoot {
                        NewRoot = MergeData.ToJavaScript()
                    };

                    // Add operations to list
                    OperationsToRun.AddRange( new List<BaseOperator> {
                        Op,
                        unwindOp,
                        ReplaceData
                    } );
                }
            }

            LastResult.Commands.AddRange( OperationsToRun );
            LastResult.PipelineResult.AddRange( new BaseERElement[] { SourceEntity, TargetEntity, Relationship } );

            return LastResult;
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Initialize a new instance of Join Operation class
        /// </summary>
        /// <param name="SourceEntity">Source entity</param>
        /// <param name="TargetEntity">Target entity</param>
        /// <param name="Relationship">Join through this relationship</param>
        /// <param name="ModelMap">Map rules between ER and Mongo</param>
        public JoinOperation( Entity SourceEntity, Entity TargetEntity, Relationship Relationship, ModelMapping ModelMap ) : base( ModelMap )
        {
            this.SourceEntity = SourceEntity;
            this.TargetEntity = TargetEntity;
            this.Relationship = Relationship;
        }
        #endregion
    }
}
