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
        /// List of entities and how they are related through this relationship
        /// </summary>
        public List<RelationshipConnection> Relations { get; set; }
        /// <summary>
        /// Relationship cardinality
        /// </summary>
        public RelationshipCardinality Cardinality { get; set; }
        #endregion

        #region Methods
        /// <summary>
        /// Checks if the source and target entities are related through this relationship
        /// </summary>
        /// <param name="Source">Source Entity</param>
        /// <param name="Target">Target Entity</param>
        /// <returns></returns>
        public bool HasRelation( Entity Source, Entity Target )
        {
            return Relations.Exists(R => R.SourceEntity == Source && R.TargetEntity == Target);
        }
        /// <summary>
        /// Returns the relation item that describes the connection between Source and Target entities
        /// </summary>
        /// <param name="Source">Source Entity</param>
        /// <param name="Target">Target Entity</param>
        /// <returns></returns>
        public RelationshipConnection GetRelation( Entity Source, Entity Target )
        {
            return Relations.Find(R => R.SourceEntity == Source && R.TargetEntity == Target);
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Initialize a new Relationship instance
        /// </summary>
        /// <param name="Name">Relationship name</param>
        public Relationship( string Name, RelationshipCardinality Cardinality )
        {
            this.Name = Name;
            Attributes = new List<DataAttribute>();
            Relations = new List<RelationshipConnection>();
            this.Cardinality = Cardinality;
        }
        #endregion
    }
}
