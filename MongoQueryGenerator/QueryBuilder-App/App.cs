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
            PersonRule.Rules.Add( "personId", "_id" );
            PersonRule.Rules.Add( "name", "name" );
            PersonRule.Rules.Add( "salary", "salary" );

            // Map Car Entity to Car Collection
            MapRule CarRule = new MapRule( ErModel.FindByName( "Car" ), MSchema.Collections.Find( C => C.Name == "Car" ) );
            CarRule.Rules.Add( "carId", "_id" );
            CarRule.Rules.Add( "name", "name" );

            MapRule InsuranceRule = new MapRule( ErModel.FindByName( "Insurance" ), MSchema.Collections.Find( C => C.Name == "Insurance" ) );
            InsuranceRule.Rules.Add( "insuranceId", "_id" );
            InsuranceRule.Rules.Add( "idCompany", "idCompany" );
            InsuranceRule.Rules.Add( "idPerson", "idPerson" );
            InsuranceRule.Rules.Add( "idCar", "idCar" );

            // Build mapping rules
            List<MapRule> Rules = new List<MapRule>();
            Rules.AddRange( new MapRule[] { PersonRule, CarRule, InsuranceRule } );

            ModelMapping map = new ModelMapping( "PersonInsCar", Rules );

            // Everything ready, we'll need a query parser to generate pipeline from a query string
            // but ww'll skip it now (query parser not available)

            JoinOperation JoinOP = new JoinOperation( (Entity)ErModel.FindByName( "Person" ),
                                                     (Relationship)ErModel.FindByName( "Insurance" ),
                                                     new List<Entity> { (Entity)ErModel.FindByName( "Car" ) },
                                                     map );

            List<BaseOperation> Operations = new List<BaseOperation> {
                JoinOP
            };

            Pipeline QueryPipeline = new Pipeline( Operations );

            QueryGenerator QueryGen = new QueryGenerator( QueryPipeline );
            QueryGen.CollectionName = "Person";

            Console.WriteLine( string.Format( "Query output: {0}", QueryGen.Run() ) );

            Console.Read();
        }

        public static Model CreateERModel()
        {
            Entity Person = new Entity( "Person" );
            Person.Attributes.Add( new DataAttribute( "personId" ) );
            Person.Attributes.Add( new DataAttribute( "name" ) );
            Person.Attributes.Add( new DataAttribute( "salary" ) );

            Entity Car = new Entity( "Car" );
            Car.Attributes.Add( new DataAttribute( "carId" ) );
            Car.Attributes.Add( new DataAttribute( "name" ) );

            Relationship Insurance = new Relationship( "Insurance" );
            Insurance.Attributes.Add( new DataAttribute( "insuranceId" ) );
            Insurance.Attributes.Add( new DataAttribute( "idCompany" ) );
            Insurance.Attributes.Add( new DataAttribute( "idPerson" ) );
            Insurance.Attributes.Add( new DataAttribute( "idCar" ) );

            RelationshipConnection PersonCarConn = new RelationshipConnection( Person,
                                                                              Person.Attributes.Find( A => A.Name == "personId" ),
                                                                              Insurance.Attributes.Find( A => A.Name == "idPerson" ),
                                                                              Car,
                                                                              Car.Attributes.Find( A => A.Name == "carId" ),
                                                                              Insurance.Attributes.Find( A => A.Name == "idCar" ),
                                                                              RelationshipCardinality.ManyToMany );

            Insurance.Relations.Add( PersonCarConn );

            Model ERModel = new Model( "PersonInsCar", new List<BaseERElement> { Person, Car, Insurance } );
            return ERModel;
        }

        public static MongoSchema CreateMongoSchema()
        {
            Collection Person = new Collection( "Person" );
            Person.DocumentSchema.Attributes.Add( new DataAttribute( "_id" ) );
            Person.DocumentSchema.Attributes.Add( new DataAttribute( "name" ) );
            Person.DocumentSchema.Attributes.Add( new DataAttribute( "salary" ) );

            Collection Car = new Collection( "Car" );
            Car.DocumentSchema.Attributes.Add( new DataAttribute( "_id" ) );
            Car.DocumentSchema.Attributes.Add( new DataAttribute( "name" ) );

            Collection Insurance = new Collection( "Insurance" );
            Insurance.DocumentSchema.Attributes.Add( new DataAttribute( "_id" ) );
            Insurance.DocumentSchema.Attributes.Add( new DataAttribute( "idCompany" ) );
            Insurance.DocumentSchema.Attributes.Add( new DataAttribute( "idPerson" ) );
            Insurance.DocumentSchema.Attributes.Add( new DataAttribute( "idCar" ) );

            MongoSchema Schema = new MongoSchema( "PersonInsuranceCar", new List<Collection> { Person, Car, Insurance } );
            return Schema;
        }
    }
}