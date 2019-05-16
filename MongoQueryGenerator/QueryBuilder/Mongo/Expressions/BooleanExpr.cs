using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueryBuilder.Mongo.Expressions
{
    /// <summary>
    /// Expression that represents a boolean value
    /// </summary>
    public class BooleanExpr : ProjectExpression
    {
        #region Properties
        /// <summary>
        /// Value
        /// </summary>
        public bool Value { get; set; }
        #endregion

        #region Methods
        /// <summary>
        /// Return a JavaScript compatible representation
        /// </summary>
        /// <returns></returns>
        public override BsonValue ToJavaScript()
        {
            return new BsonBoolean( Value ); 
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Initialize a new instance of BooleanExpr
        /// </summary>
        /// <param name="Value"></param>
        public BooleanExpr( bool Value )
        {
            this.Value = Value;
        }
        #endregion
    }
}
