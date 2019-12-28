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
        public void GetAllProducts()
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

            QueryRunner Runner3 = new QueryRunner( "mongodb://localhost:27017", "pesquisa_cms_category_duplicado" );
            string Result3 = Runner3.GetJSON( Query3 );

            QueryRunner Runner4 = new QueryRunner( "mongodb://localhost:27017", "pesquisa_cms_store_duplicado" );
            string Result4 = Runner4.GetJSON( Query4 );

            QueryRunner Runner5 = new QueryRunner( "mongodb://localhost:27017", "pesquisa_cms_user_duplicado" );
            string Result5 = Runner5.GetJSON( Query5 );

            Assert.IsNotNull( Result );
            Assert.IsNotNull( Result2 );
            Assert.IsNotNull( Result3 );
            Assert.IsNotNull( Result4 );
            Assert.IsNotNull( Result5 );

            JToken ResultJson = JToken.Parse( Result );

            Assert.IsTrue( JToken.DeepEquals( ResultJson, JToken.Parse( Result2 ) ) );
            Assert.IsTrue( JToken.DeepEquals( ResultJson, JToken.Parse( Result3 ) ) );
            Assert.IsTrue( JToken.DeepEquals( ResultJson, JToken.Parse( Result4 ) ) );
            Assert.IsTrue( JToken.DeepEquals( ResultJson, JToken.Parse( Result5 ) ) );
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

            SortArgument SortArg = new SortArgument( Product, Product.GetAttribute( "ProductID" ), MongoDBSort.Ascending );
            SortStage SortOp = new SortStage( new List<SortArgument>() { SortArg }, DataMap.ERMongoMapping );

            List<AlgebraOperator> Operators = new List<AlgebraOperator>() { SortOp, RJoinProductUser, RJoinProductStore, RJoinProductCategory };

            FromArgument FromArg = new FromArgument( Product, DataMap.ERMongoMapping );

            QueryGenerator QueryGen = new QueryGenerator( FromArg, Operators );
            string Query = QueryGen.Run();
            return Query;
        } 
    }
}