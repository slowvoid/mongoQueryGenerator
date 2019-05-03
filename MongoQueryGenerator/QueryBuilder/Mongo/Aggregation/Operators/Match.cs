using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using QueryBuilder.Mongo.Expressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueryBuilder.Mongo.Aggregation.Operators
{
    /// <summary>
    /// Represents the Match aggregation stage
    /// </summary>
    public class Match : BaseOperator
    {
        #region Properties
        /// <summary>
        /// Expression to execute
        /// </summary>
        [BsonIgnore]
        public BaseExpression Expression { get; set; }
        /// <summary>
        /// Fields to match against
        /// </summary>
        [BsonIgnore]
        public Dictionary<string, object> FieldsToMatch { get; set; }
        [BsonElement("$match")]
        private string JsonExpression { get; set; }
        #endregion

        #region Overrides
        /// <summary>
        /// Generates a JavaScript compatible version of this instance
        /// </summary>
        /// <returns></returns>
        public override string ToJavaScript()
        {
            if ( Expression != null )
            {
                JsonExpression = Expression.ToJavaScript();
            }
            else
            {
                JsonExpression = FieldsToMatch.ToJson();
            }

            return JsonExpression;
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Initialize a new instance of Match
        /// </summary>
        public Match()
        {
            FieldsToMatch = new Dictionary<string, object>();
        }
        /// <summary>
        /// Initialize a new instance of Match
        /// </summary>
        /// <param name="FieldsToMatch">Fields to match against</param>
        public Match( Dictionary<string, object> FieldsToMatch )
        {
            this.FieldsToMatch = FieldsToMatch;
        }
        /// <summary>
        /// Initialize a new instance of Match
        /// </summary>
        /// <param name="Expression">Expression to be evalueted</param>
        public Match( BaseExpression Expression )
        {
            this.Expression = Expression;
            FieldsToMatch = new Dictionary<string, object>();
        }
        #endregion
    }
}
