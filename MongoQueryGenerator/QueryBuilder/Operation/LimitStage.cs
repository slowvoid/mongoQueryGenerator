using QueryBuilder.Map;
using QueryBuilder.Mongo.Aggregation.Operators;
using QueryBuilder.Operation.Arguments;
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


        override public string SummarizeToString()
        {
            string Ret = "LimitStage";

            return Ret;
        }

        /// <summary>
        /// Generate the stage query
        /// </summary>
        /// <returns></returns>
        public override AlgebraOperatorResult Run( IModelMap inMap, IEnumerable<ProjectArgument> inAttributesToProject = null )
        {
            RuleMap = inMap;
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
