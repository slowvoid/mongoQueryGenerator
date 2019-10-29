using MongoDB.Bson;
using QueryBuilder.Javascript;
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
        /// Generates a Javascript code object representing this instance
        /// </summary>
        /// <returns></returns>
        public override JSCode ToJSCode()
        {
            return new JSBoolean( Value );
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

            this.IsAddingOrForcingAFieldVisible = Value;
        }
        #endregion
    }
}
