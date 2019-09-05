using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using QueryBuilder.ER;
using QueryBuilder.Map;
using QueryBuilder.Mongo.Expressions;
using QueryBuilder.Operation;
using QueryBuilder.Operation.Arguments;
using QueryBuilder.Query;
using System.Collections.Generic;

namespace QueryBuilder.Tests
{
    [TestClass]
    public class ProjectTests
    {
        [TestMethod]
        public void ProjectSimpleAttributes()
        {
            // Asserts if the query result for a relationship join operation is equal
            // to a handcrafted query
            RequiredDataContainer ModelData = ProjectDataProvider.SimpleModel();

            // Load handcrafted query
            string HandcraftedQuery = Utils.ReadQueryFromFile( "HandcraftedQueries/projectQuerySimple.js" );

            // Assert if the handcrafted query is not null
            Assert.IsNotNull( HandcraftedQuery );

            // Prepare query generator
            Entity Person = (Entity)ModelData.EntityRelationshipModel.FindByName( "Person" );
            Dictionary<string, ProjectExpression> ProjectPersonAttrs = new Dictionary<string, ProjectExpression>();
            ProjectPersonAttrs.Add( Person.GetAttribute( "personId" ).Name, new BooleanExpr( false ) );
            ProjectPersonAttrs.Add( Person.GetAttribute( "name" ).Name, new BooleanExpr( true ) );
            ProjectPersonAttrs.Add( Person.GetAttribute( "age" ).Name, new BooleanExpr( true ) );

            ProjectArgument PersonArgs = new ProjectArgument( new QueryableEntity( Person ), ProjectPersonAttrs );
            ProjectStage ProjectOp = new ProjectStage( new ProjectArgument[] { PersonArgs }, ModelData.ERMongoMapping );

            List<AlgebraOperator> OpList = new List<AlgebraOperator> { ProjectOp };
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
        [TestMethod]
        public void ProjectComputedEntity()
        {
            // Asserts if the query result for a relationship join operation is equal
            // to a handcrafted query
            RequiredDataContainer ModelData = ProjectDataProvider.ComputedEntityData();

            // Load handcrafted query
            string HandcraftedQuery = Utils.ReadQueryFromFile( "HandcraftedQueries/projectQuery.js" );

            // Assert if the handcrafted query is not null
            Assert.IsNotNull( HandcraftedQuery );

            // Prepare query generator
            ComputedEntity CarRepairedByGarage = new ComputedEntity( "CarManufacturedBy",
                new QueryableEntity( (Entity)ModelData.EntityRelationshipModel.FindByName( "Car" ), "car" ),
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
                new QueryableEntity( (Entity)ModelData.EntityRelationshipModel.FindByName( "Person" ), "person" ),
                new List<RelationshipJoinArgument> { RJoinArgs },
                ModelData.ERMongoMapping );

            VirtualMap VMap = RJoinOp.ComputeVirtualMap();

            Dictionary<string, ProjectExpression> ProjectPersonAttrs = new Dictionary<string, ProjectExpression>();
            Entity Person = (Entity)ModelData.EntityRelationshipModel.FindByName( "Person" );
            ProjectPersonAttrs.Add( Person.GetAttribute( "name" ).Name, new BooleanExpr( true ) );
            ProjectArgument PersonArgs = new ProjectArgument( new QueryableEntity( Person, "person" ), ProjectPersonAttrs );

            Dictionary<string, ProjectExpression> ProjectCarAttrs = new Dictionary<string, ProjectExpression>();
            Entity Car = (Entity)ModelData.EntityRelationshipModel.FindByName( "Car" );
            ProjectCarAttrs.Add( Car.GetAttribute( "model" ).Name, new BooleanExpr( true ) );
            ProjectCarAttrs.Add( Car.GetAttribute( "year" ).Name, new BooleanExpr( true ) );
            ProjectArgument CarArgs = new ProjectArgument( new QueryableEntity( Car, "car" ), ProjectCarAttrs );

            Dictionary<string, ProjectExpression> ProjectManufacturerAttrs = new Dictionary<string, ProjectExpression>();
            Entity Manufacturer = (Entity)ModelData.EntityRelationshipModel.FindByName( "Manufacturer" );
            ProjectManufacturerAttrs.Add( ModelData.EntityRelationshipModel.FindByName( "Manufacturer" ).GetAttribute( "name" ).Name, new BooleanExpr( true ) );
            ProjectArgument ManufacturerArgs = new ProjectArgument( new QueryableEntity( Manufacturer, "manufacturer" ), ProjectManufacturerAttrs );

            ProjectStage ProjectOp = new ProjectStage( new ProjectArgument[] { PersonArgs, CarArgs, ManufacturerArgs }, VMap );

            List<AlgebraOperator> OpList = new List<AlgebraOperator> { RJoinOp, ProjectOp };
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