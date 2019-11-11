namespace QueryBuilder.Parser{
    // The "bool" return is never used. All methods always return true.
    // This is because ANTLR has a mandatory generic type for the return methods
    // and C# does not allow "Void" as a generic type parameter
    public class QueryBuilderQueryGenerator : QueryBuilderMappingBaseVisitor<bool>
    {
        
    }
}