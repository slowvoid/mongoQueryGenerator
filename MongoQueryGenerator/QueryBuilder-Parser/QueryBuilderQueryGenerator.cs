using System;
using System.Collections.Generic;
using Antlr4.Runtime.Misc;
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

        public QueryBuilderQueryGenerator(QueryBuilderMappingMetadata metadata)
        {
            StartArg = null;
            PipelineOperators = new List<AlgebraOperator>();
            this.metadata = metadata;
        }

        public override void ExitQuery([NotNull] QueryBuilderQueriesParser.QueryContext context)
        {
            QueryableEntity qEntity = context.entity().qEntity;

            StartArg = getStartArg(qEntity);

            List<RelationshipJoinOperator> rjoinOps = getRelationshipJoinOperators(qEntity);
            rjoinOps.ForEach(rjo => PipelineOperators.Add(rjo));
        }

        private FromArgument getStartArg(QueryableEntity qEntity)
        {
            if (qEntity.Element.GetType() == typeof(Entity))
            {
                return new FromArgument(qEntity, metadata.ERMongoMapping);
            }
            else if (qEntity.Element.GetType() == typeof(ComputedEntity))
            {
                ComputedEntity ce = qEntity.Element as ComputedEntity;
                return getStartArg(ce.SourceEntity);
            }
            else
            {
                throw new InvalidOperationException("There should not be another element type here!");
            }
        }

        private List<RelationshipJoinOperator> getRelationshipJoinOperators(QueryableEntity qEntity)
        {
            var ret = new List<RelationshipJoinOperator>();

            if (qEntity.Element.GetType() == typeof(ComputedEntity))
            {
                ComputedEntity ce = qEntity.Element as ComputedEntity;
                ret.AddRange(getRelationshipJoinOperators(ce.SourceEntity));
                ret.Add(new RelationshipJoinOperator(getFirstEntityOnly(ce.SourceEntity),
                                                                ce.Relationship,
                                                                ce.RelationshipAlias,
                                                                ce.TargetEntities,
                                                                metadata.ERMongoMapping));
            }

            return ret;
        }

        private QueryableEntity getFirstEntityOnly(QueryableEntity qEntity)
        {
            if (qEntity.Element.GetType() == typeof(Entity))
            {
                return qEntity;
            }
            else if (qEntity.Element.GetType() == typeof(ComputedEntity))
            {
                ComputedEntity ce = qEntity.Element as ComputedEntity;
                return getFirstEntityOnly(ce.SourceEntity);
            }
            else {
                throw new InvalidOperationException("There should not be another element type here!");
            }
        }

        override public void ExitSimpleEntity(QueryBuilderQueriesParser.SimpleEntityContext context)
        {
            // Simple Entity
            string simpleEntityAlias = null;
            if (context.simpleEntityAlias != null)
            {
                simpleEntityAlias = context.simpleEntityAlias.Text;
            }
            var qEntity = new QueryableEntity(metadata.EntityRelationshipModel.FindByName(context.simpleEntityName.Text), simpleEntityAlias);
            if (qEntity.Element.GetType() != typeof(Entity))
            {
                throw new Exception($"Element {qEntity.Element.Name} is not an Entity!");
            }
            context.qEntity = qEntity;
            // if ( StartArg == null )
            // {
            //     StartArg = new FromArgument( context.qEntity, metadata.ERMongoMapping );
            // }
        }

        override public void ExitComputedEntity(QueryBuilderQueriesParser.ComputedEntityContext context)
        {
            // Computed Entity
            var computedEntityRight = new List<QueryBuilderQueriesParser.EntityContext>(context._computedEntityRight);
            var qRelationship = metadata.EntityRelationshipModel.FindByName(context.computedEntityRelationshipName.Text);
            if (qRelationship.GetType() != typeof(Relationship))
            {
                throw new Exception($"Element {qRelationship.Name} is not a Relationship!");
            }
            var cEntityName = context.computedEntityLeft.qEntity.Element.Name
                + string.Join("", computedEntityRight.ConvertAll(cer => cer.qEntity.Element.Name));
            var cEntityAlias = context.computedEntityLeft.qEntity.Alias
                + string.Join("", computedEntityRight.ConvertAll(cer => cer.qEntity.Alias));
            string computedEntityRelationshipAlias = null;
            if (context.computedEntityRelationshipAlias != null)
            {
                computedEntityRelationshipAlias = context.computedEntityRelationshipAlias.Text;
            }

            ComputedEntity cEntity = new ComputedEntity(cEntityName,
                                                        context.computedEntityLeft.qEntity,
                                                        (Relationship)qRelationship,
                                                        computedEntityRelationshipAlias,
                                                        computedEntityRight.ConvertAll(cer => cer.qEntity));
            context.qEntity = new QueryableEntity(cEntity, cEntityAlias);

            // relationshipJoinOperator = new RelationshipJoinOperator( cEntity.SourceEntity,
            //                                                     cEntity.Relationship,
            //                                                     cEntity.RelationshipAlias,
            //                                                     cEntity.TargetEntities,
            //                                                     metadata.ERMongoMapping );
            // if ( StartArg == null )
            // {
            //     StartArg = new FromArgument( context.qEntity, metadata.ERMongoMapping );
            // }
        }


        override public void ExitParenthesisEntity(QueryBuilderQueriesParser.ParenthesisEntityContext context)
        {
            context.qEntity = context.entity().qEntity;
            // if ( StartArg == null )
            // {
            //     StartArg = new FromArgument( context.qEntity, metadata.ERMongoMapping );
            // }
        }
    }
}