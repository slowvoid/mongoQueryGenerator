using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QueryBuilder.ER;
using QueryBuilder.Mongo;
using QueryBuilder.Operation.Exceptions;

namespace QueryBuilder.Map
{
    /// <summary>
    /// Defines the model mapping structure
    /// </summary>
    public class ModelMapping : IModelMap
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

        #region Methods
        /// <summary>
        /// Get rule value for an attribute that belongs to the given entity
        /// </summary>
        /// <param name="NameOrAlias"></param>
        /// <param name="AttributeName"></param>
        /// <param name="IsMain"></param>
        /// <returns></returns>
        public string GetRuleValue( string NameOrAlias, string AttributeName, bool IsMain = false )
        {
            MapRule Rule = null;

            if ( IsMain )
            {
                Rule = Rules.First( R => R.Source.Name == NameOrAlias && IsMain );
            }
            else
            {
                Rule = Rules.First( R => R.Source.Name == NameOrAlias );
            }
            
            if ( Rule != null )
            {
                return Rule.Rules.First( R => R.Key == AttributeName ).Value;
            }
            else
            {
                throw new RuleNotFoundException( $"No map rules found for ERElement [{NameOrAlias}]" );
            }
        }
        /// <summary>
        /// Find rule between an erelement and mongodb collection
        /// </summary>
        /// <param name="Source"></param>
        /// <param name="Target"></param>
        /// <returns></returns>
        public MapRule FindRule( BaseERElement Source, MongoDBCollection Target )
        {
            return Rules.FirstOrDefault( R => R.Source.Name == Source.Name && R.Target.Name == Target.Name );
        }
        /// <summary>
        /// Find the main collection of an er element
        /// </summary>
        /// <param name="Source"></param>
        /// <returns></returns>
        public MongoDBCollection FindMainCollection( BaseERElement Source )
        {
            MapRule MainRule = FindMainRule( Source );

            if ( MainRule != null )
            {
                return MainRule.Target;
            }

            return null;
        }
        /// <summary>
        /// Find rules that target the given collection
        /// </summary>
        /// <param name="Target"></param>
        /// <returns></returns>
        public List<MapRule> FindRules( MongoDBCollection Target )
        {
            return Rules.Where( R => R.Target == Target ).ToList();
        }
        /// <summary>
        /// Find rules that have the given ERElement as source
        /// </summary>
        /// <param name="Source"></param>
        /// <returns></returns>
        public List<MapRule> FindRules( BaseERElement Source )
        {
            return Rules.Where( R => R.Source == Source ).ToList();
        }
        /// <summary>
        /// Find the main mapping for the given ERElement
        /// </summary>
        /// <param name="Source"></param>
        /// <returns></returns>
        public MapRule FindMainRule( BaseERElement Source )
        {
            return Rules.FirstOrDefault( R => R.Source.Name == Source.Name && R.IsMain );
        }
        /// <summary>
        /// Find the main mapping that targets the given collection
        /// </summary>
        /// <param name="Target"></param>
        /// <returns></returns>
        public MapRule FindMainRule( MongoDBCollection Target )
        {
            return Rules.FirstOrDefault( R => R.Target == Target && R.IsMain );
        }
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
