using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using QueryBuilder.ER;
using QueryBuilder.Mongo;

namespace QueryBuilder.Map
{
    /// <summary>
    /// Defines a mapping rule between ER elements and MongoDB
    /// </summary>
    public class MapRule
    {
        #region Properties
        /// <summary>
        /// ER Element that act as the attribute source
        /// </summary>
        public BaseERElement Source { get; set; }
        /// <summary>
        /// Target MongoDB collection
        /// </summary>
        public Collection Target { get; set; }
        /// <summary>
        /// Mapping rules
        /// </summary>
        public Dictionary<string, string> Rules { get; set; }
        #endregion

        #region Methods
        /// <summary>
        /// Add a new rule
        /// </summary>
        /// <param name="Source">Source value</param>
        /// <param name="Target">Target value</param>
        public void AddRule( string Source, string Target )
        {
            Rules.Add( Source, Target );
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Initialize a new MapRule instance
        /// </summary>
        /// <param name="Source"></param>
        /// <param name="Target"></param>
        public MapRule( BaseERElement Source, Collection Target )
        {
            this.Source = Source;
            this.Target = Target;

            Rules = new Dictionary<string, string>();
        }
        /// <summary>
        /// Initialize a new MapRule instance
        /// </summary>
        /// <param name="Source"></param>
        /// <param name="Target"></param>
        /// <param name="Rules"></param>
        public MapRule( BaseERElement Source, Collection Target, Dictionary<string, string> Rules )
        {
            this.Source = Source;
            this.Target = Target;

            this.Rules = Rules;
        }
        #endregion
    }
}
