using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueryBuilder.ER
{
    /// <summary>
    /// Represents a relation ship
    /// </summary>
    public class Relationship : BaseERElement
    {
        #region Properties
        /// <summary>
        /// Entities relate through this relationship
        /// </summary>
        public List<Entity> Relates { get; set; }
        #endregion
    }
}
