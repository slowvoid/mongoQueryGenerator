using QueryBuilder.ER;
using QueryBuilder.Map;
using QueryBuilder.Mongo;
using QueryBuilder.Operation;
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

            ComputedEntity CarInsCompany = new ComputedEntity( "CarInsCompany",
                Model.FindByName( "Car" ) as Entity,
                Model.FindByName( "Repaired" ) as Relationship,
                new List<Entity> { Model.FindByName( "Garage" ) as Entity } );

            RelationshipJoinOperator RJoin = new RelationshipJoinOperator(
                Model.FindByName( "InsCompany" ) as Entity,
                Model.FindByName( "Insurance" ) as Relationship,
                new List<Entity> { CarInsCompany },
                Map );

            List<AlgebraOperator> Operators = new List<AlgebraOperator>();
            Operators.Add( RJoin );
            Pipeline Pipeline = new Pipeline( ( Operators ) );

            QueryGenerator Query = new QueryGenerator( Pipeline )
            {
                CollectionName = "InsCompany"
            };

            string QueryString = Query.Run();
            Console.WriteLine( $"Query: {QueryString}" );

            QueryRunner Runner = new QueryRunner( "mongodb://localhost:27017", "parent" );
            string Result = Runner.GetJSON( QueryString );

            Console.WriteLine( $"Result: {Result}" );

            Console.Read();
        }

        public static ERModel CreateModel()
        {
            Entity Person = new Entity( "Person" );
            Person.AddAttributes( "personId", "name", "age" );

            Entity Car = new Entity( "Car" );
            Car.AddAttributes( "carId", "model", "year", "driverId" );

            Entity InsCompany = new Entity( "InsCompany" );
            InsCompany.AddAttributes( "companyId", "name" );

            Entity Garage = new Entity( "Garage" );
            Garage.AddAttributes( "garageId", "name" );

            Relationship Drives = new Relationship( "Drives", RelationshipCardinality.OneToMany );
            RelationshipConnection PersonDrivesCar = new RelationshipConnection(
                Person,
                Person.GetAttribute( "personId" ),
                Car,
                Car.GetAttribute( "driverId" ) );
            Drives.AddRelation( PersonDrivesCar );

            Relationship Insurance = new Relationship( "Insurance", RelationshipCardinality.ManyToMany );
            Insurance.AddAttributes( "carId", "companyId" );
            RelationshipConnection CarInsuranceCompany = new RelationshipConnection(
                Car,
                Car.GetAttribute( "carId" ),
                Insurance.GetAttribute( "carId" ),
                InsCompany,
                InsCompany.GetAttribute( "companyId" ),
                Insurance.GetAttribute( "companyId" ) );
            RelationshipConnection InsCompanyCar = new RelationshipConnection(
                InsCompany,
                InsCompany.GetAttribute( "companyId" ),
                Insurance.GetAttribute( "companyId" ),
                Car,
                Car.GetAttribute( "carId" ),
                Insurance.GetAttribute( "carId" ) );

            Insurance.AddRelation( CarInsuranceCompany );
            Insurance.AddRelation( InsCompanyCar );

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

            ERModel Model = new ERModel( "ParentSample", new List<BaseERElement> { Car, Garage, Drives, InsCompany, Insurance, Person, Repaired } );
            return Model;
        }

        public static MongoSchema CreateSchema()
        {
            MongoDBCollection Person = new MongoDBCollection( "Person" );
            Person.AddAttributes( "_id", "name", "age" );

            MongoDBCollection Car = new MongoDBCollection( "Car" );
            Car.AddAttributes( "_id", "model", "year", "driverId" );

            MongoDBCollection InsCompany = new MongoDBCollection( "InsCompany" );
            InsCompany.AddAttributes( "_id", "name" );

            MongoDBCollection Garage = new MongoDBCollection( "Garage" );
            Garage.AddAttributes( "_id", "name" );

            MongoDBCollection Insurance = new MongoDBCollection( "Insurance" );
            Insurance.AddAttributes( "_id", "carId", "companyId" );

            MongoDBCollection Repaired = new MongoDBCollection( "Repaired" );
            Repaired.AddAttributes( "_id", "carId", "garageId" );

            MongoSchema Schema = new MongoSchema( "ParentSchema", new List<MongoDBCollection> { Person, Car, InsCompany, Garage, InsCompany, Insurance, Repaired } );
            return Schema;
        }

        public static ModelMapping CreateMap(ERModel Model, MongoSchema Schema)
        {
            MapRule PersonRule = new MapRule( Model.FindByName( "Person" ), Schema.FindByName( "Person" ) );
            PersonRule.AddRule( "personId", "_id" );
            PersonRule.AddRule( "name", "name" );
            PersonRule.AddRule( "age", "age" );

            MapRule CarRule = new MapRule( Model.FindByName( "Car" ), Schema.FindByName( "Car" ) );
            CarRule.AddRule( "carId", "_id" );
            CarRule.AddRule( "model", "model" );
            CarRule.AddRule( "year", "year" );
            CarRule.AddRule( "driverId", "driverId" );

            MapRule InsCompanyRule = new MapRule( Model.FindByName( "InsCompany" ), Schema.FindByName( "InsCompany" ) );
            InsCompanyRule.AddRule( "companyId", "_id" );
            InsCompanyRule.AddRule( "name", "name" );

            MapRule GarageRule = new MapRule( Model.FindByName( "Garage" ), Schema.FindByName( "Garage" ) );
            GarageRule.AddRule( "garageId", "_id" );
            GarageRule.AddRule( "name", "name" );

            MapRule InsuranceRule = new MapRule( Model.FindByName( "Insurance" ), Schema.FindByName( "Insurance" ) );
            InsuranceRule.AddRule( "carId", "carId" );
            InsuranceRule.AddRule( "companyId", "companyId" );

            MapRule RepairedRule = new MapRule( Model.FindByName( "Repaired" ), Schema.FindByName( "Repaired" ) );
            RepairedRule.AddRule( "carId", "carId" );
            RepairedRule.AddRule( "garageId", "garageId" );
            RepairedRule.AddRule( "repaired", "repaired" );

            ModelMapping Map = new ModelMapping( "ParentMap", new List<MapRule> { PersonRule, CarRule, InsCompanyRule, GarageRule, InsuranceRule, RepairedRule } );

            return Map;
        }
    }
}
