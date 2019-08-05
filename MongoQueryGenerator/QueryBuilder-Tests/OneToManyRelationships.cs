using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using QueryBuilder.ER;
using QueryBuilder.Operation;
using QueryBuilder.Operation.Arguments;
using QueryBuilder.Operation.Exceptions;
using QueryBuilder.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueryBuilder.Tests
{
    [TestClass]
    public class OneToManyRelationships
    {
        [TestMethod]
        public void OneToManyNotembedded()
        {
            // Asserts if the query result for a simple binary join is equal
            // to a handcrafted query
            RequiredDataContainer ModelData = OneToManyRelationshipsDataProvider.OneToManyNotEmbedded();

            // Load handcrafted query
            string HandcraftedQuery = Utils.ReadQueryFromFile( "HandcraftedQueries/personCarOneToMany_1.js" );

            // Assert if the handcrafted query is not null
            Assert.IsNotNull( HandcraftedQuery );

            // Prepare query generator
            RelationshipJoinArguments RJoinArgs = new RelationshipJoinArguments(
                (Relationship)ModelData.EntityRelationshipModel.FindByName( "Drives" ),
                new List<JoinableEntity> {
                    new JoinableEntity( (Entity)ModelData.EntityRelationshipModel.FindByName( "Car" ), "car" )
                } );

            RelationshipJoinOperator RJoinOp = new RelationshipJoinOperator(
                new JoinableEntity( ( Entity)ModelData.EntityRelationshipModel.FindByName( "Person" ), "person" ),
                new List<RelationshipJoinArguments> { RJoinArgs },
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
            QueryRunner Runner = new QueryRunner( "mongodb://localhost:27017", "researchDatabaseOneToMany" );

            string HandcraftedResult = Runner.GetJSON( HandcraftedQuery );
            string GeneratedResult = Runner.GetJSON( GeneratedQuery );

            // Check if either result is null
            Assert.IsNotNull( HandcraftedResult );
            Assert.IsNotNull( GeneratedResult );

            // Check if both results are equal
            Assert.IsTrue( JToken.DeepEquals( JToken.Parse( HandcraftedResult ), JToken.Parse( GeneratedResult ) ) );
        }
        [TestMethod]
        public void OneToManyEmbedded()
        {
            // Asserts if the query result for a simple binary join is equal
            // to a handcrafted query
            RequiredDataContainer ModelData = OneToManyRelationshipsDataProvider.OneToManyEmbedded();

            // Load handcrafted query
            string HandcraftedQuery = Utils.ReadQueryFromFile( "HandcraftedQueries/personCarOneToMany_2.js" );

            // Assert if the handcrafted query is not null
            Assert.IsNotNull( HandcraftedQuery );

            // Prepare query generator
            RelationshipJoinArguments RJoinArgs = new RelationshipJoinArguments(
                (Relationship)ModelData.EntityRelationshipModel.FindByName( "Drives" ),
                new List<JoinableEntity> {
                    new JoinableEntity( (Entity)ModelData.EntityRelationshipModel.FindByName( "Car" ), "car" )
                } );

            RelationshipJoinOperator RJoinOp = new RelationshipJoinOperator(
                new JoinableEntity( ( Entity)ModelData.EntityRelationshipModel.FindByName( "Person" ), "person" ),
                new List<RelationshipJoinArguments> { RJoinArgs },
                ModelData.ERMongoMapping );

            List<AlgebraOperator> OpList = new List<AlgebraOperator> { RJoinOp };
            Pipeline pipeline = new Pipeline( OpList );
            QueryGenerator QueryGen = new QueryGenerator( pipeline )
            {
                CollectionName = "PersonDrivesCars"
            };

            string GeneratedQuery = QueryGen.Run();

            // Assert if generated query is not null
            Assert.IsNotNull( GeneratedQuery );

            // Run Queries
            QueryRunner Runner = new QueryRunner( "mongodb://localhost:27017", "researchDatabaseOneToMany" );

            string HandcraftedResult = Runner.GetJSON( HandcraftedQuery );
            string GeneratedResult = Runner.GetJSON( GeneratedQuery );

            // Check if either result is null
            Assert.IsNotNull( HandcraftedResult );
            Assert.IsNotNull( GeneratedResult );

            // Check if both results are equal
            Assert.IsTrue( JToken.DeepEquals( JToken.Parse( HandcraftedResult ), JToken.Parse( GeneratedResult ) ) );
        }
        [TestMethod]
        public void OneToManyRelationshipAttributes()
        {
            // Asserts if the query result for a simple binary join is equal
            // to a handcrafted query
            RequiredDataContainer ModelData = OneToManyRelationshipsDataProvider.OneToManyRelationshipAttributes();

            // Load handcrafted query
            string HandcraftedQuery = Utils.ReadQueryFromFile( "HandcraftedQueries/personDrivesOneToManyRelationshipAttributes.js" );

            // Assert if the handcrafted query is not null
            Assert.IsNotNull( HandcraftedQuery );

            // Prepare query generator
            RelationshipJoinArguments RJoinArgs = new RelationshipJoinArguments(
                (Relationship)ModelData.EntityRelationshipModel.FindByName( "Drives" ),
                new List<JoinableEntity> {
                    new JoinableEntity( (Entity)ModelData.EntityRelationshipModel.FindByName( "Car" ), "car" )
                } );

            RelationshipJoinOperator RJoinOp = new RelationshipJoinOperator(
                new JoinableEntity( ( Entity)ModelData.EntityRelationshipModel.FindByName( "Person" ), "person" ),
                new List<RelationshipJoinArguments> { RJoinArgs },
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
            QueryRunner Runner = new QueryRunner( "mongodb://localhost:27017", "researchOneToManyRelationshipAttributes" );

            string HandcraftedResult = Runner.GetJSON( HandcraftedQuery );
            string GeneratedResult = Runner.GetJSON( GeneratedQuery );

            // Check if either result is null
            Assert.IsNotNull( HandcraftedResult );
            Assert.IsNotNull( GeneratedResult );

            // Check if both results are equal
            Assert.IsTrue( JToken.DeepEquals( JToken.Parse( HandcraftedResult ), JToken.Parse( GeneratedResult ) ) );
        }
        [TestMethod]
        public void OneToManyRelationshipAttributesEmbedded()
        {
            // Asserts if the query result for a simple binary join is equal
            // to a handcrafted query
            RequiredDataContainer ModelData = OneToManyRelationshipsDataProvider.OneToManyRelationshipAttributesEmbedded();

            // Load handcrafted query
            string HandcraftedQuery = Utils.ReadQueryFromFile( "HandcraftedQueries/personCarOneToManyRelationshipAttributeEmbedded.js" );

            // Assert if the handcrafted query is not null
            Assert.IsNotNull( HandcraftedQuery );

            // Prepare query generator
            RelationshipJoinArguments RJoinArgs = new RelationshipJoinArguments(
                (Relationship)ModelData.EntityRelationshipModel.FindByName( "Drives" ),
                new List<JoinableEntity> {
                    new JoinableEntity( (Entity)ModelData.EntityRelationshipModel.FindByName( "Car" ), "car" )
                } );

            RelationshipJoinOperator RJoinOp = new RelationshipJoinOperator(
                new JoinableEntity( ( Entity)ModelData.EntityRelationshipModel.FindByName( "Person" ), "person" ),
                new List<RelationshipJoinArguments> { RJoinArgs },
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
            QueryRunner Runner = new QueryRunner( "mongodb://localhost:27017", "researchOneToManyRelationshipAttributesEmbedded" );

            string HandcraftedResult = Runner.GetJSON( HandcraftedQuery );
            string GeneratedResult = Runner.GetJSON( GeneratedQuery );

            // Check if either result is null
            Assert.IsNotNull( HandcraftedResult );
            Assert.IsNotNull( GeneratedResult );

            // Check if both results are equal
            Assert.IsTrue( JToken.DeepEquals( JToken.Parse( HandcraftedResult ), JToken.Parse( GeneratedResult ) ) );
        }
        // Removed test
        public void OneToManyLeftSideEmbedded()
        {
            // Asserts if the query result for a simple binary join is equal
            // to a handcrafted query
            RequiredDataContainer ModelData = OneToManyRelationshipsDataProvider.OneToManyLeftSideEmbedded();

            // Load handcrafted query
            string HandcraftedQuery = Utils.ReadQueryFromFile( "HandcraftedQueries/oneToManyLeftSideEmbedded.js" );

            // Assert if the handcrafted query is not null
            Assert.IsNotNull( HandcraftedQuery );

            // Prepare query generator
            RelationshipJoinArguments RJoinArgs = new RelationshipJoinArguments(
                (Relationship)ModelData.EntityRelationshipModel.FindByName( "HasInsurance" ),
                new List<JoinableEntity> {
                    new JoinableEntity( (Entity)ModelData.EntityRelationshipModel.FindByName( "Insurance" ), "insurance" )
                } );

            RelationshipJoinOperator RJoinOp = new RelationshipJoinOperator(
                new JoinableEntity( ( Entity)ModelData.EntityRelationshipModel.FindByName( "Car" ), "car" ),
                new List<RelationshipJoinArguments> { RJoinArgs },
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
            QueryRunner Runner = new QueryRunner( "mongodb://localhost:27017", "oneToManyLeftEmbedded" );

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
