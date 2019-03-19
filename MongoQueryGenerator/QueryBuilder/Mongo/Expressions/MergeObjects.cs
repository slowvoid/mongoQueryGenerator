using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueryBuilder.Mongo.Expressions
{
    /// <summary>
    /// Represents the $mergeObjects expression
    /// </summary>
    public class MergeObjects : BaseExpression
    {
        #region Properties
        /// <summary>
        /// Objects to merge
        /// </summary>
        public List<string> Objects { get; set; }
        #endregion

        #region Methods
        /// <summary>
        /// Generates a JavaScript object representing this expression
        /// </summary>
        /// <returns></returns>
        public override string ToJavaScript()
        {
            BsonDocument Doc = new BsonDocument( new List<BsonElement> {
                new BsonElement("$mergeObjects", string.Format("[{0}]", string.Join(",", Objects)))
            } );

            return Doc.ToString();
        }
        #endregion

        #region Constructor
        /// <summary>
        /// Initializes a new instance of MergeObjects class
        /// </summary>
        public MergeObjects()
        {
            Objects = new List<string>();
        }
        #endregion
    }
}
