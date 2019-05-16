using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace QueryBuilder.Mongo.Expressions
{
    /// <summary>
    /// Represents a $map operation
    /// </summary>
    public class MapExpr : ProjectExpression
    {
        #region Properties
        /// <summary>
        /// Input field
        /// </summary>
        [BsonElement("input")]
        public string Input { get; set; }
        /// <summary>
        /// Alias to access each memeber of the array
        /// </summary>
        [BsonElement("as")]
        public string As { get; set; } 
        /// <summary>
        /// Attribute map
        /// </summary>
        [BsonElement("in")]
        public Dictionary<string, string> In { get; set; }
        #endregion

        #region Methods
        /// <summary>
        /// Returns a JavaScript compatible representation
        /// </summary>
        /// <returns></returns>
        public override BsonValue ToJavaScript()
        {
            return new BsonDocument( new BsonElement( "$map", this.ToBsonDocument() ) );
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of MapExpr
        /// </summary>
        /// <param name="Input"></param>
        /// <param name="As"></param>
        /// <param name="In"></param>
        public MapExpr(string Input, string As, Dictionary<string, string> In)
        {
            this.Input = Input;
            this.As = As;
            this.In = In;
        }
        #endregion
    }
}
