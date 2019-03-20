using QueryBuilder.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueryBuilder.ER
{
    /// <summary>
    /// Available relationship cardinality
    /// </summary>
    public enum RelationshipCardinality
    {
        /// <summary>
        /// One to One relationship
        /// </summary>
        OneToOne,
        /// <summary>
        /// One to Many relationship
        /// </summary>
        OneToMany,
        /// <summary>
        /// Many to Many relationship
        /// </summary>
        ManyToMany
    }
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
        /// <summary>
        /// Relationship cardinality
        /// </summary>
        public RelationshipCardinality Cardinality { get; set; }
        /// <summary>
        /// Reference to source attribute (entity)
        /// Used when joining the relationship is mapped to a different collection
        /// Used on Many to Many relationships
        /// </summary>
        public DataAttribute RefToSourceAttribute { get; set; }
        /// <summary>
        /// Reference to target attribute (entity)
        /// Used when joining the relationship is mapped to a different collection
        /// Used on Many to Many relationships
        /// </summary>
        public DataAttribute RefToTargetAttribute { get; set; }
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
