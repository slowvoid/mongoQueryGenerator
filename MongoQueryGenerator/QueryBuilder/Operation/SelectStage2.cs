using QueryBuilder.Mongo.Aggregation.Operators;
using QueryBuilder.Mongo.Expressions2;
using System.Collections.Generic;

namespace QueryBuilder.Operation
{
    /// <summary>
    /// Defines a selection operation
    /// aka filters over a collection
    /// </summary>
    public class SelectStage2 : AlgebraOperator
    {
        public LogicalExpression LogicalExpression { get; set; }
        override public string SummarizeToString()
        {
            string Ret = "SelectStage ";

            Ret += LogicalExpression.GetJavaScript();

            return Ret;
        }

        public override AlgebraOperatorResult Run()
        {
            MatchOperator MatchOp = new MatchOperator( LogicalExpression.ToExpr() );
            return new AlgebraOperatorResult( new List<MongoDBOperator>() { MatchOp } );
        }
    }
}