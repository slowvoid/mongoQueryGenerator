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
        public MongoDBCollection Target { get; set; }
        /// <summary>
        /// Mapping rules
        /// </summary>
        public Dictionary<string, string> Rules { get; set; }
        /// <summary>
        /// Is this the main mapping for the Source entity?
        /// </summary>
        public bool IsMain { get; set; }
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
        // Commented to make sure "Main" is always set properly
        // public MapRule( BaseERElement Source, MongoDBCollection Target )
        // {
        //     this.Source = Source;
        //     this.Target = Target;

        //     IsMain = true;

        //     Rules = new Dictionary<string, string>();
        // }
        /// <summary>
        /// Initialize a new MapRule instance
        /// </summary>
        /// <param name="Source"></param>
        /// <param name="Target"></param>
        /// <param name="IsMain"></param>
        public MapRule( BaseERElement Source, MongoDBCollection Target, bool IsMain )
        {
            this.Source = Source;
            this.Target = Target;
            this.IsMain = IsMain;

            this.Rules = new Dictionary<string, string>();

        }
        #endregion
    }
}
