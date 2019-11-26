﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using QueryBuilder.Shared;

namespace QueryBuilder.ER
{
    /// <summary>
    /// Defines the base class for Entity Relation elements
    /// </summary>
    public abstract class BaseERElement
    {
        #region Properties
        /// <summary>
        /// Element name
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Alias
        /// </summary>
        public string Alias { get; set; }
        /// <summary>
        /// Attributes
        /// </summary>
        public List<DataAttribute> Attributes { get; set; }
        #endregion

        #region Methods
        /// <summary>
        /// Add a new attribute 
        /// </summary>
        /// <param name="Name">Attribute name</param>
        /// <param name="Identifier"></param>
        public void AddAttribute( string Name, bool Identifier = false )
        {
            Attributes.Add( new DataAttribute( Name, this, Identifier ) );
        }
        /// <summary>
        /// Add a list of string as attributes
        /// </summary>
        /// <param name="Names"></param>
        public void AddAttributes( params string[] Names )
        {
            foreach ( string Name in Names )
            {
                AddAttribute( Name );
            }
        }
        /// <summary>
        /// Find an attribute with the given name
        /// </summary>
        /// <param name="Name">Attribute name</param>
        /// <returns></returns>
        public DataAttribute GetAttribute( string Name )
        {
            return Attributes.Find( A => A.Name == Name );
        }
        /// <summary>
        /// Returns the alias and if it is null returns the name
        /// </summary>
        /// <returns></returns>
        public string GetAliasOrName()
        {
            return Alias ?? Name;
        }
        /// <summary>
        /// Returns the Identifier field
        /// </summary>
        /// <returns></returns>
        public DataAttribute GetIdentifier()
        {
            return Attributes.Find( A => A.IsIdentifier );
        }
        #endregion
    }
}
