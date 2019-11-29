using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QueryBuilder.Shared;

namespace QueryBuilder.Mongo
{
    /// <summary>
    /// Represents a MongoDB collection
    /// </summary>
    public class MongoDBCollection
    {
        #region Properties
        /// <summary>
        /// Collection name
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Document schema
        /// </summary>
        public Document DocumentSchema { get; set; }
        #endregion

        #region Methods
        /// <summary>
        /// Add an attribute to the document schema
        /// </summary>
        /// <param name="Name"></param>
        public void AddAttribute(string Name, string OfType, bool MultiValued)
        {
            DocumentSchema.AddAttribute( Name, OfType, MultiValued );
        }

        public DataAttribute GetAttribute( string Name )
        {
            return DocumentSchema.Attributes.Find( A => A.Name == Name );
        }

        /// <summary>
        /// Add a sequence of attributes to the document schema
        /// </summary>
        /// <param name="Names"></param>
        // public void AddAttributes( params string[] Names )
        // {
        //     foreach ( string Name in Names )
        //     {
        //         AddAttribute( Name );
        //     }
        // }
        #endregion

        #region Constructors
        /// <summary>
        /// Initialize a new Collection instance
        /// </summary>
        /// <param name="Name"></param>
        public MongoDBCollection( string Name )
        {
            this.Name = Name;
            DocumentSchema = new Document();
        }
        #endregion
    }
}
