using System;
using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using QueryBuilder.ER;
using QueryBuilder.Parser;

namespace QueryBuilder.TestParser
{
    class Program
    {
        static void Main(string[] args)
        {
            String input = args[0];
            ICharStream stream = CharStreams.fromPath(input);
            ITokenSource lexer = new QueryBuilderMappingLexer(stream);
            ITokenStream tokens = new CommonTokenStream(lexer);
            QueryBuilderMappingParser parser = new QueryBuilderMappingParser(tokens);
            parser.BuildParseTree = true;
            Console.WriteLine("Starting parser...");
            IParseTree tree = parser.program();

            Console.WriteLine("Starting visitor...");

            var analyzer = new QueryBuilderMappingAnalyzer();
            analyzer.Visit(tree);

            Console.WriteLine("Showing results...");

            Console.WriteLine($"ERModel: {analyzer.EntityRelationshipModel.Name}");

            analyzer.EntityRelationshipModel.Elements.FindAll(e => e.GetType() == typeof(Entity)).ForEach(e =>
            {
                Console.WriteLine($"Entity: {e.Name}");
                e.Attributes.ForEach(a =>
                {
                    Console.WriteLine($"   Attribute: {a.Name}");
                });
            });

            analyzer.EntityRelationshipModel.Elements.FindAll(e => e.GetType() == typeof(Relationship)).ConvertAll<Relationship>(e => (Relationship)e).ForEach(r =>
            {
                Console.WriteLine($"Relationship: {r.Name}");

                r.Ends.ForEach(e =>
                {
                    Console.WriteLine($"   End: {e.TargetEntity.Name} : {e.Cardinality}");
                });
                r.Attributes.ForEach(a =>
                {
                    Console.WriteLine($"   Attribute: {a.Name}");
                });
            });
        }
    }
}
