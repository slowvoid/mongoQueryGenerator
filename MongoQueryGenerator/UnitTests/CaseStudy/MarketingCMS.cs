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
        /// <summary>
        /// Get All Stores test
        /// 
        /// Query: FROM Store SELECT *
        /// </summary>
        [TestMethod]
        public void GetAllStores()
        {
            RequiredDataContainer DataMap = MarketingCMSDataProvider.MapEntitiesToCollections();
            RequiredDataContainer DataMap2 = MarketingCMSDataProvider.MapEntitiesToCollectionDuplicates();
            RequiredDataContainer DataMap3 = MarketingCMSDataProvider.MapEntitiesToCollectionCategoryDuplicated();
            RequiredDataContainer DataMap4 = MarketingCMSDataProvider.MapEntitiesToCollectionsStoreDuplicated();
            RequiredDataContainer DataMap5 = MarketingCMSDataProvider.MapEntitiesToCollectionsUserDuplicated();

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
            RequiredDataContainer DataMap = MarketingCMSDataProvider.MapEntitiesToCollections();
            RequiredDataContainer DataMapDuplicates = MarketingCMSDataProvider.MapEntitiesToCollectionDuplicates();
            RequiredDataContainer DataMapCategoryDuplicated = MarketingCMSDataProvider.MapEntitiesToCollectionCategoryDuplicated();
            RequiredDataContainer DataMapStoreDuplicated = MarketingCMSDataProvider.MapEntitiesToCollectionsStoreDuplicated();
            RequiredDataContainer DataMapUserDuplicated = MarketingCMSDataProvider.MapEntitiesToCollectionsUserDuplicated();

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
            RequiredDataContainer DataMap = MarketingCMSDataProvider.MapEntitiesToCollections();
            RequiredDataContainer DataMapDuplicates = MarketingCMSDataProvider.MapEntitiesToCollectionDuplicates();
            RequiredDataContainer DataMapCategoryDuplicated = MarketingCMSDataProvider.MapEntitiesToCollectionCategoryDuplicated();
            RequiredDataContainer DataMapStoreDuplicated = MarketingCMSDataProvider.MapEntitiesToCollectionsStoreDuplicated();
            RequiredDataContainer DataMapUserDuplicated = MarketingCMSDataProvider.MapEntitiesToCollectionsUserDuplicated();

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
        /// QUERY: FROM Store 
        ///        RJOIN (Product, StoreHasProduct)
        ///        SELECT *
        /// </summary>
        [TestMethod]
        public void GetAllProductsFromStore()
        {
            RequiredDataContainer DataMap = MarketingCMSDataProvider.MapEntitiesToCollections();
            RequiredDataContainer DataMapDuplicates = MarketingCMSDataProvider.MapEntitiesToCollectionDuplicates();
            RequiredDataContainer DataMapCategoryDuplicated = MarketingCMSDataProvider.MapEntitiesToCollectionCategoryDuplicated();
            RequiredDataContainer DataMapStoreDuplicated = MarketingCMSDataProvider.MapEntitiesToCollectionsStoreDuplicated();
            RequiredDataContainer DataMapUserDuplicated = MarketingCMSDataProvider.MapEntitiesToCollectionsUserDuplicated();

            QueryableEntity Store = new QueryableEntity( DataMap.EntityRelationshipModel.FindByName( "Store" ) );
            QueryableEntity Product = new QueryableEntity( DataMap.EntityRelationshipModel.FindByName( "Product" ) );

            RelationshipJoinOperator RJoinOp1 = new RelationshipJoinOperator( Store, (Relationship)DataMap.EntityRelationshipModel.FindByName( "StoreProducts" ),
                new List<QueryableEntity>() { Product }, DataMap.ERMongoMapping );

            RelationshipJoinOperator RJoinOp2 = new RelationshipJoinOperator( Store, (Relationship)DataMap.EntityRelationshipModel.FindByName( "StoreProducts" ),
                new List<QueryableEntity>() { Product }, DataMapDuplicates.ERMongoMapping );

            RelationshipJoinOperator RJoinOp3 = new RelationshipJoinOperator( Store, (Relationship)DataMap.EntityRelationshipModel.FindByName( "StoreProducts" ),
                new List<QueryableEntity>() { Product }, DataMapCategoryDuplicated.ERMongoMapping );

            RelationshipJoinOperator RJoinOp4 = new RelationshipJoinOperator( Store, (Relationship)DataMap.EntityRelationshipModel.FindByName( "StoreProducts" ),
                new List<QueryableEntity>() { Product }, DataMapStoreDuplicated.ERMongoMapping );

            RelationshipJoinOperator RJoinOp5 = new RelationshipJoinOperator( Store, (Relationship)DataMap.EntityRelationshipModel.FindByName( "StoreProducts" ),
                new List<QueryableEntity>() { Product }, DataMapUserDuplicated.ERMongoMapping );

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
        /// QUERY: FROM Category 
        ///        RJOIN (Product, StoreHasProduct)
        ///        SELECT *
        /// </summary>
        [TestMethod]
        public void GetAllProductsFromCategory()
        {
            RequiredDataContainer DataMap = MarketingCMSDataProvider.MapEntitiesToCollections();
            RequiredDataContainer DataMapDuplicates = MarketingCMSDataProvider.MapEntitiesToCollectionDuplicates();
            RequiredDataContainer DataMapCategoryDuplicated = MarketingCMSDataProvider.MapEntitiesToCollectionCategoryDuplicated();
            RequiredDataContainer DataMapStoreDuplicated = MarketingCMSDataProvider.MapEntitiesToCollectionsStoreDuplicated();
            RequiredDataContainer DataMapUserDuplicated = MarketingCMSDataProvider.MapEntitiesToCollectionsUserDuplicated();

            QueryableEntity Category = new QueryableEntity( DataMap.EntityRelationshipModel.FindByName( "Category" ) );
            QueryableEntity Product = new QueryableEntity( DataMap.EntityRelationshipModel.FindByName( "Product" ) );

            RelationshipJoinOperator RJoinOp1 = new RelationshipJoinOperator( Category, (Relationship)DataMap.EntityRelationshipModel.FindByName( "CategoryProducts" ),
                new List<QueryableEntity>() { Product }, DataMap.ERMongoMapping );

            RelationshipJoinOperator RJoinOp2 = new RelationshipJoinOperator( Category, (Relationship)DataMap.EntityRelationshipModel.FindByName( "CategoryProducts" ),
                new List<QueryableEntity>() { Product }, DataMapDuplicates.ERMongoMapping );

            RelationshipJoinOperator RJoinOp3 = new RelationshipJoinOperator( Category, (Relationship)DataMap.EntityRelationshipModel.FindByName( "CategoryProducts" ),
                new List<QueryableEntity>() { Product }, DataMapCategoryDuplicated.ERMongoMapping );

            RelationshipJoinOperator RJoinOp4 = new RelationshipJoinOperator( Category, (Relationship)DataMap.EntityRelationshipModel.FindByName( "CategoryProducts" ),
                new List<QueryableEntity>() { Product }, DataMapStoreDuplicated.ERMongoMapping );

            RelationshipJoinOperator RJoinOp5 = new RelationshipJoinOperator( Category, (Relationship)DataMap.EntityRelationshipModel.FindByName( "CategoryProducts" ),
                new List<QueryableEntity>() { Product }, DataMapUserDuplicated.ERMongoMapping );

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
        /// QUERY: FROM Category 
        ///        RJOIN (Product, StoreHasProduct)
        ///        SELECT *
        /// </summary>
        [TestMethod]
        public void GetAllProductsFromUser()
        {
            RequiredDataContainer DataMap = MarketingCMSDataProvider.MapEntitiesToCollections();
            RequiredDataContainer DataMapDuplicates = MarketingCMSDataProvider.MapEntitiesToCollectionDuplicates();
            RequiredDataContainer DataMapCategoryDuplicated = MarketingCMSDataProvider.MapEntitiesToCollectionCategoryDuplicated();
            RequiredDataContainer DataMapStoreDuplicated = MarketingCMSDataProvider.MapEntitiesToCollectionsStoreDuplicated();
            RequiredDataContainer DataMapUserDuplicated = MarketingCMSDataProvider.MapEntitiesToCollectionsUserDuplicated();

            QueryableEntity User = new QueryableEntity( DataMap.EntityRelationshipModel.FindByName( "User" ) );
            QueryableEntity Product = new QueryableEntity( DataMap.EntityRelationshipModel.FindByName( "Product" ) );

            RelationshipJoinOperator RJoinOp1 = new RelationshipJoinOperator( User, (Relationship)DataMap.EntityRelationshipModel.FindByName( "UserProducts" ),
                new List<QueryableEntity>() { Product }, DataMap.ERMongoMapping );

            RelationshipJoinOperator RJoinOp2 = new RelationshipJoinOperator( User, (Relationship)DataMap.EntityRelationshipModel.FindByName( "UserProducts" ),
                new List<QueryableEntity>() { Product }, DataMapDuplicates.ERMongoMapping );

            RelationshipJoinOperator RJoinOp3 = new RelationshipJoinOperator( User, (Relationship)DataMap.EntityRelationshipModel.FindByName( "UserProducts" ),
                new List<QueryableEntity>() { Product }, DataMapCategoryDuplicated.ERMongoMapping );

            RelationshipJoinOperator RJoinOp4 = new RelationshipJoinOperator( User, (Relationship)DataMap.EntityRelationshipModel.FindByName( "UserProducts" ),
                new List<QueryableEntity>() { Product }, DataMapStoreDuplicated.ERMongoMapping );

            RelationshipJoinOperator RJoinOp5 = new RelationshipJoinOperator( User, (Relationship)DataMap.EntityRelationshipModel.FindByName( "UserProducts" ),
                new List<QueryableEntity>() { Product }, DataMapUserDuplicated.ERMongoMapping );

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
        /// QUERY: FROM Category 
        ///        RJOIN (Product RJOIN [(Store, StoreProduct)], CategoryProduct) 
        /// </summary>
        [TestMethod]
        public void GetAllProductsFromCategoryWithStore()
        {
            RequiredDataContainer DataMap = MarketingCMSDataProvider.MapEntitiesToCollections();
            RequiredDataContainer DataMapDuplicates = MarketingCMSDataProvider.MapEntitiesToCollectionDuplicates();
            RequiredDataContainer DataMapCategoryDuplicated = MarketingCMSDataProvider.MapEntitiesToCollectionCategoryDuplicated();
            RequiredDataContainer DataMapStoreDuplicated = MarketingCMSDataProvider.MapEntitiesToCollectionsStoreDuplicated();
            RequiredDataContainer DataMapUserDuplicated = MarketingCMSDataProvider.MapEntitiesToCollectionsUserDuplicated();

            QueryableEntity Category = new QueryableEntity( DataMap.EntityRelationshipModel.FindByName( "Category" ) );
            QueryableEntity Product = new QueryableEntity( DataMap.EntityRelationshipModel.FindByName( "Product" ) );
            QueryableEntity Store = new QueryableEntity( DataMap.EntityRelationshipModel.FindByName( "Store" ) );

            ComputedEntity ProductWithStore = new ComputedEntity( "ProductWithStore", 
                Product,
                (Relationship)DataMap.EntityRelationshipModel.FindByName( "StoreProducts" ),
                new List<QueryableEntity>() { Store } );
           
            RelationshipJoinOperator RJoinOp1 = new RelationshipJoinOperator( Category, (Relationship)DataMap.EntityRelationshipModel.FindByName( "CategoryProducts" ),
                new List<QueryableEntity>() { new QueryableEntity( ProductWithStore ) },
                DataMap.ERMongoMapping );

            RelationshipJoinOperator RJoinOp2 = new RelationshipJoinOperator( Category, (Relationship)DataMap.EntityRelationshipModel.FindByName( "CategoryProducts" ),
                new List<QueryableEntity>() { new QueryableEntity( ProductWithStore ) },
                DataMapDuplicates.ERMongoMapping );

            RelationshipJoinOperator RJoinOp3 = new RelationshipJoinOperator( Category, (Relationship)DataMap.EntityRelationshipModel.FindByName( "CategoryProducts" ),
                new List<QueryableEntity>() { new QueryableEntity( ProductWithStore ) },
                DataMapCategoryDuplicated.ERMongoMapping );

            RelationshipJoinOperator RJoinOp4 = new RelationshipJoinOperator( Category, (Relationship)DataMap.EntityRelationshipModel.FindByName( "CategoryProducts" ),
                new List<QueryableEntity>() { new QueryableEntity( ProductWithStore ) },
                DataMapStoreDuplicated.ERMongoMapping );

            RelationshipJoinOperator RJoinOp5 = new RelationshipJoinOperator( Category, (Relationship)DataMap.EntityRelationshipModel.FindByName( "CategoryProducts" ),
                new List<QueryableEntity>() { new QueryableEntity( ProductWithStore ) },
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
        /// QUERY: FROM Category 
        ///        RJOIN (Product RJOIN [(User, UserProduct)], CategoryProduct) 
        /// </summary>
        [TestMethod]
        public void GetAllProductsFromCategoryWithUser()
        {
            RequiredDataContainer DataMap = MarketingCMSDataProvider.MapEntitiesToCollections();
            RequiredDataContainer DataMapDuplicates = MarketingCMSDataProvider.MapEntitiesToCollectionDuplicates();
            RequiredDataContainer DataMapCategoryDuplicated = MarketingCMSDataProvider.MapEntitiesToCollectionCategoryDuplicated();
            RequiredDataContainer DataMapStoreDuplicated = MarketingCMSDataProvider.MapEntitiesToCollectionsStoreDuplicated();
            RequiredDataContainer DataMapUserDuplicated = MarketingCMSDataProvider.MapEntitiesToCollectionsUserDuplicated();

            QueryableEntity Category = new QueryableEntity( DataMap.EntityRelationshipModel.FindByName( "Category" ) );
            QueryableEntity Product = new QueryableEntity( DataMap.EntityRelationshipModel.FindByName( "Product" ) );
            QueryableEntity User = new QueryableEntity( DataMap.EntityRelationshipModel.FindByName( "User" ) );

            ComputedEntity ProductWithUser = new ComputedEntity( "ProductWithStore",
                Product,
                (Relationship)DataMap.EntityRelationshipModel.FindByName( "UserProducts" ),
                new List<QueryableEntity>() { User } );

            RelationshipJoinOperator RJoinOp1 = new RelationshipJoinOperator( Category, (Relationship)DataMap.EntityRelationshipModel.FindByName( "CategoryProducts" ),
                new List<QueryableEntity>() { new QueryableEntity( ProductWithUser ) },
                DataMap.ERMongoMapping );

            RelationshipJoinOperator RJoinOp2 = new RelationshipJoinOperator( Category, (Relationship)DataMap.EntityRelationshipModel.FindByName( "CategoryProducts" ),
                new List<QueryableEntity>() { new QueryableEntity( ProductWithUser ) },
                DataMapDuplicates.ERMongoMapping );

            RelationshipJoinOperator RJoinOp3 = new RelationshipJoinOperator( Category, (Relationship)DataMap.EntityRelationshipModel.FindByName( "CategoryProducts" ),
                new List<QueryableEntity>() { new QueryableEntity( ProductWithUser ) },
                DataMapCategoryDuplicated.ERMongoMapping );

            RelationshipJoinOperator RJoinOp4 = new RelationshipJoinOperator( Category, (Relationship)DataMap.EntityRelationshipModel.FindByName( "CategoryProducts" ),
                new List<QueryableEntity>() { new QueryableEntity( ProductWithUser ) },
                DataMapStoreDuplicated.ERMongoMapping );

            RelationshipJoinOperator RJoinOp5 = new RelationshipJoinOperator( Category, (Relationship)DataMap.EntityRelationshipModel.FindByName( "CategoryProducts" ),
                new List<QueryableEntity>() { new QueryableEntity( ProductWithUser ) },
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
        /// QUERY: FROM Product
        ///        RJOIN (User, UserProducts)
        ///        SELECT Product.Title, User.UserName
        /// </summary>
        [TestMethod]
        public void GetProductTitleAndUserName()
        {
            RequiredDataContainer DataMap = MarketingCMSDataProvider.MapEntitiesToCollections();
            RequiredDataContainer DataMapDuplicates = MarketingCMSDataProvider.MapEntitiesToCollectionDuplicates();
            RequiredDataContainer DataMapCategoryDuplicated = MarketingCMSDataProvider.MapEntitiesToCollectionCategoryDuplicated();
            RequiredDataContainer DataMapStoreDuplicated = MarketingCMSDataProvider.MapEntitiesToCollectionsStoreDuplicated();
            RequiredDataContainer DataMapUserDuplicated = MarketingCMSDataProvider.MapEntitiesToCollectionsUserDuplicated();

            QueryableEntity Product = new QueryableEntity( DataMap.EntityRelationshipModel.FindByName( "Product" ) );
            QueryableEntity User = new QueryableEntity( DataMap.EntityRelationshipModel.FindByName( "User" ) );

            List<ProjectArgument> ProjectArgs = new List<ProjectArgument>();
            ProjectArgs.Add( new ProjectArgument( Product.GetAttribute( "Title" ), Product, new BooleanExpr( true ) ) );
            ProjectArgs.Add( new ProjectArgument( User.GetAttribute( "UserName" ), User, new BooleanExpr( true ) ) );

            RelationshipJoinOperator RJoinOp1 = new RelationshipJoinOperator( Product, (Relationship)DataMap.EntityRelationshipModel.FindByName( "UserProducts" ),
                new List<QueryableEntity>() { User },
                DataMap.ERMongoMapping );

            ProjectStage ProjectOp1 = new ProjectStage( ProjectArgs, RJoinOp1.ComputeVirtualMap() );

            RelationshipJoinOperator RJoinOp2 = new RelationshipJoinOperator( Product, (Relationship)DataMap.EntityRelationshipModel.FindByName( "CategoryProducts" ),
                new List<QueryableEntity>() { User },
                DataMapDuplicates.ERMongoMapping );

            ProjectStage ProjectOp2 = new ProjectStage( ProjectArgs, RJoinOp2.ComputeVirtualMap() );

            RelationshipJoinOperator RJoinOp3 = new RelationshipJoinOperator( Product, (Relationship)DataMap.EntityRelationshipModel.FindByName( "CategoryProducts" ),
                new List<QueryableEntity>() { User },
                DataMapCategoryDuplicated.ERMongoMapping );

            ProjectStage ProjectOp3 = new ProjectStage( ProjectArgs, RJoinOp3.ComputeVirtualMap() );

            RelationshipJoinOperator RJoinOp4 = new RelationshipJoinOperator( Product, (Relationship)DataMap.EntityRelationshipModel.FindByName( "CategoryProducts" ),
                new List<QueryableEntity>() { User },
                DataMapStoreDuplicated.ERMongoMapping );

            ProjectStage ProjectOp4 = new ProjectStage( ProjectArgs, RJoinOp4.ComputeVirtualMap() );

            RelationshipJoinOperator RJoinOp5 = new RelationshipJoinOperator( Product, (Relationship)DataMap.EntityRelationshipModel.FindByName( "CategoryProducts" ),
                new List<QueryableEntity>() { User },
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
            FromArgument StartArgMap2 = new FromArgument( Product, DataMapDuplicates.ERMongoMapping );
            FromArgument StartArgMap3 = new FromArgument( Product, DataMapCategoryDuplicated.ERMongoMapping );
            FromArgument StartArgMap4 = new FromArgument( Product, DataMapStoreDuplicated.ERMongoMapping );
            FromArgument StartArgMap5 = new FromArgument( Product, DataMapUserDuplicated.ERMongoMapping );

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
        /// QUERY: FROM Category 
        ///        RJOIN (Product RJOIN [(User, UserProduct)], CategoryProduct)
        ///        SELECT Category.CategoryName, Product.Title, User.UserName, User.UserEmail
        /// </summary>
        [TestMethod]
        public void GetAllProductsFromCategoryWithUserAndSelectOnlyTitleNameEmailCategoryName()
        {
            RequiredDataContainer DataMap = MarketingCMSDataProvider.MapEntitiesToCollections();
            RequiredDataContainer DataMapDuplicates = MarketingCMSDataProvider.MapEntitiesToCollectionDuplicates();
            RequiredDataContainer DataMapCategoryDuplicated = MarketingCMSDataProvider.MapEntitiesToCollectionCategoryDuplicated();
            RequiredDataContainer DataMapStoreDuplicated = MarketingCMSDataProvider.MapEntitiesToCollectionsStoreDuplicated();
            RequiredDataContainer DataMapUserDuplicated = MarketingCMSDataProvider.MapEntitiesToCollectionsUserDuplicated();

            QueryableEntity Category = new QueryableEntity( DataMap.EntityRelationshipModel.FindByName( "Category" ) );
            QueryableEntity Product = new QueryableEntity( DataMap.EntityRelationshipModel.FindByName( "Product" ) );
            QueryableEntity User = new QueryableEntity( DataMap.EntityRelationshipModel.FindByName( "User" ) );

            List<ProjectArgument> ProjectArgs = new List<ProjectArgument>();
            ProjectArgs.Add( new ProjectArgument( Product.GetAttribute( "Title" ), Product, new BooleanExpr( true ) ) );
            ProjectArgs.Add( new ProjectArgument( User.GetAttribute( "UserName" ), User, new BooleanExpr( true ) ) );
            ProjectArgs.Add( new ProjectArgument( User.GetAttribute( "UserEmail" ), User, new BooleanExpr( true ) ) );
            ProjectArgs.Add( new ProjectArgument( Category.GetAttribute( "CategoryName" ), Category, new BooleanExpr( true ) ) );

            ComputedEntity ProductWithUser = new ComputedEntity( "ProductWithStore",
                Product,
                (Relationship)DataMap.EntityRelationshipModel.FindByName( "UserProducts" ),
                new List<QueryableEntity>() { User } );

            RelationshipJoinOperator RJoinOp1 = new RelationshipJoinOperator( Category, (Relationship)DataMap.EntityRelationshipModel.FindByName( "CategoryProducts" ),
                new List<QueryableEntity>() { new QueryableEntity( ProductWithUser ) },
                DataMap.ERMongoMapping );

            ProjectStage ProjectOp1 = new ProjectStage( ProjectArgs, RJoinOp1.ComputeVirtualMap() );

            RelationshipJoinOperator RJoinOp2 = new RelationshipJoinOperator( Category, (Relationship)DataMap.EntityRelationshipModel.FindByName( "CategoryProducts" ),
                new List<QueryableEntity>() { new QueryableEntity( ProductWithUser ) },
                DataMapDuplicates.ERMongoMapping );

            ProjectStage ProjectOp2 = new ProjectStage( ProjectArgs, RJoinOp2.ComputeVirtualMap() );

            RelationshipJoinOperator RJoinOp3 = new RelationshipJoinOperator( Category, (Relationship)DataMap.EntityRelationshipModel.FindByName( "CategoryProducts" ),
                new List<QueryableEntity>() { new QueryableEntity( ProductWithUser ) },
                DataMapCategoryDuplicated.ERMongoMapping );

            ProjectStage ProjectOp3 = new ProjectStage( ProjectArgs, RJoinOp3.ComputeVirtualMap() );

            RelationshipJoinOperator RJoinOp4 = new RelationshipJoinOperator( Category, (Relationship)DataMap.EntityRelationshipModel.FindByName( "CategoryProducts" ),
                new List<QueryableEntity>() { new QueryableEntity( ProductWithUser ) },
                DataMapStoreDuplicated.ERMongoMapping );

            ProjectStage ProjectOp4 = new ProjectStage( ProjectArgs, RJoinOp4.ComputeVirtualMap() );

            RelationshipJoinOperator RJoinOp5 = new RelationshipJoinOperator( Category, (Relationship)DataMap.EntityRelationshipModel.FindByName( "CategoryProducts" ),
                new List<QueryableEntity>() { new QueryableEntity( ProductWithUser ) },
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
        /// Execute GetCategoryThatIsNamedHome
        /// 
        /// Query: FROM Category
        ///        SELECT *
        ///        WHERE CategoryName = 'Home'
        /// </summary>
        [TestMethod]
        public void GetCategoryThatIsNamedHome()
        {
            RequiredDataContainer DataMap = MarketingCMSDataProvider.MapEntitiesToCollections();
            RequiredDataContainer DataMapDuplicates = MarketingCMSDataProvider.MapEntitiesToCollectionDuplicates();
            RequiredDataContainer DataMapCategoryDuplicated = MarketingCMSDataProvider.MapEntitiesToCollectionCategoryDuplicated();
            RequiredDataContainer DataMapStoreDuplicated = MarketingCMSDataProvider.MapEntitiesToCollectionsStoreDuplicated();
            RequiredDataContainer DataMapUserDuplicated = MarketingCMSDataProvider.MapEntitiesToCollectionsUserDuplicated();

            QueryableEntity Category = new QueryableEntity( DataMap.EntityRelationshipModel.FindByName( "Category" ) );

            MapRule CategoryRule01 = DataMap.ERMongoMapping.FindMainRule( Category.Element );
            SelectArgument SelectArg01 = new SelectArgument( new EqExpr( $"${CategoryRule01.GetRuleValueForAttribute( Category.GetAttribute( "CategoryName" ) )}", "Home" ) );
            SelectStage SelectOp1 = new SelectStage( SelectArg01, DataMap.ERMongoMapping );

            MapRule CategoryRule04 = DataMapStoreDuplicated.ERMongoMapping.FindMainRule( Category.Element );
            SelectArgument SelectArg04 = new SelectArgument( new EqExpr( $"${CategoryRule04.GetRuleValueForAttribute( Category.GetAttribute( "CategoryName" ) )}", "Home" ) );
            SelectStage SelectOp4 = new SelectStage( SelectArg04, DataMap.ERMongoMapping );

            MapRule CategoryRule05 = DataMapUserDuplicated.ERMongoMapping.FindMainRule( Category.Element );
            SelectArgument SelectArg05 = new SelectArgument( new EqExpr( $"${CategoryRule05.GetRuleValueForAttribute( Category.GetAttribute( "CategoryName" ) )}", "Home" ) );
            SelectStage SelectOp5 = new SelectStage( SelectArg05, DataMap.ERMongoMapping );

            SortArgument SortArg = new SortArgument( Category, Category.GetAttribute( "CategoryID" ), MongoDBSort.Ascending );

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