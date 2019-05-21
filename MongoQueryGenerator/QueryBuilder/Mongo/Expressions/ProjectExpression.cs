using QueryBuilder.Javascript;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueryBuilder.Mongo.Expressions
{
    /// <summary>
    /// Represents an expression to be used in a project operation
    /// </summary>
    public abstract class ProjectExpression
    {
        #region Methods
        /// <summary>
        /// Generates a Javascript code object representing this instance
        /// </summary>
        /// <returns></returns>
        public abstract JSCode ToJSCode();
        #endregion
    }
}
