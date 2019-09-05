using QueryBuilder.Javascript;
using QueryBuilder.Mongo.Expressions;
using System;
using System.Collections.Generic;

namespace QueryBuilderApp
{
    public static class JSTests
    {
        public static void Main()
        {
            InExpr testExpr = new InExpr( "$age", new List<object>() { 18, 19, 20, 21, 22, 29 } );
            NotInExpr test2Expr = new NotInExpr( "$age", new List<object>() { 18, 19, 20, 21, 22, 29 } );

            Console.WriteLine( testExpr.ToJavaScript() );
            Console.WriteLine( test2Expr.ToJavaScript() );

            Console.Read();
        }
    }
}