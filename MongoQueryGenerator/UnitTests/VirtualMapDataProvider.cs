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
            Person.SetIdentifier( "personId" );

            Entity Pet = new Entity( "Pet" );
            Pet.AddAttributes( "petId", "name", "type" );
            Pet.SetIdentifier( "petId" );

            Relationship HasPet = new Relationship( "HasPet" );
            HasPet.AddRelationshipEnd( new RelationshipEnd( Person ) );
            HasPet.AddRelationshipEnd( new RelationshipEnd( Pet ) );

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

            MapRule PersonPetRule = new MapRule( Person, PetCol, false );
            PersonPetRule.AddRule( "personId", "ownerId" );

            ModelMapping Map = new ModelMapping( "Map", new List<MapRule>() { PersonRules, PetRules, PersonPetRule } );

            return new RequiredDataContainer( Model, Schema, Map );
        }
    }
}