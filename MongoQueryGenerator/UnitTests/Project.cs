using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using QueryBuilder.ER;
using QueryBuilder.Map;
using QueryBuilder.Mongo.Expressions;
using QueryBuilder.Operation;
using QueryBuilder.Operation.Arguments;
using QueryBuilder.Parser;
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
            //RequiredDataContainer ModelData = ProjectDataProvider.SimpleModel();
            var ModelData = QueryBuilderParser.ParseMapping( Utils.ReadMappingFile( "Mappings/project-simple.mapping" ) );

            // Load handcrafted query
            string HandcraftedQuery = Utils.ReadQueryFromFile( "HandcraftedQueries/projectQuerySimple.js" );

            // Assert if the handcrafted query is not null
            Assert.IsNotNull( HandcraftedQuery );

            // Prepare query generator
            //QueryableEntity Person = new QueryableEntity( ModelData.EntityRelationshipModel.FindByName( "Person" ) );

            //List<ProjectArgument> Arguments = new List<ProjectArgument>();
            //Arguments.Add( new ProjectArgument( Person.GetAttribute( "personId" ), Person, new BooleanExpr( false ) ) );
            //Arguments.Add( new ProjectArgument( Person.GetAttribute( "name" ), Person, new BooleanExpr( true ) ) );
            //Arguments.Add( new ProjectArgument( Person.GetAttribute( "age" ), Person, new BooleanExpr( true ) ) );

            //ProjectStage ProjectOp = new ProjectStage( Arguments, ModelData.ERMongoMapping );

            //List<AlgebraOperator> OpList = new List<AlgebraOperator> { ProjectOp };
            //FromArgument StartArg = new FromArgument( new QueryableEntity( ModelData.EntityRelationshipModel.FindByName( "Person" ) ),
            //    ModelData.ERMongoMapping );

            //QueryGenerator QueryGen = new QueryGenerator( StartArg, OpList );

            string QueryString = "from Person select Person.name, Person.age";
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
        [TestMethod]
        public void ProjectComputedEntity()
        {
            // Asserts if the query result for a relationship join operation is equal
            // to a handcrafted query
            //RequiredDataContainer ModelData = ProjectDataProvider.ComputedEntityData();
            var ModelData = QueryBuilderParser.ParseMapping( Utils.ReadMappingFile( "Mappings/project-computed-entity.mapping" ) );

            // Load handcrafted query
            string HandcraftedQuery = Utils.ReadQueryFromFile( "HandcraftedQueries/projectQuery.js" );

            // Assert if the handcrafted query is not null
            Assert.IsNotNull( HandcraftedQuery );

            // Prepare query generator
            //ComputedEntity CarRepairedByGarage = new ComputedEntity( "CarManufacturedBy",
            //    new QueryableEntity( (Entity)ModelData.EntityRelationshipModel.FindByName( "Car" ), "car" ),
            //    (Relationship)ModelData.EntityRelationshipModel.FindByName( "ManufacturedBy" ),
            //    new List<QueryableEntity> {
            //        new QueryableEntity( (Entity)ModelData.EntityRelationshipModel.FindByName( "Manufacturer" ), "manufacturer" )
            //    } );

            //RelationshipJoinOperator RJoinOp = new RelationshipJoinOperator(
            //    new QueryableEntity( (Entity)ModelData.EntityRelationshipModel.FindByName( "Person" ), "person" ),
            //    (Relationship)ModelData.EntityRelationshipModel.FindByName( "Owns" ),
            //    new List<QueryableEntity> {
            //        new QueryableEntity( CarRepairedByGarage )
            //    },
            //    ModelData.ERMongoMapping );

            //VirtualMap VMap = RJoinOp.ComputeVirtualMap();

            //Dictionary<string, ProjectExpression> ProjectPersonAttrs = new Dictionary<string, ProjectExpression>();

            //QueryableEntity Person = new QueryableEntity( ModelData.EntityRelationshipModel.FindByName( "Person" ) );
            //QueryableEntity Car = new QueryableEntity( ModelData.EntityRelationshipModel.FindByName( "Car" ) );
            //QueryableEntity Manufacturer = new QueryableEntity( ModelData.EntityRelationshipModel.FindByName( "Manufacturer" ) );

            //List<ProjectArgument> ProjectArguments = new List<ProjectArgument>();
            //ProjectArguments.Add( new ProjectArgument( Person.GetAttribute( "name" ), Person, new BooleanExpr( true ) ) );
            //ProjectArguments.Add( new ProjectArgument( Car.GetAttribute( "model" ), Car, new BooleanExpr( true ) ) );
            //ProjectArguments.Add( new ProjectArgument( Car.GetAttribute( "year" ), Car, new BooleanExpr( true ) ) );
            //ProjectArguments.Add( new ProjectArgument( Manufacturer.GetAttribute( "name" ), Manufacturer, new BooleanExpr( true ) ) );

            //ProjectStage ProjectOp = new ProjectStage( ProjectArguments, VMap );

            //List<AlgebraOperator> OpList = new List<AlgebraOperator> { RJoinOp, ProjectOp };
            //FromArgument StartArg = new FromArgument( new QueryableEntity( ModelData.EntityRelationshipModel.FindByName( "Person" ) ),
            //    ModelData.ERMongoMapping );

            //QueryGenerator QueryGen = new QueryGenerator( StartArg, OpList );
            string QueryString = "from Person rjoin <Owns> (Car rjoin <ManufacturedBy> (Manufacturer)) select Person.name, Car.model, Car.year, Manufacturer.name";

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