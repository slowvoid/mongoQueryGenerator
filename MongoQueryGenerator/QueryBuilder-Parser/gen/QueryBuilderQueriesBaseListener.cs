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
    using IErrorNode = Antlr4.Runtime.Tree.IErrorNode;
    using ITerminalNode = Antlr4.Runtime.Tree.ITerminalNode;
    using IToken = Antlr4.Runtime.IToken;
    using ParserRuleContext = Antlr4.Runtime.ParserRuleContext;

    /// <summary>
    /// This class provides an empty implementation of <see cref="IQueryBuilderQueriesListener"/>,
    /// which can be extended to create a listener which only needs to handle a subset
    /// of the available methods.
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCode( "ANTLR", "4.7.1" )]
    [System.CLSCompliant( false )]
    public partial class QueryBuilderQueriesBaseListener : IQueryBuilderQueriesListener
    {
        /// <summary>
        /// Enter a parse tree produced by <see cref="QueryBuilderQueriesParser.query"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public virtual void EnterQuery( [NotNull] QueryBuilderQueriesParser.QueryContext context ) { }
        /// <summary>
        /// Exit a parse tree produced by <see cref="QueryBuilderQueriesParser.query"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public virtual void ExitQuery( [NotNull] QueryBuilderQueriesParser.QueryContext context ) { }
        /// <summary>
        /// Enter a parse tree produced by the <c>simpleEntity</c>
        /// labeled alternative in <see cref="QueryBuilderQueriesParser.entity"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public virtual void EnterSimpleEntity( [NotNull] QueryBuilderQueriesParser.SimpleEntityContext context ) { }
        /// <summary>
        /// Exit a parse tree produced by the <c>simpleEntity</c>
        /// labeled alternative in <see cref="QueryBuilderQueriesParser.entity"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public virtual void ExitSimpleEntity( [NotNull] QueryBuilderQueriesParser.SimpleEntityContext context ) { }
        /// <summary>
        /// Enter a parse tree produced by the <c>computedEntity</c>
        /// labeled alternative in <see cref="QueryBuilderQueriesParser.entity"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public virtual void EnterComputedEntity( [NotNull] QueryBuilderQueriesParser.ComputedEntityContext context ) { }
        /// <summary>
        /// Exit a parse tree produced by the <c>computedEntity</c>
        /// labeled alternative in <see cref="QueryBuilderQueriesParser.entity"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public virtual void ExitComputedEntity( [NotNull] QueryBuilderQueriesParser.ComputedEntityContext context ) { }
        /// <summary>
        /// Enter a parse tree produced by the <c>parenthesisEntity</c>
        /// labeled alternative in <see cref="QueryBuilderQueriesParser.entity"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public virtual void EnterParenthesisEntity( [NotNull] QueryBuilderQueriesParser.ParenthesisEntityContext context ) { }
        /// <summary>
        /// Exit a parse tree produced by the <c>parenthesisEntity</c>
        /// labeled alternative in <see cref="QueryBuilderQueriesParser.entity"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public virtual void ExitParenthesisEntity( [NotNull] QueryBuilderQueriesParser.ParenthesisEntityContext context ) { }
        /// <summary>
        /// Enter a parse tree produced by <see cref="QueryBuilderQueriesParser.select"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public virtual void EnterSelect( [NotNull] QueryBuilderQueriesParser.SelectContext context ) { }
        /// <summary>
        /// Exit a parse tree produced by <see cref="QueryBuilderQueriesParser.select"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public virtual void ExitSelect( [NotNull] QueryBuilderQueriesParser.SelectContext context ) { }
        /// <summary>
        /// Enter a parse tree produced by <see cref="QueryBuilderQueriesParser.simpleAttribute"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public virtual void EnterSimpleAttribute( [NotNull] QueryBuilderQueriesParser.SimpleAttributeContext context ) { }
        /// <summary>
        /// Exit a parse tree produced by <see cref="QueryBuilderQueriesParser.simpleAttribute"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public virtual void ExitSimpleAttribute( [NotNull] QueryBuilderQueriesParser.SimpleAttributeContext context ) { }
        /// <summary>
        /// Enter a parse tree produced by <see cref="QueryBuilderQueriesParser.listOfAttributes"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public virtual void EnterListOfAttributes( [NotNull] QueryBuilderQueriesParser.ListOfAttributesContext context ) { }
        /// <summary>
        /// Exit a parse tree produced by <see cref="QueryBuilderQueriesParser.listOfAttributes"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public virtual void ExitListOfAttributes( [NotNull] QueryBuilderQueriesParser.ListOfAttributesContext context ) { }
        /// <summary>
        /// Enter a parse tree produced by <see cref="QueryBuilderQueriesParser.alias"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public virtual void EnterAlias( [NotNull] QueryBuilderQueriesParser.AliasContext context ) { }
        /// <summary>
        /// Exit a parse tree produced by <see cref="QueryBuilderQueriesParser.alias"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public virtual void ExitAlias( [NotNull] QueryBuilderQueriesParser.AliasContext context ) { }
        /// <summary>
        /// Enter a parse tree produced by <see cref="QueryBuilderQueriesParser.aggregationFunction"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public virtual void EnterAggregationFunction( [NotNull] QueryBuilderQueriesParser.AggregationFunctionContext context ) { }
        /// <summary>
        /// Exit a parse tree produced by <see cref="QueryBuilderQueriesParser.aggregationFunction"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public virtual void ExitAggregationFunction( [NotNull] QueryBuilderQueriesParser.AggregationFunctionContext context ) { }
        /// <summary>
        /// Enter a parse tree produced by <see cref="QueryBuilderQueriesParser.where"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public virtual void EnterWhere( [NotNull] QueryBuilderQueriesParser.WhereContext context ) { }
        /// <summary>
        /// Exit a parse tree produced by <see cref="QueryBuilderQueriesParser.where"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public virtual void ExitWhere( [NotNull] QueryBuilderQueriesParser.WhereContext context ) { }
        /// <summary>
        /// Enter a parse tree produced by <see cref="QueryBuilderQueriesParser.expressionList"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public virtual void EnterExpressionList( [NotNull] QueryBuilderQueriesParser.ExpressionListContext context ) { }
        /// <summary>
        /// Exit a parse tree produced by <see cref="QueryBuilderQueriesParser.expressionList"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public virtual void ExitExpressionList( [NotNull] QueryBuilderQueriesParser.ExpressionListContext context ) { }
        /// <summary>
        /// Enter a parse tree produced by <see cref="QueryBuilderQueriesParser.arithmeticExpression"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public virtual void EnterArithmeticExpression( [NotNull] QueryBuilderQueriesParser.ArithmeticExpressionContext context ) { }
        /// <summary>
        /// Exit a parse tree produced by <see cref="QueryBuilderQueriesParser.arithmeticExpression"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public virtual void ExitArithmeticExpression( [NotNull] QueryBuilderQueriesParser.ArithmeticExpressionContext context ) { }
        /// <summary>
        /// Enter a parse tree produced by <see cref="QueryBuilderQueriesParser.otherExpression"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public virtual void EnterOtherExpression( [NotNull] QueryBuilderQueriesParser.OtherExpressionContext context ) { }
        /// <summary>
        /// Exit a parse tree produced by <see cref="QueryBuilderQueriesParser.otherExpression"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public virtual void ExitOtherExpression( [NotNull] QueryBuilderQueriesParser.OtherExpressionContext context ) { }
        /// <summary>
        /// Enter a parse tree produced by <see cref="QueryBuilderQueriesParser.logicalExpression"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public virtual void EnterLogicalExpression( [NotNull] QueryBuilderQueriesParser.LogicalExpressionContext context ) { }
        /// <summary>
        /// Exit a parse tree produced by <see cref="QueryBuilderQueriesParser.logicalExpression"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public virtual void ExitLogicalExpression( [NotNull] QueryBuilderQueriesParser.LogicalExpressionContext context ) { }
        /// <summary>
        /// Enter a parse tree produced by <see cref="QueryBuilderQueriesParser.groupby"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public virtual void EnterGroupby( [NotNull] QueryBuilderQueriesParser.GroupbyContext context ) { }
        /// <summary>
        /// Exit a parse tree produced by <see cref="QueryBuilderQueriesParser.groupby"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public virtual void ExitGroupby( [NotNull] QueryBuilderQueriesParser.GroupbyContext context ) { }
        /// <summary>
        /// Enter a parse tree produced by <see cref="QueryBuilderQueriesParser.having"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public virtual void EnterHaving( [NotNull] QueryBuilderQueriesParser.HavingContext context ) { }
        /// <summary>
        /// Exit a parse tree produced by <see cref="QueryBuilderQueriesParser.having"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public virtual void ExitHaving( [NotNull] QueryBuilderQueriesParser.HavingContext context ) { }
        /// <summary>
        /// Enter a parse tree produced by <see cref="QueryBuilderQueriesParser.orderby"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public virtual void EnterOrderby( [NotNull] QueryBuilderQueriesParser.OrderbyContext context ) { }
        /// <summary>
        /// Exit a parse tree produced by <see cref="QueryBuilderQueriesParser.orderby"/>.
        /// <para>The default implementation does nothing.</para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        public virtual void ExitOrderby( [NotNull] QueryBuilderQueriesParser.OrderbyContext context ) { }

        /// <inheritdoc/>
        /// <remarks>The default implementation does nothing.</remarks>
        public virtual void EnterEveryRule( [NotNull] ParserRuleContext context ) { }
        /// <inheritdoc/>
        /// <remarks>The default implementation does nothing.</remarks>
        public virtual void ExitEveryRule( [NotNull] ParserRuleContext context ) { }
        /// <inheritdoc/>
        /// <remarks>The default implementation does nothing.</remarks>
        public virtual void VisitTerminal( [NotNull] ITerminalNode node ) { }
        /// <inheritdoc/>
        /// <remarks>The default implementation does nothing.</remarks>
        public virtual void VisitErrorNode( [NotNull] IErrorNode node ) { }
    }
} // namespace QueryBuilder.Parser