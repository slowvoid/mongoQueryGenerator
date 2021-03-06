﻿using QueryBuilder.Javascript;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueryBuilder.Mongo.Expressions
{
    /// <summary>
    /// Represents the $expr expression
    /// </summary>
    public class Expr : BaseLogicalExpression
    {
        #region Properties
        /// <summary>
        /// Expression to be interpreted
        /// </summary>
        public BaseLogicalExpression Expression { get; set; }
        #endregion

        #region Override
        /// <summary>
        /// Generate a JavaScript representation of this instance
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
            Dictionary<string, object> ExprAttributes = new Dictionary<string, object>();
            ExprAttributes.Add( "$expr", Expression.ToJSCode() );

            JSObject ExprObj = new JSObject( ExprAttributes );

            return ExprObj;
        }
        #endregion

        #region Constructor
        /// <summary>
        /// Initializes a new Expr instance
        /// </summary>
        /// <param name="Expression">Expressiono to be evalueted</param>
        public Expr( BaseLogicalExpression Expression )
        {
            this.Expression = Expression;
        }
        #endregion
    }
}
