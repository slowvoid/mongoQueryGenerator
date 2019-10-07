using QueryBuilder.Javascript;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueryBuilder.Mongo.Expressions
{
    /// <summary>
    /// Provides base for logical expressions
    /// </summary>
    public abstract class BaseLogicalExpression
    {
        #region Methods
        /// <summary>
        /// Generates a JSCode representation of this instance
        /// </summary>
        /// <returns></returns>
        public abstract JSCode ToJSCode();

        /// <summary>
        /// Return the operator string name (MongoDB compatitle)
        /// </summary>
        /// <returns></returns>
        protected string _getOperatorString( LogicalOperator Operator )
        {
            switch ( Operator )
            {
                case LogicalOperator.AND:
                    return "$and";
                case LogicalOperator.EQUAL:
                    return "$eq";
                case LogicalOperator.GREATER_EQUAL_THAN:
                    return "$gte";
                case LogicalOperator.GREATER_THAN:
                    return "$gt";
                case LogicalOperator.IN:
                    return "$in";
                case LogicalOperator.LESS_EQUAL_THAN:
                    return "$lte";
                case LogicalOperator.LESS_THAN:
                    return "$lt";
                case LogicalOperator.NOT_EQUAL:
                    return "$neq";
                case LogicalOperator.NOT_IN:
                    return "$not"; // TODO: This should result in two operators { $not: { $in: [] } }
                case LogicalOperator.OR:
                    return "$or";
                case LogicalOperator.NOT:
                    return "$not";
                default:
                    throw new InvalidOperationException( "Logical operator cannot be null" );
            }
        }
        #endregion
    }
}
