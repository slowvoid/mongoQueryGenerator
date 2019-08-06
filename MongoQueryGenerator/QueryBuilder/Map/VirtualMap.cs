using QueryBuilder.Map.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueryBuilder.Map
{
    /// <summary>
    /// Virtual map between Entities and the output document
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
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            foreach ( VirtualRule Rule in Rules )
            {
                sb.Append( Rule.ToString() );
            }

            return sb.ToString();
        }
        #endregion

        #region Constructor
        /// <summary>
        /// Initialize a new instance of VirtualMap
        /// </summary>
        /// <param name="Rules"></param>
        public VirtualMap( List<VirtualRule> Rules )
        {
            // Check if there are rules for the same entity with the same alias
            var GroupedRulesByName = Rules.GroupBy( Rule => Rule.SourceERElement.Name );
            foreach ( var RuleGroup in GroupedRulesByName )
            {
                var GroupedRulesByAlias = RuleGroup.GroupBy( Rule => Rule.Alias );
                if ( GroupedRulesByAlias.Count() > 1 )
                {
                    throw new DuplicatedMapException( RuleGroup.Key );
                }
            }

            this.Rules = Rules;
        }
        #endregion
    }
}
