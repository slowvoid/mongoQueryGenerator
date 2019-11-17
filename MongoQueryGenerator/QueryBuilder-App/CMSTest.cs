using QueryBuilder.ER;
using QueryBuilder.Mongo.Aggregation.Operators;
using QueryBuilder.Operation;
using QueryBuilder.Operation.Arguments;
using QueryBuilder.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json.Linq;

namespace QueryBuilderApp
{
    public static class CMSTest
    {
        public static void Main()
        {
            Console.WriteLine( "Running test" );

            RunTest();

            Console.WriteLine( "Done" );
            Console.Read();
        }

        public static void RunTest()
        {
            RequiredDataContainer DataMap = MarketingCMSDataProvider.MapEntitiesToCollections();
            RequiredDataContainer DataMapDuplicates = MarketingCMSDataProvider.MapEntitiesToCollectionDuplicates();
            RequiredDataContainer DataMapCategoryDuplicated = MarketingCMSDataProvider.MapEntitiesToCollectionCategoryDuplicated();
            RequiredDataContainer DataMapStoreDuplicated = MarketingCMSDataProvider.MapEntitiesToCollectionsStoreDuplicated();
            RequiredDataContainer DataMapUserDuplicated = MarketingCMSDataProvider.MapEntitiesToCollectionsUserDuplicated();

            QueryableEntity Store = new QueryableEntity( DataMap.EntityRelationshipModel.FindByName( "Store" ) );
            QueryableEntity Product = new QueryableEntity( DataMap.EntityRelationshipModel.FindByName( "Product" ) );

            RelationshipJoinArgument JoinProductArg = new RelationshipJoinArgument( (Relationship)DataMap.EntityRelationshipModel.FindByName( "StoreHasManyProducts" ),
                new List<QueryableEntity>() { Product } );

            RelationshipJoinOperator JoinOpMap1 = new RelationshipJoinOperator( Store,
                new List<RelationshipJoinArgument>() { JoinProductArg }, DataMap.ERMongoMapping );

            RelationshipJoinOperator JoinOpMap2 = new RelationshipJoinOperator( Store,
                new List<RelationshipJoinArgument>() { JoinProductArg }, DataMapDuplicates.ERMongoMapping );

            RelationshipJoinOperator JoinOpMap3 = new RelationshipJoinOperator( Store,
                new List<RelationshipJoinArgument>() { JoinProductArg }, DataMapCategoryDuplicated.ERMongoMapping );

            RelationshipJoinOperator JoinOpMap4 = new RelationshipJoinOperator( Store,
                new List<RelationshipJoinArgument>() { JoinProductArg }, DataMapStoreDuplicated.ERMongoMapping );

            RelationshipJoinOperator JoinOpMap5 = new RelationshipJoinOperator( Store,
                new List<RelationshipJoinArgument>() { JoinProductArg }, DataMapUserDuplicated.ERMongoMapping );

            SortArgument SortArg = new SortArgument( Store, Store.GetAttribute( "store_id" ), MongoDBSort.Ascending );

            SortStage SortOpMap1 = new SortStage( new List<SortArgument>() { SortArg }, DataMap.ERMongoMapping );
            SortStage SortOpMap2 = new SortStage( new List<SortArgument>() { SortArg }, DataMapDuplicates.ERMongoMapping );
            SortStage SortOpMap3 = new SortStage( new List<SortArgument>() { SortArg }, DataMapCategoryDuplicated.ERMongoMapping );
            SortStage SortOpMap4 = new SortStage( new List<SortArgument>() { SortArg }, DataMapStoreDuplicated.ERMongoMapping );
            SortStage SortOpMap5 = new SortStage( new List<SortArgument>() { SortArg }, DataMapUserDuplicated.ERMongoMapping );

            List<AlgebraOperator> OperatorsToExecuteMap1 = new List<AlgebraOperator>() { JoinOpMap1, SortOpMap1 };
            List<AlgebraOperator> OperatorsToExecuteMap2 = new List<AlgebraOperator>() { JoinOpMap2, SortOpMap2 };
            List<AlgebraOperator> OperatorsToExecuteMap3 = new List<AlgebraOperator>() { JoinOpMap3, SortOpMap3 };
            List<AlgebraOperator> OperatorsToExecuteMap4 = new List<AlgebraOperator>() { JoinOpMap4, SortOpMap4 };
            List<AlgebraOperator> OperatorsToExecuteMap5 = new List<AlgebraOperator>() { JoinOpMap5, SortOpMap5 };

            FromArgument StartArgMap1 = new FromArgument( Store, DataMap.ERMongoMapping );
            FromArgument StartArgMap2 = new FromArgument( Store, DataMapDuplicates.ERMongoMapping );
            FromArgument StartArgMap3 = new FromArgument( Store, DataMapCategoryDuplicated.ERMongoMapping );
            FromArgument StartArgMap4 = new FromArgument( Store, DataMapStoreDuplicated.ERMongoMapping );
            FromArgument StartArgMap5 = new FromArgument( Store, DataMapUserDuplicated.ERMongoMapping );

            QueryGenerator GeneratorMap1 = new QueryGenerator( StartArgMap1, OperatorsToExecuteMap1 );
            QueryGenerator GeneratorMap2 = new QueryGenerator( StartArgMap2, OperatorsToExecuteMap2 );
            QueryGenerator GeneratorMap3 = new QueryGenerator( StartArgMap3, OperatorsToExecuteMap3 );
            QueryGenerator GeneratorMap4 = new QueryGenerator( StartArgMap4, OperatorsToExecuteMap4 );
            QueryGenerator GeneratorMap5 = new QueryGenerator( StartArgMap5, OperatorsToExecuteMap5 );

            string QueryMap1 = GeneratorMap1.Run();
            string QueryMap2 = GeneratorMap2.Run();
            string QueryMap3 = GeneratorMap3.Run();
            string QueryMap4 = GeneratorMap4.Run();
            string QueryMap5 = GeneratorMap5.Run();

            //QueryRunner RunnerMap1 = new QueryRunner( "mongodb://localhost:27017", "pesquisa_cms" );
            //QueryRunner RunnerMap2 = new QueryRunner( "mongodb://localhost:27017", "pesquisa_cms_duplicados" );
            //QueryRunner RunnerMap3 = new QueryRunner( "mongodb://localhost:27017", "pesquisa_cms_category_duplicado" );
            //QueryRunner RunnerMap4 = new QueryRunner( "mongodb://localhost:27017", "pesquisa_cms_store_duplicado" );
            //QueryRunner RunnerMap5 = new QueryRunner( "mongodb://localhost:27017", "pesquisa_cms_user_duplicado" );

            //string ResultMap1 = RunnerMap1.GetJSON( QueryMap1 );
            //string ResultMap2 = RunnerMap2.GetJSON( QueryMap2 );
            //string ResultMap3 = RunnerMap3.GetJSON( QueryMap3 );
            //string ResultMap4 = RunnerMap4.GetJSON( QueryMap4 );
            //string ResultMap5 = RunnerMap5.GetJSON( QueryMap5 );

            //Console.WriteLine( "Equals? {0}", JToken.DeepEquals( JToken.Parse( ResultMap1 ), JToken.Parse( ResultMap2 ) ) );
        }
    }
}
