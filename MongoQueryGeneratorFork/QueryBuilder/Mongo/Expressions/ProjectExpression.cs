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
        #region Properties
        /// <summary>
        /// Gets wheter the expression is adding a new field or forcing it to be visible
        /// </summary>
        public bool IsAddingOrForcingAFieldVisible { get; private protected set; }
        #endregion

        #region Methods
        /// <summary>
        /// Generates a Javascript code object representing this instance
        /// </summary>
        /// <returns></returns>
        public abstract JSCode ToJSCode();
        #endregion
    }
}
