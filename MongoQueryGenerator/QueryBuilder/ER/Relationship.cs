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
        One,
        Many
    }
    /// <summary>
    /// Represents a relation ship
    /// </summary>
    public class Relationship : BaseERElement
    {
        #region Properties
        /// <summary>
        /// Relationship endpoints
        /// Entities connected through
        /// </summary>
        public List<RelationshipEnd> Ends { get; set; }
        #endregion

        #region Methods
        /// <summary>
        /// Add a new End to the relationship
        /// </summary>
        /// <param name="End"></param>
        public void AddRelationshipEnd( RelationshipEnd End )
        {
            Ends.Add( End );
        }

        /// <summary>
        /// Returns wheter the given entities are connected
        /// through this relationship
        /// </summary>
        /// <param name="Source">Source Entity</param>
        /// <param name="Target">Target Entity</param>
        /// <returns></returns>
        public bool AreRelated( Entity Source, Entity Target )
        {
            bool FoundSourceEntity = Ends.First( E => E.TargetEntity.Name == Source.Name ) != null ? true : false;
            bool FoundTargetEntity = Ends.First( E => E.TargetEntity.Name == Target.Name ) != null ? true : false;

            return FoundSourceEntity && FoundTargetEntity;
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Initialize a new Relationship instance
        /// </summary>
        /// <param name="Name">Relationship name</param>
        public Relationship( string Name )
        {
            this.Name = Name;
            Attributes = new List<DataAttribute>();
            Ends = new List<RelationshipEnd>();
        }
        #endregion
    }
}
