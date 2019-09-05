using QueryBuilder.ER;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueryBuilder.Operation.Arguments
{
    /// <summary>
    /// Represents an Entity in a queryable context
    /// </summary>
    public class QueryableEntity
    {
        #region Properties
        /// <summary>
        /// Entity alias
        /// </summary>
        public string Alias { get; set; }
        /// <summary>
        /// Entity to be joined
        /// </summary>
        public BaseERElement Element { get; set; }
        #endregion

        #region Constructor
        /// <summary>
        /// Initialize a new instance of JoinableEntity
        /// </summary>
        /// <param name="Alias"></param>
        /// <param name="Element"></param>
        public QueryableEntity( BaseERElement Element, string Alias = null )
        {
            this.Element = Element;
            this.Alias = Alias;
        }
        #endregion
    }
}
