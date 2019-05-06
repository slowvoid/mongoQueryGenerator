using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
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
        [BsonElement("path")]
        public string Field { get; set; }
        /// <summary>
        /// Sets wheter empty results should be preserved
        /// </summary>
        [BsonElement( "preserveNullAndEmptyArrays" )]
        [BsonDefaultValue(true)]
        public bool PreserveNullOrEmpty { get; set; }
        #endregion

        #region Methods
        public override string ToJavaScript()
        {
            BsonDocument Unwind = new BsonDocument( new List<BsonElement> {
                new BsonElement("$unwind", this.ToBsonDocument())
            } );

            return Unwind.ToString();
        }
        #endregion

        #region Constructor
        /// <summary>
        /// Initialize a new Unwind instance
        /// </summary>
        public Unwind(string Field, bool PreserveNullOrEmpty = true)
        {
            this.Field = $"${Field}";
            this.PreserveNullOrEmpty = PreserveNullOrEmpty;
        }
        #endregion
    }
}
