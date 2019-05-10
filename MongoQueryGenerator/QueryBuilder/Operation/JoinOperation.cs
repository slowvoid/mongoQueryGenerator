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
                            /* Target entity is embbebed in the source entity
                               This also means that the relationship attributes (if any)
                               are mapped to the same collection.

                               We just need to move them to a more appropriate place
                            */

                            Dictionary<string, string> AddTargetAttributes = new Dictionary<string, string>();
                            Dictionary<string, bool> TargetFieldsToRemove = new Dictionary<string, bool>();
                            
                            // Move relationship attributes first
                            foreach ( DataAttribute Attribute in Relationship.Attributes )
                            {
                                // Retrieve attribute mapping
                                string AttributeMappedTo = RelationshipRule.Rules.FirstOrDefault( R => R.Key == Attribute.Name ).Value;

                                // Check if the attribute is mapped to a complex attribute (like an embbebed document)
                                if ( AttributeMappedTo.Contains( "." ) )
                                {
                                    string[] AttributeHierarchy = AttributeMappedTo.Split( new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries );
                                    string Root = AttributeHierarchy[ 0 ];

                                    // Add to remove list
                                    if ( !TargetFieldsToRemove.ContainsKey( Root ) )
                                    {
                                        TargetFieldsToRemove.Add( Root, false );
                                    }
                                }

                                // If not found, skip the attribute
                                if ( string.IsNullOrWhiteSpace( AttributeMappedTo ) )
                                {
                                    continue;
                                }

                                AddTargetAttributes.Add( $"data_{Relationship.Name}.{Relationship.Name}_{Attribute.Name}", $"${AttributeMappedTo}" );
                            }

                            // For a One-To-Many relationship, we need to move the attribute that holds
                            // the data under data_RelName attribute
                            // Fetch the root name from the first attribute
                            DataAttribute Ref = TargetEntity.Attributes.First();
                            string RefMappedTo = TargetRule.Rules.FirstOrDefault( A => A.Key == Ref.Name ).Value;
                            if ( RefMappedTo != null )
                            {
                                string[] AttributeHierarchy = RefMappedTo.Split( new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries );
                                string AttributeRoot = AttributeHierarchy.Length > 0 ? AttributeHierarchy.First() : null;
                                if ( !string.IsNullOrWhiteSpace( AttributeRoot ) )
                                {
                                    AddTargetAttributes.Add( $"data_{Relationship.Name}", $"${AttributeRoot}" );
                                    if ( !TargetFieldsToRemove.ContainsKey( AttributeRoot ) )
                                    {
                                        TargetFieldsToRemove.Add( AttributeRoot, false );
                                    }
                                }
                            }

                            // Create Operations
                            AddFields AddFieldsOp = new AddFields( AddTargetAttributes );
                            Project ProjectOp = new Project( TargetFieldsToRemove );

                            OperationsToExecute.AddRange( new BaseOperator[] { AddFieldsOp, ProjectOp } );
                        }
                        else
                        {
                            /* In this case, the target entity is mapped to its own collection
                             * Which also means that if the relationship has attributes
                             * it must have been mapped to target collection
                             */
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
            else if ( Relationship.Cardinality == RelationshipCardinality.ManyToMany )
            {

            }

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
