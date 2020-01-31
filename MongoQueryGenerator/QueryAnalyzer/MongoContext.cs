using MongoDB.Driver;
using QueryBuilder.Query;
using System;
using System.Collections.Generic;
using System.Text;

namespace QueryAnalyzer
{
    public class MongoContext
    {
        public static void InsertRecord( string Collection, QueryStats Model )
        {
            MongoClient client = new MongoClient( "mongodb://localhost:27017" );
            var db = client.GetDatabase( "research_performance_index_stats" );
            var collection = db.GetCollection<QueryStats>( Collection );

            collection.InsertOneAsync( Model );
        }

        public static void InsertManyRecords( string Collection, IEnumerable<QueryStats> Models )
        {
            MongoClient client = new MongoClient( "mongodb://localhost:27017" );
            var db = client.GetDatabase( "research_performance_index_stats" );
            var collection = db.GetCollection<QueryStats>( Collection );

            collection.InsertManyAsync( Models );
        }

        public static void DropCollection( string Collection )
        {
            MongoClient client = new MongoClient( "mongodb://localhost:27017" );
            var db = client.GetDatabase( "research_performance_index_stats" );

            db.DropCollection( Collection );
        }
    }
}
