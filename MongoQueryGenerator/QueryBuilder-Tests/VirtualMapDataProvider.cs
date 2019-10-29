using QueryBuilder.ER;
using QueryBuilder.Map;
using QueryBuilder.Mongo;
using System.Collections.Generic;

namespace QueryBuilder.Tests
{
    public static class VirtualMapDataProvider
    {
        /// <summary>
        /// Test data for virtual maps
        /// </summary>
        /// <returns></returns>
        public static RequiredDataContainer VirtualMapModel()
        {
            // ER Stuff
            Entity Person = new Entity( "Person" );
            Person.AddAttributes( "personId", "name", "age" );

            Entity Pet = new Entity( "Pet" );
            Pet.AddAttributes( "petId", "name", "type", "ownerId" );

            Relationship HasPet = new Relationship( "HasPet", RelationshipCardinality.OneToMany );
            RelationshipConnection PersonHasPet = new RelationshipConnection(
                Person,
                Person.GetAttribute( "personId" ),
                Pet,
                Pet.GetAttribute( "ownerId" ) );
            HasPet.AddRelation( PersonHasPet );

            ERModel Model = new ERModel( "Model", new List<BaseERElement>() { Person, Pet, HasPet } );

            MongoDBCollection PersonCol = new MongoDBCollection( "Person" );
            PersonCol.AddAttributes( "_id", "name", "age" );

            MongoDBCollection PetCol = new MongoDBCollection( "Pet" );
            PetCol.AddAttributes( "_id", "name", "type", "ownerId" );

            MongoSchema Schema = new MongoSchema( "Schema", new List<MongoDBCollection>() { PersonCol, PetCol } );

            MapRule PersonRules = new MapRule( Person, PersonCol );
            PersonRules.AddRule( "personId", "_id" );
            PersonRules.AddRule( "name", "name" );
            PersonRules.AddRule( "age", "age" );

            MapRule PetRules = new MapRule( Pet, PetCol );
            PetRules.AddRule( "petId", "_id" );
            PetRules.AddRule( "name", "name" );
            PetRules.AddRule( "type", "type" );
            PetRules.AddRule( "ownerId", "ownerId" );

            ModelMapping Map = new ModelMapping( "Map", new List<MapRule>() { PersonRules, PetRules } );

            return new RequiredDataContainer( Model, Schema, Map );
        }
    }
}