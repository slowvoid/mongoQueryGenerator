using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using QueryBuilder.Shared;

namespace QueryBuilder.Mongo
{
    /// <summary>
    /// Represents a MongoDB Document
    /// </summary>
    public class Document
    {
        #region Properties
        /// <summary>
        /// ID Attribute (_id)
        /// </summary>
        public object ID { get; set; }
        /// <summary>
        /// Other attributes
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
            Attributes.Add( new DataAttribute( Name, null, Identifier ) );
        }
        /// <summary>
        /// Find an attribute by name
        /// </summary>
        /// <param name="Name">Attribute name</param>
        /// <returns></returns>
        public DataAttribute GetAttribute( string Name )
        {
            return Attributes.Find( A => A.Name == Name );
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Initialize a new Document instance
        /// </summary>
        public Document()
        {
            Attributes = new List<DataAttribute>();
        }
        #endregion
    }
}
