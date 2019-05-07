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
    public class JoinOperation : BaseOperation
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
        public override OperationResult Run( OperationResult LastResult )
        {
            // Retrieve mapping rules for Source Entity and Relationship
            MapRule SourceRule = ModelMap.Rules.First( Rule => Rule.Source.Name == SourceEntity.Name );
            MapRule RelationshipRule = ModelMap.Rules.FirstOrDefault( Rule => Rule.Source.Name == Relationship.Name );

            // Check if the relationship has attributes
            bool RelationshipHasAttributes = Relationship.Attributes.Count > 0;

            List<BaseOperator> OperationsToExecute = new List<BaseOperator>();

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
                            Dictionary<string, bool> TargetFieldsToRemove = new Dictionary<string, bool>();
                            // attributes in the data_RelName attribute

                            // Check if it is possible to find the root attribute for the embbebed collection
                            bool FoundRootAttribute = false;
                            string RootAttributeMap = TargetRule.Rules.FirstOrDefault().Value;
                            if ( RootAttributeMap != null )
                            {
                                string[] AttributeHierarchy = RootAttributeMap.Split( new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries );
                                if ( AttributeHierarchy.Length > 0 )
                                {
                                    TargetFieldsToRemove.Add( AttributeHierarchy.First(), false );
                                    FoundRootAttribute = true;
                                }
                            }

                            foreach ( DataAttribute Attribute in TargetEntity.Attributes )
                            {
                                string AttributeMappedTo = TargetRule.Rules.FirstOrDefault( A => A.Key == Attribute.Name ).Value;

                                if ( AttributeMappedTo != null )
                                {
                                    AddTargetAttributes.Add( $"{joinedAttributeName}.{TargetEntity.Name}_{Attribute.Name}", $"${AttributeMappedTo}" );
                                    if ( !FoundRootAttribute )
                                    {
                                        TargetFieldsToRemove.Add( AttributeMappedTo, false );
                                    }
                                }
                            }                            

                            AddFields AddFieldsOp = new AddFields( AddTargetAttributes );
                            Project RemoveFieldsOp = new Project( TargetFieldsToRemove );

                            OperationsToExecute.AddRange( new BaseOperator[] { AddFieldsOp, RemoveFieldsOp } );
                        }
                        else
                        {
                            string TargetLookupAttribute = $"data_{TargetEntity.Name}";

                            // Lookup entity
                            LookupOperator LookupTarget = new LookupOperator
                            {
                                From = TargetRule.Target.Name,
                                ForeignField = TargetRule.Rules.First( R => R.Key == RelationshipData.TargetAttribute.Name ).Value,
                                LocalField = SourceRule.Rules.First( R => R.Key == RelationshipData.SourceAttribute.Name ).Value,
                                As = TargetLookupAttribute
                            };

                            // Add fields and hide source object
                            Dictionary<string, string> AddTargetAttributes = new Dictionary<string, string>();
                            Dictionary<string, bool> TargetFieldsToRemove = new Dictionary<string, bool>();
                            // attributes in the data_RelName attribute

                            foreach ( DataAttribute Attribute in TargetEntity.Attributes )
                            {
                                string AttributeMappedTo = TargetRule.Rules.FirstOrDefault( A => A.Key == Attribute.Name ).Value;

                                if ( AttributeMappedTo != null )
                                {
                                    AddTargetAttributes.Add( $"{joinedAttributeName}.{TargetEntity.Name}_{Attribute.Name}", $"${TargetLookupAttribute}.{AttributeMappedTo}" );
                                }
                            }

                            // Unwind joined data
                            Unwind UnwindOp = new Unwind( TargetLookupAttribute );

                            // Only one field to hide
                            TargetFieldsToRemove.Add( TargetLookupAttribute, false );

                            AddFields AddFieldsOp = new AddFields( AddTargetAttributes );
                            Project RemoveFieldsOp = new Project( TargetFieldsToRemove );

                            OperationsToExecute.AddRange( new BaseOperator[] { LookupTarget, UnwindOp, AddFieldsOp, RemoveFieldsOp } );
                        }
                    }
                }
            }
            else if ( Relationship.Cardinality == RelationshipCardinality.OneToMany )
            {

            }
            else if ( Relationship.Cardinality == RelationshipCardinality.ManyToMany )
            {

            }


            // Iterate through target entities
            // BACKUP
            /*foreach ( Entity TargetEntity in TargetEntities )
            {
                // Check if the target entity is a computed entity
                if ( TargetEntity is ComputedEntity )
                {
                    // TODO:
                }
                else
                {
                    // Check if Target Entity is reachable through the relationship
                    if ( !Relationship.HasRelation( SourceEntity, TargetEntity ) )
                    {
                        throw new ImpossibleOperationException( $"Entity {TargetEntity.Name} is not reachable through {Relationship.Name}" );
                    }
                    // Retrieve mapping rules for target entity
                    MapRule TargetRule = ModelMap.Rules.First( Rule => Rule.Source.Name == TargetEntity.Name );

                    // Get relationship data
                    RelationshipConnection RelationshipData = Relationship.GetRelation( SourceEntity, TargetEntity );

                    // Check if the Relationship has attributes and if it is mapped to either source or target entity
                    bool RelationshipSharesTarget = true;
                    if ( RelationshipRule != null )
                    {
                        RelationshipSharesTarget = ( new Collection[] { SourceRule.Target, TargetRule.Target } ).Contains( RelationshipRule.Target );
                    }

                    if ( RelationshipHasAttributes || !RelationshipSharesTarget )
                    {
                        // This requires a custom lookup pipeline
                        // When using a custom pipeline, the first thing to do is
                        // limit the amount of records return by matching it with the
                        // source entity

                        // Build variable for pipeline
                        string MatchThisVar = $"{SourceEntity.Name}_id";
                        string MatchThisValue = $"${SourceRule.Rules.First( R => R.Key == RelationshipData.SourceAttribute.Name ).Value}";

                        Dictionary<string, string> RelationshipLookupLet = new Dictionary<string, string>();
                        RelationshipLookupLet.Add( MatchThisVar, MatchThisValue );

                        List<BaseOperator> LookupPipeline = new List<BaseOperator>();

                        string LookupMatchField = $"${RelationshipRule.Rules.First( R => R.Key == RelationshipData.RefSourceAtrribute.Name ).Value}";
                        string LookupMatchValue = $"$${MatchThisVar}";

                        Match MatchOp = new Match
                        {
                            Expression = new Expr( new EqExpr( LookupMatchField, LookupMatchValue ) )
                        };                        

                        // Create lookup operation for target entity
                        LookupOperator LookupTarget = new LookupOperator
                        {
                            From = TargetRule.Target.Name,
                            ForeignField = TargetRule.Rules.First( R => R.Key == RelationshipData.TargetAttribute.Name ).Value,
                            LocalField = RelationshipRule.Rules.First( R => R.Key == RelationshipData.RefTargetAttribute.Name ).Value,
                            As = $"data_{Relationship.Name}"
                        };

                        // Unwind joined data
                        Unwind UnwindTarget = new Unwind
                        {
                            Field = $"$data_{Relationship.Name}"
                        };

                        // Bring joined entity one level above
                        Dictionary<string, string> AddTargetAttributes = new Dictionary<string, string>();
                        Dictionary<string, bool> TargetFieldsToRemove = new Dictionary<string, bool>();

                        foreach ( DataAttribute Attribute in TargetEntity.Attributes )
                        {
                            string AttributeMappedTo = TargetRule.Rules.FirstOrDefault( A => A.Key == Attribute.Name ).Value;

                            if ( AttributeMappedTo != null )
                            {
                                AddTargetAttributes.Add( $"data_{Relationship.Name}.{Attribute.Name}", $"${AttributeMappedTo}" );
                                TargetFieldsToRemove.Add( AttributeMappedTo, false );
                            }
                        }

                        AddFields AddFieldsOp = new AddFields( AddTargetAttributes );
                        Project RemoveFieldsOp = new Project( TargetFieldsToRemove );

                        // Pipeline ready, setup lookup for relationship

                    }
                    else
                    {
                        // Check if Source and Target relationships are mapped to the same MongoDB collection
                        if ( SourceRule.Target.Name == TargetRule.Target.Name )
                        {
                            // In this case we just have to setup the output to match the algebra
                            Dictionary<string, string> AddTargetAttributes = new Dictionary<string, string>();
                            Dictionary<string, bool> TargetFieldsToRemove = new Dictionary<string, bool>();
                            // If the relationship is a One-To-One, we have to add the target entity
                            // attributes in the data_RelName attribute
                            if ( RelationshipData.Cardinality == RelationshipCardinality.OneToOne )
                            {                              
                                foreach ( DataAttribute Attribute in TargetEntity.Attributes )
                                {   
                                    string AttributeMappedTo = TargetRule.Rules.FirstOrDefault( A => A.Key == Attribute.Name ).Value;

                                    if ( AttributeMappedTo != null )
                                    {
                                        AddTargetAttributes.Add( $"data_{Relationship.Name}.{Attribute.Name}", $"${AttributeMappedTo}" );
                                        TargetFieldsToRemove.Add( AttributeMappedTo, false );
                                    }                                
                                }

                                AddFields AddFieldsOp = new AddFields( AddTargetAttributes );
                                Project RemoveFieldsOp = new Project( TargetFieldsToRemove );

                                OperationsToExecute.AddRange( new BaseOperator[] { AddFieldsOp, RemoveFieldsOp } );
                            }
                            else
                            {
                                // For a One-To-Many relationship, we need to move the attribute that holds
                                // the data under data_RelName attribute

                                // Fetch the root name from the first attribute
                                DataAttribute Ref = TargetEntity.Attributes.First();
                                string RefMappedTo = TargetRule.Rules.FirstOrDefault( A => A.Key == Ref.Name ).Value;

                                if ( RefMappedTo != null )
                                {
                                    string[] AttributeHierarchy = RefMappedTo.Split( new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries );
                                    string AttributeRoot = AttributeHierarchy.Length > 0 ? AttributeHierarchy.First() : null;

                                    if ( !string.IsNullOrWhiteSpace(AttributeRoot))
                                    {
                                        AddTargetAttributes.Add( $"data_{Relationship.Name}", $"${AttributeRoot}" );
                                        TargetFieldsToRemove.Add( AttributeRoot, false );

                                        AddFields AddFieldsOp = new AddFields( AddTargetAttributes );
                                        Project RemoveFieldsOp = new Project( TargetFieldsToRemove );

                                        OperationsToExecute.AddRange( new BaseOperator[] { AddFieldsOp, RemoveFieldsOp } );
                                    }
                                }
                            }
                        }
                        else
                        {
                            // Either the relationship has no attributes or they're shared with source or target entity
                            // It means a simple lookup can do the trick
                            LookupOperator LookupOp = new LookupOperator
                            {
                                From = TargetRule.Target.Name,
                                ForeignField = TargetRule.Rules.First( R => R.Key == RelationshipData.TargetAttribute.Name ).Value,
                                LocalField = SourceRule.Rules.First( R => R.Key == RelationshipData.SourceAttribute.Name ).Value,
                                As = $"data_{Relationship.Name}"
                            };

                            OperationsToExecute.Add( LookupOp );
                        }
                    }
                }
            }
            */
            // Assign operation list
            LastResult.Commands.AddRange( OperationsToExecute );

            return LastResult;
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
        public JoinOperation( Entity SourceEntity, Relationship Relationship, List<Entity> TargetEntities, ModelMapping ModelMap ) : base( ModelMap )
        {
            this.SourceEntity = SourceEntity;
            this.TargetEntities = TargetEntities;
            this.Relationship = Relationship;
        }
        #endregion
    }
}
