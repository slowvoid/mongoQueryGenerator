using QueryBuilder.Javascript;
using System;
using System.Collections.Generic;
using System.Text;

namespace QueryBuilder.Mongo.Expressions
{
    /// <summary>
    /// In Array expression for when the Array is a field in the database
    /// </summary>
    public class InFieldArrayExpr : BaseLogicalExpression
    {
        #region Properties
        /// <summary>
        /// Array field
        /// </summary>
        public string Field { get; set; }
        /// <summary>
        /// Value to check
        /// </summary>
        public object Value { get; set; }
        #endregion

        #region Methods
        /// <summary>
        /// Generates a JavaScript representation of this expression
        /// </summary>
        /// <returns></returns>
        public override string ToJavaScript()
        {
            return ToJSCode().ToString();
        }
        /// <summary>
        /// Generates a JavaScript representation of this expression
        /// </summary>
        /// <returns></returns>
        public override JSCode ToJSCode()
        {
            Dictionary<string, object> InExprAttr = new Dictionary<string, object>();
            List<object> OpValues = new List<object>() { Value, Field };
            InExprAttr.Add( "$in", new JSArray( OpValues ) );

            return new JSObject( InExprAttr );
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Initialize a new InFieldArrayExpr instance
        /// </summary>
        /// <param name="Field"></param>
        /// <param name="Value"></param>
        public InFieldArrayExpr(string Field, object Value)
        {
            this.Field = Field;
            this.Value = Value;
        }
        #endregion
    }
}
