using QueryBuilder.ER;
using QueryBuilder.Map;
using QueryBuilder.Mongo;
using QueryBuilder.Operation;
using QueryBuilder.Operation.Arguments;
using QueryBuilder.Parser;
using QueryBuilder.Query;
using System;
using System.Collections.Generic;
using System.IO;

namespace TestApp
{
    class Program
    {
        static void Main( string[] args )
        {
            Console.WriteLine( "Running new stuff..." );

            //var map = QueryBuilderParser.ParseMapping( new FileStream( "Test.qer", FileMode.Open ) );
            //foreach ( MapRule Rule in map.ERMongoMapping.Rules )
            //{
            //    Console.WriteLine( "Rules for {0} Mapped To {1}", Rule.Source.Name, Rule.Target.Name );
            //    foreach ( KeyValuePair<string, string> Attributes in Rule.Rules )
            //    {
            //        Console.WriteLine( "Key: {0} | Value: {1}", Attributes.Key, Attributes.Value );
            //    }
            //}

            ////string query = "from Person p rjoin <Drives d> (Car c)";
            ////string query = "from Person p rjoin <HasInsurance h> (Car c, Insurance i)";
            //string query = "from Person p rjoin <HasInsurance h> (Car c rjoin <Repaired r> (Garage g))";
            //QueryGenerator gen = QueryBuilderParser.ParseQuery( query, map );

            //string mongoQuery = gen.Run();

            DataContainer Data = ModelData();

            QueryableEntity Person = new QueryableEntity( Data.EntityRelationshipModel.FindByName( "Person" ) );
            QueryableEntity Car = new QueryableEntity( Data.EntityRelationshipModel.FindByName( "Car" ) );
            QueryableEntity Garage = new QueryableEntity( Data.EntityRelationshipModel.FindByName( "Garage" ) );

            ComputedEntity CarRepairedByGarage = new ComputedEntity( "CarRepairedByGarage", Car, (Relationship)Data.EntityRelationshipModel.FindByName( "Repaired" ), new List<QueryableEntity>() { Garage } );


            RelationshipJoinOperator JoinOp = new RelationshipJoinOperator( Person, (Relationship)Data.EntityRelationshipModel.FindByName( "Drives" ), new List<QueryableEntity>() { new QueryableEntity( CarRepairedByGarage ) }, Data.ERMongoMapping );

            QueryGenerator queryGen = new QueryGenerator( new FromArgument( Person, Data.ERMongoMapping ), new List<AlgebraOperator>() { JoinOp } );

            string mongoQuery = queryGen.Run();

            Console.WriteLine( mongoQuery );

            QueryRunner runner = new QueryRunner( "mongodb://localhost:27017", "testParser" );

            Console.WriteLine( runner.GetJSON( mongoQuery ) );

            Console.Read();
        }

        static DataContainer ModelData()
        {
            // ER MODEL
            Entity Person = new Entity( "Person" );
            Person.AddAttributes( "id", "name", "age" );

            Entity Car = new Entity( "Car" );
            Car.AddAttributes( "id", "model", "year" );

            Entity Garage = new Entity( "Garage" );
            Garage.AddAttributes( "id", "name" );

            Relationship Drives = new Relationship( "Drives" );
            Drives.AddRelationshipEnd( new RelationshipEnd( Person ) );
            Drives.AddRelationshipEnd( new RelationshipEnd( Car ) );

            Relationship Repaired = new Relationship( "Repaired" );
            Repaired.AddAttributes( "items" );
            Repaired.AddRelationshipEnd( new RelationshipEnd( Car ) );
            Repaired.AddRelationshipEnd( new RelationshipEnd( Garage ) );

            ERModel Model = new ERModel( "Sample Model", new List<BaseERElement>() { Person, Car, Garage, Drives, Repaired } );

            // MONGO SCHEMA
            MongoDBCollection PersonCol = new MongoDBCollection( "Person" );
            PersonCol.AddAttributes( "_id", "fName", "fAge" );

            MongoDBCollection CarCol = new MongoDBCollection( "Car" );
            CarCol.AddAttributes( "_id", "model", "year" );

            MongoDBCollection GarageCol = new MongoDBCollection( "Garage" );
            GarageCol.AddAttributes( "_id", "name" );

            MongoDBCollection DrivesCol = new MongoDBCollection( "Drives" );
            DrivesCol.AddAttributes( "fPersonId", "fCarId" );

            MongoDBCollection RepairedCol = new MongoDBCollection( "Repaired" );
            RepairedCol.AddAttributes( "items" );

            MongoSchema Schema = new MongoSchema( "Sample schema", new List<MongoDBCollection>() { PersonCol, CarCol, GarageCol, RepairedCol } );

            // MAPPING
            MapRule PersonMainRule = new MapRule( Person, PersonCol );
            PersonMainRule.AddRule( "id", "_id" );
            PersonMainRule.AddRule( "name", "fName" );
            PersonMainRule.AddRule( "age", "fAge" );

            MapRule PersonDrivesRule = new MapRule( Person, DrivesCol, false );
            PersonDrivesRule.AddRule( "id", "fPersonId" );

            MapRule CarMainRule = new MapRule( Car, CarCol );
            CarMainRule.AddRule( "id", "_id" );
            CarMainRule.AddRule( "model", "model" );
            CarMainRule.AddRule( "year", "year" );

            MapRule CarDrivesRule = new MapRule( Car, DrivesCol, false );
            CarDrivesRule.AddRule( "id", "fCarId" );

            MapRule CarRepairedRule = new MapRule( Car, RepairedCol, false );
            CarRepairedRule.AddRule( "id", "carId" );

            MapRule DrivesRule = new MapRule( Drives, DrivesCol );

            MapRule GarageMainRule = new MapRule( Garage, GarageCol );
            GarageMainRule.AddRule( "id", "_id" );
            GarageMainRule.AddRule( "name", "name" );

            MapRule GarageRepairedRule = new MapRule( Garage, RepairedCol, false );
            GarageRepairedRule.AddRule( "id", "garageId" );

            MapRule RepairedRule = new MapRule( Repaired, RepairedCol );
            RepairedRule.AddRule( "items", "items" );

            ModelMapping Map = new ModelMapping( "Sample map", new List<MapRule>() { PersonMainRule, PersonDrivesRule, CarMainRule, CarDrivesRule, CarRepairedRule, GarageMainRule, GarageRepairedRule, RepairedRule, DrivesRule } );

            return new DataContainer( Model, Schema, Map );
        }
    }
}
