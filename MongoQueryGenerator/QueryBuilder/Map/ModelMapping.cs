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

            // Check if within the Rules there are multiple main mappings for the same entity
            var GroupedRules = Rules.GroupBy( MP => MP.Source );
            foreach ( var GRules in GroupedRules )
            {
                if ( GRules.Count( R => R.IsMain ) > 1 )
                {
                    throw new InvalidOperationException( "Cannot set multiple main mappings for the same entity" );
                }
            }
            
            this.Rules = Rules;
        }
        #endregion
    }
}
