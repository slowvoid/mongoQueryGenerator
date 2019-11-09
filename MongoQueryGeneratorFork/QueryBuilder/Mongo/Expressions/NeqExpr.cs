using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QueryBuilder.Javascript;

namespace QueryBuilder.Mongo.Expressions
{
    /// <summary>
    /// Represents a Not Equal expression ($ne)
    /// </summary>
    public class NeqExpr : BaseLogicalExpression
    {
        #region Properties
        /// <summary>
        /// Values to compare against
        /// </summary>
        public List<object> Values { get; set; }
        #endregion

        #region Methods
        /// <summary>
        /// Generate a javascript representation
        /// </summary>
        /// <returns></returns>
        public override string ToJavaScript()
        {
            return ToJSCode().ToString();
        }
        /// <summary>
        /// Generate a javascript representation
        /// </summary>
        /// <returns></returns>
        public override JSCode ToJSCode()
        {
            Dictionary<string, object> NeqExprAttr = new Dictionary<string, object>();
            NeqExprAttr.Add( "$ne", new JSArray( Values ) );

            return new JSObject( NeqExprAttr );
        }
        #endregion

        #region Constructor
        /// <summary>
        /// Initialize a new instance of NeqExpr
        /// </summary>
        /// <param name="Value"></param>
        public NeqExpr( string Field, object Value )
        {
            this.Values = new List<object>();
            Values.AddRange( new object[] { Field, Value } );
        }
        #endregion
    }
}
