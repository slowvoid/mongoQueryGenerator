using QueryBuilder.Mongo.Expressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueryBuilder.Mongo.Aggregation.Operators
{
    /// <summary>
    /// Represents the Match aggregation stage
    /// </summary>
    public class Match : BaseOperator
    {
        #region Properties
        /// <summary>
        /// Expression to execute
        /// </summary>
        public BaseExpression Expression { get; set; }
        /// <summary>
        /// Fields to match against
        /// </summary>
        public Dictionary<string, object> FieldsToMatch { get; set; }
        #endregion

        #region Overrides
        /// <summary>
        /// Generates a JavaScript compatible version of this instance
        /// </summary>
        /// <returns></returns>
        public override string ToJavaScript()
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Initialize a new instance of Match
        /// </summary>
        public Match()
        {
            FieldsToMatch = new Dictionary<string, object>();
        }
        #endregion
    }
}
