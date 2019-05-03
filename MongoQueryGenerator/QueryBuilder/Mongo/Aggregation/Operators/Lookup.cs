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
    /// Represents the Lookup Operator
    /// </summary>
    public class LookupOperator : BaseOperator
    {
        #region Properties
        /// <summary>
        /// Collection to lookup
        /// </summary>
        [BsonElement("from")]
        public string From { get; set; }
        /// <summary>
        /// Local field that contains a reference to the 'From' collection
        /// </summary>
        [BsonElement("localField")]
        [BsonIgnoreIfNull]
        public string LocalField { get; set; }
        /// <summary>
        /// Field in the <see cref="From"/> collection to match the value of <see cref="LocalField"/>
        /// </summary>
        [BsonElement("foreignField")]
        [BsonIgnoreIfNull]
        public string ForeignField { get; set; }
        /// <summary>
        /// Alias of the joined data
        /// </summary>
        [BsonElement("as")]
        public string As { get; set; }
        /// <summary>
        /// Pipeline to be executed on the <see cref="From"/> collection
        /// </summary>
        [BsonElement("pipeline")]
        [BsonIgnoreIfNull]
        public List<BaseOperator> Pipeline { get; set; }
        /// <summary>
        /// Variables to be accesible in the pipeline
        /// </summary>
        [BsonElement("let")]
        [BsonIgnoreIfNull]
        public Dictionary<string, string> Let { get; set; }
        #endregion

        #region Methods
        /// <summary>
        /// Generates a JavaScript representation of this operator
        /// </summary>
        /// <returns></returns>
        public override string ToJavaScript()
        {
            if ( Pipeline.Count > 0 )
            {
                ForeignField = null;
                LocalField = null;
            }
            else
            {
                Pipeline = null;
                Let = null;
            }

            return this.ToBsonDocument().ToString();
        }
        #endregion

        #region Constructor
        /// <summary>
        /// Initialize a new LookupOperator instance
        /// </summary>
        public LookupOperator()
        {
            Pipeline = new List<BaseOperator>();
            Let = new Dictionary<string, string>();
        }
        #endregion
    }
}
