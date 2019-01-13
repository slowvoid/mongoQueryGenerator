using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueryBuilder.Map
{
    /// <summary>
    /// Defines the model mapping structure
    /// </summary>
    public class ModelMapping
    {
        #region Properties
        /// <summary>
        /// Map name
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Mapping rules
        /// </summary>
        public List<MapRule> Rules { get; set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Initialize a new instance of ModelMapping class
        /// </summary>
        /// <param name="Name">Map name</param>
        public ModelMapping( string Name )
        {
            this.Name = Name;

            Rules = new List<MapRule>();
        }
        /// <summary>
        /// Initialize a new instance of ModelMapping class
        /// </summary>
        /// <param name="Name">Map name</param>
        /// <param name="Rules">Mapping rules</param>
        public ModelMapping( string Name, List<MapRule> Rules )
        {
            this.Name = Name;
            this.Rules = Rules;
        }
        #endregion
    }
}
