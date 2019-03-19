using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueryBuilder.Mongo.Aggregation.Operators
{
    /// <summary>
    /// Represents the $replaceRoot operator
    /// </summary>
    public class ReplaceRoot : BaseOperator
    {
        #region Properties
        /// <summary>
        /// Path to the attribute containg the replacement document
        /// </summary>
        public string NewRoot { get; set; }
        #endregion

        #region Methods
        /// <summary>
        /// Generate a JavaScript Object representing this Operator
        /// </summary>
        /// <returns></returns>
        public override string ToJavaScript()
        {
            BsonDocument Doc = new BsonDocument( new List<BsonElement> {
                new BsonElement("$replaceRoot", new BsonDocument( new List<BsonElement> {
                    new BsonElement("newRoot", NewRoot)
                }))
            });

            return Doc.ToString();
        }
        #endregion
    }
}
