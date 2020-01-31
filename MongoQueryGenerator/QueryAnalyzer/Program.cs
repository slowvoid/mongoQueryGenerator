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

            Console.WriteLine( "Query: get_all_products_from_category_with_user_select_few" );
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

            Console.WriteLine( "Query: get_category_named_home" );
            cms.GetCategoryThatIsNamedHome();

            Console.WriteLine( "Query: get_product_title_username" );
            cms.GetProductTitleAndUserName();

            DateTime EndTime = DateTime.Now;
            Console.WriteLine( "Finished" );
            Console.WriteLine( "Finished Time: {0}", EndTime.ToString() );
            Console.WriteLine( "Total Time: {0}", ( EndTime - StartTime ).TotalMinutes );
            Console.Read();
        }
    }
}
