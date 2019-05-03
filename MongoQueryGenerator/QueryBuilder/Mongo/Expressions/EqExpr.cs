using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
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
        /// Values to be matched
        /// </summary>
        [BsonElement("$eq")]
        public List<object> Values { get; set; }
        #endregion

        #region Override
        /// <summary>
        /// Generates a JavaScript representation of this instance
        /// </summary>
        /// <returns></returns>
        public override string ToJavaScript()
        {
            return this.ToJson();
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
            Values = new List<object>();
            Values.AddRange( new object[] { Field, Value } );
        }
        #endregion
    }
}
