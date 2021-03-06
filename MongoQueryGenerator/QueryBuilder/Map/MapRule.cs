﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using QueryBuilder.ER;
using QueryBuilder.Mongo;
using QueryBuilder.Shared;

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
        /// <summary>
        /// Retrieve the rule for the given Attribute
        /// </summary>
        /// <param name="Attribute"></param>
        /// <returns></returns>
        public string GetRuleValueForAttribute( DataAttribute Attribute )
        {
            return Rules.FirstOrDefault( R => R.Key == Attribute.Name ).Value;
        }
        /// <summary>
        /// Returns whether this instance has a rule for the given attribute
        /// </summary>
        /// <param name="Attribute"></param>
        /// <returns></returns>
        public bool HasRuleForAttribute( DataAttribute Attribute )
        {
            return Rules.ContainsKey( Attribute.Name );
        }
        /// <summary>
        /// Returns if the source entity is mapped to a multivalued attribute (aka embedded)
        /// </summary>
        /// <returns></returns>
        public bool BelongsToMultivaluedAttribute()
        {
            // All attributes in a embedded setting are mapped to the same root
            // so fetch the first rule and use it
            string RuleValue = Rules.First().Value;

            string[] AttributeMap = RuleValue.Split( new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries );

            if ( AttributeMap.Length > 1 )
            {
                if ( AttributeMap[0].Contains("_multivalued_") )
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// Returns the name of the root attribute in which data is mapped to
        /// </summary>
        /// <returns></returns>
        public string GetRootAttribute()
        {
            // All attributes in a embedded setting are mapped to the same root
            // so fetch the first rule and use it
            string RuleValue = Rules.First().Value;

            string[] AttributeMap = RuleValue.Split( new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries );

            if ( AttributeMap.Length > 1 )
            {
                return AttributeMap[ 0 ];
            }
            else
            {
                return null;
            }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Initialize a new MapRule instance
        /// </summary>
        /// <param name="Source"></param>
        /// <param name="Target"></param>
        public MapRule( BaseERElement Source, MongoDBCollection Target )
        {
            this.Source = Source;
            this.Target = Target;

            IsMain = true;

            Rules = new Dictionary<string, string>();
        }
        /// <summary>
        /// Initialize a new MapRule instance
        /// </summary>
        /// <param name="Source"></param>
        /// <param name="Target"></param>
        /// <param name="Rules"></param>
        public MapRule( BaseERElement Source, MongoDBCollection Target, bool IsMain = true )
        {
            this.Source = Source;
            this.Target = Target;

            this.Rules = new Dictionary<string, string>();

            this.IsMain = IsMain;
        }
        #endregion
    }
}
