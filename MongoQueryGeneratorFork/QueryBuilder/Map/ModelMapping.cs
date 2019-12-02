using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QueryBuilder.ER;
using QueryBuilder.Mongo;
using QueryBuilder.Operation.Exceptions;
using QueryBuilder.Shared;

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
        /// <returns></returns>
        public string GetRuleValue( string NameOrAlias, string AttributeName )
        {
            MapRule Rule = Rules.First( R => R.Source.Name == NameOrAlias );
            
            if ( Rule != null )
            {
                return Rule.Rules.First( R => R.Key == AttributeName ).Value;
            }
            else
            {
                throw new RuleNotFoundException( $"No map rules found for ERElement [{NameOrAlias}]" );
            }
        }

        public List<MapRule> FindRules( MongoDBCollection target )
        {
            List<MapRule> ret = new List<MapRule>();
            foreach(var r in Rules)
            {
                if(r.Target == target)
                {
                    ret.Add(r);
                }
            }
            return ret;
        }

        public MongoDBCollection FindMainCollection( BaseERElement source )
        {
            foreach(var r in Rules)
            {
                if(r.Source == source && r.IsMain)
                {
                    return r.Target;
                }
            }
             throw new RuleNotFoundException( $"No main collection found for Source={source.Name}" );
        }
        // public MapRule FindRule( BaseERElement source, MongoDBCollection target )
        // {
        //     foreach(var r in Rules)
        //     {
        //         if(r.Source == source && r.Target == target)
        //         {
        //             return r;
        //         }
        //     }
        //     throw new RuleNotFoundException( $"No map rule found for Source={source.Name} and Target={target.Name}" );
        // }

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
