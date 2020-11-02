using System;
using System.IO;
using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using QueryBuilder.ER;
using QueryBuilder.Operation.Arguments;
using QueryBuilder.Parser;
using QueryBuilder.Query;
using System.Collections.Generic;
using QueryBuilder.Operation;

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

            //string[] queries = { "from Author a rjoin <BookAndAuthor ba> (Book b)" };
            string[] queries = { "from Author a rjoin <BookAndAuthor ba> (Book b rjoin <PublishedBy pb> (Publisher p))" };

            // string[] queries = { "from Person p rjoin <Insurance i> (Car c, InsuranceCompany ic)",
            //                      "from Car c rjoin <Repaired r> (Garage g)",
            //                      "from Person p rjoin <Insurance i> (Car c rjoin <Repaired r> (Garage g), InsuranceCompany ic)",
            //                      "from (Person p rjoin <Drives d> (Car c)) rjoin <Repaired r> (Garage g)",
            //                      "from (Person p rjoin <Drives d> (Car c rjoin <Repaired r> (Garage g)))",
            //                      "from Person p rjoin <Drives d> (Car c rjoin <Repaired r> (Garage g))" };

            foreach ( var q in queries )
            {
                Console.WriteLine( q );
                var generatedQuery = QueryBuilderParser.ParseQuery( q, mapping );

                Console.WriteLine( "*************" );
                Console.WriteLine( $"Start Argument: {generatedQuery.StartArgument.Entity.Element.Name} AS {generatedQuery.StartArgument.Entity.Alias}" );
                var i = 1;
                foreach ( var Op in generatedQuery.PipelineOperators )
                {
                    Console.WriteLine( $"Operator {i++}: {Op.ToString()}" );
                }
                Console.WriteLine( "*************" );

                QueryRunner runner = new QueryRunner( "mongodb://localhost:27017", "sampleAuthorBookPublisher" );
                Console.WriteLine( runner.GetJSON( generatedQuery.Run() ) );
            }

            // Nested join example using manual code
            Console.WriteLine( "Running example ============" );
            QueryableEntity Author = new QueryableEntity( mapping.EntityRelationshipModel.FindByName( "Author" ) );
            QueryableEntity Book = new QueryableEntity( mapping.EntityRelationshipModel.FindByName( "Book" ) );
            QueryableEntity Publisher = new QueryableEntity( mapping.EntityRelationshipModel.FindByName( "Publisher" ) );

            Relationship BookAndAuthor = (Relationship)mapping.EntityRelationshipModel.FindByName( "BookAndAuthor" );
            Relationship PublishedBy = (Relationship)mapping.EntityRelationshipModel.FindByName( "PublishedBy" );

            ComputedEntity BookAndPublisher = new ComputedEntity( "BookAndPublisher", Book,
                PublishedBy, new List<QueryableEntity>() { Publisher } );

            RelationshipJoinOperator JoinOp = new RelationshipJoinOperator( Author, BookAndAuthor,
                new List<QueryableEntity>()
                {
                    new QueryableEntity(BookAndPublisher)
                },
                mapping.ERMongoMapping );

            FromArgument fromArg = new FromArgument( Author, mapping.ERMongoMapping );

            List<AlgebraOperator> Operations = new List<AlgebraOperator>() { JoinOp };
            QueryGenerator queryGen = new QueryGenerator( fromArg, Operations );
            string queryString = queryGen.Run();

            QueryRunner queryRunner = new QueryRunner( "mongodb://localhost:27017", "sampleAuthorBookPublisher" );
            Console.WriteLine( queryRunner.GetJSON( queryString ) );
        }
    }
}
