using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using QueryBuilder.Map;
using QueryBuilder.Operation;
using QueryBuilder.ER;
using QueryBuilder.Mongo.Aggregation.Operators;

namespace QueryBuilder.Query
{
    /// <summary>
    /// Class responsible for running a pipeline and generating
    /// MongoDB queries
    /// </summary>
    public class QueryGenerator
    {
        #region Properties
        /// <summary>
        /// Command pipeline
        /// </summary>
        public Pipeline Pipeline { get; set; }
        /// <summary>
        /// Collection used a start point
        /// </summary>
        public string CollectionName { get; set; }
        #endregion

        #region Methods
        /// <summary>
        /// Run the query generator
        /// </summary>
        /// <returns></returns>
        public string Run()
        {
            // Setup results
            OperationResult Result = new OperationResult( new List<BaseERElement>(), new List<BaseOperator>() );

            foreach ( BaseOperation Op in Pipeline.Operations )
            {
                Result = Op.Run( Result );
            }

            // Store command objects
            List<string> AggregatePipeline = new List<string>();

            foreach ( BaseOperator Command in Result.Commands )
            {
                AggregatePipeline.Add( Command.ToJavaScript() );
            }

            // TODO: Update this section to generate collection and aggregate
            // according to the query
            return string.Format( "db.{0}.aggregate([{1}]);", CollectionName, string.Join( ",", AggregatePipeline ) );
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Initialize a new instance of QueryGenerator class
        /// </summary>
        /// <param name="Pipeline">Command pipeline</param>
        public QueryGenerator( Pipeline Pipeline )
        {
            this.Pipeline = Pipeline;
        }
        #endregion
    }
}
