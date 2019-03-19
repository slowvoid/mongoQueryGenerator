using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueryBuilder.Mongo.Aggregation.Operators
{
    /// <summary>
    /// Represents the $unwind operator
    /// </summary>
    public class Unwind : BaseOperator
    {
        #region Properties
        /// <summary>
        /// Path to field that should be unwinded
        /// </summary>
        public string Field { get; set; }
        #endregion

        #region Methods
        public override string ToJavaScript()
        {
            BsonDocument Unwind = new BsonDocument( new List<BsonElement> {
                new BsonElement("$unwind", string.Format("${0}", Field))
            } );

            return Unwind.ToString();
        }
        #endregion
    }
}
