using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
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
    public class Expr : BaseExpression
    {
        #region Properties
        /// <summary>
        /// Expression to be interpreted
        /// </summary>
        [BsonIgnore]
        public BaseExpression Expression { get; set; }
        [BsonElement("$expr")]
        private string JsonExpression { get; set; }
        #endregion

        #region Override
        /// <summary>
        /// Generate a JavaScript representation of this instance
        /// </summary>
        /// <returns></returns>
        public override string ToJavaScript()
        {
            JsonExpression = Expression.ToJavaScript();

            return this.ToJson();
        }
        #endregion

        #region Constructor
        /// <summary>
        /// Initializes a new Expr instance
        /// </summary>
        /// <param name="Expression">Expressiono to be evalueted</param>
        public Expr( BaseExpression Expression )
        {
            this.Expression = Expression;
        }
        #endregion
    }
}
