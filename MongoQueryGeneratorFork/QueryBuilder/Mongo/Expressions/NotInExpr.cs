using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QueryBuilder.Javascript;

namespace QueryBuilder.Mongo.Expressions
{
    /// <summary>
    /// Represents a Not In expression
    /// </summary>
    public class NotInExpr : InExpr
    {
        #region Methods
        /// <summary>
        /// Return a javascript representation of this instance
        /// </summary>
        /// <returns></returns>
        public override JSCode ToJSCode()
        {
            List<object> Params = new List<object>() { base.ToJSCode() };
            Dictionary<string, object> NotInExprAttrs = new Dictionary<string, object>();
            NotInExprAttrs.Add( "$not", new JSArray( Params ) );

            return new JSObject( NotInExprAttrs );
        }
        #endregion

        #region Constructor
        /// <summary>
        /// Initialize a new instance of NotInExpr
        /// </summary>
        /// <param name="Field"></param>
        /// <param name="Values"></param>
        public NotInExpr( string Field, List<object> Values ) : base( Field, Values ) { }
        #endregion
    }
}
