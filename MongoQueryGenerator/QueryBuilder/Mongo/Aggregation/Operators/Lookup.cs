using MongoDB.Bson;
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
        public string From { get; set; }
        /// <summary>
        /// Local field that contains a reference to the 'From' collection
        /// </summary>
        public string LocalField { get; set; }
        /// <summary>
        /// Field in the <see cref="From"/> collection to match the value of <see cref="LocalField"/>
        /// </summary>
        public string ForeignField { get; set; }
        /// <summary>
        /// Alias of the joined data
        /// </summary>
        public string As { get; set; }
        /// <summary>
        /// Pipeline to be executed on the <see cref="From"/> collection
        /// </summary>
        public List<BaseOperator> Pipeline { get; set; }
        #endregion

        #region Methods
        /// <summary>
        /// Generates a JavaScript representation of this operator
        /// </summary>
        /// <returns></returns>
        public override string ToJavaScript()
        {
            // No pipeline support yet

            BsonDocument opDoc = new BsonDocument( new List<BsonElement> {
                new BsonElement("$lookup", new BsonDocument( new List<BsonElement> {
                    new BsonElement("from", From),
                    new BsonElement("localField", LocalField),
                    new BsonElement("foreignField", ForeignField),
                    new BsonElement("as", As)
                }))                
            });

            return opDoc.ToString();
        }
        #endregion
    }
}
