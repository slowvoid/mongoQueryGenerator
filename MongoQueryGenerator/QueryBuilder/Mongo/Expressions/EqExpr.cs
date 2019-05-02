using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueryBuilder.Mongo.Expressions
{
    /// <summary>
    /// Represents an $eq expression
    /// </summary>
    public class EqExpr : BaseExpression
    {
        #region Properties
        /// <summary>
        /// Field or value source to compare
        /// </summary>
        public string Field { get; set; }
        /// <summary>
        /// Value to match
        /// </summary>
        public object Value { get; set; }
        #endregion

        #region Override
        /// <summary>
        /// Generates a JavaScript representation of this instance
        /// </summary>
        /// <returns></returns>
        public override string ToJavaScript()
        {
            BsonDocument EqDoc = new BsonDocument( new List<BsonElement>
            {
                new BsonElement("$eq", $"[${Field}, {Value}]")
            } );

            return EqDoc.ToString();
        }
        /// <summary>
        /// Generates a BsonElement based on this instance data
        /// </summary>
        /// <returns></returns>
        public override BsonElement ToBsonElement()
        {
            return new BsonElement( "$eq", $"[${Field}, {Value}]" );
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Initialize a new EqExpr instance
        /// </summary>
        /// <param name="Field">Field to compare</param>
        /// <param name="Value">Value</param>
        public EqExpr(string Field, object Value)
        {
            this.Field = Field;
            this.Value = Value;
        }
        #endregion
    }
}
