﻿using QueryBuilder.ER;
using QueryBuilder.Map;
using QueryBuilder.Mongo;
using System.Collections.Generic;

namespace QueryBuilder.Tests
{
    public static class CartesianProductDataProvider
    {
        /// <summary>
        /// Generate sample data to use in for cartesian product tests
        /// </summary>
        /// <returns></returns>
        public static RequiredDataContainer SampleData()
        {
            Entity Person = new Entity( "Person" );
            Person.AddAttributes( "personId", "name" );
            Person.SetIdentifier( "personId" );

            Entity Car = new Entity( "Car" );
            Car.AddAttributes( "carId", "model", "year", "manufacturerId" );
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
            Owns.AddAttributes( "ownsId", "personId", "carId", "insuranceId" );
            Owns.AddRelationshipEnd( new RelationshipEnd( Car ) );
            Owns.AddRelationshipEnd( new RelationshipEnd( Person ) );
            Owns.AddRelationshipEnd( new RelationshipEnd( Insurance ) );

            Relationship Repaired = new Relationship( "Repaired" );
            Repaired.AddAttributes( "repairedId", "carId", "garageId", "supplierId", "repaired" );
            Repaired.AddRelationshipEnd( new RelationshipEnd( Garage ) );
            Repaired.AddRelationshipEnd( new RelationshipEnd( Car ) );
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