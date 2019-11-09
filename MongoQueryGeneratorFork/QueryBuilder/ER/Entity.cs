using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using QueryBuilder.Shared;

namespace QueryBuilder.ER
{
    /// <summary>
    /// ER entity
    /// </summary>
    public class Entity : BaseERElement
    {
        #region Constructors
        /// <summary>
        /// Initialize a new Entity instance
        /// </summary>
        public Entity()
        {
            Attributes = new List<DataAttribute>();
        }
        /// <summary>
        /// Initializes a new Entity instance
        /// </summary>
        /// <param name="Name">Entity Name</param>
        public Entity( string Name )
        {
            this.Name = Name;
            Attributes = new List<DataAttribute>();
        }
        #endregion
    }
}
