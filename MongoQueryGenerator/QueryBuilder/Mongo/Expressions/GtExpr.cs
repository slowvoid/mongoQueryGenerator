using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QueryBuilder.Javascript;

namespace QueryBuilder.Mongo.Expressions
{
    /// <summary>
    /// Greater than expression
    /// </summary>
    public class GtExpr : BaseLogicalExpression
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
            Dictionary<string, object> GtExprAttr = new Dictionary<string, object>();
            GtExprAttr.Add( "$gt", new JSArray( Values ) );

            return new JSObject( GtExprAttr );
        }
        #endregion

        #region Constructor
        /// <summary>
        /// Initialize a new instance of GtExpr
        /// </summary>
        /// <param name="Field"></param>
        /// <param name="Value"></param>
        public GtExpr( string Field, object Value )
        {
            Values = new List<object>();
            Values.AddRange( new object[] { Field, Value } );
        }
        #endregion
    }
}
