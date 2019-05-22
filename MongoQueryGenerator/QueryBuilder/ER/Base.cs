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
            Attributes.Add( new DataAttribute( Name, Identifier ) );
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
        #endregion
    }
}
