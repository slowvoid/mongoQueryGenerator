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
using System.IO;
using System;
using QueryBuilder.Parser;

namespace QueryAnalyzer
{
    public class MarketingCMS
    {
        /// <summary>
        /// Query iterations
        /// </summary>
        public int Iterations { get; set; }
        /// <summary>
        /// Target database to store results
        /// </summary>
        public string TargetDatabase { get; set; }
        /// <summary>
        /// Database prefix, for example {DatabasePrefix}
        /// The mapping number will be appended to this
        /// </summary>
        public string DatabasePrefix { get; set; }
        /// <summary>
        /// If true save queries to file, but do not run iterations
        /// </summary>
        public bool ExportQueries { get; set; }
        /// <summary>
        /// Set to true if it should generate a read query instead of explain
        /// [DEFAULT: false]
        /// </summary>
        public bool UseDefaultQueryInsteadOfExplain { get; set; }
        /// <summary>
        /// Document count for each collection
        /// </summary>
        public Dictionary<string, long> CollectionDocumentCount { get; set; }
        /// <summary>
        /// Keys to use when querying from products
        /// </summary>
        public List<int> ProductKeys { get; set; }
        /// <summary>
        /// Keys to use when querying from category
        /// </summary>
        public List<int> CategoryKeys { get; set; }
        /// <summary>
        /// Keys to use when querying from store
        /// </summary>
        public List<int> StoreKeys { get; set; }
        /// <summary>
        /// Keys to use when querying from user
        /// </summary>
        public List<int> UserKeys { get; set; }
        /// <summary>
        /// List of commands needed to execute all benchmark operations
        /// </summary>
        public List<string> TerminalCommands { get; set; }
        /// <summary>
        /// If true uses queries without a where clause
        /// </summary>
        public bool UseReadAllQueries { get; set; }
        /// <summary>
        /// YCSB workload folder
        /// </summary>
        public string BenchmarkWorkloadFolder { get; set; }
        /// <summary>
        /// [GET] Base query folder
        /// </summary>
        public string BaseQueryFolder
        {
            get
            {
                if ( UseReadAllQueries )
                {
                    return "HandcraftedQueries/read-all/";
                }

                return "HandcraftedQueries/read/";
            }
        }

        public void RunIterationsForQuery(string Database, string Collection, string Query, bool IsNonAggregate = false)
        {
            // Init a temp list to store query response
            List<QueryStats> Stats = new List<QueryStats>();

            // Initialize a query runner for the database and query
            QueryRunner Runner = new QueryRunner( "mongodb://localhost:27017", Database );

            if ( ExportQueries )
            {
                Utils.ExportQueryToFile( Query, $@"D:\Projects\mestrado\YCSB\workloads\{Collection}.mongo" );
            }
            else
            {
                // Run iterations
                for ( int i = 0; i < this.Iterations; i++ )
                {
                    QueryStats iterationResult = new QueryStats();

                    if ( IsNonAggregate )
                    {
                        iterationResult = Runner.GetExplainResultNonAggregate( Query );
                    }
                    else
                    {
                        iterationResult = Runner.GetExplainResult( Query );
                    }

                    Stats.Add( iterationResult );
                }

                MongoContext.DropCollection( this.TargetDatabase, Collection );
                MongoContext.InsertManyRecords( this.TargetDatabase, Collection, Stats );
            }            
        }
        /// <summary>
        /// Run get all products query
        /// 
        /// Query:
        /// 
        /// FROM Product p
        /// RJOIN <CategoryProducts> (Category c)
        /// RJOIN <StoreProducts> (Store s)
        /// RJOIN <UserProducts> (User u)
        /// SELECT *
        /// </summary>
        public void GetAllProducts()
        {
            QueryBuilderMappingMetadata DataMap = Utils.GetMapping( "CMS_1.mapping" );
            QueryBuilderMappingMetadata DataMap2 = Utils.GetMapping( "CMS_2.mapping" );
            QueryBuilderMappingMetadata DataMap3 = Utils.GetMapping( "CMS_3.mapping" );
            QueryBuilderMappingMetadata DataMap4 = Utils.GetMapping( "CMS_4.mapping" );
            QueryBuilderMappingMetadata DataMap5 = Utils.GetMapping( "CMS_5.mapping" );

            string queryString = "from Product rjoin <CategoryProducts> (Category) rjoin <StoreProducts> (Store) rjoin <UserProducts> (User) select *";
            QueryGenerator queryGen = QueryBuilderParser.ParseQuery( queryString, DataMap );
            QueryGenerator queryGen2 = QueryBuilderParser.ParseQuery( queryString, DataMap2 );
            QueryGenerator queryGen3 = QueryBuilderParser.ParseQuery( queryString, DataMap3 );
            QueryGenerator queryGen4 = QueryBuilderParser.ParseQuery( queryString, DataMap4 );
            QueryGenerator queryGen5 = QueryBuilderParser.ParseQuery( queryString, DataMap5 );

            string Query;
            string Query2;
            string Query3;
            string Query4;
            string Query5;

            string HandcraftedQuery1;
            string HandcraftedQuery2;
            string HandcraftedQuery3;
            string HandcraftedQuery4;
            string HandcraftedQuery5;

            if ( UseDefaultQueryInsteadOfExplain )
            {
                Query = queryGen.Run();
                Query2 = queryGen2.Run();
                Query3 = queryGen3.Run();
                Query4 = queryGen4.Run();
                Query5 = queryGen5.Run();

                YCSBWorkloadFile workload = new YCSBWorkloadFile( "get_all_products_1", $"{DatabasePrefix}_1" );
                workload.SetProperty( "recordcount", CollectionDocumentCount[ "Product" ] );
                workload.SetProperty( "keys", string.Join( ",", ProductKeys ) );
                workload.ExportToFile();
                AddCommandFromWorkloadFile( workload );
                YCSBWorkloadFile workload2 = new YCSBWorkloadFile( "get_all_products_2", $"{DatabasePrefix}_3" );
                workload2.SetProperty( "recordcount", CollectionDocumentCount[ "Product" ] );
                workload2.SetProperty( "keys", string.Join( ",", ProductKeys ) );
                workload2.ExportToFile();
                AddCommandFromWorkloadFile( workload2 );
                YCSBWorkloadFile workload3 = new YCSBWorkloadFile( "get_all_products_3", $"{DatabasePrefix}_2" );
                workload3.SetProperty( "recordcount", CollectionDocumentCount[ "Product" ] );
                workload3.SetProperty( "keys", string.Join( ",", ProductKeys ) );
                workload3.ExportToFile();
                AddCommandFromWorkloadFile( workload3 );
                YCSBWorkloadFile workload4 = new YCSBWorkloadFile( "get_all_products_4", $"{DatabasePrefix}_4" );
                workload4.SetProperty( "recordcount", CollectionDocumentCount[ "Product" ] );
                workload4.SetProperty( "keys", string.Join( ",", ProductKeys ) );
                workload4.ExportToFile();
                AddCommandFromWorkloadFile( workload4 );
                YCSBWorkloadFile workload5 = new YCSBWorkloadFile( "get_all_products_5", $"{DatabasePrefix}_5" );
                workload5.SetProperty( "recordcount", CollectionDocumentCount[ "Product" ] );
                workload5.SetProperty( "keys", string.Join( ",", ProductKeys ) );
                workload5.ExportToFile();
                AddCommandFromWorkloadFile( workload5 );

                // Load Handcrafted queries
                HandcraftedQuery1 = Utils.ReadQueryFromFile( $"{BaseQueryFolder}/get_all_products_1.mongo" );
                HandcraftedQuery2 = Utils.ReadQueryFromFile( $"{BaseQueryFolder}/get_all_products_2.mongo" );
                HandcraftedQuery3 = Utils.ReadQueryFromFile( $"{BaseQueryFolder}/get_all_products_3.mongo" );
                HandcraftedQuery4 = Utils.ReadQueryFromFile( $"{BaseQueryFolder}/get_all_products_4.mongo" );
                HandcraftedQuery5 = Utils.ReadQueryFromFile( $"{BaseQueryFolder}/get_all_products_5.mongo" );

                YCSBWorkloadFile workloadHandcrafted1 = new YCSBWorkloadFile( "get_all_products_handcrafted_1", $"{DatabasePrefix}_1" );
                workloadHandcrafted1.SetProperty( "recordcount", CollectionDocumentCount[ "Product" ] );
                workloadHandcrafted1.SetProperty( "keys", string.Join( ",", ProductKeys ) );
                workloadHandcrafted1.ExportToFile();
                AddCommandFromWorkloadFile( workloadHandcrafted1 );
                YCSBWorkloadFile workloadHandcrafted2 = new YCSBWorkloadFile( "get_all_products_handcrafted_2", $"{DatabasePrefix}_3" );
                workloadHandcrafted2.SetProperty( "recordcount", CollectionDocumentCount[ "Product" ] );
                workloadHandcrafted2.SetProperty( "alternateparse", true );
                workloadHandcrafted2.SetProperty( "keys", string.Join( ",", ProductKeys ) );
                workloadHandcrafted2.ExportToFile();
                AddCommandFromWorkloadFile( workloadHandcrafted2 );
                YCSBWorkloadFile workloadHandcrafted3 = new YCSBWorkloadFile( "get_all_products_handcrafted_3", $"{DatabasePrefix}_2" );
                workloadHandcrafted3.SetProperty( "recordcount", CollectionDocumentCount[ "Product" ] );
                workloadHandcrafted3.SetProperty( "keys", string.Join( ",", ProductKeys ) );
                workloadHandcrafted3.ExportToFile();
                AddCommandFromWorkloadFile( workloadHandcrafted3 );
                YCSBWorkloadFile workloadHandcrafted4 = new YCSBWorkloadFile( "get_all_products_handcrafted_4", $"{DatabasePrefix}_4" );
                workloadHandcrafted4.SetProperty( "recordcount", CollectionDocumentCount[ "Product" ] );
                workloadHandcrafted4.SetProperty( "keys", string.Join( ",", ProductKeys ) );
                workloadHandcrafted4.ExportToFile();
                AddCommandFromWorkloadFile( workloadHandcrafted4 );
                YCSBWorkloadFile workloadHandcrafted5 = new YCSBWorkloadFile( "get_all_products_handcrafted_5", $"{DatabasePrefix}_5" );
                workloadHandcrafted5.SetProperty( "recordcount", CollectionDocumentCount[ "Product" ] );
                workloadHandcrafted5.SetProperty( "keys", string.Join( ",", ProductKeys ) );
                workloadHandcrafted5.ExportToFile();
                AddCommandFromWorkloadFile( workloadHandcrafted5 );
            }
            else
            {
                Query = queryGen.Explain();
                Query2 = queryGen2.Explain();
                Query3 = queryGen3.Explain();
                Query4 = queryGen4.Explain();
                Query5 = queryGen5.Explain();

                // Load Handcrafted queries
                HandcraftedQuery1 = Utils.ReadQueryFromFile( "HandcraftedQueries/get_all_products_1.mongo" );
                HandcraftedQuery2 = Utils.ReadQueryFromFile( "HandcraftedQueries/get_all_products_2.mongo" );
                HandcraftedQuery3 = Utils.ReadQueryFromFile( "HandcraftedQueries/get_all_products_3.mongo" );
                HandcraftedQuery4 = Utils.ReadQueryFromFile( "HandcraftedQueries/get_all_products_4.mongo" );
                HandcraftedQuery5 = Utils.ReadQueryFromFile( "HandcraftedQueries/get_all_products_5.mongo" );
            }

            RunIterationsForQuery( $"{DatabasePrefix}_1", "get_all_products_1", Query );
            RunIterationsForQuery( $"{DatabasePrefix}_3", "get_all_products_2", Query2 );
            RunIterationsForQuery( $"{DatabasePrefix}_2", "get_all_products_3", Query3 );
            RunIterationsForQuery( $"{DatabasePrefix}_4", "get_all_products_4", Query4 );
            RunIterationsForQuery( $"{DatabasePrefix}_5", "get_all_products_5", Query5 );

            RunIterationsForQuery( $"{DatabasePrefix}_1", "get_all_products_handcrafted_1", HandcraftedQuery1 );
            RunIterationsForQuery( $"{DatabasePrefix}_3", "get_all_products_handcrafted_2", HandcraftedQuery2, true );
            RunIterationsForQuery( $"{DatabasePrefix}_2", "get_all_products_handcrafted_3", HandcraftedQuery3 );
            RunIterationsForQuery( $"{DatabasePrefix}_4", "get_all_products_handcrafted_4", HandcraftedQuery4 );
            RunIterationsForQuery( $"{DatabasePrefix}_5", "get_all_products_handcrafted_5", HandcraftedQuery5 );
        }

        /// <summary>
        /// Get All Stores test
        /// 
        /// Query: FROM Store SELECT *
        /// </summary>
        public void GetAllStores()
        {
            QueryBuilderMappingMetadata DataMap = Utils.GetMapping( "CMS_1.mapping" );
            QueryBuilderMappingMetadata DataMap3 = Utils.GetMapping( "CMS_3.mapping" );
            QueryBuilderMappingMetadata DataMap5 = Utils.GetMapping( "CMS_5.mapping" );

            string queryString = "from Store select *";
            QueryGenerator queryGen1 = QueryBuilderParser.ParseQuery( queryString, DataMap );
            QueryGenerator queryGen3 = QueryBuilderParser.ParseQuery( queryString, DataMap3 );
            QueryGenerator queryGen5 = QueryBuilderParser.ParseQuery( queryString, DataMap5 );

            string QueryStringMap1;
            string QueryStringMap3;
            string QueryStringMap5;

            // Load Handcrafted queries
            string HandcraftedQuery1;
            string HandcraftedQuery2;
            string HandcraftedQuery3;

            if ( UseDefaultQueryInsteadOfExplain )
            {
                QueryStringMap1 = queryGen1.Run();
                QueryStringMap3 = queryGen3.Run();
                QueryStringMap5 = queryGen5.Run();

                YCSBWorkloadFile workload1 = new YCSBWorkloadFile( "get_all_stores_1", $"{DatabasePrefix}_1");
                workload1.SetProperty( "recordcount", CollectionDocumentCount[ "Store" ] );
                workload1.SetProperty( "keys", string.Join( ",", StoreKeys ) );
                workload1.ExportToFile();
                AddCommandFromWorkloadFile( workload1 );
                YCSBWorkloadFile workload3 = new YCSBWorkloadFile( "get_all_stores_3", $"{DatabasePrefix}_2");
                workload3.SetProperty( "recordcount", CollectionDocumentCount[ "Store" ] );
                workload3.SetProperty( "keys", string.Join( ",", StoreKeys ) );
                workload3.ExportToFile();
                AddCommandFromWorkloadFile( workload3 );
                YCSBWorkloadFile workload5 = new YCSBWorkloadFile( "get_all_stores_5", $"{DatabasePrefix}_5");
                workload5.SetProperty( "recordcount", CollectionDocumentCount[ "Store" ] );
                workload5.SetProperty( "keys", string.Join( ",", StoreKeys ) );
                workload5.ExportToFile();
                AddCommandFromWorkloadFile( workload5 );

                HandcraftedQuery1 = Utils.ReadQueryFromFile( $"{BaseQueryFolder}/get_all_stores_1.mongo" );
                HandcraftedQuery2 = Utils.ReadQueryFromFile( $"{BaseQueryFolder}/get_all_stores_3.mongo" );
                HandcraftedQuery3 = Utils.ReadQueryFromFile( $"{BaseQueryFolder}/get_all_stores_5.mongo" );

                YCSBWorkloadFile workloadHandcrafted1 = new YCSBWorkloadFile( "get_all_stores_handcrafted_1", $"{DatabasePrefix}_1" );
                workloadHandcrafted1.SetProperty( "recordcount", CollectionDocumentCount[ "Store" ] );
                workloadHandcrafted1.SetProperty( "alternateparse", true );
                workloadHandcrafted1.SetProperty( "keys", string.Join( ",", StoreKeys ) );
                workloadHandcrafted1.ExportToFile();
                AddCommandFromWorkloadFile( workloadHandcrafted1 );
                YCSBWorkloadFile workloadHandcrafted3 = new YCSBWorkloadFile( "get_all_stores_handcrafted_3", $"{DatabasePrefix}_2" );
                workloadHandcrafted3.SetProperty( "recordcount", CollectionDocumentCount[ "Store" ] );
                workloadHandcrafted3.SetProperty( "alternateparse", true );
                workloadHandcrafted3.SetProperty( "keys", string.Join( ",", StoreKeys ) );
                workloadHandcrafted3.ExportToFile();
                AddCommandFromWorkloadFile( workloadHandcrafted3 );
                YCSBWorkloadFile workloadHandcrafted5 = new YCSBWorkloadFile( "get_all_stores_handcrafted_5", $"{DatabasePrefix}_5" );
                workloadHandcrafted5.SetProperty( "recordcount", CollectionDocumentCount[ "Store" ] );
                workloadHandcrafted5.SetProperty( "alternateparse", true );
                workloadHandcrafted5.SetProperty( "keys", string.Join( ",", StoreKeys ) );
                workloadHandcrafted5.ExportToFile();
                AddCommandFromWorkloadFile( workloadHandcrafted5 );
            }
            else
            {
                QueryStringMap1 = queryGen1.Explain();
                QueryStringMap3 = queryGen3.Explain();
                QueryStringMap5 = queryGen5.Explain();

                HandcraftedQuery1 = Utils.ReadQueryFromFile( "HandcraftedQueries/get_all_stores_1.mongo" );
                HandcraftedQuery2 = Utils.ReadQueryFromFile( "HandcraftedQueries/get_all_stores_2.mongo" );
                HandcraftedQuery3 = Utils.ReadQueryFromFile( "HandcraftedQueries/get_all_stores_3.mongo" );
            }

            RunIterationsForQuery( $"{DatabasePrefix}_1", "get_all_stores_1", QueryStringMap1 );
            RunIterationsForQuery( $"{DatabasePrefix}_2", "get_all_stores_3", QueryStringMap3 );
            RunIterationsForQuery( $"{DatabasePrefix}_5", "get_all_stores_5", QueryStringMap5 );

            RunIterationsForQuery( $"{DatabasePrefix}_1", "get_all_stores_handcrafted_1", HandcraftedQuery1, true );
            RunIterationsForQuery( $"{DatabasePrefix}_2", "get_all_stores_handcrafted_3", HandcraftedQuery2, true );
            RunIterationsForQuery( $"{DatabasePrefix}_5", "get_all_stores_handcrafted_5", HandcraftedQuery3, true );
        }
        /// <summary>
        /// Run GetAllUsers test
        /// 
        /// Query: FROM User SELECT *
        /// </summary>
        public void GetAllUsers()
        {
            QueryBuilderMappingMetadata DataMap = Utils.GetMapping( "CMS_1.mapping" );
            QueryBuilderMappingMetadata DataMapCategoryDuplicated = Utils.GetMapping( "CMS_3.mapping" );
            QueryBuilderMappingMetadata DataMapStoreDuplicated = Utils.GetMapping( "CMS_4.mapping" );

            string queryString = "from User select *";

            QueryGenerator QueryGenMap1 = QueryBuilderParser.ParseQuery( queryString, DataMap );
            QueryGenerator QueryGenMap3 = QueryBuilderParser.ParseQuery( queryString, DataMapCategoryDuplicated );
            QueryGenerator QueryGenMap4 = QueryBuilderParser.ParseQuery( queryString, DataMapStoreDuplicated );

            string QueryStringMap1;
            string QueryStringMap3;
            string QueryStringMap4;

            string HandcraftedQuery1;
            string HandcraftedQuery3;
            string HandcraftedQuery4;

            if ( UseDefaultQueryInsteadOfExplain )
            {
                QueryStringMap1 = QueryGenMap1.Run();
                QueryStringMap3 = QueryGenMap3.Run();
                QueryStringMap4 = QueryGenMap4.Run();

                YCSBWorkloadFile workload1 = new YCSBWorkloadFile( "get_all_users_1", $"{DatabasePrefix}_1" );
                workload1.SetProperty( "recordcount", CollectionDocumentCount[ "User" ] );
                workload1.SetProperty( "keys", string.Join( ",", UserKeys ) );
                workload1.ExportToFile();
                AddCommandFromWorkloadFile( workload1 );
                YCSBWorkloadFile workload3 = new YCSBWorkloadFile( "get_all_users_3", $"{DatabasePrefix}_2" );
                workload3.SetProperty( "recordcount", CollectionDocumentCount[ "User" ] );
                workload3.SetProperty( "keys", string.Join( ",", UserKeys ) );
                workload3.ExportToFile();
                AddCommandFromWorkloadFile( workload3 );
                YCSBWorkloadFile workload4 = new YCSBWorkloadFile( "get_all_users_4", $"{DatabasePrefix}_4" );
                workload4.SetProperty( "recordcount", CollectionDocumentCount[ "User" ] );
                workload4.SetProperty( "keys", string.Join( ",", UserKeys ) );
                workload4.ExportToFile();
                AddCommandFromWorkloadFile( workload4 );

                // Load Handcrafted queries
                HandcraftedQuery1 = Utils.ReadQueryFromFile( $"{BaseQueryFolder}/get_all_users_1.mongo" );
                HandcraftedQuery3 = Utils.ReadQueryFromFile( $"{BaseQueryFolder}/get_all_users_3.mongo" );
                HandcraftedQuery4 = Utils.ReadQueryFromFile( $"{BaseQueryFolder}/get_all_users_4.mongo" );

                YCSBWorkloadFile workloadHandcrafted1 = new YCSBWorkloadFile( "get_all_users_handcrafted_1", $"{DatabasePrefix}_1");
                workloadHandcrafted1.SetProperty( "recordcount", CollectionDocumentCount[ "User" ] );
                workloadHandcrafted1.SetProperty( "alternateparse", true );
                workloadHandcrafted1.SetProperty( "keys", string.Join( ",", UserKeys ) );
                workloadHandcrafted1.ExportToFile();
                AddCommandFromWorkloadFile( workloadHandcrafted1 );
                YCSBWorkloadFile workloadHandcrafted3 = new YCSBWorkloadFile( "get_all_users_handcrafted_3", $"{DatabasePrefix}_2" );
                workloadHandcrafted3.SetProperty( "recordcount", CollectionDocumentCount[ "User" ] );
                workloadHandcrafted3.SetProperty( "alternateparse", true );
                workloadHandcrafted3.SetProperty( "keys", string.Join( ",", UserKeys ) );
                workloadHandcrafted3.ExportToFile();
                AddCommandFromWorkloadFile( workloadHandcrafted3 );
                YCSBWorkloadFile workloadHandcrafted4 = new YCSBWorkloadFile( "get_all_users_handcrafted_4", $"{DatabasePrefix}_4" );
                workloadHandcrafted4.SetProperty( "recordcount", CollectionDocumentCount[ "User" ] );
                workloadHandcrafted4.SetProperty( "alternateparse", true );
                workloadHandcrafted4.SetProperty( "keys", string.Join( ",", UserKeys ) );
                workloadHandcrafted4.ExportToFile();
                AddCommandFromWorkloadFile( workloadHandcrafted4 );
            }
            else
            {
                QueryStringMap1 = QueryGenMap1.Explain();
                QueryStringMap3 = QueryGenMap3.Explain();
                QueryStringMap4 = QueryGenMap4.Explain();

                // Load Handcrafted queries
                HandcraftedQuery1 = Utils.ReadQueryFromFile( "HandcraftedQueries/get_all_users_1.mongo" );
                HandcraftedQuery3 = Utils.ReadQueryFromFile( "HandcraftedQueries/get_all_users_2.mongo" );
                HandcraftedQuery4 = Utils.ReadQueryFromFile( "HandcraftedQueries/get_all_users_3.mongo" );
            }

            RunIterationsForQuery( $"{DatabasePrefix}_1", "get_all_users_1", QueryStringMap1 );
            RunIterationsForQuery( $"{DatabasePrefix}_2", "get_all_users_3", QueryStringMap3 );
            RunIterationsForQuery( $"{DatabasePrefix}_4", "get_all_users_4", QueryStringMap4 );

            RunIterationsForQuery( $"{DatabasePrefix}_1", "get_all_users_handcrafted_1", HandcraftedQuery1, true );
            RunIterationsForQuery( $"{DatabasePrefix}_3", "get_all_users_handcrafted_3", HandcraftedQuery3, true );
            RunIterationsForQuery( $"{DatabasePrefix}_4", "get_all_users_handcrafted_4", HandcraftedQuery4, true );
        }
        /// <summary>
        /// Run GetAllCategories Test
        /// 
        /// QUERY: FROM Category SELECT *
        /// </summary>
        public void GetAllCategories()
        {
            QueryBuilderMappingMetadata DataMap = Utils.GetMapping( "CMS_1.mapping" );
            QueryBuilderMappingMetadata DataMapStoreDuplicated = Utils.GetMapping( "CMS_4.mapping" );
            QueryBuilderMappingMetadata DataMapUserDuplicated = Utils.GetMapping( "CMS_5.mapping" );

            string queryString = "from Category select *";

            QueryGenerator QueryGenMap1 = QueryBuilderParser.ParseQuery( queryString, DataMap );
            QueryGenerator QueryGenMap4 = QueryBuilderParser.ParseQuery( queryString, DataMapStoreDuplicated );
            QueryGenerator QueryGenMap5 = QueryBuilderParser.ParseQuery( queryString, DataMapUserDuplicated );

            string QueryStringMap1;
            string QueryStringMap4;
            string QueryStringMap5;

            string HandcraftedQuery1;
            string HandcraftedQuery2;
            string HandcraftedQuery3;

            if ( UseDefaultQueryInsteadOfExplain )
            {
                QueryStringMap1 = QueryGenMap1.Run();
                QueryStringMap4 = QueryGenMap4.Run();
                QueryStringMap5 = QueryGenMap5.Run();

                YCSBWorkloadFile workload1 = new YCSBWorkloadFile( "get_all_categories_1", $"{DatabasePrefix}_1" );
                workload1.SetProperty( "recordcount", CollectionDocumentCount[ "Category" ] );
                workload1.SetProperty( "keys", string.Join( ",", CategoryKeys ) );
                workload1.ExportToFile();
                AddCommandFromWorkloadFile( workload1 );
                YCSBWorkloadFile workload4 = new YCSBWorkloadFile( "get_all_categories_4", $"{DatabasePrefix}_4" );
                workload4.SetProperty( "recordcount", CollectionDocumentCount[ "Category" ] );
                workload4.SetProperty( "keys", string.Join( ",", CategoryKeys ) );
                workload4.ExportToFile();
                AddCommandFromWorkloadFile( workload4 );
                YCSBWorkloadFile workload5 = new YCSBWorkloadFile( "get_all_categories_5", $"{DatabasePrefix}_5" );
                workload5.SetProperty( "recordcount", CollectionDocumentCount[ "Category" ] );
                workload5.SetProperty( "keys", string.Join( ",", CategoryKeys ) );
                workload5.ExportToFile();
                AddCommandFromWorkloadFile( workload5 );

                // Load Handcrafted queries
                HandcraftedQuery1 = Utils.ReadQueryFromFile( $"{BaseQueryFolder}/get_all_categories_1.mongo" );
                HandcraftedQuery2 = Utils.ReadQueryFromFile( $"{BaseQueryFolder}/get_all_categories_2.mongo" );
                HandcraftedQuery3 = Utils.ReadQueryFromFile( $"{BaseQueryFolder}/get_all_categories_3.mongo" );

                YCSBWorkloadFile workloadHandcrafted1 = new YCSBWorkloadFile( "get_all_categories_handcrafted_1", $"{DatabasePrefix}_1" );
                workloadHandcrafted1.SetProperty( "recordcount", CollectionDocumentCount[ "Category" ] );
                workloadHandcrafted1.SetProperty( "alternateparse", true );
                workloadHandcrafted1.SetProperty( "keys", string.Join( ",", CategoryKeys ) );
                workloadHandcrafted1.ExportToFile();
                AddCommandFromWorkloadFile( workloadHandcrafted1 );
                YCSBWorkloadFile workloadHandcrafted4 = new YCSBWorkloadFile( "get_all_categories_handcrafted_4", $"{DatabasePrefix}_4" );
                workloadHandcrafted4.SetProperty( "recordcount", CollectionDocumentCount[ "Category" ] );
                workloadHandcrafted4.SetProperty( "alternateparse", true );
                workloadHandcrafted4.SetProperty( "keys", string.Join( ",", CategoryKeys ) );
                workloadHandcrafted4.ExportToFile();
                AddCommandFromWorkloadFile( workloadHandcrafted4 );
                YCSBWorkloadFile workloadHandcrafted5 = new YCSBWorkloadFile( "get_all_categories_handcrafted_5", $"{DatabasePrefix}_5" );
                workloadHandcrafted5.SetProperty( "recordcount", CollectionDocumentCount[ "Category" ] );
                workloadHandcrafted5.SetProperty( "alternateparse", true );
                workloadHandcrafted5.SetProperty( "keys", string.Join( ",", CategoryKeys ) );
                workloadHandcrafted5.ExportToFile();
                AddCommandFromWorkloadFile( workloadHandcrafted5 );
            }
            else
            {
                QueryStringMap1 = QueryGenMap1.Explain();
                QueryStringMap4 = QueryGenMap4.Explain();
                QueryStringMap5 = QueryGenMap5.Explain();

                // Load Handcrafted queries
                HandcraftedQuery1 = Utils.ReadQueryFromFile( "HandcraftedQueries/get_all_categories_1.mongo" );
                HandcraftedQuery2 = Utils.ReadQueryFromFile( "HandcraftedQueries/get_all_categories_2.mongo" );
                HandcraftedQuery3 = Utils.ReadQueryFromFile( "HandcraftedQueries/get_all_categories_3.mongo" );
            }

            RunIterationsForQuery( $"{DatabasePrefix}_1", "get_all_categories_1", QueryStringMap1 );
            RunIterationsForQuery( $"{DatabasePrefix}_4", "get_all_categories_4", QueryStringMap4 );
            RunIterationsForQuery( $"{DatabasePrefix}_5", "get_all_categories_5", QueryStringMap5 );

            RunIterationsForQuery( $"{DatabasePrefix}_1", "get_all_categories_handcrafted_1", HandcraftedQuery1, true );
            RunIterationsForQuery( $"{DatabasePrefix}_4", "get_all_categories_handcrafted_4", HandcraftedQuery2, true );
            RunIterationsForQuery( $"{DatabasePrefix}_5", "get_all_categories_handcrafted_5", HandcraftedQuery3, true );
        }

        /// <summary>
        /// Run GetProductsFromStore query
        /// 
        /// QUERY: FROM Store 
        ///        RJOIN <StoreProducts> (Store)
        ///        SELECT *
        /// </summary>
        public void GetAllProductsFromStore()
        {
            QueryBuilderMappingMetadata DataMap = Utils.GetMapping( "CMS_1.mapping" );
            QueryBuilderMappingMetadata DataMapCategoryDuplicated = Utils.GetMapping( "CMS_3.mapping" );
            QueryBuilderMappingMetadata DataMapUserDuplicated = Utils.GetMapping( "CMS_5.mapping" );

            string queryString = "from Store rjoin <StoreProducts> (Product) select *";

            QueryGenerator GeneratorMap1 = QueryBuilderParser.ParseQuery( queryString, DataMap );
            QueryGenerator GeneratorMap3 = QueryBuilderParser.ParseQuery( queryString, DataMapCategoryDuplicated );
            QueryGenerator GeneratorMap5 = QueryBuilderParser.ParseQuery( queryString, DataMapUserDuplicated );

            string QueryMap1;
            string QueryMap3;
            string QueryMap5;

            string HandcraftedQuery1;
            string HandcraftedQuery3;
            string HandcraftedQuery5;

            if ( UseDefaultQueryInsteadOfExplain )
            {
                QueryMap1 = GeneratorMap1.Run();
                QueryMap3 = GeneratorMap3.Run();
                QueryMap5 = GeneratorMap5.Run();

                YCSBWorkloadFile workload1 = new YCSBWorkloadFile( "get_all_products_from_store_1", $"{DatabasePrefix}_1" );
                workload1.SetProperty( "recordcount", CollectionDocumentCount[ "Store" ] );
                workload1.SetProperty( "keys", string.Join( ",", StoreKeys ) );
                workload1.ExportToFile();
                AddCommandFromWorkloadFile( workload1 );
                YCSBWorkloadFile workload3 = new YCSBWorkloadFile( "get_all_products_from_store_3", $"{DatabasePrefix}_2" );
                workload3.SetProperty( "recordcount", CollectionDocumentCount[ "Store" ] );
                workload3.SetProperty( "keys", string.Join( ",", StoreKeys ) );
                workload3.ExportToFile();
                AddCommandFromWorkloadFile( workload3 );
                YCSBWorkloadFile workload5 = new YCSBWorkloadFile( "get_all_products_from_store_5", $"{DatabasePrefix}_5" );
                workload5.SetProperty( "recordcount", CollectionDocumentCount[ "Store" ] );
                workload5.SetProperty( "keys", string.Join( ",", StoreKeys ) );
                workload5.ExportToFile();
                AddCommandFromWorkloadFile( workload5 );

                // Load Handcrafted queries
                HandcraftedQuery1 = Utils.ReadQueryFromFile( $"{BaseQueryFolder}/get_all_products_from_store_1.mongo" );
                HandcraftedQuery3 = Utils.ReadQueryFromFile( $"{BaseQueryFolder}/get_all_products_from_store_2.mongo" );
                HandcraftedQuery5 = Utils.ReadQueryFromFile( $"{BaseQueryFolder}/get_all_products_from_store_3.mongo" );

                YCSBWorkloadFile workloadHandcrafted1 = new YCSBWorkloadFile( "get_all_products_from_store_handcrafted_1", $"{DatabasePrefix}_1" );
                workloadHandcrafted1.SetProperty( "recordcount", CollectionDocumentCount[ "Store" ] );
                workloadHandcrafted1.SetProperty( "keys", string.Join( ",", StoreKeys ) );
                workloadHandcrafted1.ExportToFile();
                AddCommandFromWorkloadFile( workloadHandcrafted1 );
                YCSBWorkloadFile workloadHandcrafted3 = new YCSBWorkloadFile( "get_all_products_from_store_handcrafted_3", $"{DatabasePrefix}_2" );
                workloadHandcrafted3.SetProperty( "recordcount", CollectionDocumentCount[ "Store" ] );
                workloadHandcrafted3.SetProperty( "keys", string.Join( ",", StoreKeys ) );
                workloadHandcrafted3.ExportToFile();
                AddCommandFromWorkloadFile( workloadHandcrafted3 );
                YCSBWorkloadFile workloadHandcrafted5 = new YCSBWorkloadFile( "get_all_products_from_store_handcrafted_5", $"{DatabasePrefix}_5" );
                workloadHandcrafted5.SetProperty( "recordcount", CollectionDocumentCount[ "Store" ] );
                workloadHandcrafted5.SetProperty( "keys", string.Join( ",", StoreKeys ) );
                workloadHandcrafted5.ExportToFile();
                AddCommandFromWorkloadFile( workloadHandcrafted5 );
            }
            else
            {
                QueryMap1 = GeneratorMap1.Explain();
                QueryMap3 = GeneratorMap3.Explain();
                QueryMap5 = GeneratorMap5.Explain();

                // Load Handcrafted queries
                HandcraftedQuery1 = Utils.ReadQueryFromFile( "HandcraftedQueries/get_all_products_from_store_1.mongo" );
                HandcraftedQuery3 = Utils.ReadQueryFromFile( "HandcraftedQueries/get_all_products_from_store_2.mongo" );
                HandcraftedQuery5 = Utils.ReadQueryFromFile( "HandcraftedQueries/get_all_products_from_store_3.mongo" );
            }

            RunIterationsForQuery( $"{DatabasePrefix}_1", "get_all_products_from_store_1", QueryMap1 );
            RunIterationsForQuery( $"{DatabasePrefix}_2", "get_all_products_from_store_3", QueryMap3 );
            RunIterationsForQuery( $"{DatabasePrefix}_5", "get_all_products_from_store_5", QueryMap5 );

            RunIterationsForQuery( $"{DatabasePrefix}_1", "get_all_products_from_store_handcrafted_1", HandcraftedQuery1 );
            RunIterationsForQuery( $"{DatabasePrefix}_2", "get_all_products_from_store_handcrafted_3", HandcraftedQuery3 );
            RunIterationsForQuery( $"{DatabasePrefix}_5", "get_all_products_from_store_handcrafted_5", HandcraftedQuery5 );
        }

        /// <summary>
        /// Run GetProductsFromCategory query
        /// 
        /// QUERY: FROM Category c
        ///        RJOIN <CategoryProducts> (Product)
        ///        SELECT *
        /// </summary>
        public void GetAllProductsFromCategory()
        {
            QueryBuilderMappingMetadata DataMap = Utils.GetMapping( "CMS_1.mapping" );
            QueryBuilderMappingMetadata DataMapStoreDuplicated = Utils.GetMapping( "CMS_4.mapping" );
            QueryBuilderMappingMetadata DataMapUserDuplicated = Utils.GetMapping( "CMS_5.mapping" );

            string queryString = "from Category rjoin <CategoryProducts> (Product) select *";

            QueryGenerator GeneratorMap1 = QueryBuilderParser.ParseQuery( queryString, DataMap );
            QueryGenerator GeneratorMap4 = QueryBuilderParser.ParseQuery( queryString, DataMapStoreDuplicated );
            QueryGenerator GeneratorMap5 = QueryBuilderParser.ParseQuery( queryString, DataMapUserDuplicated );

            string QueryMap1;
            string QueryMap4;
            string QueryMap5;

            string HandcraftedQuery1;
            string HandcraftedQuery4;
            string HandcraftedQuery5;

            if ( UseDefaultQueryInsteadOfExplain )
            {
                QueryMap1 = GeneratorMap1.Run();
                QueryMap4 = GeneratorMap4.Run();
                QueryMap5 = GeneratorMap5.Run();

                YCSBWorkloadFile workload1 = new YCSBWorkloadFile( "get_all_products_from_category_1", $"{DatabasePrefix}_1" );
                workload1.SetProperty( "recordcount", CollectionDocumentCount[ "Category" ] );
                workload1.SetProperty( "keys", string.Join( ",", CategoryKeys ) );
                workload1.ExportToFile();
                AddCommandFromWorkloadFile( workload1 );
                YCSBWorkloadFile workload4 = new YCSBWorkloadFile( "get_all_products_from_category_4", $"{DatabasePrefix}_4" );
                workload4.SetProperty( "recordcount", CollectionDocumentCount[ "Category" ] );
                workload4.SetProperty( "keys", string.Join( ",", CategoryKeys ) );
                workload4.ExportToFile();
                AddCommandFromWorkloadFile( workload4 );
                YCSBWorkloadFile workload5 = new YCSBWorkloadFile( "get_all_products_from_category_5", $"{DatabasePrefix}_5" );
                workload5.SetProperty( "recordcount", CollectionDocumentCount[ "Category" ] );
                workload5.SetProperty( "keys", string.Join( ",", CategoryKeys ) );
                workload5.ExportToFile();
                AddCommandFromWorkloadFile( workload5 );

                HandcraftedQuery1 = Utils.ReadQueryFromFile( $"{BaseQueryFolder}/get_all_products_from_category_1.mongo" );
                HandcraftedQuery4 = Utils.ReadQueryFromFile( $"{BaseQueryFolder}/get_all_products_from_category_2.mongo" );
                HandcraftedQuery5 = Utils.ReadQueryFromFile( $"{BaseQueryFolder}/get_all_products_from_category_3.mongo" );

                YCSBWorkloadFile workloadHandcrafted1 = new YCSBWorkloadFile( "get_all_products_from_category_handcrafted_1", $"{DatabasePrefix}_1" );
                workloadHandcrafted1.SetProperty( "recordcount", CollectionDocumentCount[ "Category" ] );
                workloadHandcrafted1.SetProperty( "keys", string.Join( ",", CategoryKeys ) );
                workloadHandcrafted1.ExportToFile();
                AddCommandFromWorkloadFile( workloadHandcrafted1 );
                YCSBWorkloadFile workloadHandcrafted4 = new YCSBWorkloadFile( "get_all_products_from_category_handcrafted_4", $"{DatabasePrefix}_4" );
                workloadHandcrafted4.SetProperty( "recordcount", CollectionDocumentCount[ "Category" ] );
                workloadHandcrafted4.SetProperty( "keys", string.Join( ",", CategoryKeys ) );
                workloadHandcrafted4.ExportToFile();
                AddCommandFromWorkloadFile( workloadHandcrafted4 );
                YCSBWorkloadFile workloadHandcrafted5 = new YCSBWorkloadFile( "get_all_products_from_category_handcrafted_5", $"{DatabasePrefix}_5" );
                workloadHandcrafted5.SetProperty( "recordcount", CollectionDocumentCount[ "Category" ] );
                workloadHandcrafted5.SetProperty( "keys", string.Join( ",", CategoryKeys ) );
                workloadHandcrafted5.ExportToFile();
                AddCommandFromWorkloadFile( workloadHandcrafted5 );
            }
            else
            {
                QueryMap1 = GeneratorMap1.Explain();
                QueryMap4 = GeneratorMap4.Explain();
                QueryMap5 = GeneratorMap5.Explain();

                HandcraftedQuery1 = Utils.ReadQueryFromFile( "HandcraftedQueries/get_all_products_from_category_1.mongo" );
                HandcraftedQuery4 = Utils.ReadQueryFromFile( "HandcraftedQueries/get_all_products_from_category_2.mongo" );
                HandcraftedQuery5 = Utils.ReadQueryFromFile( "HandcraftedQueries/get_all_products_from_category_3.mongo" );
            }

            RunIterationsForQuery( $"{DatabasePrefix}_1", "get_all_products_from_category_1", QueryMap1 );
            RunIterationsForQuery( $"{DatabasePrefix}_4", "get_all_products_from_category_4", QueryMap4 );
            RunIterationsForQuery( $"{DatabasePrefix}_5", "get_all_products_from_category_5", QueryMap5 );

            RunIterationsForQuery( $"{DatabasePrefix}_1", "get_all_products_from_category_handcrafted_1", HandcraftedQuery1 );
            RunIterationsForQuery( $"{DatabasePrefix}_4", "get_all_products_from_category_handcrafted_4", HandcraftedQuery4 );
            RunIterationsForQuery( $"{DatabasePrefix}_5", "get_all_products_from_category_handcrafted_5", HandcraftedQuery5 );
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
 
        //public void GetAllProductsFromUser()
        //{
        //    QueryBuilderMappingMetadata DataMap = Utils.GetMapping( "CMS_1.mapping" );
        //    QueryBuilderMappingMetadata DataMapCategoryDuplicated = Utils.GetMapping( "CMS_3.mapping" );
        //    QueryBuilderMappingMetadata DataMapStoreDuplicated = Utils.GetMapping( "CMS_4.mapping" );

        //    QueryableEntity User = new QueryableEntity( DataMap.EntityRelationshipModel.FindByName( "User" ) );
        //    QueryableEntity Product = new QueryableEntity( DataMap.EntityRelationshipModel.FindByName( "Product" ) );

        //    RelationshipJoinOperator RJoinOp1 = new RelationshipJoinOperator( User, (Relationship)DataMap.EntityRelationshipModel.FindByName( "UserProducts" ),
        //        new List<QueryableEntity>() { Product }, DataMap.ERMongoMapping );

        //    RelationshipJoinOperator RJoinOp3 = new RelationshipJoinOperator( User, (Relationship)DataMap.EntityRelationshipModel.FindByName( "UserProducts" ),
        //        new List<QueryableEntity>() { Product }, DataMapCategoryDuplicated.ERMongoMapping );

        //    RelationshipJoinOperator RJoinOp4 = new RelationshipJoinOperator( User, (Relationship)DataMap.EntityRelationshipModel.FindByName( "UserProducts" ),
        //        new List<QueryableEntity>() { Product }, DataMapStoreDuplicated.ERMongoMapping );

        //    SortArgument SortArg = new SortArgument( User, User.GetAttribute( "UserID" ), MongoDBSort.Ascending );

        //    SortStage SortOpMap1 = new SortStage( new List<SortArgument>() { SortArg }, DataMap.ERMongoMapping );
        //    SortStage SortOpMap3 = new SortStage( new List<SortArgument>() { SortArg }, DataMapCategoryDuplicated.ERMongoMapping );
        //    SortStage SortOpMap4 = new SortStage( new List<SortArgument>() { SortArg }, DataMapStoreDuplicated.ERMongoMapping );

        //    List<AlgebraOperator> OperatorsToExecuteMap1 = new List<AlgebraOperator>() { RJoinOp1, SortOpMap1 };
        //    List<AlgebraOperator> OperatorsToExecuteMap3 = new List<AlgebraOperator>() { RJoinOp3, SortOpMap3 };
        //    List<AlgebraOperator> OperatorsToExecuteMap4 = new List<AlgebraOperator>() { RJoinOp4, SortOpMap4 };

        //    FromArgument StartArgMap1 = new FromArgument( User, DataMap.ERMongoMapping );
        //    FromArgument StartArgMap3 = new FromArgument( User, DataMapCategoryDuplicated.ERMongoMapping );
        //    FromArgument StartArgMap4 = new FromArgument( User, DataMapStoreDuplicated.ERMongoMapping );

        //    QueryGenerator GeneratorMap1 = new QueryGenerator( StartArgMap1, OperatorsToExecuteMap1 );
        //    QueryGenerator GeneratorMap3 = new QueryGenerator( StartArgMap3, OperatorsToExecuteMap3 );
        //    QueryGenerator GeneratorMap4 = new QueryGenerator( StartArgMap4, OperatorsToExecuteMap4 );

        //    string QueryMap1;
        //    string QueryMap3;
        //    string QueryMap4;

        //    if ( UseDefaultQueryInsteadOfExplain )
        //    {
        //        QueryMap1 = GeneratorMap1.Run();
        //        QueryMap3 = GeneratorMap3.Run();
        //        QueryMap4 = GeneratorMap4.Run();
        //    }
        //    else
        //    {
        //        QueryMap1 = GeneratorMap1.Explain();
        //        QueryMap3 = GeneratorMap3.Explain();
        //        QueryMap4 = GeneratorMap4.Explain();
        //    }

        //    //QueryRunner RunnerMap1 = new QueryRunner( "mongodb://localhost:27017", "{DatabasePrefix}_index_1" );
        //    //QueryRunner RunnerMap3 = new QueryRunner( "mongodb://localhost:27017", "{DatabasePrefix}_index_2" );
        //    //QueryRunner RunnerMap4 = new QueryRunner( "mongodb://localhost:27017", "{DatabasePrefix}_index_4" );

        //    //List<QueryStats> Stats1 = new List<QueryStats>();
        //    //for ( int i = 0; i < 100; i++ )
        //    //{
        //    //    QueryStats iterationResult = RunnerMap1.GetExplainResult( QueryMap1 );
        //    //    Stats1.Add( iterationResult );
        //    //}

        //    //// Drop before saving new
        //    //MongoContext.DropCollection( "get_all_products_from_user_query_1" );
        //    //// Save all
        //    //MongoContext.InsertManyRecords( "get_all_products_from_user_query_1", Stats1 );

        //    //List<QueryStats> Stats2 = new List<QueryStats>();
        //    //for ( int i = 0; i < 100; i++ )
        //    //{
        //    //    QueryStats iterationResult = RunnerMap3.GetExplainResult( QueryMap3 );
        //    //    Stats2.Add( iterationResult );
        //    //}

        //    //// Drop before saving new
        //    //MongoContext.DropCollection( "get_all_products_from_user_query_2" );
        //    //// Save all
        //    //MongoContext.InsertManyRecords( "get_all_products_from_user_query_2", Stats2 );

        //    //List<QueryStats> Stats3 = new List<QueryStats>();
        //    //for ( int i = 0; i < 100; i++ )
        //    //{
        //    //    QueryStats iterationResult = RunnerMap4.GetExplainResult( QueryMap4 );
        //    //    Stats3.Add( iterationResult );
        //    //}

        //    //// Drop before saving new
        //    //MongoContext.DropCollection( "get_all_products_from_user_query_3" );
        //    //// Save all
        //    //MongoContext.InsertManyRecords( "get_all_products_from_user_query_3", Stats3 );
        //}
        /// <summary>
        /// Run GetAllProductsFromCategoryWithStore
        /// 
        /// QUERY: FROM Category
        ///        RJOIN <CategoryProducts> (Product RJOIN <StoreProducts> (Store))
        ///        SELECT *
        /// </summary>
        public void GetAllProductsFromCategoryWithStore()
        {
            QueryBuilderMappingMetadata DataMap = Utils.GetMapping( "CMS_1.mapping" );
            QueryBuilderMappingMetadata DataMapStoreDuplicated = Utils.GetMapping( "CMS_4.mapping" );
            QueryBuilderMappingMetadata DataMapUserDuplicated = Utils.GetMapping( "CMS_5.mapping" );

            string queryString = "from Category rjoin <CategoryProducts> (Product rjoin <StoreProducts> (Store)) select *";

            QueryGenerator GeneratorMap1 = QueryBuilderParser.ParseQuery( queryString, DataMap );
            QueryGenerator GeneratorMap4 = QueryBuilderParser.ParseQuery( queryString, DataMapStoreDuplicated );
            QueryGenerator GeneratorMap5 = QueryBuilderParser.ParseQuery( queryString, DataMapUserDuplicated );

            string QueryMap1;
            string QueryMap4;
            string QueryMap5;

            string HandcraftedQuery1;
            string HandcraftedQuery4;
            string HandcraftedQuery5;

            if ( UseDefaultQueryInsteadOfExplain )
            {
                QueryMap1 = GeneratorMap1.Run();
                QueryMap4 = GeneratorMap4.Run();
                QueryMap5 = GeneratorMap5.Run();

                YCSBWorkloadFile workload1 = new YCSBWorkloadFile( "get_all_products_from_category_with_store_1", $"{DatabasePrefix}_1" );
                workload1.SetProperty( "recordcount", CollectionDocumentCount[ "Category" ] );
                workload1.SetProperty( "keys", string.Join( ",", CategoryKeys ) );
                workload1.ExportToFile();
                AddCommandFromWorkloadFile( workload1 );
                YCSBWorkloadFile workload4 = new YCSBWorkloadFile( "get_all_products_from_category_with_store_4", $"{DatabasePrefix}_4" );
                workload4.SetProperty( "recordcount", CollectionDocumentCount[ "Category" ] );
                workload4.SetProperty( "keys", string.Join( ",", CategoryKeys ) );
                workload4.ExportToFile();
                AddCommandFromWorkloadFile( workload4 );
                YCSBWorkloadFile workload5 = new YCSBWorkloadFile( "get_all_products_from_category_with_store_5", $"{DatabasePrefix}_5" );
                workload5.SetProperty( "recordcount", CollectionDocumentCount[ "Category" ] );
                workload5.SetProperty( "keys", string.Join( ",", CategoryKeys ) );
                workload5.ExportToFile();
                AddCommandFromWorkloadFile( workload5 );

                // Load Handcrafted queries
                HandcraftedQuery1 = Utils.ReadQueryFromFile( $"{BaseQueryFolder}/get_all_products_from_category_with_store_1.mongo" );
                HandcraftedQuery4 = Utils.ReadQueryFromFile( $"{BaseQueryFolder}/get_all_products_from_category_with_store_2.mongo" );
                HandcraftedQuery5 = Utils.ReadQueryFromFile( $"{BaseQueryFolder}/get_all_products_from_category_with_store_3.mongo" );

                YCSBWorkloadFile workloadHandcrafted1 = new YCSBWorkloadFile( "get_all_products_from_category_with_store_handcrafted_1", $"{DatabasePrefix}_1" );
                workloadHandcrafted1.SetProperty( "recordcount", CollectionDocumentCount[ "Category" ] );
                workloadHandcrafted1.SetProperty( "keys", string.Join( ",", CategoryKeys ) );
                workloadHandcrafted1.ExportToFile();
                AddCommandFromWorkloadFile( workloadHandcrafted1 );
                YCSBWorkloadFile workloadHandcrafted4 = new YCSBWorkloadFile( "get_all_products_from_category_with_store_handcrafted_4", $"{DatabasePrefix}_4" );
                workloadHandcrafted4.SetProperty( "recordcount", CollectionDocumentCount[ "Category" ] );
                workloadHandcrafted4.SetProperty( "keys", string.Join( ",", CategoryKeys ) );
                workloadHandcrafted4.ExportToFile();
                AddCommandFromWorkloadFile( workloadHandcrafted4 );
                YCSBWorkloadFile workloadHandcrafted5 = new YCSBWorkloadFile( "get_all_products_from_category_with_store_handcrafted_5", $"{DatabasePrefix}_5" );
                workloadHandcrafted5.SetProperty( "recordcount", CollectionDocumentCount[ "Category" ] );
                workloadHandcrafted5.SetProperty( "keys", string.Join( ",", CategoryKeys ) );
                workloadHandcrafted5.ExportToFile();
                AddCommandFromWorkloadFile( workloadHandcrafted5 );
            }
            else
            {
                QueryMap1 = GeneratorMap1.Explain();
                QueryMap4 = GeneratorMap4.Explain();
                QueryMap5 = GeneratorMap5.Explain();

                // Load Handcrafted queries
                HandcraftedQuery1 = Utils.ReadQueryFromFile( "HandcraftedQueries/get_all_products_from_category_with_store_1.mongo" );
                HandcraftedQuery4 = Utils.ReadQueryFromFile( "HandcraftedQueries/get_all_products_from_category_with_store_2.mongo" );
                HandcraftedQuery5 = Utils.ReadQueryFromFile( "HandcraftedQueries/get_all_products_from_category_with_store_3.mongo" );
            }

            RunIterationsForQuery( $"{DatabasePrefix}_1", "get_all_products_from_category_with_store_1", QueryMap1 );
            RunIterationsForQuery( $"{DatabasePrefix}_4", "get_all_products_from_category_with_store_4", QueryMap4 );
            RunIterationsForQuery( $"{DatabasePrefix}_5", "get_all_products_from_category_with_store_5", QueryMap5 );

            RunIterationsForQuery( $"{DatabasePrefix}_1", "get_all_products_from_category_with_store_handcrafted_1", HandcraftedQuery1 );
            RunIterationsForQuery( $"{DatabasePrefix}_4", "get_all_products_from_category_with_store_handcrafted_4", HandcraftedQuery4 );
            RunIterationsForQuery( $"{DatabasePrefix}_5", "get_all_products_from_category_with_store_handcrafted_5", HandcraftedQuery5 );
        }

        /// <summary>
        /// Run GetAllProductsFromCategoryWithUser
        /// 
        /// QUERY: FROM Category
        ///        RJOIN <CategoryProducts> (Product RJOIN <UserProducts> (User))
        ///        SELECT *
        /// </summary>
        public void GetAllProductsFromCategoryWithUser()
        {
            QueryBuilderMappingMetadata DataMap = Utils.GetMapping( "CMS_1.mapping" );
            QueryBuilderMappingMetadata DataMapStoreDuplicated = Utils.GetMapping( "CMS_4.mapping" );
            QueryBuilderMappingMetadata DataMapUserDuplicated = Utils.GetMapping( "CMS_5.mapping" );

            string queryString = "from Category rjoin <CategoryProducts> (Product rjoin <UserProducts> (User)) select *";

            QueryGenerator GeneratorMap1 = QueryBuilderParser.ParseQuery( queryString, DataMap );
            QueryGenerator GeneratorMap4 = QueryBuilderParser.ParseQuery( queryString, DataMapStoreDuplicated );
            QueryGenerator GeneratorMap5 = QueryBuilderParser.ParseQuery( queryString, DataMapUserDuplicated );

            string QueryMap1;
            string QueryMap4;
            string QueryMap5;

            string HandcraftedQuery1;
            string HandcraftedQuery4;
            string HandcraftedQuery5;

            if ( UseDefaultQueryInsteadOfExplain )
            {
                QueryMap1 = GeneratorMap1.Run();
                QueryMap4 = GeneratorMap4.Run();
                QueryMap5 = GeneratorMap5.Run();

                YCSBWorkloadFile workload1 = new YCSBWorkloadFile( "get_all_products_from_category_with_user_1", $"{DatabasePrefix}_1" );
                workload1.SetProperty( "recordcount", CollectionDocumentCount[ "Category" ] );
                workload1.SetProperty( "keys", string.Join( ",", CategoryKeys ) );
                workload1.ExportToFile();
                AddCommandFromWorkloadFile( workload1 );
                YCSBWorkloadFile workload4 = new YCSBWorkloadFile( "get_all_products_from_category_with_user_4", $"{DatabasePrefix}_4" );
                workload4.SetProperty( "recordcount", CollectionDocumentCount[ "Category" ] );
                workload4.SetProperty( "keys", string.Join( ",", CategoryKeys ) );
                workload4.ExportToFile();
                AddCommandFromWorkloadFile( workload4 );
                YCSBWorkloadFile workload5 = new YCSBWorkloadFile( "get_all_products_from_category_with_user_5", $"{DatabasePrefix}_5" );
                workload5.SetProperty( "recordcount", CollectionDocumentCount[ "Category" ] );
                workload5.SetProperty( "keys", string.Join( ",", CategoryKeys ) );
                workload5.ExportToFile();
                AddCommandFromWorkloadFile( workload5 );

                HandcraftedQuery1 = Utils.ReadQueryFromFile( $"{BaseQueryFolder}/get_all_products_from_category_with_user_1.mongo" );
                HandcraftedQuery4 = Utils.ReadQueryFromFile( $"{BaseQueryFolder}/get_all_products_from_category_with_user_4.mongo" );
                HandcraftedQuery5 = Utils.ReadQueryFromFile( $"{BaseQueryFolder}/get_all_products_from_category_with_user_5.mongo" );

                YCSBWorkloadFile workloadHandcrafted1 = new YCSBWorkloadFile( "get_all_products_from_category_with_user_handcrafted_1", $"{DatabasePrefix}_1" );
                workloadHandcrafted1.SetProperty( "recordcount", CollectionDocumentCount[ "Category" ] );
                workloadHandcrafted1.SetProperty( "keys", string.Join( ",", CategoryKeys ) );
                workloadHandcrafted1.ExportToFile();
                AddCommandFromWorkloadFile( workloadHandcrafted1 );
                YCSBWorkloadFile workloadHandcrafted4 = new YCSBWorkloadFile( "get_all_products_from_category_with_user_handcrafted_4", $"{DatabasePrefix}_4" );
                workloadHandcrafted4.SetProperty( "recordcount", CollectionDocumentCount[ "Category" ] );
                workloadHandcrafted4.SetProperty( "keys", string.Join( ",", CategoryKeys ) );
                workloadHandcrafted4.ExportToFile();
                AddCommandFromWorkloadFile( workloadHandcrafted4 );
                YCSBWorkloadFile workloadHandcrafted5 = new YCSBWorkloadFile( "get_all_products_from_category_with_user_handcrafted_5", $"{DatabasePrefix}_5" );
                workloadHandcrafted5.SetProperty( "recordcount", CollectionDocumentCount[ "Category" ] );
                workloadHandcrafted5.SetProperty( "keys", string.Join( ",", CategoryKeys ) );
                workloadHandcrafted5.ExportToFile();
                AddCommandFromWorkloadFile( workloadHandcrafted5 );
            }
            else
            {
                QueryMap1 = GeneratorMap1.Explain();
                QueryMap4 = GeneratorMap4.Explain();
                QueryMap5 = GeneratorMap5.Explain();

                HandcraftedQuery1 = Utils.ReadQueryFromFile( "HandcraftedQueries/get_all_products_from_category_with_user_1.mongo" );
                HandcraftedQuery4 = Utils.ReadQueryFromFile( "HandcraftedQueries/get_all_products_from_category_with_user_2.mongo" );
                HandcraftedQuery5 = Utils.ReadQueryFromFile( "HandcraftedQueries/get_all_products_from_category_with_user_3.mongo" );
            }

            RunIterationsForQuery( $"{DatabasePrefix}_1", "get_all_products_from_category_with_user_1", QueryMap1 );
            RunIterationsForQuery( $"{DatabasePrefix}_4", "get_all_products_from_category_with_user_4", QueryMap4 );
            RunIterationsForQuery( $"{DatabasePrefix}_5", "get_all_products_from_category_with_user_5", QueryMap5 );

            RunIterationsForQuery( $"{DatabasePrefix}_1", "get_all_products_from_category_with_user_handcrafted_1", HandcraftedQuery1 );
            RunIterationsForQuery( $"{DatabasePrefix}_4", "get_all_products_from_category_with_user_handcrafted_4", HandcraftedQuery4 );
            RunIterationsForQuery( $"{DatabasePrefix}_5", "get_all_products_from_category_with_user_handcrafted_5", HandcraftedQuery5 );
        }
        /// <summary>
        /// Run GetProductTitleAndUserName test
        /// 
        /// QUERY: FROM Product
        ///        RJOIN <UserProducts> (User)
        ///        SELECT Product.Title, User.UserName
        /// </summary>
        public void GetProductTitleAndUserName()
        {
            QueryBuilderMappingMetadata DataMap = Utils.GetMapping( "CMS_1.mapping" );
            QueryBuilderMappingMetadata DataMapDuplicates = Utils.GetMapping( "CMS_2.mapping" );
            QueryBuilderMappingMetadata DataMapCategoryDuplicated = Utils.GetMapping( "CMS_3.mapping" );
            QueryBuilderMappingMetadata DataMapStoreDuplicated = Utils.GetMapping( "CMS_4.mapping" );
            QueryBuilderMappingMetadata DataMapUserDuplicated = Utils.GetMapping( "CMS_5.mapping" );

            string queryString = "from Product rjoin <UserProducts> (User) select Product.Title, User.UserName";

            QueryGenerator GeneratorMap1 = QueryBuilderParser.ParseQuery( queryString, DataMap );
            QueryGenerator GeneratorMap2 = QueryBuilderParser.ParseQuery( queryString, DataMapDuplicates );
            QueryGenerator GeneratorMap3 = QueryBuilderParser.ParseQuery( queryString, DataMapCategoryDuplicated );
            QueryGenerator GeneratorMap4 = QueryBuilderParser.ParseQuery( queryString, DataMapStoreDuplicated );
            QueryGenerator GeneratorMap5 = QueryBuilderParser.ParseQuery( queryString, DataMapUserDuplicated );

            string QueryMap1;
            string QueryMap2;
            string QueryMap3;
            string QueryMap4;
            string QueryMap5;

            string HandcraftedQuery1;
            string HandcraftedQuery2;
            string HandcraftedQuery3;
            string HandcraftedQuery4;
            string HandcraftedQuery5;

            if ( UseDefaultQueryInsteadOfExplain )
            {
                QueryMap1 = GeneratorMap1.Run();
                QueryMap2 = GeneratorMap2.Run();
                QueryMap3 = GeneratorMap3.Run();
                QueryMap4 = GeneratorMap4.Run();
                QueryMap5 = GeneratorMap5.Run();

                YCSBWorkloadFile workload1 = new YCSBWorkloadFile( "get_product_title_and_username_1", $"{DatabasePrefix}_1" );
                workload1.SetProperty( "recordcount", CollectionDocumentCount[ "Product" ] );
                workload1.SetProperty( "keys", string.Join( ",", ProductKeys ) );
                workload1.ExportToFile();
                AddCommandFromWorkloadFile( workload1 );
                YCSBWorkloadFile workload2 = new YCSBWorkloadFile( "get_product_title_and_username_2", $"{DatabasePrefix}_3" );
                workload2.SetProperty( "recordcount", CollectionDocumentCount[ "Product" ] );
                workload2.SetProperty( "keys", string.Join( ",", ProductKeys ) );
                workload2.ExportToFile();
                AddCommandFromWorkloadFile( workload2 );
                YCSBWorkloadFile workload3 = new YCSBWorkloadFile( "get_product_title_and_username_3", $"{DatabasePrefix}_2" );
                workload3.SetProperty( "recordcount", CollectionDocumentCount[ "Product" ] );
                workload3.SetProperty( "keys", string.Join( ",", ProductKeys ) );
                workload3.ExportToFile();
                AddCommandFromWorkloadFile( workload3 );
                YCSBWorkloadFile workload4 = new YCSBWorkloadFile( "get_product_title_and_username_4", $"{DatabasePrefix}_4" );
                workload4.SetProperty( "recordcount", CollectionDocumentCount[ "Product" ] );
                workload4.SetProperty( "keys", string.Join( ",", ProductKeys ) );
                workload4.ExportToFile();
                AddCommandFromWorkloadFile( workload4 );
                YCSBWorkloadFile workload5 = new YCSBWorkloadFile( "get_product_title_and_username_5", $"{DatabasePrefix}_5" );
                workload5.SetProperty( "recordcount", CollectionDocumentCount[ "Product" ] );
                workload5.SetProperty( "keys", string.Join( ",", ProductKeys ) );
                workload5.ExportToFile();
                AddCommandFromWorkloadFile( workload5 );

                HandcraftedQuery1 = Utils.ReadQueryFromFile( $"{BaseQueryFolder}/get_product_title_and_username_1.mongo" );
                HandcraftedQuery2 = Utils.ReadQueryFromFile( $"{BaseQueryFolder}/get_product_title_and_username_2.mongo" );
                HandcraftedQuery3 = Utils.ReadQueryFromFile( $"{BaseQueryFolder}/get_product_title_and_username_3.mongo" );
                HandcraftedQuery4 = Utils.ReadQueryFromFile( $"{BaseQueryFolder}/get_product_title_and_username_4.mongo" );
                HandcraftedQuery5 = Utils.ReadQueryFromFile( $"{BaseQueryFolder}/get_product_title_and_username_5.mongo" );

                YCSBWorkloadFile workloadHandcrafted1 = new YCSBWorkloadFile( "get_product_title_and_username_handcrafted_1", $"{DatabasePrefix}_1" );
                workloadHandcrafted1.SetProperty( "recordcount", CollectionDocumentCount[ "Product" ] );
                workloadHandcrafted1.SetProperty( "keys", string.Join( ",", ProductKeys ) );
                workloadHandcrafted1.ExportToFile();
                AddCommandFromWorkloadFile( workloadHandcrafted1 );
                YCSBWorkloadFile workloadHandcrafted2 = new YCSBWorkloadFile( "get_product_title_and_username_handcrafted_2", $"{DatabasePrefix}_3" );
                workloadHandcrafted2.SetProperty( "recordcount", CollectionDocumentCount[ "Product" ] );
                workloadHandcrafted2.SetProperty( "keys", string.Join( ",", ProductKeys ) );
                workloadHandcrafted2.ExportToFile();
                AddCommandFromWorkloadFile( workloadHandcrafted2 );
                YCSBWorkloadFile workloadHandcrafted3 = new YCSBWorkloadFile( "get_product_title_and_username_handcrafted_3", $"{DatabasePrefix}_2" );
                workloadHandcrafted3.SetProperty( "recordcount", CollectionDocumentCount[ "Product" ] );
                workloadHandcrafted3.SetProperty( "keys", string.Join( ",", ProductKeys ) );
                workloadHandcrafted3.ExportToFile();
                AddCommandFromWorkloadFile( workloadHandcrafted3 );
                YCSBWorkloadFile workloadHandcrafted4 = new YCSBWorkloadFile( "get_product_title_and_username_handcrafted_4", $"{DatabasePrefix}_4" );
                workloadHandcrafted4.SetProperty( "recordcount", CollectionDocumentCount[ "Product" ] );
                workloadHandcrafted4.SetProperty( "keys", string.Join( ",", ProductKeys ) );
                workloadHandcrafted4.ExportToFile();
                AddCommandFromWorkloadFile( workloadHandcrafted4 );
                YCSBWorkloadFile workloadHandcrafted5 = new YCSBWorkloadFile( "get_product_title_and_username_handcrafted_5", $"{DatabasePrefix}_5" );
                workloadHandcrafted5.SetProperty( "recordcount", CollectionDocumentCount[ "Product" ] );
                workloadHandcrafted5.SetProperty( "keys", string.Join( ",", ProductKeys ) );
                workloadHandcrafted5.ExportToFile();
                AddCommandFromWorkloadFile( workloadHandcrafted5 );
            }
            else
            {
                QueryMap1 = GeneratorMap1.Explain();
                QueryMap2 = GeneratorMap2.Explain();
                QueryMap3 = GeneratorMap3.Explain();
                QueryMap4 = GeneratorMap4.Explain();
                QueryMap5 = GeneratorMap5.Explain();

                HandcraftedQuery1 = Utils.ReadQueryFromFile( "HandcraftedQueries/get_product_title_and_username_1.mongo" );
                HandcraftedQuery2 = Utils.ReadQueryFromFile( "HandcraftedQueries/get_product_title_and_username_2.mongo" );
                HandcraftedQuery3 = Utils.ReadQueryFromFile( "HandcraftedQueries/get_product_title_and_username_3.mongo" );
                HandcraftedQuery4 = Utils.ReadQueryFromFile( "HandcraftedQueries/get_product_title_and_username_4.mongo" );
                HandcraftedQuery5 = Utils.ReadQueryFromFile( "HandcraftedQueries/get_product_title_and_username_5.mongo" );
            }

            RunIterationsForQuery( $"{DatabasePrefix}_1", "get_product_title_and_username_1", QueryMap1 );
            RunIterationsForQuery( $"{DatabasePrefix}_3", "get_product_title_and_username_2", QueryMap2 );
            RunIterationsForQuery( $"{DatabasePrefix}_2", "get_product_title_and_username_3", QueryMap3 );
            RunIterationsForQuery( $"{DatabasePrefix}_4", "get_product_title_and_username_4", QueryMap4 );
            RunIterationsForQuery( $"{DatabasePrefix}_5", "get_product_title_and_username_5", QueryMap5 );

            RunIterationsForQuery( $"{DatabasePrefix}_1", "get_product_title_and_username_handcrafted_1", HandcraftedQuery1 );
            RunIterationsForQuery( $"{DatabasePrefix}_3", "get_product_title_and_username_handcrafted_2", HandcraftedQuery2 );
            RunIterationsForQuery( $"{DatabasePrefix}_2", "get_product_title_and_username_handcrafted_3", HandcraftedQuery3 );
            RunIterationsForQuery( $"{DatabasePrefix}_4", "get_product_title_and_username_handcrafted_4", HandcraftedQuery4 );
            RunIterationsForQuery( $"{DatabasePrefix}_5", "get_product_title_and_username_handcrafted_5", HandcraftedQuery5 );
        }

        /// <summary>
        /// Run GetAllProductsFromCategoryWithUserAndSelectOnlyTitleNameEmailCategoryName
        /// 
        /// QUERY: FROM Category
        ///        RJOIN <CategoryProducts> (Product RJOIN <UserProducts> (User))
        ///        SELECT Category.CategoryName, Product.Title, User.UserName, User.UserEmail
        /// </summary>
        public void GetAllProductsFromCategoryWithUserAndSelectOnlyTitleNameEmailCategoryName()
        {
            QueryBuilderMappingMetadata DataMap = Utils.GetMapping( "CMS_1.mapping" );
            QueryBuilderMappingMetadata DataMapStoreDuplicated = Utils.GetMapping( "CMS_4.mapping" );
            QueryBuilderMappingMetadata DataMapUserDuplicated = Utils.GetMapping( "CMS_5.mapping" );

            string queryString = "from Category rjoin <CategoryProducts> (Product rjoin <UserProducts> (User)) select Category.CategoryName, Product.Title, User.UserName, User.UserEmail";

            QueryGenerator GeneratorMap1 = QueryBuilderParser.ParseQuery( queryString, DataMap );
            QueryGenerator GeneratorMap4 = QueryBuilderParser.ParseQuery( queryString, DataMapStoreDuplicated );
            QueryGenerator GeneratorMap5 = QueryBuilderParser.ParseQuery( queryString, DataMapUserDuplicated );

            string QueryMap1;
            string QueryMap4;
            string QueryMap5;

            string HandcraftedQuery1;
            string HandcraftedQuery4;
            string HandcraftedQuery5;

            if ( UseDefaultQueryInsteadOfExplain )
            {
                QueryMap1 = GeneratorMap1.Run();
                QueryMap4 = GeneratorMap4.Run();
                QueryMap5 = GeneratorMap5.Run();

                YCSBWorkloadFile workload1 = new YCSBWorkloadFile( "get_product_from_category_with_user_project_1", $"{DatabasePrefix}_1" );
                workload1.SetProperty( "recordcount", CollectionDocumentCount[ "Category" ] );
                workload1.SetProperty( "keys", string.Join( ",", CategoryKeys ) );
                workload1.ExportToFile();
                AddCommandFromWorkloadFile( workload1 );
                YCSBWorkloadFile workload4 = new YCSBWorkloadFile( "get_product_from_category_with_user_project_4", $"{DatabasePrefix}_4" );
                workload4.SetProperty( "recordcount", CollectionDocumentCount[ "Category" ] );
                workload4.SetProperty( "keys", string.Join( ",", CategoryKeys ) );
                workload4.ExportToFile();
                AddCommandFromWorkloadFile( workload4 );
                YCSBWorkloadFile workload5 = new YCSBWorkloadFile( "get_product_from_category_with_user_project_5", $"{DatabasePrefix}_5" );
                workload5.SetProperty( "recordcount", CollectionDocumentCount[ "Category" ] );
                workload5.SetProperty( "keys", string.Join( ",", CategoryKeys ) );
                workload5.ExportToFile();
                AddCommandFromWorkloadFile( workload5 );

                HandcraftedQuery1 = Utils.ReadQueryFromFile( $"{BaseQueryFolder}/get_product_from_category_with_user_project_1.mongo" );
                HandcraftedQuery4 = Utils.ReadQueryFromFile( $"{BaseQueryFolder}/get_product_from_category_with_user_project_4.mongo" );
                HandcraftedQuery5 = Utils.ReadQueryFromFile( $"{BaseQueryFolder}/get_product_from_category_with_user_project_5.mongo" );

                YCSBWorkloadFile workloadHandcrafted1 = new YCSBWorkloadFile( "get_product_from_category_with_user_project_handcrafted_1", $"{DatabasePrefix}_1" );
                workloadHandcrafted1.SetProperty( "recordcount", CollectionDocumentCount[ "Category" ] );
                workloadHandcrafted1.SetProperty( "keys", string.Join( ",", CategoryKeys ) );
                workloadHandcrafted1.ExportToFile();
                AddCommandFromWorkloadFile( workloadHandcrafted1 );
                YCSBWorkloadFile workloadHandcrafted4 = new YCSBWorkloadFile( "get_product_from_category_with_user_project_handcrafted_4", $"{DatabasePrefix}_4" );
                workloadHandcrafted4.SetProperty( "recordcount", CollectionDocumentCount[ "Category" ] );
                workloadHandcrafted4.SetProperty( "keys", string.Join( ",", CategoryKeys ) );
                workloadHandcrafted4.ExportToFile();
                AddCommandFromWorkloadFile( workloadHandcrafted4 );
                YCSBWorkloadFile workloadHandcrafted5 = new YCSBWorkloadFile( "get_product_from_category_with_user_project_handcrafted_5", $"{DatabasePrefix}_5" );
                workloadHandcrafted5.SetProperty( "recordcount", CollectionDocumentCount[ "Category" ] );
                workloadHandcrafted5.SetProperty( "keys", string.Join( ",", CategoryKeys ) );
                workloadHandcrafted5.ExportToFile();
                AddCommandFromWorkloadFile( workloadHandcrafted5 );
            }
            else
            {
                QueryMap1 = GeneratorMap1.Explain();
                QueryMap4 = GeneratorMap4.Explain();
                QueryMap5 = GeneratorMap5.Explain();

                HandcraftedQuery1 = Utils.ReadQueryFromFile( "HandcraftedQueries/get_product_from_category_with_user_project_1.mongo" );
                HandcraftedQuery4 = Utils.ReadQueryFromFile( "HandcraftedQueries/get_product_from_category_with_user_project_2.mongo" );
                HandcraftedQuery5 = Utils.ReadQueryFromFile( "HandcraftedQueries/get_product_from_category_with_user_project_3.mongo" );
            }

            RunIterationsForQuery( $"{DatabasePrefix}_1", "get_product_from_category_with_user_project_1", QueryMap1 );
            RunIterationsForQuery( $"{DatabasePrefix}_4", "get_product_from_category_with_user_project_4", QueryMap4 );
            RunIterationsForQuery( $"{DatabasePrefix}_5", "get_product_from_category_with_user_project_5", QueryMap5 );

            RunIterationsForQuery( $"{DatabasePrefix}_index_1", "get_product_from_category_with_user_project_handcrafted_1", HandcraftedQuery1 );
            RunIterationsForQuery( $"{DatabasePrefix}_index_4", "get_product_from_category_with_user_project_handcrafted_4", HandcraftedQuery4 );
            RunIterationsForQuery( $"{DatabasePrefix}_index_5", "get_product_from_category_with_user_project_handcrafted_5", HandcraftedQuery5 );
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
            QueryBuilderMappingMetadata DataMap = Utils.GetMapping( "CMS_1.mapping" );
            QueryBuilderMappingMetadata DataMapStoreDuplicated = Utils.GetMapping( "CMS_4.mapping" );
            QueryBuilderMappingMetadata DataMapUserDuplicated = Utils.GetMapping( "CMS_5.mapping" );

            string queryString = "from Category select *";

            QueryGenerator GeneratorMap1 = QueryBuilderParser.ParseQuery( queryString, DataMap );
            QueryGenerator GeneratorMap4 = QueryBuilderParser.ParseQuery( queryString, DataMapStoreDuplicated );
            QueryGenerator GeneratorMap5 = QueryBuilderParser.ParseQuery( queryString, DataMapUserDuplicated );

            string QueryMap1;
            string QueryMap4;
            string QueryMap5;

            string HandcraftedQuery1;
            string HandcraftedQuery4;
            string HandcraftedQuery5;

            if ( UseDefaultQueryInsteadOfExplain )
            {
                QueryMap1 = GeneratorMap1.Run();
                QueryMap4 = GeneratorMap4.Run();
                QueryMap5 = GeneratorMap5.Run();

                YCSBWorkloadFile workload1 = new YCSBWorkloadFile( "get_category_named_home_1", $"{DatabasePrefix}_1" );
                workload1.SetProperty( "recordcount", CollectionDocumentCount[ "Category" ] );
                workload1.ExportToFile();
                AddCommandFromWorkloadFile( workload1 );
                YCSBWorkloadFile workload4 = new YCSBWorkloadFile( "get_category_named_home_4", $"{DatabasePrefix}_4" );
                workload4.SetProperty( "recordcount", CollectionDocumentCount[ "Category" ] );
                workload4.ExportToFile();
                AddCommandFromWorkloadFile( workload4 );
                YCSBWorkloadFile workload5 = new YCSBWorkloadFile( "get_category_named_home_5", $"{DatabasePrefix}_5" );
                workload5.SetProperty( "recordcount", CollectionDocumentCount[ "Category" ] );
                workload5.ExportToFile();
                AddCommandFromWorkloadFile( workload5 );

                HandcraftedQuery1 = Utils.ReadQueryFromFile( $"{BaseQueryFolder}/get_category_named_home_1.mongo" );
                HandcraftedQuery4 = Utils.ReadQueryFromFile( $"{BaseQueryFolder}/get_category_named_home_2.mongo" );
                HandcraftedQuery5 = Utils.ReadQueryFromFile( $"{BaseQueryFolder}/get_category_named_home_3.mongo" );

                YCSBWorkloadFile workloadHandcrafted1 = new YCSBWorkloadFile( "get_category_named_home_handcrafted_1", $"{DatabasePrefix}_1" );
                workloadHandcrafted1.SetProperty( "recordcount", CollectionDocumentCount[ "Category" ] );
                workloadHandcrafted1.ExportToFile();
                AddCommandFromWorkloadFile( workloadHandcrafted1 );
                YCSBWorkloadFile workloadHandcrafted4 = new YCSBWorkloadFile( "get_category_named_home_handcrafted_4", $"{DatabasePrefix}_4" );
                workloadHandcrafted4.SetProperty( "recordcount", CollectionDocumentCount[ "Category" ] );
                workloadHandcrafted4.ExportToFile();
                AddCommandFromWorkloadFile( workloadHandcrafted4 );
                YCSBWorkloadFile workloadHandcrafted5 = new YCSBWorkloadFile( "get_category_named_home_handcrafted_5", $"{DatabasePrefix}_5" );
                workloadHandcrafted5.SetProperty( "recordcount", CollectionDocumentCount[ "Category" ] );
                workloadHandcrafted5.ExportToFile();
                AddCommandFromWorkloadFile( workloadHandcrafted5 );
            }
            else
            {
                QueryMap1 = GeneratorMap1.Explain();
                QueryMap4 = GeneratorMap4.Explain();
                QueryMap5 = GeneratorMap5.Explain();

                HandcraftedQuery1 = Utils.ReadQueryFromFile( "HandcraftedQueries/get_category_named_home_1.mongo" );
                HandcraftedQuery4 = Utils.ReadQueryFromFile( "HandcraftedQueries/get_category_named_home_2.mongo" );
                HandcraftedQuery5 = Utils.ReadQueryFromFile( "HandcraftedQueries/get_category_named_home_3.mongo" );
            }

            RunIterationsForQuery( $"{DatabasePrefix}_1", "get_category_named_home_1", QueryMap1 );
            RunIterationsForQuery( $"{DatabasePrefix}_4", "get_category_named_home_4", QueryMap4 );
            RunIterationsForQuery( $"{DatabasePrefix}_5", "get_category_named_home_5", QueryMap5 );

            RunIterationsForQuery( $"{DatabasePrefix}_1", "get_category_named_home_handcrafted_1", HandcraftedQuery1, true );
            RunIterationsForQuery( $"{DatabasePrefix}_4", "get_category_named_home_handcrafted_4", HandcraftedQuery4, true );
            RunIterationsForQuery( $"{DatabasePrefix}_5", "get_category_named_home_handcrafted_5", HandcraftedQuery5, true );
        }
        /// <summary>
        /// Execute GetAllProductsThatCostsLessThan5
        /// 
        /// Query:
        /// 
        /// FROM Product
        /// RJOIN <CategoryProducts> ( Category )
        /// RJOIN <StoreProducts> ( Store )
        /// RJOIN <UserProducts> ( User )
        /// SELECT *
        /// WHERE Product.Price < 5
        /// </summary>
        public void GetAllProductsThatCostsLessThan5()
        {
            QueryBuilderMappingMetadata DataMap1 = Utils.GetMapping( "CMS_1.mapping" );
            QueryBuilderMappingMetadata DataMap2 = Utils.GetMapping( "CMS_2.mapping" );
            QueryBuilderMappingMetadata DataMap3 = Utils.GetMapping( "CMS_3.mapping" );
            QueryBuilderMappingMetadata DataMap4 = Utils.GetMapping( "CMS_4.mapping" );
            QueryBuilderMappingMetadata DataMap5 = Utils.GetMapping( "CMS_5.mapping" );

            string queryString = "from Product rjoin <CategoryProducts> (Category) rjoin <StoreProducts> (Store) rjoin <UserProducts> (User) select * where Product.Price < 5";

            QueryableEntity Product = new QueryableEntity( DataMap1.EntityRelationshipModel.FindByName( "Product" ) );

            QueryGenerator QueryGen1 = QueryBuilderParser.ParseQuery( queryString, DataMap1 );
            QueryGenerator QueryGen2 = QueryBuilderParser.ParseQuery( queryString, DataMap2 );
            QueryGenerator QueryGen3 = QueryBuilderParser.ParseQuery( queryString, DataMap3 );
            QueryGenerator QueryGen4 = QueryBuilderParser.ParseQuery( queryString, DataMap4 );
            QueryGenerator QueryGen5 = QueryBuilderParser.ParseQuery( queryString, DataMap5 );

            string Query1;
            string Query2;
            string Query3;
            string Query4;
            string Query5;

            string HandcraftedQuery1;
            string HandcraftedQuery2;
            string HandcraftedQuery3;
            string HandcraftedQuery4;
            string HandcraftedQuery5;

            if ( UseDefaultQueryInsteadOfExplain )
            {
                Query1 = QueryGen1.Run();
                Query2 = QueryGen2.Run();
                Query3 = QueryGen3.Run();
                Query4 = QueryGen4.Run();
                Query5 = QueryGen5.Run();

                YCSBWorkloadFile workload1 = new YCSBWorkloadFile( "get_all_products_that_costs_less5_1", $"{DatabasePrefix}_1" );
                workload1.SetProperty( "recordcount", CollectionDocumentCount[ "Product" ] );
                workload1.ExportToFile();
                AddCommandFromWorkloadFile( workload1 );
                YCSBWorkloadFile workload2 = new YCSBWorkloadFile( "get_all_products_that_costs_less5_2", $"{DatabasePrefix}_3" );
                workload2.SetProperty( "recordcount", CollectionDocumentCount[ "Product" ] );
                workload2.ExportToFile();
                AddCommandFromWorkloadFile( workload2 );
                YCSBWorkloadFile workload3 = new YCSBWorkloadFile( "get_all_products_that_costs_less5_3", $"{DatabasePrefix}_2" );
                workload3.SetProperty( "recordcount", CollectionDocumentCount[ "Product" ] );
                workload3.ExportToFile();
                AddCommandFromWorkloadFile( workload3 );
                YCSBWorkloadFile workload4 = new YCSBWorkloadFile( "get_all_products_that_costs_less5_4", $"{DatabasePrefix}_4" );
                workload4.SetProperty( "recordcount", CollectionDocumentCount[ "Product" ] );
                workload4.ExportToFile();
                AddCommandFromWorkloadFile( workload4 );
                YCSBWorkloadFile workload5 = new YCSBWorkloadFile( "get_all_products_that_costs_less5_5", $"{DatabasePrefix}_5" );
                workload5.SetProperty( "recordcount", CollectionDocumentCount[ "Product" ] );
                workload5.ExportToFile();
                AddCommandFromWorkloadFile( workload5 );

                HandcraftedQuery1 = Utils.ReadQueryFromFile( $"{BaseQueryFolder}/get_all_products_that_costs_less5_1.mongo" );
                HandcraftedQuery2 = Utils.ReadQueryFromFile( $"{BaseQueryFolder}/get_all_products_that_costs_less5_2.mongo" );
                HandcraftedQuery3 = Utils.ReadQueryFromFile( $"{BaseQueryFolder}/get_all_products_that_costs_less5_3.mongo" );
                HandcraftedQuery4 = Utils.ReadQueryFromFile( $"{BaseQueryFolder}/get_all_products_that_costs_less5_4.mongo" );
                HandcraftedQuery5 = Utils.ReadQueryFromFile( $"{BaseQueryFolder}/get_all_products_that_costs_less5_5.mongo" );

                YCSBWorkloadFile workloadHandcrafted1 = new YCSBWorkloadFile( "get_all_products_that_costs_less5_handcrafted_1", $"{DatabasePrefix}_1" );
                workloadHandcrafted1.SetProperty( "recordcount", CollectionDocumentCount[ "Product" ] );
                workloadHandcrafted1.ExportToFile();
                AddCommandFromWorkloadFile( workloadHandcrafted1 );
                YCSBWorkloadFile workloadHandcrafted2 = new YCSBWorkloadFile( "get_all_products_that_costs_less5_handcrafted_2", $"{DatabasePrefix}_3" );
                workloadHandcrafted2.SetProperty( "recordcount", CollectionDocumentCount[ "Product" ] );
                workloadHandcrafted2.ExportToFile();
                AddCommandFromWorkloadFile( workloadHandcrafted2 );
                YCSBWorkloadFile workloadHandcrafted3 = new YCSBWorkloadFile( "get_all_products_that_costs_less5_handcrafted_3", $"{DatabasePrefix}_2" );
                workloadHandcrafted3.SetProperty( "recordcount", CollectionDocumentCount[ "Product" ] );
                workloadHandcrafted3.ExportToFile();
                AddCommandFromWorkloadFile( workloadHandcrafted3 );
                YCSBWorkloadFile workloadHandcrafted4 = new YCSBWorkloadFile( "get_all_products_that_costs_less5_handcrafted_4", $"{DatabasePrefix}_4" );
                workloadHandcrafted4.SetProperty( "recordcount", CollectionDocumentCount[ "Product" ] );
                workloadHandcrafted4.ExportToFile();
                AddCommandFromWorkloadFile( workloadHandcrafted4 );
                YCSBWorkloadFile workloadHandcrafted5 = new YCSBWorkloadFile( "get_all_products_that_costs_less5_handcrafted_5", $"{DatabasePrefix}_5" );
                workloadHandcrafted5.SetProperty( "recordcount", CollectionDocumentCount[ "Product" ] );
                workloadHandcrafted5.ExportToFile();
                AddCommandFromWorkloadFile( workloadHandcrafted5 );
            }
            else
            {
                Query1 = QueryGen1.Explain();
                Query2 = QueryGen2.Explain();
                Query3 = QueryGen3.Explain();
                Query4 = QueryGen4.Explain();
                Query5 = QueryGen5.Explain();

                HandcraftedQuery1 = Utils.ReadQueryFromFile( "HandcraftedQueries/get_all_products_that_costs_less5_1.mongo" );
                HandcraftedQuery2 = Utils.ReadQueryFromFile( "HandcraftedQueries/get_all_products_that_costs_less5_2.mongo" );
                HandcraftedQuery3 = Utils.ReadQueryFromFile( "HandcraftedQueries/get_all_products_that_costs_less5_3.mongo" );
                HandcraftedQuery4 = Utils.ReadQueryFromFile( "HandcraftedQueries/get_all_products_that_costs_less5_4.mongo" );
                HandcraftedQuery5 = Utils.ReadQueryFromFile( "HandcraftedQueries/get_all_products_that_costs_less5_5.mongo" );
            }

            RunIterationsForQuery( $"{DatabasePrefix}_1", "get_all_products_that_costs_less5_1", Query1 );
            RunIterationsForQuery( $"{DatabasePrefix}_3", "get_all_products_that_costs_less5_2", Query2 );
            RunIterationsForQuery( $"{DatabasePrefix}_2", "get_all_products_that_costs_less5_3", Query3 );
            RunIterationsForQuery( $"{DatabasePrefix}_4", "get_all_products_that_costs_less5_4", Query4 );
            RunIterationsForQuery( $"{DatabasePrefix}_5", "get_all_products_that_costs_less5_5", Query5 );

            RunIterationsForQuery( $"{DatabasePrefix}_1", "get_all_products_that_costs_less5_handcrafted_1", HandcraftedQuery1 );
            RunIterationsForQuery( $"{DatabasePrefix}_3", "get_all_products_that_costs_less5_handcrafted_2", HandcraftedQuery2, true );
            RunIterationsForQuery( $"{DatabasePrefix}_2", "get_all_products_that_costs_less5_handcrafted_3", HandcraftedQuery3 );
            RunIterationsForQuery( $"{DatabasePrefix}_4", "get_all_products_that_costs_less5_handcrafted_4", HandcraftedQuery4 );
            RunIterationsForQuery( $"{DatabasePrefix}_5", "get_all_products_that_costs_less5_handcrafted_5", HandcraftedQuery5 );
        }

        public void RunCustomQueriesTest()
        {
            string CustomQuery1 = Utils.ReadQueryFromFile( "CustomQueries/get_all_products_that_costs_less5_m_1.mongo" );
            string CustomQuery2 = Utils.ReadQueryFromFile( "CustomQueries/get_all_products_that_costs_less5_m_2.mongo" );
            string CustomQuery3 = Utils.ReadQueryFromFile( "CustomQueries/get_all_products_that_costs_less5_m_3.mongo" );
            string CustomQuery4 = Utils.ReadQueryFromFile( "CustomQueries/get_all_products_that_costs_less5_m_4.mongo" );
            string CustomQuery5 = Utils.ReadQueryFromFile( "CustomQueries/get_all_products_that_costs_less5_m_5.mongo" );

            RunIterationsForQuery( $"{DatabasePrefix}_1", "get_all_products_that_costs_less5_modified_1", CustomQuery1 );
            RunIterationsForQuery( $"{DatabasePrefix}_3", "get_all_products_that_costs_less5_modified_2", CustomQuery2 );
            RunIterationsForQuery( $"{DatabasePrefix}_2", "get_all_products_that_costs_less5_modified_3", CustomQuery3 );
            RunIterationsForQuery( $"{DatabasePrefix}_4", "get_all_products_that_costs_less5_modified_4", CustomQuery4 );
            RunIterationsForQuery( $"{DatabasePrefix}_5", "get_all_products_that_costs_less5_modified_5", CustomQuery5 );
        }

        /// <summary>
        /// Adds a PS command to run YCSB with the given workload file
        /// </summary>
        /// <param name="inWorkload"></param>
        public void AddCommandFromWorkloadFile(YCSBWorkloadFile inWorkload)
        {
            string command = $@"del .\{inWorkload.Name}.txt;" +
                $@".\bin\ycsb load mongodb -P {inWorkload.FileName} -p dotransactions=true >> .\{inWorkload.Name}.txt;" +
                $@"Get-Content -Path .\{inWorkload.Name}.txt";

            TerminalCommands.Add( command );
        }
        /// <summary>
        /// Writes terminal commands to a file
        /// </summary>
        public void ExportCommandsToFile()
        {
            // Add Command to compute total processing time
            TerminalCommands.Add( "$EndDate=(GET-DATE);" );
            TerminalCommands.Add( "Write-OutPut \"Processing ended at $EndDate;\"" );
            TerminalCommands.Add( "NEW-TIMESPAN -Start $StartDate -End $EndDate;" );

            using ( StreamWriter sw = new StreamWriter( @"D:\Projects\mestrado\YCSB\benchmark-commands.ps1", false ) )
            {
                foreach ( string command in TerminalCommands )
                {
                    sw.WriteLine( command );
                }

                sw.Close();
            }
        }
        /// <summary>
        /// Generate keys based on the iteration count
        /// </summary>
        public void GenerateKeys()
        {
            if ( Iterations == 0 )
            {
                return;
            }

            // Instantiate random
            Random rand = new Random();

            // Category
            int CategoryRecordCount = (int)CollectionDocumentCount.GetValueOrDefault( "Category" );
            for ( int i = 0; i < Iterations; i++ )
            {
                int key = rand.Next( 1, CategoryRecordCount + 1 );
                CategoryKeys.Add( key );
            }

            // Product
            int ProductRecordCount = (int)CollectionDocumentCount.GetValueOrDefault( "Product" );
            for ( int i = 0; i < Iterations; i++ )
            {
                int key = rand.Next( 1, ProductRecordCount + 1 );
                ProductKeys.Add( key );
            }

            // Store
            int StoreRecordCount = (int)CollectionDocumentCount.GetValueOrDefault( "Store" );
            for ( int i = 0; i < Iterations; i++ )
            {
                int key = rand.Next( 1, StoreRecordCount + 1 );
                StoreKeys.Add( key );
            }

            // User
            int UserRecordCount = (int)CollectionDocumentCount.GetValueOrDefault( "User" );
            for ( int i = 0; i < Iterations; i++ )
            {
                int key = rand.Next( 1, UserRecordCount + 1 );
                UserKeys.Add( key );
            }
        }
        /// <summary>
        /// Copy all queries to benchmark
        /// </summary>
        public void CopyQueriesToBenchmarkFolder()
        {
            if ( string.IsNullOrWhiteSpace( BenchmarkWorkloadFolder ) )
            {
                throw new ArgumentNullException( $"Value for BenchmarkWorkloadFolder cannot be null" );
            }

            DirectoryInfo workloadDir = new DirectoryInfo( BaseQueryFolder );
            FileInfo[] queryFiles = workloadDir.GetFiles( "*.mongo" );

            Console.WriteLine( $"Found {queryFiles.Length} files." );
            Console.WriteLine( $"Copying files to {BenchmarkWorkloadFolder}" );

            foreach ( FileInfo file in queryFiles )
            {
                file.CopyTo( $"{BenchmarkWorkloadFolder}/{file.Name}", true );
            }

            Console.WriteLine( "Files copied." );
        }

        public MarketingCMS(string inDatabasePrefix)
        {
            CollectionDocumentCount = new Dictionary<string, long>();
            TerminalCommands = new List<string>();
            CategoryKeys = new List<int>();
            ProductKeys = new List<int>();
            StoreKeys = new List<int>();
            UserKeys = new List<int>();
            UseReadAllQueries = false;
            DatabasePrefix = inDatabasePrefix;

            // Add Command to compute total processing time
            TerminalCommands.Add( "$StartDate=(GET-DATE);" );
            TerminalCommands.Add( "Write-OutPut \"Processing started at $StartDate;\"" );

            // Add Command to rebuild mongodb-binding
            TerminalCommands.Add( @"Write-OutPut ""Deleting MongoDB binding [YCSB will rebuild this]."";del .\mongodb\target\mongodb-binding-0.18.0-SNAPSHOT.jar;" );
            TerminalCommands.Add( "Write-OutPut \"Running tests...\"" );

            // Load document count
            long CategoryCount = MongoContext.GetCollectionDocumentCount( $"{DatabasePrefix}_1", "Category" );
            long ProductCount = MongoContext.GetCollectionDocumentCount( $"{DatabasePrefix}_1", "Product" );
            long StoreCount = MongoContext.GetCollectionDocumentCount( $"{DatabasePrefix}_1", "Store" );
            long UserCount = MongoContext.GetCollectionDocumentCount( $"{DatabasePrefix}_1", "User" );

            CollectionDocumentCount.Add( "Category", CategoryCount );
            CollectionDocumentCount.Add( "Product", ProductCount );
            CollectionDocumentCount.Add( "Store", StoreCount );
            CollectionDocumentCount.Add( "User", UserCount );

            Console.WriteLine( $"Category: {CategoryCount}" );
            Console.WriteLine( $"Product: {ProductCount}" );
            Console.WriteLine( $"Store: {StoreCount}" );
            Console.WriteLine( $"User: {UserCount}" );
        }
    }
}