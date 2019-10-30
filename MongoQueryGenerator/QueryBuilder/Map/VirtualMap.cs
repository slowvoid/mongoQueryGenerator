using QueryBuilder.ER;
using QueryBuilder.Map.Exceptions;
using QueryBuilder.Operation.Exceptions;
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
    public class VirtualMap : IModelMap
    {
        #region Properties
        /// <summary>
        /// Rules
        /// </summary>
        public List<VirtualRule> Rules { get; set; }
        #endregion

        #region Methods
        /// <summary>
        /// Returns a List with all rule strings
        /// </summary>
        /// <returns></returns>
        public List<string> GetRulesAsStringList()
        {
            List<string> RuleStringList = new List<string>();

            foreach ( VirtualRule Rule in Rules )
            {
                foreach ( KeyValuePair<string, string> RulePair in Rule.Rules )
                {
                    RuleStringList.Add( RulePair.Value );
                }
            }

            return RuleStringList;
        } 
        /// <summary>
        /// Generate a string representation
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            foreach ( VirtualRule Rule in Rules )
            {
                sb.Append( Rule.ToString() );
            }

            return sb.ToString();
        }
        /// <summary>
        /// Get rule value for an attribute that belongs to the given entity
        /// </summary>
        /// <param name="NameOrAlias"></param>
        /// <param name="AttributeName"></param>
        /// <returns></returns>
        public string GetRuleValue( string NameOrAlias, string AttributeName )
        {
            VirtualRule Rule = Rules.First( R => R.SourceERElement.Name == NameOrAlias || R.Alias == NameOrAlias );

            if ( Rule != null )
            {
                return Rule.Rules.First( R => R.Key == AttributeName ).Value;
            }
            else
            {
                throw new RuleNotFoundException( $"No map rule found for ER Element [{NameOrAlias}]" );
            }
        }
        /// <summary>
        /// Generates a VirtualMap instance from a ModelMapping instance
        /// </summary>
        /// <param name="ModelMap"></param>
        /// <returns></returns>
        public static VirtualMap FromModelMap( ModelMapping ModelMap )
        {
            // Create list of virtual rules
            List<VirtualRule> VirtualRules = new List<VirtualRule>();

            // Iterate ModelMap rules
            foreach ( MapRule Rule in ModelMap.Rules )
            {
                VirtualRule ConvertedRule = new VirtualRule( Rule.Source );
                ConvertedRule.Rules = Rule.Rules;

                VirtualRules.Add( ConvertedRule );
            }

            return new VirtualMap( VirtualRules );
        }
        #endregion

        #region Constructor
        /// <summary>
        /// Initialize a new instance of VirtualMap
        /// </summary>
        /// <param name="Rules"></param>
        public VirtualMap( List<VirtualRule> Rules )
        {
            this.Rules = new List<VirtualRule>();

            foreach ( VirtualRule Rule in Rules )
            {
                // Check if the rule already exists in the list
                if ( this.Rules.FirstOrDefault( R => R.SourceERElement.Name == Rule.SourceERElement.Name && R.Alias == Rule.Alias  ) == null )
                {
                    this.Rules.Add( Rule );
                }
            }
        }
        #endregion
    }
}
