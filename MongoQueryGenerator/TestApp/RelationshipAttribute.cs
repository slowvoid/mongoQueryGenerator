using QueryBuilder.ER;
using QueryBuilder.Map;
using QueryBuilder.Mongo;
using QueryBuilder.Operation;
using QueryBuilder.Operation.Arguments;
using QueryBuilder.Query;
using System;
using System.Collections.Generic;

namespace TestApp
{
    public static class RelationshipAttributeTest
    {
        public static void Main()
        {
            DataContainer data = CreateDataContainer();

            QueryableEntity Person = new QueryableEntity( data.EntityRelationshipModel.FindByName( "Person" ) );
            QueryableEntity Car = new QueryableEntity( data.EntityRelationshipModel.FindByName( "Car" ) );
            Relationship Drives = (Relationship)data.EntityRelationshipModel.FindByName( "Drives" );

            ModelMapping Map = data.ERMongoMapping;

            RelationshipJoinOperator RJoinDrives = new RelationshipJoinOperator(
                Person,
                Drives,
                new List<QueryableEntity>() { Car },
                Map );

            FromArgument From = new FromArgument( Person, Map );
            QueryGenerator QueryGen = new QueryGenerator( From, new List<AlgebraOperator>() { RJoinDrives } );
            string query = QueryGen.Run();

            Console.WriteLine( query );
        }

        public static DataContainer CreateDataContainer()
        {
            Entity Person = new Entity( "Person" );
            Person.AddAttribute( "personId", true );
            Person.AddAttribute( "name" );

            Entity Car = new Entity( "Car" );
            Car.AddAttribute( "carId", true );
            Car.AddAttributes( "plate", "color" );

            Relationship Drives = new Relationship( "Drives" );
            Drives.AddAttribute( "note" );
            Drives.AddRelationshipEnd( new RelationshipEnd( Person ) );
            Drives.AddRelationshipEnd( new RelationshipEnd( Car ) );

            ERModel Model = new ERModel( "Model", new List<BaseERElement>() { Person, Car, Drives } );

            MongoDBCollection PersonCol = new MongoDBCollection( "Person" );
            PersonCol.AddAttributes( "_id", "name", "cars_multivalued_.id", "cars_multivalued_.note" );

            MongoDBCollection CarCol = new MongoDBCollection( "Car" );
            CarCol.AddAttributes( "_id", "plate", "color" );

            MongoSchema Schema = new MongoSchema( "Schema", new List<MongoDBCollection>() { PersonCol, CarCol } );

            MapRule PersonRule = new MapRule( Person, PersonCol );
            PersonRule.AddRule( "personId", "_id" );
            PersonRule.AddRule( "name", "name" );

            MapRule CarRule = new MapRule( Car, CarCol );
            CarRule.AddRule( "carId", "_id" );
            CarRule.AddRule( "plate", "plate" );
            CarRule.AddRule( "color", "color" );

            MapRule CarPersonRule = new MapRule( Car, PersonCol, false );
            CarPersonRule.AddRule( "carId", "cars_multivalued_.id" );

            MapRule DrivesPersonRule = new MapRule( Drives, PersonCol, false );
            DrivesPersonRule.AddRule( "note", "cars_multivalued_.note" );

            ModelMapping Map = new ModelMapping( "Map", new List<MapRule>() { PersonRule, CarRule, CarPersonRule, DrivesPersonRule } );

            return new DataContainer( Model, Schema, Map );
        }
    }
}