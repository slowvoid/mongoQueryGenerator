using System;
using System.Collections.Generic;
using QueryBuilder.ER;
using QueryBuilder.ER.Exceptions;
using QueryBuilder.Operation.Exceptions;
using QueryBuilder.Map;
using QueryBuilder.Mongo;
using QueryBuilder.Shared;
using static QueryBuilder.Parser.QueryBuilderMappingParser;

namespace QueryBuilder.Parser
{

    // The "bool" return is never used. All methods always return true.
    // This is because ANTLR has a mandatory generic type for the return methods
    // and C# does not allow "Void" as a generic type parameter
    public class QueryBuilderMappingAnalyzer : QueryBuilderMappingBaseVisitor<bool>
    {
        const string MULTIVALUED_PREFIX = "_multivalued_";

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
            foreach (var e in EntityRelationshipModel.Elements)
            {
                int numMappings = 0;
                int numMainMappings = 0;
                foreach (var r in ERMongoMapping.Rules)
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
                if (numMappings == 0)
                {
                    Warnings.Add($"Warning: element {e.Name} has no mapped document");
                }
                if (numMainMappings == 0)
                {
                    Warnings.Add($"Warning: element {e.Name} has no mapped main document");
                }
                if (numMainMappings > 1)
                {
                    Warnings.Add($"Warning: element {e.Name} has more than one main document");
                }

                if (e is Relationship)
                {
                    Relationship relationship = e as Relationship;
                    try
                    {
                        MongoDBCollection relationshipMainCollection = ERMongoMapping.FindMainCollection(relationship);
                        List<MapRule> rules = ERMongoMapping.FindRules(relationshipMainCollection);

                        foreach (RelationshipEnd re in relationship.Ends)
                        {
                            DataAttribute da = re.TargetEntity.GetIdentifierAttribute();
                            bool hasMapping = false;
                            Console.WriteLine($"Searching r {relationship.Name} / {relationshipMainCollection.Name} / re {re.TargetEntity.Name}.id...");
                            foreach (MapRule mr in rules)
                            {
                                Console.WriteLine($"  Rule: {mr.Source.Name} - {mr.Target.Name}");
                                foreach (var i in mr.Rules)
                                {
                                    Console.WriteLine($"  Subrule: {i.Key} - {i.Value}");
                                }

                                if (mr.Source == re.TargetEntity && mr.Rules.ContainsKey(da.Name))
                                {
                                    hasMapping = true;
                                }
                            }
                            Console.WriteLine($"Has Mapping = {hasMapping}");
                            if (!hasMapping)
                            {
                                Errors.Add($"Error 001: the main collection {relationshipMainCollection.Name} for " +
                                            $"relationship {relationship.Name} does not have a mapping for " +
                                            $"identifier {da.Name} of entity {re.TargetEntity.Name}");
                            }
                        }
                    }
                    catch (RuleNotFoundException rnfe)
                    {
                        Errors.Add($"Error 002: relationship {relationship.Name} has no mapped main collection");
                    }
                }
            }
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
                foreach (QueryBuilderMappingParser.AttributeContext ac in context.attribute())
                {
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

                foreach (QueryBuilderMappingParser.AttributeContext ac in context.attribute())
                {
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
                            Errors.Add($"Error 003: (line {end.name.Line}:{end.name.Column}): relationship end '{end.name.Text}' is not an Entity!");
                        }
                        else
                        {
                            rend.TargetEntity = (Entity)target;

                            relationship.AddRelationshipEnd(rend);
                        }
                    }
                    catch (Exception)
                    {
                        Errors.Add($"Error 004: (line {end.name.Line}:{end.name.Column}): relationship end '{end.name.Text}' not found!");
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

                List<MapRule> mapRules = new List<MapRule>();
                if (context.erRefs() != null)
                {
                    foreach (var er in context.erRefs().erRef())
                    {
                        try
                        {
                            var erElement = EntityRelationshipModel.FindByName(er.refName.Text);
                            var isMain = er.main == null ? false : true;
                            MapRule mapRule = new MapRule(erElement, collection, isMain);

                            mapRules.Add(mapRule);
                        }
                        catch (Exception)
                        {
                            Errors.Add($"Error 005: (line {er.refName.Line}:{er.refName.Column}): referenced ER element '{er.refName.Text}' not found!");
                        }
                    }
                }

                foreach (var f in context.field())
                {
                    VisitField("", f, collection, mapRules);

                }

                MongoDBSchema.Collections.Add(collection);

                foreach (var mr in mapRules)
                {
                    ERMongoMapping.Rules.Add(mr);
                }

            }
            return true;
        }

        private void VisitField(string prefix, FieldContext field, MongoDBCollection collection, List<MapRule> mapRules)
        {
            var prefixedName = prefix+field.name.Text;
            if (field.fieldType().simpleType() != null)
            {
                var fieldType = field.fieldType().simpleType();
                if (fieldType.monovaluedType != null)
                {
                    collection.AddAttribute(prefixedName, fieldType.monovaluedType.Text, false);
                }
                else if (fieldType.multivaluedType != null)
                {
                    collection.AddAttribute(prefixedName, fieldType.multivaluedType.Text, true);
                }
                if (field.erAttributeRef() != null)
                {
                    var refElementName = field.erAttributeRef().refName.Text;
                    var refAttributeName = field.erAttributeRef().attributeName.Text;
                    var foundRefElementName = false;
                    foreach (MapRule mr in mapRules)
                    {
                        if (mr.Source.Name == refElementName)
                        {
                            foundRefElementName = true;
                            DataAttribute sourceAttribute = mr.Source.GetAttribute(refAttributeName);
                            DataAttribute targetAttribute = mr.Target.GetAttribute(prefixedName);
                            if (sourceAttribute == null)
                            {
                                Errors.Add($"Error 006: attribute {mr.Source.Name}.{refAttributeName} not found in ER model"); ;
                            }
                            if (targetAttribute == null)
                            {
                                Errors.Add($"Error 007: field {mr.Target.Name}.{prefixedName} not found in Mongo DB Schema"); ;
                            }
                            if (sourceAttribute != null && targetAttribute != null)
                            {
                                mr.AddRule(sourceAttribute.Name, targetAttribute.Name);
                            }
                        }
                    }
                    if (!foundRefElementName)
                    {
                        Errors.Add($"Error 008: mapped attribute {refElementName}.{refAttributeName} references entity {refElementName} which is not mapped in collection {collection.Name}");
                    }
                }
            }
            else if (field.fieldType().complexType() != null)
            {
                foreach (FieldContext f in field.fieldType().complexType()._monovaluedFields)
                {
                    VisitField(prefixedName + ".", f, collection, mapRules);
                }
                foreach (FieldContext f in field.fieldType().complexType()._multivaluedFields)
                {
                    VisitField(MULTIVALUED_PREFIX + prefixedName + ".", f, collection, mapRules);
                }
            }
        }
    }
}
