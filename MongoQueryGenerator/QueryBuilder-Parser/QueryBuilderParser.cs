using System;
using System.IO;
using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using QueryBuilder.Query;

namespace QueryBuilder.Parser
{
    public class QueryBuilderParser
    {
        public static QueryBuilderMappingMetadata ParseMapping( Stream stream )
        {
            ICharStream mappingStream = CharStreams.fromStream( stream );
            ITokenSource mappingLexer = new QueryBuilderMappingLexer( mappingStream );
            ITokenStream mappingTokens = new CommonTokenStream( mappingLexer );
            QueryBuilderMappingParser mappingParser = new QueryBuilderMappingParser( mappingTokens );
            mappingParser.BuildParseTree = true;

            IParseTree mappingTree = mappingParser.program();

            var mappingAnalyzer = new QueryBuilderMappingAnalyzer();
            mappingAnalyzer.Visit( mappingTree );

            return new QueryBuilderMappingMetadata( mappingAnalyzer.EntityRelationshipModel,
                                                mappingAnalyzer.MongoDBSchema,
                                                mappingAnalyzer.ERMongoMapping,
                                                mappingAnalyzer.Warnings,
                                                mappingAnalyzer.Errors );
        }

        public static QueryGenerator ParseQuery( string query, QueryBuilderMappingMetadata metadata )
        {
            ICharStream queryStream = CharStreams.fromstring( query );
            ITokenSource queryLexer = new QueryBuilderQueriesLexer( queryStream );
            ITokenStream queryTokens = new CommonTokenStream( queryLexer );
            QueryBuilderQueriesParser queryParser = new QueryBuilderQueriesParser( queryTokens );
            queryParser.BuildParseTree = true;

            IParseTree tree = queryParser.query();

            QueryBuilderQueryGenerator generator = new QueryBuilderQueryGenerator( metadata );
            ParseTreeWalker.Default.Walk( generator, tree );

            return new QueryGenerator( generator.StartArg, generator.PipelineOperators );
        }
    }
}