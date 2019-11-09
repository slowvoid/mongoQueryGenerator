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
        /// Cardinality = One
        /// </summary>
        One,
        /// <summary>
        /// Cardinality = Many
        /// </summary>
        Many,
        OneToMany, // please erase this after fixing the model!
        OneToOne, // please erase this after fixing the model!
        ManyToMany // please erase this after fixing the model!
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
        public List<RelationshipConnection> Relations
        {
            get
            {
                throw new NotImplementedException("Um Relationship não deveria ter connections FixME");
            }
        }

        public List<RelationshipEnd> Ends { get; set; }

        public RelationshipCardinality Cardinality
        {
            get
            {
                throw new NotImplementedException("Um Relationship não deveria ter cardinalidade FixME");
            }
        }


        #endregion

        #region Methods
        /// <summary>
        /// Checks if the source and target entities are related through this relationship
        /// </summary>
        /// <param name="Source">Source Entity</param>
        /// <param name="Target">Target Entity</param>
        /// <returns></returns>
        public bool HasRelation(Entity Source, Entity Target)
        {
            throw new NotImplementedException("Um Relationship não deveria ter relations FixME");
            return Relations.Exists(R => R.SourceEntity == Source && R.TargetEntity == Target);
        }
        /// <summary>
        /// Returns the relation item that describes the connection between Source and Target entities
        /// </summary>
        /// <param name="Source">Source Entity</param>
        /// <param name="Target">Target Entity</param>
        /// <returns></returns>
        public RelationshipConnection GetRelation(Entity Source, Entity Target)
        {
            throw new NotImplementedException("Um Relationship não deveria ter relations FixME");
            return Relations.Find(R => R.SourceEntity == Source && R.TargetEntity == Target);
        }
        /// <summary>
        /// Add relation
        /// </summary>
        /// <param name="End"></param>
        public void AddRelationshipEnd(RelationshipEnd End)
        {
            Ends.Add(End);
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Initialize a new Relationship instance
        /// </summary>
        /// <param name="Name">Relationship name</param>
        public Relationship(string Name)
        {
            this.Name = Name;
            Attributes = new List<DataAttribute>();
            Ends = new List<RelationshipEnd>();
        }
        #endregion
    }
}
