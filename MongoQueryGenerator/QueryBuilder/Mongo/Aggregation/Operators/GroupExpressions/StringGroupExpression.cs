using QueryBuilder.Javascript;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueryBuilder.Mongo.Aggregation.Operators.GroupExpressions
{
    /// <summary>
    /// Represents a string value (constant or document attribute reference)
    /// </summary>
    public class StringGroupExpression : GroupExpression
    {
        #region Properties
        /// <summary>
        /// String expression
        /// </summary>
        public string Expression { get; set; }
        #endregion

        #region Methods
        /// <summary>
        /// Get a JSCode instance based on this expression
        /// </summary>
        /// <returns></returns>
        public override JSCode ToJSCode()
        {
            return new JSString( $"\"{Expression}\"" );                        
        }
        /// <summary>
        /// Returns the corresponde JavaScript code as string
        /// </summary>
        /// <returns></returns>
        public override string ToJavaScript()
        {
            return ToJSCode().ToString();
        }
        #endregion

        #region Constructor
        /// <summary>
        /// Initialize a new StringGroupExpression
        /// </summary>
        /// <param name="inExpression"></param>
        public StringGroupExpression( string inExpression, bool isAttributeReference = false )
        {
            Expression = inExpression;

            if ( isAttributeReference )
            {
                Expression = $"${inExpression}";
            }
        }
        #endregion
    }
}
