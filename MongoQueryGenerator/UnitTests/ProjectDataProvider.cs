using QueryBuilder.ER;
using QueryBuilder.Map;
using QueryBuilder.Mongo;
using System.Collections.Generic;

namespace QueryBuilder.Tests
{
    /// <summary>
    /// Provides data to test Project operations
    /// </summary>
    public static class ProjectDataProvider
    {
        /// <summary>
        /// Generate a simple model
        /// </summary>
        /// <returns></returns>
        public static RequiredDataContainer SimpleModel()
        {
            Entity Person = new Entity( "Person" );
            Person.AddAttributes( "personId", "name", "age" );

            ERModel Model = new ERModel( "ProjectModel", new List<BaseERElement> { Person } );

            MongoDBCollection PersonCol = new MongoDBCollection( "Person" );
            PersonCol.AddAttributes( "_id", "name", "age" );

            MongoSchema Schema = new MongoSchema( "PersonSchema", new List<MongoDBCollection> { PersonCol } );

            MapRule PersonRule = new MapRule( Person, PersonCol );
            PersonRule.AddRule( "personId", "_id" );
            PersonRule.AddRule( "name", "name" );
            PersonRule.AddRule( "age", "age" );

            ModelMapping Map = new ModelMapping( "PersonMap", new List<MapRule> { PersonRule } );

            return new RequiredDataContainer( Model, Schema, Map );
        }
        /// <summary>
        /// Generate data with a computed entity
        /// </summary>
        /// <returns></returns>
        public static RequiredDataContainer ComputedEntityData()
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
            Insurance.SetIdentifier( "insuranceId" );

            Entity Manufacturer = new Entity( "Manufacturer" );
            Manufacturer.AddAttributes( "manufacturerId", "name" );
            Manufacturer.SetIdentifier( "manufacturerId" );

            Relationship Owns = new Relationship( "Owns" );
            Owns.AddAttributes( "ownsId" );
            Owns.AddRelationshipEnd( new RelationshipEnd( Car ) );
            Owns.AddRelationshipEnd( new RelationshipEnd( Person ) );
            Owns.AddRelationshipEnd( new RelationshipEnd( Insurance ) );

            Relationship Repaired = new Relationship( "Repaired" );
            Repaired.AddAttributes( "repairedId", "repaired" );
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

            MapRule GarageRule = new MapRule( Garage, GarageCol );
            GarageRule.AddRule( "garageId", "_id" );
            GarageRule.AddRule( "name", "name" );

            MapRule SupplierRule = new MapRule( Supplier, SupplierCol );
            SupplierRule.AddRule( "supplierId", "_id" );
            SupplierRule.AddRule( "name", "name" );

            MapRule RepairedRule = new MapRule( Repaired, RepairedCol );
            RepairedRule.AddRule( "repairedId", "_id" );
            RepairedRule.AddRule( "repaired", "repaired" );

            MapRule InsuranceRule = new MapRule( Insurance, InsuranceCol );
            InsuranceRule.AddRule( "insuranceId", "_id" );
            InsuranceRule.AddRule( "name", "name" );
            InsuranceRule.AddRule( "value", "value" );

            MapRule OwnsRule = new MapRule( Owns, OwnsCol );
            OwnsRule.AddRule( "ownsId", "_id" );

            MapRule ManufacturerRule = new MapRule( Manufacturer, ManufacturerCol );
            ManufacturerRule.AddRule( "manufacturerId", "_id" );
            ManufacturerRule.AddRule( "name", "name" );

            MapRule PersonOwnsRule = new MapRule( Person, OwnsCol, false );
            PersonOwnsRule.AddRule( "personId", "personId" );

            MapRule CarOwnsRule = new MapRule( Car, OwnsCol, false );
            CarOwnsRule.AddRule( "carId", "carId" );

            MapRule InsuranceOwnsRule = new MapRule( Insurance, OwnsCol, false );
            InsuranceOwnsRule.AddRule( "insuranceId", "insuranceId" );

            MapRule CarRepairedRule = new MapRule( Car, RepairedCol, false );
            CarRepairedRule.AddRule( "carId", "carId" );

            MapRule GarageRepairedRule = new MapRule( Garage, RepairedCol, false );
            GarageRepairedRule.AddRule( "garageId", "garageId" );

            MapRule SupplierRepairedRule = new MapRule( Supplier, RepairedCol, false );
            SupplierRepairedRule.AddRule( "supplierId", "supplierId" );

            MapRule ManufacturerCarRule = new MapRule( Manufacturer, CarCol, false );
            ManufacturerCarRule.AddRule( "manufacturerId", "manufacturerId" );

            ModelMapping Map = new ModelMapping( "Map", new List<MapRule> { PersonRule, CarRule, GarageRule, RepairedRule, SupplierRule, InsuranceRule, OwnsRule, ManufacturerRule, PersonOwnsRule, CarOwnsRule, InsuranceOwnsRule, CarRepairedRule, GarageRepairedRule, SupplierRepairedRule, ManufacturerCarRule } );

            return new RequiredDataContainer( Model, Schema, Map );
        }
    }
}