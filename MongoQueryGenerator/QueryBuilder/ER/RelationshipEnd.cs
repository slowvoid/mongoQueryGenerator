using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueryBuilder.ER
{
    /// <summary>
    /// Represents an relationship end
    /// Basically the entity that is connect through and the cardinality
    /// </summary>
    public class RelationshipEnd
    {
        #region Properties
        /// <summary>
        /// Target entity (Connected through the relationship)
        /// </summary>
        public Entity TargetEntity { get; set; }
        /// <summary>
        /// Cardinality
        /// </summary>
        public RelationshipCardinality Cardinality { get; set; }
        #endregion

        public string SummarizeToString()
        {
            return "RelationshipEnd[ "+TargetEntity.SummarizeToString()+" ]";
        }


        #region Constructor
        /// <summary>
        /// Initialize a new instance of RelationshipEnd
        /// </summary>
        /// <param name="TargetEntity"></param>
        /// <param name="Cardinality"></param>
        public RelationshipEnd( Entity TargetEntity )
        {
            this.TargetEntity = TargetEntity;
        }
        /// <summary>
        /// Initialize empty instance
        /// </summary>
        public RelationshipEnd()
        { }
        #endregion
    }
}
