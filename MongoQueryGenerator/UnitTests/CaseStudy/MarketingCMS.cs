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
        /// rjoin <UserProducts> (User u)
        /// rjoin <StoreProducts> (Store s)
        /// rjoin <CategoryProducts> (Category c)
        /// SELECT *
        /// </summary>
        [TestMethod]
        public void GetAllProducts()
        {
            var DataMap = QueryBuilderParser.ParseMapping( Utils.ReadMappingFile( "Mappings/MarketingCMS/entities-to-collections.mapping" ) );
            var DataMap2 = QueryBuilderParser.ParseMapping( Utils.ReadMappingFile( "Mappings/MarketingCMS/entities-to-collections-duplicates.mapping" ) );
            var DataMap3 = QueryBuilderParser.ParseMapping( Utils.ReadMappingFile( "Mappings/MarketingCMS/entities-to-collections-category-duplicated.mapping" ) );
            var DataMap4 = QueryBuilderParser.ParseMapping( Utils.ReadMappingFile( "Mappings/MarketingCMS/entities-to-collections-store-duplicated.mapping" ) );
            var DataMap5 = QueryBuilderParser.ParseMapping( Utils.ReadMappingFile( "Mappings/MarketingCMS/entities-to-collections-user-duplicated.mapping" ) );

            string QueryString = "from Product rjoin <UserProducts> (User) rjoin <StoreProducts> (Store) rjoin <CategoryProducts> (Category) select *";

            QueryGenerator QueryGen = QueryBuilderParser.ParseQuery( QueryString, DataMap );
            QueryGenerator QueryGen2 = QueryBuilderParser.ParseQuery( QueryString, DataMap2 );
            QueryGenerator QueryGen3 = QueryBuilderParser.ParseQuery( QueryString, DataMap3 );
            QueryGenerator QueryGen4 = QueryBuilderParser.ParseQuery( QueryString, DataMap4 );
            QueryGenerator QueryGen5 = QueryBuilderParser.ParseQuery( QueryString, DataMap5 );

            string Query = QueryGen.Run();
            string Query2 = QueryGen2.Run();
            string Query3 = QueryGen3.Run();
            string Query4 = QueryGen4.Run();
            string Query5 = QueryGen5.Run();

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
            RelationshipJoinOperator rjoinProductUser = new RelationshipJoinOperator(
                            Product,
                            (Relationship)DataMap.EntityRelationshipModel.FindByName( "UserProducts" ),
                            new List<QueryableEntity>() { User },
                            DataMap.ERMongoMapping );

            RelationshipJoinOperator rjoinProductStore = new RelationshipJoinOperator(
                Product,
                (Relationship)DataMap.EntityRelationshipModel.FindByName( "StoreProducts" ),
                new List<QueryableEntity>() { Store },
                DataMap.ERMongoMapping );

            RelationshipJoinOperator rjoinProductCategory = new RelationshipJoinOperator(
                Product,
                (Relationship)DataMap.EntityRelationshipModel.FindByName( "CategoryProducts" ),
                new List<QueryableEntity>() { Category },
                DataMap.ERMongoMapping );

            SortArgument SortArg = new SortArgument( Product, Product.GetAttribute( "ProductID" ), MongoDBSort.Ascending );
            SortStage SortOp = new SortStage( new List<SortArgument>() { SortArg }, DataMap.ERMongoMapping );

            List<AlgebraOperator> Operators = new List<AlgebraOperator>() { SortOp, rjoinProductUser, rjoinProductStore, rjoinProductCategory };

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
            var DataMap = QueryBuilderParser.ParseMapping( Utils.ReadMappingFile( "Mappings/MarketingCMS/entities-to-collections.mapping" ) );
            var DataMap2 = QueryBuilderParser.ParseMapping( Utils.ReadMappingFile( "Mappings/MarketingCMS/entities-to-collections-duplicates.mapping" ) );
            var DataMap3 = QueryBuilderParser.ParseMapping( Utils.ReadMappingFile( "Mappings/MarketingCMS/entities-to-collections-category-duplicated.mapping" ) );
            var DataMap4 = QueryBuilderParser.ParseMapping( Utils.ReadMappingFile( "Mappings/MarketingCMS/entities-to-collections-store-duplicated.mapping" ) );
            var DataMap5 = QueryBuilderParser.ParseMapping( Utils.ReadMappingFile( "Mappings/MarketingCMS/entities-to-collections-user-duplicated.mapping" ) );

            string QueryString = "from Store select *";

            QueryGenerator QueryGenMap1 = QueryBuilderParser.ParseQuery( QueryString, DataMap );
            QueryGenerator QueryGenMap2 = QueryBuilderParser.ParseQuery( QueryString, DataMap2 );
            QueryGenerator QueryGenMap3 = QueryBuilderParser.ParseQuery( QueryString, DataMap3 );
            QueryGenerator QueryGenMap4 = QueryBuilderParser.ParseQuery( QueryString, DataMap4 );
            QueryGenerator QueryGenMap5 = QueryBuilderParser.ParseQuery( QueryString, DataMap5 );

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
            var DataMap = QueryBuilderParser.ParseMapping( Utils.ReadMappingFile( "Mappings/MarketingCMS/entities-to-collections.mapping" ) );
            var DataMapDuplicates = QueryBuilderParser.ParseMapping( Utils.ReadMappingFile( "Mappings/MarketingCMS/entities-to-collections-duplicates.mapping" ) );
            var DataMapCategoryDuplicated = QueryBuilderParser.ParseMapping( Utils.ReadMappingFile( "Mappings/MarketingCMS/entities-to-collections-category-duplicated.mapping" ) );
            var DataMapStoreDuplicated = QueryBuilderParser.ParseMapping( Utils.ReadMappingFile( "Mappings/MarketingCMS/entities-to-collections-store-duplicated.mapping" ) );
            var DataMapUserDuplicated = QueryBuilderParser.ParseMapping( Utils.ReadMappingFile( "Mappings/MarketingCMS/entities-to-collections-user-duplicated.mapping" ) );

            string QueryString = "from Users select *";

            QueryGenerator QueryGenMap1 = QueryBuilderParser.ParseQuery( QueryString, DataMap );
            QueryGenerator QueryGenMap2 = QueryBuilderParser.ParseQuery( QueryString, DataMapDuplicates );
            QueryGenerator QueryGenMap3 = QueryBuilderParser.ParseQuery( QueryString, DataMapCategoryDuplicated );
            QueryGenerator QueryGenMap4 = QueryBuilderParser.ParseQuery( QueryString, DataMapStoreDuplicated );
            QueryGenerator QueryGenMap5 = QueryBuilderParser.ParseQuery( QueryString, DataMapUserDuplicated );

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
            var DataMap = QueryBuilderParser.ParseMapping( Utils.ReadMappingFile( "Mappings/MarketingCMS/entities-to-collections.mapping" ) );
            var DataMapDuplicates = QueryBuilderParser.ParseMapping( Utils.ReadMappingFile( "Mappings/MarketingCMS/entities-to-collections-duplicates.mapping" ) );
            var DataMapCategoryDuplicated = QueryBuilderParser.ParseMapping( Utils.ReadMappingFile( "Mappings/MarketingCMS/entities-to-collections-category-duplicated.mapping" ) );
            var DataMapStoreDuplicated = QueryBuilderParser.ParseMapping( Utils.ReadMappingFile( "Mappings/MarketingCMS/entities-to-collections-store-duplicated.mapping" ) );
            var DataMapUserDuplicated = QueryBuilderParser.ParseMapping( Utils.ReadMappingFile( "Mappings/MarketingCMS/entities-to-collections-user-duplicated.mapping" ) );

            string QueryString = "from Category select *";

            QueryGenerator QueryGenMap1 = QueryBuilderParser.ParseQuery( QueryString, DataMap );
            QueryGenerator QueryGenMap2 = QueryBuilderParser.ParseQuery( QueryString, DataMapDuplicates );
            QueryGenerator QueryGenMap3 = QueryBuilderParser.ParseQuery( QueryString, DataMapCategoryDuplicated );
            QueryGenerator QueryGenMap4 = QueryBuilderParser.ParseQuery( QueryString, DataMapStoreDuplicated );
            QueryGenerator QueryGenMap5 = QueryBuilderParser.ParseQuery( QueryString, DataMapUserDuplicated );

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
        ///        rjoin <StoreProducts> (Product p)
        ///        SELECT *
        /// </summary>
        [TestMethod]
        public void GetAllProductsFromStore()
        {
            var DataMap = QueryBuilderParser.ParseMapping( Utils.ReadMappingFile( "Mappings/MarketingCMS/entities-to-collections.mapping" ) );
            var DataMapDuplicates = QueryBuilderParser.ParseMapping( Utils.ReadMappingFile( "Mappings/MarketingCMS/entities-to-collections-duplicates.mapping" ) );
            var DataMapCategoryDuplicated = QueryBuilderParser.ParseMapping( Utils.ReadMappingFile( "Mappings/MarketingCMS/entities-to-collections-category-duplicated.mapping" ) );
            var DataMapStoreDuplicated = QueryBuilderParser.ParseMapping( Utils.ReadMappingFile( "Mappings/MarketingCMS/entities-to-collections-store-duplicated.mapping" ) );
            var DataMapUserDuplicated = QueryBuilderParser.ParseMapping( Utils.ReadMappingFile( "Mappings/MarketingCMS/entities-to-collections-user-duplicated.mapping" ) );

            string QueryString = "from Store rjoin <StoreProducts> (Product) select *";

            QueryGenerator GeneratorMap1 = QueryBuilderParser.ParseQuery( QueryString, DataMap );
            QueryGenerator GeneratorMap2 = QueryBuilderParser.ParseQuery( QueryString, DataMapDuplicates );
            QueryGenerator GeneratorMap3 = QueryBuilderParser.ParseQuery( QueryString, DataMapCategoryDuplicated );
            QueryGenerator GeneratorMap4 = QueryBuilderParser.ParseQuery( QueryString, DataMapStoreDuplicated );
            QueryGenerator GeneratorMap5 = QueryBuilderParser.ParseQuery( QueryString, DataMapUserDuplicated );

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
        ///        rjoin <CategoryProducts> (Product p)
        ///        SELECT *
        /// </summary>
        [TestMethod]
        public void GetAllProductsFromCategory()
        {
            var DataMap = QueryBuilderParser.ParseMapping( Utils.ReadMappingFile( "Mappings/MarketingCMS/entities-to-collections.mapping" ) );
            var DataMapDuplicates = QueryBuilderParser.ParseMapping( Utils.ReadMappingFile( "Mappings/MarketingCMS/entities-to-collections-duplicates.mapping" ) );
            var DataMapCategoryDuplicated = QueryBuilderParser.ParseMapping( Utils.ReadMappingFile( "Mappings/MarketingCMS/entities-to-collections-category-duplicated.mapping" ) );
            var DataMapStoreDuplicated = QueryBuilderParser.ParseMapping( Utils.ReadMappingFile( "Mappings/MarketingCMS/entities-to-collections-store-duplicated.mapping" ) );
            var DataMapUserDuplicated = QueryBuilderParser.ParseMapping( Utils.ReadMappingFile( "Mappings/MarketingCMS/entities-to-collections-user-duplicated.mapping" ) );

            string QueryString = "from Category rjoin <CategoryProducts> (Product) select *";

            QueryGenerator GeneratorMap1 = QueryBuilderParser.ParseQuery( QueryString, DataMap );
            QueryGenerator GeneratorMap2 = QueryBuilderParser.ParseQuery( QueryString, DataMapDuplicates );
            QueryGenerator GeneratorMap3 = QueryBuilderParser.ParseQuery( QueryString, DataMapCategoryDuplicated );
            QueryGenerator GeneratorMap4 = QueryBuilderParser.ParseQuery( QueryString, DataMapStoreDuplicated );
            QueryGenerator GeneratorMap5 = QueryBuilderParser.ParseQuery( QueryString, DataMapUserDuplicated );

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
        ///        rjoin <UserProducts> (Product p)
        ///        SELECT *
        /// </summary>
        [TestMethod]
        public void GetAllProductsFromUser()
        {
            var DataMap = QueryBuilderParser.ParseMapping( Utils.ReadMappingFile( "Mappings/MarketingCMS/entities-to-collections.mapping" ) );
            var DataMapDuplicates = QueryBuilderParser.ParseMapping( Utils.ReadMappingFile( "Mappings/MarketingCMS/entities-to-collections-duplicates.mapping" ) );
            var DataMapCategoryDuplicated = QueryBuilderParser.ParseMapping( Utils.ReadMappingFile( "Mappings/MarketingCMS/entities-to-collections-category-duplicated.mapping" ) );
            var DataMapStoreDuplicated = QueryBuilderParser.ParseMapping( Utils.ReadMappingFile( "Mappings/MarketingCMS/entities-to-collections-store-duplicated.mapping" ) );
            var DataMapUserDuplicated = QueryBuilderParser.ParseMapping( Utils.ReadMappingFile( "Mappings/MarketingCMS/entities-to-collections-user-duplicated.mapping" ) );

            string QueryString = "from User rjoin <UserProducts> (Product) select *";

            QueryGenerator GeneratorMap1 = QueryBuilderParser.ParseQuery( QueryString, DataMap );
            QueryGenerator GeneratorMap2 = QueryBuilderParser.ParseQuery( QueryString, DataMapDuplicates );
            QueryGenerator GeneratorMap3 = QueryBuilderParser.ParseQuery( QueryString, DataMapCategoryDuplicated );
            QueryGenerator GeneratorMap4 = QueryBuilderParser.ParseQuery( QueryString, DataMapStoreDuplicated );
            QueryGenerator GeneratorMap5 = QueryBuilderParser.ParseQuery( QueryString, DataMapUserDuplicated );

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
        ///        rjoin <CategoryProducts> (Product p 
        ///                                  rjoin <StoreProducts> (Store s))
        ///        SELECT *
        /// </summary>
        [TestMethod]
        public void GetAllProductsFromCategoryWithStore()
        {
            var DataMap = QueryBuilderParser.ParseMapping( Utils.ReadMappingFile( "Mappings/MarketingCMS/entities-to-collections.mapping" ) );
            var DataMapDuplicates = QueryBuilderParser.ParseMapping( Utils.ReadMappingFile( "Mappings/MarketingCMS/entities-to-collections-duplicates.mapping" ) );
            var DataMapCategoryDuplicated = QueryBuilderParser.ParseMapping( Utils.ReadMappingFile( "Mappings/MarketingCMS/entities-to-collections-category-duplicated.mapping" ) );
            var DataMapStoreDuplicated = QueryBuilderParser.ParseMapping( Utils.ReadMappingFile( "Mappings/MarketingCMS/entities-to-collections-store-duplicated.mapping" ) );
            var DataMapUserDuplicated = QueryBuilderParser.ParseMapping( Utils.ReadMappingFile( "Mappings/MarketingCMS/entities-to-collections-user-duplicated.mapping" ) );

            string QueryString = "from Category rjoin <CategoryProducts> (Product rjoin <StoreProducts> (Store)) select *";

            QueryGenerator GeneratorMap1 = QueryBuilderParser.ParseQuery( QueryString, DataMap );
            QueryGenerator GeneratorMap2 = QueryBuilderParser.ParseQuery( QueryString, DataMapDuplicates );
            QueryGenerator GeneratorMap3 = QueryBuilderParser.ParseQuery( QueryString, DataMapCategoryDuplicated );
            QueryGenerator GeneratorMap4 = QueryBuilderParser.ParseQuery( QueryString, DataMapStoreDuplicated );
            QueryGenerator GeneratorMap5 = QueryBuilderParser.ParseQuery( QueryString, DataMapUserDuplicated );

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
        ///        rjoin <CategoryProducts> (Product p 
        ///                                  rjoin <UserProducts> (User u))
        ///        SELECT *
        /// </summary>
        [TestMethod]
        public void GetAllProductsFromCategoryWithUser()
        {
            var DataMap = QueryBuilderParser.ParseMapping( Utils.ReadMappingFile( "Mappings/MarketingCMS/entities-to-collections.mapping" ) );
            var DataMapDuplicates = QueryBuilderParser.ParseMapping( Utils.ReadMappingFile( "Mappings/MarketingCMS/entities-to-collections-duplicates.mapping" ) );
            var DataMapCategoryDuplicated = QueryBuilderParser.ParseMapping( Utils.ReadMappingFile( "Mappings/MarketingCMS/entities-to-collections-category-duplicated.mapping" ) );
            var DataMapStoreDuplicated = QueryBuilderParser.ParseMapping( Utils.ReadMappingFile( "Mappings/MarketingCMS/entities-to-collections-store-duplicated.mapping" ) );
            var DataMapUserDuplicated = QueryBuilderParser.ParseMapping( Utils.ReadMappingFile( "Mappings/MarketingCMS/entities-to-collections-user-duplicated.mapping" ) );

            string QueryString = "from Category rjoin <CategoryProducts> (Product <UserProducts> (User)) select *";

            QueryGenerator GeneratorMap1 = QueryBuilderParser.ParseQuery( QueryString, DataMap );
            QueryGenerator GeneratorMap2 = QueryBuilderParser.ParseQuery( QueryString, DataMapDuplicates );
            QueryGenerator GeneratorMap3 = QueryBuilderParser.ParseQuery( QueryString, DataMapCategoryDuplicated );
            QueryGenerator GeneratorMap4 = QueryBuilderParser.ParseQuery( QueryString, DataMapStoreDuplicated );
            QueryGenerator GeneratorMap5 = QueryBuilderParser.ParseQuery( QueryString, DataMapUserDuplicated );

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
        ///        rjoin <UserProducts> (User u)
        ///        SELECT Product.Title, User.UserName
        /// </summary>
        [TestMethod]
        public void GetProductTitleAndUserName()
        {
            var DataMap = QueryBuilderParser.ParseMapping( Utils.ReadMappingFile( "Mappings/MarketingCMS/entities-to-collections.mapping" ) );
            var DataMapDuplicates = QueryBuilderParser.ParseMapping( Utils.ReadMappingFile( "Mappings/MarketingCMS/entities-to-collections-duplicates.mapping" ) );
            var DataMapCategoryDuplicated = QueryBuilderParser.ParseMapping( Utils.ReadMappingFile( "Mappings/MarketingCMS/entities-to-collections-category-duplicated.mapping" ) );
            var DataMapStoreDuplicated = QueryBuilderParser.ParseMapping( Utils.ReadMappingFile( "Mappings/MarketingCMS/entities-to-collections-store-duplicated.mapping" ) );
            var DataMapUserDuplicated = QueryBuilderParser.ParseMapping( Utils.ReadMappingFile( "Mappings/MarketingCMS/entities-to-collections-user-duplicated.mapping" ) );

            string QueryString = "from Product rjoin <UserProducts> (User) select Product.Title, User.UserName";

            QueryGenerator GeneratorMap1 = QueryBuilderParser.ParseQuery( QueryString, DataMap );
            QueryGenerator GeneratorMap2 = QueryBuilderParser.ParseQuery( QueryString, DataMapDuplicates );
            QueryGenerator GeneratorMap3 = QueryBuilderParser.ParseQuery( QueryString, DataMapCategoryDuplicated );
            QueryGenerator GeneratorMap4 = QueryBuilderParser.ParseQuery( QueryString, DataMapStoreDuplicated );
            QueryGenerator GeneratorMap5 = QueryBuilderParser.ParseQuery( QueryString, DataMapUserDuplicated );

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
        ///        rjoin <CategoryProducts> (Product p
        ///                                  rjoin <UserProducts> (User u))
        ///        SELECT Category.CategoryName, Product.Title, User.UserName, User.UserEmail
        /// </summary>
        [TestMethod]
        public void GetAllProductsFromCategoryWithUserAndSelectOnlyTitleNameEmailCategoryName()
        {
            var DataMap = QueryBuilderParser.ParseMapping( Utils.ReadMappingFile( "Mappings/MarketingCMS/entities-to-collections.mapping" ) );
            var DataMapDuplicates = QueryBuilderParser.ParseMapping( Utils.ReadMappingFile( "Mappings/MarketingCMS/entities-to-collections-duplicates.mapping" ) );
            var DataMapCategoryDuplicated = QueryBuilderParser.ParseMapping( Utils.ReadMappingFile( "Mappings/MarketingCMS/entities-to-collections-category-duplicated.mapping" ) );
            var DataMapStoreDuplicated = QueryBuilderParser.ParseMapping( Utils.ReadMappingFile( "Mappings/MarketingCMS/entities-to-collections-store-duplicated.mapping" ) );
            var DataMapUserDuplicated = QueryBuilderParser.ParseMapping( Utils.ReadMappingFile( "Mappings/MarketingCMS/entities-to-collections-user-duplicated.mapping" ) );

            string QueryString = "from Category rjoin <CategoryProducts> (Category rjoin <UserProducts> (User)) select Product.Title, User.UserName, User.UserEmail, Category.CategoryName";

            QueryGenerator GeneratorMap1 = QueryBuilderParser.ParseQuery( QueryString, DataMap );
            QueryGenerator GeneratorMap2 = QueryBuilderParser.ParseQuery( QueryString, DataMapDuplicates );
            QueryGenerator GeneratorMap3 = QueryBuilderParser.ParseQuery( QueryString, DataMap );
            QueryGenerator GeneratorMap4 = QueryBuilderParser.ParseQuery( QueryString, DataMap );
            QueryGenerator GeneratorMap5 = QueryBuilderParser.ParseQuery( QueryString, DataMap );

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
            var DataMap = QueryBuilderParser.ParseMapping( Utils.ReadMappingFile( "Mappings/MarketingCMS/entities-to-collections.mapping" ) );
            var DataMapDuplicates = QueryBuilderParser.ParseMapping( Utils.ReadMappingFile( "Mappings/MarketingCMS/entities-to-collections-duplicates.mapping" ) );
            var DataMapCategoryDuplicated = QueryBuilderParser.ParseMapping( Utils.ReadMappingFile( "Mappings/MarketingCMS/entities-to-collections-category-duplicated.mapping" ) );
            var DataMapStoreDuplicated = QueryBuilderParser.ParseMapping( Utils.ReadMappingFile( "Mappings/MarketingCMS/entities-to-collections-store-duplicated.mapping" ) );
            var DataMapUserDuplicated = QueryBuilderParser.ParseMapping( Utils.ReadMappingFile( "Mappings/MarketingCMS/entities-to-collections-user-duplicated.mapping" ) );

            string QueryString = "from Category where CategoryName = 'Home' select *";

            QueryGenerator GeneratorMap1 = QueryBuilderParser.ParseQuery( QueryString, DataMap );
            QueryGenerator GeneratorMap4 = QueryBuilderParser.ParseQuery( QueryString, DataMapStoreDuplicated );
            QueryGenerator GeneratorMap5 = QueryBuilderParser.ParseQuery( QueryString, DataMapUserDuplicated );

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