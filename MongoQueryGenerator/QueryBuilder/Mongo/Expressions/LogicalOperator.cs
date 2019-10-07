using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueryBuilder.Mongo.Expressions
{
    /// <summary>
    /// Represents a logical operator
    /// </summary>
    public enum LogicalOperator
    {
        /// <summary>
        /// AND operator $and: [...]
        /// </summary>
        AND,
        /// <summary>
        /// OR operator $or: [...]
        /// </summary>
        OR,
        /// <summary>
        /// EQUAL operator $eq: [...]
        /// </summary>
        EQUAL,
        /// <summary>
        /// NOT EQUAL operator $neq: [...]
        /// </summary>
        NOT_EQUAL,
        /// <summary>
        /// LESS THAN operator $lt: [...]
        /// </summary>
        LESS_THAN,
        /// <summary>
        /// LESS OR EQUAL THAN operator $lte: [...]
        /// </summary>
        LESS_EQUAL_THAN,
        /// <summary>
        /// GREATER THAN operator $gt: [...]
        /// </summary>
        GREATER_THAN,
        /// <summary>
        /// GREATER OR EQUAL THAN operator $gte: [...]
        /// </summary>
        GREATER_EQUAL_THAN,
        /// <summary>
        /// IN ARRAY operator $in: [a,[...]]
        /// </summary>
        IN,
        /// <summary>
        /// NOT IN ARRAY operator $not: { $in: [a,[...]] }
        /// </summary>
        NOT_IN,
        /// <summary>
        /// NOT operator (negation) $not: {}
        /// </summary>
        NOT
    }
}
