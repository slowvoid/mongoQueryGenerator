using QueryBuilder.Mongo.Expressions;
using QueryBuilder.Shared;
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
        /// Attribute name
        /// </summary>
        public DataAttribute Attribute { get; set; }
        /// <summary>
        /// Attribute parent
        /// </summary>
        public QueryableEntity ParentEntity { get; set; }
        /// <summary>
        /// Expression to apply to the attribute
        /// </summary>
        public ProjectExpression Expression { get; set; }
        #endregion

        #region Constructor
        /// <summary>
        /// Initialize a new instance of ProjectArgument
        /// </summary>
        /// <param name="Attribute">Attribute name</param>
        /// <param name="ParentEntity">Entity that owns attribute</param>
        /// <param name="Expression">Expression to apply to the attribute</param>
        public ProjectArgument(DataAttribute Attribute, QueryableEntity ParentEntity, ProjectExpression Expression )
        {
            this.Attribute = Attribute;
            this.ParentEntity = ParentEntity;
            this.Expression = Expression;
        }
        #endregion
    }
}
