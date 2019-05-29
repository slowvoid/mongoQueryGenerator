using QueryBuilder.ER;
using QueryBuilder.Map;
using QueryBuilder.Mongo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueryBuilder.Tests
{
    /// <summary>
    /// Provides data for tests
    /// </summary>
    public static class OneToManyRelationshipsDataProvider
    {
        #region Methods
        /// <summary>
        /// Generates required data for a OneToMany relationship without embbebed documents
        /// </summary>
        /// <returns></returns>
        public static RequiredDataContainer OneToManyNotEmbbebed()
        {
            // Create ER Stuff
            Entity Person = new Entity( "Person" );
            Person.AddAttribute( "personId" );
            Person.AddAttribute( "name" );

            Entity Car = new Entity( "Car" );
            Car.AddAttribute( "carId" );
            Car.AddAttribute( "name" );
            Car.AddAttribute( "year" );
            Car.AddAttribute( "personId" );

            Relationship Drives = new Relationship( "Drives", RelationshipCardinality.OneToMany );
            RelationshipConnection PersonCar = new RelationshipConnection(
                Person, Person.GetAttribute( "personId" ), Car, Car.GetAttribute( "personId" ) );
            Drives.AddRelation( PersonCar );

            ERModel Model = new ERModel( "PersonCarModel", new List<BaseERElement> { Person, Car, Drives } );

            // Create MongoDB schema
            MongoDBCollection PersonCollection = new MongoDBCollection( "Person" );
            PersonCollection.DocumentSchema.AddAttribute( "_id" );
            PersonCollection.DocumentSchema.AddAttribute( "name" );

            MongoDBCollection CarCollection = new MongoDBCollection( "Car" );
            CarCollection.DocumentSchema.AddAttribute( "_id" );
            CarCollection.DocumentSchema.AddAttribute( "name" );
            CarCollection.DocumentSchema.AddAttribute( "year" );
            CarCollection.DocumentSchema.AddAttribute( "personId" );

            MongoSchema Schema = new MongoSchema( "PersonCarSchema", new List<MongoDBCollection> { PersonCollection, CarCollection } );

            // Create Map
            MapRule PersonRule = new MapRule( Model.FindByName( "Person" ), Schema.FindByName( "Person" ) );
            PersonRule.AddRule( "personId", "_id" );
            PersonRule.AddRule( "name", "name" );

            MapRule CarRule = new MapRule( Model.FindByName( "Car" ), Schema.FindByName( "Car" ) );
            CarRule.AddRule( "carId", "_id" );
            CarRule.AddRule( "name", "name" );
            CarRule.AddRule( "year", "year" );
            CarRule.AddRule( "personId", "personId" );

            ModelMapping Map = new ModelMapping( "PersonCarMap", new List<MapRule> { PersonRule, CarRule } );

            return new RequiredDataContainer( Model, Schema, Map );
        }
        #endregion
    }
}
