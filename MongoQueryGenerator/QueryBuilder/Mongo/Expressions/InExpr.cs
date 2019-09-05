using QueryBuilder.Javascript;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueryBuilder.Mongo.Expressions
{
    /// <summary>
    /// In array expression
    /// </summary>
    public class InExpr : BaseExpression
    {
        #region Properties
        /// <summary>
        /// Field
        /// </summary>
        public string Field { get; set; }
        /// <summary>
        /// Values
        /// </summary>
        public List<object> Values { get; set; }
        #endregion

        #region Methods
        /// <summary>
        /// Generates a JavaScript representation of this instance
        /// </summary>
        /// <returns></returns>
        public override string ToJavaScript()
        {
            return ToJSCode().ToString();
        }
        /// <summary>
        /// Generates a JavaScript representation of this instance
        /// </summary>
        /// <returns></returns>
        public override JSCode ToJSCode()
        {
            Dictionary<string, object> InExprAttr = new Dictionary<string, object>();
            List<object> OpValues = new List<object>() { Field, new JSArray( Values ) };
            InExprAttr.Add( "$in", new JSArray( OpValues ) );

            return new JSObject( InExprAttr );
        }
        #endregion

        #region Constructor
        /// <summary>
        /// Initialize a new instance of InExpr
        /// </summary>
        /// <param name="Field"></param>
        /// <param name="Value"></param>
        public InExpr( string Field, List<object> Values )
        {
            this.Field = Field;
            this.Values = Values;
        }
        #endregion
    }
}
