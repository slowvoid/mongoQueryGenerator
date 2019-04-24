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
