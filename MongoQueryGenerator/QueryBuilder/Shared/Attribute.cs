﻿using QueryBuilder.Shared.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueryBuilder.Shared
{
    /// <summary>
    /// Represents a data attribute
    /// </summary>
    public class DataAttribute
    {
        #region Properties
        /// <summary>
        /// Type of data stored by this attribute
        /// 
        /// See: <see cref="DataType"/>
        /// </summary>
        public DataType OfType { get; set; }
        /// <summary>
        /// Gets wheter this attribute is multivalued. (Array like)
        /// </summary>
        public bool IsMultiValued
        {
            get
            {
                return OfType == DataType.ARRAY;
            }
        }
        /// <summary>
        /// Attribute name
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Child attributes
        /// </summary>
        public List<DataAttribute> Children { get; set; }
        #endregion

        #region Functions
        /// <summary>
        /// Find an attribute by name
        /// </summary>
        /// <param name="Name">Name to search for</param>
        /// <returns>Returns an attribute</returns>
        public DataAttribute FindByName( string Name )
        {
            // First search at level 0
            DataAttribute FoundAttribute = Children.Find( Attr => Attr.Name == Name );

            if ( FoundAttribute is DataAttribute )
            {
                return FoundAttribute;                 
            }

            // Search through each attribute children
            foreach ( DataAttribute Attribute in Children )
            {
                FoundAttribute = Attribute.FindByName( Name );

                if ( FoundAttribute is DataAttribute )
                {
                    return FoundAttribute;
                }
            }

            // If reaches this far, it means that the attribute wasn't found
            throw new AttributeNotFoundException( string.Format( "Couldn't find attribute [{0}] as a child attribute of [{1}]", Name, this.Name ) );
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new DataAttribute instance
        /// </summary>
        /// <param name="Name">Attribute name</param>
        public DataAttribute( string Name )
        {
            this.Name = Name;
            Children = new List<DataAttribute>();
        }
        /// <summary>
        /// Initializes a new DataAttribute instance
        /// </summary>
        /// <param name="Name">Attribute name</param>
        /// <param name="Children">Child attributes</param>
        public DataAttribute( string Name, List<DataAttribute> Children )
        {
            this.Name = Name;
            this.Children = Children;
        }
        #endregion
    }
}
