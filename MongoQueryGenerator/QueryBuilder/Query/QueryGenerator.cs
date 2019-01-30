using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        /// <summary>
        /// Mapping between ER and MongoDB
        /// </summary>
        public ModelMapping Map { get; set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Initialize a new instance of QueryGenerator class
        /// </summary>
        /// <param name="Pipeline">Command pipeline</param>
        /// <param name="Map">Model mapping</param>
        public QueryGenerator( Pipeline Pipeline, ModelMapping Map )
        {
            this.Pipeline = Pipeline;
            this.Map = Map;
        }
        #endregion
    }
}
