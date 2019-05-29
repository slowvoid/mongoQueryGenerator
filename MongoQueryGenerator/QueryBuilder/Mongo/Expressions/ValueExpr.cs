using QueryBuilder.Javascript;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueryBuilder.Mongo.Expressions
{
    /// <summary>
    /// Represents an expression that points to a value
    /// </summary>
    public class ValueExpr : ProjectExpression
    {
        #region Properties
        /// <summary>
        /// Value
        /// </summary>
        public string Value { get; set; }
        #endregion

        #region Methods
        /// <summary>
        /// Generates a javascript representation of this instance
        /// </summary>
        /// <returns></returns>
        public override JSCode ToJSCode()
        {
            return new JSString( Value );
        }
        #endregion

        #region Constructor
        /// <summary>
        /// Initializes a new instance of ValueExpr
        /// </summary>
        /// <param name="Value">Value to reference</param>
        public ValueExpr( string Value )
        {
            this.Value = Value;
        }
        #endregion
    }
}
