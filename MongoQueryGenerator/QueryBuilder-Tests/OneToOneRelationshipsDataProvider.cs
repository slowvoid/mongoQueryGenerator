using QueryBuilder.ER;
using QueryBuilder.Map;
using QueryBuilder.Mongo;
using System.Collections.Generic;

namespace QueryBuilder.Tests
{
    /// <summary>
    /// Provides data for tests
    /// </summary>
    public static class OneToOneRelationshipsDataProvider
    {
        #region Methods
        /// <summary>
        /// Generates required data to test a One to One relationship
        /// without an embedded document
        /// </summary>
        /// <returns></returns>
        public static RequiredDataContainer OneToOneNotEmbedded()
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
            PersonCollection.DocumentSchema.AddAttribute( "carId" );

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
        /// <summary>
        /// Generates required data to test a One to One relationship
        /// with an embedded document 
        /// </summary>
        /// <returns></returns>
        public static RequiredDataContainer OneToOneEmbedded()
        {
            // Create ER Stuff
            Entity Person = new Entity( "Person" );
            Person.AddAttribute( "personId" );
            Person.AddAttribute( "name" );
            Person.AddAttribute( "carId" );

            Entity Car = new Entity( "Car" );
            Car.AddAttribute( "carId" );
            Car.AddAttribute( "carName" );
            Car.AddAttribute( "carYear" );

            Relationship Drives = new Relationship( "Drives", RelationshipCardinality.OneToOne );
            RelationshipConnection PersonCar = new RelationshipConnection(
                Person, Person.GetAttribute( "carId" ), Car, Car.GetAttribute( "carId" ) );
            Drives.AddRelation( PersonCar );

            ERModel Model = new ERModel( "PersonCarModel", new List<BaseERElement> { Person, Car, Drives } );

            // Create MongoDB schema
            MongoDBCollection PersonCollection = new MongoDBCollection( "Person" );
            PersonCollection.DocumentSchema.AddAttribute( "_id" );
            PersonCollection.DocumentSchema.AddAttribute( "name" );
            PersonCollection.DocumentSchema.AddAttribute( "carId" );
            PersonCollection.DocumentSchema.AddAttribute( "drives.carId" );
            PersonCollection.DocumentSchema.AddAttribute( "drives.carName" );
            PersonCollection.DocumentSchema.AddAttribute( "drives.carYear" );

            MongoSchema Schema = new MongoSchema( "PersonCarSchema", new List<MongoDBCollection> { PersonCollection } );

            // Create Map
            MapRule PersonRule = new MapRule( Model.FindByName( "Person" ), Schema.FindByName( "Person" ) );
            PersonRule.AddRule( "personId", "_id" );
            PersonRule.AddRule( "name", "name" );
            PersonRule.AddRule( "carId", "carId" );

            MapRule CarRule = new MapRule( Model.FindByName( "Car" ), Schema.FindByName( "Person" ) );
            CarRule.AddRule( "carId", "drives.carId" );
            CarRule.AddRule( "carName", "drives.carName" );
            CarRule.AddRule( "carYear", "drives.carYear" );

            ModelMapping Map = new ModelMapping( "PersonCarMap", new List<MapRule> { PersonRule, CarRule } );

            return new RequiredDataContainer( Model, Schema, Map );
        }
        /// <summary>
        /// Generates required data to test a One to One relationship
        /// with an embedded document without a master attribute
        /// </summary>
        /// <returns></returns>
        public static RequiredDataContainer OneToOneEmbeddedNoMasterAttribute()
        {
            // Create ER Stuff
            Entity Person = new Entity( "Person" );
            Person.AddAttribute( "personId" );
            Person.AddAttribute( "name" );

            Entity Car = new Entity( "Car" );
            Car.AddAttribute( "carId" );
            Car.AddAttribute( "carName" );
            Car.AddAttribute( "carYear" );

            Relationship Drives = new Relationship( "Drives", RelationshipCardinality.OneToOne );
            RelationshipConnection PersonCar = new RelationshipConnection(
                Person, Person.GetAttribute( "carId" ), Car, Car.GetAttribute( "carId" ) );
            Drives.AddRelation( PersonCar );

            ERModel Model = new ERModel( "PersonCarModel", new List<BaseERElement> { Person, Car, Drives } );

            // Create MongoDB schema
            MongoDBCollection PersonCollection = new MongoDBCollection( "PersonDrivesCar" );
            PersonCollection.DocumentSchema.AddAttribute( "_id" );
            PersonCollection.DocumentSchema.AddAttribute( "name" );
            PersonCollection.DocumentSchema.AddAttribute( "carId" );
            PersonCollection.DocumentSchema.AddAttribute( "carName" );
            PersonCollection.DocumentSchema.AddAttribute( "carYear" );

            MongoSchema Schema = new MongoSchema( "PersonCarSchema", new List<MongoDBCollection> { PersonCollection } );

            // Create Map
            MapRule PersonRule = new MapRule( Model.FindByName( "Person" ), Schema.FindByName( "PersonDrivesCar" ) );
            PersonRule.AddRule( "personId", "_id" );
            PersonRule.AddRule( "name", "name" );

            MapRule CarRule = new MapRule( Model.FindByName( "Car" ), Schema.FindByName( "PersonDrivesCar" ) );
            CarRule.AddRule( "carId", "carId" );
            CarRule.AddRule( "carName", "carName" );
            CarRule.AddRule( "carYear", "carYear" );

            ModelMapping Map = new ModelMapping( "PersonCarMap", new List<MapRule> { PersonRule, CarRule } );

            return new RequiredDataContainer( Model, Schema, Map );
        }
        /// <summary>
        /// Generates required data to test a One to One relationship
        /// with an embedded document with data mixed in direct embedded and using master attribute
        /// </summary>
        /// <returns></returns>
        public static RequiredDataContainer OneToOneEmbeddedMixed()
        {
            // Create ER Stuff
            Entity Person = new Entity( "Person" );
            Person.AddAttribute( "personId" );
            Person.AddAttribute( "name" );

            Entity Car = new Entity( "Car" );
            Car.AddAttribute( "carId" );
            Car.AddAttribute( "carName" );
            Car.AddAttribute( "carYear" );

            Relationship Drives = new Relationship( "Drives", RelationshipCardinality.OneToOne );
            RelationshipConnection PersonCar = new RelationshipConnection(
                Person, Person.GetAttribute( "carId" ), Car, Car.GetAttribute( "carId" ) );
            Drives.AddRelation( PersonCar );

            ERModel Model = new ERModel( "PersonCarModel", new List<BaseERElement> { Person, Car, Drives } );

            // Create MongoDB schema
            MongoDBCollection PersonCollection = new MongoDBCollection( "PersonDrivesCarMixed" );
            PersonCollection.DocumentSchema.AddAttribute( "_id" );
            PersonCollection.DocumentSchema.AddAttribute( "name" );
            PersonCollection.DocumentSchema.AddAttribute( "carData.carId" );
            PersonCollection.DocumentSchema.AddAttribute( "carData.carName" );
            PersonCollection.DocumentSchema.AddAttribute( "carData.carYear" );

            MongoSchema Schema = new MongoSchema( "PersonCarSchema", new List<MongoDBCollection> { PersonCollection } );

            // Create Map
            MapRule PersonRule = new MapRule( Model.FindByName( "Person" ), Schema.FindByName( "PersonDrivesCarMixed" ) );
            PersonRule.AddRule( "personId", "_id" );
            PersonRule.AddRule( "name", "name" );

            MapRule CarRule = new MapRule( Model.FindByName( "Car" ), Schema.FindByName( "PersonDrivesCarMixed" ) );
            CarRule.AddRule( "carId", "carId" );
            CarRule.AddRule( "carName", "carData.carName" );
            CarRule.AddRule( "carYear", "carData.carYear" );

            ModelMapping Map = new ModelMapping( "PersonCarMap", new List<MapRule> { PersonRule, CarRule } );

            return new RequiredDataContainer( Model, Schema, Map );
        }
        #endregion
    }
}