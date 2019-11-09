using QueryBuilder.Map;
using QueryBuilder.Operation.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueryBuilder.Operation.Arguments
{
    /// <summary>
    /// Represents data from the FROM argument in the query string
    /// 
    /// Example:
    ///    QUERY: FROM [ENTITY] ...
    /// </summary>
    public class FromArgument
    {
        #region Properties
        /// <summary>
        /// Entity
        /// </summary>
        public QueryableEntity Entity { get; set; }
        /// <summary>
        /// Map Rules
        /// </summary>
        public ModelMapping MapRules { get; set; }
        #endregion

        #region Methods
        /// <summary>
        /// Retrieve the collection based on the map rules
        /// </summary>
        /// <returns></returns>
        public string GetCollectionName()
        {
            /* When an entity is set in the FROM part of the query
             * it must have a Main mapping rule
             */
            MapRule EntityRule = MapRules.Rules.FirstOrDefault( R => R.Source.Name == Entity.GetName() && R.IsMain );

            if ( EntityRule == null )
            {
                throw new ImpossibleOperationException( $"Entity {Entity.GetName()} doesn't have a MAIN mapping" );
            }

            return EntityRule.Target.Name;
        }
        #endregion

        #region Constructor
        /// <summary>
        /// Initialize a new instance of FromArgument class
        /// </summary>
        /// <param name="Entity"></param>
        /// <param name="MapRules"></param>
        public FromArgument( QueryableEntity Entity, ModelMapping MapRules )
        {
            this.Entity = Entity;
            this.MapRules = MapRules;
        }
        #endregion
    }
}
