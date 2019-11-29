using System;
using System.Collections.Generic;
using QueryBuilder.ER;
using QueryBuilder.ER.Exceptions;
using QueryBuilder.Operation.Exceptions;
using QueryBuilder.Map;
using QueryBuilder.Mongo;

namespace QueryBuilder.Parser
{

    // The "bool" return is never used. All methods always return true.
    // This is because ANTLR has a mandatory generic type for the return methods
    // and C# does not allow "Void" as a generic type parameter
    public class QueryBuilderMappingAnalyzer : QueryBuilderMappingBaseVisitor<bool>
    {
        public ERModel EntityRelationshipModel { get; set; }
        public MongoSchema MongoDBSchema { get; set; }
        public ModelMapping ERMongoMapping { get; set; }
        public List<string> Warnings { get; set; }
        public List<string> Errors { get; set; }

        private int Pass;

        private string StripQuotes(string s)
        {
            return s.Substring(1, s.Length - 2);
        }

        private void RunAdditionalVerifications()
        {
            foreach(var e in EntityRelationshipModel.Elements)
            {
                int numMappings = 0;
                int numMainMappings = 0;
                foreach(var r in ERMongoMapping.Rules)
                {
                    if (r.Source == e)
                    {
                        numMappings++;
                        if (r.IsMain)
                        {
                            numMainMappings++;
                        }
                    }

                };
                if(numMappings == 0) {
                    Warnings.Add($"Warning: element {e.Name} has no mapped document");
                }
                if(numMainMappings == 0) {
                    Warnings.Add($"Warning: element {e.Name} has no mapped main document");
                }
                if(numMainMappings > 1) {
                    Warnings.Add($"Warning: element {e.Name} has more than one main document");
                }
            };

        }

        override public bool Visit(Antlr4.Runtime.Tree.IParseTree tree)
        {
            Warnings = new List<string>();
            Errors = new List<string>();
            Pass = 1;
            base.Visit(tree);
            Pass = 2;
            base.Visit(tree);
            Pass = 3;
            base.Visit(tree);
            RunAdditionalVerifications();
            return true;
        }

        override public bool VisitProgram(QueryBuilderMappingParser.ProgramContext context)
        {
            if (Pass == 1)
            {
                EntityRelationshipModel = new ERModel(StripQuotes(context.name.Text) + "ERModel", new List<BaseERElement>());
                MongoDBSchema = new MongoSchema(StripQuotes(context.name.Text) + "MongoSchema", new List<MongoDBCollection>());
                ERMongoMapping = new ModelMapping(StripQuotes(context.name.Text) + "Map", new List<MapRule>());
            }
            base.VisitProgram(context);
            return true;
        }

        override public bool VisitEntity(QueryBuilderMappingParser.EntityContext context)
        {
            if (Pass == 1)
            {
                Entity entity = new Entity(context.name.Text);
                foreach(QueryBuilderMappingParser.AttributeContext ac in context.attribute()) {
                    entity.AddAttribute(ac.name.Text, ac.type.Text, ac.mutivalued != null);
                }
                EntityRelationshipModel.Elements.Add(entity);
            }
            return true;
        }

        override public bool VisitRelationship(QueryBuilderMappingParser.RelationshipContext context)
        {
            if (Pass == 2)
            {
                // RelationshipCardinality deveria fazer parte de RelationshipConnection
                Relationship relationship = new Relationship(context.name.Text);

                foreach(QueryBuilderMappingParser.AttributeContext ac in context.attribute()) {
                    relationship.AddAttribute(ac.name.Text, ac.type.Text, ac.mutivalued != null);
                }


                foreach (var end in context.relationshipEnd())
                {
                    RelationshipEnd rend = new RelationshipEnd();
                    try
                    {
                        var target = EntityRelationshipModel.FindByName(end.name.Text);
                        if (target.GetType() != typeof(Entity))
                        {
                            Errors.Add($"Error (line {end.name.Line}:{end.name.Column}): relationship end '{end.name.Text}' is not an Entity!");
                        }
                        else
                        {
                            rend.TargetEntity = (Entity)target;
                            rend.Cardinality = RelationshipCardinality.One;
                            if (end.cardinality.Text.Equals("N"))
                            {
                                rend.Cardinality = RelationshipCardinality.Many;
                            }

                            relationship.AddRelationshipEnd(rend);
                        }
                    }
                    catch (Exception)
                    {
                        Errors.Add($"Error (line {end.name.Line}:{end.name.Column}): relationship end '{end.name.Text}' not found!");
                    }

                }

                EntityRelationshipModel.Elements.Add(relationship);
            }
            return true;
        }

        override public bool VisitCollection(QueryBuilderMappingParser.CollectionContext context)
        {
            if (Pass == 3)
            {
                MongoDBCollection collection = new MongoDBCollection(context.name.Text);

                foreach(QueryBuilderMappingParser.FieldContext fc in context.field()) {
                    collection.AddAttribute(fc.name.Text, fc.type.Text, fc.mutivalued != null);
                }

                MongoDBSchema.Collections.Add(collection);

                if (context.erRefs() != null)
                {
                    foreach(var er in context.erRefs().erRef())
                    {
                        try
                        {
                            var erElement = EntityRelationshipModel.FindByName(er.refName.Text);
                            var isMain = er.main == null ? false : true;
                            MapRule mapRule = new MapRule(erElement, collection, isMain);

                            ERMongoMapping.Rules.Add(mapRule);
                        }
                        catch (Exception)
                        {
                            Errors.Add($"Error (line {er.refName.Line}:{er.refName.Column}): referenced ER element '{er.refName.Text}' not found!");
                        }
                    };
                }

                foreach(var f in context.field())
                {
                    if(f.erAttributeRef() != null)
                    {
                        try
                        {
                            var element = EntityRelationshipModel.FindByName(f.erAttributeRef().refName.Text);
                            var rule = ERMongoMapping.FindRule(element, collection);
                            rule.AddRule(f.erAttributeRef().attributeName.Text, f.name.Text);
                        }
                        catch (ElementNotFoundException)
                        {
                            Errors.Add($"Error (line {f.erAttributeRef().refName.Line}:{f.erAttributeRef().refName.Column}): referenced ER element '{f.erAttributeRef().refName.Text}' not found!");
                        }
                    }

                }
            }
            return true;
        }
    }
}
