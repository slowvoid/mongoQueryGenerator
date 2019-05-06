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
            MapRule PersonRule = new MapRule( ErModel.FindByName( "Person" ), MSchema.Collections.Find( C => C.Name == "Person" ) );
            PersonRule.Rules.Add( "personId", "_id" );
            PersonRule.Rules.Add( "name", "name" );
            PersonRule.Rules.Add( "salary", "salary" );
            PersonRule.Rules.Add( "carId", "carId" );
            PersonRule.Rules.Add( "insuranceId", "insuranceId" );

            // Map Car Entity to Car Collection
            MapRule CarRule = new MapRule( ErModel.FindByName( "Car" ), MSchema.Collections.Find( C => C.Name == "Car" ) );
            CarRule.Rules.Add( "carId", "_id" );
            CarRule.Rules.Add( "name", "name" );

            MapRule InsuranceRule = new MapRule( ErModel.FindByName( "LifeInsurance" ), MSchema.Collections.Find( C => C.Name == "LifeInsuranceOne" ) );
            InsuranceRule.Rules.Add( "insuranceId", "_id" );
            InsuranceRule.Rules.Add( "benefit", "benefit" );
            InsuranceRule.Rules.Add( "name", "name" );

            // Build mapping rules
            List<MapRule> Rules = new List<MapRule>();
            Rules.AddRange( new MapRule[] { PersonRule, CarRule, InsuranceRule } );

            ModelMapping map = new ModelMapping( "PersonHasCar", Rules );

            // Everything ready, we'll need a query parser to generate pipeline from a query string
            // but we'll skip it now (query parser not available)

            JoinOperation JoinOP = new JoinOperation( (Entity)ErModel.FindByName( "Person" ),
                                                     (Relationship)ErModel.FindByName( "HasCar" ),
                                                     new List<Entity> { (Entity)ErModel.FindByName( "Car" ), (Entity)ErModel.FindByName( "LifeInsurance" ) },
                                                     map );

            List<BaseOperation> Operations = new List<BaseOperation> {
                JoinOP
            };

            Pipeline QueryPipeline = new Pipeline( Operations );

            QueryGenerator QueryGen = new QueryGenerator( QueryPipeline )
            {
                CollectionName = "PersonOwnsCar"
            };

            Console.WriteLine( string.Format( "Query output: {0}", QueryGen.Run() ) );

            Console.Read();
        }

        public static Model CreateERModel()
        {
            Entity Person = new Entity( "Person" );
            Person.Attributes.Add( new DataAttribute( "personId" ) );
            Person.Attributes.Add( new DataAttribute( "name" ) );
            Person.Attributes.Add( new DataAttribute( "salary" ) );
            Person.Attributes.Add( new DataAttribute( "carId" ) );
            Person.Attributes.Add( new DataAttribute( "insuranceId" ) );

            Entity Car = new Entity( "Car" );
            Car.Attributes.Add( new DataAttribute( "carId" ) );
            Car.Attributes.Add( new DataAttribute( "name" ) );

            Entity LifeInsurance = new Entity( "LifeInsurance" );
            LifeInsurance.Attributes.Add( new DataAttribute( "insuranceId" ) );
            LifeInsurance.Attributes.Add( new DataAttribute( "benefit" ) );
            LifeInsurance.Attributes.Add( new DataAttribute( "name" ) );

            Relationship OwnsCar = new Relationship( "HasCar", RelationshipCardinality.OneToOne );
            RelationshipConnection PersonCarConn = new RelationshipConnection( Person, Person.Attributes.Find( A => A.Name == "carId" ), Car, Car.Attributes.Find( A => A.Name == "carId" ), RelationshipCardinality.OneToOne );
            RelationshipConnection PersonInsuranceConn = new RelationshipConnection( Person, Person.Attributes.Find( A => A.Name == "insuranceId" ), LifeInsurance, LifeInsurance.Attributes.Find( A => A.Name == "insuranceId" ), RelationshipCardinality.OneToOne );

            OwnsCar.Relations.Add( PersonCarConn );
            OwnsCar.Relations.Add( PersonInsuranceConn );

            Model ERModel = new Model( "PersonInsCar", new List<BaseERElement> { Person, Car, OwnsCar, LifeInsurance } );
            return ERModel;
        }

        public static MongoSchema CreateMongoSchema()
        {
            Collection Person = new Collection( "Person" );
            Person.DocumentSchema.Attributes.Add( new DataAttribute( "_id" ) );
            Person.DocumentSchema.Attributes.Add( new DataAttribute( "name" ) );
            Person.DocumentSchema.Attributes.Add( new DataAttribute( "salary" ) );
            Person.DocumentSchema.Attributes.Add( new DataAttribute( "carId" ) );
            Person.DocumentSchema.Attributes.Add( new DataAttribute( "insuranceId" ) );

            Collection Car = new Collection( "Car" );
            Car.DocumentSchema.Attributes.Add( new DataAttribute( "_id" ) );
            Car.DocumentSchema.Attributes.Add( new DataAttribute( "name" ) );

            Collection Insurance = new Collection( "LifeInsuranceOne" );
            Insurance.DocumentSchema.Attributes.Add( new DataAttribute( "_id" ) );
            Insurance.DocumentSchema.Attributes.Add( new DataAttribute( "benefid" ) );
            Insurance.DocumentSchema.Attributes.Add( new DataAttribute( "name" ) );

            MongoSchema Schema = new MongoSchema( "PersonHasCar", new List<Collection> { Person, Car, Insurance } );
            return Schema;
        }
    }
}