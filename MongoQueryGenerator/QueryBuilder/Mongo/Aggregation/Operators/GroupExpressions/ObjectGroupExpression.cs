using QueryBuilder.Javascript;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueryBuilder.Mongo.Aggregation.Operators.GroupExpressions
{
    /// <summary>
    /// Represents an object as value of an attribute for $group
    /// </summary>
    public class ObjectGroupExpression : GroupExpression
    {
        #region Properties
        /// <summary>
        /// Object attributes
        /// </summary>
        public Dictionary<string, GroupExpression> Attributes { get; set; }
        /// <summary>
        /// [Optional] Object key
        /// </summary>
        public string Key { get; set; }
        #endregion

        #region Methods
        /// <summary>
        /// Get a JSCode instance based on this expression
        /// </summary>
        /// <returns></returns>
        public override JSCode ToJSCode()
        {
            Dictionary<string, object> ProcessedAttributes = new Dictionary<string, object>();

            foreach ( KeyValuePair<string, GroupExpression> Attribute in Attributes )
            {
                ProcessedAttributes.Add( Attribute.Key, Attribute.Value.ToJSCode() );
            }

            if ( string.IsNullOrWhiteSpace(Key) )
            {
                return new JSObject( ProcessedAttributes );
            }

            return new JSObject( Key, ProcessedAttributes );
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
        /// Initialize a new ObjectGroupExpression instance
        /// </summary>
        /// <param name="inAttributes"></param>
        public ObjectGroupExpression( Dictionary<string, GroupExpression> inAttributes )
        {
            Attributes = inAttributes;
        }
        /// <summary>
        /// Initialize a new ObjectGroupExpression instance
        /// </summary>
        /// <param name="inKey"></param>
        /// <param name="inAttributes"></param>
        public ObjectGroupExpression( string inKey, Dictionary<string, GroupExpression> inAttributes )
        {
            Key = inKey;
            Attributes = inAttributes;
        }
        #endregion
    }
}
