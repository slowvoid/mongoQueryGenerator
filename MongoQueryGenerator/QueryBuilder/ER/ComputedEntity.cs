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
        /// Source Entity
        /// </summary>
        public Entity SourceEntity { get; set; }
        /// <summary>
        /// Relationship
        /// </summary>
        public Relationship Relationship { get; set; }
        /// <summary>
        /// Joined entities (They can be Computed Entities)
        /// </summary>
        public List<Entity> TargetEntities { get; set; }
        #endregion

        #region Constructor
        /// <summary>
        /// Initialize a new ComputedEntity instance
        /// </summary>
        /// <param name="Name">Computed Entity name</param>
        /// <param name="Attributes">Attributes</param>
        public ComputedEntity( string Name, Entity SourceEntity, Relationship Relationship, List<Entity> TargetEntities )
        {
            this.Name = Name;
            this.SourceEntity = SourceEntity;
            this.Relationship = Relationship;
            this.TargetEntities = TargetEntities;

            // Attributes for a computed entity are composed from all attributes from the source entity + relationship and target entities
            Attributes.AddRange( SourceEntity.Attributes );
            DataAttribute R = new DataAttribute( $"data_{Relationship.Name}" );
            R.Children.AddRange( Relationship.Attributes );
            TargetEntities.ForEach( E => R.Children.AddRange( E.Attributes ) );
            Attributes.Add( R );
        }
        #endregion
    }
}
