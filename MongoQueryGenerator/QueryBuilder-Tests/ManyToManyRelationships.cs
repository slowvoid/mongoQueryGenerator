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
    public class ManyToManyRelationships
    {
        [TestMethod]
        public void ManyToManySingleEntity()
        {
            // Asserts if the query result for a simple binary join is equal
            // to a handcrafted query
            RequiredDataContainer ModelData = ManyToManyRelationshipsDataProvider.ManyToManySingleEntity();

            // Load handcrafted query
            string HandcraftedQuery = Utils.ReadQueryFromFile( "HandcraftedQueries/personCarManyToMany_1.js" );

            // Assert if the handcrafted query is not null
            Assert.IsNotNull( HandcraftedQuery );

            // Prepare query generator
            RelationshipJoinArgument RJoinArgs = new RelationshipJoinArgument(
                (Relationship)ModelData.EntityRelationshipModel.FindByName( "Insurance" ),
                new List<QueryableEntity> {
                    new QueryableEntity( (Entity)ModelData.EntityRelationshipModel.FindByName( "Car" ), "car" )
                } );

            RelationshipJoinOperator RJoinOp = new RelationshipJoinOperator(
                new QueryableEntity( ( Entity)ModelData.EntityRelationshipModel.FindByName( "Person" ), "person" ),
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
            QueryRunner Runner = new QueryRunner( "mongodb://localhost:27017", "researchManyToMany" );

            string HandcraftedResult = Runner.GetJSON( HandcraftedQuery );
            string GeneratedResult = Runner.GetJSON( GeneratedQuery );

            // Check if either result is null
            Assert.IsNotNull( HandcraftedResult );
            Assert.IsNotNull( GeneratedResult );

            // Check if both results are equal
            Assert.IsTrue( JToken.DeepEquals( JToken.Parse( HandcraftedResult ), JToken.Parse( GeneratedResult ) ) );
        }
        [TestMethod]
        public void ManyToManyMultipleEntities()
        {
            // Asserts if the query result for a simple binary join is equal
            // to a handcrafted query
            RequiredDataContainer ModelData = ManyToManyRelationshipsDataProvider.ManyToManyMultipleEntities();

            // Load handcrafted query
            string HandcraftedQuery = Utils.ReadQueryFromFile( "HandcraftedQueries/personCarInsCompanyManyToMany.js" );

            // Assert if the handcrafted query is not null
            Assert.IsNotNull( HandcraftedQuery );

            // Prepare query generator

            // FROM Person RJOIN (Car, InsCompany)

            RelationshipJoinArgument RJoinArgs = new RelationshipJoinArgument(
                (Relationship)ModelData.EntityRelationshipModel.FindByName( "Insurance" ),
                new List<QueryableEntity> {
                    new QueryableEntity( (Entity)ModelData.EntityRelationshipModel.FindByName( "Car" ), "car" ),
                    new QueryableEntity( (Entity)ModelData.EntityRelationshipModel.FindByName("InsCompany"), "inscompany" )
                } );

            RelationshipJoinOperator RJoinOp = new RelationshipJoinOperator(
                new QueryableEntity( ( Entity)ModelData.EntityRelationshipModel.FindByName( "Person" ), "person" ),
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
            QueryRunner Runner = new QueryRunner( "mongodb://localhost:27017", "researchManyToMany" );

            string HandcraftedResult = Runner.GetJSON( HandcraftedQuery );
            string GeneratedResult = Runner.GetJSON( GeneratedQuery );

            // Check if either result is null
            Assert.IsNotNull( HandcraftedResult );
            Assert.IsNotNull( GeneratedResult );

            // Check if both results are equal
            Assert.IsTrue( JToken.DeepEquals( JToken.Parse( HandcraftedResult ), JToken.Parse( GeneratedResult ) ) );
        }
        [TestMethod]
        public void ManyToManyRelationshipAttributeSingleEntity()
        {
            // Asserts if the query result for a simple binary join is equal
            // to a handcrafted query
            RequiredDataContainer ModelData = ManyToManyRelationshipsDataProvider.ManyToManyRelationshipAttributeSingleEntity();

            // Load handcrafted query
            string HandcraftedQuery = Utils.ReadQueryFromFile( "HandcraftedQueries/manyToManyRelationshipAttributes.js" );

            // Assert if the handcrafted query is not null
            Assert.IsNotNull( HandcraftedQuery );

            // Prepare query generator
            RelationshipJoinArgument RJoinArgs = new RelationshipJoinArgument(
                (Relationship)ModelData.EntityRelationshipModel.FindByName( "Insurance" ),
                new List<QueryableEntity> {
                    new QueryableEntity( (Entity)ModelData.EntityRelationshipModel.FindByName( "Car" ), "car" )
                } );

            RelationshipJoinOperator RJoinOp = new RelationshipJoinOperator(
                new QueryableEntity( ( Entity)ModelData.EntityRelationshipModel.FindByName( "Person" ), "person" ),
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
            QueryRunner Runner = new QueryRunner( "mongodb://localhost:27017", "researchManyToManyRelationshipAttributes" );

            string HandcraftedResult = Runner.GetJSON( HandcraftedQuery );
            string GeneratedResult = Runner.GetJSON( GeneratedQuery );

            // Check if either result is null
            Assert.IsNotNull( HandcraftedResult );
            Assert.IsNotNull( GeneratedResult );

            // Check if both results are equal
            Assert.IsTrue( JToken.DeepEquals( JToken.Parse( HandcraftedResult ), JToken.Parse( GeneratedResult ) ) );
        }
        [TestMethod]
        public void ManyToManyRelationshipAttributesMultipleEntities()
        {
            // Asserts if the query result for a simple binary join is equal
            // to a handcrafted query
            RequiredDataContainer ModelData = ManyToManyRelationshipsDataProvider.ManyToManyRelationshipAttributeMultipleEntities();

            // Load handcrafted query
            string HandcraftedQuery = Utils.ReadQueryFromFile( "HandcraftedQueries/manyToManyRelationshipAttributesMultipleEntities.js" );

            // Assert if the handcrafted query is not null
            Assert.IsNotNull( HandcraftedQuery );

            // Prepare query generator
            RelationshipJoinArgument RJoinArgs = new RelationshipJoinArgument(
                (Relationship)ModelData.EntityRelationshipModel.FindByName( "Insurance" ),
                new List<QueryableEntity> {
                    new QueryableEntity( (Entity)ModelData.EntityRelationshipModel.FindByName( "Car" ), "car" ),
                    new QueryableEntity( (Entity)ModelData.EntityRelationshipModel.FindByName("InsCompany"), "inscompany" )
                } );

            RelationshipJoinOperator RJoinOp = new RelationshipJoinOperator(
                new QueryableEntity( ( Entity)ModelData.EntityRelationshipModel.FindByName( "Person" ), "person" ),
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
            QueryRunner Runner = new QueryRunner( "mongodb://localhost:27017", "researchManyToManyRelationshipAttributes" );

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
