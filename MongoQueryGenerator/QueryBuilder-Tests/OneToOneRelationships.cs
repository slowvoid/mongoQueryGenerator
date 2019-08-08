using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using QueryBuilder.ER;
using QueryBuilder.Operation;
using QueryBuilder.Operation.Arguments;
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
            RequiredDataContainer ModelData = OneToOneRelationshipsDataProvider.OneToOneNotEmbedded();

            // Load handcrafted query
            string HandcraftedQuery = Utils.ReadQueryFromFile( "HandcraftedQueries/personCarOneToOne_1.js" );

            // Assert if the handcrafted query is not null
            Assert.IsNotNull( HandcraftedQuery );

            // Prepare query generator
            RelationshipJoinArgument RJoinArgs = new RelationshipJoinArgument(
                (Relationship)ModelData.EntityRelationshipModel.FindByName( "Drives" ),
                new List<JoinableEntity> {
                    new JoinableEntity( (Entity)ModelData.EntityRelationshipModel.FindByName( "Car" ), "car" )
                } );

            RelationshipJoinOperator RJoinOp = new RelationshipJoinOperator(
                new JoinableEntity( ( Entity)ModelData.EntityRelationshipModel.FindByName( "Person" ), "person" ),
                new List<RelationshipJoinArgument> { RJoinArgs },
                ModelData.ERMongoMapping );

            List<AlgebraOperator> OpList = new List<AlgebraOperator> { RJoinOp };
            Pipeline pipeline = new Pipeline( OpList );
            QueryGenerator QueryGen = new QueryGenerator( pipeline )
            {
                CollectionName = "Person"
            };

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
            RequiredDataContainer ModelData = OneToOneRelationshipsDataProvider.OneToOneEmbedded();

            // Load handcrafted query
            string HandcraftedQuery = Utils.ReadQueryFromFile( "HandcraftedQueries/personCarOneToOne_2.js" );

            // Assert if the handcrafted query is not null
            Assert.IsNotNull( HandcraftedQuery );

            // Prepare query generator
            RelationshipJoinArgument RJoinArgs = new RelationshipJoinArgument(
                (Relationship)ModelData.EntityRelationshipModel.FindByName( "Drives" ),
                new List<JoinableEntity> {
                    new JoinableEntity( (Entity)ModelData.EntityRelationshipModel.FindByName( "Car" ), "car" )
                } );

            RelationshipJoinOperator RJoinOp = new RelationshipJoinOperator(
                new JoinableEntity( ( Entity)ModelData.EntityRelationshipModel.FindByName( "Person" ), "person" ),
                new List<RelationshipJoinArgument> { RJoinArgs },
                ModelData.ERMongoMapping );

            List<AlgebraOperator> OpList = new List<AlgebraOperator> { RJoinOp };
            Pipeline pipeline = new Pipeline( OpList );
            QueryGenerator QueryGen = new QueryGenerator( pipeline )
            {
                CollectionName = "PersonDrives"
            };

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
            RequiredDataContainer ModelData = OneToOneRelationshipsDataProvider.OneToOneEmbeddedNoMasterAttribute();

            // Load handcrafted query
            string HandcraftedQuery = Utils.ReadQueryFromFile( "HandcraftedQueries/personCarOneToOne_3.js" );

            // Assert if the handcrafted query is not null
            Assert.IsNotNull( HandcraftedQuery );

            // Prepare query generator
            RelationshipJoinArgument RJoinArgs = new RelationshipJoinArgument(
                (Relationship)ModelData.EntityRelationshipModel.FindByName( "Drives" ),
                new List<JoinableEntity> {
                    new JoinableEntity( (Entity)ModelData.EntityRelationshipModel.FindByName( "Car" ), "car" )
                } );

            RelationshipJoinOperator RJoinOp = new RelationshipJoinOperator(
                new JoinableEntity( ( Entity)ModelData.EntityRelationshipModel.FindByName( "Person" ), "person" ),
                new List<RelationshipJoinArgument> { RJoinArgs },
                ModelData.ERMongoMapping );

            List<AlgebraOperator> OpList = new List<AlgebraOperator> { RJoinOp };
            Pipeline pipeline = new Pipeline( OpList );
            QueryGenerator QueryGen = new QueryGenerator( pipeline )
            {
                CollectionName = "PersonDrivesCar"
            };

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
        public void OneToOneEmbeddedMixed()
        {
            // Asserts if the query result for a simple binary join is equal
            // to a handcrafted query
            RequiredDataContainer ModelData = OneToOneRelationshipsDataProvider.OneToOneEmbeddedMixed();

            // Load handcrafted query
            string HandcraftedQuery = Utils.ReadQueryFromFile( "HandcraftedQueries/personCarOneToOne_4.js" );

            // Assert if the handcrafted query is not null
            Assert.IsNotNull( HandcraftedQuery );

            // Prepare query generator
            RelationshipJoinArgument RJoinArgs = new RelationshipJoinArgument(
                (Relationship)ModelData.EntityRelationshipModel.FindByName( "Drives" ),
                new List<JoinableEntity> {
                    new JoinableEntity( (Entity)ModelData.EntityRelationshipModel.FindByName( "Car" ), "car" )
                } );

            RelationshipJoinOperator RJoinOp = new RelationshipJoinOperator(
                new JoinableEntity( ( Entity)ModelData.EntityRelationshipModel.FindByName( "Person" ), "person" ),
                new List<RelationshipJoinArgument> { RJoinArgs },
                ModelData.ERMongoMapping );

            List<AlgebraOperator> OpList = new List<AlgebraOperator> { RJoinOp };
            Pipeline pipeline = new Pipeline( OpList );
            QueryGenerator QueryGen = new QueryGenerator( pipeline )
            {
                CollectionName = "PersonDrivesCarMixed"
            };

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
            RequiredDataContainer ModelData = OneToOneRelationshipsDataProvider.OneToOneNotEmbeddedMultipleEntities();

            // Load handcrafted query
            string HandcraftedQuery = Utils.ReadQueryFromFile( "HandcraftedQueries/personCarInsuranceOneToOne.js" );

            // Assert if the handcrafted query is not null
            Assert.IsNotNull( HandcraftedQuery );

            // Prepare query generator
            RelationshipJoinArgument RJoinArgs = new RelationshipJoinArgument(
                (Relationship)ModelData.EntityRelationshipModel.FindByName( "HasInsurance" ),
                new List<JoinableEntity> {
                    new JoinableEntity( (Entity)ModelData.EntityRelationshipModel.FindByName( "Car" ), "car" ),
                    new JoinableEntity( (Entity)ModelData.EntityRelationshipModel.FindByName( "Insurance" ), "insurance" )
                } );

            RelationshipJoinOperator RJoinOp = new RelationshipJoinOperator(
                new JoinableEntity( ( Entity)ModelData.EntityRelationshipModel.FindByName( "Person" ), "person" ),
                new List<RelationshipJoinArgument> { RJoinArgs },
                ModelData.ERMongoMapping );

            List<AlgebraOperator> OpList = new List<AlgebraOperator> { RJoinOp };
            Pipeline pipeline = new Pipeline( OpList );
            QueryGenerator QueryGen = new QueryGenerator( pipeline )
            {
                CollectionName = "Person"
            };

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
            RequiredDataContainer ModelData = OneToOneRelationshipsDataProvider.OneToOneMultipleEntitiesMixed();

            // Load handcrafted query
            string HandcraftedQuery = Utils.ReadQueryFromFile( "HandcraftedQueries/personCarInsuranceOneToOneMultipleMixed.js" );

            // Assert if the handcrafted query is not null
            Assert.IsNotNull( HandcraftedQuery );

            // Prepare query generator
            RelationshipJoinArgument RJoinArgs = new RelationshipJoinArgument(
                (Relationship)ModelData.EntityRelationshipModel.FindByName( "HasInsurance" ),
                new List<JoinableEntity> {
                    new JoinableEntity( (Entity)ModelData.EntityRelationshipModel.FindByName( "Car" ), "car" ),
                    new JoinableEntity( (Entity)ModelData.EntityRelationshipModel.FindByName( "Insurance" ), "insurance" )
                } );

            RelationshipJoinOperator RJoinOp = new RelationshipJoinOperator(
                new JoinableEntity( ( Entity)ModelData.EntityRelationshipModel.FindByName( "Person" ), "person" ),
                new List<RelationshipJoinArgument> { RJoinArgs },
                ModelData.ERMongoMapping );

            List<AlgebraOperator> OpList = new List<AlgebraOperator> { RJoinOp };
            Pipeline pipeline = new Pipeline( OpList );
            QueryGenerator QueryGen = new QueryGenerator( pipeline )
            {
                CollectionName = "Person"
            };

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
            RequiredDataContainer ModelData = OneToOneRelationshipsDataProvider.OneToOneRelationshipAttributes();

            // Load handcrafted query
            string HandcraftedQuery = Utils.ReadQueryFromFile( "HandcraftedQueries/personCarOneToOneRelationshipAttributes.js" );

            // Assert if the handcrafted query is not null
            Assert.IsNotNull( HandcraftedQuery );

            // Prepare query generator
            RelationshipJoinArgument RJoinArgs = new RelationshipJoinArgument(
                (Relationship)ModelData.EntityRelationshipModel.FindByName( "HasInsurance" ),
                new List<JoinableEntity> {
                    new JoinableEntity( (Entity)ModelData.EntityRelationshipModel.FindByName( "Car" ), "car" ),
                    new JoinableEntity( (Entity)ModelData.EntityRelationshipModel.FindByName( "Insurance" ), "insurance" )
                } );

            RelationshipJoinOperator RJoinOp = new RelationshipJoinOperator(
                new JoinableEntity( ( Entity)ModelData.EntityRelationshipModel.FindByName( "Person" ), "person" ),
                new List<RelationshipJoinArgument> { RJoinArgs },
                ModelData.ERMongoMapping );

            List<AlgebraOperator> OpList = new List<AlgebraOperator> { RJoinOp };
            Pipeline pipeline = new Pipeline( OpList );
            QueryGenerator QueryGen = new QueryGenerator( pipeline )
            {
                CollectionName = "Person"
            };

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
        [TestMethod]
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
            RelationshipJoinArgument RJoinArgs = new RelationshipJoinArgument(
                (Relationship)ModelData.EntityRelationshipModel.FindByName( "HasInsurance" ),
                new List<JoinableEntity> {
                    new JoinableEntity( (Entity)ModelData.EntityRelationshipModel.FindByName( "Car" ), "car" ),
                    new JoinableEntity( (Entity)ModelData.EntityRelationshipModel.FindByName( "Insurance" ), "insurance" )
                } );

            RelationshipJoinOperator RJoinOp = new RelationshipJoinOperator(
                new JoinableEntity( ( Entity)ModelData.EntityRelationshipModel.FindByName( "Person" ), "person" ),
                new List<RelationshipJoinArgument> { RJoinArgs },
                ModelData.ERMongoMapping );

            List<AlgebraOperator> OpList = new List<AlgebraOperator> { RJoinOp };
            Pipeline pipeline = new Pipeline( OpList );
            QueryGenerator QueryGen = new QueryGenerator( pipeline )
            {
                CollectionName = "Person"
            };

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
        [TestMethod]
        public void OneToOneLeftSideEmbedded()
        {
            // Asserts if the query result for a simple binary join is equal
            // to a handcrafted query
            RequiredDataContainer ModelData = OneToOneRelationshipsDataProvider.OneToOneLeftSideEmbedded();

            // Load handcrafted query
            string HandcraftedQuery = Utils.ReadQueryFromFile( "HandcraftedQueries/oneToOneLeftSideEmbedded.js" );

            // Assert if the handcrafted query is not null
            Assert.IsNotNull( HandcraftedQuery );

            // Prepare query generator
            RelationshipJoinArgument RJoinArgs = new RelationshipJoinArgument(
                (Relationship)ModelData.EntityRelationshipModel.FindByName( "HasInsurance" ),
                new List<JoinableEntity> {
                    new JoinableEntity( (Entity)ModelData.EntityRelationshipModel.FindByName( "Insurance" ), "insurance" )
                } );

            RelationshipJoinOperator RJoinOp = new RelationshipJoinOperator(
                new JoinableEntity( ( Entity)ModelData.EntityRelationshipModel.FindByName( "Car" ), "car" ),
                new List<RelationshipJoinArgument> { RJoinArgs },
                ModelData.ERMongoMapping );

            List<AlgebraOperator> OpList = new List<AlgebraOperator> { RJoinOp };
            Pipeline pipeline = new Pipeline( OpList );
            QueryGenerator QueryGen = new QueryGenerator( pipeline )
            {
                CollectionName = "Person"
            };

            string GeneratedQuery = QueryGen.Run();

            // Assert if generated query is not null
            Assert.IsNotNull( GeneratedQuery );

            // Run Queries
            QueryRunner Runner = new QueryRunner( "mongodb://localhost:27017", "oneToOneLeftEmbedded" );

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
