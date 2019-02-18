using System;
using System.Collections.Generic;
using QueryBuilder.ER;
using QueryBuilder.Map;
using QueryBuilder.Mongo;
using QueryBuilder.Operation;
using QueryBuilder.Query;
using QueryBuilder.Shared;

namespace QueryBuilderApp
{
    public static class App
    {
        public static void Main()
        {
            Console.WriteLine( "Preparing data to run" );


            Model ErModel = CreateERModel();
            MongoSchema MSchema = CreateMongoSchema();

            // ER Model and MongoDB Schema ready, time to create the mapping

            // Let's create rules first
            // Map Person Entity to Person Collection
            MapRule PersonRule = new MapRule( ErModel.FindByName( "Person" ), MSchema.Collections.Find( C => C.Name == "Person" ) );
            PersonRule.Rules.Add( "Name", "Name" );
            PersonRule.Rules.Add( "PersonId", "_id" );
            PersonRule.Rules.Add( "Age", "Age" );
            PersonRule.Rules.Add( "CarId", "CarId" );

            // Map Car Entity to Car Collection
            MapRule CarRule = new MapRule( ErModel.FindByName( "Car" ), MSchema.Collections.Find( C => C.Name == "Car" ) );
            CarRule.Rules.Add( "CarId", "_id" );
            CarRule.Rules.Add( "Model", "Model" );
            CarRule.Rules.Add( "Value", "Value" );

            // Build mapping rules
            List<MapRule> Rules = new List<MapRule>();
            Rules.AddRange( new MapRule[] { PersonRule, CarRule } );

            ModelMapping map = new ModelMapping( "PersonCarMap", Rules );

            // Everything ready, we'll need a query parser to generate pipeline from a query string
            // but ww'll skip it now (query parser not available)

            // Let's say our query is composed of a simple join
            // FROM Person RJOIN (Car Drives)

            // Only a JOIN operation is required
            JoinOperation JoinOp = new JoinOperation( (Entity)ErModel.FindByName( "Person" ), (Entity)ErModel.FindByName( "Car" ), (Relationship)ErModel.FindByName( "Drives" ), map );
            JoinOp.Relationship = (Relationship)ErModel.FindByName( "Drives" );

            JoinOperation ReverseJoin = new JoinOperation( (Entity)ErModel.FindByName( "Car" ), (Entity)ErModel.FindByName( "Person" ), (Relationship)ErModel.FindByName( "Drives" ), map );

            List<BaseOperation> Operations = new List<BaseOperation> {
                JoinOp,
                ReverseJoin
            };

            Pipeline QueryPipeline = new Pipeline( Operations );

            QueryGenerator QueryGen = new QueryGenerator( QueryPipeline );

            Console.WriteLine( string.Format( "Query output: {0}", QueryGen.Run() ) );

            Console.Read();
        }

        public static Model CreateERModel()
        {
            List<BaseERElement> ERElements = new List<BaseERElement>();
            Entity Person = new Entity( "Person" );
            Person.Attributes.Add( new DataAttribute( "Name" ) );
            Person.Attributes.Add( new DataAttribute( "PersonId" ) );
            Person.Attributes.Add( new DataAttribute( "Age" ) );
            Person.Attributes.Add( new DataAttribute( "CarId" ) );

            Entity Car = new Entity( "Car" );
            Car.Attributes.Add( new DataAttribute( "CarId" ) );
            Car.Attributes.Add( new DataAttribute( "Model" ) );
            Car.Attributes.Add( new DataAttribute( "Value" ) );

            Relationship Drives = new Relationship( "Drives" );
            Drives.Relates.AddRange( new Entity[] { Person, Car } );
            Drives.SourceAttribute = Person.Attributes.Find( A => A.Name == "CarId" );
            Drives.TargetAttribute = Car.Attributes.Find( A => A.Name == "CarId" );

            ERElements.AddRange( new BaseERElement[] { Person, Car, Drives } );

            return new Model( "PersonCarERModel", ERElements );
        }

        public static MongoSchema CreateMongoSchema()
        {
            Collection Person = new Collection( "Person" );

            Person.DocumentSchema.Attributes.Add( new DataAttribute( "_id" ) );
            Person.DocumentSchema.Attributes.Add( new DataAttribute( "Name" ) );
            Person.DocumentSchema.Attributes.Add( new DataAttribute( "Age" ) );
            Person.DocumentSchema.Attributes.Add( new DataAttribute( "CarId" ) );

            Collection Car = new Collection( "Car" );

            Car.DocumentSchema.Attributes.Add( new DataAttribute( "_id" ) );
            Car.DocumentSchema.Attributes.Add( new DataAttribute( "Model" ) );
            Car.DocumentSchema.Attributes.Add( new DataAttribute( "Value" ) );

            List<Collection> Collections = new List<Collection>();
            Collections.AddRange( new Collection[] { Person, Car } );

            return new MongoSchema( "PersonCarMongoSchema", Collections );
        }
    }
}