using QueryBuilder.ER;
using QueryBuilder.Map;
using QueryBuilder.Mongo;
using QueryBuilder.Operation;
using QueryBuilder.Operation.Arguments;
using QueryBuilder.Query;
using System;
using System.Collections.Generic;

namespace QueryBuilderApp
{
    public static class MultipleRJOINApp
    {
        public static void Main()
        {
            Console.WriteLine( "Multiple RJOIN TEST" );

            ERModel Model = CreateModel();
            MongoSchema Schema = CreateSchema();
            ModelMapping Map = CreateMap( Model, Schema );

            QueryableEntity Person = new QueryableEntity( Model.FindByName( "Person" ) );
            QueryableEntity Car = new QueryableEntity( Model.FindByName( "Car" ) );
            QueryableEntity Pet = new QueryableEntity( Model.FindByName( "Pet" ) );

            RelationshipJoinArgument Arg1 = new RelationshipJoinArgument( (Relationship)Model.FindByName( "HasCar" ), new List<QueryableEntity>() { Car } );
            RelationshipJoinArgument Arg2 = new RelationshipJoinArgument( (Relationship)Model.FindByName( "HasPet" ), new List<QueryableEntity>() { Pet } );

            RelationshipJoinOperator RJOINOp1 = new RelationshipJoinOperator( Person, new List<RelationshipJoinArgument>() { Arg1 }, Map );
            RelationshipJoinOperator RJOINOp2 = new RelationshipJoinOperator( Person, new List<RelationshipJoinArgument>() { Arg2 }, Map );

            VirtualMap VMap1 = RJOINOp1.ComputeVirtualMap();
            VirtualMap VMap2 = RJOINOp2.ComputeVirtualMap( VMap1 );

            Console.WriteLine( "Virtual Map 1 {0}{1}", Environment.NewLine, VMap1.ToString() );
            Console.WriteLine( "Virtual Map 2 {0}{1}", Environment.NewLine, VMap2.ToString() );

            Pipeline QueryPipeline = new Pipeline( new List<AlgebraOperator>() { RJOINOp1, RJOINOp2 } );
            QueryGenerator QueryGen = new QueryGenerator( QueryPipeline );
            QueryGen.CollectionName = "Person";

            string QueryString = QueryGen.Run();
            Console.WriteLine( "Query result: {0}{1}", Environment.NewLine, QueryString );

            QueryRunner Runner = new QueryRunner( "mongodb://localhost:27017", "multipleRJOIN" );
            Console.WriteLine( "Query Result: {0}{1}", Environment.NewLine, Runner.GetJSON( QueryString ) );

            Console.Read();
        }

        public static ERModel CreateModel()
        {
            Entity Person = new Entity( "Person" );
            Person.AddAttributes( "personId", "name", "age" );

            Entity Car = new Entity( "Car" );
            Car.AddAttributes( "carId", "model", "year", "ownerId" );

            Entity Pet = new Entity( "Pet" );
            Pet.AddAttributes( "petId", "name", "ownerId" );

            Entity InsCompany = new Entity( "InsCompany" );
            InsCompany.AddAttributes( "companyId", "name" );

            Relationship HasPet = new Relationship( "HasPet", RelationshipCardinality.OneToMany );
            RelationshipConnection PersonHasPets = new RelationshipConnection(
                Person,
                Person.GetAttribute( "personId" ),
                Pet,
                Pet.GetAttribute( "ownerId" ) );
            HasPet.AddRelation( PersonHasPets );

            Relationship HasCar = new Relationship( "HasCar", RelationshipCardinality.OneToMany );
            RelationshipConnection PersonHasCar = new RelationshipConnection(
                Person,
                Person.GetAttribute( "personId" ),
                Car,
                Car.GetAttribute( "ownerId" ) );
            HasCar.AddRelation( PersonHasCar );

            Relationship HasInsurance = new Relationship( "HasInsurance", RelationshipCardinality.ManyToMany );
            HasInsurance.AddAttributes( "carId", "companyId" );
            RelationshipConnection CarHasInsurance = new RelationshipConnection(
                Car,
                Car.GetAttribute( "carId" ),
                HasInsurance.GetAttribute( "carId" ),
                InsCompany,
                InsCompany.GetAttribute( "companyId" ),
                HasInsurance.GetAttribute( "companyId" ) );
            HasInsurance.AddRelation( CarHasInsurance );

            return new ERModel( "Model", new List<BaseERElement>() { Person, Car, Pet, InsCompany, HasInsurance, HasPet, HasCar } );
        }

        public static MongoSchema CreateSchema()
        {
            MongoDBCollection PersonCol = new MongoDBCollection( "Person" );
            PersonCol.AddAttributes( "_id", "name", "age" );

            MongoDBCollection CarCol = new MongoDBCollection( "Car" );
            CarCol.AddAttributes( "_id", "model", "year", "ownerId" );

            MongoDBCollection PetCol = new MongoDBCollection( "Pet" );
            PetCol.AddAttributes( "_id", "name", "ownerId" );

            MongoDBCollection InsCompanyCol = new MongoDBCollection( "InsCompany" );
            InsCompanyCol.AddAttributes( "_id", "name" );

            MongoDBCollection HasInsuranceCol = new MongoDBCollection( "HasInsurance" );
            HasInsuranceCol.AddAttributes( "carId", "companyId" );

            return new MongoSchema( "Schema", new List<MongoDBCollection>() { PersonCol, CarCol, PetCol, InsCompanyCol, HasInsuranceCol } );
        }

        public static ModelMapping CreateMap( ERModel Model, MongoSchema Schema )
        {
            MapRule PersonRules = new MapRule( Model.FindByName( "Person" ), Schema.FindByName( "Person" ) );
            PersonRules.AddRule( "personId", "_id" );
            PersonRules.AddRule( "name", "name" );
            PersonRules.AddRule( "age", "age" );

            MapRule CarRules = new MapRule( Model.FindByName( "Car" ), Schema.FindByName( "Car" ) );
            CarRules.AddRule( "carId", "_id" );
            CarRules.AddRule( "model", "model" );
            CarRules.AddRule( "year", "year" );
            CarRules.AddRule( "ownerId", "ownerId" );

            MapRule PetRules = new MapRule( Model.FindByName( "Pet" ), Schema.FindByName( "Pet" ) );
            PetRules.AddRule( "petId", "_id" );
            PetRules.AddRule( "name", "name" );
            PetRules.AddRule( "ownerId", "ownerId" );

            MapRule InsCompanyRules = new MapRule( Model.FindByName( "InsCompany" ), Schema.FindByName( "InsCompany" ) );
            InsCompanyRules.AddRule( "companyId", "_id" );
            InsCompanyRules.AddRule( "name", "name" );

            MapRule HasInsuranceRules = new MapRule( Model.FindByName( "HasInsurance" ), Schema.FindByName( "HasInsurance" ) );
            HasInsuranceRules.AddRule( "companyId", "companyId" );
            HasInsuranceRules.AddRule( "carId", "carId" );

            return new ModelMapping( "Map", new List<MapRule>() { PersonRules, CarRules, PetRules, InsCompanyRules, HasInsuranceRules } );
        }
    }
}