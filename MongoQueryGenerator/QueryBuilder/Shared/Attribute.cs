using QueryBuilder.ER;
using QueryBuilder.Shared.Exceptions;
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
        /// Parent Element
        /// </summary>
        public BaseERElement Parent { get; set; }
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
        /// <summary>
        /// Gets/Sets wheter this attribute is also an identifier
        /// </summary>
        public bool IsIdentifier { get; set; }
        #endregion

        #region Functions
        /// <summary>
        /// Returns if the given name belongs to a child attribute
        /// </summary>
        /// <param name="Name">Name to search for</param>
        /// <returns>Returns an attribute</returns>
        public bool HasAttribute( string Name )
        {
            // Search first for all children, then if not found lookup recursively
            bool FoundAttribute = Children.Exists( Attr => Attr.Name == Name );

            if ( !FoundAttribute )
            {
                // Attribute not found, search through each child children
                foreach ( DataAttribute Child in Children )
                {
                    FoundAttribute = Child.HasAttribute( Name );

                    if ( FoundAttribute )
                    {
                        break;
                    }
                }
            }

            return FoundAttribute;
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new DataAttribute instance
        /// </summary>
        /// <param name="Name">Attribute name</param>
        public DataAttribute( string Name, BaseERElement Parent )
        {
            this.Name = Name;
            this.Parent = Parent;
            IsIdentifier = false;
            Children = new List<DataAttribute>();
        }
        /// <summary>
        /// Initializes a new DataAttribute instance
        /// </summary>
        /// <param name="Name">Attribute name</param>
        /// <param name="Identifier">Is this attribute an identifier</param>
        public DataAttribute( string Name, BaseERElement Parent, bool Identifier )
        {
            this.Name = Name;
            this.Parent = Parent;
            this.IsIdentifier = Identifier;
            Children = new List<DataAttribute>();
        }
        /// <summary>
        /// Initializes a new DataAttribute instance
        /// </summary>
        /// <param name="Name">Attribute name</param>
        /// <param name="Identifier">Is this attribute an identifier</param>
        /// <param name="Children">Child attributes</param>
        public DataAttribute( string Name, BaseERElement Parent, bool Identifier, List<DataAttribute> Children )
        {
            this.Name = Name;
            this.Parent = Parent;
            this.IsIdentifier = Identifier;
            this.Children = Children;
        }
        #endregion
    }
}
