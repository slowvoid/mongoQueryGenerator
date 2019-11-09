using System;
using System.Collections.Generic;
using QueryBuilder.ER;
using QueryBuilder.Map;
using QueryBuilder.Mongo;

namespace QueryBuilder.Parser
{

    public class QueryBuilderMappingAnalyzer : QueryBuilderMappingBaseVisitor<string>
    {
        #region Properties
        /// <summary>
        /// The ER Model
        /// </summary>
        public ERModel EntityRelationshipModel { get; set; }
        /// <summary>
        /// MongoDB Schema
        /// </summary>
        public MongoSchema MongoDBSchema { get; set; }
        /// <summary>
        /// Mapping between ER and MongoDB
        /// </summary>
        public ModelMapping ERMongoMapping { get; set; }
        #endregion        
        private string StripQuotes(string s)
        {
            return s.Substring(1, s.Length - 2);
        }
        override public string VisitProgram(QueryBuilderMappingParser.ProgramContext context)
        {
            EntityRelationshipModel = new ERModel(StripQuotes(context.name.Text) + "ERModel", new List<BaseERElement>());
            return base.VisitProgram(context);
        }

        override public string VisitEntity(QueryBuilderMappingParser.EntityContext context)
        {
            Entity entity = new Entity(context.name.Text);
            if (context.attributes() != null)
                entity.AddAttributes(Array.ConvertAll(context.attributes().attribute(), a => a.name.Text));
            EntityRelationshipModel.Elements.Add(entity);
            return null;
        }

        override public string VisitRelationship(QueryBuilderMappingParser.RelationshipContext context)
        {
            // RelationshipCardinality deveria fazer parte de RelationshipConnection
            Relationship relationship = new Relationship(context.name.Text);

            if (context.attributes() != null)
                relationship.AddAttributes(Array.ConvertAll(context.attributes().attribute(), a => a.name.Text));

            foreach (var end in context.relationshipEnd())
            {
                RelationshipEnd rend = new RelationshipEnd();
                try
                {
                    var target = EntityRelationshipModel.FindByName(end.name.Text);
                    if (target.GetType() != typeof(Entity))
                    {
                        Console.WriteLine($"Error (line {end.name.Line}:{end.name.Column}): relationship end '{end.name.Text}' is not an Entity!");
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
                    Console.WriteLine($"Error (line {end.name.Line}:{end.name.Column}): relationship end '{end.name.Text}' not found!");

                }

            }

            EntityRelationshipModel.Elements.Add(relationship);
            return null;
        }
    }
}
