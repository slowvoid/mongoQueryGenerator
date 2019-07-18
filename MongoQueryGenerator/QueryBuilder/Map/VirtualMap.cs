using QueryBuilder.ER;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueryBuilder.Map
{
    /// <summary>
    /// Represents a map between Entities and Computed Entities
    /// A computed entity is a real time representation of the MongoDB Pipeline in relation to
    /// the original ER Model
    /// </summary>
    public class VirtualMap
    {
        #region Properties
        /// <summary>
        /// Rules
        /// </summary>
        public List<VirtualRule> Rules { get; set; }
        #endregion

        #region Methods
        /// <summary>
        /// Add a new rule
        /// </summary>
        /// <param name="Element"></param>
        /// <param name="Rule"></param>
        public void AddRule(BaseERElement Element, string Rule)
        {
            Rules.Add( new VirtualRule( Element, Rule ) );
        }
        #endregion

        #region Constructor
        /// <summary>
        /// Initialize a new instance of VirtualMap
        /// </summary>
        public VirtualMap()
        {
            Rules = new List<VirtualRule>();
        }
        #endregion
    }
}
