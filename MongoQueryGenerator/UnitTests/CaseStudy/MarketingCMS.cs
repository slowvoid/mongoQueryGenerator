using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using QueryBuilder.ER;
using QueryBuilder.Mongo.Aggregation.Operators;
using QueryBuilder.Operation;
using QueryBuilder.Operation.Arguments;
using QueryBuilder.Query;
using System.Collections.Generic;
using FluentAssertions;

namespace QueryBuilder.Tests
{
    [TestClass]
    public class MarketingCMS
    {
        [TestMethod]
        public void GetAllProductsTest()
        {
            RequiredDataContainer DataMap = MarketingCMSDataProvider.MapEntitiesToCollections();
            RequiredDataContainer DataMap2 = MarketingCMSDataProvider.MapEntitiesToCollectionDuplicates();
            RequiredDataContainer DataMap3 = MarketingCMSDataProvider.MapEntitiesToCollectionCategoryDuplicated();
            RequiredDataContainer DataMap4 = MarketingCMSDataProvider.MapEntitiesToCollectionsStoreDuplicated();
            RequiredDataContainer DataMap5 = MarketingCMSDataProvider.MapEntitiesToCollectionsUserDuplicated();

            QueryableEntity Product = new QueryableEntity( DataMap.EntityRelationshipModel.FindByName( "Product" ) );
            QueryableEntity Store = new QueryableEntity( DataMap.EntityRelationshipModel.FindByName( "Store" ) );
            QueryableEntity Category = new QueryableEntity( DataMap.EntityRelationshipModel.FindByName( "Category" ) );
            QueryableEntity User = new QueryableEntity( DataMap.EntityRelationshipModel.FindByName( "User" ) );

            string Query = _getQueryForTestAllProducts( DataMap, Product, Store, Category, User );
            string Query2 = _getQueryForTestAllProducts( DataMap2, Product, Store, Category, User );
            string Query3 = _getQueryForTestAllProducts( DataMap3, Product, Store, Category, User );
            string Query4 = _getQueryForTestAllProducts( DataMap4, Product, Store, Category, User );
            string Query5 = _getQueryForTestAllProducts( DataMap5, Product, Store, Category, User );

            QueryRunner Runner = new QueryRunner( "mongodb://localhost:27017", "pesquisa_cms" );
            string Result = Runner.GetJSON( Query );

            QueryRunner Runner2 = new QueryRunner( "mongodb://localhost:27017", "pesquisa_cms_duplicados" );
            string Result2 = Runner2.GetJSON( Query2 );

            //QueryRunner Runner3 = new QueryRunner( "mongodb://localhost:27017", "pesquisa_cms_category_duplicado" );
            //string Result3 = Runner3.GetJSON( Query3 );

            //QueryRunner Runner4 = new QueryRunner( "mongodb://localhost:27017", "pesquisa_cms_store_duplicado" );
            //string Result4 = Runner4.GetJSON( Query4 );

            //QueryRunner Runner5 = new QueryRunner( "mongodb://localhost:27017", "pesquisa_cms_user_duplicado" );
            //string Result5 = Runner5.GetJSON( Query5 );

            Assert.IsNotNull( Result );
            Assert.IsNotNull( Result2 );
            //Assert.IsNotNull( Result3 );
            //Assert.IsNotNull( Result4 );
            //Assert.IsNotNull( Result5 );

            JToken ResultJson = JToken.Parse( Result );
            JToken ResultJson2 = JToken.Parse( Result2 );
            //JToken ResultJson3 = JToken.Parse( Result3 );
            //JToken ResultJson4 = JToken.Parse( Result4 );
            //JToken ResultJson5 = JToken.Parse( Result5 );

            //ResultJson.Should().BeEquivalentTo( ResultJson2 );

            Assert.IsTrue( JToken.DeepEquals( ResultJson, JToken.Parse( Result2 ) ) );
            //Assert.IsTrue( JToken.DeepEquals( ResultJson, JToken.Parse( Result3 ) ) );
            //Assert.IsTrue( JToken.DeepEquals( ResultJson, JToken.Parse( Result4 ) ) );
            //Assert.IsTrue( JToken.DeepEquals( ResultJson, JToken.Parse( Result5 ) ) );
        }

        private static string _getQueryForTestAllProducts( RequiredDataContainer DataMap, QueryableEntity Product, QueryableEntity Store, QueryableEntity Category, QueryableEntity User )
        {
            RelationshipJoinOperator RJoinProductUser = new RelationshipJoinOperator(
                            Product,
                            (Relationship)DataMap.EntityRelationshipModel.FindByName( "UserProducts" ),
                            new List<QueryableEntity>() { User },
                            DataMap.ERMongoMapping );

            RelationshipJoinOperator RJoinProductStore = new RelationshipJoinOperator(
                Product,
                (Relationship)DataMap.EntityRelationshipModel.FindByName( "StoreProducts" ),
                new List<QueryableEntity>() { Store },
                DataMap.ERMongoMapping );

            RelationshipJoinOperator RJoinProductCategory = new RelationshipJoinOperator(
                Product,
                (Relationship)DataMap.EntityRelationshipModel.FindByName( "CategoryProducts" ),
                new List<QueryableEntity>() { Category },
                DataMap.ERMongoMapping );

            SortArgument SortArg = new SortArgument( Product, Product.GetAttribute( "product_id" ), MongoDBSort.Ascending );
            SortStage SortOp = new SortStage( new List<SortArgument>() { SortArg }, DataMap.ERMongoMapping );

            List<AlgebraOperator> Operators = new List<AlgebraOperator>() { SortOp, RJoinProductUser, RJoinProductStore, RJoinProductCategory };

            FromArgument FromArg = new FromArgument( Product, DataMap.ERMongoMapping );

            QueryGenerator QueryGen = new QueryGenerator( FromArg, Operators );
            string Query = QueryGen.Run();
            return Query;
        }

        /// <summary>
        /// QUERY: FROM Product
        ///        RJOIN (Store, StoreProducts),
        ///              (Category, CategoryProducts),
        ///              (User, UserProducts)
        /// </summary>
        [TestMethod]
        public void GetAllProducts()
        {
            //RequiredDataContainer DataMap = MarketingCMSDataProvider.MapEntitiesToCollections();
            //RequiredDataContainer DataMapDuplicates = MarketingCMSDataProvider.MapEntitiesToCollectionDuplicates();
            //RequiredDataContainer DataMapCategoryDuplicated = MarketingCMSDataProvider.MapEntitiesToCollectionCategoryDuplicated();
            //RequiredDataContainer DataMapStoreDuplicated = MarketingCMSDataProvider.MapEntitiesToCollectionsStoreDuplicated();
            //RequiredDataContainer DataMapUserDuplicated = MarketingCMSDataProvider.MapEntitiesToCollectionsUserDuplicated();

            //QueryableEntity Product = new QueryableEntity( DataMap.EntityRelationshipModel.FindByName( "Product" ) );
            //QueryableEntity Store = new QueryableEntity( DataMap.EntityRelationshipModel.FindByName( "Store" ) );
            //QueryableEntity Category = new QueryableEntity( DataMap.EntityRelationshipModel.FindByName( "Category" ) );
            //QueryableEntity User = new QueryableEntity( DataMap.EntityRelationshipModel.FindByName( "User" ) );

            
            
            
            
            
            
            //// REMOVE ===
            //RelationshipJoinArgument JoinStoreArgs = new RelationshipJoinArgument(
            //    (Relationship)DataMap.EntityRelationshipModel.FindByName( "ProductBelongsToStore" ),
            //    new List<QueryableEntity>() { Store } );

            //RelationshipJoinArgument JoinCategoryArgs = new RelationshipJoinArgument(
            //    (Relationship)DataMap.EntityRelationshipModel.FindByName( "ProductBelongsToCategory" ),
            //    new List<QueryableEntity>() { Category } );

            //RelationshipJoinArgument JoinUserArgs = new RelationshipJoinArgument(
            //    (Relationship)DataMap.EntityRelationshipModel.FindByName( "ProductBelongsToUser" ),
            //    new List<QueryableEntity>() { User } );

            //RelationshipJoinOperator RJoinMap1Op = new RelationshipJoinOperator( Product,
            //    new List<RelationshipJoinArgument>() { JoinStoreArgs, JoinCategoryArgs, JoinUserArgs },
            //    DataMap.ERMongoMapping );
            //// END REMOVE ===

            //// DEBUG
            ////LimitStage LimitOp = new LimitStage( 150000 );
            //SortArgument SortArg = new SortArgument( Product, Product.GetAttribute( "product_id" ), MongoDBSort.Ascending );
            //SortStage SortOp = new SortStage( new List<SortArgument>() { SortArg }, DataMap.ERMongoMapping );
            //SortStage SortOp2 = new SortStage( new List<SortArgument>() { SortArg }, DataMapDuplicates.ERMongoMapping );
            //SortStage SortOp3 = new SortStage( new List<SortArgument>() { SortArg }, DataMapCategoryDuplicated.ERMongoMapping );
            //SortStage SortOp4 = new SortStage( new List<SortArgument>() { SortArg }, DataMapStoreDuplicated.ERMongoMapping );
            //SortStage SortOp5 = new SortStage( new List<SortArgument>() { SortArg }, DataMapUserDuplicated.ERMongoMapping );
            //// =====

            //// Build and execute Map1 query
            //List<AlgebraOperator> Map1OperatorsList = new List<AlgebraOperator>() { RJoinMap1Op, SortOp };

            //FromArgument StartArgMap1 = new FromArgument( Product, DataMap.ERMongoMapping );
            //QueryGenerator Generator = new QueryGenerator( StartArgMap1, Map1OperatorsList );

            //string Map1Query = Generator.Run();

            //Assert.IsNotNull( Map1Query, "Generated query [Map1Query] cannot be null" );

            //RelationshipJoinOperator RJoinMapDuplicatesOp = new RelationshipJoinOperator( Product,
            //    new List<RelationshipJoinArgument>() { JoinStoreArgs, JoinCategoryArgs, JoinUserArgs },
            //    DataMapDuplicates.ERMongoMapping );

            //List<AlgebraOperator> MapDuplicatesOpList = new List<AlgebraOperator>() { RJoinMapDuplicatesOp, SortOp2 };

            //FromArgument StartArgMap2 = new FromArgument( Product, DataMapDuplicates.ERMongoMapping );
            //QueryGenerator GeneratorDuplicates = new QueryGenerator( StartArgMap2, MapDuplicatesOpList );

            //string MapDuplicatesQuery = GeneratorDuplicates.Run();

            //Assert.IsNotNull( MapDuplicatesQuery, "Generated query [MapDuplicatesQuery] cannot be null" );

            //RelationshipJoinOperator RJoinMapCategoryDuplicatedOp = new RelationshipJoinOperator( Product,
            //    new List<RelationshipJoinArgument>() { JoinStoreArgs, JoinCategoryArgs, JoinUserArgs },
            //    DataMapCategoryDuplicated.ERMongoMapping );

            //List<AlgebraOperator> MapCategoryDuplicatedOpList = new List<AlgebraOperator>() { RJoinMapCategoryDuplicatedOp, SortOp3 };

            //FromArgument StartArgCategoryDuplicated = new FromArgument( Product, DataMapCategoryDuplicated.ERMongoMapping );
            //QueryGenerator GeneratorCategoryDuplicated = new QueryGenerator( StartArgCategoryDuplicated, MapCategoryDuplicatedOpList );

            //string MapCategoryDuplicatedQuery = GeneratorCategoryDuplicated.Run();

            //Assert.IsNotNull( MapCategoryDuplicatedQuery, "Generated query [MapCategoryDuplicatedQuery] cannot be null" );

            //RelationshipJoinOperator RJoinMapStoreDuplicatedOp = new RelationshipJoinOperator( Product,
            //    new List<RelationshipJoinArgument>() { JoinStoreArgs, JoinCategoryArgs, JoinUserArgs },
            //    DataMapStoreDuplicated.ERMongoMapping );

            //List<AlgebraOperator> MapStoreDuplicatedOpList = new List<AlgebraOperator>() { RJoinMapStoreDuplicatedOp, SortOp4 };

            //FromArgument StartArgStoreDuplicated = new FromArgument( Product, DataMapStoreDuplicated.ERMongoMapping );
            //QueryGenerator GeneratorStoreDuplicated = new QueryGenerator( StartArgStoreDuplicated, MapStoreDuplicatedOpList );

            //string MapStoreDuplicatedQuery = GeneratorStoreDuplicated.Run();

            //Assert.IsNotNull( MapStoreDuplicatedQuery, "Generated query [MapStoreDuplicatedQuery] cannot be null" );

            //RelationshipJoinOperator RJoinMapUserDuplicatedOp = new RelationshipJoinOperator( Product,
            //    new List<RelationshipJoinArgument>() { JoinUserArgs, JoinCategoryArgs, JoinStoreArgs },
            //    DataMapUserDuplicated.ERMongoMapping );

            //List<AlgebraOperator> MapUserDuplicatedOpList = new List<AlgebraOperator>() { RJoinMapUserDuplicatedOp, SortOp5 };

            //FromArgument StartArgUserDuplicated = new FromArgument( Product, DataMapUserDuplicated.ERMongoMapping );
            //QueryGenerator GeneratorUserDuplicated = new QueryGenerator( StartArgUserDuplicated, MapUserDuplicatedOpList );

            //string MapUserDuplicatedQuery = GeneratorUserDuplicated.Run();

            //Assert.IsNotNull( MapUserDuplicatedQuery, "Generated query [MapUserDuplicatedQuery] cannot be null" );

            //QueryRunner RunnerMap1 = new QueryRunner( "mongodb://localhost:27017", "pesquisa_cms" );
            //QueryRunner RunnerMap2 = new QueryRunner( "mongodb://localhost:27017", "pesquisa_cms_duplicados" );
            //QueryRunner RunnerMap3 = new QueryRunner( "mongodb://localhost:27017", "pesquisa_cms_category_duplicado" );
            //QueryRunner RunnerMap4 = new QueryRunner( "mongodb://localhost:27017", "pesquisa_cms_store_duplicado" );
            //QueryRunner RunnerMap5 = new QueryRunner( "mongodb://localhost:27017", "pesquisa_cms_user_duplicado" );

            //string ResultMap1 = RunnerMap1.GetJSON( Map1Query );
            //string ResultMap2 = RunnerMap2.GetJSON( MapDuplicatesQuery );
            //string ResultMap3 = RunnerMap3.GetJSON( MapCategoryDuplicatedQuery );
            //string ResultMap4 = RunnerMap4.GetJSON( MapStoreDuplicatedQuery );
            //string ResultMap5 = RunnerMap5.GetJSON( MapUserDuplicatedQuery );

            //Assert.IsNotNull( ResultMap1, "Result [Map1] cannot be null" );
            //Assert.IsNotNull( ResultMap2, "Result [Map2] cannot be null" );
            //Assert.IsNotNull( ResultMap3, "Result [Map3] cannot be null" );
            //Assert.IsNotNull( ResultMap4, "Result [Map4] cannot be null" );
            //Assert.IsNotNull( ResultMap5, "Result [Map5] cannot be null" );

            //Assert.IsTrue( JToken.DeepEquals( JToken.Parse( ResultMap1 ), JToken.Parse( ResultMap2 ) ) );
            //Assert.IsTrue( JToken.DeepEquals( JToken.Parse( ResultMap1 ), JToken.Parse( ResultMap3 ) ) );
            //Assert.IsTrue( JToken.DeepEquals( JToken.Parse( ResultMap1 ), JToken.Parse( ResultMap4 ) ) );
            //Assert.IsTrue( JToken.DeepEquals( JToken.Parse( ResultMap1 ), JToken.Parse( ResultMap5 ) ) );
        }
    }
}