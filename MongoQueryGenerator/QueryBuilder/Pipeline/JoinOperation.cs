using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using QueryBuilder.ER;

namespace QueryBuilder.Pipeline
{
    /// <summary>
    /// Represents a JOIN operation
    /// </summary>
    public class JoinOperation : Operation
    {
        #region Properties
        /// <summary>
        /// Relationship startpoint
        /// </summary>
        public Entity SourceEntity { get; set; }
        /// <summary>
        /// Relationship endpoint
        /// </summary>
        public Entity TargetEntity { get; set; }
        /// <summary>
        /// Relationship connecting both entities
        /// </summary>
        public Relationship Relationship { get; set; }
        /// <summary>
        /// Attribute in the source entity responsible for this relationship
        /// </summary>
        public string SourceAttribute { get; set; }
        /// <summary>
        /// Attribute in the target entity responsible for this relationship
        /// </summary>
        public string TargetAttribute { get; set; }
        #endregion

        #region Methods
        #endregion

        #region Constructors
        #endregion
    }
}
