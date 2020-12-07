﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
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
    public class ManyToManyRelationships
    {
        [TestMethod]
        public void ManyToManySingleEntity()
        {
            // Asserts if the query result for a simple binary join is equal
            // to a handcrafted query
            //RequiredDataContainer ModelData = ManyToManyRelationshipsDataProvider.ManyToManySingleEntity();
            var ModelData = QueryBuilderParser.ParseMapping( Utils.ReadMappingFile( "Mappings/many-to-many-single-entity.mapping" ) );

            // Load handcrafted query
            string HandcraftedQuery = Utils.ReadQueryFromFile( "HandcraftedQueries/personCarManyToMany_1.js" );

            // Assert if the handcrafted query is not null
            Assert.IsNotNull( HandcraftedQuery );

            // Prepare query generator
            RelationshipJoinOperator RJoinOp = new RelationshipJoinOperator(
                new QueryableEntity( ( Entity)ModelData.EntityRelationshipModel.FindByName( "Person" ), "person" ),
                (Relationship)ModelData.EntityRelationshipModel.FindByName( "Insurance" ),
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
            //RequiredDataContainer ModelData = ManyToManyRelationshipsDataProvider.ManyToManyMultipleEntities();
            var ModelData = QueryBuilderParser.ParseMapping( Utils.ReadMappingFile( "Mappings/many-to-many-multiple-entities.mapping" ) );

            // Load handcrafted query
            string HandcraftedQuery = Utils.ReadQueryFromFile( "HandcraftedQueries/personCarInsCompanyManyToMany.js" );

            // Assert if the handcrafted query is not null
            Assert.IsNotNull( HandcraftedQuery );

            // Prepare query generator

            // FROM Person RJOIN (Car, InsCompany)

            RelationshipJoinOperator RJoinOp = new RelationshipJoinOperator(
                new QueryableEntity( ( Entity)ModelData.EntityRelationshipModel.FindByName( "Person" ), "person" ),
                (Relationship)ModelData.EntityRelationshipModel.FindByName( "Insurance" ),
                new List<QueryableEntity> {
                    new QueryableEntity( (Entity)ModelData.EntityRelationshipModel.FindByName( "Car" ), "car" ),
                    new QueryableEntity( (Entity)ModelData.EntityRelationshipModel.FindByName("InsCompany"), "inscompany" )
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
            //RequiredDataContainer ModelData = ManyToManyRelationshipsDataProvider.ManyToManyRelationshipAttributeSingleEntity();
            var ModelData = QueryBuilderParser.ParseMapping( Utils.ReadMappingFile( "Mappings/many-to-many-relationship-attribute-single-entity.mapping" ) );

            // Load handcrafted query
            string HandcraftedQuery = Utils.ReadQueryFromFile( "HandcraftedQueries/manyToManyRelationshipAttributes.js" );

            // Assert if the handcrafted query is not null
            Assert.IsNotNull( HandcraftedQuery );

            // Prepare query generator
            RelationshipJoinOperator RJoinOp = new RelationshipJoinOperator(
                new QueryableEntity( ( Entity)ModelData.EntityRelationshipModel.FindByName( "Person" ), "person" ),
                (Relationship)ModelData.EntityRelationshipModel.FindByName( "Insurance" ),
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
            //RequiredDataContainer ModelData = ManyToManyRelationshipsDataProvider.ManyToManyRelationshipAttributeMultipleEntities();
            var ModelData = QueryBuilderParser.ParseMapping( Utils.ReadMappingFile( "Mappings/many-to-many-relationship-attribute-multiple-entities.mapping" ) );

            // Load handcrafted query
            string HandcraftedQuery = Utils.ReadQueryFromFile( "HandcraftedQueries/manyToManyRelationshipAttributesMultipleEntities.js" );

            // Assert if the handcrafted query is not null
            Assert.IsNotNull( HandcraftedQuery );

            // Prepare query generator
            RelationshipJoinOperator RJoinOp = new RelationshipJoinOperator(
                new QueryableEntity( ( Entity)ModelData.EntityRelationshipModel.FindByName( "Person" ), "person" ),
                (Relationship)ModelData.EntityRelationshipModel.FindByName( "Insurance" ),
                new List<QueryableEntity> {
                    new QueryableEntity( (Entity)ModelData.EntityRelationshipModel.FindByName( "Car" ), "car" ),
                    new QueryableEntity( (Entity)ModelData.EntityRelationshipModel.FindByName("InsCompany"), "inscompany" )
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
        public void ManyToManyTargetEmbedded()
        {
            //RequiredDataContainer ModelData = ManyToManyRelationshipsDataProvider.ManyToManyEmbeddedTarget();
            var ModelData = QueryBuilderParser.ParseMapping( Utils.ReadMappingFile( "Mappings/many-to-many-embedded-target.mapping" ) );

            // Load handcrafted query
            string HandcraftedQuery = Utils.ReadQueryFromFile( "HandcraftedQueries/manyToManyEmbedded.js" );

            // Assert if the handcrafted query is not null
            Assert.IsNotNull( HandcraftedQuery );

            QueryableEntity Person = new QueryableEntity( ModelData.EntityRelationshipModel.FindByName( "Person" ) );
            QueryableEntity Car = new QueryableEntity( ModelData.EntityRelationshipModel.FindByName( "Car" ) );

            RelationshipJoinOperator RJoinOp = new RelationshipJoinOperator(
                Person,
                (Relationship)ModelData.EntityRelationshipModel.FindByName( "Drives" ),
                new List<QueryableEntity>() { Car },
                ModelData.ERMongoMapping );

            FromArgument FromArg = new FromArgument( Person, ModelData.ERMongoMapping );

            QueryGenerator QueryGen = new QueryGenerator( FromArg, new List<AlgebraOperator>() { RJoinOp } );
            string GeneratedQuery = QueryGen.Run();

            Assert.IsNotNull( GeneratedQuery );

            // Run Queries
            QueryRunner Runner = new QueryRunner( "mongodb://localhost:27017", "manyToManyEmbedded" );

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
