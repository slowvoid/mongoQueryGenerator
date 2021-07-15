using QueryBuilder.Javascript;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueryBuilder.Mongo.Aggregation.Operators.GroupExpressions
{
    /// <summary>
    /// Expression that uses the first ocurrence of the given attribute
    /// </summary>
    public class FirstGroupExpression : GroupExpression
    {
        #region Properties
        /// <summary>
        /// Attribute to output
        /// </summary>
        public string Attribute { get; set; }
        #endregion

        #region Methods
        /// <summary>
        /// Get a JSCode instance based on this expression
        /// </summary>
        /// <returns></returns>
        public override JSCode ToJSCode()
        {
            Dictionary<string, object> Attributes = new Dictionary<string, object>();
            Attributes.Add( "$first", $"${Attribute}" );

            return new JSObject( Attributes );
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
        /// Initialize a new instance of FirstGroupExpression
        /// </summary>
        /// <param name="inAttribute"></param>
        public FirstGroupExpression( string inAttribute )
        {
            Attribute = inAttribute;
        }
        #endregion
    }
}
