﻿using QueryBuilder.ER;
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
            Person.AddAttribute( "personId" );
            Person.AddAttribute( "name" );

            Entity Car = new Entity( "Car" );
            Car.AddAttribute( "carId" );
            Car.AddAttribute( "name" );
            Car.AddAttribute( "year" );

            Relationship Insurance = new Relationship( "Insurance" );
            Insurance.AddAttribute( "insuranceId" );
            Insurance.AddAttribute( "personId" );
            Insurance.AddAttribute( "carId" );
            Insurance.AddAttribute( "companyId" );
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
            InsuranceRule.AddRule( "personId", "personId" );
            InsuranceRule.AddRule( "carId", "carId" );
            InsuranceRule.AddRule( "companyId", "companyId" );

            ModelMapping Map = new ModelMapping( "PersonCarMap", new List<MapRule> { PersonRule,
                CarRule, InsuranceRule} );

            return new RequiredDataContainer( Model, DBSchema, Map );
        }
        /// <summary>
        /// Generates data to test a Many To Many relationship joining multiple entities
        /// </summary>
        /// <returns></returns>
        public static RequiredDataContainer ManyToManyMultipleEntities()
        {
            Entity Person = new Entity( "Person" );
            Person.AddAttribute( "personId" );
            Person.AddAttribute( "name" );

            Entity Car = new Entity( "Car" );
            Car.AddAttribute( "carId" );
            Car.AddAttribute( "name" );
            Car.AddAttribute( "year" );

            Entity InsCompany = new Entity( "InsCompany" );
            InsCompany.AddAttribute( "companyId" );
            InsCompany.AddAttribute( "name" );

            Relationship Insurance = new Relationship( "Insurance" );
            Insurance.AddAttribute( "insuranceId" );
            Insurance.AddAttribute( "personId" );
            Insurance.AddAttribute( "carId" );
            Insurance.AddAttribute( "companyId" );
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
            InsuranceRule.AddRule( "personId", "personId" );
            InsuranceRule.AddRule( "carId", "carId" );
            InsuranceRule.AddRule( "companyId", "companyId" );

            MapRule InsCompanyRule = new MapRule( InsCompany, InsCompanyCol );
            InsCompanyRule.AddRule( "companyId", "_id" );
            InsCompanyRule.AddRule( "name", "name" );

            ModelMapping Map = new ModelMapping( "PersonCarMap", new List<MapRule> { PersonRule,
                CarRule, InsuranceRule, InsCompanyRule } );

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
            Person.AddAttribute( "personId" );
            Person.AddAttribute( "name" );

            Entity Car = new Entity( "Car" );
            Car.AddAttribute( "carId" );
            Car.AddAttribute( "name" );
            Car.AddAttribute( "year" );

            Relationship Insurance = new Relationship( "Insurance" );
            Insurance.AddAttribute( "insuranceId" );
            Insurance.AddAttribute( "personId" );
            Insurance.AddAttribute( "carId" );
            Insurance.AddAttribute( "companyId" );
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
            InsuranceRule.AddRule( "personId", "personId" );
            InsuranceRule.AddRule( "carId", "carId" );
            InsuranceRule.AddRule( "companyId", "companyId" );
            InsuranceRule.AddRule( "insuranceValue", "insuranceValue" );
            InsuranceRule.AddRule( "aRandomValue", "aRandomValue" );

            ModelMapping Map = new ModelMapping( "PersonCarMap", new List<MapRule> { PersonRule,
                CarRule, InsuranceRule} );

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
            Person.AddAttribute( "personId" );
            Person.AddAttribute( "name" );

            Entity Car = new Entity( "Car" );
            Car.AddAttribute( "carId" );
            Car.AddAttribute( "name" );
            Car.AddAttribute( "year" );

            Entity InsCompany = new Entity( "InsCompany" );
            InsCompany.AddAttribute( "companyId" );
            InsCompany.AddAttribute( "name" );

            Relationship Insurance = new Relationship( "Insurance" );
            Insurance.AddAttribute( "insuranceId" );
            Insurance.AddAttribute( "personId" );
            Insurance.AddAttribute( "carId" );
            Insurance.AddAttribute( "companyId" );
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
            InsuranceRule.AddRule( "personId", "personId" );
            InsuranceRule.AddRule( "carId", "carId" );
            InsuranceRule.AddRule( "companyId", "companyId" );
            InsuranceRule.AddRule( "insuranceValue", "insuranceValue" );
            InsuranceRule.AddRule( "aRandomValue", "aRandomValue" );

            MapRule InsCompanyRule = new MapRule( InsCompany, InsCompanyCol );
            InsCompanyRule.AddRule( "companyId", "_id" );
            InsCompanyRule.AddRule( "name", "name" );

            ModelMapping Map = new ModelMapping( "PersonCarMap", new List<MapRule> { PersonRule,
                CarRule, InsuranceRule, InsCompanyRule } );

            return new RequiredDataContainer( Model, DBSchema, Map );
        }
    }
}
