using Newtonsoft.Json.Linq;
using QueryBuilder.Operation;
using QueryBuilder.Operation.Arguments;
using QueryBuilder.Query;
using System;
using System.Collections.Generic;
using System.IO;

namespace TestApp
{
    public static class JSONTests
    {
        public static void Main()
        {
            Console.WriteLine( "Running..." );

            DataContainer data = MarketingCMSDataProvider.MapEntitiesToCollections();

            QueryableEntity User = new QueryableEntity( data.EntityRelationshipModel.FindByName( "User" ) );
            QueryableEntity Product = new QueryableEntity( data.EntityRelationshipModel.FindByName( "Product" ) );
            QueryableEntity Store = new QueryableEntity( data.EntityRelationshipModel.FindByName( "Store" ) );
            QueryableEntity Category = new QueryableEntity( data.EntityRelationshipModel.FindByName( "Category" ) );

            string Query = MarketingCMS._getQueryForTestAllProducts( data, Product, Store, Category, User );

            QueryRunner Runner = new QueryRunner( "mongodb://127.0.0.1:27017", "pesquisa_cms" );
            string result = Runner.GetJSON( Query );

            JToken JSONResult = JToken.Parse( result );

            if ( JSONResult.Type == JTokenType.Array )
            {
                Console.WriteLine( "This is an array" );

                JContainer ResultArray = (JContainer)JSONResult;
                Console.WriteLine( "Count: {0}", ResultArray.Count );
            }

            using ( StreamWriter sw = new StreamWriter( @"E:\Mestrado\result.json", false ) )
            {
                sw.Write( result );
                sw.Close();
            }
        }
    }
}