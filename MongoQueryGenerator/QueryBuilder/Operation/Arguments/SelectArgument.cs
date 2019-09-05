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
        /// Entity in which the attributes belong to
        /// </summary>
        public QueryableEntity Element { get; set; }
        /// <summary>
        /// Attributes and expressions to apply to them
        /// </summary>
        public Dictionary<string, BaseExpression> Attributes { get; set; }
        #endregion

        #region Constructor
        /// <summary>
        /// Initialize a new instance of Select Argument class
        /// </summary>
        /// <param name="Element"></param>
        /// <param name="Attributes"></param>
        public SelectArgument(QueryableEntity Element, Dictionary<string, BaseExpression> Attributes )
        {
            this.Element = Element;
            this.Attributes = Attributes;
        }
        #endregion
    }
}
