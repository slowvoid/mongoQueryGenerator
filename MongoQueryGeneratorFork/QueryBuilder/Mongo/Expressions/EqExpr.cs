using QueryBuilder.Javascript;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueryBuilder.Mongo.Expressions
{
    /// <summary>
    /// Represents an $eq expression
    /// </summary>
    public class EqExpr : BaseLogicalExpression
    {
        #region Properties
        /// <summary>
        /// Values to be matched
        /// </summary>
        public List<object> Values { get; set; }
        #endregion

        #region Override
        /// <summary>
        /// Generates a JavaScript representation of this instance
        /// </summary>
        /// <returns></returns>
        public override string ToJavaScript()
        {
            return ToJSCode().ToString();
        }
        /// <summary>
        /// Generates a Javascript code object representing this instance
        /// </summary>
        /// <returns></returns>
        public override JSCode ToJSCode()
        {
            Dictionary<string, object> EqExprAttr = new Dictionary<string, object>();
            EqExprAttr.Add( "$eq", new JSArray( Values ) );

            return new JSObject( EqExprAttr );
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Initialize a new EqExpr instance
        /// </summary>
        /// <param name="Field">Field to compare</param>
        /// <param name="Value">Value</param>
        public EqExpr(string Field, object Value)
        {
            Values = new List<object>();
            Values.AddRange( new object[] { Field, Value } );
        }
        #endregion
    }
}
