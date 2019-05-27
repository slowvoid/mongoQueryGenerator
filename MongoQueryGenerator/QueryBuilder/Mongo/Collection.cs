using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
