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
    public class ComputedEntityTests
    {
        [TestMethod]
        public void OneToOneComputedEntity()
        {
            // Asserts if the query result for a relationship join operation is equal
            // to a handcrafted query
            var ModelData = QueryBuilderParser.ParseMapping( Utils.ReadMappingFile( "Mappings/one-to-one-computed-entity.mapping" ) );

            // Load handcrafted query
            string HandcraftedQuery = Utils.ReadQueryFromFile( "HandcraftedQueries/ceOneToOne.js" );

            // Assert if the handcrafted query is not null
            Assert.IsNotNull( HandcraftedQuery );

            // Prepare query generator
            string QueryString = "from Person RJOIN <Drives> (Car RJOIN <Repaired> (Garage)) select *";
            QueryGenerator QueryGen = QueryBuilderParser.ParseQuery( QueryString, ModelData );

            string GeneratedQuery = QueryGen.Run();

            // Assert if generated query is not null
            Assert.IsNotNull( GeneratedQuery );

            // Run Queries
            QueryRunner Runner = new QueryRunner( "mongodb://localhost:27017", "ceOneToOne" );

            string HandcraftedResult = Runner.GetJSON( HandcraftedQuery );
            string GeneratedResult = Runner.GetJSON( GeneratedQuery );

            // Check if either result is null
            Assert.IsNotNull( HandcraftedResult );
            Assert.IsNotNull( GeneratedResult );

            // Check if both results are equal
            Assert.IsTrue( JToken.DeepEquals( JToken.Parse( HandcraftedResult ), JToken.Parse( GeneratedResult ) ) );
        }
        [TestMethod]
        public void OneToOneComputedEntityMultiple()
        {
            // Asserts if the query result for a relationship join operation is equal
            // to a handcrafted query
            var ModelData = QueryBuilderParser.ParseMapping( Utils.ReadMappingFile( "Mappings/one-to-one-computed-entity-multiple.mapping" ) );

            // Load handcrafted query
            string HandcraftedQuery = Utils.ReadQueryFromFile( "HandcraftedQueries/ceOneToOne-Multiple.js" );

            // Assert if the handcrafted query is not null
            Assert.IsNotNull( HandcraftedQuery );

            // Prepare query generator
            string QueryString = "from Person RJOIN <Drives> (Car RJOIN <Repaired> (Garage, Supplier)) select *";
            QueryGenerator QueryGen = QueryBuilderParser.ParseQuery( QueryString, ModelData );

            string GeneratedQuery = QueryGen.Run();

            // Assert if generated query is not null
            Assert.IsNotNull( GeneratedQuery );

            // Run Queries
            QueryRunner Runner = new QueryRunner( "mongodb://localhost:27017", "ceOneToOneMultiple" );

            string HandcraftedResult = Runner.GetJSON( HandcraftedQuery );
            string GeneratedResult = Runner.GetJSON( GeneratedQuery );

            // Check if either result is null
            Assert.IsNotNull( HandcraftedResult );
            Assert.IsNotNull( GeneratedResult );

            // Check if both results are equal
            Assert.IsTrue( JToken.DeepEquals( JToken.Parse( HandcraftedResult ), JToken.Parse( GeneratedResult ) ) );
        }
        [TestMethod]
        public void OneToOneComputedEntityMultiple2()
        {
            // Asserts if the query result for a relationship join operation is equal
            // to a handcrafted query
            var ModelData = QueryBuilderParser.ParseMapping( Utils.ReadMappingFile( "Mappings/one-to-one-computed-entity-multiple-2.mapping" ) );

            // Load handcrafted query
            string HandcraftedQuery = Utils.ReadQueryFromFile( "HandcraftedQueries/ceOneToOne-Multiple2.js" );

            // Assert if the handcrafted query is not null
            Assert.IsNotNull( HandcraftedQuery );

            // Prepare query generator
            string QueryString = "from Person RJOIN <Drives> (Car RJOIN <Repaired> (Garage, Supplier)) RJOIN <HasInsurance> (Insurance) select *";
            QueryGenerator QueryGen = QueryBuilderParser.ParseQuery( QueryString, ModelData );

            string GeneratedQuery = QueryGen.Run();

            // Assert if generated query is not null
            Assert.IsNotNull( GeneratedQuery );

            // Run Queries
            QueryRunner Runner = new QueryRunner( "mongodb://localhost:27017", "ceOneToOneMultiple2" );

            string HandcraftedResult = Runner.GetJSON( HandcraftedQuery );
            string GeneratedResult = Runner.GetJSON( GeneratedQuery );

            // Check if either result is null
            Assert.IsNotNull( HandcraftedResult );
            Assert.IsNotNull( GeneratedResult );

            // Check if both results are equal
            Assert.IsTrue( JToken.DeepEquals( JToken.Parse( HandcraftedResult ), JToken.Parse( GeneratedResult ) ) );
        }
        [TestMethod]
        public void OneToManyComputedEntity()
        {
            // Asserts if the query result for a relationship join operation is equal
            // to a handcrafted query
            var ModelData = QueryBuilderParser.ParseMapping( Utils.ReadMappingFile( "Mappings/one-to-many-computed-entity.mapping" ) );

            // Load handcrafted query
            string HandcraftedQuery = Utils.ReadQueryFromFile( "HandcraftedQueries/ceOneToMany.js" );

            // Assert if the handcrafted query is not null
            Assert.IsNotNull( HandcraftedQuery );

            // Prepare query generator
            string QueryString = "from Person RJOIN <Drives> (Car <Repaired> (Garage)) select *";
            QueryGenerator QueryGen = QueryBuilderParser.ParseQuery( QueryString, ModelData );

            string GeneratedQuery = QueryGen.Run();

            // Assert if generated query is not null
            Assert.IsNotNull( GeneratedQuery );

            // Run Queries
            QueryRunner Runner = new QueryRunner( "mongodb://localhost:27017", "ceOneToMany" );

            string HandcraftedResult = Runner.GetJSON( HandcraftedQuery );
            string GeneratedResult = Runner.GetJSON( GeneratedQuery );

            // Check if either result is null
            Assert.IsNotNull( HandcraftedResult );
            Assert.IsNotNull( GeneratedResult );

            // Check if both results are equal
            Assert.IsTrue( JToken.DeepEquals( JToken.Parse( HandcraftedResult ), JToken.Parse( GeneratedResult ) ) );
        }
        [TestMethod]
        public void OneToManyComputedEntityMultipleEntities()
        {
            // Asserts if the query result for a relationship join operation is equal
            // to a handcrafted query
            var ModelData = QueryBuilderParser.ParseMapping( Utils.ReadMappingFile( "Mappings/one-to-many-computed-entity.mapping" ) );

            // Load handcrafted query
            string HandcraftedQuery = Utils.ReadQueryFromFile( "HandcraftedQueries/ceOneToMany-2.js" );

            // Assert if the handcrafted query is not null
            Assert.IsNotNull( HandcraftedQuery );

            // Prepare query generator
            string QueryString = "from Person RJOIN <Drives> (Car <Repaired> (Garage, Supplier)) RJOIN <HasInsurance> (Insurance) select *";
            QueryGenerator QueryGen = QueryBuilderParser.ParseQuery( QueryString, ModelData );

            string GeneratedQuery = QueryGen.Run();

            // Assert if generated query is not null
            Assert.IsNotNull( GeneratedQuery );

            // Run Queries
            QueryRunner Runner = new QueryRunner( "mongodb://localhost:27017", "ceOneToMany" );

            string HandcraftedResult = Runner.GetJSON( HandcraftedQuery );
            string GeneratedResult = Runner.GetJSON( GeneratedQuery );

            // Check if either result is null
            Assert.IsNotNull( HandcraftedResult );
            Assert.IsNotNull( GeneratedResult );

            // Check if both results are equal
            Assert.IsTrue( JToken.DeepEquals( JToken.Parse( HandcraftedResult ), JToken.Parse( GeneratedResult ) ) );
        }
        [TestMethod]
        public void ManyToManyComputedEntityMultipleEntities()
        {
            // Asserts if the query result for a relationship join operation is equal
            // to a handcrafted query
            var ModelData = QueryBuilderParser.ParseMapping( Utils.ReadMappingFile( "Mappings/many-to-many-computed-entity.mapping" ) );

            // Load handcrafted query
            string HandcraftedQuery = Utils.ReadQueryFromFile( "HandcraftedQueries/ceManyToMany.js" );

            // Assert if the handcrafted query is not null
            Assert.IsNotNull( HandcraftedQuery );

            // Prepare query generator
            string QueryString = "from Person RJOIN <Owns> (Car <Repaired> (Garage, Supplier), Insurance) select *";
            QueryGenerator QueryGen = QueryBuilderParser.ParseQuery( QueryString, ModelData );

            string GeneratedQuery = QueryGen.Run();

            // Assert if generated query is not null
            Assert.IsNotNull( GeneratedQuery );

            // Run Queries
            QueryRunner Runner = new QueryRunner( "mongodb://localhost:27017", "ceManyToMany" );

            string HandcraftedResult = Runner.GetJSON( HandcraftedQuery );
            string GeneratedResult = Runner.GetJSON( GeneratedQuery );

            // Check if either result is null
            Assert.IsNotNull( HandcraftedResult );
            Assert.IsNotNull( GeneratedResult );

            // Check if both results are equal
            Assert.IsTrue( JToken.DeepEquals( JToken.Parse( HandcraftedResult ), JToken.Parse( GeneratedResult ) ) );
        }
        [TestMethod]
        public void ManyToManyComputedEntityOneToOne()
        {
            // Asserts if the query result for a relationship join operation is equal
            // to a handcrafted query
            var ModelData = QueryBuilderParser.ParseMapping( Utils.ReadMappingFile( "Mappings/many-to-many-computed-entity-2.mapping" ) );

            // Load handcrafted query
            string HandcraftedQuery = Utils.ReadQueryFromFile( "HandcraftedQueries/ceManyToMany-2.js" );

            // Assert if the handcrafted query is not null
            Assert.IsNotNull( HandcraftedQuery );

            // Prepare query generator
            string QueryString = "from Person RJOIN <Owns> (Car <ManufacturedBy> (Manufacturer)) select *";
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
