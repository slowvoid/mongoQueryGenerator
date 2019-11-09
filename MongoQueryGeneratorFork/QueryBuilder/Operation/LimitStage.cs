using QueryBuilder.Mongo.Aggregation.Operators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueryBuilder.Operation
{
    /// <summary>
    /// Represents a stage that uses a $limit operator
    /// </summary>
    public class LimitStage : AlgebraOperator
    {
        #region Properties
        /// <summary>
        /// Limit threshold
        /// </summary>
        public int Count { get; set; }
        #endregion

        #region Methods
        /// <summary>
        /// Generate the stage query
        /// </summary>
        /// <returns></returns>
        public override AlgebraOperatorResult Run()
        {
            List<MongoDBOperator> OperatorsToExecute = new List<MongoDBOperator>();
            OperatorsToExecute.Add( new LimitOperator( Count ) );

            return new AlgebraOperatorResult( OperatorsToExecute );            
        }
        #endregion

        #region Constructor
        /// <summary>
        /// Initialize a new LimitStage instance
        /// </summary>
        /// <param name="Count"></param>
        public LimitStage(int Count)
        {
            this.Count = Count;
        }
        #endregion
    }
}
