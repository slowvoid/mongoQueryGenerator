using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using QueryBuilder.ER;
using QueryBuilder.Operation;
using QueryBuilder.Query;
using System.Collections.Generic;

namespace QueryBuilder.Tests
{
    [TestClass]
    public class CartersiaProductTests
    {
        [TestMethod]
        public void CartesianProduct()
        {
            // Asserts if the query result for a relationship join operation is equal
            // to a handcrafted query
            RequiredDataContainer ModelData = CartesianProductDataProvider.SampleData();

            // Load handcrafted query
            string HandcraftedQuery = Utils.ReadQueryFromFile( "HandcraftedQueries/cartersianProduct-1.js" );

            // Assert if the handcrafted query is not null
            Assert.IsNotNull( HandcraftedQuery );

            // Prepare query generator
            CartesianProductOperator CartesianOp = new CartesianProductOperator( 
                (Entity)ModelData.EntityRelationshipModel.FindByName( "Person" ),
                (Entity)ModelData.EntityRelationshipModel.FindByName( "Car" ), ModelData.ERMongoMapping );

            CartesianProductOperator CartesianOp2 = new CartesianProductOperator(
                (Entity)ModelData.EntityRelationshipModel.FindByName( "Person" ),
                (Entity)ModelData.EntityRelationshipModel.FindByName( "Supplier" ), ModelData.ERMongoMapping );

            List<AlgebraOperator> OpList = new List<AlgebraOperator> { CartesianOp, CartesianOp2 };
            Pipeline pipeline = new Pipeline( OpList );
            QueryGenerator QueryGen = new QueryGenerator( pipeline )
            {
                CollectionName = "Person"
            };

            string GeneratedQuery = QueryGen.Run();

            // Assert if generated query is not null
            Assert.IsNotNull( GeneratedQuery );

            // Run Queries
            QueryRunner Runner = new QueryRunner( "mongodb://localhost:27017", "ceManyToMany2" );

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