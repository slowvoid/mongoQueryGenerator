using QueryBuilder.ER;
using QueryBuilder.Map;
using QueryBuilder.Mongo;
using System;
using System.Collections.Generic;

namespace QueryBuilder.Tests
{
    /// <summary>
    /// Provides data for SELECT operation tests
    /// </summary>
    public static class SelectDataProvider
    {
        /// <summary>
        /// Generate data for the 
        /// </summary>
        /// <returns></returns>
        public static RequiredDataContainer GetData()
        {
            // ER Stuff
            Entity Person = new Entity( "Person" );
            Person.AddAttributes( "personId", "age", "name" );

            ERModel Model = new ERModel( "ERModel", new List<BaseERElement> { Person } );

            MongoDBCollection PersonCol = new MongoDBCollection( "Person" );
            PersonCol.AddAttributes( "_id", "age", "name" );

            MongoSchema Schema = new MongoSchema( "Schema", new List<MongoDBCollection> { PersonCol } );

            MapRule PersonRule = new MapRule( Person, PersonCol );
            PersonRule.AddRule( "personId", "_id" );
            PersonRule.AddRule( "age", "age" );
            PersonRule.AddRule( "name", "name" );

            ModelMapping Map = new ModelMapping( "Map", new List<MapRule> { PersonRule } );

            return new RequiredDataContainer( Model, Schema, Map );
        }
    }
}