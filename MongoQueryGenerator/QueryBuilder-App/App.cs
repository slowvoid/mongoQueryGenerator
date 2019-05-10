using System;
using System.Collections.Generic;
using QueryBuilder.ER;
using QueryBuilder.Map;
using QueryBuilder.Mongo;
using QueryBuilder.Mongo.Expressions;
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
            MapRule PersonRule = new MapRule( ErModel.FindByName( "PersonOne" ), MSchema.Collections.Find( C => C.Name == "PersonOne" ) );
            PersonRule.Rules.Add( "personId", "_id" );
            PersonRule.Rules.Add( "name", "name" );
            PersonRule.Rules.Add( "age", "age" );

            // Map Car Entity to Car Collection
            MapRule CarRule = new MapRule( ErModel.FindByName( "CarOne" ), MSchema.Collections.Find( C => C.Name == "CarOne" ) );
            CarRule.Rules.Add( "carId", "_id" );
            CarRule.Rules.Add( "model", "model" );
            CarRule.Rules.Add( "year", "year" );
            CarRule.Rules.Add( "ownerId", "ownerId" );

            // Build mapping rules
            List<MapRule> Rules = new List<MapRule>();
            Rules.AddRange( new MapRule[] { PersonRule, CarRule } );

            ModelMapping map = new ModelMapping( "PersonCar", Rules );

            // Everything ready, we'll need a query parser to generate pipeline from a query string
            // but we'll skip it now (query parser not available)
            //(Entity)ErModel.FindByName( "LifeInsurance" )
            JoinOperation JoinOP = new JoinOperation( (Entity)ErModel.FindByName( "PersonOne" ),
                                                     (Relationship)ErModel.FindByName( "Owns" ),
                                                     new List<Entity> { (Entity)ErModel.FindByName( "CarOne" ) },
                                                     map );

            List<BaseOperation> Operations = new List<BaseOperation> {
                JoinOP
            };

            Pipeline QueryPipeline = new Pipeline( Operations );

            QueryGenerator QueryGen = new QueryGenerator( QueryPipeline )
            {
                CollectionName = "PersonOne"
            };

            Console.WriteLine( string.Format( "Query output: {0}", QueryGen.Run() ) );

            Console.Read();
        }

        public static Model CreateERModel()
        {
            Entity Person = new Entity( "PersonOne" );
            Person.Attributes.Add( new DataAttribute( "personId" ) );
            Person.Attributes.Add( new DataAttribute( "name" ) );
            Person.Attributes.Add( new DataAttribute( "age" ) );

            Entity Car = new Entity( "CarOne" );
            Car.Attributes.Add( new DataAttribute( "carId" ) );
            Car.Attributes.Add( new DataAttribute( "model" ) );
            Car.Attributes.Add( new DataAttribute( "year" ) );
            Car.Attributes.Add( new DataAttribute( "ownerId" ) );

            Relationship Owns = new Relationship( "Owns", RelationshipCardinality.OneToMany );
            RelationshipConnection PersonCarConn = new RelationshipConnection( Person, Person.Attributes.Find( A => A.Name == "personId" ), Car, Car.Attributes.Find( A => A.Name == "ownerId" ), RelationshipCardinality.OneToMany );

            Owns.Relations.Add( PersonCarConn );

            Model ERModel = new Model( "PersonCarModel", new List<BaseERElement> { Person, Car, Owns } );
            return ERModel;
        }

        public static MongoSchema CreateMongoSchema()
        {
            Collection Person = new Collection( "PersonOne" );
            Person.DocumentSchema.Attributes.Add( new DataAttribute( "_id" ) );
            Person.DocumentSchema.Attributes.Add( new DataAttribute( "name" ) );
            Person.DocumentSchema.Attributes.Add( new DataAttribute( "age" ) );

            Collection Car = new Collection( "CarOne" );
            Car.DocumentSchema.Attributes.Add( new DataAttribute( "_id" ) );
            Car.DocumentSchema.Attributes.Add( new DataAttribute( "model" ) );
            Car.DocumentSchema.Attributes.Add( new DataAttribute( "year" ) );
            Car.DocumentSchema.Attributes.Add( new DataAttribute( "ownerId" ) );

            MongoSchema Schema = new MongoSchema( "PersonHasCar", new List<Collection> { Person, Car } );
            return Schema;
        }
    }
}