using QueryBuilder.ER;
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
    public static class MultipleEntitiesApp
    {
        public static void Main()
        {
            Console.WriteLine( "Testing multiple entities" );

            ERModel Model = MultipleEntitiesDataProvider.CreateModel();
            MongoSchema Schema = MultipleEntitiesDataProvider.CreateSchema();
            ModelMapping Map = MultipleEntitiesDataProvider.CreateMap( Model, Schema );

            QueryableEntity Person = new QueryableEntity( Model.FindByName( "Person" ) );
            QueryableEntity Pet = new QueryableEntity( Model.FindByName( "Pet" ) );
            QueryableEntity Job = new QueryableEntity( Model.FindByName( "Job" ) );
            QueryableEntity Car = new QueryableEntity( Model.FindByName( "Car" ) );
            QueryableEntity Contract = new QueryableEntity( Model.FindByName( "Contract" ) );
            QueryableEntity Company = new QueryableEntity( Model.FindByName( "Company" ) );

            RelationshipJoinArgument Arg1 = new RelationshipJoinArgument(
                (Relationship)Model.FindByName( "HasPet" ),
                new List<QueryableEntity>() { Pet } );
            RelationshipJoinArgument Arg2 = new RelationshipJoinArgument(
                (Relationship)Model.FindByName( "HasJob" ),
                new List<QueryableEntity>() { Job } );

            RelationshipJoinArgument Arg3 = new RelationshipJoinArgument(
                (Relationship)Model.FindByName( "Drives" ),
                new List<QueryableEntity>() { Car, Company } );
            RelationshipJoinArgument Arg4 = new RelationshipJoinArgument(
                (Relationship)Model.FindByName( "HasContract" ),
                new List<QueryableEntity>() { Contract, Company } );

            RelationshipJoinOperator RJoinOp1 = new RelationshipJoinOperator( Person, new List<RelationshipJoinArgument>() { Arg1 }, Map );
            RelationshipJoinOperator RJoinOp2 = new RelationshipJoinOperator( Person, new List<RelationshipJoinArgument>() { Arg2 }, Map );

            RelationshipJoinOperator RJoinOp3 = new RelationshipJoinOperator( Person, new List<RelationshipJoinArgument>() { Arg3 }, Map );
            RelationshipJoinOperator RJoinOp4 = new RelationshipJoinOperator( Person, new List<RelationshipJoinArgument>() { Arg4 }, Map );

            VirtualMap VMap1 = RJoinOp1.ComputeVirtualMap();
            VirtualMap VMap2 = RJoinOp2.ComputeVirtualMap( VMap1 );
            VirtualMap VMap3 = RJoinOp3.ComputeVirtualMap( VMap2 );
            VirtualMap VMap4 = RJoinOp4.ComputeVirtualMap( VMap3 );

            Console.WriteLine( "Virtual map from RJoinOp1 {0}{1}", Environment.NewLine, VMap1.ToString() );
            Console.WriteLine( "Virtual map from RJoinOp2 {0}{1}", Environment.NewLine, VMap2.ToString() );
            Console.WriteLine( "Virtual map from RJoinOp3 {0}{1}", Environment.NewLine, VMap3.ToString() );
            Console.WriteLine( "Virtual map from RJoinOp4 {0}{1}", Environment.NewLine, VMap4.ToString() );

            FromArgument StartArg = new FromArgument( Person, Map );
            QueryGenerator QueryGen = new QueryGenerator( StartArg, new List<AlgebraOperator>() { RJoinOp1, RJoinOp2, RJoinOp3, RJoinOp4 } );

            string QueryString = QueryGen.Run();
            Console.WriteLine( "Query: {0}{1}", Environment.NewLine, QueryString );

            QueryRunner Runner = new QueryRunner( "mongodb://localhost:27017", "multipleEntitiesTest" );
            Console.WriteLine( "Query Result: {0}{1}", Environment.NewLine, Runner.GetJSON( QueryString ) );

            Console.Read();
        }
    }
}
