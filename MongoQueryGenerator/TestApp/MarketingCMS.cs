using Newtonsoft.Json.Linq;
using QueryBuilder.ER;
using QueryBuilder.Mongo.Aggregation.Operators;
using QueryBuilder.Operation;
using QueryBuilder.Operation.Arguments;
using QueryBuilder.Query;
using System.Collections.Generic;


namespace TestApp
{
    public class MarketingCMS
    {
        public void GetAllProductsTest()
        {
            DataContainer DataMap = MarketingCMSDataProvider.MapEntitiesToCollections();
            DataContainer DataMap2 = MarketingCMSDataProvider.MapEntitiesToCollectionDuplicates();
            DataContainer DataMap3 = MarketingCMSDataProvider.MapEntitiesToCollectionCategoryDuplicated();
            DataContainer DataMap4 = MarketingCMSDataProvider.MapEntitiesToCollectionsStoreDuplicated();
            DataContainer DataMap5 = MarketingCMSDataProvider.MapEntitiesToCollectionsUserDuplicated();

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

            JToken ResultJson = JToken.Parse( Result );
            JToken ResultJson2 = JToken.Parse( Result2 );
            //JToken ResultJson3 = JToken.Parse( Result3 );
            //JToken ResultJson4 = JToken.Parse( Result4 );
            //JToken ResultJson5 = JToken.Parse( Result5 );
        }

        public static string _getQueryForTestAllProducts( DataContainer DataMap, QueryableEntity Product, QueryableEntity Store, QueryableEntity Category, QueryableEntity User )
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

            List<AlgebraOperator> Operators = new List<AlgebraOperator>() { RJoinProductUser, RJoinProductStore, RJoinProductCategory, SortOp };

            FromArgument FromArg = new FromArgument( Product, DataMap.ERMongoMapping );

            QueryGenerator QueryGen = new QueryGenerator( FromArg, Operators );
            string Query = QueryGen.Run();
            return Query;
        }
    }
}