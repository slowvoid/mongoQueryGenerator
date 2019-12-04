using System;
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
        /// Add a new attribute
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="OfType"></param>
        /// <param name="MultiValued"></param>
        /// <param name="Identifier"></param>
        public void AddAttribute( string Name, string OfType, bool MultiValued, bool Identifier = false )
        {
            Attributes.Add( new DataAttribute( Name, OfType, this, MultiValued, Identifier ) );
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
        public DataAttribute GetIdentifierAttribute()
        {
            return Attributes.Find( A => A.Name == "id" );
        }
        /// <summary>
        /// Sets the given attribute name the identifier status
        /// </summary>
        /// <param name="Name"></param>
        public void SetIdentifier( string Name )
        {
            DataAttribute Attr = Attributes.FirstOrDefault( A => A.Name == Name );

            if ( Attr != null )
            {
                Attr.IsIdentifier = true;
            }
        }
        #endregion
    }
}
