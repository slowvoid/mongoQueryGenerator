using QueryBuilder.Parser;
using System;
using System.IO;

namespace TestApp
{
    class GenerateQueries
    {
        static void Main( string[] args )
        {
            string[] mappings = { "map1.mapping", "map2.mapping", "map3.mapping", "map4.mapping", "map5.mapping" };
            string[] queries =
            {
                "from Category select * where Category.CategoryID = 1",
                "from Product rjoin <CategoryProducts> (Category) rjoin <StoreProducts> (Store) rjoin <UserProducts> (User) select * where Product.ProductID = 1",
                "from Category rjoin <CategoryProducts> (Product) select * where Category.CategoryID = 1",
                "from Category rjoin <CategoryProducts> (Product rjoin <StoreProducts> (Store)) select * where Category.CategoryID = 1",
                "from Category rjoin <CategoryProducts> (Product rjoin <UserProducts> (User)) select * where Category.CategoryID = 1",
                "from Store rjoin <StoreProducts> (Product) select * where Store.StoreID = 1",
                "from Store select * where Store.StoreID = 1",
                "from User select * where User.UserID = 1",
                "from Category rjoin <CategoryProducts> (Product rjoin <UserProducts> (User)) select Category.CategoryName, Product.Title, User.UserName, User.UserEmail where Category.CategoryID = 1",
                "from Product rjoin <UserProducts> (User) select Product.Title, User.UserName where Product.ProductID = 1"
            };

            int queryIndex = 1;
            foreach ( string query in queries )
            {
                foreach ( string map in mappings )
                {
                    var mapping = QueryBuilderParser.ParseMapping(new FileStream(map, FileMode.Open));
                    string mapName = map.Replace(".mapping", "");
                    try
                    {
                        var queryGen = QueryBuilderParser.ParseQuery(query, mapping);
                        string explainQuery = queryGen.Explain();

                        using (var sw = new StreamWriter($"query-{queryIndex}-{mapName}.mongo"))
                        {
                            sw.Write(explainQuery);
                            sw.Close();
                        }
                    }
                    catch ( Exception ex )
                    {
                        Console.WriteLine( ex.Message );
                    }
                    finally
                    {
                    }
                }
                queryIndex++;
            }
        }
    }
}