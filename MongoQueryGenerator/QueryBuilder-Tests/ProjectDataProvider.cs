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

            Relationship Owns = new Relationship( "Owns", RelationshipCardinality.ManyToMany );
            Owns.AddAttributes( "ownsId", "personId", "carId", "insuranceId" );
            RelationshipConnection PersonOwnsCar = new RelationshipConnection(
                Person,
                Person.GetAttribute( "personId" ),
                Owns.GetAttribute( "personId" ),
                Car,
                Car.GetAttribute( "carId" ),
                Owns.GetAttribute( "carId" ) );
            Owns.AddRelation( PersonOwnsCar );

            RelationshipConnection PersonOwnsInsurance = new RelationshipConnection(
                Person,
                Person.GetAttribute( "personId" ),
                Owns.GetAttribute( "personId" ),
                Insurance,
                Insurance.GetAttribute( "insuranceId" ),
                Owns.GetAttribute( "insuranceId" ) );
            Owns.AddRelation( PersonOwnsInsurance );

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

            Relationship ManufacturedBy = new Relationship( "ManufacturedBy", RelationshipCardinality.OneToOne );
            RelationshipConnection CarManufacturedBy = new RelationshipConnection(
                Car,
                Car.GetAttribute( "manufacturerId" ),
                Manufacturer,
                Manufacturer.GetAttribute( "manufacturerId" ) );
            ManufacturedBy.AddRelation( CarManufacturedBy );

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