using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using QueryBuilder.Map;
using QueryBuilder.Operation;

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
            StringBuilder sb = new StringBuilder();

            foreach ( BaseOperation Op in Pipeline.Operations )
            {
                sb.Append( JsonConvert.SerializeObject( Op.Run() ) );
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
