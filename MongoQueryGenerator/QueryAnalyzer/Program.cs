using System;

namespace QueryAnalyzer
{
    class Program
    {
        static void Main( string[] args )
        {
            Console.WriteLine( "Running and storing query stats" );
            DateTime StartTime = DateTime.Now;
            Console.WriteLine( "Starting Time: {0}", StartTime.ToString() );

            MarketingCMS cms = new MarketingCMS();
            cms.Iterations = 100;
            cms.TargetDatabase = "research_performance_stats_nosort";
            cms.ExportQueries = true;
            cms.UseDefaultQueryInsteadOfExplain = true;

            Console.WriteLine( "Iterations to run: {0} | Target database: {1}", cms.Iterations, cms.TargetDatabase );

            Console.WriteLine( "Query: get_all_products" );
            cms.GetAllProducts();

            Console.WriteLine( "Query: get_all_categories" );
            cms.GetAllCategories();

            Console.WriteLine( "Query: get_all_products_from_category" );
            cms.GetAllProductsFromCategory();

            Console.WriteLine( "Query: get_all_products_from_category_with_store" );
            cms.GetAllProductsFromCategoryWithStore();

            Console.WriteLine( "Query: get_all_products_from_category_with_user" );
            cms.GetAllProductsFromCategoryWithUser();

            Console.WriteLine( "Query: get_product_from_category_with_user_project" );
            cms.GetAllProductsFromCategoryWithUserAndSelectOnlyTitleNameEmailCategoryName();

            Console.WriteLine( "Query: get_all_products_from_store" );
            cms.GetAllProductsFromStore();

            //TODO: RE RUN THIS
            //BUG: NOT WORKING
            //Console.WriteLine( "Query: get_all_products_from_user" );
            //cms.GetAllProductsFromUser();

            Console.WriteLine( "Query: get_all_stores" );
            cms.GetAllStores();

            Console.WriteLine( "Query: get_all_users" );
            cms.GetAllUsers();

            //Console.WriteLine( "Query: get_category_named_home" );
            //cms.GetCategoryThatIsNamedHome();

            Console.WriteLine( "Query: get_product_title_username" );
            cms.GetProductTitleAndUserName();

            //Console.WriteLine( "Query: get_products_prices_less" );
            //cms.GetAllProductsThatCostsLessThan5();

            //Console.WriteLine( "Query: Running custom test" );
            //cms.RunCustomQueriesTest();

            cms.ExportCommandsToFile();

            DateTime EndTime = DateTime.Now;
            Console.WriteLine( "Finished" );
            Console.WriteLine( "Finished Time: {0}", EndTime.ToString() );
            Console.WriteLine( "Total Time: {0}", ( EndTime - StartTime ).TotalMinutes );
            Console.Read();
        }
    }
}
