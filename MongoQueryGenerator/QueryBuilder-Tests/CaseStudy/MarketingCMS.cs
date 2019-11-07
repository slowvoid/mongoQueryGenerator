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
            // =====

            // Build and execute Map1 query
            List<AlgebraOperator> Map1OperatorsList = new List<AlgebraOperator>() { RJoinMap1Op, SortOp };

            Pipeline Map1Pipeline = new Pipeline( Map1OperatorsList );
            QueryGenerator Generator = new QueryGenerator( Map1Pipeline );
            Generator.CollectionName = "Product";

            string Map1Query = Generator.Run();

            Assert.IsNotNull( Map1Query, "Generated query [Map1Query] cannot be null" );

            RelationshipJoinOperator RJoinMapDuplicatesOp = new RelationshipJoinOperator( Product,
                new List<RelationshipJoinArgument>() { JoinStoreArgs, JoinCategoryArgs, JoinUserArgs },
                DataMapDuplicates.ERMongoMapping );

            List<AlgebraOperator> MapDuplicatesOpList = new List<AlgebraOperator>() { RJoinMapDuplicatesOp, SortOp2 };
            Pipeline MapDuplicatesPipeline = new Pipeline( MapDuplicatesOpList );
            QueryGenerator GeneratorDuplicates = new QueryGenerator( MapDuplicatesPipeline );
            GeneratorDuplicates.CollectionName = "Product";

            string MapDuplicatesQuery = GeneratorDuplicates.Run();

            Assert.IsNotNull( MapDuplicatesQuery, "Generated query [MapDuplicatesQuery] cannot be null" );

            QueryRunner RunnerMap1 = new QueryRunner( "mongodb://localhost:27017", "pesquisa_cms" );
            QueryRunner RunnerMap2 = new QueryRunner( "mongodb://localhost:27017", "pesquisa_cms_duplicados" );

            string ResultMap1 = RunnerMap1.GetJSON( Map1Query );
            string ResultMap2 = RunnerMap2.GetJSON( MapDuplicatesQuery );

            Assert.IsNotNull( ResultMap1, "Result [Map1] cannot be null" );
            Assert.IsNotNull( ResultMap2, "Result [Map2] cannot be null" );

            Assert.IsTrue( JToken.DeepEquals( JToken.Parse( ResultMap1 ), JToken.Parse( ResultMap2 ) ) );
        }
    }
}