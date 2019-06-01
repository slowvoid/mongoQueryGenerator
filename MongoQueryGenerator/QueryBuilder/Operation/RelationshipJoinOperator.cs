using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using QueryBuilder.ER;
using QueryBuilder.Map;
using QueryBuilder.Mongo;
using QueryBuilder.Mongo.Aggregation.Operators;
using QueryBuilder.Mongo.Expressions;
using QueryBuilder.Operation.Exceptions;
using QueryBuilder.Query;
using QueryBuilder.Shared;

namespace QueryBuilder.Operation
{
    /// <summary>
    /// Represents a JOIN operation
    /// </summary>
    public class RelationshipJoinOperator : AlgebraOperator
    {
        #region Properties
        /// <summary>
        /// Relationship startpoint
        /// </summary>
        public Entity SourceEntity { get; set; }
        /// <summary>
        /// Relationship endpoint
        /// </summary>
        public List<Entity> TargetEntities { get; set; }
        /// <summary>
        /// Relationship connecting both entities
        /// </summary>
        public Relationship Relationship { get; set; }
        #endregion

        #region Private Data
        
        #endregion

        #region Methods
        public override void Run( ref AlgebraOperatorResult LastResult )
        {
            // Retrieve mapping rules for Source Entity and Relationship
            MapRule SourceRule = ModelMap.Rules.First( Rule => Rule.Source.Name == SourceEntity.Name );
            MapRule RelationshipRule = ModelMap.Rules.FirstOrDefault( Rule => Rule.Source.Name == Relationship.Name );

            // Check if the relationship has attributes
            bool RelationshipHasAttributes = Relationship.Attributes.Count > 0;

            List<MongoDBOperator> OperationsToExecute = new List<MongoDBOperator>();

            string joinedAttributeName = $"data_{Relationship.Name}";

            if ( Relationship.Cardinality == RelationshipCardinality.OneToOne )
            {
                // Go through all target entities
                foreach ( Entity TargetEntity in TargetEntities )
                {
                    if ( TargetEntity is ComputedEntity )
                    {
                        // TODO
                    }
                    else
                    {
                        // Check if the target entity is really related to the source entity
                        if ( !Relationship.HasRelation( SourceEntity, TargetEntity ) )
                        {
                            throw new ImpossibleOperationException( $"Entity {TargetEntity.Name} is not reachable through {Relationship.Name}" );
                        }

                        // Check if the target entity shares the same mapping as the source
                        // Retrieve mapping rules for target entity
                        MapRule TargetRule = ModelMap.Rules.First( Rule => Rule.Source.Name == TargetEntity.Name );

                        // Get relationship data
                        RelationshipConnection RelationshipData = Relationship.GetRelation( SourceEntity, TargetEntity );

                        bool SharesMapping = SourceRule.Target.Name == TargetRule.Target.Name;

                        if ( SharesMapping )
                        {
                            // This means that target entity is embbeded in source entity
                            // So we have to add the fields to the relationship attribute
                            // In this case we just have to setup the output to match the algebra
                            Dictionary<string, string> AddTargetAttributes = new Dictionary<string, string>();
                            Dictionary<string, ProjectExpression> TargetFieldsToRemove = new Dictionary<string, ProjectExpression>();
                            // attributes in the data_RelName attribute

                            // Check if it is possible to find the root attribute for the embedded collection
                            bool FoundRootAttribute = false;
                            string RootAttributeMap = TargetRule.Rules.FirstOrDefault().Value;
                            if ( RootAttributeMap != null )
                            {
                                string[] AttributeHierarchy = RootAttributeMap.Split( new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries );
                                if ( AttributeHierarchy.Length > 0 )
                                {
                                    TargetFieldsToRemove.Add( AttributeHierarchy.First(), new BooleanExpr( false ) );
                                    FoundRootAttribute = true;
                                }
                            }

                            string JoinedObject = $"{joinedAttributeName}Obj";

                            foreach ( DataAttribute Attribute in TargetEntity.Attributes )
                            {
                                string AttributeMappedTo = TargetRule.Rules.FirstOrDefault( A => A.Key == Attribute.Name ).Value;

                                if ( AttributeMappedTo != null )
                                {
                                    AddTargetAttributes.Add( $"\"{JoinedObject}.{TargetEntity.Name}_{Attribute.Name}\"", $"${AttributeMappedTo}" );
                                    if ( !FoundRootAttribute )
                                    {
                                        TargetFieldsToRemove.Add( AttributeMappedTo, new BooleanExpr( false ) );
                                    }
                                }
                            }                            

                            AddFields AddFieldsOp = new AddFields( AddTargetAttributes );
                            Project RemoveFieldsOp = new Project( TargetFieldsToRemove );

                            // Project attributes converting the joined object into array
                            Dictionary<string, ProjectExpression> RemapAttributes = new Dictionary<string, ProjectExpression>();
                            // Add source entity attributes
                            foreach ( DataAttribute Attribute in SourceEntity.Attributes )
                            {
                                string AttributeMappedTo = SourceRule.Rules.FirstOrDefault( A => A.Key == Attribute.Name ).Value;
                                if ( AttributeMappedTo != null )
                                {
                                    RemapAttributes.Add( AttributeMappedTo, new BooleanExpr( true ) );
                                }
                            }
                            // Convert object to array
                            RemapAttributes.Add( joinedAttributeName, new ValueExpr( $"[\"${JoinedObject}\"]" ) );

                            Project RemapOp = new Project( RemapAttributes );

                            OperationsToExecute.AddRange( new MongoDBOperator[] { AddFieldsOp, RemoveFieldsOp, RemapOp } );
                        }
                        else
                        {
                            string TargetLookupAttribute = $"data_{Relationship.Name}";

                            // Lookup entity
                            LookupOperator LookupTarget = new LookupOperator
                            {
                                From = TargetRule.Target.Name,
                                ForeignField = TargetRule.Rules.First( R => R.Key == RelationshipData.TargetAttribute.Name ).Value,
                                LocalField = SourceRule.Rules.First( R => R.Key == RelationshipData.SourceAttribute.Name ).Value,
                                As = TargetLookupAttribute
                            };

                            // Rename joined data to match algebra
                            string MapInput = $"${TargetLookupAttribute}";
                            string MapAs = $"{TargetEntity.Name.ToLower()}_data";

                            Dictionary<string, string> AttributeMapRules = new Dictionary<string, string>();
                            foreach ( DataAttribute Attribute in TargetEntity.Attributes )
                            {
                                // Find Attribute mapping
                                string AttributeMappedTo = TargetRule.Rules.First( R => R.Key == Attribute.Name ).Value;
                                AttributeMapRules.Add( $"{TargetEntity.Name}_{Attribute.Name}", $"$${MapAs}.{AttributeMappedTo}" );
                            }

                            MapExpr MapOp = new MapExpr( MapInput, MapAs, AttributeMapRules );

                            Dictionary<string, ProjectExpression> ProjectAttributes = new Dictionary<string, ProjectExpression>();

                            foreach ( DataAttribute Attribute in SourceEntity.Attributes )
                            {
                                string AttributeMappedTo = SourceRule.Rules.First( R => R.Key == Attribute.Name ).Value;
                                ProjectAttributes.Add( AttributeMappedTo, new BooleanExpr( true ) );
                            }

                            // Also add MapOp to list
                            ProjectAttributes.Add( TargetLookupAttribute, MapOp );

                            Project ProjectOp = new Project( ProjectAttributes );

                            OperationsToExecute.AddRange( new MongoDBOperator[] { LookupTarget, ProjectOp } );
                        }
                    }
                }
            }
            else if ( Relationship.Cardinality == RelationshipCardinality.OneToMany )
            {
                // Go through all target entities
                foreach ( Entity TargetEntity in TargetEntities )
                {
                    if ( TargetEntity is ComputedEntity )
                    {
                        // Computed entities require better handling
                    }
                    else
                    {
                        // Retrieve mapping rule for target
                        MapRule TargetRule = ModelMap.Rules.FirstOrDefault( R => R.Source.Name == TargetEntity.Name );
                        if ( TargetRule == null )
                        {
                            throw new ImpossibleOperationException( $"Entity {TargetEntity.Name} has no valid mapping." );
                        }

                        if ( !Relationship.HasRelation( SourceEntity, TargetEntity ) )
                        {
                            throw new ImpossibleOperationException( $"Entities {SourceEntity.Name} and {TargetEntity.Name} are not related through {Relationship.Name}" );
                        }

                        RelationshipConnection RelationshipData = Relationship.GetRelation( SourceEntity, TargetEntity );

                        // Are source and target mapped to the same collection
                        if ( SourceRule.Target.Name == TargetRule.Target.Name )
                        {
                            /* Target entity is embedded in the source entity
                               This also means that the relationship attributes (if any)
                               are mapped to the same collection.

                               We just need to move them to a more appropriate place
                            */

                            Dictionary<string, string> AddTargetAttributes = new Dictionary<string, string>();
                            Dictionary<string, ProjectExpression> TargetFieldsToRemove = new Dictionary<string, ProjectExpression>();
                            
                            // Move relationship attributes first
                            foreach ( DataAttribute Attribute in Relationship.Attributes )
                            {
                                // Retrieve attribute mapping
                                string AttributeMappedTo = RelationshipRule.Rules.FirstOrDefault( R => R.Key == Attribute.Name ).Value;

                                // Check if the attribute is mapped to a complex attribute (like an embedded document)
                                if ( AttributeMappedTo.Contains( "." ) )
                                {
                                    string[] AttributeHierarchy = AttributeMappedTo.Split( new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries );
                                    string Root = AttributeHierarchy[ 0 ];

                                    // Add to remove list
                                    if ( !TargetFieldsToRemove.ContainsKey( Root ) )
                                    {
                                        TargetFieldsToRemove.Add( Root, new BooleanExpr( false ) );
                                    }
                                }

                                // If not found, skip the attribute
                                if ( string.IsNullOrWhiteSpace( AttributeMappedTo ) )
                                {
                                    continue;
                                }

                                AddTargetAttributes.Add( $"data_{Relationship.Name}.{Relationship.Name}_{Attribute.Name}", $"${AttributeMappedTo}" );
                            }

                            // For an One-To-Many relationship, we need to map the array containing
                            // the joined entity and rename it to match the algebra
                            Dictionary<string, string> AttributeMapRules = new Dictionary<string, string>();
                            // Retrieve the name of the attribute that holds the embedded document
                            string AttributePath = TargetRule.Rules.First( R => R.Key == TargetEntity.Attributes.First().Name ).Value;
                            string[] AttributeMap = AttributePath.Split( new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries );
                            string AttributeRoot = AttributeMap.First();

                            string AttributeMapNameRef = $"{ Relationship.Name.ToLower() }_{ TargetEntity.Name}";

                            foreach ( DataAttribute Attribute in TargetEntity.Attributes )
                            {
                                string AttrMappedTo = TargetRule.Rules.First( R => R.Key == Attribute.Name ).Value;
                                string[] AttrMap = AttrMappedTo.Split( new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries );
                                string AttrName = AttrMap[ 1 ];

                                AttributeMapRules.Add( $"{TargetEntity.Name}_{Attribute.Name}", $"$${AttributeMapNameRef}.{AttrName}" );
                            }

                            MapExpr ProjectMap = new MapExpr( $"${AttributeRoot}", AttributeMapNameRef, AttributeMapRules );

                            Dictionary<string, ProjectExpression> ProjectFields = new Dictionary<string, ProjectExpression>();
                            ProjectFields.Add( $"data_{Relationship.Name}", ProjectMap );
                            
                            // As we're executing a project operation to add a new field, we need to also
                            // set the source entity attributes as visible
                            foreach ( DataAttribute Attribute in SourceEntity.Attributes )
                            {
                                string SourceAttrMappedTo = SourceRule.Rules.First( R => R.Key == Attribute.Name ).Value;
                                ProjectFields.Add( SourceAttrMappedTo, new BooleanExpr( true ) );
                            }


                            Project ProjectOp = new Project( ProjectFields );


                            OperationsToExecute.AddRange( new MongoDBOperator[] { ProjectOp } );
                        }
                        else
                        {
                            /* 
                             * In this case, the target entity is mapped to its own collection
                             * Which also means that if the relationship has attributes
                             * it must have been mapped to target collection
                             */

                            // Run a custom pipeline to rename the joined entity attributes
                            List<MongoDBOperator> CustomPipeline = new List<MongoDBOperator>();

                            // First thing to do in the pipeline is to match the joined entity with the source entity
                            Dictionary<string, string> PipelineVariables = new Dictionary<string, string>();
                            RelationshipConnection SourceConnection = Relationship.Relations.First();
                            string SourceRef = SourceRule.Rules.First( R => R.Key == SourceConnection.SourceAttribute.Name ).Value;
                            string SourceVar = $"source_{SourceConnection.SourceAttribute.Name}";
                            PipelineVariables.Add( SourceVar, $"${SourceRef}" );

                            string SourceEntityAttribute = TargetRule.Rules.First( R => R.Key == SourceConnection.TargetAttribute.Name ).Value;
                            EqExpr MatchSourceEq = new EqExpr( $"${SourceEntityAttribute}", $"$${SourceVar}" );
                            Match MatchSourceOp = new Match( new Expr( MatchSourceEq ) );

                            // Rename attributes
                            Dictionary<string, ProjectExpression> RenameAttributes = new Dictionary<string, ProjectExpression>();
                            foreach ( DataAttribute Attribute in TargetEntity.Attributes )
                            {
                                string AttributeMappedTo = TargetRule.Rules.First( R => R.Key == Attribute.Name ).Value;
                                if ( AttributeMappedTo != null )
                                {
                                    RenameAttributes.Add( $"{TargetEntity.Name}_{Attribute.Name}", new ValueExpr( $"\"${AttributeMappedTo}\"" ) );
                                }
                            }

                            // Force remove _id
                            RenameAttributes.Add( "_id", new BooleanExpr( false ) );

                            Project RenameOp = new Project( RenameAttributes );

                            CustomPipeline.AddRange( new List<MongoDBOperator> { MatchSourceOp, RenameOp } );

                            LookupOperator RelationshipLookup = new LookupOperator
                            {
                                From = TargetRule.Target.Name,
                                Let = PipelineVariables,
                                Pipeline = CustomPipeline,
                                As = $"data_{Relationship.Name}"
                            };
                          

                            OperationsToExecute.Add( RelationshipLookup );
                        }
                    }
                }
            }
            else if ( Relationship.Cardinality == RelationshipCardinality.ManyToMany )
            {
                // On Many to Many relationships we need to run custom pipelines to join entities
                // and multiple entities must be in the same pipeline as the relationship
                List<MongoDBOperator> CustomPipeline = new List<MongoDBOperator>();

                // The first operation for each relationship is to match the relationship document to the source entity
                Dictionary<string, string> PipelineVariables = new Dictionary<string, string>();
                RelationshipConnection SourceConnection = Relationship.Relations.First();
                string SourceRef = SourceRule.Rules.First( R => R.Key == SourceConnection.SourceAttribute.Name ).Value;
                string SourceVar = $"source_{SourceConnection.SourceAttribute.Name}";

                PipelineVariables.Add( SourceVar, $"${SourceRef}" );

                string RelationshipSourceRef = RelationshipRule.Rules.First( R => R.Key == SourceConnection.RefSourceAtrribute.Name ).Value;
                EqExpr MatchSourceEq = new EqExpr( $"${RelationshipSourceRef}", $"$${SourceVar}" );

                Match MatchSourceOp = new Match( new Expr( MatchSourceEq ) );

                CustomPipeline.Add( MatchSourceOp );

                // Add Lookup for relationship
                LookupOperator RelationshipLookup = new LookupOperator
                {
                    From = RelationshipRule.Target.Name,
                    Let = PipelineVariables,
                    Pipeline = CustomPipeline,
                    As = $"data_{Relationship.Name}"
                };

                OperationsToExecute.Add( RelationshipLookup );

                foreach ( Entity TargetEntity in TargetEntities )
                {
                    if ( TargetEntity is ComputedEntity )
                    {
                    }
                    else
                    {
                        // Get Target rule
                        MapRule TargetRule = ModelMap.Rules.FirstOrDefault( R => R.Source.Name == TargetEntity.Name );
                        if ( TargetRule == null )
                        {
                            throw new ImpossibleOperationException( $"Entity {TargetEntity.Name} has no valid mapping." );
                        }

                        if ( !Relationship.HasRelation( SourceEntity, TargetEntity ) )
                        {
                            throw new ImpossibleOperationException( $"Entities {SourceEntity.Name} and {TargetEntity.Name} are not related through {Relationship.Name}" );
                        }

                        RelationshipConnection RelationshipData = Relationship.GetRelation( SourceEntity, TargetEntity );

                        // Many to many relationships so far we're only considering that entities are mapped to distinct collections
                        // and the relationship linking them has it's own collection

                        // Build the operations for the custom pipeline

                        // Foreach target entity, we must do a lookup, unwind, addfields and project operations
                        // Lookup target
                        string TargetLookupAs = $"data_{TargetEntity.Name}"; 

                        LookupOperator LookupTargetOp = new LookupOperator
                        {
                            From = TargetRule.Target.Name,
                            ForeignField = TargetRule.Rules.First( R => R.Key == RelationshipData.TargetAttribute.Name ).Value,
                            LocalField = RelationshipRule.Rules.First( R => R.Key == RelationshipData.RefTargetAttribute.Name ).Value,
                            As = TargetLookupAs
                        };

                        // Unwind
                        Unwind UnwindTarget = new Unwind( TargetLookupAs );


                        // Add fields
                        Dictionary<string, string> FieldsToAdd = new Dictionary<string, string>();

                        foreach ( DataAttribute Attribute in TargetEntity.Attributes )
                        {
                            string AttributeMappedTo = TargetRule.Rules.First( R => R.Key == Attribute.Name ).Value;
                            FieldsToAdd.Add( $"{TargetEntity.Name}_{Attribute.Name}", $"${TargetLookupAs}.{AttributeMappedTo}" );
                        }

                        AddFields AddFieldsOp = new AddFields( FieldsToAdd );

                        // Project - remove joined data extra data
                        Dictionary<string, ProjectExpression> ProjectExpressions = new Dictionary<string, ProjectExpression>
                        {
                            { TargetLookupAs, new BooleanExpr( false ) }
                        };
                        Project ProjectOp = new Project( ProjectExpressions );

                        CustomPipeline.AddRange( new MongoDBOperator[] { LookupTargetOp, UnwindTarget, AddFieldsOp, ProjectOp } );
                    }
                }                
            }

            // Assign operation list
            LastResult.Commands.AddRange( OperationsToExecute );
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Initialize a new instance of Join Operation class
        /// </summary>
        /// <param name="SourceEntity">Source entity</param>
        /// <param name="Relationship">Join through this relationship</param>
        /// <param name="TargetEntities">Target entities</param>
        /// <param name="ModelMap">Map rules between ER and Mongo</param>
        public RelationshipJoinOperator( Entity SourceEntity, Relationship Relationship, List<Entity> TargetEntities, ModelMapping ModelMap ) : base( ModelMap )
        {
            this.SourceEntity = SourceEntity;
            this.TargetEntities = TargetEntities;
            this.Relationship = Relationship;
        }
        #endregion
    }
}
