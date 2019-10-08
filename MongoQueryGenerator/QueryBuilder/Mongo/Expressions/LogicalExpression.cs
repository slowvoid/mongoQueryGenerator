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
    public class LogicalExpression : BaseLogicalExpression
    {
        #region Properties
        /// <summary>
        /// Expression left operand
        /// </summary>
        public object LeftOperand { get; set; }
        /// <summary>
        /// Expression right operand
        /// </summary>
        public object RightOperand { get; set; }
        /// <summary>
        /// Logical operator
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
        /// Generates a JSCode representation of this instance
        /// </summary>
        /// <returns></returns>
        public override JSCode ToJSCode()
        {
            string OperatorName = _getOperatorString( Operator );
            JSArray OperatorValues = new JSArray( new List<object> { LeftOperand, RightOperand } );

            Dictionary<string, object> ObjectData = new Dictionary<string, object>();
            ObjectData.Add( OperatorName, OperatorValues );

            return new JSObject( ObjectData );
        }
        #endregion

        #region Constructor
        /// <summary>
        /// Initialize a new instance of LogicalExpression
        /// </summary>
        /// <param name="LeftOperand"></param>
        /// <param name="Operator"></param>
        /// <param name="RightOperand"></param>
        public LogicalExpression( object LeftOperand, LogicalOperator Operator, object RightOperand )
        {
            this.LeftOperand = LeftOperand;
            this.Operator = Operator;
            this.RightOperand = RightOperand;
        }
        #endregion
    }
}
