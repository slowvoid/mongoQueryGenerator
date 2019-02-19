using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using QueryBuilder.Map;
using QueryBuilder.Operation;
using QueryBuilder.ER;

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
        #endregion

        #region Methods
        /// <summary>
        /// Run the query generator
        /// </summary>
        /// <returns></returns>
        public string Run()
        {
            // Setup results
            OperationResult Result = new OperationResult( new List<BaseERElement>(), new List<QueryCommand>() );

            foreach ( BaseOperation Op in Pipeline.Operations )
            {
                Result = Op.Run( Result );
            }

            // Generate mongodb commands
            StringBuilder sb = new StringBuilder();

            foreach ( QueryCommand Command in Result.Commands )
            {
                sb.Append( JsonConvert.SerializeObject( Command ) );
            }

            return sb.ToString();
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
