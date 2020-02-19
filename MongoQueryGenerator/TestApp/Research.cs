using QueryBuilder.ER;
using QueryBuilder.Map;
using QueryBuilder.Mongo;
using QueryBuilder.Mongo.Aggregation.Operators;
using QueryBuilder.Mongo.Expressions;
using QueryBuilder.Operation;
using QueryBuilder.Operation.Arguments;
using QueryBuilder.Query;
using System;
using System.Collections.Generic;

namespace TestApp
{
    public static class ResearchApp
    {
        public static void Main()
        {
            DataContainer data = CreateDataContainer();

            QueryableEntity Person = new QueryableEntity( data.EntityRelationshipModel.FindByName( "Person" ) );
            QueryableEntity Car = new QueryableEntity( data.EntityRelationshipModel.FindByName( "Car" ) );
            Relationship Drives = (Relationship)data.EntityRelationshipModel.FindByName( "Drives" );
            ModelMapping Map = data.ERMongoMapping;

            QueryableEntity Garage = new QueryableEntity( data.EntityRelationshipModel.FindByName( "Garage" ) );

            RelationshipJoinOperator RJoinDrives = new RelationshipJoinOperator(
                Person,
                Drives,
                new List<QueryableEntity>() { Car },
                Map );

            CartesianProductOperator CartesianOp = new CartesianProductOperator(
                Person,
                Garage,
                Map );

            ProjectArgument ProjectArg1 = new ProjectArgument( Person.GetAttribute( "name" ), Person, new BooleanExpr( true ) );
            ProjectArgument ProjectArg2 = new ProjectArgument( Person.GetAttribute( "surname" ), Person, new BooleanExpr( true ) );
            ProjectArgument ProjectArg3 = new ProjectArgument( Car.GetAttribute( "reg_no" ), Car, new BooleanExpr( true ) );

            ProjectStage ProjectOp = new ProjectStage( new ProjectArgument[] { ProjectArg1, ProjectArg2, ProjectArg3 }, RJoinDrives.ComputeVirtualMap() );

            MapRule PersonRule = Map.FindMainRule( Person.Element );
            SelectArgument SelectArg = new SelectArgument( new EqExpr( $"${PersonRule.GetRuleValueForAttribute( Person.GetAttribute( "surname" ) )}", "Smith" ) );
            SelectStage SelectOp = new SelectStage( SelectArg, Map );

            //List<MongoDBOperator> Operations = RJoinDrives.Run().Commands;

            //foreach ( MongoDBOperator Op in Operations )
            //{
            //    Console.WriteLine( Op.GetType().Name );
            //    Console.WriteLine( Op.ToString() );
            //    Console.WriteLine( Op.ToJavaScript() );

            //    if ( Op is LookupOperator )
            //    {
            //        LookupOperator OpAsLookup = (LookupOperator)Op;
            //        foreach ( MongoDBOperator PipelineOp in OpAsLookup.Pipeline )
            //        {
            //            Console.WriteLine( PipelineOp.GetType().Name );
            //            Console.WriteLine( PipelineOp.ToString() );
            //            Console.WriteLine( PipelineOp.ToJavaScript() );
            //        }
            //    }
            //}

            QueryGenerator QueryGen = new QueryGenerator( new FromArgument( Person, Map ), new List<AlgebraOperator>() { SelectOp } );
            string Query = QueryGen.Run();

            Console.WriteLine( $"Query: {Environment.NewLine}{Query}" );

            Console.Read();
        }

        public static DataContainer CreateDataContainer()
        {
            // ER MODEL
            Entity Person = new Entity( "Person" );
            Person.AddAttribute( "id", true );
            Person.AddAttributes( "name", "surname", "salary" );

            Entity Car = new Entity( "Car" );
            Car.AddAttribute( "id", true );
            Car.AddAttributes( "reg_no" );

            Entity InsuranceCompany = new Entity( "InsuranceCompany" );
            InsuranceCompany.AddAttribute( "id", true );
            InsuranceCompany.AddAttributes( "name" );

            Entity Garage = new Entity( "Garage" );
            Garage.AddAttribute( "id", true );
            Garage.AddAttributes( "name", "phone" );

            Relationship Insurance = new Relationship( "Insurance" );
            Insurance.AddAttributes( "contract" );
            Insurance.AddRelationshipEnd( new RelationshipEnd( Person ) );
            Insurance.AddRelationshipEnd( new RelationshipEnd( Car ) );
            Insurance.AddRelationshipEnd( new RelationshipEnd( InsuranceCompany ) );

            Relationship Drives = new Relationship( "Drives" );
            Drives.AddRelationshipEnd( new RelationshipEnd( Person ) );
            Drives.AddRelationshipEnd( new RelationshipEnd( Car ) );

            Relationship Repaired = new Relationship( "Repaired" );
            Repaired.AddAttributes( "date" );
            Repaired.AddRelationshipEnd( new RelationshipEnd( Car ) );
            Repaired.AddRelationshipEnd( new RelationshipEnd( Garage ) );

            ERModel Model = new ERModel( "SampleModel", new List<BaseERElement>() { Person, Car, InsuranceCompany, Garage, Insurance, Drives, Repaired } );

            // SCHEMA
            MongoDBCollection PersonCol = new MongoDBCollection( "PersonCollection" );
            PersonCol.AddAttributes( "_id", "fName", "fSurname", "fSalary" );

            MongoDBCollection CarCol = new MongoDBCollection( "CarCollection" );
            CarCol.AddAttributes( "_id", "fReg_no" );

            MongoDBCollection InsuranceCompanyCol = new MongoDBCollection( "InsuranceCompanyCollection" );
            InsuranceCompanyCol.AddAttributes( "_id", "fName" );

            MongoDBCollection GarageCol = new MongoDBCollection( "GarageCollection" );
            GarageCol.AddAttributes( "_id", "fName", "fPhone" );

            MongoDBCollection InsuranceCol = new MongoDBCollection( "InsuranceCollection" );
            InsuranceCol.AddAttributes( "contract" );

            MongoDBCollection DrivesCol = new MongoDBCollection( "DrivesCollection" );
            DrivesCol.AddAttributes( "fPersonId", "fCarId" );

            MongoDBCollection RepairedCol = new MongoDBCollection( "RepairedCollection" );
            RepairedCol.AddAttributes( "fCarId", "fGarageId", "fDate" );

            MongoSchema Schema = new MongoSchema( "SampleSchema", new List<MongoDBCollection>() { PersonCol, CarCol, InsuranceCompanyCol, GarageCol, InsuranceCol, DrivesCol, RepairedCol } );

            // Mapping
            MapRule PersonRule = new MapRule( Person, PersonCol );
            PersonRule.AddRule( "id", "_id" );
            PersonRule.AddRule( "name", "fName" );
            PersonRule.AddRule( "surname", "fSurname" );
            PersonRule.AddRule( "salary", "fSalary" );

            MapRule CarRule = new MapRule( Car, CarCol );
            CarRule.AddRule( "id", "_id" );
            CarRule.AddRule( "reg_no", "fReg_no" );

            MapRule InsCompanyRule = new MapRule( InsuranceCompany, InsuranceCompanyCol );
            InsCompanyRule.AddRule( "id", "_id" );
            InsCompanyRule.AddRule( "name", "fName" );

            MapRule GarageRule = new MapRule( Garage, GarageCol );
            GarageRule.AddRule( "id", "_id" );
            GarageRule.AddRule( "name", "fName" );
            GarageRule.AddRule( "phone", "fPhone" );

            MapRule InsuranceRule = new MapRule( Insurance, InsuranceCol );
            InsuranceRule.AddRule( "contract", "contract" );

            MapRule PersonInsuranceRule = new MapRule( Person, InsuranceCol, false );
            PersonInsuranceRule.AddRule( "id", "fPersonId" );

            MapRule CarInsuranceRule = new MapRule( Car, InsuranceCol, false );
            CarInsuranceRule.AddRule( "id", "fCarId" );

            MapRule InsCompanyInsuranceRule = new MapRule( InsuranceCompany, InsuranceCol, false );
            InsCompanyInsuranceRule.AddRule( "id", "fInsCoId" );

            MapRule DrivesRule = new MapRule( Drives, DrivesCol );

            MapRule PersonDrivesRule = new MapRule( Person, DrivesCol, false );
            PersonDrivesRule.AddRule( "id", "fPersonId" );

            MapRule CarDrivesRule = new MapRule( Car, DrivesCol, false );
            CarDrivesRule.AddRule( "id", "fCarId" );

            MapRule RepairedRule = new MapRule( Repaired, RepairedCol );
            RepairedRule.AddRule( "date", "fDate" );

            MapRule CarRepairedRule = new MapRule( Car, RepairedCol, false );
            CarRepairedRule.AddRule( "id", "fCarId" );

            MapRule GarageRepairedRule = new MapRule( Garage, RepairedCol, false );
            GarageRepairedRule.AddRule( "id", "fGarageId" );

            ModelMapping Mapping = new ModelMapping( "SampleMapping", new List<MapRule>() { PersonRule, CarRule, InsCompanyRule, GarageRule, InsuranceRule, PersonInsuranceRule, CarInsuranceRule, InsCompanyInsuranceRule, DrivesRule, PersonDrivesRule, CarDrivesRule, RepairedRule, CarRepairedRule, GarageRepairedRule } );

            return new DataContainer( Model, Schema, Mapping );
        }
    }
}