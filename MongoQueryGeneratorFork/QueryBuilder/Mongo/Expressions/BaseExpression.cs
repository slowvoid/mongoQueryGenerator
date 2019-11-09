using QueryBuilder.Javascript;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueryBuilder.Mongo.Expressions
{
    /// <summary>
    /// Base class for expressions
    /// </summary>
    public abstract class BaseExpression
    {
        #region Methods
        /// <summary>
        /// Generates a JavaScript object representing this expression
        /// </summary>
        /// <returns></returns>
        public abstract string ToJavaScript();
        /// <summary>
        /// Generates a Javascript code representing this expression
        /// </summary>
        /// <returns></returns>
        public abstract JSCode ToJSCode();
        #endregion
    }
}
