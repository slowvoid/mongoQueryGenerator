using QueryBuilder.ER;
using QueryBuilder.Map;
using QueryBuilder.Mongo;
using QueryBuilder.Mongo.Expressions;
using QueryBuilder.Operation;
using QueryBuilder.Operation.Arguments;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using QueryBuilder.Mongo.Aggregation.Operators;
using QueryBuilder.Query;
using QueryBuilder.Javascript;

namespace QueryBuilderApp
{
    public static class LogicalExpressionsTest
    {
        public static void Main()
        {
            LogicalExpression expr = new LogicalExpression( 20, LogicalOperator.GREATER_EQUAL_THAN, "30" );
            LogicalExpression expr2 = new LogicalExpression( "name", LogicalOperator.NOT_EQUAL, "me" );

            Console.WriteLine( expr.ToJSCode().ToString() );
            Console.WriteLine( expr2.ToJSCode().ToString() );

            LogicalExpressionGroup exprGroup = new LogicalExpressionGroup( expr, LogicalOperator.OR, expr2 );

            Console.WriteLine( exprGroup.ToJSCode().ToString() );

            LogicalExpressionGroup exprGroup2 = new LogicalExpressionGroup( exprGroup, LogicalOperator.AND, expr );

            Console.WriteLine( exprGroup2.ToJSCode().ToString() );

            ERModel Model = TestDataProvider.CreateModel();
            MongoSchema Schema = TestDataProvider.CreateSchema();
            ModelMapping Map = TestDataProvider.CreateMap( Model, Schema );

            MapRule PersonRule = Map.Rules.FirstOrDefault( R => R.Source.Name == "Person" );
            /*LogicalExpression left = new LogicalExpression( $"${PersonRule.Rules.First( R => R.Key == "name" ).Value}", LogicalOperator.NOT_IN,
                new JSArray( new List<object>() { "Astrid", "Jane", "Loiuse" } ) );

            SelectArgument Arg = new SelectArgument( left );
            SelectStage SelectOp = new SelectStage( Arg, Map );*/

            //NotInExpr notIn = new NotInExpr($"${PersonRule.Rules.First(R => R.Key == "name").Value}", new List<object>() { "Astrid", "Jane", "Louise"} );
            //LogicalExpression OrValues = new LogicalExpression( $"${PersonRule.Rules.First( R => R.Key == "age" ).Value}", LogicalOperator.OR, new JSArray( new List<object>() { 26, 27, 43 } ) );

            OrExpr OrValues = new OrExpr( $"${PersonRule.Rules.First( R => R.Key == "age" ).Value}", new List<object>() { 18, 21, 36 } );
            SelectArgument Arg = new SelectArgument( OrValues );
            SelectStage SelectOp = new SelectStage( Arg, Map );

            FromArgument StartArg = new FromArgument( new QueryableEntity( Model.FindByName( "Person" ) ), Map );
            QueryGenerator QueryGen = new QueryGenerator( StartArg, new List<AlgebraOperator>() { SelectOp } );

            string QueryString = QueryGen.Run();

            Console.WriteLine( QueryString );

            QueryRunner runner = new QueryRunner( "mongodb://localhost:27017", "testStuff" );
            Console.WriteLine( runner.GetJSON( QueryString ) );

            Console.ReadLine();
        }
    }
}