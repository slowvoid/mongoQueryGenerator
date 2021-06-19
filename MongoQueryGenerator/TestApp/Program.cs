using QueryBuilder.Operation;
using QueryBuilder.Parser;
using System;
using System.IO;

namespace TestApp
{
    class Program
    {
        static void Main( string[] args )
        {
            var mapping = QueryBuilderParser.ParseMapping( new FileStream( "select.mapping", FileMode.Open ) );

            while ( true )
            {
                try
                {
                    Console.Write( "Query: " );

                    string query = Console.ReadLine();

                    if ( query == "q" )
                    {
                        break;
                    }

                    var queryGen = QueryBuilderParser.ParseQuery( query, mapping );

                    Console.WriteLine( queryGen.Run() );
                    foreach ( AlgebraOperator Op in queryGen.PipelineOperators )
                    {
                        Op.SummarizeToString();
                    }
                }
                catch ( Exception ex )
                {
                    Console.WriteLine( ex.Message );
                }                
            }

            Console.Read();
        }
    }
}
