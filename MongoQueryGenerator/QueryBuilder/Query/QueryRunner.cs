﻿using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueryBuilder.Query
{
    /// <summary>
    /// Provides methods to execute queries into MongoDB
    /// </summary>
    public class QueryRunner
    {
        #region Properties
        /// <summary>
        /// Database connection string
        /// </summary>
        public string ConnectionString { get; set; }
        /// <summary>
        /// Database name
        /// </summary>
        public string Database { get; set; }
        #endregion

        #region Methods
        /// <summary>
        /// Runs a query against mongodb
        /// </summary>
        /// <param name="Query">Query to run</param>
        /// <returns></returns>
        private BsonDocument _executeQuery( string Query )
        {
            // Connect to MongoDB
            MongoClient Client = new MongoClient( ConnectionString );

            // Select database
            IMongoDatabase db = Client.GetDatabase( Database );

            // Prepare query string
            BsonDocument QueryDocument = new BsonDocument()
            {
                { "eval", Query }
            };

            BsonDocumentCommand<BsonDocument> Command = new BsonDocumentCommand<BsonDocument>( QueryDocument );
            BsonDocument QueryResult = db.RunCommand( Command );

            return QueryResult;
        }
        /// <summary>
        /// Execute a query and retrieve results as json
        /// </summary>
        /// <param name="Query">Query to execute</param>
        /// <returns></returns>
        public string GetJSON( string Query )
        {
            BsonDocument QueryResult = _executeQuery( Query );
            
            return QueryResult.GetElement( "retval" ).Value.ToBsonDocument().GetElement( "_batch" ).Value.ToJson();
        }
        /// <summary>
        /// Parse explain('executionStats') for aggregate queries
        /// </summary>
        /// <param name="ExplainQuery"></param>
        /// <returns></returns>
        public QueryStats GetExplainResult( string ExplainQuery )
        {
            BsonDocument QueryExplainResult = _executeQuery( ExplainQuery );

            string resultjson = QueryExplainResult.GetElement( "retval" ).Value.ToBsonDocument()
                .GetElement( "stages" ).Value.AsBsonArray.First().ToBsonDocument()
                .GetElement( "$cursor" ).Value.ToBsonDocument()
                .GetElement( "executionStats" ).Value.ToJson();

            return JsonConvert.DeserializeObject<QueryStats>( resultjson );
        }
        /// <summary>
        /// Parse explain('executionStats') for non aggregate queries
        /// </summary>
        /// <param name="ExplainQuery"></param>
        /// <returns></returns>
        public QueryStats GetExplainResultNonAggregate( string ExplainQuery )
        {
            BsonDocument QueryExplainResult = _executeQuery( ExplainQuery );

            string resultjson = QueryExplainResult.GetElement( "retval" ).Value.ToBsonDocument()
                .GetElement( "executionStats" ).Value.ToJson();

            return JsonConvert.DeserializeObject<QueryStats>( resultjson );
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Initialize a new QueryRunner instance
        /// </summary>
        public QueryRunner( string ConnectionString, string Database )
        {
            this.ConnectionString = ConnectionString;
            this.Database = Database;
        }
        #endregion
    }
}
