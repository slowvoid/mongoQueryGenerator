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

            IParseTree tree = parser.program();

            var analyzer = new QueryBuilderMappingAnalyzer();
            analyzer.Visit(tree);
            Console.WriteLine($"ERModel: {analyzer.EntityRelationshipModel.Name}");

            Console.WriteLine("\n\n****** ER Model ********\n\n");

            foreach(var e in analyzer.EntityRelationshipModel.Elements.FindAll(e => e.GetType() == typeof(Entity)))
            {
                Console.WriteLine($"Entity: {e.Name}");
                e.Attributes.ForEach(a =>
                {
                    Console.WriteLine($"   Attribute: {a.Name}");
                });
            }

            foreach(var r in analyzer.EntityRelationshipModel.Elements.FindAll(e => e.GetType() == typeof(Relationship)).ConvertAll<Relationship>(e => (Relationship)e))
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
            }

            Console.WriteLine("\n\n****** Mongo DB Schema ********\n\n");


            foreach(var c in analyzer.MongoDBSchema.Collections)
            {
                Console.WriteLine($"Collection: {c.Name}");
                c.DocumentSchema.Attributes.ForEach(a =>
                {
                    Console.WriteLine($"   Field: {a.Name}");
                });
            }

            Console.WriteLine("\n\n****** Mapping ********\n\n");


            foreach(var r in analyzer.ERMongoMapping.Rules)
            {
                Console.WriteLine($"Rule: {r.Source.Name} = {r.Target.Name} (Main={r.IsMain})");
                foreach(var sr in r.Rules)
                {
                    Console.WriteLine($"   {sr.Key} - {sr.Value}");
                }
            }

            Console.WriteLine("\n\n****** Warnings and Errors ********\n\n");


            analyzer.Warnings.ForEach(w => Console.WriteLine(w));
            analyzer.Errors.ForEach(e => Console.WriteLine(e));

        }
    }
}
