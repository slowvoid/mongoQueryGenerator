using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using QueryBuilder.Query;

namespace QueryBuilder.Parser
{
    public class QueryBuilderParser
    {
        public static QueryBuilderParserResult ParseMapping(string file)
        {
            ICharStream mappingStream = CharStreams.fromPath(file);
            ITokenSource mappingLexer = new QueryBuilderMappingLexer(mappingStream);
            ITokenStream mappingTokens = new CommonTokenStream(mappingLexer);
            QueryBuilderMappingParser mappingParser = new QueryBuilderMappingParser(mappingTokens);
            mappingParser.BuildParseTree = true;

            IParseTree mappingTree = mappingParser.program();

            var mappingAnalyzer = new QueryBuilderMappingAnalyzer();
            mappingAnalyzer.Visit(mappingTree);

            return new QueryBuilderParserResult(mappingAnalyzer.EntityRelationshipModel,
                                                mappingAnalyzer.MongoDBSchema,
                                                mappingAnalyzer.ERMongoMapping,
                                                mappingAnalyzer.Warnings,
                                                mappingAnalyzer.Errors);
        }

        public static QueryGenerator ParseQuery(string query)
        {
            ICharStream queryStream = CharStreams.fromstring(query);
            ITokenSource queryLexer = new QueryBuilderQueriesLexer(queryStream);
            ITokenStream queryTokens = new CommonTokenStream(queryLexer);
            QueryBuilderQueriesParser queryParser = new QueryBuilderQueriesParser(queryTokens);
            queryParser.BuildParseTree = true;

            IParseTree tree = queryParser.query();

            // TODO: visit the query tree to assemble the QueryGenerator object

            return new QueryGenerator(null, null);
        }
    }
}