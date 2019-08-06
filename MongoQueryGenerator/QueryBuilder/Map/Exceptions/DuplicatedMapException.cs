using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueryBuilder.Map.Exceptions
{
    /// <summary>
    /// Duplicated Map Exception
    /// Thrown when there is more than one virtual map for the same entity (same entity and same alias)
    /// </summary>
    public class DuplicatedMapException : Exception
    {
        /// <summary>
        /// Throw a new DuplicatedMapException
        /// </summary>
        /// <param name="EntityName"></param>
        public DuplicatedMapException( string EntityName ) : base( $"Duplicated virtual map for entity {EntityName}" )
        {           
        }
    }
}
