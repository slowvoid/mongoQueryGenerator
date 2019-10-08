using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QueryBuilder.Javascript;

namespace QueryBuilder.Mongo.Expressions
{
    /// <summary>
    /// Represents a logical expression
    /// </summary>
    public class LogicalExpressionGroup : BaseLogicalExpression
    {
        #region Properties
        /// <summary>
        /// Expression on the left operand
        /// </summary>
        public BaseLogicalExpression LeftOperand { get; set; }
        /// <summary>
        /// Expression on the right operand
        /// </summary>
        public BaseLogicalExpression RightOperand { get; set; }
        /// <summary>
        /// Logical operator applied on operands
        /// </summary>
        public LogicalOperator Operator { get; set; }
        #endregion

        #region Methods
        /// <summary>
        /// Generates a string representing the javascript version of this instance
        /// </summary>
        /// <returns></returns>
        public override string ToJavaScript()
        {
            return ToJSCode().ToString();
        }
        /// <summary>
        /// Create a JavaScript representation of this object
        /// </summary>
        /// <returns></returns>
        public override JSCode ToJSCode()
        {
            string OperatorName = _getOperatorString( Operator );
            JSArray OperatorValues = new JSArray( new List<object> { LeftOperand.ToJSCode(), RightOperand.ToJSCode() } );

            Dictionary<string, object> ObjectData = new Dictionary<string, object>();
            ObjectData.Add( OperatorName, OperatorValues );

            return new JSObject( ObjectData );
        }
        #endregion

        #region Constructor
        public LogicalExpressionGroup( BaseLogicalExpression LeftOperand, LogicalOperator Operator, BaseLogicalExpression RightOperand )
        {
            this.LeftOperand = LeftOperand;
            this.Operator = Operator;
            this.RightOperand = RightOperand;
        }
        #endregion
    }
}
