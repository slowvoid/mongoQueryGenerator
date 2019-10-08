using QueryBuilder.ER;
using QueryBuilder.Map;
using QueryBuilder.Mongo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueryBuilderApp
{
    /// <summary>
    /// Provides testing data
    /// </summary>
    public static class TestDataProvider
    {
        public static ERModel CreateModel()
        {
            Entity Person = new Entity( "Person" );
            Person.AddAttributes( "personId", "age", "name", "value" );

            ERModel Model = new ERModel( "Model", new List<BaseERElement>() { Person } );
            return Model;
        }

        public static MongoSchema CreateSchema()
        {
            MongoDBCollection PersonCol = new MongoDBCollection( "Person" );
            PersonCol.AddAttributes( "_id", "age", "name", "value" );

            MongoSchema Schema = new MongoSchema( "Schema", new List<MongoDBCollection>() { PersonCol } );
            return Schema;
        }

        public static ModelMapping CreateMap(ERModel Model, MongoSchema Schema)
        {
            MapRule PersonRules = new MapRule( Model.FindByName( "Person" ), Schema.FindByName( "Person" ) );
            PersonRules.AddRule( "personId", "_id" );
            PersonRules.AddRule( "age", "age" );
            PersonRules.AddRule( "name", "name" );
            PersonRules.AddRule( "value", "value" );

            ModelMapping Map = new ModelMapping( "Map", new List<MapRule>() { PersonRules } );
            return Map;
        }
    }
}
