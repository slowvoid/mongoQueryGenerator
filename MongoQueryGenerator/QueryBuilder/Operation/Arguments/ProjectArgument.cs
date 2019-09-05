using QueryBuilder.Mongo.Expressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueryBuilder.Operation.Arguments
{
    /// <summary>
    /// Represents an argument to be passed to ProjectStage
    /// </summary>
    public class ProjectArgument
    {
        #region Properties
        /// <summary>
        /// Base Element
        /// </summary>
        public QueryableEntity Element { get; set; }
        /// <summary>
        /// Attributes and the expression to apply
        /// </summary>
        public Dictionary<string, ProjectExpression> Attributes { get; set; }
        #endregion

        #region Constructor
        /// <summary>
        /// Initialize a new ProjectArgument instance
        /// </summary>
        /// <param name="Element"></param>
        /// <param name="Attributes"></param>
        public ProjectArgument( QueryableEntity Element, Dictionary<string, ProjectExpression> Attributes )
        {
            this.Element = Element;
            this.Attributes = Attributes;
        }
        #endregion
    }
}
