using Bogus;
using DataFaker.Models.CMS;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataFaker
{
    public static class MainApp
    {
        public static void Main()
        {
            Console.Write( "Enter 1 to process CMS data or 2 to process Progradweb: " );
            int dbPicked = Convert.ToInt32( Console.ReadLine() );

            switch ( dbPicked )
            {
                case 1:
                    RunCSMStuff();
                    break;
                case 2:
                    RunProgradStuff();
                    break;
                default:
                    Console.WriteLine( "Invalid choice\nPress enter to exit." );
                    break;
            }

            Console.Read();
        }

        public static void RunCSMStuff()
        {
            // Connect to db
            ResearchCMSContext dbCMS = new ResearchCMSContext();

            // Truncate tables
            dbCMS.Database.ExecuteSqlCommand( "TRUNCATE TABLE users" );
            dbCMS.Database.ExecuteSqlCommand( "TRUNCATE TABLE products" );
            dbCMS.Database.ExecuteSqlCommand( "TRUNCATE TABLE stores" );
            dbCMS.Database.ExecuteSqlCommand( "TRUNCATE TABLE categories" );

            Faker<User> testUsers = new Faker<User>( "pt_BR" )
                .RuleFor( u => u.UserID, ( f, u ) => ++u.UserID )
                .RuleFor( u => u.UserName, ( f, u ) => f.Name.FullName() )
                .RuleFor( u => u.UserEmail, ( f, u ) => f.Internet.Email( u.UserName ) )
                .FinishWith( ( f, u ) =>
                {
                    Console.WriteLine( "User Created! Name = {0}", u.UserName );
                } );

            int amountOfUsers = 50;

            List<User> users = testUsers.Generate( amountOfUsers );
            dbCMS.Users.AddRange( users );
            dbCMS.SaveChanges();

            Console.WriteLine( "Done! Current user count: {0}", dbCMS.Users.Count() );

            string[] categoryNames = new Faker( "pt_BR" ).Commerce.Categories( 30 ).Distinct().ToArray();
            List<Category> categories = new List<Category>();
            int amountOfCategories = categoryNames.Length;

            for ( int i = 0; i < amountOfCategories; i++ )
            {
                Category cat = new Category()
                {
                    CategoryName = categoryNames[ i ]
                };

                categories.Add( cat );
            }

            dbCMS.Categories.AddRange( categories );
            dbCMS.SaveChanges();

            Console.WriteLine( "Done! Current category count: {0}", dbCMS.Categories.Count() );

            Faker<Store> testStores = new Faker<Store>( "pt_BR" )
                .RuleFor( s => s.StoreID, ( f, s ) => ++s.StoreID )
                .RuleFor( s => s.StoreName, ( f, s ) => f.Company.CompanyName() )
                .FinishWith( ( f, s ) =>
                {
                    Console.WriteLine( "Store created! Name = {0}", s.StoreName );
                } );

            List<Store> stores = testStores.Generate( 20 );
            dbCMS.Stores.AddRange( stores );
            dbCMS.SaveChanges();

            Console.WriteLine( "Done! Current store count: {0}", dbCMS.Stores.Count() );

            Faker<Product> testProducts = new Faker<Product>( "pt_BR" )
                .RuleFor( p => p.ProductID, ( f, p ) => ++p.ProductID )
                .RuleFor( p => p.Title, ( f, p ) => f.Commerce.ProductName() )
                .RuleFor( p => p.Description, ( f, p ) => f.Commerce.ProductAdjective() )
                .RuleFor( p => p.UserID, ( f, p ) => f.PickRandom( users ).UserID )
                .RuleFor( p => p.CategoryID, ( f, p ) => f.PickRandom( categories ).CategoryID )
                .RuleFor( p => p.StoreID, ( f, p ) => f.PickRandom( stores ).StoreID )
                .FinishWith( ( f, p ) =>
                {
                    Console.WriteLine( "Product created! Title = {0}", p.Title );
                } );

            List<Product> products = testProducts.Generate( 50 );
            dbCMS.Products.AddRange( products );
            dbCMS.SaveChanges();

            Console.WriteLine( "Done! Current product count: {0}", dbCMS.Products.Count() );
        }

        public static void RunProgradStuff()
        {
            ProgradContext dbContext = new ProgradContext();

            Console.WriteLine( "Processing prograd stuff" );

            // Truncate tables
            dbContext.Database.ExecuteSqlCommand( "TRUNCATE TABLE cursograd" );
        }
    }
}