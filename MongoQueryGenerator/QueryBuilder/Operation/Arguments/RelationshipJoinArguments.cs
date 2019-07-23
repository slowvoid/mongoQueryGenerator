using QueryBuilder.ER;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueryBuilder.Operation.Arguments
{
    /// <summary>
    /// Represents the arguments to be used in a Relationship Join operator
    /// </summary>
    public class RelationshipJoinArguments
    {
        #region Properties
        /// <summary>
        /// Join entities through this relationship
        /// </summary>
        public Relationship Relationship { get; set; }
        /// <summary>
        /// Target entities
        /// </summary>
        public List<Entity> Targets { get; set; }
        #endregion

        #region Constructor
        /// <summary>
        /// Initialize a new RelationshipJoinArguments instance
        /// </summary>
        /// <param name="Relationship"></param>
        /// <param name="Targets"></param>
        public RelationshipJoinArguments( Relationship Relationship, List<Entity> Targets )
        {
            this.Relationship = Relationship;
            this.Targets = Targets;
        }
        #endregion
    }
}
