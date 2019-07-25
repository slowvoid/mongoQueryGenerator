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
    }
}