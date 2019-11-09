using QueryBuilder.Javascript;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueryBuilder.Mongo.Expressions
{
    /// <summary>
    /// Less than expression
    /// </summary>
    public class LtExpr : BaseLogicalExpression
    {
        #region Properties
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
            Dictionary<string, object> LtExprAttr = new Dictionary<string, object>();
            LtExprAttr.Add( "$lt", new JSArray( Values ) );

            return new JSObject( LtExprAttr );
        }
        #endregion

        #region Constructor
        /// <summary>
        /// Initialize a new instance of LtExpr
        /// </summary>
        /// <param name="Field"></param>
        /// <param name="Value"></param>
        public LtExpr( string Field, object Value )
        {
            Values = new List<object>();
            Values.AddRange( new object[] { Field, Value } );
        }
        #endregion
    }
}
