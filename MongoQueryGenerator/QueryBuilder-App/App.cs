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
            PersonRule.Rules.Add( "age", "age" );

            // Map Car Entity to Car Collection
            MapRule CarRule = new MapRule( ErModel.FindByName( "Car" ), MSchema.Collections.Find( C => C.Name == "Person" ) );
            CarRule.Rules.Add( "model", "drives.model" );
            CarRule.Rules.Add( "year", "drives.year" );

            MapRule InsuranceRule = new MapRule( ErModel.FindByName( "Insurance" ), MSchema.Collections.Find( C => C.Name == "Person" ) );
            InsuranceRule.Rules.Add( "companyName", "insurance.companyName" );
            InsuranceRule.Rules.Add( "benefit", "insurance.benefit" );

            // Build mapping rules
            List<MapRule> Rules = new List<MapRule>();
            Rules.AddRange( new MapRule[] { PersonRule, CarRule, InsuranceRule } );

            ModelMapping map = new ModelMapping( "PersonCar", Rules );

            // Everything ready, we'll need a query parser to generate pipeline from a query string
            // but we'll skip it now (query parser not available)
            //(Entity)ErModel.FindByName( "LifeInsurance" )
            JoinOperation JoinOP = new JoinOperation( (Entity)ErModel.FindByName( "Person" ),
                                                     (Relationship)ErModel.FindByName( "Drives" ),
                                                     new List<Entity> { (Entity)ErModel.FindByName( "Car" ) },
                                                     map );

            List<BaseOperation> Operations = new List<BaseOperation> {
                JoinOP
            };

            Pipeline QueryPipeline = new Pipeline( Operations );

            QueryGenerator QueryGen = new QueryGenerator( QueryPipeline )
            {
                CollectionName = "PersonDrivesCar"
            };

            Console.WriteLine( string.Format( "Query output: {0}", QueryGen.Run() ) );

            Console.Read();
        }

        public static Model CreateERModel()
        {
            Entity Person = new Entity( "Person" );
            Person.Attributes.Add( new DataAttribute( "personId" ) );
            Person.Attributes.Add( new DataAttribute( "name" ) );
            Person.Attributes.Add( new DataAttribute( "age" ) );

            Entity Car = new Entity( "Car" );
            Car.Attributes.Add( new DataAttribute( "model" ) );
            Car.Attributes.Add( new DataAttribute( "year" ) );

            Entity Insurance = new Entity( "Insurance" );
            Insurance.Attributes.Add( new DataAttribute( "companyName" ) );
            Insurance.Attributes.Add( new DataAttribute( "benefit" ) );

            Relationship Owns = new Relationship( "Drives", RelationshipCardinality.OneToMany );
            RelationshipConnection PersonCarConn = new RelationshipConnection( Person,
                                                                              Person.Attributes.Find( A => A.Name == "personId" ),
                                                                              Car,
                                                                              Car.Attributes.Find( A => A.Name == "carId" ),
                                                                              RelationshipCardinality.OneToMany );

            RelationshipConnection PersonInsuranceConn = new RelationshipConnection( Person,
                                                                                    Person.Attributes.Find( A => A.Name == "personId" ),
                                                                                    Insurance,
                                                                                    Insurance.Attributes.Find( A => A.Name == "insuranceId" ),
                                                                                    RelationshipCardinality.OneToMany );
            Owns.Relations.Add( PersonCarConn );
            Owns.Relations.Add( PersonInsuranceConn );

            Model ERModel = new Model( "PersonCarModel", new List<BaseERElement> { Person, Car, Owns, Insurance } );
            return ERModel;
        }

        public static MongoSchema CreateMongoSchema()
        {
            Collection Person = new Collection( "Person" );
            Person.DocumentSchema.Attributes.Add( new DataAttribute( "_id" ) );
            Person.DocumentSchema.Attributes.Add( new DataAttribute( "name" ) );
            Person.DocumentSchema.Attributes.Add( new DataAttribute( "age" ) );

            Collection Car = new Collection( "Car" );
            Car.DocumentSchema.Attributes.Add( new DataAttribute( "_id" ) );
            Car.DocumentSchema.Attributes.Add( new DataAttribute( "model" ) );
            Car.DocumentSchema.Attributes.Add( new DataAttribute( "year" ) );

            MongoSchema Schema = new MongoSchema( "PersonDrivesCar", new List<Collection> { Person, Car } );
            return Schema;
        }
    }
}