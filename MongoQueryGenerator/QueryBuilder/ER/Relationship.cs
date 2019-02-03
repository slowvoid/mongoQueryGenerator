using QueryBuilder.Shared;
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
        /// <summary>
        /// Origin attribute (Left side)
        /// </summary>
        public DataAttribute SourceAttribute { get; set; }
        /// <summary>
        /// Destination attribute (Right side)
        /// </summary>
        public DataAttribute TargetAttribute { get; set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Initialize a new Relationship instance
        /// </summary>
        /// <param name="Name">Relationship name</param>
        public Relationship( string Name )
        {
            this.Name = Name;
            Relates = new List<Entity>();
            Attributes = new List<DataAttribute>();
        }
        #endregion
    }
}
