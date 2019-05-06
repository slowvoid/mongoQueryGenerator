using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueryBuilder.Mongo.Aggregation.Operators
{
    /// <summary>
    /// Base aggregate operator
    /// </summary>
    public abstract class BaseOperator
    {
        #region Properties
        /// <summary>
        /// Operator name
        /// </summary>
        [BsonIgnore]
        public string Name { get; set; }
        #endregion

        #region Methods
        /// <summary>
        /// Generates a JavaScript object representing this operator
        /// </summary>
        /// <returns></returns>
        public abstract string ToJavaScript();
        #endregion
    }
}
