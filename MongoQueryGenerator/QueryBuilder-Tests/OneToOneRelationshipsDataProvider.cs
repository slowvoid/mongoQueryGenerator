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
        /// <summary>
        /// Generates required data to test a One to One relationship
        /// without an embedded document with multiple entities
        /// </summary>
        /// <returns></returns>
        public static RequiredDataContainer OneToOneNotEmbeddedMultipleEntities()
        {
            // Create ER Stuff
            Entity Person = new Entity( "Person" );
            Person.AddAttribute( "personId" );
            Person.AddAttribute( "name" );
            Person.AddAttribute( "carId" );
            Person.AddAttribute( "insuranceId" );

            Entity Car = new Entity( "Car" );
            Car.AddAttribute( "carId" );
            Car.AddAttribute( "name" );
            Car.AddAttribute( "year" );

            Entity Insurance = new Entity( "Insurance" );
            Insurance.AddAttribute( "insuranceId" );
            Insurance.AddAttribute( "name" );

            Relationship HasInsurance = new Relationship( "HasInsurance", RelationshipCardinality.OneToOne );
            RelationshipConnection PersonCar = new RelationshipConnection(
                Person, Person.GetAttribute( "carId" ), Car, Car.GetAttribute( "carId" ) );
            HasInsurance.AddRelation( PersonCar );

            RelationshipConnection PersonInsurance = new RelationshipConnection(
                Person, Person.GetAttribute( "insuranceId" ), Insurance, Insurance.GetAttribute( "insuranceId" ) );
            HasInsurance.AddRelation( PersonInsurance );

            ERModel Model = new ERModel( "PersonCarModel", new List<BaseERElement> { Person, Car, HasInsurance, Insurance } );

            // Create MongoDB schema
            MongoDBCollection PersonCollection = new MongoDBCollection( "Person" );
            PersonCollection.DocumentSchema.AddAttribute( "_id" );
            PersonCollection.DocumentSchema.AddAttribute( "name" );
            PersonCollection.DocumentSchema.AddAttribute( "carId" );
            PersonCollection.DocumentSchema.AddAttribute( "insuranceId" );

            MongoDBCollection CarCollection = new MongoDBCollection( "Car" );
            CarCollection.DocumentSchema.AddAttribute( "_id" );
            CarCollection.DocumentSchema.AddAttribute( "name" );
            CarCollection.DocumentSchema.AddAttribute( "year" );

            MongoDBCollection InsuranceCollection = new MongoDBCollection( "Insurance" );
            InsuranceCollection.DocumentSchema.AddAttribute( "_id" );
            InsuranceCollection.DocumentSchema.AddAttribute( "name" );

            MongoSchema Schema = new MongoSchema( "PersonCarSchema", new List<MongoDBCollection> { PersonCollection, CarCollection, InsuranceCollection } );

            // Create Map
            MapRule PersonRule = new MapRule( Model.FindByName( "Person" ), Schema.FindByName( "Person" ) );
            PersonRule.AddRule( "personId", "_id" );
            PersonRule.AddRule( "name", "name" );
            PersonRule.AddRule( "carId", "carId" );
            PersonRule.AddRule( "insuranceId", "insuranceId" );

            MapRule CarRule = new MapRule( Model.FindByName( "Car" ), Schema.FindByName( "Car" ) );
            CarRule.AddRule( "carId", "_id" );
            CarRule.AddRule( "name", "name" );
            CarRule.AddRule( "year", "year" );

            MapRule InsuranceRule = new MapRule( Model.FindByName( "Insurance" ), Schema.FindByName( "Insurance" ) );
            InsuranceRule.AddRule( "insuranceId", "_id" );
            InsuranceRule.AddRule( "name", "name" );

            ModelMapping Map = new ModelMapping( "PersonCarMap", new List<MapRule> { PersonRule, CarRule, InsuranceRule } );

            return new RequiredDataContainer( Model, Schema, Map );
        }
        /// <summary>
        /// Generates required data to test a One to One relationship
        /// joining multiple entities with and without being embedded
        /// </summary>
        /// <returns></returns>
        public static RequiredDataContainer OneToOneMultipleEntitiesMixed()
        {
            // Create ER Stuff
            Entity Person = new Entity( "Person" );
            Person.AddAttribute( "personId" );
            Person.AddAttribute( "name" );
            Person.AddAttribute( "insuranceId" );

            Entity Car = new Entity( "Car" );
            Car.AddAttribute( "name" );
            Car.AddAttribute( "year" );

            Entity Insurance = new Entity( "Insurance" );
            Insurance.AddAttribute( "insuranceId" );
            Insurance.AddAttribute( "name" );

            Relationship HasInsurance = new Relationship( "HasInsurance", RelationshipCardinality.OneToOne );
            RelationshipConnection PersonCar = new RelationshipConnection(
                Person, Person.GetAttribute( "carId" ), Car, Car.GetAttribute( "name" ) );
            HasInsurance.AddRelation( PersonCar );

            RelationshipConnection PersonInsurance = new RelationshipConnection(
                Person, Person.GetAttribute( "insuranceId" ), Insurance, Insurance.GetAttribute( "insuranceId" ) );
            HasInsurance.AddRelation( PersonInsurance );

            ERModel Model = new ERModel( "PersonCarModel", new List<BaseERElement> { Person, Car, HasInsurance, Insurance } );

            // Create MongoDB schema
            MongoDBCollection PersonCollection = new MongoDBCollection( "Person" );
            PersonCollection.DocumentSchema.AddAttribute( "_id" );
            PersonCollection.DocumentSchema.AddAttribute( "name" );
            PersonCollection.DocumentSchema.AddAttribute( "car.name" );
            PersonCollection.DocumentSchema.AddAttribute( "car.year" );
            PersonCollection.DocumentSchema.AddAttribute( "insuranceId" );

            MongoDBCollection InsuranceCollection = new MongoDBCollection( "Insurance" );
            InsuranceCollection.DocumentSchema.AddAttribute( "_id" );
            InsuranceCollection.DocumentSchema.AddAttribute( "name" );

            MongoSchema Schema = new MongoSchema( "PersonCarSchema", new List<MongoDBCollection> { PersonCollection, InsuranceCollection } );

            // Create Map
            MapRule PersonRule = new MapRule( Model.FindByName( "Person" ), Schema.FindByName( "Person" ) );
            PersonRule.AddRule( "personId", "_id" );
            PersonRule.AddRule( "name", "name" );
            PersonRule.AddRule( "carId", "carId" );
            PersonRule.AddRule( "insuranceId", "insuranceId" );

            MapRule CarRule = new MapRule( Model.FindByName( "Car" ), Schema.FindByName( "Person" ) );
            CarRule.AddRule( "name", "car.name" );
            CarRule.AddRule( "year", "car.year" );

            MapRule InsuranceRule = new MapRule( Model.FindByName( "Insurance" ), Schema.FindByName( "Insurance" ) );
            InsuranceRule.AddRule( "insuranceId", "_id" );
            InsuranceRule.AddRule( "name", "name" );

            ModelMapping Map = new ModelMapping( "PersonCarMap", new List<MapRule> { PersonRule, CarRule, InsuranceRule } );

            return new RequiredDataContainer( Model, Schema, Map );
        }
        /// <summary>
        /// Generates required data to test a One to One relationship
        /// joining multiple entities with relationship attribute and mixed embedded/external entities
        /// </summary>
        /// <returns></returns>
        public static RequiredDataContainer OneToOneRelationshipAttributes()
        {
            // Create ER Stuff
            Entity Person = new Entity( "Person" );
            Person.AddAttribute( "personId" );
            Person.AddAttribute( "name" );
            Person.AddAttribute( "insuranceId" );

            Entity Car = new Entity( "Car" );
            Car.AddAttribute( "name" );
            Car.AddAttribute( "year" );

            Entity Insurance = new Entity( "Insurance" );
            Insurance.AddAttribute( "insuranceId" );
            Insurance.AddAttribute( "name" );

            Relationship HasInsurance = new Relationship( "HasInsurance", RelationshipCardinality.OneToOne );
            HasInsurance.AddAttribute( "insuranceValue" );
            RelationshipConnection PersonCar = new RelationshipConnection(
                Person, Person.GetAttribute( "carId" ), Car, Car.GetAttribute( "name" ) );
            HasInsurance.AddRelation( PersonCar );

            RelationshipConnection PersonInsurance = new RelationshipConnection(
                Person, Person.GetAttribute( "insuranceId" ), Insurance, Insurance.GetAttribute( "insuranceId" ) );
            HasInsurance.AddRelation( PersonInsurance );

            ERModel Model = new ERModel( "PersonCarModel", new List<BaseERElement> { Person, Car, HasInsurance, Insurance } );

            // Create MongoDB schema
            MongoDBCollection PersonCollection = new MongoDBCollection( "Person" );
            PersonCollection.DocumentSchema.AddAttribute( "_id" );
            PersonCollection.DocumentSchema.AddAttribute( "name" );
            PersonCollection.DocumentSchema.AddAttribute( "car.name" );
            PersonCollection.DocumentSchema.AddAttribute( "car.year" );
            PersonCollection.DocumentSchema.AddAttribute( "insuranceId" );
            PersonCollection.DocumentSchema.AddAttribute( "insuranceValue" );

            MongoDBCollection InsuranceCollection = new MongoDBCollection( "Insurance" );
            InsuranceCollection.DocumentSchema.AddAttribute( "_id" );
            InsuranceCollection.DocumentSchema.AddAttribute( "name" );

            MongoSchema Schema = new MongoSchema( "PersonCarSchema", new List<MongoDBCollection> { PersonCollection, InsuranceCollection } );

            // Create Map
            MapRule PersonRule = new MapRule( Model.FindByName( "Person" ), Schema.FindByName( "Person" ) );
            PersonRule.AddRule( "personId", "_id" );
            PersonRule.AddRule( "name", "name" );
            PersonRule.AddRule( "carId", "carId" );
            PersonRule.AddRule( "insuranceId", "insuranceId" );

            MapRule CarRule = new MapRule( Model.FindByName( "Car" ), Schema.FindByName( "Person" ) );
            CarRule.AddRule( "name", "car.name" );
            CarRule.AddRule( "year", "car.year" );

            MapRule InsuranceRule = new MapRule( Model.FindByName( "Insurance" ), Schema.FindByName( "Insurance" ) );
            InsuranceRule.AddRule( "insuranceId", "_id" );
            InsuranceRule.AddRule( "name", "name" );

            MapRule RelationshipRule = new MapRule( Model.FindByName( "HasInsurance" ), Schema.FindByName( "Person" ) );
            RelationshipRule.AddRule( "insuranceValue", "insuranceValue" );

            ModelMapping Map = new ModelMapping( "PersonCarMap", new List<MapRule> { PersonRule, CarRule, InsuranceRule, RelationshipRule } );

            return new RequiredDataContainer( Model, Schema, Map );
        }
        /// <summary>
        /// Generates required data to test a One to One relationship
        /// joining multiple entities with relationship attribute and multiple root attributes 
        /// </summary>
        /// <returns></returns>
        public static RequiredDataContainer OneToOneRelationshipMultipleRootAttributes()
        {
            // Create ER Stuff
            Entity Person = new Entity( "Person" );
            Person.AddAttribute( "personId" );
            Person.AddAttribute( "name" );
            Person.AddAttribute( "insuranceId" );

            Entity Car = new Entity( "Car" );
            Car.AddAttribute( "name" );
            Car.AddAttribute( "year" );
            Car.AddAttribute( "engine" );
            Car.AddAttribute( "fuel" );

            Entity Insurance = new Entity( "Insurance" );
            Insurance.AddAttribute( "insuranceId" );
            Insurance.AddAttribute( "name" );

            Relationship HasInsurance = new Relationship( "HasInsurance", RelationshipCardinality.OneToOne );
            HasInsurance.AddAttribute( "insuranceValue" );
            RelationshipConnection PersonCar = new RelationshipConnection(
                Person, Person.GetAttribute( "carId" ), Car, Car.GetAttribute( "name" ) );
            HasInsurance.AddRelation( PersonCar );

            RelationshipConnection PersonInsurance = new RelationshipConnection(
                Person, Person.GetAttribute( "insuranceId" ), Insurance, Insurance.GetAttribute( "insuranceId" ) );
            HasInsurance.AddRelation( PersonInsurance );

            ERModel Model = new ERModel( "PersonCarModel", new List<BaseERElement> { Person, Car, HasInsurance, Insurance } );

            // Create MongoDB schema
            MongoDBCollection PersonCollection = new MongoDBCollection( "Person" );
            PersonCollection.DocumentSchema.AddAttribute( "_id" );
            PersonCollection.DocumentSchema.AddAttribute( "name" );
            PersonCollection.DocumentSchema.AddAttribute( "car.name" );
            PersonCollection.DocumentSchema.AddAttribute( "car.year" );
            PersonCollection.DocumentSchema.AddAttribute( "carDetails.engine" );
            PersonCollection.DocumentSchema.AddAttribute( "CarDetails.fuel" );
            PersonCollection.DocumentSchema.AddAttribute( "insuranceId" );
            PersonCollection.DocumentSchema.AddAttribute( "insuranceValue" );

            MongoDBCollection InsuranceCollection = new MongoDBCollection( "Insurance" );
            InsuranceCollection.DocumentSchema.AddAttribute( "_id" );
            InsuranceCollection.DocumentSchema.AddAttribute( "name" );

            MongoSchema Schema = new MongoSchema( "PersonCarSchema", new List<MongoDBCollection> { PersonCollection, InsuranceCollection } );

            // Create Map
            MapRule PersonRule = new MapRule( Model.FindByName( "Person" ), Schema.FindByName( "Person" ) );
            PersonRule.AddRule( "personId", "_id" );
            PersonRule.AddRule( "name", "name" );
            PersonRule.AddRule( "carId", "carId" );
            PersonRule.AddRule( "insuranceId", "insuranceId" );

            MapRule CarRule = new MapRule( Model.FindByName( "Car" ), Schema.FindByName( "Person" ) );
            CarRule.AddRule( "name", "car.name" );
            CarRule.AddRule( "year", "car.year" );
            CarRule.AddRule( "engine", "carDetails.engine" );
            CarRule.AddRule( "fuel", "carDetails.fuel" );

            MapRule InsuranceRule = new MapRule( Model.FindByName( "Insurance" ), Schema.FindByName( "Insurance" ) );
            InsuranceRule.AddRule( "insuranceId", "_id" );
            InsuranceRule.AddRule( "name", "name" );

            MapRule RelationshipRule = new MapRule( Model.FindByName( "HasInsurance" ), Schema.FindByName( "Person" ) );
            RelationshipRule.AddRule( "insuranceValue", "insuranceValue" );

            ModelMapping Map = new ModelMapping( "PersonCarMap", new List<MapRule> { PersonRule, CarRule, InsuranceRule, RelationshipRule } );

            return new RequiredDataContainer( Model, Schema, Map );
        }
        /// <summary>
        /// Generates required data to test a One to One relationship
        /// joining one entity while the left side entity is embedded into another
        /// </summary>
        /// <returns></returns>
        public static RequiredDataContainer OneToOneLeftSideEmbedded()
        {
            Entity Car = new Entity( "Car" );
            Car.AddAttribute( "model" );
            Car.AddAttribute( "insuranceId" );

            Entity Insurance = new Entity( "Insurance" );
            Insurance.AddAttribute( "insuranceId" );
            Insurance.AddAttribute( "name" );
            Insurance.AddAttribute( "value" );

            Relationship HasInsurance = new Relationship( "HasInsurance", RelationshipCardinality.OneToOne );
            RelationshipConnection CarInsuranceConn = new RelationshipConnection(
                Car,
                Car.GetAttribute( "insuranceId" ),
                Insurance,
                Insurance.GetAttribute( "insuranceId" ) );

            HasInsurance.AddRelation( CarInsuranceConn );

            ERModel Model = new ERModel( "CarIns", new List<BaseERElement> { Car, Insurance, HasInsurance } );

            MongoDBCollection CarCol = new MongoDBCollection( "Car" );
            CarCol.DocumentSchema.AddAttribute( "model" );
            CarCol.DocumentSchema.AddAttribute( "insuranceId" );

            MongoDBCollection InsCol = new MongoDBCollection( "Insurance" );
            InsCol.DocumentSchema.AddAttribute( "insuranceId" );
            InsCol.DocumentSchema.AddAttribute( "name" );
            InsCol.DocumentSchema.AddAttribute( "value" );

            MongoSchema Schema = new MongoSchema( "CarInsSchema", new List<MongoDBCollection> { CarCol, InsCol } );

            MapRule CarRules = new MapRule( Car, CarCol );
            CarRules.AddRule( "model", "car.model" );
            CarRules.AddRule( "insuranceId", "car.insuranceId" );

            MapRule InsRules = new MapRule( Insurance, InsCol );
            InsRules.AddRule( "insuranceId", "_id" );
            InsRules.AddRule( "name", "name" );
            InsRules.AddRule( "value", "value" );

            ModelMapping Map = new ModelMapping( "CarInsModel" );
            Map.Rules.AddRange( new MapRule[] { CarRules, InsRules } );

            return new RequiredDataContainer( Model, Schema, Map );
        }
        #endregion
    }
}