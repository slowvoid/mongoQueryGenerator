using QueryBuilder.Javascript;
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
    public abstract class MongoDBOperator
    {
        #region Properties
        /// <summary>
        /// Operator name
        /// </summary>
        public string Name { get; set; }
        #endregion

        #region Methods
        /// <summary>
        /// Generates a JavaScript object representing this operator
        /// </summary>
        /// <returns></returns>
        public abstract string ToJavaScript();
        /// <summary>
        /// Generates a Javascript code object representing this instance
        /// </summary>
        /// <returns></returns>
        public abstract JSCode ToJSCode();
        #endregion
    }
}
