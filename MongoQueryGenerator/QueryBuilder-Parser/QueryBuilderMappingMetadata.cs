using System.Collections.Generic;
using QueryBuilder.ER;
using QueryBuilder.Map;
using QueryBuilder.Mongo;

namespace QueryBuilder.Parser
{
    public class QueryBuilderMappingMetadata
    {
        public ERModel EntityRelationshipModel { get; set; }
        public MongoSchema MongoDBSchema { get; set; }
        public ModelMapping ERMongoMapping { get; set; }
        public List<string> Warnings { get; set; }
        public List<string> Errors { get; set; }

        public QueryBuilderMappingMetadata( ERModel EntityRelationshipModel,
                                        MongoSchema MongoDBSchema,
                                        ModelMapping ERMongoMapping,
                                        List<string> Warnings,
                                        List<string> Errors )
        {
            this.EntityRelationshipModel = EntityRelationshipModel;
            this.MongoDBSchema = MongoDBSchema;
            this.ERMongoMapping = ERMongoMapping;
            this.Warnings = Warnings;
            this.Errors = Errors;
        }
    }
}