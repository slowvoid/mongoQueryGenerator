using QueryBuilder.ER;
using QueryBuilder.Map;
using QueryBuilder.Mongo;
using System;
using System.Collections.Generic;

namespace QueryBuilder.Tests
{
    /// <summary>
    /// Provides content for tests
    /// </summary>
    public static class ComputedEntityDataProvider
    {
        /// <summary>
        /// Generates data to test a RJOIN operation with a one to one relationship
        /// connecting to a composition of two other entities
        /// </summary>
        /// <returns></returns>
        public static RequiredDataContainer OneToOneComputedEntity()
        {
            Entity Person = new Entity( "Person" );
            Person.AddAttributes( "personId", "name", "carId" );

            Entity Car = new Entity( "Car" );
            Car.AddAttributes( "carId", "model", "year" );

            Entity Garage = new Entity( "Garage" );
            Garage.AddAttributes( "garageId", "name" );

            Relationship Drives = new Relationship( "Drives", RelationshipCardinality.OneToOne );
            RelationshipConnection PersonDrivesCar = new RelationshipConnection(
                Person,
                Person.GetAttribute( "carId" ),
                Car,
                Car.GetAttribute( "carId" ) );

            Drives.AddRelation( PersonDrivesCar );

            Relationship Repaired = new Relationship( "Repaired", RelationshipCardinality.ManyToMany );
            Repaired.AddAttributes( "repairedId", "carId", "garageId", "repaired" );
            RelationshipConnection CarRepairedByGarage = new RelationshipConnection(
                Car,
                Car.GetAttribute( "carId" ),
                Repaired.GetAttribute( "carId" ),
                Garage,
                Garage.GetAttribute( "garageId" ),
                Repaired.GetAttribute( "garageId" ) );
            Repaired.AddRelation( CarRepairedByGarage );

            ERModel Model = new ERModel( "ERModel", new List<BaseERElement> { Person, Car, Garage, Drives, Repaired } );

            // Mongo Schema
            MongoDBCollection PersonCol = new MongoDBCollection( "Person" );
            PersonCol.AddAttributes( "_id", "name", "carId" );

            MongoDBCollection CarCol = new MongoDBCollection( "Car" );
            CarCol.AddAttributes( "_id", "model", "year" );

            MongoDBCollection GarageCol = new MongoDBCollection( "Garage" );
            GarageCol.AddAttributes( "_id", "name" );

            MongoDBCollection RepairedCol = new MongoDBCollection( "Repaired" );
            RepairedCol.AddAttributes( "_id", "carId", "garageId", "repaired" );

            MongoSchema Schema = new MongoSchema( "Schema", new List<MongoDBCollection> { PersonCol, CarCol, GarageCol, RepairedCol } );

            // Map Rules
            MapRule PersonRule = new MapRule( Person, PersonCol );
            PersonRule.AddRule( "personId", "_id" );
            PersonRule.AddRule( "name", "name" );
            PersonRule.AddRule( "carId", "carId" );

            MapRule CarRule = new MapRule( Car, CarCol );
            CarRule.AddRule( "carId", "_id" );
            CarRule.AddRule( "model", "model" );
            CarRule.AddRule( "year", "year" );

            MapRule GarageRule = new MapRule( Garage, GarageCol );
            GarageRule.AddRule( "garageId", "_id" );
            GarageRule.AddRule( "name", "name" );

            MapRule RepairedRule = new MapRule( Repaired, RepairedCol );
            RepairedRule.AddRule( "repairedId", "_id" );
            RepairedRule.AddRule( "carId", "carId" );
            RepairedRule.AddRule( "garageId", "garageId" );
            RepairedRule.AddRule( "repaired", "repaired" );

            ModelMapping Map = new ModelMapping( "Map", new List<MapRule> { PersonRule, CarRule, GarageRule, RepairedRule } );

            return new RequiredDataContainer( Model, Schema, Map );
        }
        /// <summary>
        /// Generates data to test a RJOIN operation with a one to one relationship
        /// connecting to a composition of three other entities
        /// </summary>
        /// <returns></returns>
        public static RequiredDataContainer OneToOneComputedEntityMultiple()
        {
            Entity Person = new Entity( "Person" );
            Person.AddAttributes( "personId", "name", "carId" );

            Entity Car = new Entity( "Car" );
            Car.AddAttributes( "carId", "model", "year" );

            Entity Garage = new Entity( "Garage" );
            Garage.AddAttributes( "garageId", "name" );

            Entity Supplier = new Entity( "Supplier" );
            Supplier.AddAttributes( "supplierId", "name" );

            Relationship Drives = new Relationship( "Drives", RelationshipCardinality.OneToOne );
            RelationshipConnection PersonDrivesCar = new RelationshipConnection(
                Person,
                Person.GetAttribute( "carId" ),
                Car,
                Car.GetAttribute( "carId" ) );

            Drives.AddRelation( PersonDrivesCar );

            Relationship Repaired = new Relationship( "Repaired", RelationshipCardinality.ManyToMany );
            Repaired.AddAttributes( "repairedId", "carId", "garageId", "supplierId", "repaired" );
            RelationshipConnection CarRepairedByGarage = new RelationshipConnection(
                Car,
                Car.GetAttribute( "carId" ),
                Repaired.GetAttribute( "carId" ),
                Garage,
                Garage.GetAttribute( "garageId" ),
                Repaired.GetAttribute( "garageId" ) );
            Repaired.AddRelation( CarRepairedByGarage );

            RelationshipConnection CarRepairedSupplier = new RelationshipConnection(
                Car,
                Car.GetAttribute( "carId" ),
                Repaired.GetAttribute( "carId" ),
                Supplier,
                Supplier.GetAttribute( "supplierId" ),
                Repaired.GetAttribute( "supplierId" ) );
            Repaired.AddRelation( CarRepairedSupplier );

            ERModel Model = new ERModel( "ERModel", new List<BaseERElement> { Person, Car, Garage, Drives, Repaired, Supplier } );

            // Mongo Schema
            MongoDBCollection PersonCol = new MongoDBCollection( "Person" );
            PersonCol.AddAttributes( "_id", "name", "carId" );

            MongoDBCollection CarCol = new MongoDBCollection( "Car" );
            CarCol.AddAttributes( "_id", "model", "year" );

            MongoDBCollection GarageCol = new MongoDBCollection( "Garage" );
            GarageCol.AddAttributes( "_id", "name" );

            MongoDBCollection SupplierCol = new MongoDBCollection( "Supplier" );
            SupplierCol.AddAttributes( "_id", "name" );

            MongoDBCollection RepairedCol = new MongoDBCollection( "Repaired" );
            RepairedCol.AddAttributes( "_id", "carId", "garageId", "supplierId", "repaired" );

            MongoSchema Schema = new MongoSchema( "Schema", new List<MongoDBCollection> { PersonCol, CarCol, GarageCol, RepairedCol, SupplierCol } );

            // Map Rules
            MapRule PersonRule = new MapRule( Person, PersonCol );
            PersonRule.AddRule( "personId", "_id" );
            PersonRule.AddRule( "name", "name" );
            PersonRule.AddRule( "carId", "carId" );

            MapRule CarRule = new MapRule( Car, CarCol );
            CarRule.AddRule( "carId", "_id" );
            CarRule.AddRule( "model", "model" );
            CarRule.AddRule( "year", "year" );

            MapRule GarageRule = new MapRule( Garage, GarageCol );
            GarageRule.AddRule( "garageId", "_id" );
            GarageRule.AddRule( "name", "name" );

            MapRule SupplierRule = new MapRule( Supplier, SupplierCol );
            SupplierRule.AddRule( "supplierId", "_id" );
            SupplierRule.AddRule( "name", "name" );

            MapRule RepairedRule = new MapRule( Repaired, RepairedCol );
            RepairedRule.AddRule( "repairedId", "_id" );
            RepairedRule.AddRule( "carId", "carId" );
            RepairedRule.AddRule( "garageId", "garageId" );
            RepairedRule.AddRule( "supplierId", "supplierId" );
            RepairedRule.AddRule( "repaired", "repaired" );

            ModelMapping Map = new ModelMapping( "Map", new List<MapRule> { PersonRule, CarRule, GarageRule, RepairedRule, SupplierRule } );

            return new RequiredDataContainer( Model, Schema, Map );
        }

        /// <summary>
        /// Generates data to test a RJOIN operation with a one to one relationship
        /// connecting to a composition of three other entities
        /// </summary>
        /// <returns></returns>
        public static RequiredDataContainer OneToOneComputedEntityMultiple2()
        {
            Entity Person = new Entity( "Person" );
            Person.AddAttributes( "personId", "name", "carId", "insuranceId" );

            Entity Car = new Entity( "Car" );
            Car.AddAttributes( "carId", "model", "year" );

            Entity Garage = new Entity( "Garage" );
            Garage.AddAttributes( "garageId", "name" );

            Entity Supplier = new Entity( "Supplier" );
            Supplier.AddAttributes( "supplierId", "name" );

            Entity Insurance = new Entity( "Insurance" );
            Insurance.AddAttributes( "insuranceId", "name", "value" );

            Relationship Drives = new Relationship( "Drives", RelationshipCardinality.OneToOne );
            RelationshipConnection PersonDrivesCar = new RelationshipConnection(
                Person,
                Person.GetAttribute( "carId" ),
                Car,
                Car.GetAttribute( "carId" ) );

            Drives.AddRelation( PersonDrivesCar );

            Relationship Repaired = new Relationship( "Repaired", RelationshipCardinality.ManyToMany );
            Repaired.AddAttributes( "repairedId", "carId", "garageId", "supplierId", "repaired" );
            RelationshipConnection CarRepairedByGarage = new RelationshipConnection(
                Car,
                Car.GetAttribute( "carId" ),
                Repaired.GetAttribute( "carId" ),
                Garage,
                Garage.GetAttribute( "garageId" ),
                Repaired.GetAttribute( "garageId" ) );
            Repaired.AddRelation( CarRepairedByGarage );

            RelationshipConnection CarRepairedSupplier = new RelationshipConnection(
                Car,
                Car.GetAttribute( "carId" ),
                Repaired.GetAttribute( "carId" ),
                Supplier,
                Supplier.GetAttribute( "supplierId" ),
                Repaired.GetAttribute( "supplierId" ) );
            Repaired.AddRelation( CarRepairedSupplier );

            Relationship HasInsurance = new Relationship( "HasInsurance", RelationshipCardinality.OneToOne );
            RelationshipConnection PersonHasInsurance = new RelationshipConnection(
                Person,
                Person.GetAttribute( "insuranceId" ),
                Insurance,
                Insurance.GetAttribute( "insuranceId" ) );
            HasInsurance.AddRelation( PersonHasInsurance );

            ERModel Model = new ERModel( "ERModel", new List<BaseERElement> { Person, Car, Garage, Drives, Repaired, Supplier, Insurance, HasInsurance } );

            // Mongo Schema
            MongoDBCollection PersonCol = new MongoDBCollection( "Person" );
            PersonCol.AddAttributes( "_id", "name", "carId", "insuranceId" );

            MongoDBCollection CarCol = new MongoDBCollection( "Car" );
            CarCol.AddAttributes( "_id", "model", "year" );

            MongoDBCollection GarageCol = new MongoDBCollection( "Garage" );
            GarageCol.AddAttributes( "_id", "name" );

            MongoDBCollection SupplierCol = new MongoDBCollection( "Supplier" );
            SupplierCol.AddAttributes( "_id", "name" );

            MongoDBCollection RepairedCol = new MongoDBCollection( "Repaired" );
            RepairedCol.AddAttributes( "_id", "carId", "garageId", "supplierId", "repaired" );

            MongoDBCollection InsuranceCol = new MongoDBCollection( "Insurance" );
            InsuranceCol.AddAttributes( "_id", "name", "value" );

            MongoSchema Schema = new MongoSchema( "Schema", new List<MongoDBCollection> { PersonCol, CarCol, GarageCol, RepairedCol, SupplierCol, InsuranceCol } );

            // Map Rules
            MapRule PersonRule = new MapRule( Person, PersonCol );
            PersonRule.AddRule( "personId", "_id" );
            PersonRule.AddRule( "name", "name" );
            PersonRule.AddRule( "carId", "carId" );
            PersonRule.AddRule( "insuranceId", "insuranceId" );

            MapRule CarRule = new MapRule( Car, CarCol );
            CarRule.AddRule( "carId", "_id" );
            CarRule.AddRule( "model", "model" );
            CarRule.AddRule( "year", "year" );

            MapRule GarageRule = new MapRule( Garage, GarageCol );
            GarageRule.AddRule( "garageId", "_id" );
            GarageRule.AddRule( "name", "name" );

            MapRule SupplierRule = new MapRule( Supplier, SupplierCol );
            SupplierRule.AddRule( "supplierId", "_id" );
            SupplierRule.AddRule( "name", "name" );

            MapRule RepairedRule = new MapRule( Repaired, RepairedCol );
            RepairedRule.AddRule( "repairedId", "_id" );
            RepairedRule.AddRule( "carId", "carId" );
            RepairedRule.AddRule( "garageId", "garageId" );
            RepairedRule.AddRule( "supplierId", "supplierId" );
            RepairedRule.AddRule( "repaired", "repaired" );

            MapRule InsuranceRule = new MapRule( Insurance, InsuranceCol );
            InsuranceRule.AddRule( "insuranceId", "_id" );
            InsuranceRule.AddRule( "name", "name" );
            InsuranceRule.AddRule( "value", "value" );

            ModelMapping Map = new ModelMapping( "Map", new List<MapRule> { PersonRule, CarRule, GarageRule, RepairedRule, SupplierRule, InsuranceRule } );

            return new RequiredDataContainer( Model, Schema, Map );
        }
    }
}