using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using QueryBuilder.ER;
using QueryBuilder.Operation;
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
        public void OneToOneNotEmbbebed()
        {
            // Asserts if the query result for a simple binary join is equal
            // to a handcrafted query
            RequiredDataContainer ModelData = OneToOneRelationshipsDataProvider.OneToOneNotEmbbebed();

            // Load handcrafted query
            string HandcraftedQuery = Utils.ReadQueryFromFile( "HandcraftedQueries/personCarOneToOne_1.js" );

            // Assert if the handcrafted query is not null
            Assert.IsNotNull( HandcraftedQuery );

            // Prepare query generator
            RelationshipJoinOperator RJoinOp = new RelationshipJoinOperator( (Entity)ModelData.EntityRelationshipModel.FindByName( "Person" ),
                (Relationship)ModelData.EntityRelationshipModel.FindByName( "Drives" ), new List<Entity> { (Entity)ModelData.EntityRelationshipModel.FindByName( "Car" ) },
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
        public void OneToOneEmbbebed()
        {
            // Asserts if the query result for a simple binary join is equal
            // to a handcrafted query
            RequiredDataContainer ModelData = OneToOneRelationshipsDataProvider.OneToOneEmbbebed();

            // Load handcrafted query
            string HandcraftedQuery = Utils.ReadQueryFromFile( "HandcraftedQueries/personCarOneToOne_2.js" );

            // Assert if the handcrafted query is not null
            Assert.IsNotNull( HandcraftedQuery );

            // Prepare query generator
            RelationshipJoinOperator RJoinOp = new RelationshipJoinOperator( (Entity)ModelData.EntityRelationshipModel.FindByName( "Person" ),
                (Relationship)ModelData.EntityRelationshipModel.FindByName( "Drives" ), new List<Entity> { (Entity)ModelData.EntityRelationshipModel.FindByName( "Car" ) },
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
    }
}
