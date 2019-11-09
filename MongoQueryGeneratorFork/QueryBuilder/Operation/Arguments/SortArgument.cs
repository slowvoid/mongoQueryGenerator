using QueryBuilder.Mongo.Aggregation.Operators;
using QueryBuilder.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueryBuilder.Operation.Arguments
{
    /// <summary>
    /// Argument for sorting stage
    /// </summary>
    public class SortArgument
    {
        #region Properties
        /// <summary>
        /// Entity
        /// </summary>
        public QueryableEntity Entity { get; set; }
        /// <summary>
        /// Attribute to sort
        /// </summary>
        public DataAttribute Attribute { get; set; }
        /// <summary>
        /// Sort option
        /// </summary>
        public MongoDBSort SortOption { get; set; }
        #endregion

        #region Constructor
        /// <summary>
        /// Initialize a new instance of SortArgument
        /// </summary>
        /// <param name="Entity"></param>
        /// <param name="Attribute"></param>
        /// <param name="SortOption"></param>
        public SortArgument( QueryableEntity Entity, DataAttribute Attribute, MongoDBSort SortOption )
        {
            this.Entity = Entity;
            this.Attribute = Attribute;
            this.SortOption = SortOption;
        }
        #endregion
    }
}
