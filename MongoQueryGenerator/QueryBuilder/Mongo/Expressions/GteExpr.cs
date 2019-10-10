using QueryBuilder.Javascript;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueryBuilder.Mongo.Expressions
{
    /// <summary>
    /// Greater than or equal expression
    /// </summary>
    public class GteExpr : BaseLogicalExpression
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
            Dictionary<string, object> GteExprAttr = new Dictionary<string, object>();
            GteExprAttr.Add( "$gte", new JSArray( Values ) );

            return new JSObject( GteExprAttr );
        }
        #endregion

        #region Constructor
        /// <summary>
        /// Initialize a new instance of GteExpr
        /// </summary>
        /// <param name="Field"></param>
        /// <param name="Value"></param>
        public GteExpr( string Field, object Value )
        {
            Values = new List<object>();
            Values.AddRange( new object[] { Field, Value } );
        }
        #endregion
    }
}
