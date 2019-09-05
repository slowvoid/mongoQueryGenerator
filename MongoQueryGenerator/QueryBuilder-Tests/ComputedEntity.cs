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
    public class ComputedEntityTests
    {
        [TestMethod]
        public void OneToOneComputedEntity()
        {
            // Asserts if the query result for a relationship join operation is equal
            // to a handcrafted query
            RequiredDataContainer ModelData = ComputedEntityDataProvider.OneToOneComputedEntity();

            // Load handcrafted query
            string HandcraftedQuery = Utils.ReadQueryFromFile( "HandcraftedQueries/ceOneToOne.js" );

            // Assert if the handcrafted query is not null
            Assert.IsNotNull( HandcraftedQuery );

            // Prepare query generator
            ComputedEntity CarRepairedByGarage = new ComputedEntity( "CarRepairedByGarage",
                new QueryableEntity( ( Entity)ModelData.EntityRelationshipModel.FindByName( "Car" ), "car" ),
                (Relationship)ModelData.EntityRelationshipModel.FindByName( "Repaired" ),
                new List<QueryableEntity> {
                    new QueryableEntity( (Entity)ModelData.EntityRelationshipModel.FindByName( "Garage" ), "garage" ) } );

            RelationshipJoinArgument RJoinArgs = new RelationshipJoinArgument(
                (Relationship)ModelData.EntityRelationshipModel.FindByName( "Drives" ),
                new List<QueryableEntity> { new QueryableEntity( CarRepairedByGarage ) } );

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
            RequiredDataContainer ModelData = ComputedEntityDataProvider.OneToOneComputedEntityMultiple();

            // Load handcrafted query
            string HandcraftedQuery = Utils.ReadQueryFromFile( "HandcraftedQueries/ceOneToOne-Multiple.js" );

            // Assert if the handcrafted query is not null
            Assert.IsNotNull( HandcraftedQuery );

            // Prepare query generator
            ComputedEntity CarRepairedByGarage = new ComputedEntity( "CarRepairedByGarage",
                new QueryableEntity( ( Entity)ModelData.EntityRelationshipModel.FindByName( "Car" ), "car" ),
                (Relationship)ModelData.EntityRelationshipModel.FindByName( "Repaired" ),
                new List<QueryableEntity> {
                    new QueryableEntity( (Entity)ModelData.EntityRelationshipModel.FindByName( "Garage" ), "garage" ),
                    new QueryableEntity( (Entity)ModelData.EntityRelationshipModel.FindByName("Supplier"), "supplier" )
                } );

            RelationshipJoinArgument RJoinArgs = new RelationshipJoinArgument(
                (Relationship)ModelData.EntityRelationshipModel.FindByName( "Drives" ),
                new List<QueryableEntity> { new QueryableEntity( CarRepairedByGarage ) } );

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
            RequiredDataContainer ModelData = ComputedEntityDataProvider.OneToOneComputedEntityMultiple2();

            // Load handcrafted query
            string HandcraftedQuery = Utils.ReadQueryFromFile( "HandcraftedQueries/ceOneToOne-Multiple2.js" );

            // Assert if the handcrafted query is not null
            Assert.IsNotNull( HandcraftedQuery );

            // Prepare query generator
            ComputedEntity CarRepairedByGarage = new ComputedEntity( "CarRepairedByGarage",
                new QueryableEntity( ( Entity)ModelData.EntityRelationshipModel.FindByName( "Car" ), "car" ),
                (Relationship)ModelData.EntityRelationshipModel.FindByName( "Repaired" ),
                new List<QueryableEntity> {
                    new QueryableEntity( (Entity)ModelData.EntityRelationshipModel.FindByName( "Garage" ), "garage" ),
                    new QueryableEntity( (Entity)ModelData.EntityRelationshipModel.FindByName("Supplier"), "supplier" )
                } );

            RelationshipJoinArgument RJoinArgs = new RelationshipJoinArgument(
                (Relationship)ModelData.EntityRelationshipModel.FindByName( "Drives" ),
                new List<QueryableEntity> { new QueryableEntity( CarRepairedByGarage ) } );

            RelationshipJoinArgument RJoinArgs2 = new RelationshipJoinArgument(
                (Relationship)ModelData.EntityRelationshipModel.FindByName( "HasInsurance" ),
                new List<QueryableEntity> { new QueryableEntity( (Entity)ModelData.EntityRelationshipModel.FindByName( "Insurance" ), "insurance") } );

            RelationshipJoinOperator RJoinOp = new RelationshipJoinOperator(
                new QueryableEntity( ( Entity)ModelData.EntityRelationshipModel.FindByName( "Person" ), "person" ),
                new List<RelationshipJoinArgument> { RJoinArgs, RJoinArgs2 },
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
            RequiredDataContainer ModelData = ComputedEntityDataProvider.OneToManyComputedEntity();

            // Load handcrafted query
            string HandcraftedQuery = Utils.ReadQueryFromFile( "HandcraftedQueries/ceOneToMany.js" );

            // Assert if the handcrafted query is not null
            Assert.IsNotNull( HandcraftedQuery );

            // Prepare query generator
            ComputedEntity CarRepairedByGarage = new ComputedEntity( "CarRepairedByGarage",
                new QueryableEntity( ( Entity)ModelData.EntityRelationshipModel.FindByName( "Car" ), "car" ),
                (Relationship)ModelData.EntityRelationshipModel.FindByName( "Repaired" ),
                new List<QueryableEntity> {
                    new QueryableEntity( (Entity)ModelData.EntityRelationshipModel.FindByName( "Garage" ), "garage" )
                } );

            RelationshipJoinArgument RJoinArgs = new RelationshipJoinArgument(
                (Relationship)ModelData.EntityRelationshipModel.FindByName( "Drives" ),
                new List<QueryableEntity> { new QueryableEntity( CarRepairedByGarage ) } );

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
            RequiredDataContainer ModelData = ComputedEntityDataProvider.OneToManyComputedEntity();

            // Load handcrafted query
            string HandcraftedQuery = Utils.ReadQueryFromFile( "HandcraftedQueries/ceOneToMany-2.js" );

            // Assert if the handcrafted query is not null
            Assert.IsNotNull( HandcraftedQuery );

            // Prepare query generator
            ComputedEntity CarRepairedByGarage = new ComputedEntity( "CarRepairedByGarage",
                new QueryableEntity( ( Entity)ModelData.EntityRelationshipModel.FindByName( "Car" ), "car" ),
                (Relationship)ModelData.EntityRelationshipModel.FindByName( "Repaired" ),
                new List<QueryableEntity> {
                    new QueryableEntity( (Entity)ModelData.EntityRelationshipModel.FindByName( "Garage" ), "garage" ),
                    new QueryableEntity( (Entity)ModelData.EntityRelationshipModel.FindByName( "Supplier" ), "supplier" )
                } );

            RelationshipJoinArgument RJoinArgs = new RelationshipJoinArgument(
                (Relationship)ModelData.EntityRelationshipModel.FindByName( "Drives" ),
                new List<QueryableEntity> {
                    new QueryableEntity( CarRepairedByGarage )
                } );

            RelationshipJoinArgument RJoinArgs2 = new RelationshipJoinArgument(
                (Relationship)ModelData.EntityRelationshipModel.FindByName( "HasInsurance" ),
                new List<QueryableEntity> {
                    new QueryableEntity( (Entity)ModelData.EntityRelationshipModel.FindByName( "Insurance" ), "insurance" )
                } );

            RelationshipJoinOperator RJoinOp = new RelationshipJoinOperator(
                new QueryableEntity( ( Entity)ModelData.EntityRelationshipModel.FindByName( "Person" ), "person" ),
                new List<RelationshipJoinArgument> { RJoinArgs, RJoinArgs2 },
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
            RequiredDataContainer ModelData = ComputedEntityDataProvider.ManyToManyComputedEntity();

            // Load handcrafted query
            string HandcraftedQuery = Utils.ReadQueryFromFile( "HandcraftedQueries/ceManyToMany.js" );

            // Assert if the handcrafted query is not null
            Assert.IsNotNull( HandcraftedQuery );

            // Prepare query generator
            ComputedEntity CarRepairedByGarage = new ComputedEntity( "CarRepairedByGarage",
                new QueryableEntity( ( Entity)ModelData.EntityRelationshipModel.FindByName( "Car" ), "car" ),
                (Relationship)ModelData.EntityRelationshipModel.FindByName( "Repaired" ),
                new List<QueryableEntity> {
                    new QueryableEntity( (Entity)ModelData.EntityRelationshipModel.FindByName( "Garage" ), "garage" ),
                    new QueryableEntity( (Entity)ModelData.EntityRelationshipModel.FindByName( "Supplier" ), "supplier" )
                } );

            RelationshipJoinArgument RJoinArgs = new RelationshipJoinArgument(
                (Relationship)ModelData.EntityRelationshipModel.FindByName( "Owns" ),
                new List<QueryableEntity> {
                    new QueryableEntity( CarRepairedByGarage ),
                    new QueryableEntity( (Entity)ModelData.EntityRelationshipModel.FindByName("Insurance"), "insurance" )
                } );

            RelationshipJoinOperator RJoinOp = new RelationshipJoinOperator(
                new QueryableEntity( ( Entity)ModelData.EntityRelationshipModel.FindByName( "Person" ), "person" ),
                new List<RelationshipJoinArgument> { RJoinArgs},
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
            RequiredDataContainer ModelData = ComputedEntityDataProvider.ManyToManyComputedEntity2();

            // Load handcrafted query
            string HandcraftedQuery = Utils.ReadQueryFromFile( "HandcraftedQueries/ceManyToMany-2.js" );

            // Assert if the handcrafted query is not null
            Assert.IsNotNull( HandcraftedQuery );

            // Prepare query generator
            ComputedEntity CarRepairedByGarage = new ComputedEntity( "CarManufacturedBy",
                new QueryableEntity( ( Entity)ModelData.EntityRelationshipModel.FindByName( "Car" ), "car" ),
                (Relationship)ModelData.EntityRelationshipModel.FindByName( "ManufacturedBy" ),
                new List<QueryableEntity> {
                    new QueryableEntity( (Entity)ModelData.EntityRelationshipModel.FindByName( "Manufacturer" ), "manufacturer" )
                } );

            RelationshipJoinArgument RJoinArgs = new RelationshipJoinArgument(
                (Relationship)ModelData.EntityRelationshipModel.FindByName( "Owns" ),
                new List<QueryableEntity> {
                    new QueryableEntity( CarRepairedByGarage )
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
