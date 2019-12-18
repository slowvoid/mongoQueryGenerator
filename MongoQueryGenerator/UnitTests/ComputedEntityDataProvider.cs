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
            Person.AddAttribute( "personId", true );
            Person.AddAttributes( "name" );

            Entity Car = new Entity( "Car" );
            Car.AddAttribute( "carId", true );
            Car.AddAttributes( "model", "year" );

            Entity Garage = new Entity( "Garage" );
            Garage.AddAttribute( "garageId", true );
            Garage.AddAttributes( "name" );

            Relationship Drives = new Relationship( "Drives" );
            Drives.AddRelationshipEnd( new RelationshipEnd( Person ) );
            Drives.AddRelationshipEnd( new RelationshipEnd( Car ) );

            Relationship Repaired = new Relationship( "Repaired" );
            Repaired.AddRelationshipEnd( new RelationshipEnd( Car ) );
            Repaired.AddRelationshipEnd( new RelationshipEnd( Garage ) );
            Repaired.AddAttribute( "repairedId", true );
            Repaired.AddAttributes( "repaired" );

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

            MapRule CarRule = new MapRule( Car, CarCol );
            CarRule.AddRule( "carId", "_id" );
            CarRule.AddRule( "model", "model" );
            CarRule.AddRule( "year", "year" );

            MapRule GarageRule = new MapRule( Garage, GarageCol );
            GarageRule.AddRule( "garageId", "_id" );
            GarageRule.AddRule( "name", "name" );

            MapRule RepairedRule = new MapRule( Repaired, RepairedCol );
            RepairedRule.AddRule( "repairedId", "_id" );
            RepairedRule.AddRule( "repaired", "repaired" );

            MapRule CarPersonRule = new MapRule( Car, PersonCol, false );
            CarPersonRule.AddRule( "carId", "carId" );

            MapRule CarRepairedRule = new MapRule( Car, RepairedCol, false );
            CarRepairedRule.AddRule( "carId", "carId" );

            MapRule GarageRepairedRule = new MapRule( Garage, RepairedCol, false );
            GarageRepairedRule.AddRule( "garageId", "garageId" );

            ModelMapping Map = new ModelMapping( "Map", new List<MapRule> { PersonRule, CarRule, GarageRule, RepairedRule, CarPersonRule, CarRepairedRule, GarageRepairedRule } );

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
            Person.AddAttribute( "personId", true );
            Person.AddAttributes( "name" );

            Entity Car = new Entity( "Car" );
            Car.AddAttribute( "carId", true );
            Car.AddAttributes( "model", "year" );

            Entity Garage = new Entity( "Garage" );
            Garage.AddAttribute( "garageId", true );
            Garage.AddAttributes( "name" );

            Entity Supplier = new Entity( "Supplier" );
            Supplier.AddAttribute( "supplierId", true );
            Supplier.AddAttributes( "name" );

            Relationship Drives = new Relationship( "Drives" );
            Drives.AddRelationshipEnd( new RelationshipEnd( Person ) );
            Drives.AddRelationshipEnd( new RelationshipEnd( Car ) );

            Relationship Repaired = new Relationship( "Repaired" );
            Repaired.AddRelationshipEnd( new RelationshipEnd( Car ) );
            Repaired.AddRelationshipEnd( new RelationshipEnd( Garage ) );
            Repaired.AddRelationshipEnd( new RelationshipEnd( Supplier ) );
            Repaired.AddAttributes( "repairedId", "repaired" );

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
            RepairedRule.AddRule( "repaired", "repaired" );

            MapRule CarPersonRule = new MapRule( Car, PersonCol, false );
            CarPersonRule.AddRule( "carId", "carId" );

            MapRule CarRepairedRule = new MapRule( Car, RepairedCol, false );
            CarRepairedRule.AddRule( "carId", "carId" );

            MapRule GarageRepairedRule = new MapRule( Garage, RepairedCol, false );
            GarageRepairedRule.AddRule( "garageId", "garageId" );

            MapRule SupplierRepairedRule = new MapRule( Supplier, RepairedCol, false );
            SupplierRepairedRule.AddRule( "supplierId", "supplierId" );

            ModelMapping Map = new ModelMapping( "Map", new List<MapRule> { PersonRule, CarRule, GarageRule, RepairedRule, SupplierRule, CarPersonRule, CarRepairedRule, GarageRepairedRule, SupplierRepairedRule } );

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

            Relationship Drives = new Relationship( "Drives" );
            Drives.AddRelationshipEnd( new RelationshipEnd( Person ) );
            Drives.AddRelationshipEnd( new RelationshipEnd( Car ) );

            Relationship Repaired = new Relationship( "Repaired" );
            Repaired.AddAttributes( "repairedId", "carId", "garageId", "supplierId", "repaired" );
            Repaired.AddRelationshipEnd( new RelationshipEnd( Car ) );
            Repaired.AddRelationshipEnd( new RelationshipEnd( Garage ) );
            Repaired.AddRelationshipEnd( new RelationshipEnd( Supplier ) );;

            Relationship HasInsurance = new Relationship( "HasInsurance" );
            HasInsurance.AddRelationshipEnd( new RelationshipEnd( Person ) );
            HasInsurance.AddRelationshipEnd( new RelationshipEnd( Insurance ) );

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
        /// <summary>
        /// Data for a computed entity test starting from a one to many relationship
        /// </summary>
        /// <returns></returns>
        public static RequiredDataContainer OneToManyComputedEntity()
        {
            Entity Person = new Entity( "Person" );
            Person.AddAttributes( "personId", "name", "insuranceId" );

            Entity Car = new Entity( "Car" );
            Car.AddAttributes( "carId", "model", "year", "driverId" );

            Entity Garage = new Entity( "Garage" );
            Garage.AddAttributes( "garageId", "name" );

            Entity Supplier = new Entity( "Supplier" );
            Supplier.AddAttributes( "supplierId", "name" );

            Entity Insurance = new Entity( "Insurance" );
            Insurance.AddAttributes( "insuranceId", "name", "value" );

            Relationship Drives = new Relationship( "Drives" );
            Drives.AddRelationshipEnd( new RelationshipEnd( Person ) );
            Drives.AddRelationshipEnd( new RelationshipEnd( Car ) );

            Relationship Repaired = new Relationship( "Repaired" );
            Repaired.AddAttributes( "repairedId", "carId", "garageId", "supplierId", "repaired" );
            Repaired.AddRelationshipEnd( new RelationshipEnd( Car ) );
            Repaired.AddRelationshipEnd( new RelationshipEnd( Garage ) );
            Repaired.AddRelationshipEnd( new RelationshipEnd( Supplier ) ); ;

            Relationship HasInsurance = new Relationship( "HasInsurance" );
            HasInsurance.AddRelationshipEnd( new RelationshipEnd( Person ) );
            HasInsurance.AddRelationshipEnd( new RelationshipEnd( Insurance ) );

            ERModel Model = new ERModel( "ERModel", new List<BaseERElement> { Person, Car, Garage, Drives, Repaired, Supplier, Insurance, HasInsurance } );

            // Mongo Schema
            MongoDBCollection PersonCol = new MongoDBCollection( "Person" );
            PersonCol.AddAttributes( "_id", "name", "insuranceId" );

            MongoDBCollection CarCol = new MongoDBCollection( "Car" );
            CarCol.AddAttributes( "_id", "model", "year", "driverId" );

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
            PersonRule.AddRule( "insuranceId", "insuranceId" );

            MapRule CarRule = new MapRule( Car, CarCol );
            CarRule.AddRule( "carId", "_id" );
            CarRule.AddRule( "model", "model" );
            CarRule.AddRule( "year", "year" );
            CarRule.AddRule( "driverId", "driverId" );

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
        /// <summary>
        /// Generate data to test a computed entity from a many to many relationship
        /// </summary>
        /// <returns></returns>
        public static RequiredDataContainer ManyToManyComputedEntity()
        {
            Entity Person = new Entity( "Person" );
            Person.AddAttributes( "personId", "name" );
            Person.SetIdentifier( "personId" );

            Entity Car = new Entity( "Car" );
            Car.AddAttributes( "carId", "model", "year" );
            Car.SetIdentifier( "carId" );

            Entity Garage = new Entity( "Garage" );
            Garage.AddAttributes( "garageId", "name" );
            Garage.SetIdentifier( "garageId" );

            Entity Supplier = new Entity( "Supplier" );
            Supplier.AddAttributes( "supplierId", "name" );
            Supplier.SetIdentifier( "supplierId" );

            Entity Insurance = new Entity( "Insurance" );
            Insurance.AddAttributes( "insuranceId", "name", "value" );
            Insurance.AddAttribute( "insuranceId" );

            Relationship Owns = new Relationship( "Owns" );
            Owns.AddRelationshipEnd( new RelationshipEnd( Person ) );
            Owns.AddRelationshipEnd( new RelationshipEnd( Car ) );
            Owns.AddRelationshipEnd( new RelationshipEnd( Insurance ) );
            Owns.AddAttributes( "ownsId", "personId", "carId", "insuranceId" );

            Relationship Repaired = new Relationship( "Repaired" );
            Repaired.AddAttributes( "repairedId", "carId", "garageId", "supplierId", "repaired" );
            Repaired.AddRelationshipEnd( new RelationshipEnd( Car ) );
            Repaired.AddRelationshipEnd( new RelationshipEnd( Garage ) );
            Repaired.AddRelationshipEnd( new RelationshipEnd( Supplier ) ); ;

            ERModel Model = new ERModel( "ERModel", new List<BaseERElement> { Person, Car, Garage, Repaired, Supplier, Insurance, Owns } );

            // Mongo Schema
            MongoDBCollection PersonCol = new MongoDBCollection( "Person" );
            PersonCol.AddAttributes( "_id", "name", "insuranceId" );

            MongoDBCollection CarCol = new MongoDBCollection( "Car" );
            CarCol.AddAttributes( "_id", "model", "year", "driverId" );

            MongoDBCollection GarageCol = new MongoDBCollection( "Garage" );
            GarageCol.AddAttributes( "_id", "name" );

            MongoDBCollection SupplierCol = new MongoDBCollection( "Supplier" );
            SupplierCol.AddAttributes( "_id", "name" );

            MongoDBCollection RepairedCol = new MongoDBCollection( "Repaired" );
            RepairedCol.AddAttributes( "_id", "carId", "garageId", "supplierId", "repaired" );

            MongoDBCollection InsuranceCol = new MongoDBCollection( "Insurance" );
            InsuranceCol.AddAttributes( "_id", "name", "value" );

            MongoDBCollection OwnsCol = new MongoDBCollection( "Owns" );
            OwnsCol.AddAttributes( "_id", "personId", "carId", "insuranceId" );

            MongoSchema Schema = new MongoSchema( "Schema", new List<MongoDBCollection> { PersonCol, CarCol, GarageCol, RepairedCol, SupplierCol, InsuranceCol, OwnsCol } );

            // Map Rules
            MapRule PersonRule = new MapRule( Person, PersonCol );
            PersonRule.AddRule( "personId", "_id" );
            PersonRule.AddRule( "name", "name" );

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

            MapRule OwnsRule = new MapRule( Owns, OwnsCol );
            OwnsRule.AddRule( "ownsId", "_id" );
            OwnsRule.AddRule( "personId", "personId" );
            OwnsRule.AddRule( "carId", "carId" );
            OwnsRule.AddRule( "insuranceId", "insuranceId" );

            ModelMapping Map = new ModelMapping( "Map", new List<MapRule> { PersonRule, CarRule, GarageRule, RepairedRule, SupplierRule, InsuranceRule, OwnsRule } );

            return new RequiredDataContainer( Model, Schema, Map );
        }
        /// <summary>
        /// Generate data to test a computed entity from a many to many relationship
        /// </summary>
        /// <returns></returns>
        public static RequiredDataContainer ManyToManyComputedEntity2()
        {
            Entity Person = new Entity( "Person" );
            Person.AddAttributes( "personId", "name" );

            Entity Car = new Entity( "Car" );
            Car.AddAttributes( "carId", "model", "year", "manufacturerId" );

            Entity Garage = new Entity( "Garage" );
            Garage.AddAttributes( "garageId", "name" );

            Entity Supplier = new Entity( "Supplier" );
            Supplier.AddAttributes( "supplierId", "name" );

            Entity Insurance = new Entity( "Insurance" );
            Insurance.AddAttributes( "insuranceId", "name", "value" );

            Entity Manufacturer = new Entity( "Manufacturer" );
            Manufacturer.AddAttributes( "manufacturerId", "name" );

            Relationship Owns = new Relationship( "Owns" );
            Owns.AddAttributes( "ownsId", "personId", "carId", "insuranceId" );
            Owns.AddRelationshipEnd( new RelationshipEnd( Person ) );
            Owns.AddRelationshipEnd( new RelationshipEnd( Car ) );
            Owns.AddRelationshipEnd( new RelationshipEnd( Insurance ) );

            Relationship Repaired = new Relationship( "Repaired" );
            Repaired.AddAttributes( "repairedId", "carId", "garageId", "supplierId", "repaired" );
            Repaired.AddRelationshipEnd( new RelationshipEnd( Car ) );
            Repaired.AddRelationshipEnd( new RelationshipEnd( Garage ) );
            Repaired.AddRelationshipEnd( new RelationshipEnd( Supplier ) );

            Relationship ManufacturedBy = new Relationship( "ManufacturedBy" );
            ManufacturedBy.AddRelationshipEnd( new RelationshipEnd( Car ) );
            ManufacturedBy.AddRelationshipEnd( new RelationshipEnd( Manufacturer ) );

            ERModel Model = new ERModel( "ERModel", new List<BaseERElement> { Person, Car, Garage, Repaired, Supplier, Insurance, Owns, Manufacturer, ManufacturedBy } );

            // Mongo Schema
            MongoDBCollection PersonCol = new MongoDBCollection( "Person" );
            PersonCol.AddAttributes( "_id", "name" );

            MongoDBCollection CarCol = new MongoDBCollection( "Car" );
            CarCol.AddAttributes( "_id", "model", "year", "manufacturerId" );

            MongoDBCollection GarageCol = new MongoDBCollection( "Garage" );
            GarageCol.AddAttributes( "_id", "name" );

            MongoDBCollection SupplierCol = new MongoDBCollection( "Supplier" );
            SupplierCol.AddAttributes( "_id", "name" );

            MongoDBCollection RepairedCol = new MongoDBCollection( "Repaired" );
            RepairedCol.AddAttributes( "_id", "carId", "garageId", "supplierId", "repaired" );

            MongoDBCollection InsuranceCol = new MongoDBCollection( "Insurance" );
            InsuranceCol.AddAttributes( "_id", "name", "value" );

            MongoDBCollection OwnsCol = new MongoDBCollection( "Owns" );
            OwnsCol.AddAttributes( "_id", "personId", "carId", "insuranceId" );

            MongoDBCollection ManufacturerCol = new MongoDBCollection( "Manufacturer" );
            ManufacturerCol.AddAttributes( "_id", "name" );

            MongoSchema Schema = new MongoSchema( "Schema", new List<MongoDBCollection> { PersonCol, CarCol, GarageCol, RepairedCol, SupplierCol, InsuranceCol, OwnsCol, ManufacturerCol } );

            // Map Rules
            MapRule PersonRule = new MapRule( Person, PersonCol );
            PersonRule.AddRule( "personId", "_id" );
            PersonRule.AddRule( "name", "name" );

            MapRule CarRule = new MapRule( Car, CarCol );
            CarRule.AddRule( "carId", "_id" );
            CarRule.AddRule( "model", "model" );
            CarRule.AddRule( "year", "year" );
            CarRule.AddRule( "manufacturerId", "manufacturerId" );

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

            MapRule OwnsRule = new MapRule( Owns, OwnsCol );
            OwnsRule.AddRule( "ownsId", "_id" );
            OwnsRule.AddRule( "personId", "personId" );
            OwnsRule.AddRule( "carId", "carId" );
            OwnsRule.AddRule( "insuranceId", "insuranceId" );

            MapRule ManufacturerRule = new MapRule( Manufacturer, ManufacturerCol );
            ManufacturerRule.AddRule( "manufacturerId", "_id" );
            ManufacturerRule.AddRule( "name", "name" );

            ModelMapping Map = new ModelMapping( "Map", new List<MapRule> { PersonRule, CarRule, GarageRule, RepairedRule, SupplierRule, InsuranceRule, OwnsRule, ManufacturerRule } );

            return new RequiredDataContainer( Model, Schema, Map );
        }
    }
}