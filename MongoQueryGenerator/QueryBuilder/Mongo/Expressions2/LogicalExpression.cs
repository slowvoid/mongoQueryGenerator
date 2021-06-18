using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QueryBuilder.Javascript;
using QueryBuilder.ER;
using QueryBuilder.Shared;
using QueryBuilder.Mongo.Expressions;
using QueryBuilder.Map;

namespace QueryBuilder.Mongo.Expressions2
{
    public class LogicalExpression
    {
        // There is one more term than operator
        // Ex: A or B and C
        // LogicalTerms = {A,B,C}
        // LogicalOperators = {or,and}
        public List<LogicalTerm> LogicalTerms { get; set; }
        public List<LogicalOperator> LogicalOperators { get; set; }

        public LogicalExpression()
        {
            this.LogicalTerms = new List<LogicalTerm>();
            this.LogicalOperators = new List<LogicalOperator>();
        }

        public Expr ToExpr()
        {
            Expr expr = null;

            for ( int i = 0; i < LogicalTerms.Count; i += 2 )
            {
                BaseLogicalExpression left = LogicalTerms[ i ].ToExpression();

                if ( i + 1 >= LogicalTerms.Count )
                {
                    // return just the first term
                    expr = new Expr( left );
                    break;
                }

                BaseLogicalExpression right = LogicalTerms[ i + 1 ].ToExpression();

                if ( i < LogicalOperators.Count )
                {
                    LogicalOperator op = LogicalOperators.ElementAt( i );
                    switch ( op )
                    {
                        case LogicalOperator.AND:
                            expr = new Expr(new LogicalExpressionGroup( left, Expressions.LogicalOperator.AND, right ));
                            break;
                        case LogicalOperator.OR:
                            expr = new Expr(new LogicalExpressionGroup( left, Expressions.LogicalOperator.OR, right ));
                            break;
                    }
                }
                else
                {
                    // This is probably invalid
                    throw new InvalidOperationException();
                }
            }

            return expr;
        }

        public string GetJavaScript()
        {
            string jsCode = "";

            for (int i = 0; i < LogicalTerms.Count; i++)
            {
                jsCode += LogicalTerms.ElementAt(i).GetJavaScript();
                if (i < LogicalOperators.Count)
                {
                    switch (LogicalOperators.ElementAt(i))
                    {
                        case LogicalOperator.AND:
                            jsCode += " && ";
                            break;
                        case LogicalOperator.OR:
                            jsCode += " || ";
                            break;
                    }
                }
            }


            return jsCode;
        }
    }

    public abstract class LogicalTerm
    {
        public abstract string GetJavaScript();
        public abstract BaseLogicalExpression ToExpression();
    }
    public class RelationalLogicalTerm : LogicalTerm
    {
        public SimpleAttribute SimpleAttribute { get; set; }
        public RelationalOperator RelationalOperator { get; set; }
        public string Value { get; set; }

        public override BaseLogicalExpression ToExpression()
        {
            BaseLogicalExpression expr = null;
            switch ( RelationalOperator )
            {
                case RelationalOperator.EQUAL:
                    expr = new EqExpr( SimpleAttribute.value, Value );
                    break;
                case RelationalOperator.NOT_EQUAL:
                    expr = new NeqExpr( SimpleAttribute.value, Value );
                    break;
                case RelationalOperator.GREATER_EQUAL:
                    expr = new GteExpr( SimpleAttribute.value, Value );
                    break;
                case RelationalOperator.LESS_EQUAL:
                    expr = new LteExpr( SimpleAttribute.value, Value );
                    break;
                case RelationalOperator.GREATER:
                    expr = new GtExpr( SimpleAttribute.value, Value );
                    break;
                case RelationalOperator.LESS:
                    expr = new LtExpr( SimpleAttribute.value, Value );
                    break;
                case RelationalOperator.LIKE:
                case RelationalOperator.IS:
                    throw new NotSupportedException( "Operation not supported" );
            }

            return expr;
        }

        override public string GetJavaScript()
        {
            string jsCode = "";

            jsCode += SimpleAttribute.GetJavaScript();
            switch (RelationalOperator)
            {
                case RelationalOperator.EQUAL:
                    jsCode += " == " + Value;
                    break;
                case RelationalOperator.NOT_EQUAL:
                    jsCode += " != " + Value;
                    break;
                case RelationalOperator.GREATER_EQUAL:
                    jsCode += " >= " + Value;
                    break;
                case RelationalOperator.LESS_EQUAL:
                    jsCode += " <= " + Value;
                    break;
                case RelationalOperator.GREATER:
                    jsCode += " > " + Value;
                    break;
                case RelationalOperator.LESS:
                    jsCode += " < " + Value;
                    break;
                case RelationalOperator.LIKE:
                    jsCode += " like(" + Value + ")";
                    break;
                case RelationalOperator.IS:
                    jsCode += " is " + Value;
                    break;
            }

            return jsCode;
        }
    }

    public class RangeLogicalTerm : LogicalTerm
    {
        public SimpleAttribute SimpleAttribute { get; set; }
        public RangeOperator RangeOperator { get; set; }

        // Depending on the operator, it could be one, two or a list of values
        public List<string> Values { get; set; }

        public RangeLogicalTerm()
        {
            Values = new List<string>();
        }

        public override BaseLogicalExpression ToExpression()
        {
            throw new NotImplementedException();
        }

        override public string GetJavaScript()
        {
            string jsCode = "";

            switch (RangeOperator)
            {
                case RangeOperator.BETWEEN:
                    jsCode += "(" + SimpleAttribute.GetJavaScript() + " > " + Values.ElementAt(0) + " && " +
                              SimpleAttribute.GetJavaScript() + " < " + Values.ElementAt(1) + ")";
                    break;
                case RangeOperator.NOT_IN_QUERY:
                    jsCode += "NOT_SUPPORTED";
                    break;
                case RangeOperator.NOT_IN_VALUES:
                    jsCode += "NOT_SUPPORTED";
                    break;
                case RangeOperator.IN_QUERY:
                    jsCode += "NOT_SUPPORTED";
                    break;
                case RangeOperator.IN_VALUES:
                    jsCode += "NOT_SUPPORTED";
                    break;
                case RangeOperator.NOT_EXISTS_QUERY:
                    jsCode += "NOT_SUPPORTED";
                    break;
                case RangeOperator.EXISTS_QUERY:
                    jsCode += "NOT_SUPPORTED";
                    break;
            }

            return jsCode;
        }
    }

    public class ParenthesisLogicalTerm : LogicalTerm
    {
        public LogicalExpression LogicalExpression { get; set; }

        override public string GetJavaScript()
        {
            return "(" + LogicalExpression.GetJavaScript() + ")";
        }

        public override BaseLogicalExpression ToExpression()
        {
            throw new NotImplementedException();
        }

    }

    public class SimpleAttribute
    {

        public SimpleAttribute(string value)
        {
            this.value = value;
        }
        public string value { get; private set; }
        public string GetJavaScript()
        {
            return value;
        }

    }

    public enum LogicalOperator { AND, OR }
    public enum RelationalOperator { EQUAL, NOT_EQUAL, GREATER_EQUAL, LESS_EQUAL, GREATER, LESS, LIKE, IS }
    public enum RangeOperator { BETWEEN, NOT_IN_QUERY, NOT_IN_VALUES, IN_QUERY, IN_VALUES, NOT_EXISTS_QUERY, EXISTS_QUERY }


}