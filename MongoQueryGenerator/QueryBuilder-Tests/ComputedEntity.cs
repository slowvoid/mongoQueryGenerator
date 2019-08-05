﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
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
                new JoinableEntity( ( Entity)ModelData.EntityRelationshipModel.FindByName( "Car" ), "car" ),
                (Relationship)ModelData.EntityRelationshipModel.FindByName( "Repaired" ),
                new List<JoinableEntity> {
                    new JoinableEntity( (Entity)ModelData.EntityRelationshipModel.FindByName( "Garage" ), "garage" ) } );

            RelationshipJoinArguments RJoinArgs = new RelationshipJoinArguments(
                (Relationship)ModelData.EntityRelationshipModel.FindByName( "Drives" ),
                new List<JoinableEntity> { new JoinableEntity( CarRepairedByGarage ) } );

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
                new JoinableEntity( ( Entity)ModelData.EntityRelationshipModel.FindByName( "Car" ), "car" ),
                (Relationship)ModelData.EntityRelationshipModel.FindByName( "Repaired" ),
                new List<JoinableEntity> {
                    new JoinableEntity( (Entity)ModelData.EntityRelationshipModel.FindByName( "Garage" ), "garage" ),
                    new JoinableEntity( (Entity)ModelData.EntityRelationshipModel.FindByName("Supplier"), "supplier" )
                } );

            RelationshipJoinArguments RJoinArgs = new RelationshipJoinArguments(
                (Relationship)ModelData.EntityRelationshipModel.FindByName( "Drives" ),
                new List<JoinableEntity> { new JoinableEntity( CarRepairedByGarage ) } );

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
                new JoinableEntity( ( Entity)ModelData.EntityRelationshipModel.FindByName( "Car" ), "car" ),
                (Relationship)ModelData.EntityRelationshipModel.FindByName( "Repaired" ),
                new List<JoinableEntity> {
                    new JoinableEntity( (Entity)ModelData.EntityRelationshipModel.FindByName( "Garage" ), "garage" ),
                    new JoinableEntity( (Entity)ModelData.EntityRelationshipModel.FindByName("Supplier"), "supplier" )
                } );

            RelationshipJoinArguments RJoinArgs = new RelationshipJoinArguments(
                (Relationship)ModelData.EntityRelationshipModel.FindByName( "Drives" ),
                new List<JoinableEntity> { new JoinableEntity( CarRepairedByGarage ) } );

            RelationshipJoinArguments RJoinArgs2 = new RelationshipJoinArguments(
                (Relationship)ModelData.EntityRelationshipModel.FindByName( "HasInsurance" ),
                new List<JoinableEntity> { new JoinableEntity( (Entity)ModelData.EntityRelationshipModel.FindByName( "Insurance" ), "insurance") } );

            RelationshipJoinOperator RJoinOp = new RelationshipJoinOperator(
                new JoinableEntity( ( Entity)ModelData.EntityRelationshipModel.FindByName( "Person" ), "person" ),
                new List<RelationshipJoinArguments> { RJoinArgs, RJoinArgs2 },
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
                new JoinableEntity( ( Entity)ModelData.EntityRelationshipModel.FindByName( "Car" ), "car" ),
                (Relationship)ModelData.EntityRelationshipModel.FindByName( "Repaired" ),
                new List<JoinableEntity> {
                    new JoinableEntity( (Entity)ModelData.EntityRelationshipModel.FindByName( "Garage" ), "garage" )
                } );

            RelationshipJoinArguments RJoinArgs = new RelationshipJoinArguments(
                (Relationship)ModelData.EntityRelationshipModel.FindByName( "Drives" ),
                new List<JoinableEntity> { new JoinableEntity( CarRepairedByGarage ) } );

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
                new JoinableEntity( ( Entity)ModelData.EntityRelationshipModel.FindByName( "Car" ), "car" ),
                (Relationship)ModelData.EntityRelationshipModel.FindByName( "Repaired" ),
                new List<JoinableEntity> {
                    new JoinableEntity( (Entity)ModelData.EntityRelationshipModel.FindByName( "Garage" ), "garage" ),
                    new JoinableEntity( (Entity)ModelData.EntityRelationshipModel.FindByName( "Supplier" ), "supplier" )
                } );

            RelationshipJoinArguments RJoinArgs = new RelationshipJoinArguments(
                (Relationship)ModelData.EntityRelationshipModel.FindByName( "Drives" ),
                new List<JoinableEntity> {
                    new JoinableEntity( CarRepairedByGarage )
                } );

            RelationshipJoinArguments RJoinArgs2 = new RelationshipJoinArguments(
                (Relationship)ModelData.EntityRelationshipModel.FindByName( "HasInsurance" ),
                new List<JoinableEntity> {
                    new JoinableEntity( (Entity)ModelData.EntityRelationshipModel.FindByName( "Insurance" ), "insurance" )
                } );

            RelationshipJoinOperator RJoinOp = new RelationshipJoinOperator(
                new JoinableEntity( ( Entity)ModelData.EntityRelationshipModel.FindByName( "Person" ), "person" ),
                new List<RelationshipJoinArguments> { RJoinArgs, RJoinArgs2 },
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
                new JoinableEntity( ( Entity)ModelData.EntityRelationshipModel.FindByName( "Car" ), "car" ),
                (Relationship)ModelData.EntityRelationshipModel.FindByName( "Repaired" ),
                new List<JoinableEntity> {
                    new JoinableEntity( (Entity)ModelData.EntityRelationshipModel.FindByName( "Garage" ), "garage" ),
                    new JoinableEntity( (Entity)ModelData.EntityRelationshipModel.FindByName( "Supplier" ), "supplier" )
                } );

            RelationshipJoinArguments RJoinArgs = new RelationshipJoinArguments(
                (Relationship)ModelData.EntityRelationshipModel.FindByName( "Owns" ),
                new List<JoinableEntity> {
                    new JoinableEntity( CarRepairedByGarage ),
                    new JoinableEntity( (Entity)ModelData.EntityRelationshipModel.FindByName("Insurance"), "insurance" )
                } );

            RelationshipJoinOperator RJoinOp = new RelationshipJoinOperator(
                new JoinableEntity( ( Entity)ModelData.EntityRelationshipModel.FindByName( "Person" ), "person" ),
                new List<RelationshipJoinArguments> { RJoinArgs},
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
                new JoinableEntity( ( Entity)ModelData.EntityRelationshipModel.FindByName( "Car" ), "car" ),
                (Relationship)ModelData.EntityRelationshipModel.FindByName( "ManufacturedBy" ),
                new List<JoinableEntity> {
                    new JoinableEntity( (Entity)ModelData.EntityRelationshipModel.FindByName( "Manufacturer" ), "manufacturer" )
                } );

            RelationshipJoinArguments RJoinArgs = new RelationshipJoinArguments(
                (Relationship)ModelData.EntityRelationshipModel.FindByName( "Owns" ),
                new List<JoinableEntity> {
                    new JoinableEntity( CarRepairedByGarage )
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
