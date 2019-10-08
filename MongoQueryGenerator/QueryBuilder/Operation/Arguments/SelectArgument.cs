using QueryBuilder.Mongo.Expressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueryBuilder.Operation.Arguments
{
    /// <summary>
    /// Represents an argument used in the select stage
    /// </summary>
    public class SelectArgument
    {
        #region Properties
        /// <summary>
        /// Expression group to resolve
        /// </summary>
        public LogicalExpressionGroup ExpressionGroup { get; set; }
        #endregion

        #region Constructor
        /// <summary>
        /// Initialize a new instance of SelectArgument
        /// </summary>
        /// <param name="ExpressionGroup"></param>
        public SelectArgument( LogicalExpressionGroup ExpressionGroup )
        {
            this.ExpressionGroup = ExpressionGroup;
        }
        #endregion
    }
}
