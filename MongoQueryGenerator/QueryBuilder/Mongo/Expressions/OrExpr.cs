using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QueryBuilder.Javascript;

namespace QueryBuilder.Mongo.Expressions
{
    /// <summary>
    /// Represents an OR expresssion $or: [...]
    /// This expressions support comparing a single field to One or more values
    /// For multiple field comparision see <see cref="LogicalExpressionGroup"/>
    /// </summary>
    public class OrExpr : BaseLogicalExpression
    {
        #region Properties
        /// <summary>
        /// Set of expression to evaluate
        /// </summary>
        public List<object> ValuesToCompare { get; set; }
        /// <summary>
        /// Field to compare to
        /// </summary>
        public string Field { get; set; }
        #endregion

        #region Methods
        /// <summary>
        /// Generate a JavaScript representation
        /// </summary>
        /// <returns></returns>
        public override JSCode ToJSCode()
        {
            string OperatorName = _getOperatorString( LogicalOperator.OR );
            List<object> OperatorValues = new List<object>();

            ValuesToCompare.ForEach( E => OperatorValues.Add( new EqExpr( Field, E ).ToJSCode() ) );

            Dictionary<string, object> ObjectData = new Dictionary<string, object>();
            ObjectData.Add( OperatorName, new JSArray( OperatorValues ) );

            return new JSObject( ObjectData );
        }
        /// <summary>
        /// Generate a JavaScript representation
        /// </summary>
        /// <returns></returns>
        public override string ToJavaScript()
        {
            return ToJSCode().ToString();
        }
        #endregion

        #region Constructor
        /// <summary>
        /// Initialize a new instance of OrExpr
        /// </summary>
        /// <param name="Field"></param>
        /// <param name="ValuesToCompare"></param>
        public OrExpr( string Field, List<object> ValuesToCompare )
        {
            this.Field = Field;
            this.ValuesToCompare = ValuesToCompare;
        }
        #endregion
    }
}
