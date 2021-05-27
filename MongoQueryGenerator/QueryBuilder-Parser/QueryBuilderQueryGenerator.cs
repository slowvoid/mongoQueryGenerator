using System;
using System.Collections.Generic;
using System.Linq;
using Antlr4.Runtime.Misc;
using QueryBuilder.ER;
using QueryBuilder.Operation;
using QueryBuilder.Operation.Arguments;
using QueryBuilder.Mongo.Expressions2;

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

        private ProjectStage ProjectOp;

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

            if (ProjectOp != null)
            {
                PipelineOperators.Add(ProjectOp);
            }

            SelectStage2 SelectOp = new SelectStage2();
            SelectOp.LogicalExpression = getLogicalExpression(context.where().logicalExpression());
            PipelineOperators.Add(SelectOp);
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
            else
            {
                throw new InvalidOperationException("There should not be another element type here!");
            }
        }

        private LogicalExpression getLogicalExpression(QueryBuilderQueriesParser.LogicalExpressionContext context)
        {
            var ret = new LogicalExpression();

            context.logicalTerm().ToList().ForEach(lt => ret.LogicalTerms.Add(getLogicalTerm(lt)));
            context.logicalOperator().ToList().ForEach(lo =>
            {
                switch (lo.GetText())
                {
                    case "and":
                        ret.LogicalOperators.Add(LogicalOperator.AND);
                        break;
                    case "or":
                        ret.LogicalOperators.Add(LogicalOperator.OR);
                        break;
                }
            });

            return ret;
        }

        private LogicalTerm getLogicalTerm(QueryBuilderQueriesParser.LogicalTermContext context)
        {
            if (context.relationalOperator() != null)
            {
                var ret = new RelationalLogicalTerm();
                SimpleAttribute sa = new SimpleAttribute();
                sa.Element = metadata.EntityRelationshipModel.FindByName(context.simpleAttribute().elementName.Text);
                sa.Attribute = sa.Element.GetAttribute(context.simpleAttribute().attribute.Text);
                ret.SimpleAttribute = sa;
                ret.Value = context.value().GetText();

                switch (context.relationalOperator().GetText())
                {
                    case "=":
                        ret.RelationalOperator = RelationalOperator.EQUAL;
                        break;
                    case "<>":
                        ret.RelationalOperator = RelationalOperator.NOT_EQUAL;
                        break;
                    case ">=":
                        ret.RelationalOperator = RelationalOperator.GREATER_EQUAL;
                        break;
                    case "<=":
                        ret.RelationalOperator = RelationalOperator.LESS_EQUAL;
                        break;
                    case ">":
                        ret.RelationalOperator = RelationalOperator.GREATER;
                        break;
                    case "<":
                        ret.RelationalOperator = RelationalOperator.LESS;
                        break;
                    case "like":
                        ret.RelationalOperator = RelationalOperator.LIKE;
                        break;
                    case "is":
                        ret.RelationalOperator = RelationalOperator.IS;
                        break;
                }

                return ret;
            }
            else if (context.rangeOperator() != null)
            {
                var ret = new RangeLogicalTerm();
                SimpleAttribute sa = new SimpleAttribute();
                sa.Element = metadata.EntityRelationshipModel.FindByName(context.simpleAttribute().elementName.Text);
                sa.Attribute = sa.Element.GetAttribute(context.simpleAttribute().attribute.Text);
                ret.SimpleAttribute = sa;

                switch (context.rangeOperator().type)
                {
                    case "BETWEEN":
                        ret.RangeOperator = RangeOperator.BETWEEN;
                        context.rangeOperator().value().ToList().ForEach(v => ret.Values.Add(v.GetText()));
                        break;
                    case "NOT_IN_QUERY":
                        ret.RangeOperator = RangeOperator.NOT_IN_QUERY;
                        break;
                    case "NOT_IN_VALUES":
                        ret.RangeOperator = RangeOperator.NOT_IN_VALUES;
                        context.rangeOperator().value().ToList().ForEach(v => ret.Values.Add(v.GetText()));
                        break;
                    case "IN_QUERY":
                        ret.RangeOperator = RangeOperator.IN_QUERY;
                        break;
                    case "IN_VALUES":
                        ret.RangeOperator = RangeOperator.IN_VALUES;
                        context.rangeOperator().value().ToList().ForEach(v => ret.Values.Add(v.GetText()));
                        break;
                    case "NOT_EXISTS_QUERY":
                        ret.RangeOperator = RangeOperator.NOT_EXISTS_QUERY;
                        break;
                    case "EXISTS_QUERY":
                        ret.RangeOperator = RangeOperator.EXISTS_QUERY;
                        break;
                }
                return ret;
            }
            else
            {
                var ret = new ParenthesisLogicalTerm();
                ret.LogicalExpression = getLogicalExpression(context.logicalExpression());
                return ret;
            }
        }

        override public void ExitSimpleEntity(QueryBuilderQueriesParser.SimpleEntityContext context)
        {
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
        }

        override public void ExitComputedEntity(QueryBuilderQueriesParser.ComputedEntityContext context)
        {
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
        }


        override public void ExitParenthesisEntity(QueryBuilderQueriesParser.ParenthesisEntityContext context)
        {
            context.qEntity = context.entity().qEntity;
        }

        public override void ExitSelectAttributeOrFunction([NotNull] QueryBuilderQueriesParser.SelectAttributeOrFunctionContext context)
        {
            List<ProjectArgument> Arguments = new List<ProjectArgument>();
            foreach (var selectAttribute in context.attributeOrFunction().Where(af => af.simpleAttribute() != null))
            {
                var sa = selectAttribute.simpleAttribute();
                QueryableEntity qElement = new QueryableEntity(metadata.EntityRelationshipModel.FindByName(sa.elementName.Text));
                Arguments.Add(new ProjectArgument(qElement.GetAttribute(sa.attribute.Text), qElement, new QueryBuilder.Mongo.Expressions.BooleanExpr(true)));
            }
            ProjectOp = new ProjectStage(Arguments, metadata.ERMongoMapping);
        }
    }
}