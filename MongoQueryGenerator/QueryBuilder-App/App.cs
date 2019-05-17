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

            // Map Car Entity to Car Collection
            MapRule CarRule = new MapRule( ErModel.FindByName( "Car" ), MSchema.Collections.Find( C => C.Name == "Car" ) );
            CarRule.Rules.Add( "carId", "_id" );
            CarRule.Rules.Add( "name", "name" );

            MapRule InsCompanyRule = new MapRule( ErModel.FindByName( "InsCompany" ), MSchema.Collections.Find( C => C.Name == "InsCompany" ) );
            InsCompanyRule.Rules.Add( "companyId", "_id" );
            InsCompanyRule.Rules.Add( "name", "name" );

            MapRule InsuranceRule = new MapRule( ErModel.FindByName( "Insurance" ), MSchema.Collections.Find( C => C.Name == "Insurance" ) );
            InsuranceRule.Rules.Add( "insuranceId", "_id" );
            InsuranceRule.Rules.Add( "idCompany", "idCompany" );
            InsuranceRule.Rules.Add( "idPerson", "idPerson" );
            InsuranceRule.Rules.Add( "idCar", "idCar" );

            // Build mapping rules
            List<MapRule> Rules = new List<MapRule>();
            Rules.AddRange( new MapRule[] { PersonRule, CarRule, InsCompanyRule, InsuranceRule } );

            ModelMapping map = new ModelMapping( "PersonCar", Rules );

            // Everything ready, we'll need a query parser to generate pipeline from a query string
            // but we'll skip it now (query parser not available)
            //(Entity)ErModel.FindByName( "LifeInsurance" )
            JoinOperation JoinOP = new JoinOperation( (Entity)ErModel.FindByName( "Person" ),
                                                     (Relationship)ErModel.FindByName( "Insurance" ),
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
            Person.Attributes.Add( new DataAttribute( "salary" ) );

            Entity Car = new Entity( "Car" );
            Car.Attributes.Add( new DataAttribute( "carId" ) );
            Car.Attributes.Add( new DataAttribute( "name" ) );

            Entity InsCompany = new Entity( "InsCompany" );
            InsCompany.Attributes.Add( new DataAttribute( "companyId" ) );
            InsCompany.Attributes.Add( new DataAttribute( "name" ) );

            Relationship Insurance = new Relationship( "Insurance", RelationshipCardinality.ManyToMany );
            Insurance.Attributes.Add( new DataAttribute( "insuranceId" ) );
            Insurance.Attributes.Add( new DataAttribute( "idCompany" ) );
            Insurance.Attributes.Add( new DataAttribute( "idPerson" ) );
            Insurance.Attributes.Add( new DataAttribute( "idCar" ) );

            RelationshipConnection PersonCar = new RelationshipConnection(
                Person,
                Person.Attributes.Find( A => A.Name == "personId" ),
                Insurance.Attributes.Find( A => A.Name == "idPerson" ),
                Car,
                Car.Attributes.Find( A => A.Name == "carId" ),
                Insurance.Attributes.Find( A => A.Name == "idCar" ),
                RelationshipCardinality.ManyToMany
            );

            RelationshipConnection PersonInsCompany = new RelationshipConnection(
                Person,
                Person.Attributes.Find( A => A.Name == "personId" ),
                Insurance.Attributes.Find( A => A.Name == "idPerson" ),
                InsCompany,
                InsCompany.Attributes.Find( A => A.Name == "companyId" ),
                Insurance.Attributes.Find( A => A.Name == "idCompany" ),
                RelationshipCardinality.ManyToMany
            );

            Insurance.Relations.AddRange( new RelationshipConnection[] { PersonCar, PersonInsCompany } );

            Model ERModel = new Model( "PersonCarModel", new List<BaseERElement> { Person, Car, Insurance, InsCompany } );
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

            Collection InsCompany = new Collection( "InsCompany" );
            InsCompany.DocumentSchema.Attributes.Add( new DataAttribute( "_id" ) );
            InsCompany.DocumentSchema.Attributes.Add( new DataAttribute( "name" ) );

            Collection Insurance = new Collection( "Insurance" );
            Insurance.DocumentSchema.Attributes.Add( new DataAttribute( "_id" ) );
            Insurance.DocumentSchema.Attributes.Add( new DataAttribute( "idCompany" ) );
            Insurance.DocumentSchema.Attributes.Add( new DataAttribute( "idPerson" ) );
            Insurance.DocumentSchema.Attributes.Add( new DataAttribute( "idCar" ) );

            MongoSchema Schema = new MongoSchema( "PersonDrivesCar", new List<Collection> { Person, Car, InsCompany, Insurance } );
            return Schema;
        }
    }
}