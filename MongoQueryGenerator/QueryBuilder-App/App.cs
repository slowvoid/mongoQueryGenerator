using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Bson;
using MongoDB.Driver;
using QueryBuilder.ER;
using QueryBuilder.Map;
using QueryBuilder.Mongo;
using QueryBuilder.Mongo.Expressions;
using QueryBuilder.Operation;
using QueryBuilder.Operation.Arguments;
using QueryBuilder.Query;
using QueryBuilder.Shared;

namespace QueryBuilderApp
{
    public static class App
    {
        public static void Main()
        {
            ERModel Model = CreateERModel();
            MongoSchema Schema = CreateSchema();
            ModelMapping Map = CreateMap( Model, Schema );

            Console.WriteLine( "Data ready, initializing query generator..." );

            string InitialEntity = "Person";

            QueryableEntity Car = new QueryableEntity( (Entity)Model.FindByName( "Car" ), "car" );
            QueryableEntity Person = new QueryableEntity( (Entity)Model.FindByName( "Person" ), "person" );

            RelationshipJoinArgument JoinArgs = new RelationshipJoinArgument(
                (Relationship)Model.FindByName( "Drives" ),
                new List<QueryableEntity> {
                    Car
                } );

            RelationshipJoinOperator RJoin = new RelationshipJoinOperator(
                Person,
                new List<RelationshipJoinArgument> {
                    JoinArgs
                }, Map );

            VirtualMap VMap = RJoin.ComputeVirtualMap();

            // Select only name and car model
            List<ProjectArgument> Arguments = new List<ProjectArgument>();
            Arguments.Add( new ProjectArgument( Person.GetAttribute( "name" ), Person, new BooleanExpr( true ) ) );
            Arguments.Add( new ProjectArgument( Car.GetAttribute( "model" ), Car, new BooleanExpr( true ) ) );
            Arguments.Add( new ProjectArgument( Car.GetAttribute( "year" ), Car, new BooleanExpr( true ) ) );

            // Project
            ProjectStage ProjectOp = new ProjectStage( Arguments, VMap );

            CartesianProductOperator CartesianOp = new CartesianProductOperator( 
                Person,
                Car,
                Map );

            QueryableEntity InsCompany = new QueryableEntity( Model.FindByName( "InsCompany" ), "inscompany" );

            CartesianProductOperator CartesianOp2 = new CartesianProductOperator(
                Person,
                InsCompany,
                Map );

            Pipeline QueryPipeline = new Pipeline( new List<AlgebraOperator> {
                //RJoin,
                //ProjectOp,
                CartesianOp,
                CartesianOp2
            } );

            VirtualMap VCartesianMap = CartesianOp.ComputeVirtualMap();

            Console.WriteLine( CartesianOp2.ComputeVirtualMap( VCartesianMap ).ToString() );

            QueryGenerator QueryGen = new QueryGenerator( QueryPipeline );
            QueryGen.CollectionName = InitialEntity;
            string Query = QueryGen.Run();

            Console.WriteLine( Query );

            Console.Read();
        }

        public static ERModel CreateERModel()
        {
            Entity Person = new Entity( "Person" );
            Person.AddAttribute( "personId" );
            Person.AddAttribute( "name" );

            Entity Car = new Entity( "Car" );
            Car.AddAttribute( "carId" );
            Car.AddAttribute( "model" );
            Car.AddAttribute( "year" );
            Car.AddAttribute( "engine" );
            Car.AddAttribute( "fuel" );

            Entity InsCompany = new Entity( "InsCompany" );
            InsCompany.AddAttribute( "companyId" );
            InsCompany.AddAttribute( "name" );

            Relationship Drives = new Relationship( "Drives", RelationshipCardinality.OneToMany );
            RelationshipConnection PersonCar = new RelationshipConnection(
                Person,
                Person.GetAttribute( "personId" ),
                Car,
                Car.GetAttribute( "carId" ) );
            Drives.AddRelation( PersonCar );

            Relationship HasInsurance = new Relationship( "HasInsurance", RelationshipCardinality.ManyToMany );
            HasInsurance.AddAttribute( "carId" );
            HasInsurance.AddAttribute( "companyId" );
            HasInsurance.AddAttribute( "value" );
            RelationshipConnection CarInsCompany = new RelationshipConnection(
                Car,
                Car.GetAttribute( "carId" ),
                HasInsurance.GetAttribute( "carId" ),
                InsCompany,
                InsCompany.GetAttribute( "companyId" ),
                HasInsurance.GetAttribute( "companyId" ) );
            HasInsurance.AddRelation( CarInsCompany );

            ERModel Model = new ERModel( "RefactorModel", new List<BaseERElement> { Person, Car, InsCompany, Drives, HasInsurance } );
            return Model;
        }

        public static MongoSchema CreateSchema()
        {
            MongoDBCollection PersonCol = new MongoDBCollection( "Person" );
            PersonCol.DocumentSchema.AddAttribute( "_id" );
            PersonCol.DocumentSchema.AddAttribute( "name" );
            PersonCol.DocumentSchema.AddAttribute( "cars" );

            MongoDBCollection CarCol = new MongoDBCollection( "Car" );
            CarCol.DocumentSchema.AddAttribute( "_id" );
            CarCol.DocumentSchema.AddAttribute( "model" );
            CarCol.DocumentSchema.AddAttribute( "year" );
            CarCol.DocumentSchema.AddAttribute( "engine" );
            CarCol.DocumentSchema.AddAttribute( "fuel" );

            MongoDBCollection InsCompanyCol = new MongoDBCollection( "InsCompany" );
            InsCompanyCol.DocumentSchema.AddAttribute( "_id" );
            InsCompanyCol.DocumentSchema.AddAttribute( "name" );

            MongoDBCollection InsuranceCol = new MongoDBCollection( "HasInsurance" );
            InsuranceCol.DocumentSchema.AddAttribute( "_id" );
            InsuranceCol.DocumentSchema.AddAttribute( "carId" );
            InsuranceCol.DocumentSchema.AddAttribute( "companyId" );
            InsuranceCol.DocumentSchema.AddAttribute( "value" );

            MongoSchema Schema = new MongoSchema( "RefactorSchema", new List<MongoDBCollection> { PersonCol, CarCol, InsCompanyCol, InsuranceCol } );
            return Schema;
        }

        public static ModelMapping CreateMap(ERModel Model, MongoSchema Schema)
        {
            MapRule PersonRules = new MapRule( Model.FindByName( "Person" ), Schema.FindByName( "Person" ) );
            PersonRules.AddRule( "personId", "_id" );
            PersonRules.AddRule( "name", "name" );

            MapRule CarRules = new MapRule( Model.FindByName( "Car" ), Schema.FindByName( "Car" ) );
            CarRules.AddRule( "carId", "_id" );
            CarRules.AddRule( "model", "model" );
            CarRules.AddRule( "year", "year" );
            CarRules.AddRule( "engine", "engine" );
            CarRules.AddRule( "fuel", "fuel" );

            MapRule CarRules_Embedded = new MapRule( Model.FindByName( "Car" ), Schema.FindByName( "Person" ) );
            CarRules_Embedded.AddRule( "carId", "cars._id" );
            CarRules_Embedded.AddRule( "model", "cars.model" );
            CarRules_Embedded.AddRule( "year", "cars.year" );
            CarRules_Embedded.IsMain = false;

            MapRule InsCompanyRules = new MapRule( Model.FindByName( "InsCompany" ), Schema.FindByName( "InsCompany" ) );
            InsCompanyRules.AddRule( "companyId", "_id" );
            InsCompanyRules.AddRule( "name", "name" );

            MapRule InsuranceRules = new MapRule( Model.FindByName( "HasInsurance" ), Schema.FindByName( "HasInsurance" ) );
            InsuranceRules.AddRule( "carId", "carId" );
            InsuranceRules.AddRule( "companyId", "companyId" );
            InsuranceRules.AddRule( "value", "value" );

            ModelMapping Map = new ModelMapping( "RefactorMapping", new List<MapRule> { PersonRules, CarRules, CarRules_Embedded, InsuranceRules, InsCompanyRules } );
            return Map;
        }
    }
}