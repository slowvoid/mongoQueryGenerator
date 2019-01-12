using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using QueryBuilder.Shared;

namespace QueryBuilder.ER
{
    /// <summary>
    /// Defines the base class for Entity Relation elements
    /// </summary>
    public abstract class BaseERElement
    {
        #region Properties
        /// <summary>
        /// Element name
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Attributes
        /// </summary>
        public List<DataAttribute> Attributes { get; set; }
        #endregion
    }
}
