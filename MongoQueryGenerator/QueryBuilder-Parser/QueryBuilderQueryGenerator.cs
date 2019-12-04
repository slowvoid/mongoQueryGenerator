using System;
using System.Collections.Generic;
using QueryBuilder.ER;
using QueryBuilder.Operation;
using QueryBuilder.Operation.Arguments;

namespace QueryBuilder.Parser
{
    // The "bool" return is never used. All methods always return true.
    // This is because ANTLR has a mandatory generic type for the return methods
    // and C# does not allow "Void" as a generic type parameter
    public class QueryBuilderQueryGenerator : QueryBuilderQueriesBaseListener
    {
        public FromArgument StartArg { set; get; }

        public List<AlgebraOperator> PipelineOperators { set; get; }

        private QueryBuilderMappingMetadata metadata;

        public QueryBuilderQueryGenerator( QueryBuilderMappingMetadata metadata )
        {
            StartArg = null;
            PipelineOperators = new List<AlgebraOperator>();
            this.metadata = metadata;
        }

        override public void ExitSimpleEntity( QueryBuilderQueriesParser.SimpleEntityContext context )
        {
            // Simple Entity
            var qEntity = new QueryableEntity( metadata.EntityRelationshipModel.FindByName( context.simpleEntityName.Text ), context.simpleEntityAlias.Text );
            if ( qEntity.Element.GetType() != typeof( Entity ) )
            {
                throw new Exception( $"Element {qEntity.Element.Name} is not an Entity!" );
            }
            context.qEntity = qEntity;
            if ( StartArg == null )
            {
                StartArg = new FromArgument( context.qEntity, metadata.ERMongoMapping );
            }
        }

        override public void ExitComputedEntity( QueryBuilderQueriesParser.ComputedEntityContext context )
        {
            // Computed Entity
            var computedEntityRight = new List<QueryBuilderQueriesParser.EntityContext>( context._computedEntityRight );
            var qRelationship = metadata.EntityRelationshipModel.FindByName( context.computedEntityRelationshipName.Text );
            if ( qRelationship.GetType() != typeof( Relationship ) )
            {
                throw new Exception( $"Element {qRelationship.Name} is not a Relationship!" );
            }
            var cEntityName = context.computedEntityLeft.qEntity.Element.Name
                + string.Join( "", computedEntityRight.ConvertAll( cer => cer.qEntity.Element.Name ) );
            var cEntityAlias = context.computedEntityLeft.qEntity.Alias
                + string.Join( "", computedEntityRight.ConvertAll( cer => cer.qEntity.Alias ) );
            ComputedEntity cEntity = new ComputedEntity( cEntityName,
                                                        context.computedEntityLeft.qEntity,
                                                        (Relationship)qRelationship,
                                                        context.computedEntityRelationshipAlias.Text,
                                                        computedEntityRight.ConvertAll( cer => cer.qEntity ) );
            context.qEntity = new QueryableEntity( cEntity, cEntityAlias );

            PipelineOperators.Add( new RelationshipJoinOperator( cEntity.SourceEntity,
                                                                cEntity.Relationship,
                                                                cEntity.RelationshipAlias,
                                                                cEntity.TargetEntities,
                                                                metadata.ERMongoMapping ) );
            if ( StartArg == null )
            {
                StartArg = new FromArgument( context.qEntity, metadata.ERMongoMapping );
            }
        }


        override public void ExitParenthesisEntity( QueryBuilderQueriesParser.ParenthesisEntityContext context )
        {
            context.qEntity = context.entity().qEntity;
            if ( StartArg == null )
            {
                StartArg = new FromArgument( context.qEntity, metadata.ERMongoMapping );
            }
        }
    }
}