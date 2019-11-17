using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using QueryBuilder.ER;
using QueryBuilder.Operation;
using QueryBuilder.Operation.Arguments;
using QueryBuilder.Query;
using System.Collections.Generic;
using QueryBuilder.Mongo.Aggregation.Operators;

namespace QueryBuilder.Tests
{
    [TestClass]
    public class MarketingCMSTests
    {
        /// <summary>
        /// Run tests based on the query
        /// SQL: SELECT * FROM Product 
        ///      JOIN Store ON Product.product_store_id = Store.store_id
        ///      JOIN Category ON Product.product_category_id = Category_category_id
        ///      JOIN User ON Product.product_author_id = User.user_id
        ///      
        /// QUERY: FROM Product
        ///        RJOIN (Store, ProductoBelongsToStore),
        ///              (Category, ProductBelongsToCategory),
        ///              (User, ProductBelongsToUser)
        ///        SELECT *
        /// </summary>
        [TestMethod]
        public void GetAllProducts()
        {
            RequiredDataContainer DataMap = MarketingCMSDataProvider.MapEntitiesToCollections();
            RequiredDataContainer DataMapDuplicates = MarketingCMSDataProvider.MapEntitiesToCollectionDuplicates();
            RequiredDataContainer DataMapCategoryDuplicated = MarketingCMSDataProvider.MapEntitiesToCollectionCategoryDuplicated();
            RequiredDataContainer DataMapStoreDuplicated = MarketingCMSDataProvider.MapEntitiesToCollectionsStoreDuplicated();
            RequiredDataContainer DataMapUserDuplicated = MarketingCMSDataProvider.MapEntitiesToCollectionsUserDuplicated();

            QueryableEntity Product = new QueryableEntity( DataMap.EntityRelationshipModel.FindByName( "Product" ) );
            QueryableEntity Store = new QueryableEntity( DataMap.EntityRelationshipModel.FindByName( "Store" ) );
            QueryableEntity Category = new QueryableEntity( DataMap.EntityRelationshipModel.FindByName( "Category" ) );
            QueryableEntity User = new QueryableEntity( DataMap.EntityRelationshipModel.FindByName( "User" ) );

            RelationshipJoinArgument JoinStoreArgs = new RelationshipJoinArgument(
                (Relationship)DataMap.EntityRelationshipModel.FindByName( "ProductBelongsToStore" ),
                new List<QueryableEntity>() { Store } );

            RelationshipJoinArgument JoinCategoryArgs = new RelationshipJoinArgument(
                (Relationship)DataMap.EntityRelationshipModel.FindByName( "ProductBelongsToCategory" ),
                new List<QueryableEntity>() { Category } );

            RelationshipJoinArgument JoinUserArgs = new RelationshipJoinArgument(
                (Relationship)DataMap.EntityRelationshipModel.FindByName( "ProductBelongsToUser" ),
                new List<QueryableEntity>() { User } );

            RelationshipJoinOperator RJoinMap1Op = new RelationshipJoinOperator( Product,
                new List<RelationshipJoinArgument>() { JoinStoreArgs, JoinCategoryArgs, JoinUserArgs },
                DataMap.ERMongoMapping );

            // DEBUG
            //LimitStage LimitOp = new LimitStage( 150000 );
            SortArgument SortArg = new SortArgument( Product, Product.GetAttribute( "product_id" ), MongoDBSort.Ascending );
            SortStage SortOp = new SortStage( new List<SortArgument>() { SortArg }, DataMap.ERMongoMapping );
            SortStage SortOp2 = new SortStage( new List<SortArgument>() { SortArg }, DataMapDuplicates.ERMongoMapping );
            SortStage SortOp3 = new SortStage( new List<SortArgument>() { SortArg }, DataMapCategoryDuplicated.ERMongoMapping );
            SortStage SortOp4 = new SortStage( new List<SortArgument>() { SortArg }, DataMapStoreDuplicated.ERMongoMapping );
            SortStage SortOp5 = new SortStage( new List<SortArgument>() { SortArg }, DataMapUserDuplicated.ERMongoMapping );
            // =====

            // Build and execute Map1 query
            List<AlgebraOperator> Map1OperatorsList = new List<AlgebraOperator>() { RJoinMap1Op, SortOp };

            FromArgument StartArgMap1 = new FromArgument( Product, DataMap.ERMongoMapping );
            QueryGenerator Generator = new QueryGenerator( StartArgMap1, Map1OperatorsList );

            string Map1Query = Generator.Run();

            Assert.IsNotNull( Map1Query, "Generated query [Map1Query] cannot be null" );

            RelationshipJoinOperator RJoinMapDuplicatesOp = new RelationshipJoinOperator( Product,
                new List<RelationshipJoinArgument>() { JoinStoreArgs, JoinCategoryArgs, JoinUserArgs },
                DataMapDuplicates.ERMongoMapping );

            List<AlgebraOperator> MapDuplicatesOpList = new List<AlgebraOperator>() { RJoinMapDuplicatesOp, SortOp2 };

            FromArgument StartArgMap2 = new FromArgument( Product, DataMapDuplicates.ERMongoMapping );
            QueryGenerator GeneratorDuplicates = new QueryGenerator( StartArgMap2, MapDuplicatesOpList );

            string MapDuplicatesQuery = GeneratorDuplicates.Run();

            Assert.IsNotNull( MapDuplicatesQuery, "Generated query [MapDuplicatesQuery] cannot be null" );

            RelationshipJoinOperator RJoinMapCategoryDuplicatedOp = new RelationshipJoinOperator( Product,
                new List<RelationshipJoinArgument>() { JoinStoreArgs, JoinCategoryArgs, JoinUserArgs },
                DataMapCategoryDuplicated.ERMongoMapping );

            List<AlgebraOperator> MapCategoryDuplicatedOpList = new List<AlgebraOperator>() { RJoinMapCategoryDuplicatedOp, SortOp3 };

            FromArgument StartArgCategoryDuplicated = new FromArgument( Product, DataMapCategoryDuplicated.ERMongoMapping );
            QueryGenerator GeneratorCategoryDuplicated = new QueryGenerator( StartArgCategoryDuplicated, MapCategoryDuplicatedOpList );

            string MapCategoryDuplicatedQuery = GeneratorCategoryDuplicated.Run();

            Assert.IsNotNull( MapCategoryDuplicatedQuery, "Generated query [MapCategoryDuplicatedQuery] cannot be null" );

            RelationshipJoinOperator RJoinMapStoreDuplicatedOp = new RelationshipJoinOperator( Product,
                new List<RelationshipJoinArgument>() { JoinStoreArgs, JoinCategoryArgs, JoinUserArgs },
                DataMapStoreDuplicated.ERMongoMapping );

            List<AlgebraOperator> MapStoreDuplicatedOpList = new List<AlgebraOperator>() { RJoinMapStoreDuplicatedOp, SortOp4 };

            FromArgument StartArgStoreDuplicated = new FromArgument( Product, DataMapStoreDuplicated.ERMongoMapping );
            QueryGenerator GeneratorStoreDuplicated = new QueryGenerator( StartArgStoreDuplicated, MapStoreDuplicatedOpList );

            string MapStoreDuplicatedQuery = GeneratorStoreDuplicated.Run();

            Assert.IsNotNull( MapStoreDuplicatedQuery, "Generated query [MapStoreDuplicatedQuery] cannot be null" );

            RelationshipJoinOperator RJoinMapUserDuplicatedOp = new RelationshipJoinOperator( Product,
                new List<RelationshipJoinArgument>() { JoinUserArgs, JoinCategoryArgs, JoinStoreArgs },
                DataMapUserDuplicated.ERMongoMapping );

            List<AlgebraOperator> MapUserDuplicatedOpList = new List<AlgebraOperator>() { RJoinMapUserDuplicatedOp, SortOp5 };

            FromArgument StartArgUserDuplicated = new FromArgument( Product, DataMapUserDuplicated.ERMongoMapping );
            QueryGenerator GeneratorUserDuplicated = new QueryGenerator( StartArgUserDuplicated, MapUserDuplicatedOpList );

            string MapUserDuplicatedQuery = GeneratorUserDuplicated.Run();

            Assert.IsNotNull( MapUserDuplicatedQuery, "Generated query [MapUserDuplicatedQuery] cannot be null" );

            QueryRunner RunnerMap1 = new QueryRunner( "mongodb://localhost:27017", "pesquisa_cms" );
            QueryRunner RunnerMap2 = new QueryRunner( "mongodb://localhost:27017", "pesquisa_cms_duplicados" );
            QueryRunner RunnerMap3 = new QueryRunner( "mongodb://localhost:27017", "pesquisa_cms_category_duplicado" );
            QueryRunner RunnerMap4 = new QueryRunner( "mongodb://localhost:27017", "pesquisa_cms_store_duplicado" );
            QueryRunner RunnerMap5 = new QueryRunner( "mongodb://localhost:27017", "pesquisa_cms_user_duplicado" );

            string ResultMap1 = RunnerMap1.GetJSON( Map1Query );
            string ResultMap2 = RunnerMap2.GetJSON( MapDuplicatesQuery );
            string ResultMap3 = RunnerMap3.GetJSON( MapCategoryDuplicatedQuery );
            string ResultMap4 = RunnerMap4.GetJSON( MapStoreDuplicatedQuery );
            string ResultMap5 = RunnerMap5.GetJSON( MapUserDuplicatedQuery );

            Assert.IsNotNull( ResultMap1, "Result [Map1] cannot be null" );
            Assert.IsNotNull( ResultMap2, "Result [Map2] cannot be null" );
            Assert.IsNotNull( ResultMap3, "Result [Map3] cannot be null" );
            Assert.IsNotNull( ResultMap4, "Result [Map4] cannot be null" );
            Assert.IsNotNull( ResultMap5, "Result [Map5] cannot be null" );

            Assert.IsTrue( JToken.DeepEquals( JToken.Parse( ResultMap1 ), JToken.Parse( ResultMap2 ) ) );
            Assert.IsTrue( JToken.DeepEquals( JToken.Parse( ResultMap1 ), JToken.Parse( ResultMap3 ) ) );
            Assert.IsTrue( JToken.DeepEquals( JToken.Parse( ResultMap1 ), JToken.Parse( ResultMap4 ) ) );
            Assert.IsTrue( JToken.DeepEquals( JToken.Parse( ResultMap1 ), JToken.Parse( ResultMap5 ) ) );
        }
        /// <summary>
        /// Run GetAllStores query
        /// 
        /// SQL: SELECT * FROM Store;
        /// 
        /// QUERY: FROM Store SELECT *
        /// </summary>
        [TestMethod]
        public void GetAllStores()
        {
            RequiredDataContainer DataMap = MarketingCMSDataProvider.MapEntitiesToCollections();
            RequiredDataContainer DataMapDuplicates = MarketingCMSDataProvider.MapEntitiesToCollectionDuplicates();
            RequiredDataContainer DataMapCategoryDuplicated = MarketingCMSDataProvider.MapEntitiesToCollectionCategoryDuplicated();
            RequiredDataContainer DataMapStoreDuplicated = MarketingCMSDataProvider.MapEntitiesToCollectionsStoreDuplicated();
            RequiredDataContainer DataMapUserDuplicated = MarketingCMSDataProvider.MapEntitiesToCollectionsUserDuplicated();

            QueryableEntity Store = new QueryableEntity( DataMap.EntityRelationshipModel.FindByName( "Store" ) );

            SortArgument SortArg = new SortArgument( Store, Store.GetAttribute( "store_id" ), MongoDBSort.Ascending );

            SortStage SortOpMap1 = new SortStage( new List<SortArgument>() { SortArg }, DataMap.ERMongoMapping );
            SortStage SortOpMap2 = new SortStage( new List<SortArgument>() { SortArg }, DataMapDuplicates.ERMongoMapping );
            SortStage SortOpMap3 = new SortStage( new List<SortArgument>() { SortArg }, DataMapCategoryDuplicated.ERMongoMapping );
            SortStage SortOpMap4 = new SortStage( new List<SortArgument>() { SortArg }, DataMapStoreDuplicated.ERMongoMapping );
            SortStage SortOpMap5 = new SortStage( new List<SortArgument>() { SortArg }, DataMapUserDuplicated.ERMongoMapping );

            FromArgument StartArgMap1 = new FromArgument( Store, DataMap.ERMongoMapping );
            FromArgument StartArgMap2 = new FromArgument( Store, DataMapDuplicates.ERMongoMapping );
            FromArgument StartArgMap3 = new FromArgument( Store, DataMapCategoryDuplicated.ERMongoMapping );
            FromArgument StartArgMap4 = new FromArgument( Store, DataMapStoreDuplicated.ERMongoMapping );
            FromArgument StartArgMap5 = new FromArgument( Store, DataMapUserDuplicated.ERMongoMapping );

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
            string QueryStringMap2 = QueryGenMap2.Run();
            string QueryStringMap3 = QueryGenMap3.Run();
            string QueryStringMap4 = QueryGenMap4.Run();
            string QueryStringMap5 = QueryGenMap5.Run();

            QueryRunner RunnerMap1 = new QueryRunner( "mongodb://localhost:27017", "pesquisa_cms" );
            QueryRunner RunnerMap2 = new QueryRunner( "mongodb://localhost:27017", "pesquisa_cms_duplicados" );
            QueryRunner RunnerMap3 = new QueryRunner( "mongodb://localhost:27017", "pesquisa_cms_category_duplicado" );
            QueryRunner RunnerMap4 = new QueryRunner( "mongodb://localhost:27017", "pesquisa_cms_store_duplicado" );
            QueryRunner RunnerMap5 = new QueryRunner( "mongodb://localhost:27017", "pesquisa_cms_user_duplicado" );

            string ResultMap1 = RunnerMap1.GetJSON( QueryStringMap1 );
            string ResultMap2 = RunnerMap2.GetJSON( QueryStringMap2 );
            string ResultMap3 = RunnerMap3.GetJSON( QueryStringMap3 );
            string ResultMap4 = RunnerMap4.GetJSON( QueryStringMap4 );
            string ResultMap5 = RunnerMap5.GetJSON( QueryStringMap5 );

            Assert.IsNotNull( ResultMap1, "Result [Map1] cannot be null" );
            Assert.IsNotNull( ResultMap2, "Result [Map2] cannot be null" );
            Assert.IsNotNull( ResultMap3, "Result [Map3] cannot be null" );
            Assert.IsNotNull( ResultMap4, "Result [Map4] cannot be null" );
            Assert.IsNotNull( ResultMap5, "Result [Map5] cannot be null" );

            Assert.IsTrue( JToken.DeepEquals( JToken.Parse( ResultMap1 ), JToken.Parse( ResultMap2 ) ) );
            Assert.IsTrue( JToken.DeepEquals( JToken.Parse( ResultMap1 ), JToken.Parse( ResultMap3 ) ) );
            Assert.IsTrue( JToken.DeepEquals( JToken.Parse( ResultMap1 ), JToken.Parse( ResultMap4 ) ) );
            Assert.IsTrue( JToken.DeepEquals( JToken.Parse( ResultMap1 ), JToken.Parse( ResultMap5 ) ) );
        }
        /// <summary>
        /// Run GetAllCategories query
        /// 
        /// SQL: SELECT * FROM Category;
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

            SortArgument SortArg = new SortArgument( Category, Category.GetAttribute( "category_id" ), MongoDBSort.Ascending );

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
            string QueryStringMap2 = QueryGenMap2.Run();
            string QueryStringMap3 = QueryGenMap3.Run();
            string QueryStringMap4 = QueryGenMap4.Run();
            string QueryStringMap5 = QueryGenMap5.Run();

            QueryRunner RunnerMap1 = new QueryRunner( "mongodb://localhost:27017", "pesquisa_cms" );
            QueryRunner RunnerMap2 = new QueryRunner( "mongodb://localhost:27017", "pesquisa_cms_duplicados" );
            QueryRunner RunnerMap3 = new QueryRunner( "mongodb://localhost:27017", "pesquisa_cms_category_duplicado" );
            QueryRunner RunnerMap4 = new QueryRunner( "mongodb://localhost:27017", "pesquisa_cms_store_duplicado" );
            QueryRunner RunnerMap5 = new QueryRunner( "mongodb://localhost:27017", "pesquisa_cms_user_duplicado" );

            string ResultMap1 = RunnerMap1.GetJSON( QueryStringMap1 );
            string ResultMap2 = RunnerMap2.GetJSON( QueryStringMap2 );
            string ResultMap3 = RunnerMap3.GetJSON( QueryStringMap3 );
            string ResultMap4 = RunnerMap4.GetJSON( QueryStringMap4 );
            string ResultMap5 = RunnerMap5.GetJSON( QueryStringMap5 );

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

            Assert.IsTrue( JToken.DeepEquals( JToken.Parse( ResultMap1 ), JToken.Parse( ResultMap2 ) ) );
            Assert.IsTrue( JToken.DeepEquals( JToken.Parse( ResultMap1 ), JToken.Parse( ResultMap3 ) ) );
            Assert.IsTrue( JToken.DeepEquals( JToken.Parse( ResultMap1 ), JToken.Parse( ResultMap4 ) ) );
            Assert.IsTrue( JToken.DeepEquals( JToken.Parse( ResultMap1 ), JToken.Parse( ResultMap5 ) ) );
        }
        /// <summary>
        /// Run GetAllUsers query
        /// 
        /// SQL: SELECT * FROM User;
        /// 
        /// QUERY: FROM User SELECT *
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

            SortArgument SortArg = new SortArgument( User, User.GetAttribute( "user_id" ), MongoDBSort.Ascending );

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
            string QueryStringMap2 = QueryGenMap2.Run();
            string QueryStringMap3 = QueryGenMap3.Run();
            string QueryStringMap4 = QueryGenMap4.Run();
            string QueryStringMap5 = QueryGenMap5.Run();

            QueryRunner RunnerMap1 = new QueryRunner( "mongodb://localhost:27017", "pesquisa_cms" );
            QueryRunner RunnerMap2 = new QueryRunner( "mongodb://localhost:27017", "pesquisa_cms_duplicados" );
            QueryRunner RunnerMap3 = new QueryRunner( "mongodb://localhost:27017", "pesquisa_cms_category_duplicado" );
            QueryRunner RunnerMap4 = new QueryRunner( "mongodb://localhost:27017", "pesquisa_cms_store_duplicado" );
            QueryRunner RunnerMap5 = new QueryRunner( "mongodb://localhost:27017", "pesquisa_cms_user_duplicado" );

            string ResultMap1 = RunnerMap1.GetJSON( QueryStringMap1 );
            string ResultMap2 = RunnerMap2.GetJSON( QueryStringMap2 );
            string ResultMap3 = RunnerMap3.GetJSON( QueryStringMap3 );
            string ResultMap4 = RunnerMap4.GetJSON( QueryStringMap4 );
            string ResultMap5 = RunnerMap5.GetJSON( QueryStringMap5 );

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

            Assert.IsTrue( JToken.DeepEquals( JToken.Parse( ResultMap1 ), JToken.Parse( ResultMap2 ) ) );
            Assert.IsTrue( JToken.DeepEquals( JToken.Parse( ResultMap1 ), JToken.Parse( ResultMap3 ) ) );
            Assert.IsTrue( JToken.DeepEquals( JToken.Parse( ResultMap1 ), JToken.Parse( ResultMap4 ) ) );
            Assert.IsTrue( JToken.DeepEquals( JToken.Parse( ResultMap1 ), JToken.Parse( ResultMap5 ) ) );
        }
        /// <summary>
        /// Run GetProductsFromStore query
        /// 
        /// SQL: SELECT * FROM Store 
        ///      JOIN Product ON Product.product_store_id = Store.store_id;
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

            Assert.IsTrue( JToken.DeepEquals( JToken.Parse( ResultMap1 ), JToken.Parse( ResultMap2 ) ) );
            Assert.IsTrue( JToken.DeepEquals( JToken.Parse( ResultMap1 ), JToken.Parse( ResultMap3 ) ) );
            Assert.IsTrue( JToken.DeepEquals( JToken.Parse( ResultMap1 ), JToken.Parse( ResultMap4 ) ) );
            Assert.IsTrue( JToken.DeepEquals( JToken.Parse( ResultMap1 ), JToken.Parse( ResultMap5 ) ) );
        }
    }
}