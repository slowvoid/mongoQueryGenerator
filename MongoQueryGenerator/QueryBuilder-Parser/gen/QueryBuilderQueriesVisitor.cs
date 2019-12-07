//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     ANTLR Version: 4.7.1
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// Generated from /Users/daniellucredio/GitProjects/mongoQueryGenerator/MongoQueryGeneratorFork/QueryBuilder-Parser/QueryBuilderQueries.g4 by ANTLR 4.7.1

// Unreachable code detected
#pragma warning disable 0162
// The variable '...' is assigned but its value is never used
#pragma warning disable 0219
// Missing XML comment for publicly visible type or member '...'
#pragma warning disable 1591
// Ambiguous reference in cref attribute
#pragma warning disable 419

namespace QueryBuilder.Parser
{
    using Antlr4.Runtime.Misc;
    using Antlr4.Runtime.Tree;
    using IToken = Antlr4.Runtime.IToken;

    /// <summary>
    /// This interface defines a complete generic visitor for a parse tree produced
    /// by <see cref="QueryBuilderQueriesParser"/>.
    /// </summary>
    /// <typeparam name="Result">The return type of the visit operation.</typeparam>
    [System.CodeDom.Compiler.GeneratedCode( "ANTLR", "4.7.1" )]
    [System.CLSCompliant( false )]
    public interface IQueryBuilderQueriesVisitor<Result> : IParseTreeVisitor<Result>
    {
        /// <summary>
        /// Visit a parse tree produced by <see cref="QueryBuilderQueriesParser.query"/>.
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor result.</return>
        Result VisitQuery( [NotNull] QueryBuilderQueriesParser.QueryContext context );
        /// <summary>
        /// Visit a parse tree produced by the <c>simpleEntity</c>
        /// labeled alternative in <see cref="QueryBuilderQueriesParser.entity"/>.
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor result.</return>
        Result VisitSimpleEntity( [NotNull] QueryBuilderQueriesParser.SimpleEntityContext context );
        /// <summary>
        /// Visit a parse tree produced by the <c>computedEntity</c>
        /// labeled alternative in <see cref="QueryBuilderQueriesParser.entity"/>.
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor result.</return>
        Result VisitComputedEntity( [NotNull] QueryBuilderQueriesParser.ComputedEntityContext context );
        /// <summary>
        /// Visit a parse tree produced by the <c>parenthesisEntity</c>
        /// labeled alternative in <see cref="QueryBuilderQueriesParser.entity"/>.
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor result.</return>
        Result VisitParenthesisEntity( [NotNull] QueryBuilderQueriesParser.ParenthesisEntityContext context );
        /// <summary>
        /// Visit a parse tree produced by <see cref="QueryBuilderQueriesParser.select"/>.
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor result.</return>
        Result VisitSelect( [NotNull] QueryBuilderQueriesParser.SelectContext context );
        /// <summary>
        /// Visit a parse tree produced by <see cref="QueryBuilderQueriesParser.simpleAttribute"/>.
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor result.</return>
        Result VisitSimpleAttribute( [NotNull] QueryBuilderQueriesParser.SimpleAttributeContext context );
        /// <summary>
        /// Visit a parse tree produced by <see cref="QueryBuilderQueriesParser.listOfAttributes"/>.
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor result.</return>
        Result VisitListOfAttributes( [NotNull] QueryBuilderQueriesParser.ListOfAttributesContext context );
        /// <summary>
        /// Visit a parse tree produced by <see cref="QueryBuilderQueriesParser.alias"/>.
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor result.</return>
        Result VisitAlias( [NotNull] QueryBuilderQueriesParser.AliasContext context );
        /// <summary>
        /// Visit a parse tree produced by <see cref="QueryBuilderQueriesParser.aggregationFunction"/>.
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor result.</return>
        Result VisitAggregationFunction( [NotNull] QueryBuilderQueriesParser.AggregationFunctionContext context );
        /// <summary>
        /// Visit a parse tree produced by <see cref="QueryBuilderQueriesParser.where"/>.
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor result.</return>
        Result VisitWhere( [NotNull] QueryBuilderQueriesParser.WhereContext context );
        /// <summary>
        /// Visit a parse tree produced by <see cref="QueryBuilderQueriesParser.expressionList"/>.
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor result.</return>
        Result VisitExpressionList( [NotNull] QueryBuilderQueriesParser.ExpressionListContext context );
        /// <summary>
        /// Visit a parse tree produced by <see cref="QueryBuilderQueriesParser.arithmeticExpression"/>.
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor result.</return>
        Result VisitArithmeticExpression( [NotNull] QueryBuilderQueriesParser.ArithmeticExpressionContext context );
        /// <summary>
        /// Visit a parse tree produced by <see cref="QueryBuilderQueriesParser.otherExpression"/>.
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor result.</return>
        Result VisitOtherExpression( [NotNull] QueryBuilderQueriesParser.OtherExpressionContext context );
        /// <summary>
        /// Visit a parse tree produced by <see cref="QueryBuilderQueriesParser.logicalExpression"/>.
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor result.</return>
        Result VisitLogicalExpression( [NotNull] QueryBuilderQueriesParser.LogicalExpressionContext context );
        /// <summary>
        /// Visit a parse tree produced by <see cref="QueryBuilderQueriesParser.groupby"/>.
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor result.</return>
        Result VisitGroupby( [NotNull] QueryBuilderQueriesParser.GroupbyContext context );
        /// <summary>
        /// Visit a parse tree produced by <see cref="QueryBuilderQueriesParser.having"/>.
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor result.</return>
        Result VisitHaving( [NotNull] QueryBuilderQueriesParser.HavingContext context );
        /// <summary>
        /// Visit a parse tree produced by <see cref="QueryBuilderQueriesParser.orderby"/>.
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor result.</return>
        Result VisitOrderby( [NotNull] QueryBuilderQueriesParser.OrderbyContext context );
    }
} // namespace QueryBuilder.Parser