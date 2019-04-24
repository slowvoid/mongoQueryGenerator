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
            CarRule.Rules.Add( "OwnedBy", "OwnedBy" );

            MapRule OwnsRule = new MapRule( ErModel.FindByName( "Owns" ), MSchema.Collections.Find( C => C.Name == "Owns" ) );
            OwnsRule.Rules.Add( "OwnsId", "OwnsId" );
            OwnsRule.Rules.Add( "Whatever", "Whatever" );
            OwnsRule.Rules.Add( "CarId", "CarId" );
            OwnsRule.Rules.Add( "PersonId", "PersonId" );

            // Build mapping rules
            List<MapRule> Rules = new List<MapRule>();
            Rules.AddRange( new MapRule[] { PersonRule, CarRule, OwnsRule } );

            ModelMapping map = new ModelMapping( "PersonCarMap", Rules );

            // Everything ready, we'll need a query parser to generate pipeline from a query string
            // but ww'll skip it now (query parser not available)

            // Let's say our query is composed of a simple join
            // FROM Person RJOIN (Car Drives)

            // Only a JOIN operation is required
            JoinOperation JoinOp = new JoinOperation( (Entity)ErModel.FindByName( "Person" ), (Entity)ErModel.FindByName( "Car" ), (Relationship)ErModel.FindByName( "Drives" ), map );
            JoinOp.Relationship = (Relationship)ErModel.FindByName( "Drives" );

            JoinOperation ReverseJoin = new JoinOperation( (Entity)ErModel.FindByName( "Car" ), (Entity)ErModel.FindByName( "Person" ), (Relationship)ErModel.FindByName( "Drives" ), map );
            JoinOperation Owns = new JoinOperation( (Entity)ErModel.FindByName( "Person" ), (Entity)ErModel.FindByName( "Car" ), (Relationship)ErModel.FindByName( "Owns" ), map );
            Dictionary<string, bool> ProjectAttributes = new Dictionary<string, bool>();
            ProjectAttributes.Add( "Person.Name", true );
            ProjectAttributes.Add( "Person.PersonId", false );
            ProjectAttributes.Add( "Car.Model", true );
            ProjectAttributes.Add( "Owns.Whatever", true );

            ProjectOperation ProjectOp = new ProjectOperation( ProjectAttributes, map );

            List<BaseOperation> Operations = new List<BaseOperation> {
                Owns,
                //ProjectOp
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
            Car.Attributes.Add( new DataAttribute( "OwnedBy" ) );

            Relationship Drives = new Relationship( "Drives" );
            Drives.Relations.Add(new RelationshipConnection(Person,
                                                            Person.Attributes.Find(A => A.Name == "CarId"),
                                                            Car,
                                                            Car.Attributes.Find(A => A.Name == "CarId"),
                                                            RelationshipCardinality.OneToOne));

            Relationship Owns = new Relationship( "Owns" );
            Owns.Relations.Add(new RelationshipConnection(Person,
                                                          Person.Attributes.Find(A => A.Name == "PersonId"),
                                                          Owns.Attributes.Find(A => A.Name == "PersonId"),
                                                          Car,
                                                          Car.Attributes.Find(A => A.Name == "CarId"),
                                                          Owns.Attributes.Find(A => A.Name == "CarId"),
                                                          RelationshipCardinality.ManyToMany));

            ERElements.AddRange( new BaseERElement[] { Person, Car, Drives, Owns } );

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
            Car.DocumentSchema.Attributes.Add( new DataAttribute( "OwnedBy" ) );
            Car.DocumentSchema.Attributes.Add( new DataAttribute( "Whatever" ) );

            Collection Owns = new Collection( "Owns" );
            Owns.DocumentSchema.Attributes.Add( new DataAttribute( "OwnsId" ) );
            Owns.DocumentSchema.Attributes.Add( new DataAttribute( "Whatever" ) );
            Owns.DocumentSchema.Attributes.Add( new DataAttribute( "CarId" ) );
            Owns.DocumentSchema.Attributes.Add( new DataAttribute( "PersonId" ) );
            
            List<Collection> Collections = new List<Collection>();
            Collections.AddRange( new Collection[] { Person, Car, Owns } );

            return new MongoSchema( "PersonCarMongoSchema", Collections );
        }
    }
}