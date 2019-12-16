using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using QueryBuilder.Javascript;
using QueryBuilder.Map;
using QueryBuilder.Mongo.Expressions;
using QueryBuilder.Operation;
using QueryBuilder.Operation.Arguments;
using QueryBuilder.Query;
using System;
using System.Collections.Generic;
using System.Linq;

namespace QueryBuilder.Tests
{
    [TestClass]
    public class Select
    {
        [TestMethod]
        public void Equal()
        {
            RequiredDataContainer ModelData = SelectDataProvider.GetData();

            string HandcraftedQuery = Utils.ReadQueryFromFile( "HandcraftedQueries/selectEqual.js" );

            Assert.IsNotNull( HandcraftedQuery );

            MapRule PersonRule = ModelData.ERMongoMapping.Rules.First( R => R.Source.Name == "Person" );
            string AgeMap = PersonRule.Rules.First( R => R.Key == "age" ).Value;
            LogicalExpression left = new LogicalExpression( $"${AgeMap}", LogicalOperator.EQUAL, 27 );

            SelectArgument Arg = new SelectArgument( left );
            SelectStage SelectOp = new SelectStage( Arg, ModelData.ERMongoMapping );

            List<AlgebraOperator> OperatorsToExecute = new List<AlgebraOperator>() { SelectOp };
            FromArgument StartArg = new FromArgument( new QueryableEntity( ModelData.EntityRelationshipModel.FindByName( "Person" ) ),
                ModelData.ERMongoMapping );
            QueryGenerator QueryGen = new QueryGenerator( StartArg, OperatorsToExecute );

            string GeneratedQuery = QueryGen.Run();

            Assert.IsNotNull( GeneratedQuery );

            QueryRunner Runner = new QueryRunner( "mongodb://localhost:27017", "select" );

            string HandcraftedResult = Runner.GetJSON( HandcraftedQuery );
            string GeneratedResult = Runner.GetJSON( GeneratedQuery );

            // Check if either result is null
            Assert.IsNotNull( HandcraftedResult );
            Assert.IsNotNull( GeneratedResult );

            // Check if both results are equal
            Assert.IsTrue( JToken.DeepEquals( JToken.Parse( HandcraftedResult ), JToken.Parse( GeneratedResult ) ) );
        }
        [TestMethod]
        public void And()
        {
            RequiredDataContainer ModelData = SelectDataProvider.GetData();

            string HandcraftedQuery = Utils.ReadQueryFromFile( "HandcraftedQueries/selectAnd.js" );

            Assert.IsNotNull( HandcraftedQuery );

            MapRule PersonRule = ModelData.ERMongoMapping.Rules.First( R => R.Source.Name == "Person" );
            string AgeMap = PersonRule.Rules.First( R => R.Key == "age" ).Value;
            string NameMap = PersonRule.Rules.First( R => R.Key == "name" ).Value;
            LogicalExpression left = new LogicalExpression( $"${AgeMap}", LogicalOperator.EQUAL, 27 );
            LogicalExpression right = new LogicalExpression( $"${NameMap}", LogicalOperator.EQUAL, "Summer" );

            LogicalExpressionGroup expr = new LogicalExpressionGroup( left, LogicalOperator.AND, right );
            SelectArgument Arg = new SelectArgument( expr );
            SelectStage SelectOp = new SelectStage( Arg, ModelData.ERMongoMapping );

            List<AlgebraOperator> OperatorsToExecute = new List<AlgebraOperator>() { SelectOp };
            FromArgument StartArg = new FromArgument( new QueryableEntity( ModelData.EntityRelationshipModel.FindByName( "Person" ) ),
                ModelData.ERMongoMapping );
            QueryGenerator QueryGen = new QueryGenerator( StartArg, OperatorsToExecute );

            string GeneratedQuery = QueryGen.Run();

            Assert.IsNotNull( GeneratedQuery );

            QueryRunner Runner = new QueryRunner( "mongodb://localhost:27017", "select" );

            string HandcraftedResult = Runner.GetJSON( HandcraftedQuery );
            string GeneratedResult = Runner.GetJSON( GeneratedQuery );

            // Check if either result is null
            Assert.IsNotNull( HandcraftedResult );
            Assert.IsNotNull( GeneratedResult );

            // Check if both results are equal
            Assert.IsTrue( JToken.DeepEquals( JToken.Parse( HandcraftedResult ), JToken.Parse( GeneratedResult ) ) );
        }
        [TestMethod]
        public void GreaterOrEqualThan()
        {
            RequiredDataContainer ModelData = SelectDataProvider.GetData();

            string HandcraftedQuery = Utils.ReadQueryFromFile( "HandcraftedQueries/selectGreaterOrEqualThan.js" );

            Assert.IsNotNull( HandcraftedQuery );

            MapRule PersonRule = ModelData.ERMongoMapping.Rules.First( R => R.Source.Name == "Person" );
            string AgeMap = PersonRule.Rules.First( R => R.Key == "age" ).Value;
            LogicalExpression left = new LogicalExpression( $"${AgeMap}", LogicalOperator.GREATER_EQUAL_THAN, 27 );
            SelectArgument Arg = new SelectArgument( left );
            SelectStage SelectOp = new SelectStage( Arg, ModelData.ERMongoMapping );

            List<AlgebraOperator> OperatorsToExecute = new List<AlgebraOperator>() { SelectOp };
            FromArgument StartArg = new FromArgument( new QueryableEntity( ModelData.EntityRelationshipModel.FindByName( "Person" ) ),
                ModelData.ERMongoMapping );
            QueryGenerator QueryGen = new QueryGenerator( StartArg, OperatorsToExecute );

            string GeneratedQuery = QueryGen.Run();

            Assert.IsNotNull( GeneratedQuery );

            QueryRunner Runner = new QueryRunner( "mongodb://localhost:27017", "select" );

            string HandcraftedResult = Runner.GetJSON( HandcraftedQuery );
            string GeneratedResult = Runner.GetJSON( GeneratedQuery );

            // Check if either result is null
            Assert.IsNotNull( HandcraftedResult );
            Assert.IsNotNull( GeneratedResult );

            // Check if both results are equal
            Assert.IsTrue( JToken.DeepEquals( JToken.Parse( HandcraftedResult ), JToken.Parse( GeneratedResult ) ) );
        }
        [TestMethod]
        public void GreaterThan()
        {
            RequiredDataContainer ModelData = SelectDataProvider.GetData();

            string HandcraftedQuery = Utils.ReadQueryFromFile( "HandcraftedQueries/selectGreaterThan.js" );

            Assert.IsNotNull( HandcraftedQuery );

            MapRule PersonRule = ModelData.ERMongoMapping.Rules.First( R => R.Source.Name == "Person" );
            string AgeMap = PersonRule.Rules.First( R => R.Key == "age" ).Value;
            LogicalExpression left = new LogicalExpression( $"${AgeMap}", LogicalOperator.GREATER_THAN, 27 );

            SelectArgument Arg = new SelectArgument( left );
            SelectStage SelectOp = new SelectStage( Arg, ModelData.ERMongoMapping );

            List<AlgebraOperator> OperatorsToExecute = new List<AlgebraOperator>() { SelectOp };
            FromArgument StartArg = new FromArgument( new QueryableEntity( ModelData.EntityRelationshipModel.FindByName( "Person" ) ),
                ModelData.ERMongoMapping );
            QueryGenerator QueryGen = new QueryGenerator( StartArg, OperatorsToExecute );

            string GeneratedQuery = QueryGen.Run();

            Assert.IsNotNull( GeneratedQuery );

            QueryRunner Runner = new QueryRunner( "mongodb://localhost:27017", "select" );

            string HandcraftedResult = Runner.GetJSON( HandcraftedQuery );
            string GeneratedResult = Runner.GetJSON( GeneratedQuery );

            // Check if either result is null
            Assert.IsNotNull( HandcraftedResult );
            Assert.IsNotNull( GeneratedResult );

            // Check if both results are equal
            Assert.IsTrue( JToken.DeepEquals( JToken.Parse( HandcraftedResult ), JToken.Parse( GeneratedResult ) ) );
        }
        [TestMethod]
        public void InArray()
        {
            RequiredDataContainer ModelData = SelectDataProvider.GetData();

            string HandcraftedQuery = Utils.ReadQueryFromFile( "HandcraftedQueries/selectInArray.js" );

            Assert.IsNotNull( HandcraftedQuery );

            MapRule PersonRule = ModelData.ERMongoMapping.Rules.First( R => R.Source.Name == "Person" );
            string AgeMap = PersonRule.Rules.First( R => R.Key == "age" ).Value;
            LogicalExpression left = new LogicalExpression( $"${AgeMap}", LogicalOperator.IN,
                new JSArray( new List<object>() { 26, 27, 28, 29 } ) );

            SelectArgument Arg = new SelectArgument( left );
            SelectStage SelectOp = new SelectStage( Arg, ModelData.ERMongoMapping );

            List<AlgebraOperator> OperatorsToExecute = new List<AlgebraOperator>() { SelectOp };
            FromArgument StartArg = new FromArgument( new QueryableEntity( ModelData.EntityRelationshipModel.FindByName( "Person" ) ),
                ModelData.ERMongoMapping );
            QueryGenerator QueryGen = new QueryGenerator( StartArg, OperatorsToExecute );

            string GeneratedQuery = QueryGen.Run();

            Assert.IsNotNull( GeneratedQuery );

            QueryRunner Runner = new QueryRunner( "mongodb://localhost:27017", "select" );

            string HandcraftedResult = Runner.GetJSON( HandcraftedQuery );
            string GeneratedResult = Runner.GetJSON( GeneratedQuery );

            // Check if either result is null
            Assert.IsNotNull( HandcraftedResult );
            Assert.IsNotNull( GeneratedResult );

            // Check if both results are equal
            Assert.IsTrue( JToken.DeepEquals( JToken.Parse( HandcraftedResult ), JToken.Parse( GeneratedResult ) ) );
        }
        [TestMethod]
        public void LessOrEqualThan()
        {
            RequiredDataContainer ModelData = SelectDataProvider.GetData();

            string HandcraftedQuery = Utils.ReadQueryFromFile( "HandcraftedQueries/selectLessOrEqualThan.js" );

            Assert.IsNotNull( HandcraftedQuery );

            MapRule PersonRule = ModelData.ERMongoMapping.Rules.First( R => R.Source.Name == "Person" );
            string AgeMap = PersonRule.Rules.First( R => R.Key == "age" ).Value;
            LogicalExpression left = new LogicalExpression( $"${AgeMap}", LogicalOperator.LESS_EQUAL_THAN, 27 );

            SelectArgument Arg = new SelectArgument( left );
            SelectStage SelectOp = new SelectStage( Arg, ModelData.ERMongoMapping );

            List<AlgebraOperator> OperatorsToExecute = new List<AlgebraOperator>() { SelectOp };
            FromArgument StartArg = new FromArgument( new QueryableEntity( ModelData.EntityRelationshipModel.FindByName( "Person" ) ),
                ModelData.ERMongoMapping );
            QueryGenerator QueryGen = new QueryGenerator( StartArg, OperatorsToExecute );

            string GeneratedQuery = QueryGen.Run();

            Assert.IsNotNull( GeneratedQuery );

            QueryRunner Runner = new QueryRunner( "mongodb://localhost:27017", "select" );

            string HandcraftedResult = Runner.GetJSON( HandcraftedQuery );
            string GeneratedResult = Runner.GetJSON( GeneratedQuery );

            // Check if either result is null
            Assert.IsNotNull( HandcraftedResult );
            Assert.IsNotNull( GeneratedResult );

            // Check if both results are equal
            Assert.IsTrue( JToken.DeepEquals( JToken.Parse( HandcraftedResult ), JToken.Parse( GeneratedResult ) ) );
        }
        [TestMethod]
        public void LessThan()
        {
            RequiredDataContainer ModelData = SelectDataProvider.GetData();

            string HandcraftedQuery = Utils.ReadQueryFromFile( "HandcraftedQueries/selectLessThan.js" );

            Assert.IsNotNull( HandcraftedQuery );

            MapRule PersonRule = ModelData.ERMongoMapping.Rules.First( R => R.Source.Name == "Person" );
            string AgeMap = PersonRule.Rules.First( R => R.Key == "age" ).Value;
            LogicalExpression left = new LogicalExpression( $"${AgeMap}", LogicalOperator.LESS_THAN, 27 );

            SelectArgument Arg = new SelectArgument( left );
            SelectStage SelectOp = new SelectStage( Arg, ModelData.ERMongoMapping );

            List<AlgebraOperator> OperatorsToExecute = new List<AlgebraOperator>() { SelectOp };
            FromArgument StartArg = new FromArgument( new QueryableEntity( ModelData.EntityRelationshipModel.FindByName( "Person" ) ),
                ModelData.ERMongoMapping );
            QueryGenerator QueryGen = new QueryGenerator( StartArg, OperatorsToExecute );

            string GeneratedQuery = QueryGen.Run();

            Assert.IsNotNull( GeneratedQuery );

            QueryRunner Runner = new QueryRunner( "mongodb://localhost:27017", "select" );

            string HandcraftedResult = Runner.GetJSON( HandcraftedQuery );
            string GeneratedResult = Runner.GetJSON( GeneratedQuery );

            // Check if either result is null
            Assert.IsNotNull( HandcraftedResult );
            Assert.IsNotNull( GeneratedResult );

            // Check if both results are equal
            Assert.IsTrue( JToken.DeepEquals( JToken.Parse( HandcraftedResult ), JToken.Parse( GeneratedResult ) ) );
        }
        [TestMethod]
        public void NotEqual()
        {
            RequiredDataContainer ModelData = SelectDataProvider.GetData();

            string HandcraftedQuery = Utils.ReadQueryFromFile( "HandcraftedQueries/selectNotEqual.js" );

            Assert.IsNotNull( HandcraftedQuery );

            MapRule PersonRule = ModelData.ERMongoMapping.Rules.First( R => R.Source.Name == "Person" );
            string AgeMap = PersonRule.Rules.First( R => R.Key == "age" ).Value;

            LogicalExpression left = new LogicalExpression( $"${AgeMap}", LogicalOperator.NOT_EQUAL, 27 );

            SelectArgument Arg = new SelectArgument( left );
            SelectStage SelectOp = new SelectStage( Arg, ModelData.ERMongoMapping );

            List<AlgebraOperator> OperatorsToExecute = new List<AlgebraOperator>() { SelectOp };
            FromArgument StartArg = new FromArgument( new QueryableEntity( ModelData.EntityRelationshipModel.FindByName( "Person" ) ),
                ModelData.ERMongoMapping );
            QueryGenerator QueryGen = new QueryGenerator( StartArg, OperatorsToExecute );

            string GeneratedQuery = QueryGen.Run();

            Assert.IsNotNull( GeneratedQuery );

            QueryRunner Runner = new QueryRunner( "mongodb://localhost:27017", "select" );

            string HandcraftedResult = Runner.GetJSON( HandcraftedQuery );
            string GeneratedResult = Runner.GetJSON( GeneratedQuery );

            // Check if either result is null
            Assert.IsNotNull( HandcraftedResult );
            Assert.IsNotNull( GeneratedResult );

            // Check if both results are equal
            Assert.IsTrue( JToken.DeepEquals( JToken.Parse( HandcraftedResult ), JToken.Parse( GeneratedResult ) ) );
        }
        [TestMethod]
        public void NotInArray()
        {
            RequiredDataContainer ModelData = SelectDataProvider.GetData();

            string HandcraftedQuery = Utils.ReadQueryFromFile( "HandcraftedQueries/selectNotInArray.js" );

            Assert.IsNotNull( HandcraftedQuery );

            MapRule PersonRule = ModelData.ERMongoMapping.Rules.First( R => R.Source.Name == "Person" );
            string AgeMap = PersonRule.Rules.First( R => R.Key == "age" ).Value;

            LogicalExpression left = new LogicalExpression( $"${AgeMap}", LogicalOperator.NOT_IN,
                new JSArray( new List<object>() { 26, 27, 28, 29 } ) );

            SelectArgument Arg = new SelectArgument( left );
            SelectStage SelectOp = new SelectStage( Arg, ModelData.ERMongoMapping );

            List<AlgebraOperator> OperatorsToExecute = new List<AlgebraOperator>() { SelectOp };
            FromArgument StartArg = new FromArgument( new QueryableEntity( ModelData.EntityRelationshipModel.FindByName( "Person" ) ),
                ModelData.ERMongoMapping );
            QueryGenerator QueryGen = new QueryGenerator( StartArg, OperatorsToExecute );

            string GeneratedQuery = QueryGen.Run();

            Assert.IsNotNull( GeneratedQuery );

            QueryRunner Runner = new QueryRunner( "mongodb://localhost:27017", "select" );

            string HandcraftedResult = Runner.GetJSON( HandcraftedQuery );
            string GeneratedResult = Runner.GetJSON( GeneratedQuery );

            // Check if either result is null
            Assert.IsNotNull( HandcraftedResult );
            Assert.IsNotNull( GeneratedResult );

            // Check if both results are equal
            Assert.IsTrue( JToken.DeepEquals( JToken.Parse( HandcraftedResult ), JToken.Parse( GeneratedResult ) ) );
        }
        [TestMethod]
        public void Or()
        {
            RequiredDataContainer ModelData = SelectDataProvider.GetData();

            string HandcraftedQuery = Utils.ReadQueryFromFile( "HandcraftedQueries/selectOr.js" );

            Assert.IsNotNull( HandcraftedQuery );

            MapRule PersonRule = ModelData.ERMongoMapping.Rules.First( R => R.Source.Name == "Person" );
            string AgeMap = PersonRule.Rules.First( R => R.Key == "age" ).Value;
            LogicalExpression left = new LogicalExpression( $"${AgeMap}", LogicalOperator.EQUAL, 26 );
            LogicalExpression right = new LogicalExpression( $"${AgeMap}", LogicalOperator.EQUAL, 27 );

            LogicalExpressionGroup expr = new LogicalExpressionGroup( left, LogicalOperator.OR, right );
            SelectArgument Arg = new SelectArgument( expr );
            SelectStage SelectOp = new SelectStage( Arg, ModelData.ERMongoMapping );

            List<AlgebraOperator> OperatorsToExecute = new List<AlgebraOperator>() { SelectOp };
            FromArgument StartArg = new FromArgument( new QueryableEntity( ModelData.EntityRelationshipModel.FindByName( "Person" ) ),
                ModelData.ERMongoMapping );
            QueryGenerator QueryGen = new QueryGenerator( StartArg, OperatorsToExecute );

            string GeneratedQuery = QueryGen.Run();

            Assert.IsNotNull( GeneratedQuery );

            QueryRunner Runner = new QueryRunner( "mongodb://localhost:27017", "select" );

            string HandcraftedResult = Runner.GetJSON( HandcraftedQuery );
            string GeneratedResult = Runner.GetJSON( GeneratedQuery );

            // Check if either result is null
            Assert.IsNotNull( HandcraftedResult );
            Assert.IsNotNull( GeneratedResult );

            // Check if both results are equal
            Assert.IsTrue( JToken.DeepEquals( JToken.Parse( HandcraftedResult ), JToken.Parse( GeneratedResult ) ) );
        }
        [TestMethod]
        public void OrMultiple()
        {
            RequiredDataContainer ModelData = SelectDataProvider.GetData();

            string HandcraftedQuery = Utils.ReadQueryFromFile( "HandcraftedQueries/selectOrMultiple.js" );

            Assert.IsNotNull( HandcraftedQuery );

            MapRule PersonRule = ModelData.ERMongoMapping.Rules.First( R => R.Source.Name == "Person" );
            string AgeMap = PersonRule.Rules.First( R => R.Key == "age" ).Value;
            OrExpr expr = new OrExpr( $"${AgeMap}", new List<object>() { 18, 21, 36 } );

            SelectArgument Arg = new SelectArgument( expr );
            SelectStage SelectOp = new SelectStage( Arg, ModelData.ERMongoMapping );

            List<AlgebraOperator> OperatorsToExecute = new List<AlgebraOperator>() { SelectOp };
            FromArgument StartArg = new FromArgument( new QueryableEntity( ModelData.EntityRelationshipModel.FindByName( "Person" ) ),
                ModelData.ERMongoMapping );
            QueryGenerator QueryGen = new QueryGenerator( StartArg, OperatorsToExecute );

            string GeneratedQuery = QueryGen.Run();

            Assert.IsNotNull( GeneratedQuery );

            QueryRunner Runner = new QueryRunner( "mongodb://localhost:27017", "select" );

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