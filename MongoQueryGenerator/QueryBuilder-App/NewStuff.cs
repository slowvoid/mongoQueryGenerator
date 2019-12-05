using QueryBuilder.Map;
using QueryBuilder.Parser;
using QueryBuilder.Query;
using System;
using System.Collections.Generic;
using System.IO;

namespace QueryBuilderApp
{
    public static class NewStuff
    {
        public static void Main()
        {
            Console.WriteLine( "Running new stuff..." );

            var map = QueryBuilderParser.ParseMapping( new FileStream( "Test.qer", FileMode.Open ) );
            foreach ( MapRule Rule in map.ERMongoMapping.Rules )
            {
                Console.WriteLine( "Rules for {0} Mapped To {1}", Rule.Source.Name, Rule.Target.Name );
                foreach ( KeyValuePair<string,string> Attributes in Rule.Rules )
                {
                    Console.WriteLine( "Key: {0} | Value: {1}", Attributes.Key, Attributes.Value );
                }
            }

            string query = "from Person p rjoin <Drives d> (Car c)";
            QueryGenerator gen = QueryBuilderParser.ParseQuery( query, map );

            string mongoQuery = gen.Run();

            Console.WriteLine( mongoQuery );

            QueryRunner runner = new QueryRunner( "mongodb://localhost:27017", "testParser" );

            Console.WriteLine( runner.GetJSON( mongoQuery ) );

            Console.Read();
        }
    }
}