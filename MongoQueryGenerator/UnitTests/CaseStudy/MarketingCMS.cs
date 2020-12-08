using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using QueryBuilder.ER;
using QueryBuilder.Mongo.Aggregation.Operators;
using QueryBuilder.Operation;
using QueryBuilder.Operation.Arguments;
using QueryBuilder.Query;
using System.Collections.Generic;
using FluentAssertions;
using QueryBuilder.Operation.Exceptions;
using QueryBuilder.Mongo.Expressions;
using QueryBuilder.Map;
using QueryBuilder.Parser;

namespace QueryBuilder.Tests
{
    [TestClass]
    public class MarketingCMS
    {
        /// <summary>
        /// Query:
        /// 
        /// FROM Product p
        /// RJOIN <UserProducts> (User u)
        /// RJOIN <StoreProducts> (Store s)
        /// RJOIN <CategoryProducts> (Category c)
        /// SELECT *
        /// </summary>
        [TestMethod]
        public void GetAllProducts()
        {
            //RequiredDataContainer DataMap = MarketingCMSDataProvider.MapEntitiesToCollections();
            //RequiredDataContainer DataMap2 = MarketingCMSDataProvider.MapEntitiesToCollectionDuplicates();
            //RequiredDataContainer DataMap3 = MarketingCMSDataProvider.MapEntitiesToCollectionCategoryDuplicated();
            //RequiredDataContainer DataMap4 = MarketingCMSDataProvider.MapEntitiesToCollectionsStoreDuplicated();
            //RequiredDataContainer DataMap5 = MarketingCMSDataProvider.MapEntitiesToCollectionsUserDuplicated();
            var DataMap = QueryBuilderParser.ParseMapping( Utils.ReadMappingFile( "Mappings/MarketingCMS/entities-to-collections.mapping" ) );
            var DataMap2 = QueryBuilderParser.ParseMapping( Utils.ReadMappingFile( "Mappings/MarketingCMS/entities-to-collections-duplicates.mapping" ) );
            var DataMap3 = QueryBuilderParser.ParseMapping( Utils.ReadMappingFile( "Mappings/MarketingCMS/entities-to-collections-category-duplicated.mapping" ) );
            var DataMap4 = QueryBuilderParser.ParseMapping( Utils.ReadMappingFile( "Mappings/MarketingCMS/entities-to-collections-store-duplicated.mapping" ) );
            var DataMap5 = QueryBuilderParser.ParseMapping( Utils.ReadMappingFile( "Mappings/MarketingCMS/entities-to-collections-user-duplicated.mapping" ) );

            QueryableEntity Product = new QueryableEntity( DataMap.EntityRelationshipModel.FindByName( "Product" ) );
            QueryableEntity Store = new QueryableEntity( DataMap.EntityRelationshipModel.FindByName( "Store" ) );
            QueryableEntity Category = new QueryableEntity( DataMap.EntityRelationshipModel.FindByName( "Category" ) );
            QueryableEntity User = new QueryableEntity( DataMap.EntityRelationshipModel.FindByName( "User" ) );

            QueryableEntity Product2 = new QueryableEntity( DataMap2.EntityRelationshipModel.FindByName( "Product" ) );
            QueryableEntity Store2 = new QueryableEntity( DataMap2.EntityRelationshipModel.FindByName( "Store" ) );
            QueryableEntity Category2 = new QueryableEntity( DataMap2.EntityRelationshipModel.FindByName( "Category" ) );
            QueryableEntity User2 = new QueryableEntity( DataMap2.EntityRelationshipModel.FindByName( "User" ) );

            QueryableEntity Product3 = new QueryableEntity( DataMap3.EntityRelationshipModel.FindByName( "Product" ) );
            QueryableEntity Store3 = new QueryableEntity( DataMap3.EntityRelationshipModel.FindByName( "Store" ) );
            QueryableEntity Category3 = new QueryableEntity( DataMap3.EntityRelationshipModel.FindByName( "Category" ) );
            QueryableEntity User3 = new QueryableEntity( DataMap3.EntityRelationshipModel.FindByName( "User" ) );

            QueryableEntity Product4 = new QueryableEntity( DataMap4.EntityRelationshipModel.FindByName( "Product" ) );
            QueryableEntity Store4 = new QueryableEntity( DataMap4.EntityRelationshipModel.FindByName( "Store" ) );
            QueryableEntity Category4 = new QueryableEntity( DataMap4.EntityRelationshipModel.FindByName( "Category" ) );
            QueryableEntity User4 = new QueryableEntity( DataMap4.EntityRelationshipModel.FindByName( "User" ) );

            QueryableEntity Product5 = new QueryableEntity( DataMap5.EntityRelationshipModel.FindByName( "Product" ) );
            QueryableEntity Store5 = new QueryableEntity( DataMap5.EntityRelationshipModel.FindByName( "Store" ) );
            QueryableEntity Category5 = new QueryableEntity( DataMap5.EntityRelationshipModel.FindByName( "Category" ) );
            QueryableEntity User5 = new QueryableEntity( DataMap5.EntityRelationshipModel.FindByName( "User" ) );

            string Query = _getQueryForTestAllProducts( DataMap, Product, Store, Category, User );
            string Query2 = _getQueryForTestAllProducts( DataMap2, Product2, Store2, Category2, User2 );
            string Query3 = _getQueryForTestAllProducts( DataMap3, Product3, Store3, Category3, User3 );
            string Query4 = _getQueryForTestAllProducts( DataMap4, Product4, Store4, Category4, User4 );
            string Query5 = _getQueryForTestAllProducts( DataMap5, Product5, Store5, Category5, User5 );

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

        private static string _getQueryForTestAllProducts( QueryBuilderMappingMetadata DataMap, QueryableEntity Product, QueryableEntity Store, QueryableEntity Category, QueryableEntity User )
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
        /// <summary>
        /// Get All Stores test
        /// 
        /// Query: FROM Store SELECT *
        /// </summary>
        [TestMethod]
        public void GetAllStores()
        {
            //RequiredDataContainer DataMap = MarketingCMSDataProvider.MapEntitiesToCollections();
            //RequiredDataContainer DataMap2 = MarketingCMSDataProvider.MapEntitiesToCollectionDuplicates();
            //RequiredDataContainer DataMap3 = MarketingCMSDataProvider.MapEntitiesToCollectionCategoryDuplicated();
            //RequiredDataContainer DataMap4 = MarketingCMSDataProvider.MapEntitiesToCollectionsStoreDuplicated();
            //RequiredDataContainer DataMap5 = MarketingCMSDataProvider.MapEntitiesToCollectionsUserDuplicated();
            var DataMap = QueryBuilderParser.ParseMapping( Utils.ReadMappingFile( "Mappings/MarketingCMS/entities-to-collections.mapping" ) );
            var DataMap2 = QueryBuilderParser.ParseMapping( Utils.ReadMappingFile( "Mappings/MarketingCMS/entities-to-collections-duplicates.mapping" ) );
            var DataMap3 = QueryBuilderParser.ParseMapping( Utils.ReadMappingFile( "Mappings/MarketingCMS/entities-to-collections-category-duplicated.mapping" ) );
            var DataMap4 = QueryBuilderParser.ParseMapping( Utils.ReadMappingFile( "Mappings/MarketingCMS/entities-to-collections-store-duplicated.mapping" ) );
            var DataMap5 = QueryBuilderParser.ParseMapping( Utils.ReadMappingFile( "Mappings/MarketingCMS/entities-to-collections-user-duplicated.mapping" ) );

            QueryableEntity Store = new QueryableEntity( DataMap.EntityRelationshipModel.FindByName( "Store" ) );

            SortArgument SortArg = new SortArgument( Store, Store.GetAttribute( "StoreID" ), MongoDBSort.Ascending );

            SortStage SortOpMap1 = new SortStage( new List<SortArgument>() { SortArg }, DataMap.ERMongoMapping );
            SortStage SortOpMap2 = new SortStage( new List<SortArgument>() { SortArg }, DataMap2.ERMongoMapping );
            SortStage SortOpMap3 = new SortStage( new List<SortArgument>() { SortArg }, DataMap3.ERMongoMapping );
            SortStage SortOpMap4 = new SortStage( new List<SortArgument>() { SortArg }, DataMap4.ERMongoMapping );
            SortStage SortOpMap5 = new SortStage( new List<SortArgument>() { SortArg }, DataMap5.ERMongoMapping );

            FromArgument StartArgMap1 = new FromArgument( Store, DataMap.ERMongoMapping );
            FromArgument StartArgMap2 = new FromArgument( Store, DataMap2.ERMongoMapping );
            FromArgument StartArgMap3 = new FromArgument( Store, DataMap3.ERMongoMapping );
            FromArgument StartArgMap4 = new FromArgument( Store, DataMap4.ERMongoMapping );
            FromArgument StartArgMap5 = new FromArgument( Store, DataMap5.ERMongoMapping );

            List<AlgebraOperator> OperatorsMap1 = new List<AlgebraOperator>() { SortOpMap1 };
            List<AlgebraOperator> OperatorsMap2 = new List<AlgebraOperator>() { SortOpMap2 };
            List<AlgebraOperator> OperatorsMap3 = new List<AlgebraOperator>() { SortOpMap3 };
            List<AlgebraOperator> OperatorsMap4 = new List<AlgebraOperator>() { SortOpMap4 };
            List<AlgebraOperator> OperatorsMap5 = new List<AlgebraOperator>() { SortOpMap5 };

            QueryGenerator QueryGenMap1 = new QueryGenerator( StartArgMap1, OperatorsMap1 );
            QueryGenerator QueryGenMap2 = new QueryGenerator( StartArgMap2, OperatorsMap2 );
            QueryGenerator QueryGenMap3 = new QueryGenerator( StartArgMap3, OperatorsMap3 );
            QueryGenerator QueryGenMap4 = new QueryGenerator( StartArgMap4, OperatorsMap4 );
            QueryGenerator QueryGenMap5 = new QueryGenerator( StartArgMap5, OperatorsMap5 );

            string QueryStringMap1 = QueryGenMap1.Run();
            Assert.ThrowsException<ImpossibleOperationException>( QueryGenMap2.Run );
            string QueryStringMap3 = QueryGenMap3.Run();
            Assert.ThrowsException<ImpossibleOperationException>( QueryGenMap4.Run );
            string QueryStringMap5 = QueryGenMap5.Run();

            QueryRunner RunnerMap1 = new QueryRunner( "mongodb://localhost:27017", "pesquisa_cms" );
            QueryRunner RunnerMap3 = new QueryRunner( "mongodb://localhost:27017", "pesquisa_cms_category_duplicado" );
            QueryRunner RunnerMap5 = new QueryRunner( "mongodb://localhost:27017", "pesquisa_cms_user_duplicado" );

            string ResultMap1 = RunnerMap1.GetJSON( QueryStringMap1 );
            string ResultMap3 = RunnerMap3.GetJSON( QueryStringMap3 );
            string ResultMap5 = RunnerMap5.GetJSON( QueryStringMap5 );

            Assert.IsNotNull( ResultMap1, "Result [Map1] cannot be null" );
            Assert.IsNotNull( ResultMap3, "Result [Map3] cannot be null" );
            Assert.IsNotNull( ResultMap5, "Result [Map5] cannot be null" );

            Assert.IsTrue( JToken.DeepEquals( JToken.Parse( ResultMap1 ), JToken.Parse( ResultMap3 ) ) );
            Assert.IsTrue( JToken.DeepEquals( JToken.Parse( ResultMap1 ), JToken.Parse( ResultMap5 ) ) );
        }
        /// <summary>
        /// Run GetAllUsers test
        /// 
        /// Query: FROM User SELECT *
        /// </summary>
        [TestMethod]
        public void GetAllUsers()
        {
            //RequiredDataContainer DataMap = MarketingCMSDataProvider.MapEntitiesToCollections();
            //RequiredDataContainer DataMapDuplicates = MarketingCMSDataProvider.MapEntitiesToCollectionDuplicates();
            //RequiredDataContainer DataMapCategoryDuplicated = MarketingCMSDataProvider.MapEntitiesToCollectionCategoryDuplicated();
            //RequiredDataContainer DataMapStoreDuplicated = MarketingCMSDataProvider.MapEntitiesToCollectionsStoreDuplicated();
            //RequiredDataContainer DataMapUserDuplicated = MarketingCMSDataProvider.MapEntitiesToCollectionsUserDuplicated();
            var DataMap = QueryBuilderParser.ParseMapping( Utils.ReadMappingFile( "Mappings/MarketingCMS/entities-to-collections.mapping" ) );
            var DataMapDuplicates = QueryBuilderParser.ParseMapping( Utils.ReadMappingFile( "Mappings/MarketingCMS/entities-to-collections-duplicates.mapping" ) );
            var DataMapCategoryDuplicated = QueryBuilderParser.ParseMapping( Utils.ReadMappingFile( "Mappings/MarketingCMS/entities-to-collections-category-duplicated.mapping" ) );
            var DataMapStoreDuplicated = QueryBuilderParser.ParseMapping( Utils.ReadMappingFile( "Mappings/MarketingCMS/entities-to-collections-store-duplicated.mapping" ) );
            var DataMapUserDuplicated = QueryBuilderParser.ParseMapping( Utils.ReadMappingFile( "Mappings/MarketingCMS/entities-to-collections-user-duplicated.mapping" ) );

            QueryableEntity User = new QueryableEntity( DataMap.EntityRelationshipModel.FindByName( "User" ) );

            SortArgument SortArg = new SortArgument( User, User.GetAttribute( "UserID" ), MongoDBSort.Ascending );

            SortStage SortOpMap1 = new SortStage( new List<SortArgument>() { SortArg }, DataMap.ERMongoMapping );
            SortStage SortOpMap2 = new SortStage( new List<SortArgument>() { SortArg }, DataMapDuplicates.ERMongoMapping );
            SortStage SortOpMap3 = new SortStage( new List<SortArgument>() { SortArg }, DataMapCategoryDuplicated.ERMongoMapping );
            SortStage SortOpMap4 = new SortStage( new List<SortArgument>() { SortArg }, DataMapStoreDuplicated.ERMongoMapping );
            SortStage SortOpMap5 = new SortStage( new List<SortArgument>() { SortArg }, DataMapUserDuplicated.ERMongoMapping );

            FromArgument StartArgMap1 = new FromArgument( User, DataMap.ERMongoMapping );
            FromArgument StartArgMap2 = new FromArgument( User, DataMapDuplicates.ERMongoMapping );
            FromArgument StartArgMap3 = new FromArgument( User, DataMapCategoryDuplicated.ERMongoMapping );
            FromArgument StartArgMap4 = new FromArgument( User, DataMapStoreDuplicated.ERMongoMapping );
            FromArgument StartArgMap5 = new FromArgument( User, DataMapUserDuplicated.ERMongoMapping );

            List<AlgebraOperator> OperatorsMap1 = new List<AlgebraOperator>() { SortOpMap1 };
            List<AlgebraOperator> OperatorsMap2 = new List<AlgebraOperator>() { SortOpMap2 };
            List<AlgebraOperator> OperatorsMap3 = new List<AlgebraOperator>() { SortOpMap3 };
            List<AlgebraOperator> OperatorsMap4 = new List<AlgebraOperator>() { SortOpMap4 };
            List<AlgebraOperator> OperatorsMap5 = new List<AlgebraOperator>() { SortOpMap5 };

            QueryGenerator QueryGenMap1 = new QueryGenerator( StartArgMap1, OperatorsMap1 );
            QueryGenerator QueryGenMap2 = new QueryGenerator( StartArgMap2, OperatorsMap2 );
            QueryGenerator QueryGenMap3 = new QueryGenerator( StartArgMap3, OperatorsMap3 );
            QueryGenerator QueryGenMap4 = new QueryGenerator( StartArgMap4, OperatorsMap4 );
            QueryGenerator QueryGenMap5 = new QueryGenerator( StartArgMap5, OperatorsMap5 );

            string QueryStringMap1 = QueryGenMap1.Run();
            Assert.ThrowsException<ImpossibleOperationException>( QueryGenMap2.Run );
            string QueryStringMap3 = QueryGenMap3.Run();
            string QueryStringMap4 = QueryGenMap4.Run();
            Assert.ThrowsException<ImpossibleOperationException>( QueryGenMap5.Run );

            QueryRunner RunnerMap1 = new QueryRunner( "mongodb://localhost:27017", "pesquisa_cms" );
            QueryRunner RunnerMap3 = new QueryRunner( "mongodb://localhost:27017", "pesquisa_cms_category_duplicado" );
            QueryRunner RunnerMap4 = new QueryRunner( "mongodb://localhost:27017", "pesquisa_cms_store_duplicado" );

            string ResultMap1 = RunnerMap1.GetJSON( QueryStringMap1 );
            string ResultMap3 = RunnerMap3.GetJSON( QueryStringMap3 );
            string ResultMap4 = RunnerMap4.GetJSON( QueryStringMap4 );

            Assert.IsNotNull( ResultMap1, "Result [Map1] cannot be null" );
            Assert.IsNotNull( ResultMap3, "Result [Map3] cannot be null" );
            Assert.IsNotNull( ResultMap4, "Result [Map4] cannot be null" );

            Assert.IsTrue( ResultMap1 != string.Empty, "Result [Map1] cannot be empty" );
            Assert.IsTrue( ResultMap3 != string.Empty, "Result [Map3] cannot be empty" );
            Assert.IsTrue( ResultMap4 != string.Empty, "Result [Map4] cannot be empty" );

            Assert.IsTrue( JToken.DeepEquals( JToken.Parse( ResultMap1 ), JToken.Parse( ResultMap3 ) ) );
            Assert.IsTrue( JToken.DeepEquals( JToken.Parse( ResultMap1 ), JToken.Parse( ResultMap4 ) ) );
        }
        /// <summary>
        /// Run GetAllCategories Test
        /// 
        /// QUERY: FROM Category SELECT *
        /// </summary>
        [TestMethod]
        public void GetAllCategories()
        {
            //RequiredDataContainer DataMap = MarketingCMSDataProvider.MapEntitiesToCollections();
            //RequiredDataContainer DataMapDuplicates = MarketingCMSDataProvider.MapEntitiesToCollectionDuplicates();
            //RequiredDataContainer DataMapCategoryDuplicated = MarketingCMSDataProvider.MapEntitiesToCollectionCategoryDuplicated();
            //RequiredDataContainer DataMapStoreDuplicated = MarketingCMSDataProvider.MapEntitiesToCollectionsStoreDuplicated();
            //RequiredDataContainer DataMapUserDuplicated = MarketingCMSDataProvider.MapEntitiesToCollectionsUserDuplicated();
            var DataMap = QueryBuilderParser.ParseMapping( Utils.ReadMappingFile( "Mappings/MarketingCMS/entities-to-collections.mapping" ) );
            var DataMapDuplicates = QueryBuilderParser.ParseMapping( Utils.ReadMappingFile( "Mappings/MarketingCMS/entities-to-collections-duplicates.mapping" ) );
            var DataMapCategoryDuplicated = QueryBuilderParser.ParseMapping( Utils.ReadMappingFile( "Mappings/MarketingCMS/entities-to-collections-category-duplicated.mapping" ) );
            var DataMapStoreDuplicated = QueryBuilderParser.ParseMapping( Utils.ReadMappingFile( "Mappings/MarketingCMS/entities-to-collections-store-duplicated.mapping" ) );
            var DataMapUserDuplicated = QueryBuilderParser.ParseMapping( Utils.ReadMappingFile( "Mappings/MarketingCMS/entities-to-collections-user-duplicated.mapping" ) );

            QueryableEntity Category = new QueryableEntity( DataMap.EntityRelationshipModel.FindByName( "Category" ) );

            SortArgument SortArg = new SortArgument( Category, Category.GetAttribute( "CategoryID" ), MongoDBSort.Ascending );

            SortStage SortOpMap1 = new SortStage( new List<SortArgument>() { SortArg }, DataMap.ERMongoMapping );
            SortStage SortOpMap2 = new SortStage( new List<SortArgument>() { SortArg }, DataMapDuplicates.ERMongoMapping );
            SortStage SortOpMap3 = new SortStage( new List<SortArgument>() { SortArg }, DataMapCategoryDuplicated.ERMongoMapping );
            SortStage SortOpMap4 = new SortStage( new List<SortArgument>() { SortArg }, DataMapStoreDuplicated.ERMongoMapping );
            SortStage SortOpMap5 = new SortStage( new List<SortArgument>() { SortArg }, DataMapUserDuplicated.ERMongoMapping );

            FromArgument StartArgMap1 = new FromArgument( Category, DataMap.ERMongoMapping );
            FromArgument StartArgMap2 = new FromArgument( Category, DataMapDuplicates.ERMongoMapping );
            FromArgument StartArgMap3 = new FromArgument( Category, DataMapCategoryDuplicated.ERMongoMapping );
            FromArgument StartArgMap4 = new FromArgument( Category, DataMapStoreDuplicated.ERMongoMapping );
            FromArgument StartArgMap5 = new FromArgument( Category, DataMapUserDuplicated.ERMongoMapping );

            List<AlgebraOperator> OperatorsMap1 = new List<AlgebraOperator>() { SortOpMap1 };
            List<AlgebraOperator> OperatorsMap2 = new List<AlgebraOperator>() { SortOpMap2 };
            List<AlgebraOperator> OperatorsMap3 = new List<AlgebraOperator>() { SortOpMap3 };
            List<AlgebraOperator> OperatorsMap4 = new List<AlgebraOperator>() { SortOpMap4 };
            List<AlgebraOperator> OperatorsMap5 = new List<AlgebraOperator>() { SortOpMap5 };

            QueryGenerator QueryGenMap1 = new QueryGenerator( StartArgMap1, OperatorsMap1 );
            QueryGenerator QueryGenMap2 = new QueryGenerator( StartArgMap2, OperatorsMap2 );
            QueryGenerator QueryGenMap3 = new QueryGenerator( StartArgMap3, OperatorsMap3 );
            QueryGenerator QueryGenMap4 = new QueryGenerator( StartArgMap4, OperatorsMap4 );
            QueryGenerator QueryGenMap5 = new QueryGenerator( StartArgMap5, OperatorsMap5 );

            string QueryStringMap1 = QueryGenMap1.Run();
            Assert.ThrowsException<ImpossibleOperationException>( QueryGenMap2.Run );
            Assert.ThrowsException<ImpossibleOperationException>( QueryGenMap3.Run );
            string QueryStringMap4 = QueryGenMap4.Run();
            string QueryStringMap5 = QueryGenMap5.Run();

            QueryRunner RunnerMap1 = new QueryRunner( "mongodb://localhost:27017", "pesquisa_cms" );
            QueryRunner RunnerMap4 = new QueryRunner( "mongodb://localhost:27017", "pesquisa_cms_store_duplicado" );
            QueryRunner RunnerMap5 = new QueryRunner( "mongodb://localhost:27017", "pesquisa_cms_user_duplicado" );

            string ResultMap1 = RunnerMap1.GetJSON( QueryStringMap1 );;
            string ResultMap4 = RunnerMap4.GetJSON( QueryStringMap4 );
            string ResultMap5 = RunnerMap5.GetJSON( QueryStringMap5 );

            Assert.IsNotNull( ResultMap1, "Result [Map1] cannot be null" );
            Assert.IsNotNull( ResultMap4, "Result [Map4] cannot be null" );
            Assert.IsNotNull( ResultMap5, "Result [Map5] cannot be null" );

            Assert.IsTrue( ResultMap1 != string.Empty, "Result [Map1] cannot be empty" );
            Assert.IsTrue( ResultMap4 != string.Empty, "Result [Map4] cannot be empty" );
            Assert.IsTrue( ResultMap5 != string.Empty, "Result [Map5] cannot be empty" );

            Assert.IsTrue( JToken.DeepEquals( JToken.Parse( ResultMap1 ), JToken.Parse( ResultMap4 ) ) );
            Assert.IsTrue( JToken.DeepEquals( JToken.Parse( ResultMap1 ), JToken.Parse( ResultMap5 ) ) );
        }

        /// <summary>
        /// Run GetProductsFromStore query
        /// 
        /// QUERY: FROM Store s 
        ///        RJOIN <StoreProducts> (Product p)
        ///        SELECT *
        /// </summary>
        [TestMethod]
        public void GetAllProductsFromStore()
        {
            //RequiredDataContainer DataMap = MarketingCMSDataProvider.MapEntitiesToCollections();
            //RequiredDataContainer DataMapDuplicates = MarketingCMSDataProvider.MapEntitiesToCollectionDuplicates();
            //RequiredDataContainer DataMapCategoryDuplicated = MarketingCMSDataProvider.MapEntitiesToCollectionCategoryDuplicated();
            //RequiredDataContainer DataMapStoreDuplicated = MarketingCMSDataProvider.MapEntitiesToCollectionsStoreDuplicated();
            //RequiredDataContainer DataMapUserDuplicated = MarketingCMSDataProvider.MapEntitiesToCollectionsUserDuplicated();
            var DataMap = QueryBuilderParser.ParseMapping( Utils.ReadMappingFile( "Mappings/MarketingCMS/entities-to-collections.mapping" ) );
            var DataMapDuplicates = QueryBuilderParser.ParseMapping( Utils.ReadMappingFile( "Mappings/MarketingCMS/entities-to-collections-duplicates.mapping" ) );
            var DataMapCategoryDuplicated = QueryBuilderParser.ParseMapping( Utils.ReadMappingFile( "Mappings/MarketingCMS/entities-to-collections-category-duplicated.mapping" ) );
            var DataMapStoreDuplicated = QueryBuilderParser.ParseMapping( Utils.ReadMappingFile( "Mappings/MarketingCMS/entities-to-collections-store-duplicated.mapping" ) );
            var DataMapUserDuplicated = QueryBuilderParser.ParseMapping( Utils.ReadMappingFile( "Mappings/MarketingCMS/entities-to-collections-user-duplicated.mapping" ) );

            QueryableEntity Store = new QueryableEntity( DataMap.EntityRelationshipModel.FindByName( "Store" ) );
            QueryableEntity Product = new QueryableEntity( DataMap.EntityRelationshipModel.FindByName( "Product" ) );

            QueryableEntity Store2 = new QueryableEntity( DataMapDuplicates.EntityRelationshipModel.FindByName( "Store" ) );
            QueryableEntity Product2 = new QueryableEntity( DataMapDuplicates.EntityRelationshipModel.FindByName( "Product" ) );

            QueryableEntity Store3 = new QueryableEntity( DataMapCategoryDuplicated.EntityRelationshipModel.FindByName( "Store" ) );
            QueryableEntity Product3 = new QueryableEntity( DataMapCategoryDuplicated.EntityRelationshipModel.FindByName( "Product" ) );

            QueryableEntity Store4 = new QueryableEntity( DataMapStoreDuplicated.EntityRelationshipModel.FindByName( "Store" ) );
            QueryableEntity Product4 = new QueryableEntity( DataMapStoreDuplicated.EntityRelationshipModel.FindByName( "Product" ) );

            QueryableEntity Store5 = new QueryableEntity( DataMapUserDuplicated.EntityRelationshipModel.FindByName( "Store" ) );
            QueryableEntity Product5 = new QueryableEntity( DataMapUserDuplicated.EntityRelationshipModel.FindByName( "Product" ) );

            RelationshipJoinOperator RJoinOp1 = new RelationshipJoinOperator( Store, (Relationship)DataMap.EntityRelationshipModel.FindByName( "StoreProducts" ),
                new List<QueryableEntity>() { Product }, DataMap.ERMongoMapping );

            RelationshipJoinOperator RJoinOp2 = new RelationshipJoinOperator( Store2, (Relationship)DataMapDuplicates.EntityRelationshipModel.FindByName( "StoreProducts" ),
                new List<QueryableEntity>() { Product2 }, DataMapDuplicates.ERMongoMapping );

            RelationshipJoinOperator RJoinOp3 = new RelationshipJoinOperator( Store3, (Relationship)DataMapCategoryDuplicated.EntityRelationshipModel.FindByName( "StoreProducts" ),
                new List<QueryableEntity>() { Product3 }, DataMapCategoryDuplicated.ERMongoMapping );

            RelationshipJoinOperator RJoinOp4 = new RelationshipJoinOperator( Store4, (Relationship)DataMapStoreDuplicated.EntityRelationshipModel.FindByName( "StoreProducts" ),
                new List<QueryableEntity>() { Product4 }, DataMapStoreDuplicated.ERMongoMapping );

            RelationshipJoinOperator RJoinOp5 = new RelationshipJoinOperator( Store5, (Relationship)DataMapUserDuplicated.EntityRelationshipModel.FindByName( "StoreProducts" ),
                new List<QueryableEntity>() { Product5 }, DataMapUserDuplicated.ERMongoMapping );

            SortArgument SortArg = new SortArgument( Store, Store.GetAttribute( "StoreID" ), MongoDBSort.Ascending );

            SortStage SortOpMap1 = new SortStage( new List<SortArgument>() { SortArg }, DataMap.ERMongoMapping );
            SortStage SortOpMap2 = new SortStage( new List<SortArgument>() { SortArg }, DataMapDuplicates.ERMongoMapping );
            SortStage SortOpMap3 = new SortStage( new List<SortArgument>() { SortArg }, DataMapCategoryDuplicated.ERMongoMapping );
            SortStage SortOpMap4 = new SortStage( new List<SortArgument>() { SortArg }, DataMapStoreDuplicated.ERMongoMapping );
            SortStage SortOpMap5 = new SortStage( new List<SortArgument>() { SortArg }, DataMapUserDuplicated.ERMongoMapping );

            List<AlgebraOperator> OperatorsToExecuteMap1 = new List<AlgebraOperator>() { RJoinOp1, SortOpMap1 };
            List<AlgebraOperator> OperatorsToExecuteMap2 = new List<AlgebraOperator>() { RJoinOp2, SortOpMap2 };
            List<AlgebraOperator> OperatorsToExecuteMap3 = new List<AlgebraOperator>() { RJoinOp3, SortOpMap3 };
            List<AlgebraOperator> OperatorsToExecuteMap4 = new List<AlgebraOperator>() { RJoinOp4, SortOpMap4 };
            List<AlgebraOperator> OperatorsToExecuteMap5 = new List<AlgebraOperator>() { RJoinOp5, SortOpMap5 };

            FromArgument StartArgMap1 = new FromArgument( Store, DataMap.ERMongoMapping );
            FromArgument StartArgMap2 = new FromArgument( Store2, DataMapDuplicates.ERMongoMapping );
            FromArgument StartArgMap3 = new FromArgument( Store3, DataMapCategoryDuplicated.ERMongoMapping );
            FromArgument StartArgMap4 = new FromArgument( Store4, DataMapStoreDuplicated.ERMongoMapping );
            FromArgument StartArgMap5 = new FromArgument( Store5, DataMapUserDuplicated.ERMongoMapping );

            QueryGenerator GeneratorMap1 = new QueryGenerator( StartArgMap1, OperatorsToExecuteMap1 );
            QueryGenerator GeneratorMap2 = new QueryGenerator( StartArgMap2, OperatorsToExecuteMap2 );
            QueryGenerator GeneratorMap3 = new QueryGenerator( StartArgMap3, OperatorsToExecuteMap3 );
            QueryGenerator GeneratorMap4 = new QueryGenerator( StartArgMap4, OperatorsToExecuteMap4 );
            QueryGenerator GeneratorMap5 = new QueryGenerator( StartArgMap5, OperatorsToExecuteMap5 );

            string QueryMap1 = GeneratorMap1.Run();
            Assert.ThrowsException<ImpossibleOperationException>( GeneratorMap2.Run );
            string QueryMap3 = GeneratorMap3.Run();
            Assert.ThrowsException<ImpossibleOperationException>( GeneratorMap4.Run );
            string QueryMap5 = GeneratorMap5.Run();

            Assert.IsNotNull( QueryMap1, "Query [Map1] cannot be null" );
            Assert.IsNotNull( QueryMap3, "Query [Map3] cannot be null" );
            Assert.IsNotNull( QueryMap5, "Query [Map5] cannot be null" );

            QueryRunner RunnerMap1 = new QueryRunner( "mongodb://localhost:27017", "pesquisa_cms" );
            QueryRunner RunnerMap3 = new QueryRunner( "mongodb://localhost:27017", "pesquisa_cms_category_duplicado" );
            QueryRunner RunnerMap5 = new QueryRunner( "mongodb://localhost:27017", "pesquisa_cms_user_duplicado" );

            string ResultMap1 = RunnerMap1.GetJSON( QueryMap1 );
            string ResultMap3 = RunnerMap3.GetJSON( QueryMap3 );
            string ResultMap5 = RunnerMap5.GetJSON( QueryMap5 );

            Assert.IsNotNull( ResultMap1, "Result [Map1] cannot be null" );
            Assert.IsNotNull( ResultMap3, "Result [Map3] cannot be null" );
            Assert.IsNotNull( ResultMap5, "Result [Map5] cannot be null" );

            Assert.IsTrue( ResultMap1 != string.Empty, "Result [Map1] cannot be empty" );
            Assert.IsTrue( ResultMap3 != string.Empty, "Result [Map3] cannot be empty" );
            Assert.IsTrue( ResultMap5 != string.Empty, "Result [Map5] cannot be empty" );

            JToken TokenResult1 = JToken.Parse( ResultMap1 );

            TokenResult1.Should().BeEquivalentTo( JToken.Parse( ResultMap3 ) );
            TokenResult1.Should().BeEquivalentTo( JToken.Parse( ResultMap5 ) );
        }

        /// <summary>
        /// Run GetProductsFromCategory query
        /// 
        /// QUERY: FROM Category c 
        ///        RJOIN <CategoryProducts> (Product p)
        ///        SELECT *
        /// </summary>
        [TestMethod]
        public void GetAllProductsFromCategory()
        {
            //RequiredDataContainer DataMap = MarketingCMSDataProvider.MapEntitiesToCollections();
            //RequiredDataContainer DataMapDuplicates = MarketingCMSDataProvider.MapEntitiesToCollectionDuplicates();
            //RequiredDataContainer DataMapCategoryDuplicated = MarketingCMSDataProvider.MapEntitiesToCollectionCategoryDuplicated();
            //RequiredDataContainer DataMapStoreDuplicated = MarketingCMSDataProvider.MapEntitiesToCollectionsStoreDuplicated();
            //RequiredDataContainer DataMapUserDuplicated = MarketingCMSDataProvider.MapEntitiesToCollectionsUserDuplicated();
            var DataMap = QueryBuilderParser.ParseMapping( Utils.ReadMappingFile( "Mappings/MarketingCMS/entities-to-collections.mapping" ) );
            var DataMapDuplicates = QueryBuilderParser.ParseMapping( Utils.ReadMappingFile( "Mappings/MarketingCMS/entities-to-collections-duplicates.mapping" ) );
            var DataMapCategoryDuplicated = QueryBuilderParser.ParseMapping( Utils.ReadMappingFile( "Mappings/MarketingCMS/entities-to-collections-category-duplicated.mapping" ) );
            var DataMapStoreDuplicated = QueryBuilderParser.ParseMapping( Utils.ReadMappingFile( "Mappings/MarketingCMS/entities-to-collections-store-duplicated.mapping" ) );
            var DataMapUserDuplicated = QueryBuilderParser.ParseMapping( Utils.ReadMappingFile( "Mappings/MarketingCMS/entities-to-collections-user-duplicated.mapping" ) );

            QueryableEntity Category = new QueryableEntity( DataMap.EntityRelationshipModel.FindByName( "Category" ) );
            QueryableEntity Product = new QueryableEntity( DataMap.EntityRelationshipModel.FindByName( "Product" ) );

            QueryableEntity Category2 = new QueryableEntity( DataMapDuplicates.EntityRelationshipModel.FindByName( "Category" ) );
            QueryableEntity Product2 = new QueryableEntity( DataMapDuplicates.EntityRelationshipModel.FindByName( "Product" ) );

            QueryableEntity Category3 = new QueryableEntity( DataMapCategoryDuplicated.EntityRelationshipModel.FindByName( "Category" ) );
            QueryableEntity Product3 = new QueryableEntity( DataMapCategoryDuplicated.EntityRelationshipModel.FindByName( "Product" ) );

            QueryableEntity Category4 = new QueryableEntity( DataMapStoreDuplicated.EntityRelationshipModel.FindByName( "Category" ) );
            QueryableEntity Product4 = new QueryableEntity( DataMapStoreDuplicated.EntityRelationshipModel.FindByName( "Product" ) );

            QueryableEntity Category5 = new QueryableEntity( DataMapUserDuplicated.EntityRelationshipModel.FindByName( "Category" ) );
            QueryableEntity Product5 = new QueryableEntity( DataMapUserDuplicated.EntityRelationshipModel.FindByName( "Product" ) );

            RelationshipJoinOperator RJoinOp1 = new RelationshipJoinOperator( Category, (Relationship)DataMap.EntityRelationshipModel.FindByName( "CategoryProducts" ),
                new List<QueryableEntity>() { Product }, DataMap.ERMongoMapping );

            RelationshipJoinOperator RJoinOp2 = new RelationshipJoinOperator( Category2, (Relationship)DataMapDuplicates.EntityRelationshipModel.FindByName( "CategoryProducts" ),
                new List<QueryableEntity>() { Product2 }, DataMapDuplicates.ERMongoMapping );

            RelationshipJoinOperator RJoinOp3 = new RelationshipJoinOperator( Category3, (Relationship)DataMapCategoryDuplicated.EntityRelationshipModel.FindByName( "CategoryProducts" ),
                new List<QueryableEntity>() { Product3 }, DataMapCategoryDuplicated.ERMongoMapping );

            RelationshipJoinOperator RJoinOp4 = new RelationshipJoinOperator( Category4, (Relationship)DataMapStoreDuplicated.EntityRelationshipModel.FindByName( "CategoryProducts" ),
                new List<QueryableEntity>() { Product4 }, DataMapStoreDuplicated.ERMongoMapping );

            RelationshipJoinOperator RJoinOp5 = new RelationshipJoinOperator( Category5, (Relationship)DataMapUserDuplicated.EntityRelationshipModel.FindByName( "CategoryProducts" ),
                new List<QueryableEntity>() { Product5 }, DataMapUserDuplicated.ERMongoMapping );

            SortArgument SortArg = new SortArgument( Category, Category.GetAttribute( "CategoryID" ), MongoDBSort.Ascending );

            SortStage SortOpMap1 = new SortStage( new List<SortArgument>() { SortArg }, DataMap.ERMongoMapping );
            SortStage SortOpMap2 = new SortStage( new List<SortArgument>() { SortArg }, DataMapDuplicates.ERMongoMapping );
            SortStage SortOpMap3 = new SortStage( new List<SortArgument>() { SortArg }, DataMapCategoryDuplicated.ERMongoMapping );
            SortStage SortOpMap4 = new SortStage( new List<SortArgument>() { SortArg }, DataMapStoreDuplicated.ERMongoMapping );
            SortStage SortOpMap5 = new SortStage( new List<SortArgument>() { SortArg }, DataMapUserDuplicated.ERMongoMapping );

            List<AlgebraOperator> OperatorsToExecuteMap1 = new List<AlgebraOperator>() { RJoinOp1, SortOpMap1 };
            List<AlgebraOperator> OperatorsToExecuteMap2 = new List<AlgebraOperator>() { RJoinOp2, SortOpMap2 };
            List<AlgebraOperator> OperatorsToExecuteMap3 = new List<AlgebraOperator>() { RJoinOp3, SortOpMap3 };
            List<AlgebraOperator> OperatorsToExecuteMap4 = new List<AlgebraOperator>() { RJoinOp4, SortOpMap4 };
            List<AlgebraOperator> OperatorsToExecuteMap5 = new List<AlgebraOperator>() { RJoinOp5, SortOpMap5 };

            FromArgument StartArgMap1 = new FromArgument( Category, DataMap.ERMongoMapping );
            FromArgument StartArgMap2 = new FromArgument( Category, DataMapDuplicates.ERMongoMapping );
            FromArgument StartArgMap3 = new FromArgument( Category, DataMapCategoryDuplicated.ERMongoMapping );
            FromArgument StartArgMap4 = new FromArgument( Category, DataMapStoreDuplicated.ERMongoMapping );
            FromArgument StartArgMap5 = new FromArgument( Category, DataMapUserDuplicated.ERMongoMapping );

            QueryGenerator GeneratorMap1 = new QueryGenerator( StartArgMap1, OperatorsToExecuteMap1 );
            QueryGenerator GeneratorMap2 = new QueryGenerator( StartArgMap2, OperatorsToExecuteMap2 );
            QueryGenerator GeneratorMap3 = new QueryGenerator( StartArgMap3, OperatorsToExecuteMap3 );
            QueryGenerator GeneratorMap4 = new QueryGenerator( StartArgMap4, OperatorsToExecuteMap4 );
            QueryGenerator GeneratorMap5 = new QueryGenerator( StartArgMap5, OperatorsToExecuteMap5 );

            string QueryMap1 = GeneratorMap1.Run();
            Assert.ThrowsException<ImpossibleOperationException>( GeneratorMap2.Run );
            Assert.ThrowsException<ImpossibleOperationException>( GeneratorMap3.Run );
            string QueryMap4 = GeneratorMap4.Run();
            string QueryMap5 = GeneratorMap5.Run();

            Assert.IsNotNull( QueryMap1, "Query [Map1] cannot be null" );
            Assert.IsNotNull( QueryMap4, "Query [Map4] cannot be null" );
            Assert.IsNotNull( QueryMap5, "Query [Map5] cannot be null" );

            QueryRunner RunnerMap1 = new QueryRunner( "mongodb://localhost:27017", "pesquisa_cms" );
            QueryRunner RunnerMap4 = new QueryRunner( "mongodb://localhost:27017", "pesquisa_cms_store_duplicado" );
            QueryRunner RunnerMap5 = new QueryRunner( "mongodb://localhost:27017", "pesquisa_cms_user_duplicado" );

            string ResultMap1 = RunnerMap1.GetJSON( QueryMap1 );
            string ResultMap4 = RunnerMap4.GetJSON( QueryMap4 );
            string ResultMap5 = RunnerMap5.GetJSON( QueryMap5 );

            Assert.IsNotNull( ResultMap1, "Result [Map1] cannot be null" );
            Assert.IsNotNull( ResultMap4, "Result [Map4] cannot be null" );
            Assert.IsNotNull( ResultMap5, "Result [Map5] cannot be null" );

            Assert.IsTrue( ResultMap1 != string.Empty, "Result [Map1] cannot be empty" );
            Assert.IsTrue( ResultMap4 != string.Empty, "Result [Map4] cannot be empty" );
            Assert.IsTrue( ResultMap5 != string.Empty, "Result [Map5] cannot be empty" );

            JToken TokenResult1 = JToken.Parse( ResultMap1 );

            TokenResult1.Should().BeEquivalentTo( JToken.Parse( ResultMap4 ) );
            TokenResult1.Should().BeEquivalentTo( JToken.Parse( ResultMap5 ) );
        }

        /// <summary>
        /// Run GetProductsFromUser query
        /// 
        /// QUERY: FROM User u 
        ///        RJOIN <UserProducts> (Product p)
        ///        SELECT *
        /// </summary>
        [TestMethod]
        public void GetAllProductsFromUser()
        {
            //RequiredDataContainer DataMap = MarketingCMSDataProvider.MapEntitiesToCollections();
            //RequiredDataContainer DataMapDuplicates = MarketingCMSDataProvider.MapEntitiesToCollectionDuplicates();
            //RequiredDataContainer DataMapCategoryDuplicated = MarketingCMSDataProvider.MapEntitiesToCollectionCategoryDuplicated();
            //RequiredDataContainer DataMapStoreDuplicated = MarketingCMSDataProvider.MapEntitiesToCollectionsStoreDuplicated();
            //RequiredDataContainer DataMapUserDuplicated = MarketingCMSDataProvider.MapEntitiesToCollectionsUserDuplicated();
            var DataMap = QueryBuilderParser.ParseMapping( Utils.ReadMappingFile( "Mappings/MarketingCMS/entities-to-collections.mapping" ) );
            var DataMapDuplicates = QueryBuilderParser.ParseMapping( Utils.ReadMappingFile( "Mappings/MarketingCMS/entities-to-collections-duplicates.mapping" ) );
            var DataMapCategoryDuplicated = QueryBuilderParser.ParseMapping( Utils.ReadMappingFile( "Mappings/MarketingCMS/entities-to-collections-category-duplicated.mapping" ) );
            var DataMapStoreDuplicated = QueryBuilderParser.ParseMapping( Utils.ReadMappingFile( "Mappings/MarketingCMS/entities-to-collections-store-duplicated.mapping" ) );
            var DataMapUserDuplicated = QueryBuilderParser.ParseMapping( Utils.ReadMappingFile( "Mappings/MarketingCMS/entities-to-collections-user-duplicated.mapping" ) );

            QueryableEntity User = new QueryableEntity( DataMap.EntityRelationshipModel.FindByName( "User" ) );
            QueryableEntity Product = new QueryableEntity( DataMap.EntityRelationshipModel.FindByName( "Product" ) );

            QueryableEntity User2 = new QueryableEntity( DataMapDuplicates.EntityRelationshipModel.FindByName( "User" ) );
            QueryableEntity Product2 = new QueryableEntity( DataMapDuplicates.EntityRelationshipModel.FindByName( "Product" ) );

            QueryableEntity User3 = new QueryableEntity( DataMapCategoryDuplicated.EntityRelationshipModel.FindByName( "User" ) );
            QueryableEntity Product3 = new QueryableEntity( DataMapCategoryDuplicated.EntityRelationshipModel.FindByName( "Product" ) );

            QueryableEntity User4 = new QueryableEntity( DataMapStoreDuplicated.EntityRelationshipModel.FindByName( "User" ) );
            QueryableEntity Product4 = new QueryableEntity( DataMapStoreDuplicated.EntityRelationshipModel.FindByName( "Product" ) );

            QueryableEntity User5 = new QueryableEntity( DataMapUserDuplicated.EntityRelationshipModel.FindByName( "User" ) );
            QueryableEntity Product5 = new QueryableEntity( DataMapUserDuplicated.EntityRelationshipModel.FindByName( "Product" ) );

            RelationshipJoinOperator RJoinOp1 = new RelationshipJoinOperator( User, (Relationship)DataMap.EntityRelationshipModel.FindByName( "UserProducts" ),
                new List<QueryableEntity>() { Product }, DataMap.ERMongoMapping );

            RelationshipJoinOperator RJoinOp2 = new RelationshipJoinOperator( User2, (Relationship)DataMapDuplicates.EntityRelationshipModel.FindByName( "UserProducts" ),
                new List<QueryableEntity>() { Product2 }, DataMapDuplicates.ERMongoMapping );

            RelationshipJoinOperator RJoinOp3 = new RelationshipJoinOperator( User3, (Relationship)DataMapCategoryDuplicated.EntityRelationshipModel.FindByName( "UserProducts" ),
                new List<QueryableEntity>() { Product3 }, DataMapCategoryDuplicated.ERMongoMapping );

            RelationshipJoinOperator RJoinOp4 = new RelationshipJoinOperator( User4, (Relationship)DataMapStoreDuplicated.EntityRelationshipModel.FindByName( "UserProducts" ),
                new List<QueryableEntity>() { Product4 }, DataMapStoreDuplicated.ERMongoMapping );

            RelationshipJoinOperator RJoinOp5 = new RelationshipJoinOperator( User5, (Relationship)DataMapUserDuplicated.EntityRelationshipModel.FindByName( "UserProducts" ),
                new List<QueryableEntity>() { Product5 }, DataMapUserDuplicated.ERMongoMapping );

            SortArgument SortArg = new SortArgument( User, User.GetAttribute( "UserID" ), MongoDBSort.Ascending );

            SortStage SortOpMap1 = new SortStage( new List<SortArgument>() { SortArg }, DataMap.ERMongoMapping );
            SortStage SortOpMap2 = new SortStage( new List<SortArgument>() { SortArg }, DataMapDuplicates.ERMongoMapping );
            SortStage SortOpMap3 = new SortStage( new List<SortArgument>() { SortArg }, DataMapCategoryDuplicated.ERMongoMapping );
            SortStage SortOpMap4 = new SortStage( new List<SortArgument>() { SortArg }, DataMapStoreDuplicated.ERMongoMapping );
            SortStage SortOpMap5 = new SortStage( new List<SortArgument>() { SortArg }, DataMapUserDuplicated.ERMongoMapping );

            List<AlgebraOperator> OperatorsToExecuteMap1 = new List<AlgebraOperator>() { RJoinOp1, SortOpMap1 };
            List<AlgebraOperator> OperatorsToExecuteMap2 = new List<AlgebraOperator>() { RJoinOp2, SortOpMap2 };
            List<AlgebraOperator> OperatorsToExecuteMap3 = new List<AlgebraOperator>() { RJoinOp3, SortOpMap3 };
            List<AlgebraOperator> OperatorsToExecuteMap4 = new List<AlgebraOperator>() { RJoinOp4, SortOpMap4 };
            List<AlgebraOperator> OperatorsToExecuteMap5 = new List<AlgebraOperator>() { RJoinOp5, SortOpMap5 };

            FromArgument StartArgMap1 = new FromArgument( User, DataMap.ERMongoMapping );
            FromArgument StartArgMap2 = new FromArgument( User, DataMapDuplicates.ERMongoMapping );
            FromArgument StartArgMap3 = new FromArgument( User, DataMapCategoryDuplicated.ERMongoMapping );
            FromArgument StartArgMap4 = new FromArgument( User, DataMapStoreDuplicated.ERMongoMapping );
            FromArgument StartArgMap5 = new FromArgument( User, DataMapUserDuplicated.ERMongoMapping );

            QueryGenerator GeneratorMap1 = new QueryGenerator( StartArgMap1, OperatorsToExecuteMap1 );
            QueryGenerator GeneratorMap2 = new QueryGenerator( StartArgMap2, OperatorsToExecuteMap2 );
            QueryGenerator GeneratorMap3 = new QueryGenerator( StartArgMap3, OperatorsToExecuteMap3 );
            QueryGenerator GeneratorMap4 = new QueryGenerator( StartArgMap4, OperatorsToExecuteMap4 );
            QueryGenerator GeneratorMap5 = new QueryGenerator( StartArgMap5, OperatorsToExecuteMap5 );

            string QueryMap1 = GeneratorMap1.Run();
            Assert.ThrowsException<ImpossibleOperationException>( GeneratorMap2.Run );
            string QueryMap3 = GeneratorMap3.Run();
            string QueryMap4 = GeneratorMap4.Run();
            Assert.ThrowsException<ImpossibleOperationException>( GeneratorMap5.Run );

            Assert.IsNotNull( QueryMap1, "Query [Map1] cannot be null" );
            Assert.IsNotNull( QueryMap3, "Query [Map3] cannot be null" );
            Assert.IsNotNull( QueryMap4, "Query [Map4] cannot be null" );

            QueryRunner RunnerMap1 = new QueryRunner( "mongodb://localhost:27017", "pesquisa_cms" );
            QueryRunner RunnerMap3 = new QueryRunner( "mongodb://localhost:27017", "pesquisa_cms_category_duplicado" );
            QueryRunner RunnerMap4 = new QueryRunner( "mongodb://localhost:27017", "pesquisa_cms_store_duplicado" );

            string ResultMap1 = RunnerMap1.GetJSON( QueryMap1 );
            string ResultMap3 = RunnerMap3.GetJSON( QueryMap3 );
            string ResultMap4 = RunnerMap4.GetJSON( QueryMap4 );

            Assert.IsNotNull( ResultMap1, "Result [Map1] cannot be null" );
            Assert.IsNotNull( ResultMap3, "Result [Map3] cannot be null" );
            Assert.IsNotNull( ResultMap4, "Result [Map4] cannot be null" );

            Assert.IsTrue( ResultMap1 != string.Empty, "Result [Map1] cannot be empty" );
            Assert.IsTrue( ResultMap3 != string.Empty, "Result [Map3] cannot be empty" );
            Assert.IsTrue( ResultMap4 != string.Empty, "Result [Map4] cannot be empty" );

            JToken TokenResult1 = JToken.Parse( ResultMap1 );

            TokenResult1.Should().BeEquivalentTo( JToken.Parse( ResultMap3 ) );
            TokenResult1.Should().BeEquivalentTo( JToken.Parse( ResultMap4 ) );
        }
        /// <summary>
        /// Run GetAllProductsFromCategoryWithStore
        /// 
        /// QUERY: FROM Category c
        ///        RJOIN <CategoryProducts> (Product p 
        ///                                  RJOIN <StoreProducts> (Store s))
        ///        SELECT *
        /// </summary>
        [TestMethod]
        public void GetAllProductsFromCategoryWithStore()
        {
            //RequiredDataContainer DataMap = MarketingCMSDataProvider.MapEntitiesToCollections();
            //RequiredDataContainer DataMapDuplicates = MarketingCMSDataProvider.MapEntitiesToCollectionDuplicates();
            //RequiredDataContainer DataMapCategoryDuplicated = MarketingCMSDataProvider.MapEntitiesToCollectionCategoryDuplicated();
            //RequiredDataContainer DataMapStoreDuplicated = MarketingCMSDataProvider.MapEntitiesToCollectionsStoreDuplicated();
            //RequiredDataContainer DataMapUserDuplicated = MarketingCMSDataProvider.MapEntitiesToCollectionsUserDuplicated();
            var DataMap = QueryBuilderParser.ParseMapping( Utils.ReadMappingFile( "Mappings/MarketingCMS/entities-to-collections.mapping" ) );
            var DataMapDuplicates = QueryBuilderParser.ParseMapping( Utils.ReadMappingFile( "Mappings/MarketingCMS/entities-to-collections-duplicates.mapping" ) );
            var DataMapCategoryDuplicated = QueryBuilderParser.ParseMapping( Utils.ReadMappingFile( "Mappings/MarketingCMS/entities-to-collections-category-duplicated.mapping" ) );
            var DataMapStoreDuplicated = QueryBuilderParser.ParseMapping( Utils.ReadMappingFile( "Mappings/MarketingCMS/entities-to-collections-store-duplicated.mapping" ) );
            var DataMapUserDuplicated = QueryBuilderParser.ParseMapping( Utils.ReadMappingFile( "Mappings/MarketingCMS/entities-to-collections-user-duplicated.mapping" ) );

            QueryableEntity Category = new QueryableEntity( DataMap.EntityRelationshipModel.FindByName( "Category" ) );
            QueryableEntity Product = new QueryableEntity( DataMap.EntityRelationshipModel.FindByName( "Product" ) );
            QueryableEntity Store = new QueryableEntity( DataMap.EntityRelationshipModel.FindByName( "Store" ) );

            QueryableEntity Category2 = new QueryableEntity( DataMapDuplicates.EntityRelationshipModel.FindByName( "Category" ) );
            QueryableEntity Product2 = new QueryableEntity( DataMapDuplicates.EntityRelationshipModel.FindByName( "Product" ) );
            QueryableEntity Store2 = new QueryableEntity( DataMapDuplicates.EntityRelationshipModel.FindByName( "Store" ) );

            QueryableEntity Category3 = new QueryableEntity( DataMapCategoryDuplicated.EntityRelationshipModel.FindByName( "Category" ) );
            QueryableEntity Product3 = new QueryableEntity( DataMapCategoryDuplicated.EntityRelationshipModel.FindByName( "Product" ) );
            QueryableEntity Store3 = new QueryableEntity( DataMapCategoryDuplicated.EntityRelationshipModel.FindByName( "Store" ) );

            QueryableEntity Category4 = new QueryableEntity( DataMapStoreDuplicated.EntityRelationshipModel.FindByName( "Category" ) );
            QueryableEntity Product4 = new QueryableEntity( DataMapStoreDuplicated.EntityRelationshipModel.FindByName( "Product" ) );
            QueryableEntity Store4 = new QueryableEntity( DataMapStoreDuplicated.EntityRelationshipModel.FindByName( "Store" ) );

            QueryableEntity Category5 = new QueryableEntity( DataMapUserDuplicated.EntityRelationshipModel.FindByName( "Category" ) );
            QueryableEntity Product5 = new QueryableEntity( DataMapUserDuplicated.EntityRelationshipModel.FindByName( "Product" ) );
            QueryableEntity Store5 = new QueryableEntity( DataMapUserDuplicated.EntityRelationshipModel.FindByName( "Store" ) );

            ComputedEntity ProductWithStore = new ComputedEntity( "ProductWithStore", 
                Product,
                (Relationship)DataMap.EntityRelationshipModel.FindByName( "StoreProducts" ),
                new List<QueryableEntity>() { Store } );

            ComputedEntity ProductWithStore2 = new ComputedEntity( "ProductWithStore",
                Product2,
                (Relationship)DataMapDuplicates.EntityRelationshipModel.FindByName( "StoreProducts" ),
                new List<QueryableEntity>() { Store2 } );

            ComputedEntity ProductWithStore3 = new ComputedEntity( "ProductWithStore",
                Product3,
                (Relationship)DataMapCategoryDuplicated.EntityRelationshipModel.FindByName( "StoreProducts" ),
                new List<QueryableEntity>() { Store3 } );

            ComputedEntity ProductWithStore4 = new ComputedEntity( "ProductWithStore",
                Product4,
                (Relationship)DataMapStoreDuplicated.EntityRelationshipModel.FindByName( "StoreProducts" ),
                new List<QueryableEntity>() { Store4 } );

            ComputedEntity ProductWithStore5 = new ComputedEntity( "ProductWithStore",
                Product5,
                (Relationship)DataMapUserDuplicated.EntityRelationshipModel.FindByName( "StoreProducts" ),
                new List<QueryableEntity>() { Store5 } );

            RelationshipJoinOperator RJoinOp1 = new RelationshipJoinOperator( Category, (Relationship)DataMap.EntityRelationshipModel.FindByName( "CategoryProducts" ),
                new List<QueryableEntity>() { new QueryableEntity( ProductWithStore ) },
                DataMap.ERMongoMapping );

            RelationshipJoinOperator RJoinOp2 = new RelationshipJoinOperator( Category2, (Relationship)DataMapDuplicates.EntityRelationshipModel.FindByName( "CategoryProducts" ),
                new List<QueryableEntity>() { new QueryableEntity( ProductWithStore2 ) },
                DataMapDuplicates.ERMongoMapping );

            RelationshipJoinOperator RJoinOp3 = new RelationshipJoinOperator( Category3, (Relationship)DataMapCategoryDuplicated.EntityRelationshipModel.FindByName( "CategoryProducts" ),
                new List<QueryableEntity>() { new QueryableEntity( ProductWithStore3 ) },
                DataMapCategoryDuplicated.ERMongoMapping );

            RelationshipJoinOperator RJoinOp4 = new RelationshipJoinOperator( Category4, (Relationship)DataMapStoreDuplicated.EntityRelationshipModel.FindByName( "CategoryProducts" ),
                new List<QueryableEntity>() { new QueryableEntity( ProductWithStore4 ) },
                DataMapStoreDuplicated.ERMongoMapping );

            RelationshipJoinOperator RJoinOp5 = new RelationshipJoinOperator( Category5, (Relationship)DataMapUserDuplicated.EntityRelationshipModel.FindByName( "CategoryProducts" ),
                new List<QueryableEntity>() { new QueryableEntity( ProductWithStore5 ) },
                DataMapUserDuplicated.ERMongoMapping );

            SortArgument SortArg = new SortArgument( Category, Category.GetAttribute( "CategoryID" ), MongoDBSort.Ascending );

            SortStage SortOpMap1 = new SortStage( new List<SortArgument>() { SortArg }, DataMap.ERMongoMapping );
            SortStage SortOpMap2 = new SortStage( new List<SortArgument>() { SortArg }, DataMapDuplicates.ERMongoMapping );
            SortStage SortOpMap3 = new SortStage( new List<SortArgument>() { SortArg }, DataMapCategoryDuplicated.ERMongoMapping );
            SortStage SortOpMap4 = new SortStage( new List<SortArgument>() { SortArg }, DataMapStoreDuplicated.ERMongoMapping );
            SortStage SortOpMap5 = new SortStage( new List<SortArgument>() { SortArg }, DataMapUserDuplicated.ERMongoMapping );

            List<AlgebraOperator> OperatorsToExecuteMap1 = new List<AlgebraOperator>() { RJoinOp1, SortOpMap1 };
            List<AlgebraOperator> OperatorsToExecuteMap2 = new List<AlgebraOperator>() { RJoinOp2, SortOpMap2 };
            List<AlgebraOperator> OperatorsToExecuteMap3 = new List<AlgebraOperator>() { RJoinOp3, SortOpMap3 };
            List<AlgebraOperator> OperatorsToExecuteMap4 = new List<AlgebraOperator>() { RJoinOp4, SortOpMap4 };
            List<AlgebraOperator> OperatorsToExecuteMap5 = new List<AlgebraOperator>() { RJoinOp5, SortOpMap5 };

            FromArgument StartArgMap1 = new FromArgument( Category, DataMap.ERMongoMapping );
            FromArgument StartArgMap2 = new FromArgument( Category, DataMapDuplicates.ERMongoMapping );
            FromArgument StartArgMap3 = new FromArgument( Category, DataMapCategoryDuplicated.ERMongoMapping );
            FromArgument StartArgMap4 = new FromArgument( Category, DataMapStoreDuplicated.ERMongoMapping );
            FromArgument StartArgMap5 = new FromArgument( Category, DataMapUserDuplicated.ERMongoMapping );

            QueryGenerator GeneratorMap1 = new QueryGenerator( StartArgMap1, OperatorsToExecuteMap1 );
            QueryGenerator GeneratorMap2 = new QueryGenerator( StartArgMap2, OperatorsToExecuteMap2 );
            QueryGenerator GeneratorMap3 = new QueryGenerator( StartArgMap3, OperatorsToExecuteMap3 );
            QueryGenerator GeneratorMap4 = new QueryGenerator( StartArgMap4, OperatorsToExecuteMap4 );
            QueryGenerator GeneratorMap5 = new QueryGenerator( StartArgMap5, OperatorsToExecuteMap5 );

            string QueryMap1 = GeneratorMap1.Run();
            Assert.ThrowsException<ImpossibleOperationException>( GeneratorMap2.Run );
            Assert.ThrowsException<ImpossibleOperationException>( GeneratorMap3.Run );
            string QueryMap4 = GeneratorMap4.Run();
            string QueryMap5 = GeneratorMap5.Run();

            Assert.IsNotNull( QueryMap1, "Query [Map1] cannot be null" );
            Assert.IsNotNull( QueryMap4, "Query [Map4] cannot be null" );
            Assert.IsNotNull( QueryMap5, "Query [Map5] cannot be null" );

            QueryRunner RunnerMap1 = new QueryRunner( "mongodb://localhost:27017", "pesquisa_cms" );
            QueryRunner RunnerMap4 = new QueryRunner( "mongodb://localhost:27017", "pesquisa_cms_store_duplicado" );
            QueryRunner RunnerMap5 = new QueryRunner( "mongodb://localhost:27017", "pesquisa_cms_user_duplicado" );

            string ResultMap1 = RunnerMap1.GetJSON( QueryMap1 );
            string ResultMap4 = RunnerMap4.GetJSON( QueryMap4 );
            string ResultMap5 = RunnerMap5.GetJSON( QueryMap5 );

            Assert.IsNotNull( ResultMap1, "Result [Map1] cannot be null" );
            Assert.IsNotNull( ResultMap4, "Result [Map4] cannot be null" );
            Assert.IsNotNull( ResultMap5, "Result [Map5] cannot be null" );

            Assert.IsTrue( ResultMap1 != string.Empty, "Result [Map1] cannot be empty" );
            Assert.IsTrue( ResultMap4 != string.Empty, "Result [Map4] cannot be empty" );
            Assert.IsTrue( ResultMap5 != string.Empty, "Result [Map5] cannot be empty" );

            JToken TokenResult1 = JToken.Parse( ResultMap1 );

            TokenResult1.Should().BeEquivalentTo( JToken.Parse( ResultMap4 ) );
            TokenResult1.Should().BeEquivalentTo( JToken.Parse( ResultMap5 ) );
        }

        /// <summary>
        /// Run GetAllProductsFromCategoryWithUser
        /// 
        /// QUERY: FROM Category c
        ///        RJOIN <CategoryProducts> (Product p 
        ///                                  RJOIN <UserProducts> (User u))
        ///        SELECT *
        /// </summary>
        [TestMethod]
        public void GetAllProductsFromCategoryWithUser()
        {
            //RequiredDataContainer DataMap = MarketingCMSDataProvider.MapEntitiesToCollections();
            //RequiredDataContainer DataMapDuplicates = MarketingCMSDataProvider.MapEntitiesToCollectionDuplicates();
            //RequiredDataContainer DataMapCategoryDuplicated = MarketingCMSDataProvider.MapEntitiesToCollectionCategoryDuplicated();
            //RequiredDataContainer DataMapStoreDuplicated = MarketingCMSDataProvider.MapEntitiesToCollectionsStoreDuplicated();
            //RequiredDataContainer DataMapUserDuplicated = MarketingCMSDataProvider.MapEntitiesToCollectionsUserDuplicated();
            var DataMap = QueryBuilderParser.ParseMapping( Utils.ReadMappingFile( "Mappings/MarketingCMS/entities-to-collections.mapping" ) );
            var DataMapDuplicates = QueryBuilderParser.ParseMapping( Utils.ReadMappingFile( "Mappings/MarketingCMS/entities-to-collections-duplicates.mapping" ) );
            var DataMapCategoryDuplicated = QueryBuilderParser.ParseMapping( Utils.ReadMappingFile( "Mappings/MarketingCMS/entities-to-collections-category-duplicated.mapping" ) );
            var DataMapStoreDuplicated = QueryBuilderParser.ParseMapping( Utils.ReadMappingFile( "Mappings/MarketingCMS/entities-to-collections-store-duplicated.mapping" ) );
            var DataMapUserDuplicated = QueryBuilderParser.ParseMapping( Utils.ReadMappingFile( "Mappings/MarketingCMS/entities-to-collections-user-duplicated.mapping" ) );

            QueryableEntity Category = new QueryableEntity( DataMap.EntityRelationshipModel.FindByName( "Category" ) );
            QueryableEntity Product = new QueryableEntity( DataMap.EntityRelationshipModel.FindByName( "Product" ) );
            QueryableEntity User = new QueryableEntity( DataMap.EntityRelationshipModel.FindByName( "User" ) );

            QueryableEntity Category2 = new QueryableEntity( DataMapDuplicates.EntityRelationshipModel.FindByName( "Category" ) );
            QueryableEntity Product2 = new QueryableEntity( DataMapDuplicates.EntityRelationshipModel.FindByName( "Product" ) );
            QueryableEntity User2 = new QueryableEntity( DataMapDuplicates.EntityRelationshipModel.FindByName( "User" ) );

            QueryableEntity Category3 = new QueryableEntity( DataMapCategoryDuplicated.EntityRelationshipModel.FindByName( "Category" ) );
            QueryableEntity Product3 = new QueryableEntity( DataMapCategoryDuplicated.EntityRelationshipModel.FindByName( "Product" ) );
            QueryableEntity User3 = new QueryableEntity( DataMapCategoryDuplicated.EntityRelationshipModel.FindByName( "User" ) );

            QueryableEntity Category4 = new QueryableEntity( DataMapStoreDuplicated.EntityRelationshipModel.FindByName( "Category" ) );
            QueryableEntity Product4 = new QueryableEntity( DataMapStoreDuplicated.EntityRelationshipModel.FindByName( "Product" ) );
            QueryableEntity User4 = new QueryableEntity( DataMapStoreDuplicated.EntityRelationshipModel.FindByName( "User" ) );

            QueryableEntity Category5 = new QueryableEntity( DataMapUserDuplicated.EntityRelationshipModel.FindByName( "Category" ) );
            QueryableEntity Product5 = new QueryableEntity( DataMapUserDuplicated.EntityRelationshipModel.FindByName( "Product" ) );
            QueryableEntity User5 = new QueryableEntity( DataMapUserDuplicated.EntityRelationshipModel.FindByName( "User" ) );

            ComputedEntity ProductWithUser = new ComputedEntity( "ProductWithStore",
                Product,
                (Relationship)DataMap.EntityRelationshipModel.FindByName( "UserProducts" ),
                new List<QueryableEntity>() { User } );

            ComputedEntity ProductWithUser2 = new ComputedEntity( "ProductWithStore",
                Product2,
                (Relationship)DataMapDuplicates.EntityRelationshipModel.FindByName( "UserProducts" ),
                new List<QueryableEntity>() { User2 } );

            ComputedEntity ProductWithUser3 = new ComputedEntity( "ProductWithStore",
                Product3,
                (Relationship)DataMapCategoryDuplicated.EntityRelationshipModel.FindByName( "UserProducts" ),
                new List<QueryableEntity>() { User3 } );

            ComputedEntity ProductWithUser4 = new ComputedEntity( "ProductWithStore",
                Product4,
                (Relationship)DataMapStoreDuplicated.EntityRelationshipModel.FindByName( "UserProducts" ),
                new List<QueryableEntity>() { User4 } );

            ComputedEntity ProductWithUser5 = new ComputedEntity( "ProductWithStore",
                Product5,
                (Relationship)DataMapUserDuplicated.EntityRelationshipModel.FindByName( "UserProducts" ),
                new List<QueryableEntity>() { User5 } );

            RelationshipJoinOperator RJoinOp1 = new RelationshipJoinOperator( Category, (Relationship)DataMap.EntityRelationshipModel.FindByName( "CategoryProducts" ),
                new List<QueryableEntity>() { new QueryableEntity( ProductWithUser ) },
                DataMap.ERMongoMapping );

            RelationshipJoinOperator RJoinOp2 = new RelationshipJoinOperator( Category2, (Relationship)DataMapDuplicates.EntityRelationshipModel.FindByName( "CategoryProducts" ),
                new List<QueryableEntity>() { new QueryableEntity( ProductWithUser2 ) },
                DataMapDuplicates.ERMongoMapping );

            RelationshipJoinOperator RJoinOp3 = new RelationshipJoinOperator( Category3, (Relationship)DataMapCategoryDuplicated.EntityRelationshipModel.FindByName( "CategoryProducts" ),
                new List<QueryableEntity>() { new QueryableEntity( ProductWithUser3 ) },
                DataMapCategoryDuplicated.ERMongoMapping );

            RelationshipJoinOperator RJoinOp4 = new RelationshipJoinOperator( Category4, (Relationship)DataMapStoreDuplicated.EntityRelationshipModel.FindByName( "CategoryProducts" ),
                new List<QueryableEntity>() { new QueryableEntity( ProductWithUser4 ) },
                DataMapStoreDuplicated.ERMongoMapping );

            RelationshipJoinOperator RJoinOp5 = new RelationshipJoinOperator( Category5, (Relationship)DataMapUserDuplicated.EntityRelationshipModel.FindByName( "CategoryProducts" ),
                new List<QueryableEntity>() { new QueryableEntity( ProductWithUser5 ) },
                DataMapUserDuplicated.ERMongoMapping );

            SortArgument SortArg = new SortArgument( Category, Category.GetAttribute( "CategoryID" ), MongoDBSort.Ascending );

            SortStage SortOpMap1 = new SortStage( new List<SortArgument>() { SortArg }, DataMap.ERMongoMapping );
            SortStage SortOpMap2 = new SortStage( new List<SortArgument>() { SortArg }, DataMapDuplicates.ERMongoMapping );
            SortStage SortOpMap3 = new SortStage( new List<SortArgument>() { SortArg }, DataMapCategoryDuplicated.ERMongoMapping );
            SortStage SortOpMap4 = new SortStage( new List<SortArgument>() { SortArg }, DataMapStoreDuplicated.ERMongoMapping );
            SortStage SortOpMap5 = new SortStage( new List<SortArgument>() { SortArg }, DataMapUserDuplicated.ERMongoMapping );

            List<AlgebraOperator> OperatorsToExecuteMap1 = new List<AlgebraOperator>() { RJoinOp1, SortOpMap1 };
            List<AlgebraOperator> OperatorsToExecuteMap2 = new List<AlgebraOperator>() { RJoinOp2, SortOpMap2 };
            List<AlgebraOperator> OperatorsToExecuteMap3 = new List<AlgebraOperator>() { RJoinOp3, SortOpMap3 };
            List<AlgebraOperator> OperatorsToExecuteMap4 = new List<AlgebraOperator>() { RJoinOp4, SortOpMap4 };
            List<AlgebraOperator> OperatorsToExecuteMap5 = new List<AlgebraOperator>() { RJoinOp5, SortOpMap5 };

            FromArgument StartArgMap1 = new FromArgument( Category, DataMap.ERMongoMapping );
            FromArgument StartArgMap2 = new FromArgument( Category, DataMapDuplicates.ERMongoMapping );
            FromArgument StartArgMap3 = new FromArgument( Category, DataMapCategoryDuplicated.ERMongoMapping );
            FromArgument StartArgMap4 = new FromArgument( Category, DataMapStoreDuplicated.ERMongoMapping );
            FromArgument StartArgMap5 = new FromArgument( Category, DataMapUserDuplicated.ERMongoMapping );

            QueryGenerator GeneratorMap1 = new QueryGenerator( StartArgMap1, OperatorsToExecuteMap1 );
            QueryGenerator GeneratorMap2 = new QueryGenerator( StartArgMap2, OperatorsToExecuteMap2 );
            QueryGenerator GeneratorMap3 = new QueryGenerator( StartArgMap3, OperatorsToExecuteMap3 );
            QueryGenerator GeneratorMap4 = new QueryGenerator( StartArgMap4, OperatorsToExecuteMap4 );
            QueryGenerator GeneratorMap5 = new QueryGenerator( StartArgMap5, OperatorsToExecuteMap5 );

            string QueryMap1 = GeneratorMap1.Run();
            Assert.ThrowsException<ImpossibleOperationException>( GeneratorMap2.Run );
            Assert.ThrowsException<ImpossibleOperationException>( GeneratorMap3.Run );
            string QueryMap4 = GeneratorMap4.Run();
            string QueryMap5 = GeneratorMap5.Run();

            Assert.IsNotNull( QueryMap1, "Query [Map1] cannot be null" );
            Assert.IsNotNull( QueryMap4, "Query [Map4] cannot be null" );
            Assert.IsNotNull( QueryMap5, "Query [Map5] cannot be null" );

            QueryRunner RunnerMap1 = new QueryRunner( "mongodb://localhost:27017", "pesquisa_cms" );
            QueryRunner RunnerMap4 = new QueryRunner( "mongodb://localhost:27017", "pesquisa_cms_store_duplicado" );
            QueryRunner RunnerMap5 = new QueryRunner( "mongodb://localhost:27017", "pesquisa_cms_user_duplicado" );

            string ResultMap1 = RunnerMap1.GetJSON( QueryMap1 );
            string ResultMap4 = RunnerMap4.GetJSON( QueryMap4 );
            string ResultMap5 = RunnerMap5.GetJSON( QueryMap5 );

            Assert.IsNotNull( ResultMap1, "Result [Map1] cannot be null" );
            Assert.IsNotNull( ResultMap4, "Result [Map4] cannot be null" );
            Assert.IsNotNull( ResultMap5, "Result [Map5] cannot be null" );

            Assert.IsTrue( ResultMap1 != string.Empty, "Result [Map1] cannot be empty" );
            Assert.IsTrue( ResultMap4 != string.Empty, "Result [Map4] cannot be empty" );
            Assert.IsTrue( ResultMap5 != string.Empty, "Result [Map5] cannot be empty" );

            JToken TokenResult1 = JToken.Parse( ResultMap1 );

            TokenResult1.Should().BeEquivalentTo( JToken.Parse( ResultMap4 ) );
            TokenResult1.Should().BeEquivalentTo( JToken.Parse( ResultMap5 ) );
        }
        /// <summary>
        /// Run GetProductTitleAndUserName test
        /// 
        /// QUERY: FROM Product p
        ///        RJOIN <UserProducts> (User u)
        ///        SELECT Product.Title, User.UserName
        /// </summary>
        [TestMethod]
        public void GetProductTitleAndUserName()
        {
            //RequiredDataContainer DataMap = MarketingCMSDataProvider.MapEntitiesToCollections();
            //RequiredDataContainer DataMapDuplicates = MarketingCMSDataProvider.MapEntitiesToCollectionDuplicates();
            //RequiredDataContainer DataMapCategoryDuplicated = MarketingCMSDataProvider.MapEntitiesToCollectionCategoryDuplicated();
            //RequiredDataContainer DataMapStoreDuplicated = MarketingCMSDataProvider.MapEntitiesToCollectionsStoreDuplicated();
            //RequiredDataContainer DataMapUserDuplicated = MarketingCMSDataProvider.MapEntitiesToCollectionsUserDuplicated();
            var DataMap = QueryBuilderParser.ParseMapping( Utils.ReadMappingFile( "Mappings/MarketingCMS/entities-to-collections.mapping" ) );
            var DataMapDuplicates = QueryBuilderParser.ParseMapping( Utils.ReadMappingFile( "Mappings/MarketingCMS/entities-to-collections-duplicates.mapping" ) );
            var DataMapCategoryDuplicated = QueryBuilderParser.ParseMapping( Utils.ReadMappingFile( "Mappings/MarketingCMS/entities-to-collections-category-duplicated.mapping" ) );
            var DataMapStoreDuplicated = QueryBuilderParser.ParseMapping( Utils.ReadMappingFile( "Mappings/MarketingCMS/entities-to-collections-store-duplicated.mapping" ) );
            var DataMapUserDuplicated = QueryBuilderParser.ParseMapping( Utils.ReadMappingFile( "Mappings/MarketingCMS/entities-to-collections-user-duplicated.mapping" ) );

            QueryableEntity Product = new QueryableEntity( DataMap.EntityRelationshipModel.FindByName( "Product" ) );
            QueryableEntity User = new QueryableEntity( DataMap.EntityRelationshipModel.FindByName( "User" ) );

            QueryableEntity Product2 = new QueryableEntity( DataMapDuplicates.EntityRelationshipModel.FindByName( "Product" ) );
            QueryableEntity User2 = new QueryableEntity( DataMapDuplicates.EntityRelationshipModel.FindByName( "User" ) );

            QueryableEntity Product3 = new QueryableEntity( DataMapCategoryDuplicated.EntityRelationshipModel.FindByName( "Product" ) );
            QueryableEntity User3 = new QueryableEntity( DataMapCategoryDuplicated.EntityRelationshipModel.FindByName( "User" ) );

            QueryableEntity Product4 = new QueryableEntity( DataMapStoreDuplicated.EntityRelationshipModel.FindByName( "Product" ) );
            QueryableEntity User4 = new QueryableEntity( DataMapStoreDuplicated.EntityRelationshipModel.FindByName( "User" ) );

            QueryableEntity Product5 = new QueryableEntity( DataMapUserDuplicated.EntityRelationshipModel.FindByName( "Product" ) );
            QueryableEntity User5 = new QueryableEntity( DataMapUserDuplicated.EntityRelationshipModel.FindByName( "User" ) );

            List<ProjectArgument> ProjectArgs = new List<ProjectArgument>();
            ProjectArgs.Add( new ProjectArgument( Product.GetAttribute( "Title" ), Product, new BooleanExpr( true ) ) );
            ProjectArgs.Add( new ProjectArgument( User.GetAttribute( "UserName" ), User, new BooleanExpr( true ) ) );

            RelationshipJoinOperator RJoinOp1 = new RelationshipJoinOperator( Product, (Relationship)DataMap.EntityRelationshipModel.FindByName( "UserProducts" ),
                new List<QueryableEntity>() { User },
                DataMap.ERMongoMapping );

            ProjectStage ProjectOp1 = new ProjectStage( ProjectArgs, RJoinOp1.ComputeVirtualMap() );

            RelationshipJoinOperator RJoinOp2 = new RelationshipJoinOperator( Product2, (Relationship)DataMapDuplicates.EntityRelationshipModel.FindByName( "CategoryProducts" ),
                new List<QueryableEntity>() { User2 },
                DataMapDuplicates.ERMongoMapping );

            ProjectStage ProjectOp2 = new ProjectStage( ProjectArgs, RJoinOp2.ComputeVirtualMap() );

            RelationshipJoinOperator RJoinOp3 = new RelationshipJoinOperator( Product3, (Relationship)DataMapCategoryDuplicated.EntityRelationshipModel.FindByName( "CategoryProducts" ),
                new List<QueryableEntity>() { User3 },
                DataMapCategoryDuplicated.ERMongoMapping );

            ProjectStage ProjectOp3 = new ProjectStage( ProjectArgs, RJoinOp3.ComputeVirtualMap() );

            RelationshipJoinOperator RJoinOp4 = new RelationshipJoinOperator( Product4, (Relationship)DataMapStoreDuplicated.EntityRelationshipModel.FindByName( "CategoryProducts" ),
                new List<QueryableEntity>() { User4 },
                DataMapStoreDuplicated.ERMongoMapping );

            ProjectStage ProjectOp4 = new ProjectStage( ProjectArgs, RJoinOp4.ComputeVirtualMap() );

            RelationshipJoinOperator RJoinOp5 = new RelationshipJoinOperator( Product5, (Relationship)DataMapUserDuplicated.EntityRelationshipModel.FindByName( "CategoryProducts" ),
                new List<QueryableEntity>() { User5 },
                DataMapUserDuplicated.ERMongoMapping );

            ProjectStage ProjectOp5 = new ProjectStage( ProjectArgs, RJoinOp5.ComputeVirtualMap() );

            SortArgument SortArg = new SortArgument( Product, Product.GetAttribute( "ProductID" ), MongoDBSort.Ascending );

            SortStage SortOpMap1 = new SortStage( new List<SortArgument>() { SortArg }, DataMap.ERMongoMapping );
            SortStage SortOpMap2 = new SortStage( new List<SortArgument>() { SortArg }, DataMapDuplicates.ERMongoMapping );
            SortStage SortOpMap3 = new SortStage( new List<SortArgument>() { SortArg }, DataMapCategoryDuplicated.ERMongoMapping );
            SortStage SortOpMap4 = new SortStage( new List<SortArgument>() { SortArg }, DataMapStoreDuplicated.ERMongoMapping );
            SortStage SortOpMap5 = new SortStage( new List<SortArgument>() { SortArg }, DataMapUserDuplicated.ERMongoMapping );

            List<AlgebraOperator> OperatorsToExecuteMap1 = new List<AlgebraOperator>() { RJoinOp1, ProjectOp1, SortOpMap1 };
            List<AlgebraOperator> OperatorsToExecuteMap2 = new List<AlgebraOperator>() { RJoinOp2, ProjectOp2, SortOpMap2 };
            List<AlgebraOperator> OperatorsToExecuteMap3 = new List<AlgebraOperator>() { RJoinOp3, ProjectOp3, SortOpMap3 };
            List<AlgebraOperator> OperatorsToExecuteMap4 = new List<AlgebraOperator>() { RJoinOp4, ProjectOp4, SortOpMap4 };
            List<AlgebraOperator> OperatorsToExecuteMap5 = new List<AlgebraOperator>() { RJoinOp5, ProjectOp5, SortOpMap5 };

            FromArgument StartArgMap1 = new FromArgument( Product, DataMap.ERMongoMapping );
            FromArgument StartArgMap2 = new FromArgument( Product2, DataMapDuplicates.ERMongoMapping );
            FromArgument StartArgMap3 = new FromArgument( Product3, DataMapCategoryDuplicated.ERMongoMapping );
            FromArgument StartArgMap4 = new FromArgument( Product4, DataMapStoreDuplicated.ERMongoMapping );
            FromArgument StartArgMap5 = new FromArgument( Product5, DataMapUserDuplicated.ERMongoMapping );

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

            Assert.IsNotNull( QueryMap1, "Query [Map1] cannot be null" );
            Assert.IsNotNull( QueryMap2, "Query [Map2] cannot be null" );
            Assert.IsNotNull( QueryMap3, "Query [Map3] cannot be null" );
            Assert.IsNotNull( QueryMap4, "Query [Map4] cannot be null" );
            Assert.IsNotNull( QueryMap5, "Query [Map5] cannot be null" );

            QueryRunner RunnerMap1 = new QueryRunner( "mongodb://localhost:27017", "pesquisa_cms" );
            QueryRunner RunnerMap2 = new QueryRunner( "mongodb://localhost:27017", "pesquisa_cms_duplicados" );
            QueryRunner RunnerMap3 = new QueryRunner( "mongodb://localhost:27017", "pesquisa_cms_category_duplicado" );
            QueryRunner RunnerMap4 = new QueryRunner( "mongodb://localhost:27017", "pesquisa_cms_store_duplicado" );
            QueryRunner RunnerMap5 = new QueryRunner( "mongodb://localhost:27017", "pesquisa_cms_user_duplicado" );

            string ResultMap1 = RunnerMap1.GetJSON( QueryMap1 );
            string ResultMap2 = RunnerMap2.GetJSON( QueryMap2 );
            string ResultMap3 = RunnerMap3.GetJSON( QueryMap3 );
            string ResultMap4 = RunnerMap4.GetJSON( QueryMap4 );
            string ResultMap5 = RunnerMap5.GetJSON( QueryMap5 );

            Assert.IsNotNull( ResultMap1, "Result [Map1] cannot be null" );
            Assert.IsNotNull( ResultMap2, "Result [Map2] cannot be null" );
            Assert.IsNotNull( ResultMap3, "Result [Map3] cannot be null" );
            Assert.IsNotNull( ResultMap4, "Result [Map4] cannot be null" );
            Assert.IsNotNull( ResultMap5, "Result [Map5] cannot be null" );

            Assert.IsTrue( ResultMap1 != string.Empty, "Result [Map1] cannot be empty" );
            Assert.IsTrue( ResultMap2 != string.Empty, "Result [Map2] cannot be empty" );
            Assert.IsTrue( ResultMap3 != string.Empty, "Result [Map3] cannot be empty" );
            Assert.IsTrue( ResultMap4 != string.Empty, "Result [Map4] cannot be empty" );
            Assert.IsTrue( ResultMap5 != string.Empty, "Result [Map5] cannot be empty" );

            JToken TokenResult1 = JToken.Parse( ResultMap1 );

            TokenResult1.Should().BeEquivalentTo( JToken.Parse( ResultMap2 ) );
            TokenResult1.Should().BeEquivalentTo( JToken.Parse( ResultMap3 ) );
            TokenResult1.Should().BeEquivalentTo( JToken.Parse( ResultMap4 ) );
            TokenResult1.Should().BeEquivalentTo( JToken.Parse( ResultMap5 ) );
        }

        /// <summary>
        /// Run GetAllProductsFromCategoryWithUserAndSelectOnlyTitleNameEmailCategoryName
        /// 
        /// QUERY: FROM Category c
        ///        RJOIN <CategoryProducts> (Product p
        ///                                  RJOIN <UserProducts> (User u))
        ///        SELECT Category.CategoryName, Product.Title, User.UserName, User.UserEmail
        /// </summary>
        [TestMethod]
        public void GetAllProductsFromCategoryWithUserAndSelectOnlyTitleNameEmailCategoryName()
        {
            //RequiredDataContainer DataMap = MarketingCMSDataProvider.MapEntitiesToCollections();
            //RequiredDataContainer DataMapDuplicates = MarketingCMSDataProvider.MapEntitiesToCollectionDuplicates();
            //RequiredDataContainer DataMapCategoryDuplicated = MarketingCMSDataProvider.MapEntitiesToCollectionCategoryDuplicated();
            //RequiredDataContainer DataMapStoreDuplicated = MarketingCMSDataProvider.MapEntitiesToCollectionsStoreDuplicated();
            //RequiredDataContainer DataMapUserDuplicated = MarketingCMSDataProvider.MapEntitiesToCollectionsUserDuplicated();
            var DataMap = QueryBuilderParser.ParseMapping( Utils.ReadMappingFile( "Mappings/MarketingCMS/entities-to-collections.mapping" ) );
            var DataMapDuplicates = QueryBuilderParser.ParseMapping( Utils.ReadMappingFile( "Mappings/MarketingCMS/entities-to-collections-duplicates.mapping" ) );
            var DataMapCategoryDuplicated = QueryBuilderParser.ParseMapping( Utils.ReadMappingFile( "Mappings/MarketingCMS/entities-to-collections-category-duplicated.mapping" ) );
            var DataMapStoreDuplicated = QueryBuilderParser.ParseMapping( Utils.ReadMappingFile( "Mappings/MarketingCMS/entities-to-collections-store-duplicated.mapping" ) );
            var DataMapUserDuplicated = QueryBuilderParser.ParseMapping( Utils.ReadMappingFile( "Mappings/MarketingCMS/entities-to-collections-user-duplicated.mapping" ) );

            QueryableEntity Category = new QueryableEntity( DataMap.EntityRelationshipModel.FindByName( "Category" ) );
            QueryableEntity Product = new QueryableEntity( DataMap.EntityRelationshipModel.FindByName( "Product" ) );
            QueryableEntity User = new QueryableEntity( DataMap.EntityRelationshipModel.FindByName( "User" ) );

            QueryableEntity Category2 = new QueryableEntity( DataMapDuplicates.EntityRelationshipModel.FindByName( "Category" ) );
            QueryableEntity Product2 = new QueryableEntity( DataMapDuplicates.EntityRelationshipModel.FindByName( "Product" ) );
            QueryableEntity User2 = new QueryableEntity( DataMapDuplicates.EntityRelationshipModel.FindByName( "User" ) );

            QueryableEntity Category3 = new QueryableEntity( DataMapCategoryDuplicated.EntityRelationshipModel.FindByName( "Category" ) );
            QueryableEntity Product3 = new QueryableEntity( DataMapCategoryDuplicated.EntityRelationshipModel.FindByName( "Product" ) );
            QueryableEntity User3 = new QueryableEntity( DataMapCategoryDuplicated.EntityRelationshipModel.FindByName( "User" ) );

            QueryableEntity Category4 = new QueryableEntity( DataMapStoreDuplicated.EntityRelationshipModel.FindByName( "Category" ) );
            QueryableEntity Product4 = new QueryableEntity( DataMapStoreDuplicated.EntityRelationshipModel.FindByName( "Product" ) );
            QueryableEntity User4 = new QueryableEntity( DataMapStoreDuplicated.EntityRelationshipModel.FindByName( "User" ) );

            QueryableEntity Category5 = new QueryableEntity( DataMapUserDuplicated.EntityRelationshipModel.FindByName( "Category" ) );
            QueryableEntity Product5 = new QueryableEntity( DataMapUserDuplicated.EntityRelationshipModel.FindByName( "Product" ) );
            QueryableEntity User5 = new QueryableEntity( DataMapUserDuplicated.EntityRelationshipModel.FindByName( "User" ) );

            List<ProjectArgument> ProjectArgs = new List<ProjectArgument>();
            ProjectArgs.Add( new ProjectArgument( Product.GetAttribute( "Title" ), Product, new BooleanExpr( true ) ) );
            ProjectArgs.Add( new ProjectArgument( User.GetAttribute( "UserName" ), User, new BooleanExpr( true ) ) );
            ProjectArgs.Add( new ProjectArgument( User.GetAttribute( "UserEmail" ), User, new BooleanExpr( true ) ) );
            ProjectArgs.Add( new ProjectArgument( Category.GetAttribute( "CategoryName" ), Category, new BooleanExpr( true ) ) );

            ComputedEntity ProductWithUser = new ComputedEntity( "ProductWithStore",
                Product,
                (Relationship)DataMap.EntityRelationshipModel.FindByName( "UserProducts" ),
                new List<QueryableEntity>() { User } );

            ComputedEntity ProductWithUser2 = new ComputedEntity( "ProductWithStore",
                Product2,
                (Relationship)DataMapDuplicates.EntityRelationshipModel.FindByName( "UserProducts" ),
                new List<QueryableEntity>() { User2 } );

            ComputedEntity ProductWithUser3 = new ComputedEntity( "ProductWithStore",
                Product3,
                (Relationship)DataMapCategoryDuplicated.EntityRelationshipModel.FindByName( "UserProducts" ),
                new List<QueryableEntity>() { User3 } );

            ComputedEntity ProductWithUser4 = new ComputedEntity( "ProductWithStore",
                Product4,
                (Relationship)DataMapStoreDuplicated.EntityRelationshipModel.FindByName( "UserProducts" ),
                new List<QueryableEntity>() { User4 } );

            ComputedEntity ProductWithUser5 = new ComputedEntity( "ProductWithStore",
                Product5,
                (Relationship)DataMapUserDuplicated.EntityRelationshipModel.FindByName( "UserProducts" ),
                new List<QueryableEntity>() { User5 } );

            RelationshipJoinOperator RJoinOp1 = new RelationshipJoinOperator( Category, (Relationship)DataMap.EntityRelationshipModel.FindByName( "CategoryProducts" ),
                new List<QueryableEntity>() { new QueryableEntity( ProductWithUser ) },
                DataMap.ERMongoMapping );

            ProjectStage ProjectOp1 = new ProjectStage( ProjectArgs, RJoinOp1.ComputeVirtualMap() );

            RelationshipJoinOperator RJoinOp2 = new RelationshipJoinOperator( Category2, (Relationship)DataMapDuplicates.EntityRelationshipModel.FindByName( "CategoryProducts" ),
                new List<QueryableEntity>() { new QueryableEntity( ProductWithUser2 ) },
                DataMapDuplicates.ERMongoMapping );

            ProjectStage ProjectOp2 = new ProjectStage( ProjectArgs, RJoinOp2.ComputeVirtualMap() );

            RelationshipJoinOperator RJoinOp3 = new RelationshipJoinOperator( Category3, (Relationship)DataMapCategoryDuplicated.EntityRelationshipModel.FindByName( "CategoryProducts" ),
                new List<QueryableEntity>() { new QueryableEntity( ProductWithUser3 ) },
                DataMapCategoryDuplicated.ERMongoMapping );

            ProjectStage ProjectOp3 = new ProjectStage( ProjectArgs, RJoinOp3.ComputeVirtualMap() );

            RelationshipJoinOperator RJoinOp4 = new RelationshipJoinOperator( Category4, (Relationship)DataMapStoreDuplicated.EntityRelationshipModel.FindByName( "CategoryProducts" ),
                new List<QueryableEntity>() { new QueryableEntity( ProductWithUser4 ) },
                DataMapStoreDuplicated.ERMongoMapping );

            ProjectStage ProjectOp4 = new ProjectStage( ProjectArgs, RJoinOp4.ComputeVirtualMap() );

            RelationshipJoinOperator RJoinOp5 = new RelationshipJoinOperator( Category5, (Relationship)DataMapUserDuplicated.EntityRelationshipModel.FindByName( "CategoryProducts" ),
                new List<QueryableEntity>() { new QueryableEntity( ProductWithUser5 ) },
                DataMapUserDuplicated.ERMongoMapping );

            ProjectStage ProjectOp5 = new ProjectStage( ProjectArgs, RJoinOp5.ComputeVirtualMap() );

            SortArgument SortArg = new SortArgument( Category, Category.GetAttribute( "CategoryID" ), MongoDBSort.Ascending );

            SortStage SortOpMap1 = new SortStage( new List<SortArgument>() { SortArg }, DataMap.ERMongoMapping );
            SortStage SortOpMap2 = new SortStage( new List<SortArgument>() { SortArg }, DataMapDuplicates.ERMongoMapping );
            SortStage SortOpMap3 = new SortStage( new List<SortArgument>() { SortArg }, DataMapCategoryDuplicated.ERMongoMapping );
            SortStage SortOpMap4 = new SortStage( new List<SortArgument>() { SortArg }, DataMapStoreDuplicated.ERMongoMapping );
            SortStage SortOpMap5 = new SortStage( new List<SortArgument>() { SortArg }, DataMapUserDuplicated.ERMongoMapping );

            List<AlgebraOperator> OperatorsToExecuteMap1 = new List<AlgebraOperator>() { RJoinOp1, ProjectOp1, SortOpMap1 };
            List<AlgebraOperator> OperatorsToExecuteMap2 = new List<AlgebraOperator>() { RJoinOp2, ProjectOp2, SortOpMap2 };
            List<AlgebraOperator> OperatorsToExecuteMap3 = new List<AlgebraOperator>() { RJoinOp3, ProjectOp3, SortOpMap3 };
            List<AlgebraOperator> OperatorsToExecuteMap4 = new List<AlgebraOperator>() { RJoinOp4, ProjectOp4, SortOpMap4 };
            List<AlgebraOperator> OperatorsToExecuteMap5 = new List<AlgebraOperator>() { RJoinOp5, ProjectOp5, SortOpMap5 };

            FromArgument StartArgMap1 = new FromArgument( Category, DataMap.ERMongoMapping );
            FromArgument StartArgMap2 = new FromArgument( Category2, DataMapDuplicates.ERMongoMapping );
            FromArgument StartArgMap3 = new FromArgument( Category3, DataMapCategoryDuplicated.ERMongoMapping );
            FromArgument StartArgMap4 = new FromArgument( Category4, DataMapStoreDuplicated.ERMongoMapping );
            FromArgument StartArgMap5 = new FromArgument( Category5, DataMapUserDuplicated.ERMongoMapping );

            QueryGenerator GeneratorMap1 = new QueryGenerator( StartArgMap1, OperatorsToExecuteMap1 );
            QueryGenerator GeneratorMap2 = new QueryGenerator( StartArgMap2, OperatorsToExecuteMap2 );
            QueryGenerator GeneratorMap3 = new QueryGenerator( StartArgMap3, OperatorsToExecuteMap3 );
            QueryGenerator GeneratorMap4 = new QueryGenerator( StartArgMap4, OperatorsToExecuteMap4 );
            QueryGenerator GeneratorMap5 = new QueryGenerator( StartArgMap5, OperatorsToExecuteMap5 );

            string QueryMap1 = GeneratorMap1.Run();
            Assert.ThrowsException<ImpossibleOperationException>( GeneratorMap2.Run );
            Assert.ThrowsException<ImpossibleOperationException>( GeneratorMap3.Run );
            string QueryMap4 = GeneratorMap4.Run();
            string QueryMap5 = GeneratorMap5.Run();

            Assert.IsNotNull( QueryMap1, "Query [Map1] cannot be null" );
            Assert.IsNotNull( QueryMap4, "Query [Map4] cannot be null" );
            Assert.IsNotNull( QueryMap5, "Query [Map5] cannot be null" );

            QueryRunner RunnerMap1 = new QueryRunner( "mongodb://localhost:27017", "pesquisa_cms" );
            QueryRunner RunnerMap4 = new QueryRunner( "mongodb://localhost:27017", "pesquisa_cms_store_duplicado" );
            QueryRunner RunnerMap5 = new QueryRunner( "mongodb://localhost:27017", "pesquisa_cms_user_duplicado" );

            string ResultMap1 = RunnerMap1.GetJSON( QueryMap1 );
            string ResultMap4 = RunnerMap4.GetJSON( QueryMap4 );
            string ResultMap5 = RunnerMap5.GetJSON( QueryMap5 );

            Assert.IsNotNull( ResultMap1, "Result [Map1] cannot be null" );
            Assert.IsNotNull( ResultMap4, "Result [Map4] cannot be null" );
            Assert.IsNotNull( ResultMap5, "Result [Map5] cannot be null" );

            Assert.IsTrue( ResultMap1 != string.Empty, "Result [Map1] cannot be empty" );
            Assert.IsTrue( ResultMap4 != string.Empty, "Result [Map4] cannot be empty" );
            Assert.IsTrue( ResultMap5 != string.Empty, "Result [Map5] cannot be empty" );

            JToken TokenResult1 = JToken.Parse( ResultMap1 );

            TokenResult1.Should().BeEquivalentTo( JToken.Parse( ResultMap4 ) );
            TokenResult1.Should().BeEquivalentTo( JToken.Parse( ResultMap5 ) );
        }

        /// <summary>
        /// Execute GetCategoryThatIsNamedHome
        /// 
        /// Query: FROM Category
        ///        WHERE CategoryName = 'Home'
        ///        SELECT *
        /// </summary>
        [TestMethod]
        public void GetCategoryThatIsNamedHome()
        {
            //RequiredDataContainer DataMap = MarketingCMSDataProvider.MapEntitiesToCollections();
            //RequiredDataContainer DataMapDuplicates = MarketingCMSDataProvider.MapEntitiesToCollectionDuplicates();
            //RequiredDataContainer DataMapCategoryDuplicated = MarketingCMSDataProvider.MapEntitiesToCollectionCategoryDuplicated();
            //RequiredDataContainer DataMapStoreDuplicated = MarketingCMSDataProvider.MapEntitiesToCollectionsStoreDuplicated();
            //RequiredDataContainer DataMapUserDuplicated = MarketingCMSDataProvider.MapEntitiesToCollectionsUserDuplicated();
            var DataMap = QueryBuilderParser.ParseMapping( Utils.ReadMappingFile( "Mappings/MarketingCMS/entities-to-collections.mapping" ) );
            var DataMapDuplicates = QueryBuilderParser.ParseMapping( Utils.ReadMappingFile( "Mappings/MarketingCMS/entities-to-collections-duplicates.mapping" ) );
            var DataMapCategoryDuplicated = QueryBuilderParser.ParseMapping( Utils.ReadMappingFile( "Mappings/MarketingCMS/entities-to-collections-category-duplicated.mapping" ) );
            var DataMapStoreDuplicated = QueryBuilderParser.ParseMapping( Utils.ReadMappingFile( "Mappings/MarketingCMS/entities-to-collections-store-duplicated.mapping" ) );
            var DataMapUserDuplicated = QueryBuilderParser.ParseMapping( Utils.ReadMappingFile( "Mappings/MarketingCMS/entities-to-collections-user-duplicated.mapping" ) );

            QueryableEntity Category = new QueryableEntity( DataMap.EntityRelationshipModel.FindByName( "Category" ) );
            QueryableEntity Category4 = new QueryableEntity( DataMapStoreDuplicated.EntityRelationshipModel.FindByName( "Category" ) );
            QueryableEntity Category5 = new QueryableEntity( DataMapUserDuplicated.EntityRelationshipModel.FindByName( "Category" ) );

            MapRule CategoryRule01 = DataMap.ERMongoMapping.FindMainRule( Category.Element );
            SelectArgument SelectArg01 = new SelectArgument( new EqExpr( $"${CategoryRule01.GetRuleValueForAttribute( Category.GetAttribute( "CategoryName" ) )}", "Home" ) );
            SelectStage SelectOp1 = new SelectStage( SelectArg01, DataMap.ERMongoMapping );

            MapRule CategoryRule04 = DataMapStoreDuplicated.ERMongoMapping.FindMainRule( Category4.Element );
            SelectArgument SelectArg04 = new SelectArgument( new EqExpr( $"${CategoryRule04.GetRuleValueForAttribute( Category4.GetAttribute( "CategoryName" ) )}", "Home" ) );
            SelectStage SelectOp4 = new SelectStage( SelectArg04, DataMapStoreDuplicated.ERMongoMapping );

            MapRule CategoryRule05 = DataMapUserDuplicated.ERMongoMapping.FindMainRule( Category5.Element );
            SelectArgument SelectArg05 = new SelectArgument( new EqExpr( $"${CategoryRule05.GetRuleValueForAttribute( Category5.GetAttribute( "CategoryName" ) )}", "Home" ) );
            SelectStage SelectOp5 = new SelectStage( SelectArg05, DataMapUserDuplicated.ERMongoMapping );

            SortArgument SortArg = new SortArgument( Category, Category.GetAttribute( "CategoryID" ), MongoDBSort.Ascending );
            SortArgument SortArg4 = new SortArgument( Category4, Category4.GetAttribute( "CategoryID" ), MongoDBSort.Ascending );
            SortArgument SortArg5 = new SortArgument( Category5, Category5.GetAttribute( "CategoryID" ), MongoDBSort.Ascending );

            SortStage SortOpMap1 = new SortStage( new List<SortArgument>() { SortArg }, DataMap.ERMongoMapping );
            SortStage SortOpMap4 = new SortStage( new List<SortArgument>() { SortArg }, DataMapStoreDuplicated.ERMongoMapping );
            SortStage SortOpMap5 = new SortStage( new List<SortArgument>() { SortArg }, DataMapUserDuplicated.ERMongoMapping );

            List<AlgebraOperator> OperatorsToExecuteMap1 = new List<AlgebraOperator>() { SelectOp1, SortOpMap1 };
            List<AlgebraOperator> OperatorsToExecuteMap4 = new List<AlgebraOperator>() { SelectOp4, SortOpMap4 };
            List<AlgebraOperator> OperatorsToExecuteMap5 = new List<AlgebraOperator>() { SelectOp5, SortOpMap5 };

            FromArgument StartArgMap1 = new FromArgument( Category, DataMap.ERMongoMapping );
            FromArgument StartArgMap4 = new FromArgument( Category, DataMapStoreDuplicated.ERMongoMapping );
            FromArgument StartArgMap5 = new FromArgument( Category, DataMapUserDuplicated.ERMongoMapping );

            QueryGenerator GeneratorMap1 = new QueryGenerator( StartArgMap1, OperatorsToExecuteMap1 );
            QueryGenerator GeneratorMap4 = new QueryGenerator( StartArgMap4, OperatorsToExecuteMap4 );
            QueryGenerator GeneratorMap5 = new QueryGenerator( StartArgMap5, OperatorsToExecuteMap5 );

            string QueryMap1 = GeneratorMap1.Run();
            string QueryMap4 = GeneratorMap4.Run();
            string QueryMap5 = GeneratorMap5.Run();

            Assert.IsNotNull( QueryMap1, "Query [Map1] cannot be null" );
            Assert.IsNotNull( QueryMap4, "Query [Map4] cannot be null" );
            Assert.IsNotNull( QueryMap5, "Query [Map5] cannot be null" );

            QueryRunner RunnerMap1 = new QueryRunner( "mongodb://localhost:27017", "pesquisa_cms" );
            QueryRunner RunnerMap4 = new QueryRunner( "mongodb://localhost:27017", "pesquisa_cms_store_duplicado" );
            QueryRunner RunnerMap5 = new QueryRunner( "mongodb://localhost:27017", "pesquisa_cms_user_duplicado" );

            string ResultMap1 = RunnerMap1.GetJSON( QueryMap1 );
            string ResultMap4 = RunnerMap4.GetJSON( QueryMap4 );
            string ResultMap5 = RunnerMap5.GetJSON( QueryMap5 );

            Assert.IsNotNull( ResultMap1, "Result [Map1] cannot be null" );
            Assert.IsNotNull( ResultMap4, "Result [Map4] cannot be null" );
            Assert.IsNotNull( ResultMap5, "Result [Map5] cannot be null" );

            Assert.IsTrue( ResultMap1 != string.Empty, "Result [Map1] cannot be empty" );
            Assert.IsTrue( ResultMap4 != string.Empty, "Result [Map4] cannot be empty" );
            Assert.IsTrue( ResultMap5 != string.Empty, "Result [Map5] cannot be empty" );

            JToken TokenResult1 = JToken.Parse( ResultMap1 );

            TokenResult1.Should().BeEquivalentTo( JToken.Parse( ResultMap4 ) );
            TokenResult1.Should().BeEquivalentTo( JToken.Parse( ResultMap5 ) );
        }
    }
}