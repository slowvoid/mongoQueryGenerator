using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using QueryBuilder.ER;
using QueryBuilder.Operation;
using QueryBuilder.Operation.Arguments;
using QueryBuilder.Parser;
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
            var ModelData = QueryBuilderParser.ParseMapping( Utils.ReadMappingFile( "Mappings/cartesian.mapping" ) );

            // Load handcrafted query
            string HandcraftedQuery = Utils.ReadQueryFromFile( "HandcraftedQueries/cartersianProduct-1.js" );

            // Assert if the handcrafted query is not null
            Assert.IsNotNull( HandcraftedQuery );

            string QueryString = "from Person CARTESIANPRODUCT Car CARTESIANPRODUCT Supplier select *";
            QueryGenerator QueryGen = QueryBuilderParser.ParseQuery( QueryString, ModelData );

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