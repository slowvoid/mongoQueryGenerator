using Newtonsoft.Json.Linq;
using QueryBuilder.ER;
using QueryBuilder.Mongo.Aggregation.Operators;
using QueryBuilder.Operation;
using QueryBuilder.Operation.Arguments;
using QueryBuilder.Query;
using System.Collections.Generic;
using QueryBuilder.Operation.Exceptions;
using QueryBuilder.Mongo.Expressions;
using QueryBuilder.Map;

namespace QueryAnalyzer
{
    public class MarketingCMS
    {
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

            QueryRunner Runner = new QueryRunner( "mongodb://localhost:27017", "research_performance_index_1" );

            // Run each query 100 times
            List<QueryStats> Stats1 = new List<QueryStats>();
            for ( int i = 0; i < 100; i++ )
            {
                QueryStats iterationResult = Runner.GetExplainResult( Query );
                Stats1.Add( iterationResult );
            }

            // Drop before saving new
            MongoContext.DropCollection( "get_all_products_query_1" );
            // Save all
            MongoContext.InsertManyRecords( "get_all_products_query_1", Stats1 );

            QueryRunner Runner2 = new QueryRunner( "mongodb://localhost:27017", "research_performance_index_3" );
            List<QueryStats> Stats2 = new List<QueryStats>();
            for ( int i = 0; i < 100; i++ )
            {
                QueryStats iterationResult = Runner2.GetExplainResult( Query2 );
                Stats2.Add( iterationResult );
            }

            MongoContext.DropCollection( "get_all_products_query_2" );
            MongoContext.InsertManyRecords( "get_all_products_query_2", Stats2 );

            QueryRunner Runner3 = new QueryRunner( "mongodb://localhost:27017", "research_performance_index_2" );
            List<QueryStats> Stats3 = new List<QueryStats>();
            for ( int i = 0; i < 100; i++ )
            {
                QueryStats iterationResult = Runner3.GetExplainResult( Query3 );
                Stats3.Add( iterationResult );
            }

            MongoContext.DropCollection( "get_all_products_query_3" );
            MongoContext.InsertManyRecords( "get_all_products_query_3", Stats3 );

            QueryRunner Runner4 = new QueryRunner( "mongodb://localhost:27017", "research_performance_index_4" );
            List<QueryStats> Stats4 = new List<QueryStats>();
            for ( int i = 0; i < 100; i++ )
            {
                QueryStats iterationResult = Runner4.GetExplainResult( Query4 );
                Stats4.Add( iterationResult );
            }

            MongoContext.DropCollection( "get_all_products_query_4" );
            MongoContext.InsertManyRecords( "get_all_products_query_4", Stats4 );

            QueryRunner Runner5 = new QueryRunner( "mongodb://localhost:27017", "research_performance_index_5" );
            List<QueryStats> Stats5 = new List<QueryStats>();
            for ( int i = 0; i < 100; i++ )
            {
                QueryStats iterationResult = Runner5.GetExplainResult( Query5 );
                Stats5.Add( iterationResult );
            }

            MongoContext.DropCollection( "get_all_products_query_5" );
            MongoContext.InsertManyRecords( "get_all_products_query_5", Stats5 );
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
            string Query = QueryGen.Explain();
            return Query;
        }
        /// <summary>
        /// Get All Stores test
        /// 
        /// Query: FROM Store SELECT *
        /// </summary>
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
            QueryGenerator QueryGenMap3 = new QueryGenerator( StartArgMap3, OperatorsMap3 );
            QueryGenerator QueryGenMap5 = new QueryGenerator( StartArgMap5, OperatorsMap5 );

            string QueryStringMap1 = QueryGenMap1.Explain();
            string QueryStringMap3 = QueryGenMap3.Explain();
            string QueryStringMap5 = QueryGenMap5.Explain();

            QueryRunner RunnerMap1 = new QueryRunner( "mongodb://localhost:27017", "research_performance_index_1" );
            QueryRunner RunnerMap3 = new QueryRunner( "mongodb://localhost:27017", "research_performance_index_2" );
            QueryRunner RunnerMap5 = new QueryRunner( "mongodb://localhost:27017", "research_performance_index_5" );

            // Run each query 100 times
            List<QueryStats> Stats1 = new List<QueryStats>();
            for ( int i = 0; i < 100; i++ )
            {
                QueryStats iterationResult = RunnerMap1.GetExplainResult( QueryStringMap1 );
                Stats1.Add( iterationResult );
            }

            // Drop before saving new
            MongoContext.DropCollection( "get_all_stores_query_1" );
            // Save all
            MongoContext.InsertManyRecords( "get_all_stores_query_1", Stats1 );

            List<QueryStats> Stats2 = new List<QueryStats>();
            for ( int i = 0; i < 100; i++ )
            {
                QueryStats iterationResult = RunnerMap3.GetExplainResult( QueryStringMap3 );
                Stats2.Add( iterationResult );
            }

            // Drop before saving new
            MongoContext.DropCollection( "get_all_stores_query_2" );
            // Save all
            MongoContext.InsertManyRecords( "get_all_stores_query_2", Stats2 );

            List<QueryStats> Stats3 = new List<QueryStats>();
            for ( int i = 0; i < 100; i++ )
            {
                QueryStats iterationResult = RunnerMap5.GetExplainResult( QueryStringMap5 );
                Stats3.Add( iterationResult );
            }

            // Drop before saving new
            MongoContext.DropCollection( "get_all_stores_query_3" );
            // Save all
            MongoContext.InsertManyRecords( "get_all_stores_query_3", Stats3 );
        }
        /// <summary>
        /// Run GetAllUsers test
        /// 
        /// Query: FROM User SELECT *
        /// </summary>
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
            QueryGenerator QueryGenMap3 = new QueryGenerator( StartArgMap3, OperatorsMap3 );
            QueryGenerator QueryGenMap4 = new QueryGenerator( StartArgMap4, OperatorsMap4 );

            string QueryStringMap1 = QueryGenMap1.Explain();
            string QueryStringMap3 = QueryGenMap3.Explain();
            string QueryStringMap4 = QueryGenMap4.Explain();

            QueryRunner RunnerMap1 = new QueryRunner( "mongodb://localhost:27017", "research_performance_index_1" );
            QueryRunner RunnerMap3 = new QueryRunner( "mongodb://localhost:27017", "research_performance_index_2" );
            QueryRunner RunnerMap4 = new QueryRunner( "mongodb://localhost:27017", "research_performance_index_4" );

            List<QueryStats> Stats1 = new List<QueryStats>();
            for ( int i = 0; i < 100; i++ )
            {
                QueryStats iterationResult = RunnerMap1.GetExplainResult( QueryStringMap1 );
                Stats1.Add( iterationResult );
            }

            // Drop before saving new
            MongoContext.DropCollection( "get_all_users_query_1" );
            // Save all
            MongoContext.InsertManyRecords( "get_all_users_query_1", Stats1 );

            List<QueryStats> Stats2 = new List<QueryStats>();
            for ( int i = 0; i < 100; i++ )
            {
                QueryStats iterationResult = RunnerMap3.GetExplainResult( QueryStringMap3 );
                Stats2.Add( iterationResult );
            }

            // Drop before saving new
            MongoContext.DropCollection( "get_all_users_query_2" );
            // Save all
            MongoContext.InsertManyRecords( "get_all_users_query_2", Stats2 );

            List<QueryStats> Stats3 = new List<QueryStats>();
            for ( int i = 0; i < 100; i++ )
            {
                QueryStats iterationResult = RunnerMap4.GetExplainResult( QueryStringMap4 );
                Stats3.Add( iterationResult );
            }

            // Drop before saving new
            MongoContext.DropCollection( "get_all_users_query_3" );
            // Save all
            MongoContext.InsertManyRecords( "get_all_users_query_3", Stats3 );
        }
        /// <summary>
        /// Run GetAllCategories Test
        /// 
        /// QUERY: FROM Category SELECT *
        /// </summary>
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
            QueryGenerator QueryGenMap4 = new QueryGenerator( StartArgMap4, OperatorsMap4 );
            QueryGenerator QueryGenMap5 = new QueryGenerator( StartArgMap5, OperatorsMap5 );

            string QueryStringMap1 = QueryGenMap1.Explain();
            string QueryStringMap4 = QueryGenMap4.Explain();
            string QueryStringMap5 = QueryGenMap5.Explain();

            QueryRunner RunnerMap1 = new QueryRunner( "mongodb://localhost:27017", "research_performance_index_1" );
            QueryRunner RunnerMap4 = new QueryRunner( "mongodb://localhost:27017", "research_performance_index_4" );
            QueryRunner RunnerMap5 = new QueryRunner( "mongodb://localhost:27017", "research_performance_index_5" );

            List<QueryStats> Stats1 = new List<QueryStats>();
            for ( int i = 0; i < 100; i++ )
            {
                QueryStats iterationResult = RunnerMap1.GetExplainResult( QueryStringMap1 );
                Stats1.Add( iterationResult );
            }

            // Drop before saving new
            MongoContext.DropCollection( "get_all_categories_query_1" );
            // Save all
            MongoContext.InsertManyRecords( "get_all_categories_query_1", Stats1 );

            List<QueryStats> Stats2 = new List<QueryStats>();
            for ( int i = 0; i < 100; i++ )
            {
                QueryStats iterationResult = RunnerMap4.GetExplainResult( QueryStringMap4 );
                Stats2.Add( iterationResult );
            }

            // Drop before saving new
            MongoContext.DropCollection( "get_all_categories_query_2" );
            // Save all
            MongoContext.InsertManyRecords( "get_all_categories_query_2", Stats2 );

            List<QueryStats> Stats3 = new List<QueryStats>();
            for ( int i = 0; i < 100; i++ )
            {
                QueryStats iterationResult = RunnerMap5.GetExplainResult( QueryStringMap5 );
                Stats3.Add( iterationResult );
            }

            // Drop before saving new
            MongoContext.DropCollection( "get_all_categories_query_3" );
            // Save all
            MongoContext.InsertManyRecords( "get_all_categories_query_3", Stats3 );
        }

        /// <summary>
        /// Run GetProductsFromStore query
        /// 
        /// QUERY: FROM Store 
        ///        RJOIN (Product, StoreHasProduct)
        ///        SELECT *
        /// </summary>
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
            QueryGenerator GeneratorMap3 = new QueryGenerator( StartArgMap3, OperatorsToExecuteMap3 );
            QueryGenerator GeneratorMap5 = new QueryGenerator( StartArgMap5, OperatorsToExecuteMap5 );

            string QueryMap1 = GeneratorMap1.Explain();
            string QueryMap3 = GeneratorMap3.Explain();
            string QueryMap5 = GeneratorMap5.Explain();

            QueryRunner RunnerMap1 = new QueryRunner( "mongodb://localhost:27017", "research_performance_index_1" );
            QueryRunner RunnerMap3 = new QueryRunner( "mongodb://localhost:27017", "research_performance_index_2" );
            QueryRunner RunnerMap5 = new QueryRunner( "mongodb://localhost:27017", "research_performance_index_5" );

            List<QueryStats> Stats1 = new List<QueryStats>();
            for ( int i = 0; i < 100; i++ )
            {
                QueryStats iterationResult = RunnerMap1.GetExplainResult( QueryMap1 );
                Stats1.Add( iterationResult );
            }

            // Drop before saving new
            MongoContext.DropCollection( "get_all_products_from_store_query_1" );
            // Save all
            MongoContext.InsertManyRecords( "get_all_products_from_store_query_1", Stats1 );

            List<QueryStats> Stats2 = new List<QueryStats>();
            for ( int i = 0; i < 100; i++ )
            {
                QueryStats iterationResult = RunnerMap3.GetExplainResult( QueryMap3 );
                Stats2.Add( iterationResult );
            }

            // Drop before saving new
            MongoContext.DropCollection( "get_all_products_from_store_query_2" );
            // Save all
            MongoContext.InsertManyRecords( "get_all_products_from_store_query_2", Stats2 );

            List<QueryStats> Stats3 = new List<QueryStats>();
            for ( int i = 0; i < 100; i++ )
            {
                QueryStats iterationResult = RunnerMap5.GetExplainResult( QueryMap5 );
                Stats3.Add( iterationResult );
            }

            // Drop before saving new
            MongoContext.DropCollection( "get_all_products_from_store_query_3" );
            // Save all
            MongoContext.InsertManyRecords( "get_all_products_from_store_query_3", Stats3 );
        }

        /// <summary>
        /// Run GetProductsFromCategory query
        /// 
        /// QUERY: FROM Category 
        ///        RJOIN (Product, StoreHasProduct)
        ///        SELECT *
        /// </summary>
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
            QueryGenerator GeneratorMap4 = new QueryGenerator( StartArgMap4, OperatorsToExecuteMap4 );
            QueryGenerator GeneratorMap5 = new QueryGenerator( StartArgMap5, OperatorsToExecuteMap5 );

            string QueryMap1 = GeneratorMap1.Explain();
            string QueryMap4 = GeneratorMap4.Explain();
            string QueryMap5 = GeneratorMap5.Explain();

            QueryRunner RunnerMap1 = new QueryRunner( "mongodb://localhost:27017", "research_performance_index_1" );
            QueryRunner RunnerMap4 = new QueryRunner( "mongodb://localhost:27017", "research_performance_index_4" );
            QueryRunner RunnerMap5 = new QueryRunner( "mongodb://localhost:27017", "research_performance_index_5" );

            List<QueryStats> Stats1 = new List<QueryStats>();
            for ( int i = 0; i < 100; i++ )
            {
                QueryStats iterationResult = RunnerMap1.GetExplainResult( QueryMap1 );
                Stats1.Add( iterationResult );
            }

            // Drop before saving new
            MongoContext.DropCollection( "get_all_products_from_category_query_1" );
            // Save all
            MongoContext.InsertManyRecords( "get_all_products_from_category_query_1", Stats1 );

            List<QueryStats> Stats2 = new List<QueryStats>();
            for ( int i = 0; i < 100; i++ )
            {
                QueryStats iterationResult = RunnerMap4.GetExplainResult( QueryMap4 );
                Stats2.Add( iterationResult );
            }

            // Drop before saving new
            MongoContext.DropCollection( "get_all_products_from_category_query_2" );
            // Save all
            MongoContext.InsertManyRecords( "get_all_products_from_category_query_2", Stats2 );

            List<QueryStats> Stats3 = new List<QueryStats>();
            for ( int i = 0; i < 100; i++ )
            {
                QueryStats iterationResult = RunnerMap5.GetExplainResult( QueryMap5 );
                Stats3.Add( iterationResult );
            }

            // Drop before saving new
            MongoContext.DropCollection( "get_all_products_from_category_query_3" );
            // Save all
            MongoContext.InsertManyRecords( "get_all_products_from_category_query_3", Stats3 );
        }

        /// <summary>
        /// Run GetProductsFromUser query
        /// 
        /// IMPORTANT: THIS ONE IS NOT REALLY WORKING (THE QUERY IS FINE, THE EXPLAIN MODE IS NOT)
        /// 
        /// QUERY: FROM Category 
        ///        RJOIN (Product, StoreHasProduct)
        ///        SELECT *
        /// </summary>
 
        public void GetAllProductsFromUser()
        {
            RequiredDataContainer DataMap = MarketingCMSDataProvider.MapEntitiesToCollections();
            RequiredDataContainer DataMapCategoryDuplicated = MarketingCMSDataProvider.MapEntitiesToCollectionCategoryDuplicated();
            RequiredDataContainer DataMapStoreDuplicated = MarketingCMSDataProvider.MapEntitiesToCollectionsStoreDuplicated();

            QueryableEntity User = new QueryableEntity( DataMap.EntityRelationshipModel.FindByName( "User" ) );
            QueryableEntity Product = new QueryableEntity( DataMap.EntityRelationshipModel.FindByName( "Product" ) );

            RelationshipJoinOperator RJoinOp1 = new RelationshipJoinOperator( User, (Relationship)DataMap.EntityRelationshipModel.FindByName( "UserProducts" ),
                new List<QueryableEntity>() { Product }, DataMap.ERMongoMapping );

            RelationshipJoinOperator RJoinOp3 = new RelationshipJoinOperator( User, (Relationship)DataMap.EntityRelationshipModel.FindByName( "UserProducts" ),
                new List<QueryableEntity>() { Product }, DataMapCategoryDuplicated.ERMongoMapping );

            RelationshipJoinOperator RJoinOp4 = new RelationshipJoinOperator( User, (Relationship)DataMap.EntityRelationshipModel.FindByName( "UserProducts" ),
                new List<QueryableEntity>() { Product }, DataMapStoreDuplicated.ERMongoMapping );

            SortArgument SortArg = new SortArgument( User, User.GetAttribute( "UserID" ), MongoDBSort.Ascending );

            SortStage SortOpMap1 = new SortStage( new List<SortArgument>() { SortArg }, DataMap.ERMongoMapping );
            SortStage SortOpMap3 = new SortStage( new List<SortArgument>() { SortArg }, DataMapCategoryDuplicated.ERMongoMapping );
            SortStage SortOpMap4 = new SortStage( new List<SortArgument>() { SortArg }, DataMapStoreDuplicated.ERMongoMapping );

            List<AlgebraOperator> OperatorsToExecuteMap1 = new List<AlgebraOperator>() { RJoinOp1, SortOpMap1 };
            List<AlgebraOperator> OperatorsToExecuteMap3 = new List<AlgebraOperator>() { RJoinOp3, SortOpMap3 };
            List<AlgebraOperator> OperatorsToExecuteMap4 = new List<AlgebraOperator>() { RJoinOp4, SortOpMap4 };

            FromArgument StartArgMap1 = new FromArgument( User, DataMap.ERMongoMapping );
            FromArgument StartArgMap3 = new FromArgument( User, DataMapCategoryDuplicated.ERMongoMapping );
            FromArgument StartArgMap4 = new FromArgument( User, DataMapStoreDuplicated.ERMongoMapping );

            QueryGenerator GeneratorMap1 = new QueryGenerator( StartArgMap1, OperatorsToExecuteMap1 );
            QueryGenerator GeneratorMap3 = new QueryGenerator( StartArgMap3, OperatorsToExecuteMap3 );
            QueryGenerator GeneratorMap4 = new QueryGenerator( StartArgMap4, OperatorsToExecuteMap4 );

            string QueryMap1 = GeneratorMap1.Explain();
            string QueryMap3 = GeneratorMap3.Explain();
            string QueryMap4 = GeneratorMap4.Explain();

            //QueryRunner RunnerMap1 = new QueryRunner( "mongodb://localhost:27017", "research_performance_index_1" );
            //QueryRunner RunnerMap3 = new QueryRunner( "mongodb://localhost:27017", "research_performance_index_2" );
            //QueryRunner RunnerMap4 = new QueryRunner( "mongodb://localhost:27017", "research_performance_index_4" );

            //List<QueryStats> Stats1 = new List<QueryStats>();
            //for ( int i = 0; i < 100; i++ )
            //{
            //    QueryStats iterationResult = RunnerMap1.GetExplainResult( QueryMap1 );
            //    Stats1.Add( iterationResult );
            //}

            //// Drop before saving new
            //MongoContext.DropCollection( "get_all_products_from_user_query_1" );
            //// Save all
            //MongoContext.InsertManyRecords( "get_all_products_from_user_query_1", Stats1 );

            //List<QueryStats> Stats2 = new List<QueryStats>();
            //for ( int i = 0; i < 100; i++ )
            //{
            //    QueryStats iterationResult = RunnerMap3.GetExplainResult( QueryMap3 );
            //    Stats2.Add( iterationResult );
            //}

            //// Drop before saving new
            //MongoContext.DropCollection( "get_all_products_from_user_query_2" );
            //// Save all
            //MongoContext.InsertManyRecords( "get_all_products_from_user_query_2", Stats2 );

            //List<QueryStats> Stats3 = new List<QueryStats>();
            //for ( int i = 0; i < 100; i++ )
            //{
            //    QueryStats iterationResult = RunnerMap4.GetExplainResult( QueryMap4 );
            //    Stats3.Add( iterationResult );
            //}

            //// Drop before saving new
            //MongoContext.DropCollection( "get_all_products_from_user_query_3" );
            //// Save all
            //MongoContext.InsertManyRecords( "get_all_products_from_user_query_3", Stats3 );
        }
        /// <summary>
        /// Run GetAllProductsFromCategoryWithStore
        /// 
        /// QUERY: FROM Category 
        ///        RJOIN (Product RJOIN [(Store, StoreProduct)], CategoryProduct) 
        /// </summary>
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
            QueryGenerator GeneratorMap4 = new QueryGenerator( StartArgMap4, OperatorsToExecuteMap4 );
            QueryGenerator GeneratorMap5 = new QueryGenerator( StartArgMap5, OperatorsToExecuteMap5 );

            string QueryMap1 = GeneratorMap1.Explain();
            string QueryMap4 = GeneratorMap4.Explain();
            string QueryMap5 = GeneratorMap5.Explain();

            QueryRunner RunnerMap1 = new QueryRunner( "mongodb://localhost:27017", "research_performance_index_1" );
            QueryRunner RunnerMap4 = new QueryRunner( "mongodb://localhost:27017", "research_performance_index_4" );
            QueryRunner RunnerMap5 = new QueryRunner( "mongodb://localhost:27017", "research_performance_index_5" );

            List<QueryStats> Stats1 = new List<QueryStats>();
            for ( int i = 0; i < 100; i++ )
            {
                QueryStats iterationResult = RunnerMap1.GetExplainResult( QueryMap1 );
                Stats1.Add( iterationResult );
            }

            // Drop before saving new
            MongoContext.DropCollection( "get_all_products_from_category_with_store_query_1" );
            // Save all
            MongoContext.InsertManyRecords( "get_all_products_from_category_with_store_query_1", Stats1 );

            List<QueryStats> Stats2 = new List<QueryStats>();
            for ( int i = 0; i < 100; i++ )
            {
                QueryStats iterationResult = RunnerMap4.GetExplainResult( QueryMap4 );
                Stats2.Add( iterationResult );
            }

            // Drop before saving new
            MongoContext.DropCollection( "get_all_products_from_category_with_store_query_2" );
            // Save all
            MongoContext.InsertManyRecords( "get_all_products_from_category_with_store_query_2", Stats2 );

            List<QueryStats> Stats3 = new List<QueryStats>();
            for ( int i = 0; i < 100; i++ )
            {
                QueryStats iterationResult = RunnerMap5.GetExplainResult( QueryMap5 );
                Stats3.Add( iterationResult );
            }

            // Drop before saving new
            MongoContext.DropCollection( "get_all_products_from_category_with_store_query_3" );
            // Save all
            MongoContext.InsertManyRecords( "get_all_products_from_category_with_store_query_3", Stats3 );
        }

        /// <summary>
        /// Run GetAllProductsFromCategoryWithUser
        /// 
        /// QUERY: FROM Category 
        ///        RJOIN (Product RJOIN [(User, UserProduct)], CategoryProduct) 
        /// </summary>
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
            List<AlgebraOperator> OperatorsToExecuteMap4 = new List<AlgebraOperator>() { RJoinOp4, SortOpMap4 };
            List<AlgebraOperator> OperatorsToExecuteMap5 = new List<AlgebraOperator>() { RJoinOp5, SortOpMap5 };

            FromArgument StartArgMap1 = new FromArgument( Category, DataMap.ERMongoMapping );
            FromArgument StartArgMap4 = new FromArgument( Category, DataMapStoreDuplicated.ERMongoMapping );
            FromArgument StartArgMap5 = new FromArgument( Category, DataMapUserDuplicated.ERMongoMapping );

            QueryGenerator GeneratorMap1 = new QueryGenerator( StartArgMap1, OperatorsToExecuteMap1 );
            QueryGenerator GeneratorMap4 = new QueryGenerator( StartArgMap4, OperatorsToExecuteMap4 );
            QueryGenerator GeneratorMap5 = new QueryGenerator( StartArgMap5, OperatorsToExecuteMap5 );

            string QueryMap1 = GeneratorMap1.Explain();
            string QueryMap4 = GeneratorMap4.Explain();
            string QueryMap5 = GeneratorMap5.Explain();

            QueryRunner RunnerMap1 = new QueryRunner( "mongodb://localhost:27017", "research_performance_index_1" );
            QueryRunner RunnerMap4 = new QueryRunner( "mongodb://localhost:27017", "research_performance_index_4" );
            QueryRunner RunnerMap5 = new QueryRunner( "mongodb://localhost:27017", "research_performance_index_5" );

            List<QueryStats> Stats1 = new List<QueryStats>();
            for ( int i = 0; i < 100; i++ )
            {
                QueryStats iterationResult = RunnerMap1.GetExplainResult( QueryMap1 );
                Stats1.Add( iterationResult );
            }

            // Drop before saving new
            MongoContext.DropCollection( "get_all_products_from_category_with_user_query_1" );
            // Save all
            MongoContext.InsertManyRecords( "get_all_products_from_category_with_user_query_1", Stats1 );

            List<QueryStats> Stats2 = new List<QueryStats>();
            for ( int i = 0; i < 100; i++ )
            {
                QueryStats iterationResult = RunnerMap4.GetExplainResult( QueryMap4 );
                Stats2.Add( iterationResult );
            }

            // Drop before saving new
            MongoContext.DropCollection( "get_all_products_from_category_with_user_query_2" );
            // Save all
            MongoContext.InsertManyRecords( "get_all_products_from_category_with_user_query_2", Stats2 );

            List<QueryStats> Stats3 = new List<QueryStats>();
            for ( int i = 0; i < 100; i++ )
            {
                QueryStats iterationResult = RunnerMap5.GetExplainResult( QueryMap5 );
                Stats3.Add( iterationResult );
            }

            // Drop before saving new
            MongoContext.DropCollection( "get_all_products_from_category_with_user_query_3" );
            // Save all
            MongoContext.InsertManyRecords( "get_all_products_from_category_with_user_query_3", Stats3 );
        }
        /// <summary>
        /// Run GetProductTitleAndUserName test
        /// 
        /// QUERY: FROM Product
        ///        RJOIN (User, UserProducts)
        ///        SELECT Product.Title, User.UserName
        /// </summary>
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

            string QueryMap1 = GeneratorMap1.Explain();
            string QueryMap2 = GeneratorMap2.Explain();
            string QueryMap3 = GeneratorMap3.Explain();
            string QueryMap4 = GeneratorMap4.Explain();
            string QueryMap5 = GeneratorMap5.Explain();

            QueryRunner RunnerMap1 = new QueryRunner( "mongodb://localhost:27017", "research_performance_index_1" );
            QueryRunner RunnerMap2 = new QueryRunner( "mongodb://localhost:27017", "research_performance_index_3" );
            QueryRunner RunnerMap3 = new QueryRunner( "mongodb://localhost:27017", "research_performance_index_2" );
            QueryRunner RunnerMap4 = new QueryRunner( "mongodb://localhost:27017", "research_performance_index_4" );
            QueryRunner RunnerMap5 = new QueryRunner( "mongodb://localhost:27017", "research_performance_index_5" );

            List<QueryStats> Stats1 = new List<QueryStats>();
            for ( int i = 0; i < 100; i++ )
            {
                QueryStats iterationResult = RunnerMap1.GetExplainResult( QueryMap1 );
                Stats1.Add( iterationResult );
            }

            // Drop before saving new
            MongoContext.DropCollection( "get_product_title_username_query_1" );
            // Save all
            MongoContext.InsertManyRecords( "get_product_title_username_query_1", Stats1 );

            List<QueryStats> Stats2 = new List<QueryStats>();
            for ( int i = 0; i < 100; i++ )
            {
                QueryStats iterationResult = RunnerMap2.GetExplainResult( QueryMap2 );
                Stats2.Add( iterationResult );
            }

            // Drop before saving new
            MongoContext.DropCollection( "get_product_title_username_query_2" );
            // Save all
            MongoContext.InsertManyRecords( "get_product_title_username_query_2", Stats2 );

            List<QueryStats> Stats3 = new List<QueryStats>();
            for ( int i = 0; i < 100; i++ )
            {
                QueryStats iterationResult = RunnerMap3.GetExplainResult( QueryMap3 );
                Stats3.Add( iterationResult );
            }

            // Drop before saving new
            MongoContext.DropCollection( "get_product_title_username_query_3" );
            // Save all
            MongoContext.InsertManyRecords( "get_product_title_username_query_3", Stats3 );

            List<QueryStats> Stats4 = new List<QueryStats>();
            for ( int i = 0; i < 100; i++ )
            {
                QueryStats iterationResult = RunnerMap4.GetExplainResult( QueryMap4 );
                Stats4.Add( iterationResult );
            }

            // Drop before saving new
            MongoContext.DropCollection( "get_product_title_username_query_4" );
            // Save all
            MongoContext.InsertManyRecords( "get_product_title_username_query_4", Stats4 );

            List<QueryStats> Stats5 = new List<QueryStats>();
            for ( int i = 0; i < 100; i++ )
            {
                QueryStats iterationResult = RunnerMap5.GetExplainResult( QueryMap5 );
                Stats5.Add( iterationResult );
            }

            // Drop before saving new
            MongoContext.DropCollection( "get_product_title_username_query_5" );
            // Save all
            MongoContext.InsertManyRecords( "get_product_title_username_query_5", Stats5 );
        }

        /// <summary>
        /// Run GetAllProductsFromCategoryWithUserAndSelectOnlyTitleNameEmailCategoryName
        /// 
        /// QUERY: FROM Category 
        ///        RJOIN (Product RJOIN [(User, UserProduct)], CategoryProduct)
        ///        SELECT Category.CategoryName, Product.Title, User.UserName, User.UserEmail
        /// </summary>
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
            SortStage SortOpMap4 = new SortStage( new List<SortArgument>() { SortArg }, DataMapStoreDuplicated.ERMongoMapping );
            SortStage SortOpMap5 = new SortStage( new List<SortArgument>() { SortArg }, DataMapUserDuplicated.ERMongoMapping );

            List<AlgebraOperator> OperatorsToExecuteMap1 = new List<AlgebraOperator>() { RJoinOp1, ProjectOp1, SortOpMap1 };
            List<AlgebraOperator> OperatorsToExecuteMap4 = new List<AlgebraOperator>() { RJoinOp4, ProjectOp4, SortOpMap4 };
            List<AlgebraOperator> OperatorsToExecuteMap5 = new List<AlgebraOperator>() { RJoinOp5, ProjectOp5, SortOpMap5 };

            FromArgument StartArgMap1 = new FromArgument( Category, DataMap.ERMongoMapping );
            FromArgument StartArgMap4 = new FromArgument( Category, DataMapStoreDuplicated.ERMongoMapping );
            FromArgument StartArgMap5 = new FromArgument( Category, DataMapUserDuplicated.ERMongoMapping );

            QueryGenerator GeneratorMap1 = new QueryGenerator( StartArgMap1, OperatorsToExecuteMap1 );
            QueryGenerator GeneratorMap4 = new QueryGenerator( StartArgMap4, OperatorsToExecuteMap4 );
            QueryGenerator GeneratorMap5 = new QueryGenerator( StartArgMap5, OperatorsToExecuteMap5 );

            string QueryMap1 = GeneratorMap1.Explain();
            string QueryMap4 = GeneratorMap4.Explain();
            string QueryMap5 = GeneratorMap5.Explain();

            QueryRunner RunnerMap1 = new QueryRunner( "mongodb://localhost:27017", "research_performance_index_1" );
            QueryRunner RunnerMap4 = new QueryRunner( "mongodb://localhost:27017", "research_performance_index_4" );
            QueryRunner RunnerMap5 = new QueryRunner( "mongodb://localhost:27017", "research_performance_index_5" );

            List<QueryStats> Stats1 = new List<QueryStats>();
            for ( int i = 0; i < 100; i++ )
            {
                QueryStats iterationResult = RunnerMap1.GetExplainResult( QueryMap1 );
                Stats1.Add( iterationResult );
            }

            // Drop before saving new
            MongoContext.DropCollection( "get_all_products_from_category_with_user_select_few_query_1" );
            // Save all
            MongoContext.InsertManyRecords( "get_all_products_from_category_with_user_select_few_query_1", Stats1 );

            List<QueryStats> Stats2 = new List<QueryStats>();
            for ( int i = 0; i < 100; i++ )
            {
                QueryStats iterationResult = RunnerMap4.GetExplainResult( QueryMap4 );
                Stats2.Add( iterationResult );
            }

            // Drop before saving new
            MongoContext.DropCollection( "get_all_products_from_category_with_user_select_few_query_2" );
            // Save all
            MongoContext.InsertManyRecords( "get_all_products_from_category_with_user_select_few_query_2", Stats2 );

            List<QueryStats> Stats3 = new List<QueryStats>();
            for ( int i = 0; i < 100; i++ )
            {
                QueryStats iterationResult = RunnerMap5.GetExplainResult( QueryMap5 );
                Stats3.Add( iterationResult );
            }

            // Drop before saving new
            MongoContext.DropCollection( "get_all_products_from_category_with_user_select_few_query_3" );
            // Save all
            MongoContext.InsertManyRecords( "get_all_products_from_category_with_user_select_few_query_3", Stats3 );
        }

        /// <summary>
        /// Execute GetCategoryThatIsNamedHome
        /// 
        /// Query: FROM Category
        ///        SELECT *
        ///        WHERE CategoryName = 'Home'
        /// </summary>
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

            string QueryMap1 = GeneratorMap1.Explain();
            string QueryMap4 = GeneratorMap4.Explain();
            string QueryMap5 = GeneratorMap5.Explain();

            QueryRunner RunnerMap1 = new QueryRunner( "mongodb://localhost:27017", "research_performance_index_1" );
            QueryRunner RunnerMap4 = new QueryRunner( "mongodb://localhost:27017", "research_performance_index_4" );
            QueryRunner RunnerMap5 = new QueryRunner( "mongodb://localhost:27017", "research_performance_index_5" );

            List<QueryStats> Stats1 = new List<QueryStats>();
            for ( int i = 0; i < 100; i++ )
            {
                QueryStats iterationResult = RunnerMap1.GetExplainResult( QueryMap1 );
                Stats1.Add( iterationResult );
            }

            // Drop before saving new
            MongoContext.DropCollection( "get_category_named_home_query_1" );
            // Save all
            MongoContext.InsertManyRecords( "get_category_named_home_query_1", Stats1 );

            List<QueryStats> Stats2 = new List<QueryStats>();
            for ( int i = 0; i < 100; i++ )
            {
                QueryStats iterationResult = RunnerMap4.GetExplainResult( QueryMap4 );
                Stats2.Add( iterationResult );
            }

            // Drop before saving new
            MongoContext.DropCollection( "get_category_named_home_query_2" );
            // Save all
            MongoContext.InsertManyRecords( "get_category_named_home_query_2", Stats2 );

            List<QueryStats> Stats3 = new List<QueryStats>();
            for ( int i = 0; i < 100; i++ )
            {
                QueryStats iterationResult = RunnerMap5.GetExplainResult( QueryMap5 );
                Stats3.Add( iterationResult );
            }

            // Drop before saving new
            MongoContext.DropCollection( "get_category_named_home_query_3" );
            // Save all
            MongoContext.InsertManyRecords( "get_category_named_home_query_3", Stats3 );
        }
        /// <summary>
        /// Execute GetAllProductsThatCostsLessThan5
        /// 
        /// Query:
        /// 
        /// FROM Product p
        /// RJOIN <CategoryProducts> ( Category c )
        /// RJOIN <StoreProducts> ( Store s )
        /// RJOIN <UserProducts> ( User u )
        /// WHERE p.Price < 5
        /// SELECT *
        /// </summary>
        public void GetAllProductsThatCostsLessThan5()
        {
            RequiredDataContainer DataMap1 = MarketingCMSDataProvider.MapEntitiesToCollections();
            RequiredDataContainer DataMap2 = MarketingCMSDataProvider.MapEntitiesToCollectionDuplicates();
            RequiredDataContainer DataMap3 = MarketingCMSDataProvider.MapEntitiesToCollectionCategoryDuplicated();
            RequiredDataContainer DataMap4 = MarketingCMSDataProvider.MapEntitiesToCollectionsStoreDuplicated();
            RequiredDataContainer DataMap5 = MarketingCMSDataProvider.MapEntitiesToCollectionsUserDuplicated();

            QueryableEntity Product = new QueryableEntity( DataMap1.EntityRelationshipModel.FindByName( "Product" ) );
            QueryableEntity Store = new QueryableEntity( DataMap1.EntityRelationshipModel.FindByName( "Store" ) );
            QueryableEntity Category = new QueryableEntity( DataMap1.EntityRelationshipModel.FindByName( "Category" ) );
            QueryableEntity User = new QueryableEntity( DataMap1.EntityRelationshipModel.FindByName( "User" ) );

            List<AlgebraOperator> OperatorList1 = _GetAllProductsLessThan5Operations( DataMap1 );
            List<AlgebraOperator> OperatorList2 = _GetAllProductsLessThan5Operations( DataMap2 );
            List<AlgebraOperator> OperatorList3 = _GetAllProductsLessThan5Operations( DataMap3 );
            List<AlgebraOperator> OperatorList4 = _GetAllProductsLessThan5Operations( DataMap4 );
            List<AlgebraOperator> OperatorList5 = _GetAllProductsLessThan5Operations( DataMap5 );

            FromArgument StartArg1 = new FromArgument( Product, DataMap1.ERMongoMapping );
            FromArgument StartArg2 = new FromArgument( Product, DataMap2.ERMongoMapping );
            FromArgument StartArg3 = new FromArgument( Product, DataMap3.ERMongoMapping );
            FromArgument StartArg4 = new FromArgument( Product, DataMap4.ERMongoMapping );
            FromArgument StartArg5 = new FromArgument( Product, DataMap5.ERMongoMapping );

            QueryGenerator QueryGen1 = new QueryGenerator( StartArg1, OperatorList1 );
            QueryGenerator QueryGen2 = new QueryGenerator( StartArg2, OperatorList2 );
            QueryGenerator QueryGen3 = new QueryGenerator( StartArg3, OperatorList3 );
            QueryGenerator QueryGen4 = new QueryGenerator( StartArg4, OperatorList4 );
            QueryGenerator QueryGen5 = new QueryGenerator( StartArg5, OperatorList5 );

            string Query1 = QueryGen1.Explain();
            string Query2 = QueryGen2.Explain();
            string Query3 = QueryGen3.Explain();
            string Query4 = QueryGen4.Explain();
            string Query5 = QueryGen5.Explain();

            QueryRunner QueryRunner1 = new QueryRunner( "mongodb://localhost:27017", "research_performance_1" );
            QueryRunner QueryRunner2 = new QueryRunner( "mongodb://localhost:27017", "research_performance_3" );
            QueryRunner QueryRunner3 = new QueryRunner( "mongodb://localhost:27017", "research_performance_2" );
            QueryRunner QueryRunner4 = new QueryRunner( "mongodb://localhost:27017", "research_performance_4" );
            QueryRunner QueryRunner5 = new QueryRunner( "mongodb://localhost:27017", "research_performance_5" );

            List<QueryStats> Stats1 = new List<QueryStats>();
            for ( int i = 0; i < 100; i++ )
            {
                QueryStats iterationResult = QueryRunner1.GetExplainResult( Query1 );
                Stats1.Add( iterationResult );
            }

            MongoContext.DropCollection( "get_all_products_price_less_1" );
            MongoContext.InsertManyRecords( "get_all_products_price_less_1", Stats1 );

            List<QueryStats> Stats2 = new List<QueryStats>();
            for ( int i = 0; i < 100; i++ )
            {
                QueryStats iterationResult = QueryRunner2.GetExplainResult( Query2 );
                Stats2.Add( iterationResult );
            }

            MongoContext.DropCollection( "get_all_products_price_less_2" );
            MongoContext.InsertManyRecords( "get_all_products_price_less_2", Stats2 );

            List<QueryStats> Stats3 = new List<QueryStats>();
            for ( int i = 0; i < 100; i++ )
            {
                QueryStats iterationResult = QueryRunner3.GetExplainResult( Query3 );
                Stats3.Add( iterationResult );
            }

            MongoContext.DropCollection( "get_all_products_price_less_3" );
            MongoContext.InsertManyRecords( "get_all_products_price_less_3", Stats3 );

            List<QueryStats> Stats4 = new List<QueryStats>();
            for ( int i = 0; i < 100; i++ )
            {
                QueryStats iterationResult = QueryRunner4.GetExplainResult( Query4 );
                Stats4.Add( iterationResult );
            }

            MongoContext.DropCollection( "get_all_products_price_less_4" );
            MongoContext.InsertManyRecords( "get_all_products_price_less_4", Stats4 );

            List<QueryStats> Stats5 = new List<QueryStats>();
            for ( int i = 0; i < 100; i++ )
            {
                QueryStats iterationResult = QueryRunner5.GetExplainResult( Query5 );
                Stats5.Add( iterationResult );
            }

            MongoContext.DropCollection( "get_all_products_price_less_5" );
            MongoContext.InsertManyRecords( "get_all_products_price_less_5", Stats5 );
        }
        /// <summary>
        /// Create a list of operators to execute the query
        /// </summary>
        /// <param name="DataMap"></param>
        /// <returns></returns>
        public List<AlgebraOperator> _GetAllProductsLessThan5Operations( RequiredDataContainer DataMap )
        {
            QueryableEntity Product = new QueryableEntity( DataMap.EntityRelationshipModel.FindByName( "Product" ) );
            QueryableEntity Store = new QueryableEntity( DataMap.EntityRelationshipModel.FindByName( "Store" ) );
            QueryableEntity Category = new QueryableEntity( DataMap.EntityRelationshipModel.FindByName( "Category" ) );
            QueryableEntity User = new QueryableEntity( DataMap.EntityRelationshipModel.FindByName( "User" ) );

            RelationshipJoinOperator RJoinOpStore = new RelationshipJoinOperator(
                Product,
                (Relationship)DataMap.EntityRelationshipModel.FindByName( "StoreProducts" ),
                new List<QueryableEntity>() { Store },
                DataMap.ERMongoMapping );

            RelationshipJoinOperator RJoinOpCategory = new RelationshipJoinOperator(
                Product,
                (Relationship)DataMap.EntityRelationshipModel.FindByName( "CategoryProducts" ),
                new List<QueryableEntity>() { Category },
                DataMap.ERMongoMapping );

            RelationshipJoinOperator RJoinOpUser = new RelationshipJoinOperator(
                Product,
                (Relationship)DataMap.EntityRelationshipModel.FindByName( "UserProducts" ),
                new List<QueryableEntity>() { User },
                DataMap.ERMongoMapping );

            MapRule ProductRule = DataMap.ERMongoMapping.FindMainRule( Product.Element );
            SelectArgument SelectArg = new SelectArgument( new LtExpr( ProductRule.GetRuleValueForAttribute( Product.GetAttribute( "Price" ) ), 5 ) );

            SelectStage SelectOp = new SelectStage( SelectArg, DataMap.ERMongoMapping );

            return new List<AlgebraOperator>() { RJoinOpStore, RJoinOpCategory, RJoinOpUser, SelectOp };
        }
    }
}