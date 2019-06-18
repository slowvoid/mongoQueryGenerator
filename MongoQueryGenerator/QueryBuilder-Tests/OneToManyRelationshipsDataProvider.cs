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
        /// Generates required data for a OneToMany relationship without embedded documents
        /// </summary>
        /// <returns></returns>
        public static RequiredDataContainer OneToManyNotEmbedded()
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
        /// <summary>
        /// Generates required data for a OneToMany relationship with embedded documents
        /// </summary>
        /// <returns></returns>
        public static RequiredDataContainer OneToManyEmbedded()
        {
            // Create ER Stuff
            Entity Person = new Entity( "Person" );
            Person.AddAttribute( "personId" );
            Person.AddAttribute( "name" );

            Entity Car = new Entity( "Car" );
            Car.AddAttribute( "name" );
            Car.AddAttribute( "year" );

            Relationship Drives = new Relationship( "Drives", RelationshipCardinality.OneToMany );
            RelationshipConnection PersonCar = new RelationshipConnection(
                Person, Person.GetAttribute( "personId" ), Car, Car.GetAttribute( "personId" ) );
            Drives.AddRelation( PersonCar );

            ERModel Model = new ERModel( "PersonCarModel", new List<BaseERElement> { Person, Car, Drives } );

            // Create MongoDB schema
            MongoDBCollection PersonCollection = new MongoDBCollection( "PersonDrivesCars" );
            PersonCollection.DocumentSchema.AddAttribute( "_id" );
            PersonCollection.DocumentSchema.AddAttribute( "name" );
            PersonCollection.DocumentSchema.AddAttribute( "cars" );

            MongoSchema Schema = new MongoSchema( "PersonCarSchema", new List<MongoDBCollection> { PersonCollection } );

            // Create Map
            MapRule PersonRule = new MapRule( Model.FindByName( "Person" ), Schema.FindByName( "PersonDrivesCars" ) );
            PersonRule.AddRule( "personId", "_id" );
            PersonRule.AddRule( "name", "name" );

            MapRule CarRule = new MapRule( Model.FindByName( "Car" ), Schema.FindByName( "PersonDrivesCars" ) );
            CarRule.AddRule( "name", "cars.name" );
            CarRule.AddRule( "year", "cars.year" );

            ModelMapping Map = new ModelMapping( "PersonCarMap", new List<MapRule> { PersonRule, CarRule } );

            return new RequiredDataContainer( Model, Schema, Map );
        }
        /// <summary>
        /// Generates required data for a OneToMany relationship without embedded documents
        /// And with relationship attribute
        /// </summary>
        /// <returns></returns>
        public static RequiredDataContainer OneToManyRelationshipAttributes()
        {
            // Create ER Stuff
            Entity Person = new Entity( "Person" );
            Person.AddAttribute( "personId" );
            Person.AddAttribute( "name" );

            Entity Car = new Entity( "Car" );
            Car.AddAttribute( "carId" );
            Car.AddAttribute( "name" );
            Car.AddAttribute( "year" );
            Car.AddAttribute( "driverId" );

            Relationship Drives = new Relationship( "Drives", RelationshipCardinality.OneToMany );
            RelationshipConnection PersonCar = new RelationshipConnection(
                Person, Person.GetAttribute( "personId" ), Car, Car.GetAttribute( "driverId" ) );
            Drives.AddRelation( PersonCar );
            Drives.AddAttribute( "drivesFor" );

            ERModel Model = new ERModel( "PersonCarModel", new List<BaseERElement> { Person, Car, Drives } );

            // Create MongoDB schema
            MongoDBCollection PersonCollection = new MongoDBCollection( "Person" );
            PersonCollection.DocumentSchema.AddAttribute( "_id" );
            PersonCollection.DocumentSchema.AddAttribute( "name" );

            MongoDBCollection CarCollection = new MongoDBCollection( "Car" );
            CarCollection.DocumentSchema.AddAttribute( "_id" );
            CarCollection.DocumentSchema.AddAttribute( "name" );
            CarCollection.DocumentSchema.AddAttribute( "year" );
            CarCollection.DocumentSchema.AddAttribute( "driverId" );
            CarCollection.DocumentSchema.AddAttribute( "drivesFor" );

            MongoSchema Schema = new MongoSchema( "PersonCarSchema", new List<MongoDBCollection> { PersonCollection, CarCollection } );

            // Create Map
            MapRule PersonRule = new MapRule( Model.FindByName( "Person" ), Schema.FindByName( "Person" ) );
            PersonRule.AddRule( "personId", "_id" );
            PersonRule.AddRule( "name", "name" );

            MapRule CarRule = new MapRule( Model.FindByName( "Car" ), Schema.FindByName( "Car" ) );
            CarRule.AddRule( "carId", "_id" );
            CarRule.AddRule( "name", "name" );
            CarRule.AddRule( "year", "year" );
            CarRule.AddRule( "driverId", "driverId" );

            MapRule DrivesRule = new MapRule( Model.FindByName( "Drives" ), Schema.FindByName( "Car" ) );
            DrivesRule.AddRule( "drivesFor", "drivesFor" );

            ModelMapping Map = new ModelMapping( "PersonCarMap", new List<MapRule> { PersonRule, CarRule, DrivesRule } );

            return new RequiredDataContainer( Model, Schema, Map );
        }
        /// <summary>
        /// Generates required data for a OneToMany relationship with embedded documents
        /// And with relationship attribute
        /// </summary>
        /// <returns></returns>
        public static RequiredDataContainer OneToManyRelationshipAttributesEmbedded()
        {
            // Create ER Stuff
            Entity Person = new Entity( "Person" );
            Person.AddAttribute( "personId" );
            Person.AddAttribute( "name" );

            Entity Car = new Entity( "Car" );
            Car.AddAttribute( "name" );
            Car.AddAttribute( "year" );

            Relationship Drives = new Relationship( "Drives", RelationshipCardinality.OneToMany );
            RelationshipConnection PersonCar = new RelationshipConnection(
                Person, Person.GetAttribute( "personId" ), Car, Car.GetAttribute( "personId" ) );
            Drives.AddRelation( PersonCar );
            Drives.AddAttribute( "drivesFor" );

            ERModel Model = new ERModel( "PersonCarModel", new List<BaseERElement> { Person, Car, Drives } );

            // Create MongoDB schema
            MongoDBCollection PersonCollection = new MongoDBCollection( "Person" );
            PersonCollection.DocumentSchema.AddAttribute( "_id" );
            PersonCollection.DocumentSchema.AddAttribute( "name" );
            PersonCollection.DocumentSchema.AddAttribute( "cars" );

            MongoSchema Schema = new MongoSchema( "PersonCarSchema", new List<MongoDBCollection> { PersonCollection } );

            // Create Map
            MapRule PersonRule = new MapRule( Model.FindByName( "Person" ), Schema.FindByName( "Person" ) );
            PersonRule.AddRule( "personId", "_id" );
            PersonRule.AddRule( "name", "name" );

            MapRule CarRule = new MapRule( Model.FindByName( "Car" ), Schema.FindByName( "Person" ) );
            CarRule.AddRule( "name", "cars.name" );
            CarRule.AddRule( "year", "cars.year" );

            MapRule RelationshipRule = new MapRule( Model.FindByName( "Drives" ), Schema.FindByName( "Person" ) );
            RelationshipRule.AddRule( "drivesFor", "cars.drivesFor" );

            ModelMapping Map = new ModelMapping( "PersonCarMap", new List<MapRule> { PersonRule, CarRule, RelationshipRule } );

            return new RequiredDataContainer( Model, Schema, Map );
        }
        #endregion
    }
}
