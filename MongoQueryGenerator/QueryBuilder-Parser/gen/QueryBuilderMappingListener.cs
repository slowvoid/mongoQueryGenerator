//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     ANTLR Version: 4.7.1
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// Generated from /Users/daniellucredio/GitProjects/mongoQueryGenerator/MongoQueryGeneratorFork/QueryBuilder-Parser/QueryBuilderMapping.g4 by ANTLR 4.7.1

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
    using IParseTreeListener = Antlr4.Runtime.Tree.IParseTreeListener;
    using IToken = Antlr4.Runtime.IToken;

    /// <summary>
    /// This interface defines a complete listener for a parse tree produced by
    /// <see cref="QueryBuilderMappingParser"/>.
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCode( "ANTLR", "4.7.1" )]
    [System.CLSCompliant( false )]
    public interface IQueryBuilderMappingListener : IParseTreeListener
    {
        /// <summary>
        /// Enter a parse tree produced by <see cref="QueryBuilderMappingParser.program"/>.
        /// </summary>
        /// <param name="context">The parse tree.</param>
        void EnterProgram( [NotNull] QueryBuilderMappingParser.ProgramContext context );
        /// <summary>
        /// Exit a parse tree produced by <see cref="QueryBuilderMappingParser.program"/>.
        /// </summary>
        /// <param name="context">The parse tree.</param>
        void ExitProgram( [NotNull] QueryBuilderMappingParser.ProgramContext context );
        /// <summary>
        /// Enter a parse tree produced by <see cref="QueryBuilderMappingParser.ermodel"/>.
        /// </summary>
        /// <param name="context">The parse tree.</param>
        void EnterErmodel( [NotNull] QueryBuilderMappingParser.ErmodelContext context );
        /// <summary>
        /// Exit a parse tree produced by <see cref="QueryBuilderMappingParser.ermodel"/>.
        /// </summary>
        /// <param name="context">The parse tree.</param>
        void ExitErmodel( [NotNull] QueryBuilderMappingParser.ErmodelContext context );
        /// <summary>
        /// Enter a parse tree produced by <see cref="QueryBuilderMappingParser.erelement"/>.
        /// </summary>
        /// <param name="context">The parse tree.</param>
        void EnterErelement( [NotNull] QueryBuilderMappingParser.ErelementContext context );
        /// <summary>
        /// Exit a parse tree produced by <see cref="QueryBuilderMappingParser.erelement"/>.
        /// </summary>
        /// <param name="context">The parse tree.</param>
        void ExitErelement( [NotNull] QueryBuilderMappingParser.ErelementContext context );
        /// <summary>
        /// Enter a parse tree produced by <see cref="QueryBuilderMappingParser.entity"/>.
        /// </summary>
        /// <param name="context">The parse tree.</param>
        void EnterEntity( [NotNull] QueryBuilderMappingParser.EntityContext context );
        /// <summary>
        /// Exit a parse tree produced by <see cref="QueryBuilderMappingParser.entity"/>.
        /// </summary>
        /// <param name="context">The parse tree.</param>
        void ExitEntity( [NotNull] QueryBuilderMappingParser.EntityContext context );
        /// <summary>
        /// Enter a parse tree produced by <see cref="QueryBuilderMappingParser.relationship"/>.
        /// </summary>
        /// <param name="context">The parse tree.</param>
        void EnterRelationship( [NotNull] QueryBuilderMappingParser.RelationshipContext context );
        /// <summary>
        /// Exit a parse tree produced by <see cref="QueryBuilderMappingParser.relationship"/>.
        /// </summary>
        /// <param name="context">The parse tree.</param>
        void ExitRelationship( [NotNull] QueryBuilderMappingParser.RelationshipContext context );
        /// <summary>
        /// Enter a parse tree produced by <see cref="QueryBuilderMappingParser.relationshipEnd"/>.
        /// </summary>
        /// <param name="context">The parse tree.</param>
        void EnterRelationshipEnd( [NotNull] QueryBuilderMappingParser.RelationshipEndContext context );
        /// <summary>
        /// Exit a parse tree produced by <see cref="QueryBuilderMappingParser.relationshipEnd"/>.
        /// </summary>
        /// <param name="context">The parse tree.</param>
        void ExitRelationshipEnd( [NotNull] QueryBuilderMappingParser.RelationshipEndContext context );
        /// <summary>
        /// Enter a parse tree produced by <see cref="QueryBuilderMappingParser.attribute"/>.
        /// </summary>
        /// <param name="context">The parse tree.</param>
        void EnterAttribute( [NotNull] QueryBuilderMappingParser.AttributeContext context );
        /// <summary>
        /// Exit a parse tree produced by <see cref="QueryBuilderMappingParser.attribute"/>.
        /// </summary>
        /// <param name="context">The parse tree.</param>
        void ExitAttribute( [NotNull] QueryBuilderMappingParser.AttributeContext context );
        /// <summary>
        /// Enter a parse tree produced by <see cref="QueryBuilderMappingParser.mongoschema"/>.
        /// </summary>
        /// <param name="context">The parse tree.</param>
        void EnterMongoschema( [NotNull] QueryBuilderMappingParser.MongoschemaContext context );
        /// <summary>
        /// Exit a parse tree produced by <see cref="QueryBuilderMappingParser.mongoschema"/>.
        /// </summary>
        /// <param name="context">The parse tree.</param>
        void ExitMongoschema( [NotNull] QueryBuilderMappingParser.MongoschemaContext context );
        /// <summary>
        /// Enter a parse tree produced by <see cref="QueryBuilderMappingParser.collection"/>.
        /// </summary>
        /// <param name="context">The parse tree.</param>
        void EnterCollection( [NotNull] QueryBuilderMappingParser.CollectionContext context );
        /// <summary>
        /// Exit a parse tree produced by <see cref="QueryBuilderMappingParser.collection"/>.
        /// </summary>
        /// <param name="context">The parse tree.</param>
        void ExitCollection( [NotNull] QueryBuilderMappingParser.CollectionContext context );
        /// <summary>
        /// Enter a parse tree produced by <see cref="QueryBuilderMappingParser.erRefs"/>.
        /// </summary>
        /// <param name="context">The parse tree.</param>
        void EnterErRefs( [NotNull] QueryBuilderMappingParser.ErRefsContext context );
        /// <summary>
        /// Exit a parse tree produced by <see cref="QueryBuilderMappingParser.erRefs"/>.
        /// </summary>
        /// <param name="context">The parse tree.</param>
        void ExitErRefs( [NotNull] QueryBuilderMappingParser.ErRefsContext context );
        /// <summary>
        /// Enter a parse tree produced by <see cref="QueryBuilderMappingParser.erRef"/>.
        /// </summary>
        /// <param name="context">The parse tree.</param>
        void EnterErRef( [NotNull] QueryBuilderMappingParser.ErRefContext context );
        /// <summary>
        /// Exit a parse tree produced by <see cref="QueryBuilderMappingParser.erRef"/>.
        /// </summary>
        /// <param name="context">The parse tree.</param>
        void ExitErRef( [NotNull] QueryBuilderMappingParser.ErRefContext context );
        /// <summary>
        /// Enter a parse tree produced by <see cref="QueryBuilderMappingParser.field"/>.
        /// </summary>
        /// <param name="context">The parse tree.</param>
        void EnterField( [NotNull] QueryBuilderMappingParser.FieldContext context );
        /// <summary>
        /// Exit a parse tree produced by <see cref="QueryBuilderMappingParser.field"/>.
        /// </summary>
        /// <param name="context">The parse tree.</param>
        void ExitField( [NotNull] QueryBuilderMappingParser.FieldContext context );
        /// <summary>
        /// Enter a parse tree produced by <see cref="QueryBuilderMappingParser.erAttributeRef"/>.
        /// </summary>
        /// <param name="context">The parse tree.</param>
        void EnterErAttributeRef( [NotNull] QueryBuilderMappingParser.ErAttributeRefContext context );
        /// <summary>
        /// Exit a parse tree produced by <see cref="QueryBuilderMappingParser.erAttributeRef"/>.
        /// </summary>
        /// <param name="context">The parse tree.</param>
        void ExitErAttributeRef( [NotNull] QueryBuilderMappingParser.ErAttributeRefContext context );
    }
} // namespace QueryBuilder.Parser