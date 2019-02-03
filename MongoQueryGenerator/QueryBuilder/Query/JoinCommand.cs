using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueryBuilder.Query
{
    /// <summary>
    /// Join Command class
    /// </summary>
    public class JoinCommand : QueryCommand
    {
        #region Properties
        /// <summary>
        /// Join this collection
        /// </summary>
        [JsonProperty("from")]
        public string From { get; set; }
        /// <summary>
        /// Using this attribute as foreign identifier
        /// </summary>
        [JsonProperty("foreignField")]
        public string ForeignField { get; set; }
        /// <summary>
        /// Using this attribute as local identifier
        /// </summary>
        [JsonProperty("localField")]
        public string LocalField { get; set; }
        /// <summary>
        /// Return data to this field
        /// </summary>
        [JsonProperty("as")]
        public string AsField { get; set; }
        #endregion
    }
}
