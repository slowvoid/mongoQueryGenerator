using QueryBuilder.Operation.Arguments;
using QueryBuilder.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueryBuilder.ER
{
    /// <summary>
    /// Represents a Computed Entity
    /// 
    /// A Computed Entity is built from one Entity joined with N others through a relationship
    /// </summary>
    public class ComputedEntity : Entity
    {
        #region Properties
        /// <summary>
        /// Source entity
        /// </summary>
        public QueryableEntity SourceEntity { get; set; }
        /// <summary>
        /// Relationship
        /// </summary>
        public Relationship Relationship { get; set; }
        /// <summary>
        /// Joined entities (They can be Computed Entities)
        /// </summary>
        public List<QueryableEntity> TargetEntities { get; set; }

        public string RelationshipAlias { get; set; }
        #endregion

        public override string SummarizeToString()
        {
            string Ret = "ComputedEntity[ "+SourceEntity.SummarizeToString()+" x { ";
            TargetEntities.ForEach(te => Ret += te.SummarizeToString()+" ");
            Ret += "}";
            return Ret;
        }


        #region Constructor
        /// <summary>
        /// Initialize a new ComputedEntity instance
        /// </summary>
        /// <param name="Name">Computed Entity name</param>
        /// <param name="Attributes">Attributes</param>
        public ComputedEntity( string Name, QueryableEntity SourceEntity, Relationship Relationship, List<QueryableEntity> TargetEntities )
        {
            this.Name = Name;
            this.SourceEntity = SourceEntity;
            this.Relationship = Relationship;
            this.TargetEntities = TargetEntities;
        }

        public ComputedEntity( string Name, QueryableEntity SourceEntity, Relationship Relationship, string RelationshipAlias, List<QueryableEntity> TargetEntities )
        {
            this.Name = Name;
            this.SourceEntity = SourceEntity;
            this.Relationship = Relationship;
            this.TargetEntities = TargetEntities;
            this.RelationshipAlias = RelationshipAlias;
        }
        #endregion
    }
}
