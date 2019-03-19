using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using QueryBuilder.ER;
using QueryBuilder.Map;
using QueryBuilder.Mongo.Aggregation.Operators;
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

            // Start a Lookup Operator
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
            // keeping data at the same depth
            Unwind unwindOp = new Unwind {
                Field = Op.As
            };         

            // Increment result data
            LastResult.Commands.AddRange(new List<BaseOperator> {
                Op,
                unwindOp
            });

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
