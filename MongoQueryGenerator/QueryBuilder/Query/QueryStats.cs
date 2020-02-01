using MongoDB.Bson.Serialization.Attributes;
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
        /// <summary>
        /// Additional stats
        /// </summary>
        [JsonProperty("inputStage")]
        public QueryInputStage InputStage { get; set; },
        /// <summary>
        /// Filter used
        /// </summary>
        [JsonProperty("filter")]
        public string Filter { get; set; }
        #endregion
    }
    /// <summary>
    /// Addition stats
    /// </summary>
    public class QueryInputStage
    {
        /// <summary>
        /// Stage name
        /// </summary>
        [JsonProperty("stage")]
        public string Stage { get; set; }
        /// <summary>
        /// Number of documents returned
        /// </summary>
        [JsonProperty("nReturned")]
        public int DocumentsReturn { get; set; }
        /// <summary>
        /// Estimated execution time in milliseconds
        /// </summary>
        [JsonProperty( "executionTimeMillisEstimate" )]
        public int EstimatedExecutionTime { get; set; }
        /// <summary>
        /// Amount of documents returned or advanced to the parent stage
        /// </summary>
        [JsonProperty("advanced")]
        public int ReturnedOrAdvancedCount { get; set; }
        /// <summary>
        /// Name of the index used
        /// </summary>
        [JsonProperty("indexName")]
        public string IndexName { get; set; }
        /// <summary>
        /// Index direction
        /// </summary>
        [JsonProperty("direction")]
        public string IndexDirection { get; set; }
        /// <summary>
        /// Additional stats
        /// </summary>
        [JsonProperty( "inputStage", NullValueHandling = NullValueHandling.Ignore )]
        [BsonIgnoreIfNull]
        public QueryInputStage InputStage { get; set; }
    }
}
