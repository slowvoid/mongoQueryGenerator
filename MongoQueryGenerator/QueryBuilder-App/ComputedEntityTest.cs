﻿using QueryBuilder.ER;
using QueryBuilder.Map;
using QueryBuilder.Mongo;
using QueryBuilder.Operation;
using QueryBuilder.Operation.Arguments;
using QueryBuilder.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueryBuilderApp
{
    public static class ComputedEntityTest
    {
        public static void Main()
        {
            Console.WriteLine( "Creating data" );

            ERModel Model = CreateModel();
            MongoSchema Schema = CreateSchema();
            ModelMapping Map = CreateMap( Model, Schema );

            ComputedEntity CarInsCompany = new ComputedEntity( "CarRepairedGarage",
                new QueryableEntity( Model.FindByName( "Car" ) as Entity, "car" ),
                Model.FindByName( "Repaired" ) as Relationship,
                new List<QueryableEntity> {
                    new QueryableEntity( Model.FindByName( "Garage" ) as Entity, "garage" )
                } );

            RelationshipJoinArgument args = new RelationshipJoinArgument(
                Model.FindByName( "Drives" ) as Relationship,
                new List<QueryableEntity> {
                    new QueryableEntity( CarInsCompany )
                } );

            RelationshipJoinOperator RJoin = new RelationshipJoinOperator(
                new QueryableEntity( Model.FindByName( "Person" ) as Entity, "person" ),
                new List<RelationshipJoinArgument> { args },
                Map );

            // Generate virtual map
            VirtualMap VMap = RJoin.ComputeVirtualMap();

            Console.WriteLine( $"Virtual map: {VMap.ToString()}" );

            List<AlgebraOperator> Operators = new List<AlgebraOperator>();
            Operators.Add( RJoin );

            FromArgument StartArg = new FromArgument( new QueryableEntity( Model.FindByName( "Person" ) ), Map );

            QueryGenerator Query = new QueryGenerator( StartArg, Operators );

            string QueryString = Query.Run();
            Console.WriteLine( $"Query: {QueryString}" );

            QueryRunner Runner = new QueryRunner( "mongodb://localhost:27017", "ceOneToOne" );
            string Result = Runner.GetJSON( QueryString );

            Console.WriteLine( $"\n\nResult: {Result}" );

            Console.Read();
        }

        public static ERModel CreateModel()
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
            Repaired.AddAttributes( "repaired", "carId", "garageId" );
            RelationshipConnection CarRepairedGarage = new RelationshipConnection(
                Car,
                Car.GetAttribute( "carId" ),
                Repaired.GetAttribute( "carId" ),
                Garage,
                Garage.GetAttribute( "garageId" ),
                Repaired.GetAttribute( "garageId" ) );
            Repaired.AddRelation( CarRepairedGarage );

            ERModel Model = new ERModel( "ParentSample", new List<BaseERElement> { Car, Garage, Drives, Person, Repaired } );
            return Model;
        }

        public static MongoSchema CreateSchema()
        {
            MongoDBCollection Person = new MongoDBCollection( "Person" );
            Person.AddAttributes( "_id", "name", "carId" );

            MongoDBCollection Car = new MongoDBCollection( "Car" );
            Car.AddAttributes( "_id", "model", "year" );

            MongoDBCollection Garage = new MongoDBCollection( "Garage" );
            Garage.AddAttributes( "_id", "name" );

            MongoDBCollection Repaired = new MongoDBCollection( "Repaired" );
            Repaired.AddAttributes( "_id", "carId", "garageId" );

            MongoSchema Schema = new MongoSchema( "ParentSchema", new List<MongoDBCollection> { Person, Car, Garage, Repaired } );
            return Schema;
        }

        public static ModelMapping CreateMap(ERModel Model, MongoSchema Schema)
        {
            MapRule PersonRule = new MapRule( Model.FindByName( "Person" ), Schema.FindByName( "Person" ) );
            PersonRule.AddRule( "personId", "_id" );
            PersonRule.AddRule( "name", "name" );
            PersonRule.AddRule( "carId", "carId" );

            MapRule CarRule = new MapRule( Model.FindByName( "Car" ), Schema.FindByName( "Car" ) );
            CarRule.AddRule( "carId", "_id" );
            CarRule.AddRule( "model", "model" );
            CarRule.AddRule( "year", "year" );

            MapRule GarageRule = new MapRule( Model.FindByName( "Garage" ), Schema.FindByName( "Garage" ) );
            GarageRule.AddRule( "garageId", "_id" );
            GarageRule.AddRule( "name", "name" );

            MapRule RepairedRule = new MapRule( Model.FindByName( "Repaired" ), Schema.FindByName( "Repaired" ) );
            RepairedRule.AddRule( "carId", "carId" );
            RepairedRule.AddRule( "garageId", "garageId" );
            RepairedRule.AddRule( "repaired", "repaired" );

            ModelMapping Map = new ModelMapping( "ParentMap", new List<MapRule> { PersonRule, CarRule, GarageRule, RepairedRule } );

            return Map;
        }
    }
}
