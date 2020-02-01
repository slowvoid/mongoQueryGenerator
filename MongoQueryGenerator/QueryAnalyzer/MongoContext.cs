using MongoDB.Driver;
using QueryBuilder.Query;
using System;
using System.Collections.Generic;
using System.Text;

namespace QueryAnalyzer
{
    public class MongoContext
    {
        public static void InsertRecord( string Database, string Collection, QueryStats Model )
        {
            MongoClient client = new MongoClient( "mongodb://localhost:27017" );
            var db = client.GetDatabase( Database );
            var collection = db.GetCollection<QueryStats>( Collection );

            collection.InsertOne( Model );
        }

        public static void InsertManyRecords( string Database, string Collection, IEnumerable<QueryStats> Models )
        {
            MongoClient client = new MongoClient( "mongodb://localhost:27017" );
            var db = client.GetDatabase( Database );
            var collection = db.GetCollection<QueryStats>( Collection );

            collection.InsertMany( Models );
        }

        public static void DropCollection( string Database, string Collection )
        {
            MongoClient client = new MongoClient( "mongodb://localhost:27017" );
            var db = client.GetDatabase( Database );

            db.DropCollection( Collection );
        }
    }
}
