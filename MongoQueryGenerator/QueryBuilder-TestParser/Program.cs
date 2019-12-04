using System;
using System.IO;
using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using QueryBuilder.ER;
using QueryBuilder.Parser;

namespace QueryBuilder.TestParser
{
    class Program
    {
        static void Main( string[] args )
        {
            var mapping = QueryBuilderParser.ParseMapping( new FileStream( args[ 0 ], FileMode.Open ) );

            Console.WriteLine( $"ERModel: {mapping.EntityRelationshipModel.Name}" );

            Console.WriteLine( "\n\n****** ER Model ********\n\n" );

            foreach ( var e in mapping.EntityRelationshipModel.Elements.FindAll( e => e.GetType() == typeof( Entity ) ) )
            {
                Console.WriteLine( $"Entity: {e.Name}" );
                e.Attributes.ForEach( a =>
                {
                    Console.WriteLine( $"   Attribute: {a.Name} of type {a.OfType}, multivalued={a.MultiValued}" );
                } );
            }

            foreach ( var r in mapping.EntityRelationshipModel.Elements.FindAll( e => e.GetType() == typeof( Relationship ) ).ConvertAll<Relationship>( e => (Relationship)e ) )
            {
                Console.WriteLine( $"Relationship: {r.Name}" );

                r.Ends.ForEach( e =>
                {
                    Console.WriteLine( $"   End: {e.TargetEntity.Name}" );
                } );
                r.Attributes.ForEach( a =>
                {
                    Console.WriteLine( $"   Attribute: {a.Name} of type {a.OfType}, multivalued={a.MultiValued}" );
                } );
            }

            Console.WriteLine( "\n\n****** Mongo DB Schema ********\n\n" );


            foreach ( var c in mapping.MongoDBSchema.Collections )
            {
                Console.WriteLine( $"Collection: {c.Name}" );
                c.DocumentSchema.Attributes.ForEach( a =>
                {
                    Console.WriteLine( $"   Field: {a.Name} of type {a.OfType}, multivalued={a.MultiValued}" );
                } );
            }

            Console.WriteLine( "\n\n****** Mapping ********\n\n" );


            foreach ( var r in mapping.ERMongoMapping.Rules )
            {
                Console.WriteLine( $"Rule: {r.Source.Name} = {r.Target.Name} (Main={r.IsMain})" );
                foreach ( var sr in r.Rules )
                {
                    Console.WriteLine( $"   {sr.Key} - {sr.Value}" );
                }
            }

            Console.WriteLine( "\n\n****** Warnings and Errors ********\n\n" );


            mapping.Warnings.ForEach( w => Console.WriteLine( w ) );
            mapping.Errors.ForEach( e => Console.WriteLine( e ) );

            // string[] queries = { "from Person p rjoin <Insurance i> (Car c, InsuranceCompany ic)",
            //                      "from Car c rjoin <Repaired r> (Garage g)",
            //                      "from Person p rjoin <Insurance i> (Car c rjoin <Repaired r> (Garage g), InsuranceCompany ic)",
            //                      "from (Person p rjoin <Drives d> (Car c)) rjoin <Repaired r> (Garage g)",
            //                      "from (Person p rjoin <Drives d> (Car c rjoin <Repaired r> (Garage g)))",
            //                      "from Person p rjoin <Drives d> (Car c rjoin <Repaired r> (Garage g))" };

            // foreach (var q in queries)
            // {
            //     Console.WriteLine(q);
            //     var generatedQuery = QueryBuilderParser.ParseQuery(q, mapping);

            //     Console.WriteLine("*************");
            //     Console.WriteLine($"Start Argument: {generatedQuery.StartArgument.Entity.Element.Name} AS {generatedQuery.StartArgument.Entity.Alias}");
            //     var i = 1;
            //     foreach (var Op in generatedQuery.PipelineOperators)
            //     {
            //         Console.WriteLine($"Operator {i++}: {Op.ToString()}");
            //     }
            //     Console.WriteLine("*************");

            // }

        }
    }
}
