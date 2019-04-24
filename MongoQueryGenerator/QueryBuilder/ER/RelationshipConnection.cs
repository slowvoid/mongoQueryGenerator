using QueryBuilder.ER;
using QueryBuilder.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueryBuilder.ER
{
    /// <summary>
    /// Represents a connection between entities
    /// through a relationship
    /// </summary>
    public class RelationshipConnection
    {
        #region Properties
        /// <summary>
        /// Source Entity
        /// </summary>
        public Entity SourceEntity { get; set; }
        /// <summary>
        /// Attribute in source entity used as key
        /// </summary>
        public DataAttribute SourceAttribute { get; set; }
        /// <summary>
        /// Attribute in the relationship that references the Source Attribute
        /// </summary>
        public DataAttribute RefSourceAtrribute { get; set; }
        /// <summary>
        /// Target Entity
        /// </summary>
        public Entity TargetEntity { get; set; }
        /// <summary>
        /// Attribute in target entity used as key
        /// </summary>
        public DataAttribute TargetAttribute { get; set; }
        /// <summary>
        /// Attribute in the relationship that references the Target Attribute
        /// </summary>
        public DataAttribute RefTargetAttribute { get; set; }
        /// <summary>
        /// Relationship Cardinality
        /// </summary>
        public RelationshipCardinality Cardinality { get; set; }
        #endregion

        #region Contructores
        /// <summary>
        /// Initialize a new RelationshipConnection instance
        /// </summary>
        /// <param name="SourceEntity"></param>
        /// <param name="SourceAttribute"></param>
        /// <param name="TargetEntity"></param>
        /// <param name="TargetAttribute"></param>
        public RelationshipConnection(Entity SourceEntity,
                                      DataAttribute SourceAttribute,
                                      Entity TargetEntity,
                                      DataAttribute TargetAttribute,
                                      RelationshipCardinality Cardinality)
        {
            this.SourceEntity = SourceEntity;
            this.SourceAttribute = SourceAttribute;
            this.TargetEntity = TargetEntity;
            this.TargetAttribute = TargetAttribute;
            this.Cardinality = Cardinality;
        }
        /// <summary>
        /// Initialize a new RelationshipConnection instance
        /// </summary>
        /// <param name="SourceEntity"></param>
        /// <param name="SourceAttribute"></param>
        /// <param name="RefSourceAttribute"></param>
        /// <param name="TargetEntity"></param>
        /// <param name="TargetAttribute"></param>
        /// <param name="RefTargetAttribute"></param>
        public RelationshipConnection(Entity SourceEntity,
                                      DataAttribute SourceAttribute,
                                      DataAttribute RefSourceAtrribute,
                                      Entity TargetEntity,
                                      DataAttribute TargetAttribute,
                                      DataAttribute RefTargetAttribute,
                                      RelationshipCardinality Cardinality)
        {
            this.SourceEntity = SourceEntity;
            this.SourceAttribute = SourceAttribute;
            this.RefSourceAtrribute = RefSourceAtrribute;
            this.TargetEntity = TargetEntity;
            this.TargetAttribute = TargetAttribute;
            this.RefTargetAttribute = RefTargetAttribute;
            this.Cardinality = Cardinality;
        }
        #endregion
    }
}
