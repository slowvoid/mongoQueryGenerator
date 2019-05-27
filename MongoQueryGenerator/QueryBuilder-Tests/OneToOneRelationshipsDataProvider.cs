using QueryBuilder.ER;
using QueryBuilder.Map;
using QueryBuilder.Mongo;
using System.Collections.Generic;

namespace QueryBuilder.Tests
{
    /// <summary>
    /// Provides data fro tests
    /// </summary>
    public static class OneToOneRelationshipsDataProvider
    {
        #region Methods
        /// <summary>
        /// Generates required data to test a One to One relationship
        /// without an embbebed document
        /// </summary>
        /// <returns></returns>
        public static RequiredDataContainer OneToOneNotEmbbebed()
        {
            // Create ER Stuff
            Entity Person = new Entity( "Person" );
            Person.AddAttribute( "personId" );
            Person.AddAttribute( "name" );
            Person.AddAttribute( "carId" );

            Entity Car = new Entity( "Car" );
            Car.AddAttribute( "carId" );
            Car.AddAttribute( "name" );
            Car.AddAttribute( "year" );

            Relationship Drives = new Relationship( "Drives", RelationshipCardinality.OneToOne );
            RelationshipConnection PersonCar = new RelationshipConnection(
                Person, Person.GetAttribute( "carId" ), Car, Car.GetAttribute( "carId" ) );
            Drives.AddRelation( PersonCar );

            ERModel Model = new ERModel( "PersonCarModel", new List<BaseERElement> { Person, Car, Drives } );

            // Create MongoDB schema
            MongoDBCollection PersonCollection = new MongoDBCollection( "Person" );
            PersonCollection.DocumentSchema.AddAttribute( "_id" );
            PersonCollection.DocumentSchema.AddAttribute( "name" );
            PersonCollection.DocumentSchema.AddAttribute( "year" );

            MongoDBCollection CarCollection = new MongoDBCollection( "Car" );
            CarCollection.DocumentSchema.AddAttribute( "_id" );
            CarCollection.DocumentSchema.AddAttribute( "name" );
            CarCollection.DocumentSchema.AddAttribute( "year" );

            MongoSchema Schema = new MongoSchema( "PersonCarSchema", new List<MongoDBCollection> { PersonCollection, CarCollection } );

            // Create Map
            MapRule PersonRule = new MapRule( Model.FindByName( "Person" ), Schema.FindByName( "Person" ) );
            PersonRule.AddRule( "personId", "_id" );
            PersonRule.AddRule( "name", "name" );
            PersonRule.AddRule( "carId", "carId" );

            MapRule CarRule = new MapRule( Model.FindByName( "Car" ), Schema.FindByName( "Car" ) );
            CarRule.AddRule( "carId", "_id" );
            CarRule.AddRule( "name", "name" );
            CarRule.AddRule( "year", "year" );

            ModelMapping Map = new ModelMapping( "PersonCarMap", new List<MapRule> { PersonRule, CarRule } );

            return new RequiredDataContainer( Model, Schema, Map );
        }
        #endregion
    }
}