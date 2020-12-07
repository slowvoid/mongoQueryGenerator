using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using QueryBuilder.ER;
using QueryBuilder.Operation;
using QueryBuilder.Operation.Arguments;
using QueryBuilder.Parser;
using QueryBuilder.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueryBuilder.Tests
{
    [TestClass]
    public class OneToOneRelationshipsTests
    {
        [TestMethod]
        public void OneToOneNotembedded()
        {
            // Asserts if the query result for a simple binary join is equal
            // to a handcrafted query
            //RequiredDataContainer ModelData = OneToOneRelationshipsDataProvider.OneToOneNotEmbedded();
            var ModelData = QueryBuilderParser.ParseMapping( Utils.ReadMappingFile( "Mappings/one-to-one-not-embedded.mapping" ) );

            // Load handcrafted query
            string HandcraftedQuery = Utils.ReadQueryFromFile( "HandcraftedQueries/personCarOneToOne_1.js" );

            // Assert if the handcrafted query is not null
            Assert.IsNotNull( HandcraftedQuery );

            // Prepare query generator
            RelationshipJoinOperator RJoinOp = new RelationshipJoinOperator(
                new QueryableEntity( ( Entity)ModelData.EntityRelationshipModel.FindByName( "Person" ), "person" ),
                (Relationship)ModelData.EntityRelationshipModel.FindByName( "Drives" ),
                 new List<QueryableEntity> {
                    new QueryableEntity( (Entity)ModelData.EntityRelationshipModel.FindByName( "Car" ), "car" )
                },
                ModelData.ERMongoMapping );

            List<AlgebraOperator> OpList = new List<AlgebraOperator> { RJoinOp };
            FromArgument StartArg = new FromArgument( new QueryableEntity( ModelData.EntityRelationshipModel.FindByName( "Person" ) ),
                ModelData.ERMongoMapping );

            QueryGenerator QueryGen = new QueryGenerator( StartArg, OpList );

            string GeneratedQuery = QueryGen.Run();

            // Assert if generated query is not null
            Assert.IsNotNull( GeneratedQuery );

            // Run Queries
            QueryRunner Runner = new QueryRunner( "mongodb://localhost:27017", "researchDatabase" );

            string HandcraftedResult = Runner.GetJSON( HandcraftedQuery );
            string GeneratedResult = Runner.GetJSON( GeneratedQuery );

            // Check if either result is null
            Assert.IsNotNull( HandcraftedResult );
            Assert.IsNotNull( GeneratedResult );

            // Check if both results are equal
            Assert.IsTrue( JToken.DeepEquals( JToken.Parse( HandcraftedResult ), JToken.Parse( GeneratedResult ) ) );
        }
        [TestMethod]
        public void OneToOneEmbedded()
        {
            // Asserts if the query result for a simple binary join is equal
            // to a handcrafted query
            //RequiredDataContainer ModelData = OneToOneRelationshipsDataProvider.OneToOneEmbedded();
            var ModelData = QueryBuilderParser.ParseMapping( Utils.ReadMappingFile( "Mappings/one-to-one-embedded.mapping" ) );

            // Load handcrafted query
            string HandcraftedQuery = Utils.ReadQueryFromFile( "HandcraftedQueries/personCarOneToOne_2.js" );

            // Assert if the handcrafted query is not null
            Assert.IsNotNull( HandcraftedQuery );

            // Prepare query generator
            RelationshipJoinOperator RJoinOp = new RelationshipJoinOperator(
                new QueryableEntity( ( Entity)ModelData.EntityRelationshipModel.FindByName( "Person" ), "person" ),
                (Relationship)ModelData.EntityRelationshipModel.FindByName( "Drives" ),
                new List<QueryableEntity> {
                    new QueryableEntity( (Entity)ModelData.EntityRelationshipModel.FindByName( "Car" ), "car" )
                },
                ModelData.ERMongoMapping );

            List<AlgebraOperator> OpList = new List<AlgebraOperator> { RJoinOp };
            FromArgument StartArg = new FromArgument( new QueryableEntity( ModelData.EntityRelationshipModel.FindByName( "Person" ) ),
                ModelData.ERMongoMapping );

            QueryGenerator QueryGen = new QueryGenerator( StartArg, OpList );

            string GeneratedQuery = QueryGen.Run();

            // Assert if generated query is not null
            Assert.IsNotNull( GeneratedQuery );

            // Run Queries
            QueryRunner Runner = new QueryRunner( "mongodb://localhost:27017", "researchDatabase" );

            string HandcraftedResult = Runner.GetJSON( HandcraftedQuery );
            string GeneratedResult = Runner.GetJSON( GeneratedQuery );

            // Check if either result is null
            Assert.IsNotNull( HandcraftedResult );
            Assert.IsNotNull( GeneratedResult );

            // Check if both results are equal
            Assert.IsTrue( JToken.DeepEquals( JToken.Parse( HandcraftedResult ), JToken.Parse( GeneratedResult ) ) );
        }
        [TestMethod]
        public void OneToOneEmbeddedNoMasterAttribute()
        {
            // Asserts if the query result for a simple binary join is equal
            // to a handcrafted query
            //RequiredDataContainer ModelData = OneToOneRelationshipsDataProvider.OneToOneEmbeddedNoMasterAttribute();
            var ModelData = QueryBuilderParser.ParseMapping( Utils.ReadMappingFile( "Mappings/one-to-one-embedded-no-master-attribute.mapping" ) );

            // Load handcrafted query
            string HandcraftedQuery = Utils.ReadQueryFromFile( "HandcraftedQueries/personCarOneToOne_3.js" );

            // Assert if the handcrafted query is not null
            Assert.IsNotNull( HandcraftedQuery );

            // Prepare query generator
            RelationshipJoinOperator RJoinOp = new RelationshipJoinOperator(
                new QueryableEntity( ( Entity)ModelData.EntityRelationshipModel.FindByName( "Person" ), "person" ),
                (Relationship)ModelData.EntityRelationshipModel.FindByName( "Drives" ),
                new List<QueryableEntity> {
                    new QueryableEntity( (Entity)ModelData.EntityRelationshipModel.FindByName( "Car" ), "car" )
                },
                ModelData.ERMongoMapping );

            List<AlgebraOperator> OpList = new List<AlgebraOperator> { RJoinOp };
            FromArgument StartArg = new FromArgument( new QueryableEntity( ModelData.EntityRelationshipModel.FindByName( "Person" ) ),
                ModelData.ERMongoMapping );

            QueryGenerator QueryGen = new QueryGenerator( StartArg, OpList );

            string GeneratedQuery = QueryGen.Run();

            // Assert if generated query is not null
            Assert.IsNotNull( GeneratedQuery );

            // Run Queries
            QueryRunner Runner = new QueryRunner( "mongodb://localhost:27017", "researchDatabase" );

            string HandcraftedResult = Runner.GetJSON( HandcraftedQuery );
            string GeneratedResult = Runner.GetJSON( GeneratedQuery );

            // Check if either result is null
            Assert.IsNotNull( HandcraftedResult );
            Assert.IsNotNull( GeneratedResult );

            // Check if both results are equal
            Assert.IsTrue( JToken.DeepEquals( JToken.Parse( HandcraftedResult ), JToken.Parse( GeneratedResult ) ) );
        }

        public void OneToOneEmbeddedMixed()
        {
            //TODO: Check why it isn't working and why it didn't have a TestMethod attribute before
            // Asserts if the query result for a simple binary join is equal
            // to a handcrafted query
            //RequiredDataContainer ModelData = OneToOneRelationshipsDataProvider.OneToOneEmbeddedMixed();
            var ModelData = QueryBuilderParser.ParseMapping( Utils.ReadMappingFile( "Mappings/one-to-one-embedded-mixed.mapping" ) );

            // Load handcrafted query
            string HandcraftedQuery = Utils.ReadQueryFromFile( "HandcraftedQueries/personCarOneToOne_4.js" );

            // Assert if the handcrafted query is not null
            Assert.IsNotNull( HandcraftedQuery );

            // Prepare query generator
            RelationshipJoinOperator RJoinOp = new RelationshipJoinOperator(
                new QueryableEntity( ( Entity)ModelData.EntityRelationshipModel.FindByName( "Person" ), "person" ),
                (Relationship)ModelData.EntityRelationshipModel.FindByName( "Drives" ),
                new List<QueryableEntity> {
                    new QueryableEntity( (Entity)ModelData.EntityRelationshipModel.FindByName( "Car" ), "car" )
                },
                ModelData.ERMongoMapping );

            List<AlgebraOperator> OpList = new List<AlgebraOperator> { RJoinOp };
            FromArgument StartArg = new FromArgument( new QueryableEntity( ModelData.EntityRelationshipModel.FindByName( "Person" ) ),
                ModelData.ERMongoMapping );

            QueryGenerator QueryGen = new QueryGenerator( StartArg, OpList );

            string GeneratedQuery = QueryGen.Run();

            // Assert if generated query is not null
            Assert.IsNotNull( GeneratedQuery );

            // Run Queries
            QueryRunner Runner = new QueryRunner( "mongodb://localhost:27017", "researchDatabase" );

            string HandcraftedResult = Runner.GetJSON( HandcraftedQuery );
            string GeneratedResult = Runner.GetJSON( GeneratedQuery );

            // Check if either result is null
            Assert.IsNotNull( HandcraftedResult );
            Assert.IsNotNull( GeneratedResult );

            // Check if both results are equal
            Assert.IsTrue( JToken.DeepEquals( JToken.Parse( HandcraftedResult ), JToken.Parse( GeneratedResult ) ) );
        }
        [TestMethod]
        public void OneToOneNotEmbeddedMultipleEntities()
        {
            // Asserts if the query result for a simple binary join is equal
            // to a handcrafted query
            //RequiredDataContainer ModelData = OneToOneRelationshipsDataProvider.OneToOneNotEmbeddedMultipleEntities();
            var ModelData = QueryBuilderParser.ParseMapping( Utils.ReadMappingFile( "Mappings/one-to-one-not-embedded-multiple-entities.mapping" ) );

            // Load handcrafted query
            string HandcraftedQuery = Utils.ReadQueryFromFile( "HandcraftedQueries/personCarInsuranceOneToOne.js" );

            // Assert if the handcrafted query is not null
            Assert.IsNotNull( HandcraftedQuery );

            // Prepare query generator
            RelationshipJoinOperator RJoinOp = new RelationshipJoinOperator(
                new QueryableEntity( ( Entity)ModelData.EntityRelationshipModel.FindByName( "Person" ), "person" ),
                (Relationship)ModelData.EntityRelationshipModel.FindByName( "HasInsurance" ),
                new List<QueryableEntity> {
                    new QueryableEntity( (Entity)ModelData.EntityRelationshipModel.FindByName( "Car" ), "car" ),
                    new QueryableEntity( (Entity)ModelData.EntityRelationshipModel.FindByName( "Insurance" ), "insurance" )
                },
                ModelData.ERMongoMapping );

            List<AlgebraOperator> OpList = new List<AlgebraOperator> { RJoinOp };
            FromArgument StartArg = new FromArgument( new QueryableEntity( ModelData.EntityRelationshipModel.FindByName( "Person" ) ),
                ModelData.ERMongoMapping );

            QueryGenerator QueryGen = new QueryGenerator( StartArg, OpList );

            string GeneratedQuery = QueryGen.Run();

            // Assert if generated query is not null
            Assert.IsNotNull( GeneratedQuery );

            // Run Queries
            QueryRunner Runner = new QueryRunner( "mongodb://localhost:27017", "researchOneToOneMultiple" );

            string HandcraftedResult = Runner.GetJSON( HandcraftedQuery );
            string GeneratedResult = Runner.GetJSON( GeneratedQuery );

            // Check if either result is null
            Assert.IsNotNull( HandcraftedResult );
            Assert.IsNotNull( GeneratedResult );

            // Check if both results are equal
            Assert.IsTrue( JToken.DeepEquals( JToken.Parse( HandcraftedResult ), JToken.Parse( GeneratedResult ) ) );
        }
        [TestMethod]
        public void OneToOneMultipleEntitiesMixed()
        {
            // Asserts if the query result for a simple binary join is equal
            // to a handcrafted query
            //RequiredDataContainer ModelData = OneToOneRelationshipsDataProvider.OneToOneMultipleEntitiesMixed();
            var ModelData = QueryBuilderParser.ParseMapping( Utils.ReadMappingFile( "Mappings/one-to-one-multiple-entities-mixed.mapping" ) );

            // Load handcrafted query
            string HandcraftedQuery = Utils.ReadQueryFromFile( "HandcraftedQueries/personCarInsuranceOneToOneMultipleMixed.js" );

            // Assert if the handcrafted query is not null
            Assert.IsNotNull( HandcraftedQuery );

            // Prepare query generator
            RelationshipJoinOperator RJoinOp = new RelationshipJoinOperator(
                new QueryableEntity( ( Entity)ModelData.EntityRelationshipModel.FindByName( "Person" ), "person" ),
                (Relationship)ModelData.EntityRelationshipModel.FindByName( "HasInsurance" ),
                new List<QueryableEntity> {
                    new QueryableEntity( (Entity)ModelData.EntityRelationshipModel.FindByName( "Car" ), "car" ),
                    new QueryableEntity( (Entity)ModelData.EntityRelationshipModel.FindByName( "Insurance" ), "insurance" )
                },
                ModelData.ERMongoMapping );

            List<AlgebraOperator> OpList = new List<AlgebraOperator> { RJoinOp };
            FromArgument StartArg = new FromArgument( new QueryableEntity( ModelData.EntityRelationshipModel.FindByName( "Person" ) ),
                ModelData.ERMongoMapping );

            QueryGenerator QueryGen = new QueryGenerator( StartArg, OpList );

            string GeneratedQuery = QueryGen.Run();

            // Assert if generated query is not null
            Assert.IsNotNull( GeneratedQuery );

            // Run Queries
            QueryRunner Runner = new QueryRunner( "mongodb://localhost:27017", "researchOneToOneMultipleMixed" );

            string HandcraftedResult = Runner.GetJSON( HandcraftedQuery );
            string GeneratedResult = Runner.GetJSON( GeneratedQuery );

            // Check if either result is null
            Assert.IsNotNull( HandcraftedResult );
            Assert.IsNotNull( GeneratedResult );

            // Check if both results are equal
            Assert.IsTrue( JToken.DeepEquals( JToken.Parse( HandcraftedResult ), JToken.Parse( GeneratedResult ) ) );
        }
        [TestMethod]
        public void OneToOneRelationshipAttributes()
        {
            // Asserts if the query result for a simple binary join is equal
            // to a handcrafted query
            //RequiredDataContainer ModelData = OneToOneRelationshipsDataProvider.OneToOneRelationshipAttributes();
            var ModelData = QueryBuilderParser.ParseMapping( Utils.ReadMappingFile( "Mappings/one-to-one-relationship-attributes.mapping" ) );

            // Load handcrafted query
            string HandcraftedQuery = Utils.ReadQueryFromFile( "HandcraftedQueries/personCarOneToOneRelationshipAttributes.js" );

            // Assert if the handcrafted query is not null
            Assert.IsNotNull( HandcraftedQuery );

            // Prepare query generator
            RelationshipJoinOperator RJoinOp = new RelationshipJoinOperator(
                new QueryableEntity( ( Entity)ModelData.EntityRelationshipModel.FindByName( "Person" ), "person" ),
                (Relationship)ModelData.EntityRelationshipModel.FindByName( "HasInsurance" ),
                new List<QueryableEntity> {
                    new QueryableEntity( (Entity)ModelData.EntityRelationshipModel.FindByName( "Car" ), "car" ),
                    new QueryableEntity( (Entity)ModelData.EntityRelationshipModel.FindByName( "Insurance" ), "insurance" )
                },
                ModelData.ERMongoMapping );

            List<AlgebraOperator> OpList = new List<AlgebraOperator> { RJoinOp };
            FromArgument StartArg = new FromArgument( new QueryableEntity( ModelData.EntityRelationshipModel.FindByName( "Person" ) ),
                ModelData.ERMongoMapping );

            QueryGenerator QueryGen = new QueryGenerator( StartArg, OpList );

            string GeneratedQuery = QueryGen.Run();

            // Assert if generated query is not null
            Assert.IsNotNull( GeneratedQuery );

            // Run Queries
            QueryRunner Runner = new QueryRunner( "mongodb://localhost:27017", "researchOneToOneRelationshipAttribute" );

            string HandcraftedResult = Runner.GetJSON( HandcraftedQuery );
            string GeneratedResult = Runner.GetJSON( GeneratedQuery );

            // Check if either result is null
            Assert.IsNotNull( HandcraftedResult );
            Assert.IsNotNull( GeneratedResult );

            // Check if both results are equal
            Assert.IsTrue( JToken.DeepEquals( JToken.Parse( HandcraftedResult ), JToken.Parse( GeneratedResult ) ) );
        }

        public void OneToOneRelationshipAttributesMultipleRoots()
        {
            // Asserts if the query result for a simple binary join is equal
            // to a handcrafted query
            RequiredDataContainer ModelData = OneToOneRelationshipsDataProvider.OneToOneRelationshipMultipleRootAttributes();

            // Load handcrafted query
            string HandcraftedQuery = Utils.ReadQueryFromFile( "HandcraftedQueries/personCarOneToOneMultipleRootAttributes.js" );

            // Assert if the handcrafted query is not null
            Assert.IsNotNull( HandcraftedQuery );

            // Prepare query generator
            RelationshipJoinOperator RJoinOp = new RelationshipJoinOperator(
                new QueryableEntity( ( Entity)ModelData.EntityRelationshipModel.FindByName( "Person" ), "person" ),
                (Relationship)ModelData.EntityRelationshipModel.FindByName( "HasInsurance" ),
                new List<QueryableEntity> {
                    new QueryableEntity( (Entity)ModelData.EntityRelationshipModel.FindByName( "Car" ), "car" ),
                    new QueryableEntity( (Entity)ModelData.EntityRelationshipModel.FindByName( "Insurance" ), "insurance" )
                },
                ModelData.ERMongoMapping );

            List<AlgebraOperator> OpList = new List<AlgebraOperator> { RJoinOp };
            FromArgument StartArg = new FromArgument( new QueryableEntity( ModelData.EntityRelationshipModel.FindByName( "Person" ) ),
                ModelData.ERMongoMapping );

            QueryGenerator QueryGen = new QueryGenerator( StartArg, OpList );

            string GeneratedQuery = QueryGen.Run();

            // Assert if generated query is not null
            Assert.IsNotNull( GeneratedQuery );

            // Run Queries
            QueryRunner Runner = new QueryRunner( "mongodb://localhost:27017", "researchOneToOneMultipleRoots" );

            string HandcraftedResult = Runner.GetJSON( HandcraftedQuery );
            string GeneratedResult = Runner.GetJSON( GeneratedQuery );

            // Check if either result is null
            Assert.IsNotNull( HandcraftedResult );
            Assert.IsNotNull( GeneratedResult );

            // Check if both results are equal
            Assert.IsTrue( JToken.DeepEquals( JToken.Parse( HandcraftedResult ), JToken.Parse( GeneratedResult ) ) );
        }
    }
}
