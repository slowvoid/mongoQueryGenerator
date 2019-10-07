using QueryBuilder.Mongo.Expressions;
using System;

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

            Console.ReadLine();
        }
    }
}