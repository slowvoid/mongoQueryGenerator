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
            // Connect to db
            ResearchCMSContext dbCMS = new ResearchCMSContext();

            // Truncate tables
            dbCMS.Database.ExecuteSqlCommand( "TRUNCATE TABLE users" );
            dbCMS.Database.ExecuteSqlCommand( "TRUNCATE TABLE products" );
            dbCMS.Database.ExecuteSqlCommand( "TRUNCATE TABLE stores" );
            dbCMS.Database.ExecuteSqlCommand( "TRUNCATE TABLE categories" );

            Faker<User> testUsers = new Faker<User>("pt_BR")
                .RuleFor( u => u.UserID, ( f, u ) => ++u.UserID )
                .RuleFor( u => u.Name, ( f, u ) => f.Name.FullName() )
                .RuleFor( u => u.Email, ( f, u ) => f.Internet.Email( u.Name ) )
                .FinishWith( ( f, u ) =>
                {
                    Console.WriteLine( "User Created! Name = {0}", u.Name );
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
                    Name = categoryNames[ i ]
                };

                categories.Add( cat );
            }

            dbCMS.Categories.AddRange( categories );
            dbCMS.SaveChanges();

            Console.WriteLine( "Done! Current category count: {0}", dbCMS.Categories.Count() );

            Faker<Store> testStores = new Faker<Store>( "pt_BR" )
                .RuleFor( s => s.StoreID, ( f, s ) => ++s.StoreID )
                .RuleFor( s => s.Name, ( f, s ) => f.Company.CompanyName() )
                .FinishWith( ( f, s ) =>
                {
                    Console.WriteLine( "Store created! Name = {0}", s.Name );
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

            Console.Read();
        }
    }
}