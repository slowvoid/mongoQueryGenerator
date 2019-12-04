using QueryBuilder.Parser;
using QueryBuilder.Query;
using System;
using System.IO;

namespace QueryBuilderApp
{
    public static class NewStuff
    {
        public static void Main()
        {
            Console.WriteLine( "Running new stuff..." );

            var map = QueryBuilderParser.ParseMapping( new FileStream( "TestCase1.qer", FileMode.Open ) );

            string query = "from Person p rjoin <Insurance i> (Car c, InsuranceCompany ic)";
            QueryGenerator gen = QueryBuilderParser.ParseQuery( query, map );

            //Console.WriteLine( gen.Run() );

            Console.Read();
        }
    }
}