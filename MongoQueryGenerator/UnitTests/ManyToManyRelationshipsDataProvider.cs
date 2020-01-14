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
    /// Provides data to test ManyToMany relationships
    /// </summary>
    public static class ManyToManyRelationshipsDataProvider
    {
        /// <summary>
        /// Generates data to test a Many To Many relationship joining only one entity
        /// </summary>
        /// <returns></returns>
        public static RequiredDataContainer ManyToManySingleEntity()
        {
            // ER Model
            Entity Person = new Entity( "Person" );
            Person.AddAttribute( "personId", true );
            Person.AddAttribute( "name" );

            Entity Car = new Entity( "Car" );
            Car.AddAttribute( "carId", true );
            Car.AddAttribute( "name" );
            Car.AddAttribute( "year" );

            Relationship Insurance = new Relationship( "Insurance" );
            Insurance.AddAttribute( "insuranceId" );
            Insurance.AddRelationshipEnd( new RelationshipEnd( Person ) );
            Insurance.AddRelationshipEnd( new RelationshipEnd( Car ) );

            ERModel Model = new ERModel( "PersonCar", new List<BaseERElement> { Person, Car, Insurance } );

            // MongoDB Schema
            MongoDBCollection PersonCol = new MongoDBCollection( "Person" );
            PersonCol.DocumentSchema.AddAttribute( "_id" );
            PersonCol.DocumentSchema.AddAttribute( "name" );

            MongoDBCollection CarCol = new MongoDBCollection( "Car" );
            CarCol.DocumentSchema.AddAttribute( "_id" );
            CarCol.DocumentSchema.AddAttribute( "name" );
            CarCol.DocumentSchema.AddAttribute( "year" );

            MongoDBCollection InsuranceCol = new MongoDBCollection( "Insurance" );
            InsuranceCol.DocumentSchema.AddAttribute( "_id" );

            MongoSchema DBSchema = new MongoSchema( "PersonCarSchema",
                new List<MongoDBCollection> { PersonCol, CarCol, InsuranceCol } );

            // Map
            MapRule PersonRule = new MapRule( Person, PersonCol );
            PersonRule.AddRule( "personId", "_id" );
            PersonRule.AddRule( "name", "name" );

            MapRule CarRule = new MapRule( Car, CarCol );
            CarRule.AddRule( "carId", "_id" );
            CarRule.AddRule( "name", "name" );
            CarRule.AddRule( "year", "year" );

            MapRule InsuranceRule = new MapRule( Insurance, InsuranceCol );
            InsuranceRule.AddRule( "insuranceId", "_id" );

            MapRule PersonInsuranceRule = new MapRule( Person, InsuranceCol, false );
            PersonInsuranceRule.AddRule( "personId", "personId" );

            MapRule CarInsuranceRule = new MapRule( Car, InsuranceCol, false );
            CarInsuranceRule.AddRule( "carId", "carId" );

            ModelMapping Map = new ModelMapping( "PersonCarMap", new List<MapRule> { PersonRule,
                CarRule, InsuranceRule, PersonInsuranceRule, CarInsuranceRule } );

            return new RequiredDataContainer( Model, DBSchema, Map );
        }
        /// <summary>
        /// Generates data to test a Many To Many relationship joining multiple entities
        /// </summary>
        /// <returns></returns>
        public static RequiredDataContainer ManyToManyMultipleEntities()
        {
            Entity Person = new Entity( "Person" );
            Person.AddAttribute( "personId", true );
            Person.AddAttribute( "name" );

            Entity Car = new Entity( "Car" );
            Car.AddAttribute( "carId", true );
            Car.AddAttribute( "name" );
            Car.AddAttribute( "year" );

            Entity InsCompany = new Entity( "InsCompany" );
            InsCompany.AddAttribute( "companyId", true );
            InsCompany.AddAttribute( "name" );

            Relationship Insurance = new Relationship( "Insurance" );
            Insurance.AddAttribute( "insuranceId" );
            Insurance.AddRelationshipEnd( new RelationshipEnd( Person ) );
            Insurance.AddRelationshipEnd( new RelationshipEnd( Car ) );
            Insurance.AddRelationshipEnd( new RelationshipEnd( InsCompany ) );

            ERModel Model = new ERModel( "PersonCar", new List<BaseERElement> { Person, Car, Insurance, InsCompany } );

            // MongoDB Schema
            MongoDBCollection PersonCol = new MongoDBCollection( "Person" );
            PersonCol.DocumentSchema.AddAttribute( "_id" );
            PersonCol.DocumentSchema.AddAttribute( "name" );

            MongoDBCollection CarCol = new MongoDBCollection( "Car" );
            CarCol.DocumentSchema.AddAttribute( "_id" );
            CarCol.DocumentSchema.AddAttribute( "name" );
            CarCol.DocumentSchema.AddAttribute( "year" );

            MongoDBCollection InsuranceCol = new MongoDBCollection( "Insurance" );
            InsuranceCol.DocumentSchema.AddAttribute( "_id" );
            InsuranceCol.DocumentSchema.AddAttribute( "personId" );
            InsuranceCol.DocumentSchema.AddAttribute( "carId" );
            InsuranceCol.DocumentSchema.AddAttribute( "companyId" );

            MongoDBCollection InsCompanyCol = new MongoDBCollection( "InsCompany" );
            InsCompanyCol.DocumentSchema.AddAttribute( "_id" );
            InsCompanyCol.DocumentSchema.AddAttribute( "name" );

            MongoSchema DBSchema = new MongoSchema( "PersonCarSchema",
                new List<MongoDBCollection> { PersonCol, CarCol, InsuranceCol, InsCompanyCol } );

            // Map
            MapRule PersonRule = new MapRule( Person, PersonCol );
            PersonRule.AddRule( "personId", "_id" );
            PersonRule.AddRule( "name", "name" );

            MapRule CarRule = new MapRule( Car, CarCol );
            CarRule.AddRule( "carId", "_id" );
            CarRule.AddRule( "name", "name" );
            CarRule.AddRule( "year", "year" );

            MapRule InsuranceRule = new MapRule( Insurance, InsuranceCol );
            InsuranceRule.AddRule( "insuranceId", "_id" );

            MapRule InsCompanyRule = new MapRule( InsCompany, InsCompanyCol );
            InsCompanyRule.AddRule( "companyId", "_id" );
            InsCompanyRule.AddRule( "name", "name" );

            MapRule PersonInsuranceRule = new MapRule( Person, InsuranceCol, false );
            PersonInsuranceRule.AddRule( "personId", "personId" );

            MapRule CarInsuranceRule = new MapRule( Car, InsuranceCol, false );
            CarInsuranceRule.AddRule( "carId", "carId" );

            MapRule InsCompanyInsuraceRule = new MapRule( InsCompany, InsuranceCol, false );
            InsCompanyInsuraceRule.AddRule( "companyId", "companyId" );

            ModelMapping Map = new ModelMapping( "PersonCarMap", new List<MapRule> { PersonRule,
                CarRule, InsuranceRule, InsCompanyRule, PersonInsuranceRule, CarInsuranceRule, InsCompanyInsuraceRule } );

            return new RequiredDataContainer( Model, DBSchema, Map );
        }
        /// <summary>
        /// Generates data to test a Many to Many relationship join with a single entity
        /// including relationship attributes
        /// </summary>
        /// <returns></returns>
        public static RequiredDataContainer ManyToManyRelationshipAttributeSingleEntity()
        {
            // ER Model
            Entity Person = new Entity( "Person" );
            Person.AddAttribute( "personId", true );
            Person.AddAttribute( "name" );

            Entity Car = new Entity( "Car" );
            Car.AddAttribute( "carId", true );
            Car.AddAttribute( "name" );
            Car.AddAttribute( "year" );

            Relationship Insurance = new Relationship( "Insurance" );
            Insurance.AddAttribute( "insuranceId" );
            Insurance.AddAttribute( "insuranceValue" );
            Insurance.AddAttribute( "aRandomValue" );

            Insurance.AddRelationshipEnd( new RelationshipEnd( Person ) );
            Insurance.AddRelationshipEnd( new RelationshipEnd( Car ) );

            ERModel Model = new ERModel( "PersonCar", new List<BaseERElement> { Person, Car, Insurance } );

            // MongoDB Schema
            MongoDBCollection PersonCol = new MongoDBCollection( "Person" );
            PersonCol.DocumentSchema.AddAttribute( "_id" );
            PersonCol.DocumentSchema.AddAttribute( "name" );

            MongoDBCollection CarCol = new MongoDBCollection( "Car" );
            CarCol.DocumentSchema.AddAttribute( "_id" );
            CarCol.DocumentSchema.AddAttribute( "name" );
            CarCol.DocumentSchema.AddAttribute( "year" );

            MongoDBCollection InsuranceCol = new MongoDBCollection( "Insurance" );
            InsuranceCol.DocumentSchema.AddAttribute( "_id" );
            InsuranceCol.DocumentSchema.AddAttribute( "personId" );
            InsuranceCol.DocumentSchema.AddAttribute( "carId" );
            InsuranceCol.DocumentSchema.AddAttribute( "companyId" );
            InsuranceCol.DocumentSchema.AddAttribute( "insuranceValue" );
            InsuranceCol.DocumentSchema.AddAttribute( "aRandomValue" );

            MongoSchema DBSchema = new MongoSchema( "PersonCarSchema",
                new List<MongoDBCollection> { PersonCol, CarCol, InsuranceCol } );

            // Map
            MapRule PersonRule = new MapRule( Person, PersonCol );
            PersonRule.AddRule( "personId", "_id" );
            PersonRule.AddRule( "name", "name" );

            MapRule CarRule = new MapRule( Car, CarCol );
            CarRule.AddRule( "carId", "_id" );
            CarRule.AddRule( "name", "name" );
            CarRule.AddRule( "year", "year" );

            MapRule InsuranceRule = new MapRule( Insurance, InsuranceCol );
            InsuranceRule.AddRule( "insuranceId", "_id" );
            InsuranceRule.AddRule( "insuranceValue", "insuranceValue" );
            InsuranceRule.AddRule( "aRandomValue", "aRandomValue" );

            MapRule PersonInsuranceRule = new MapRule( Person, InsuranceCol, false );
            PersonInsuranceRule.AddRule( "personId", "personId" );

            MapRule CarInsuranceRule = new MapRule( Car, InsuranceCol, false );
            CarInsuranceRule.AddRule( "carId", "carId" );

            ModelMapping Map = new ModelMapping( "PersonCarMap", new List<MapRule> { PersonRule,
                CarRule, InsuranceRule, PersonInsuranceRule, CarInsuranceRule } );

            return new RequiredDataContainer( Model, DBSchema, Map );
        }
        /// <summary>
        /// Generates data to test a many to many relationship join with multiple entities
        /// and relationship attributes
        /// </summary>
        /// <returns></returns>
        public static RequiredDataContainer ManyToManyRelationshipAttributeMultipleEntities()
        {
            Entity Person = new Entity( "Person" );
            Person.AddAttribute( "personId", true );
            Person.AddAttribute( "name" );

            Entity Car = new Entity( "Car" );
            Car.AddAttribute( "carId", true );
            Car.AddAttribute( "name" );
            Car.AddAttribute( "year" );

            Entity InsCompany = new Entity( "InsCompany" );
            InsCompany.AddAttribute( "companyId", true );
            InsCompany.AddAttribute( "name" );

            Relationship Insurance = new Relationship( "Insurance" );
            Insurance.AddAttribute( "insuranceId" );
            Insurance.AddAttribute( "insuranceValue" );
            Insurance.AddAttribute( "aRandomValue" );
            Insurance.AddRelationshipEnd( new RelationshipEnd( Person ) );
            Insurance.AddRelationshipEnd( new RelationshipEnd( Car ) );
            Insurance.AddRelationshipEnd( new RelationshipEnd( InsCompany ) );

            ERModel Model = new ERModel( "PersonCar", new List<BaseERElement> { Person, Car, Insurance, InsCompany } );

            // MongoDB Schema
            MongoDBCollection PersonCol = new MongoDBCollection( "Person" );
            PersonCol.DocumentSchema.AddAttribute( "_id" );
            PersonCol.DocumentSchema.AddAttribute( "name" );

            MongoDBCollection CarCol = new MongoDBCollection( "Car" );
            CarCol.DocumentSchema.AddAttribute( "_id" );
            CarCol.DocumentSchema.AddAttribute( "name" );
            CarCol.DocumentSchema.AddAttribute( "year" );

            MongoDBCollection InsuranceCol = new MongoDBCollection( "Insurance" );
            InsuranceCol.DocumentSchema.AddAttribute( "_id" );
            InsuranceCol.DocumentSchema.AddAttribute( "personId" );
            InsuranceCol.DocumentSchema.AddAttribute( "carId" );
            InsuranceCol.DocumentSchema.AddAttribute( "companyId" );
            InsuranceCol.DocumentSchema.AddAttribute( "insuranceValue" );
            InsuranceCol.DocumentSchema.AddAttribute( "aRandomValue" );

            MongoDBCollection InsCompanyCol = new MongoDBCollection( "InsCompany" );
            InsCompanyCol.DocumentSchema.AddAttribute( "_id" );
            InsCompanyCol.DocumentSchema.AddAttribute( "name" );

            MongoSchema DBSchema = new MongoSchema( "PersonCarSchema",
                new List<MongoDBCollection> { PersonCol, CarCol, InsuranceCol, InsCompanyCol } );

            // Map
            MapRule PersonRule = new MapRule( Person, PersonCol );
            PersonRule.AddRule( "personId", "_id" );
            PersonRule.AddRule( "name", "name" );

            MapRule CarRule = new MapRule( Car, CarCol );
            CarRule.AddRule( "carId", "_id" );
            CarRule.AddRule( "name", "name" );
            CarRule.AddRule( "year", "year" );

            MapRule InsuranceRule = new MapRule( Insurance, InsuranceCol );
            InsuranceRule.AddRule( "insuranceId", "_id" );
            InsuranceRule.AddRule( "insuranceValue", "insuranceValue" );
            InsuranceRule.AddRule( "aRandomValue", "aRandomValue" );

            MapRule InsCompanyRule = new MapRule( InsCompany, InsCompanyCol );
            InsCompanyRule.AddRule( "companyId", "_id" );
            InsCompanyRule.AddRule( "name", "name" );

            MapRule PersonInsuranceRule = new MapRule( Person, InsuranceCol, false );
            PersonInsuranceRule.AddRule( "personId", "personId" );

            MapRule CarInsuranceRule = new MapRule( Car, InsuranceCol, false );
            CarInsuranceRule.AddRule( "carId", "carId" );

            MapRule InsCompanyInsuranceRule = new MapRule( InsCompany, InsuranceCol, false );
            InsCompanyInsuranceRule.AddRule( "companyId", "companyId" );

            ModelMapping Map = new ModelMapping( "PersonCarMap", new List<MapRule> { PersonRule,
                CarRule, InsuranceRule, InsCompanyRule, PersonInsuranceRule, CarInsuranceRule, InsCompanyInsuranceRule } );

            return new RequiredDataContainer( Model, DBSchema, Map );
        }
        /// <summary>
        /// Generates data to test a query in which the target entity is embedded in the middle collection
        /// </summary>
        /// <returns></returns>
        public static RequiredDataContainer ManyToManyEmbeddedTarget()
        {
            // ER Model
            Entity Person = new Entity( "Person" );
            Person.AddAttribute( "personId", true );
            Person.AddAttributes( "name", "age" );

            Entity Car = new Entity( "Car" );
            Car.AddAttribute( "carId", true );
            Car.AddAttributes( "model", "year" );

            Relationship Drives = new Relationship( "Drives" );
            Drives.AddAttribute( "something" );

            Drives.AddRelationshipEnd( new RelationshipEnd( Person ) );
            Drives.AddRelationshipEnd( new RelationshipEnd( Car ) );

            ERModel Model = new ERModel( "Model", new List<BaseERElement>() { Person, Car, Drives } );

            // Mongo Schema
            MongoDBCollection PersonCol = new MongoDBCollection( "Person" );
            PersonCol.AddAttributes( "_id", "name", "age" );

            MongoDBCollection DrivesCol = new MongoDBCollection( "Drives" );
            DrivesCol.AddAttributes( "_id", "something", "car.carId", "car.model", "car.year" );

            MongoSchema Schema = new MongoSchema( "Schema", new List<MongoDBCollection>() { PersonCol, DrivesCol } );

            // Map
            MapRule PersonRule = new MapRule( Person, PersonCol );
            PersonRule.AddRule( "personId", "_id" );
            PersonRule.AddRule( "name", "name" );
            PersonRule.AddRule( "age", "age" );

            MapRule CarRule = new MapRule( Car, DrivesCol, false );
            CarRule.AddRule( "carId", "car.carId" );
            CarRule.AddRule( "model", "car.model" );
            CarRule.AddRule( "year", "car.year" );

            MapRule DrivesRule = new MapRule( Drives, DrivesCol );
            DrivesRule.AddRule( "something", "something" );

            MapRule PersonDrivesRule = new MapRule( Person, DrivesCol, false );
            PersonDrivesRule.AddRule( "personId", "personId" );

            ModelMapping Map = new ModelMapping( "Map", new List<MapRule>() { PersonRule, CarRule, DrivesRule, PersonDrivesRule } );

            return new RequiredDataContainer( Model, Schema, Map );
        }
    }
}
