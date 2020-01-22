using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace QueryBuilder.Query
{
    /// <summary>
    /// Represents a query execution stats
    /// </summary>
    public class QueryStats
    {
        #region Properties
        /// <summary>
        /// Was query successfully executed
        /// </summary>
        [JsonProperty("executionSuccess")]
        public bool QuerySuccessful { get; set; }
        /// <summary>
        /// Number of documents returned
        /// </summary>
        [JsonProperty("nReturned")]
        public int DocumentsReturnd { get; set; }
        /// <summary>
        /// Execution time in milliseconds
        /// </summary>
        [JsonProperty("executionTimeMillis")]
        public int ExecutionTime { get; set; }
        /// <summary>
        /// Total keys (as in collection keys) examined
        /// </summary>
        [JsonProperty("totalKeysExamined")]
        public int KeysExaminedCount { get; set; }
        /// <summary>
        /// Total documents examined
        /// </summary>
        [JsonProperty("totalDocsExamined")]
        public int DocumentsExaminedCount { get; set; }
        /// <summary>
        /// Aditional stats
        /// </summary>
        [JsonProperty("executionStages")]
        public QueryExecutionStages Stages { get; set; }
        #endregion
    }
    /// <summary>
    /// Represents additional information for query execution stats
    /// </summary>
    public class QueryExecutionStages
    {
        #region Properties
        /// <summary>
        /// Execution stage (strategy)
        /// </summary>
        [JsonProperty("stage")]
        public string Stage { get; set; }
        /// <summary>
        /// Estimated execution time in milliseconds
        /// </summary>
        [JsonProperty("executionTimeMillisEstimate")]
        public int EstimatedExecutionTime { get; set; }
        #endregion
    }
}
