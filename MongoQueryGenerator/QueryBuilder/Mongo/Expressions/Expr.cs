using MongoDB.Bson;
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
        public BaseExpression Expression { get; set; }
        #endregion

        #region Override
        /// <summary>
        /// Generate a JavaScript representation of this instance
        /// </summary>
        /// <returns></returns>
        public override string ToJavaScript()
        {
            BsonDocument ExprDoc = new BsonDocument( new BsonElement( "$expr", Expression.ToBsonElement() );

            return ExprDoc.ToString();
        }

        public override BsonElement ToBsonElement()
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
