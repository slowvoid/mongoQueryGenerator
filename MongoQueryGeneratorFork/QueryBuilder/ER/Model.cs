using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using QueryBuilder.ER.Exceptions;

namespace QueryBuilder.ER
{
    /// <summary>
    /// Represents a ER Model
    /// </summary>
    public class ERModel
    {
        #region Properties
        /// <summary>
        /// Model name
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// ER elements that compose this model
        /// </summary>
        public List<BaseERElement> Elements { get; set; }
        #endregion

        #region Methods
        /// <summary>
        /// Finds an element by name
        /// </summary>
        /// <param name="Name">Name to search</param>
        /// <exception cref="ElementNotFoundException"></exception>
        /// <returns></returns>
        public BaseERElement FindByName( string Name )
        {
            BaseERElement elementFound = null;
            try {
            // Store the element
            elementFound = Elements.First( E => E.Name == Name );
            } catch(InvalidOperationException) {}

            if ( elementFound == null )
            {
                throw new ElementNotFoundException( string.Format( "Failed to find element [{0}] on model [{1}]", Name, this.Name ) );
            }

            return elementFound;
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Initialize a new Model instance
        /// </summary>
        /// <param name="Name">Model name</param>
        /// <param name="Element">Elements associated to this model</param>
        public ERModel( string Name, List<BaseERElement> Elements )
        {
            this.Name = Name;
            this.Elements = Elements;
        }
        #endregion
    }
}
