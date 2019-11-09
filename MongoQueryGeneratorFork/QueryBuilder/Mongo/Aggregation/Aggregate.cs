using QueryBuilder.Mongo.Aggregation.Operators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueryBuilder.Mongo.Aggregation
{
    /// <summary>
    /// Represents the MongoDB aggregation
    /// </summary>
    public class Aggregate
    {
        #region Properties
        /// <summary>
        /// Collection serving as start point for the aggregation
        /// </summary>
        public MongoDBCollection Collection { get; set; }
        /// <summary>
        /// List of operations to  be execute in this aggregation
        /// </summary>
        public List<MongoDBOperator> Operators { get; set; }
        #endregion

    }
}
