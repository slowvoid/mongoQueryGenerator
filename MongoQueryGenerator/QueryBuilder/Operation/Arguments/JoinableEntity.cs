using QueryBuilder.ER;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueryBuilder.Operation.Arguments
{
    /// <summary>
    /// Represents an Entity in a joinable context
    /// </summary>
    public class JoinableEntity
    {
        #region Properties
        /// <summary>
        /// Entity alias
        /// </summary>
        public string Alias { get; set; }
        /// <summary>
        /// Entity to be joined
        /// </summary>
        public BaseERElement JoinedElement { get; set; }
        #endregion

        #region Constructor
        /// <summary>
        /// Initialize a new instance of JoinableEntity
        /// </summary>
        /// <param name="Alias"></param>
        /// <param name="JoinedElement"></param>
        public JoinableEntity( BaseERElement JoinedElement, string Alias = null )
        {
            this.JoinedElement = JoinedElement;
            this.Alias = Alias;
        }
        #endregion
    }
}
