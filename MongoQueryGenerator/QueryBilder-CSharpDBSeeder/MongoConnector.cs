using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueryBuilder.CSharpDBSeeder
{
    /// <summary>
    /// Provides connection to MongoDB
    /// </summary>
    public class MongoConnector
    {
        #region Properties
        /// <summary>
        /// Database
        /// </summary>
        public IMongoDatabase Database { get; set; }
        #endregion

        #region Constructor
        /// <summary>
        /// Initializes a new MongoConnector instance and connect to a local database 
        /// </summary>
        public MongoConnector()
        {
            IMongoClient Client = new MongoClient( "mongodb://localhost" );
            Database = Client.GetDatabase( "researchDatabase" );
        }
        #endregion
    }
}
