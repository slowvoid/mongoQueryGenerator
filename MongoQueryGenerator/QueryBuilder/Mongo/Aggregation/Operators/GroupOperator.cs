using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QueryBuilder.Javascript;
using QueryBuilder.Mongo.Aggregation.Operators.Exceptions;
using QueryBuilder.Mongo.Aggregation.Operators.GroupExpressions;

namespace QueryBuilder.Mongo.Aggregation.Operators
{
    /// <summary>
    /// Represents the Group operator/stage ($group)
    /// </summary>
    public class GroupOperator : MongoDBOperator
    {
        #region Properties
        /// <summary>
        /// Expression to be used as _id
        /// Should be ObjectGroupExpression or StringGroupExpression
        /// </summary>
        public GroupExpression ID
        {
            get
            {
                return _id;
            }
            set
            {
                if ( !( value is ObjectGroupExpression ) || !( value is StringGroupExpression ) )
                {
                    throw new InvalidGroupExpressionException( "_id must be either ObjectGroupExpression or StringGroupExpression" );
                }

                _id = value;
            }
        }

        private GroupExpression _id;
        /// <summary>
        /// Attributes to projected and how to project them
        /// </summary>
        public Dictionary<string, GroupExpression> Attributes { get; set; }
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

            return new JSObject( "$group", ProcessedAttributes );
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

        #region
        /// <summary>
        /// Initialize a new GroupOperator instance
        /// </summary>
        /// <param name="inAttributes"></param>
        public GroupOperator( Dictionary<string, GroupExpression> inAttributes )
        {
            Attributes = inAttributes;
        }
        #endregion
    }
}
