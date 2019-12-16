using QueryBuilder.ER;
using QueryBuilder.Javascript;
using QueryBuilder.Map;
using QueryBuilder.Mongo.Aggregation.Operators;
using QueryBuilder.Mongo.Expressions;
using QueryBuilder.Operation.Arguments;
using QueryBuilder.Operation.Exceptions;
using QueryBuilder.Shared;
using System;
using System.Collections.Generic;
using System.Linq;

namespace QueryBuilder.Operation
{
    /// <summary>
    /// Represents a JOIN operation
    /// </summary>
    public class RelationshipJoinOperator : AlgebraOperator
    {
        #region Properties
        /// <summary>
        /// Source entity
        /// </summary>
        public QueryableEntity SourceEntity { get; set; }
        /// <summary>
        /// Relationship that connects the source and target entities
        /// </summary>
        public Relationship Relationship { get; set; }
        /// <summary>
        /// Target entities
        /// </summary>
        public List<QueryableEntity> TargetEntities { get; set; }

        public string RelationshipAlias { get; set; }
        #endregion

        #region Methods
        /// <summary>
        /// Run the algorithm
        /// </summary>
        /// <param name="LastResult"></param>
        /// <returns></returns>
        public override AlgebraOperatorResult Run()
        {
            // Store Operations
            List<MongoDBOperator> OperationsToExecute = new List<MongoDBOperator>();

            // Check if the Source entity and the relationship are connected
            if ( !Relationship.ContainsElement( (Entity)SourceEntity.Element ) )
            {
                throw new NotRelatedException( $"Entity {SourceEntity.GetName()} is not related to relationship {Relationship.Name}" );
            }

            // Retrieve main rule for relationship
            MapRule MainRelationshipRule = ModelMap.FindMainRule( Relationship );

            // Find Source Main rule
            MapRule MainSourceRule = ModelMap.FindMainRule( SourceEntity.Element );

            if ( MainSourceRule == null )
            {
                throw new InvalidMapException( $"Left side entities must have a main mapping [{SourceEntity.GetName()}" );
            }

            // Check if it is present
            if ( MainRelationshipRule != null )
            {
                // Ok, relationship has a main mapping, it usually means a N:M(:P:...:Z) relationship
                // this requires a custom lookup pipeline
                List<MongoDBOperator> PipelineOperators = new List<MongoDBOperator>();

                // In this case we should find a rule for Source Entity that targets the relationship collection
                MapRule SourceRule = ModelMap.FindRule( SourceEntity.Element, MainRelationshipRule.Target );

                // Check if the rule is present
                if ( SourceRule == null )
                {
                    throw new InvalidMapException( $"Missing rules for {SourceEntity.GetName()} that has the same target as the relationship {Relationship.Name}" );
                }

                // Source match attribute
                string SourceMatchAttribute = $"match_{SourceEntity.GetName()}";

                // Setup pipeline variables
                Dictionary<string, string> PipelineVariables = new Dictionary<string, string>();
                PipelineVariables.Add( SourceMatchAttribute, $"${MainSourceRule.GetRuleValueForAttribute( SourceEntity.Element.GetIdentifierAttribute() )}" );

                // Create a Match Operator for the pipeline
                MatchOperator MatchSourceOp = MatchOperator.CreateLookupMatch( SourceRule.GetRuleValueForAttribute( SourceEntity.Element.GetIdentifierAttribute() ), SourceMatchAttribute );

                PipelineOperators.Add( MatchSourceOp );

                // Store attributes to match with root
                List<string> FieldsToMergeWithRoot = new List<string>();

                // Iterate target entities
                foreach ( QueryableEntity Target in TargetEntities )
                {
                    if ( Target.Element is ComputedEntity )
                    {
                        ComputedEntity TargetComputedEntity = Target.GetComputedEntity();
                        
                        // Check if it is related
                        if ( !Relationship.AreRelated( (Entity)SourceEntity.Element, (Entity)TargetComputedEntity.SourceEntity.Element ) )
                        {
                            throw new NotRelatedException( $"Entities {SourceEntity.GetName()} and {TargetComputedEntity.SourceEntity.GetName()} are not related through {Relationship.Name}" );
                        }
                        
                        // In a computed entity we first check if the source entity is related to the operator source
                        // And in a compute entity the source entity can never be embbeded which means it must have a main mapping
                        MapRule MainTargetRule = ModelMap.FindMainRule( TargetComputedEntity.SourceEntity.Element );

                        if ( MainTargetRule == null )
                        {
                            throw new MissingMappingException( $"Target Entity {TargetComputedEntity.SourceEntity.GetName()}must have a main mapping" );
                        }

                        // As they can neven be embbeded, a lookup is necessary
                        // and we'll use a custom pipeline
                        List<MongoDBOperator> CEPipeline = new List<MongoDBOperator>();

                        // Find mapping between the middle collection and the target entity
                        MapRule TargetRule = ModelMap.FindRule( TargetComputedEntity.SourceEntity.Element, MainRelationshipRule.Target );

                        if ( TargetRule == null )
                        {
                            throw new MissingMappingException( $"Entity {TargetComputedEntity.SourceEntity.GetName()} must have a map that targets {MainRelationshipRule.Target.Name}" );
                        }

                        string MatchTargetAttribute = $"match_{TargetComputedEntity.SourceEntity.GetName()}";
                        // Variables
                        Dictionary<string, string> CEVariables = new Dictionary<string, string>();
                        CEVariables.Add( MatchTargetAttribute, $"${TargetRule.GetRuleValueForAttribute( TargetComputedEntity.SourceEntity.Element.GetIdentifierAttribute() )}" );

                        MatchOperator MatchTargetOp = MatchOperator.CreateLookupMatch( MainTargetRule.GetRuleValueForAttribute( TargetComputedEntity.SourceEntity.Element.GetIdentifierAttribute() ), MatchTargetAttribute );

                        // Process computed entity
                        List<MongoDBOperator> CEOperators = ProcessComputedEntity( TargetComputedEntity );

                        // Add operators to list
                        CEPipeline.Add( MatchTargetOp );
                        CEPipeline.AddRange( CEOperators );

                        string CELookupAs = $"data_{SourceEntity.GetName()}_{TargetComputedEntity.SourceEntity.GetName()}";

                        // Create lookup
                        LookupOperator LookupCEOp = new LookupOperator()
                        {
                            From = MainTargetRule.Target.Name,
                            Let = CEVariables,
                            Pipeline = CEPipeline,
                            As = CELookupAs
                        };

                        // Add a unwind op
                        UnwindOperator UnwindCEOp = new UnwindOperator( CELookupAs );

                        PipelineOperators.AddRange( new MongoDBOperator[] { LookupCEOp, UnwindCEOp } );

                        FieldsToMergeWithRoot.Add( CELookupAs );
                    }
                    else
                    {
                        // Check if the target is related to the source through the relationship
                        if ( !Relationship.AreRelated( (Entity)SourceEntity.Element, (Entity)Target.Element ) )
                        {
                            throw new NotRelatedException( $"Entities {SourceEntity.GetName()} and {Target.GetName()} are not related through {Relationship.Name}" );
                        }

                        // Check if the target has a main mapping
                        MapRule MainTargetRule = ModelMap.FindMainRule( Target.Element );

                        if ( MainTargetRule == null )
                        {
                            throw new MissingMappingException( $"Target entity {Target.GetName()} must have a main mapping" );
                        }

                        // Retrieve Target rule pointing to the relationship collection
                        MapRule TargetRule = ModelMap.FindRule( Target.Element, MainRelationshipRule.Target );

                        if ( TargetRule == null )
                        {
                            throw new MissingMappingException( $"Target entity {Target.GetName()} must have a mapping that targets {MainRelationshipRule.Target.Name}" );
                        }

                        // Create look for target 
                        string LookupTargetAs = $"data_{Target.GetName()}";

                        LookupOperator TargetLookup = new LookupOperator()
                        {
                            From = MainTargetRule.Target.Name,
                            ForeignField = MainTargetRule.GetRuleValueForAttribute( Target.Element.GetIdentifierAttribute() ),
                            LocalField = TargetRule.GetRuleValueForAttribute( Target.Element.GetIdentifierAttribute() ),
                            As = LookupTargetAs
                        };

                        // Create operations to bring fields to an upper level
                        UnwindOperator UnwindTargetOp = new UnwindOperator( LookupTargetAs );

                        // Store Attributes
                        Dictionary<string, JSCode> TargetAttributes = new Dictionary<string, JSCode>();

                        // Iterate target attributes and bring them one level up
                        foreach ( DataAttribute Attribute in Target.Element.Attributes )
                        {
                            string RuleValue = MainTargetRule.GetRuleValueForAttribute( Attribute );

                            // Ignore if no rule is found
                            if ( string.IsNullOrWhiteSpace( RuleValue ) )
                            {
                                continue;
                            }

                            TargetAttributes.Add( $"{Target.GetName()}_{Attribute.Name}", new JSString( $"\"${LookupTargetAs}.{RuleValue}\"" ) );
                        }

                        // Create AddFields Operation
                        AddFieldsOperator AddTargetFields = new AddFieldsOperator( TargetAttributes );

                        // Remove joined data (unmapped)
                        // and other unwanted attributes
                        List<string> UnwantedAttributes = new List<string>();
                        UnwantedAttributes.Add( LookupTargetAs );
                        UnwantedAttributes.Add( SourceRule.GetRuleValueForAttribute( SourceEntity.Element.GetIdentifierAttribute() ) );
                        UnwantedAttributes.Add( TargetRule.GetRuleValueForAttribute( Target.Element.GetIdentifierAttribute() ) );

                        ProjectOperator HideTargetUnmappedOp = ProjectOperator.HideAttributesOperator( UnwantedAttributes );

                        // Add operators to pipeline
                        PipelineOperators.AddRange( new MongoDBOperator[] { TargetLookup, UnwindTargetOp, AddTargetFields, HideTargetUnmappedOp } );
                    }
                }

                Dictionary<string, JSCode> RelationshipAttributesToAdd = new Dictionary<string, JSCode>();
                List<string> RelationshipAttributesToHide = new List<string>();

                // Process Relationship attributes (if any)
                foreach ( DataAttribute Attribute in Relationship.Attributes )
                {
                    string RuleValue = MainRelationshipRule.GetRuleValueForAttribute( Attribute );

                    if ( string.IsNullOrWhiteSpace( RuleValue ) )
                    {
                        continue;
                    }

                    RelationshipAttributesToAdd.Add( $"{Relationship.Name}_{Attribute.Name}", new JSString( $"\"${RuleValue}\"" ) );
                    // Add original attribute to hide list
                    RelationshipAttributesToHide.Add( RuleValue );
                }

                if ( RelationshipAttributesToAdd.Count > 0 )
                {
                    // Create Operator
                    AddFieldsOperator AddRelationshipFieldsOp = new AddFieldsOperator( RelationshipAttributesToAdd );

                    // Also hide original fields
                    ProjectOperator HideRelationshipAttributes = ProjectOperator.HideAttributesOperator( RelationshipAttributesToHide );

                    // Add to pipeline
                    PipelineOperators.AddRange( new MongoDBOperator[] { AddRelationshipFieldsOp, HideRelationshipAttributes } );
                }

                // Merge fields with root if any
                if ( FieldsToMergeWithRoot.Count > 0 )
                {
                    FieldsToMergeWithRoot.Add( "$$ROOT" );
                    MergeObjectsOperator MergeWithRootOp = new MergeObjectsOperator( FieldsToMergeWithRoot );
                    ReplaceRootOperator ReplaceRootOp = new ReplaceRootOperator( MergeWithRootOp.ToJSCode() );
                    PipelineOperators.Add( ReplaceRootOp );

                    // Hide merged
                    ProjectOperator HideMergedOp = ProjectOperator.HideAttributesOperator( FieldsToMergeWithRoot.Where( F => F != "$$ROOT" ) );
                    PipelineOperators.Add( HideMergedOp );
                }

                // Build Lookup Operator
                LookupOperator LookupRelationshipOp = new LookupOperator()
                {
                    From = MainRelationshipRule.Target.Name,
                    Let = PipelineVariables,
                    Pipeline = PipelineOperators,
                    As = $"data_{Relationship.Name}"
                };

                // Add Relationship Lookup to List of execution
                OperationsToExecute.Add( LookupRelationshipOp );
            }
            else
            {
                // In this case we have a 1:1 or 1:N relationship
                // Relationship attributes must be embbeded (on source or target)
                MapRule RelationshipRule = ModelMap.Rules.FirstOrDefault( R => R.Source == Relationship );

                // Store fields to be merged with root
                List<string> AttributesToMergeWithRelatioshipData = new List<string>();
                List<string> AttributesToConcatWithRelationshipData = new List<string>();
                List<string> AttributesToRemove = new List<string>();

                // Iterate targets
                foreach ( QueryableEntity Target in TargetEntities )
                {
                    if ( Target.Element is ComputedEntity )
                    {
                        // TODO:
                    }
                    else
                    {
                        // Check if target has a main mapping
                        MapRule MainTargetRule = ModelMap.FindMainRule( Target.Element );

                        if ( MainTargetRule == null )
                        {
                            // Target must be embedded to the source entity
                            // It could be either a 1:1 or 1:N relation, check if the root attribute is multivalued
                            // Fetch target rule
                            MapRule TargetRule = ModelMap.FindRule( Target.Element, MainSourceRule.Target );

                            if ( TargetRule == null )
                            {
                                throw new MissingMappingException( $"Missing mapping for entity {Target.GetName()}" );
                            }

                            bool IsRootMultivalued = TargetRule.BelongsToMultivaluedAttribute();

                            // Store fields to be added
                            Dictionary<string, JSCode> AddedTargetAttributes = new Dictionary<string, JSCode>();

                            string RootAttribute = TargetRule.GetRootAttribute();

                            if ( RootAttribute == null )
                            {
                                // This is a 1:1 relation with target attributes scattered through source entity
                                Dictionary<string, JSCode> AddTargetAttributes = new Dictionary<string, JSCode>();

                                foreach ( DataAttribute Attribute in Target.Element.Attributes )
                                {
                                    string RuleValue = TargetRule.GetRuleValueForAttribute( Attribute );
                                    if ( string.IsNullOrWhiteSpace( RuleValue ) )
                                    {
                                        continue;
                                    }

                                    AddTargetAttributes.Add( $"\"data_{Target.GetName()}.{Target.GetName()}_{Attribute.Name}\"", new JSString( $"\"${RuleValue}\"" ) );

                                    AttributesToRemove.Add( RuleValue );
                                }

                                AttributesToMergeWithRelatioshipData.Add( $"data_{Target.GetName()}" );
                                AttributesToRemove.Add( $"data_{Target.GetName()}" );

                                AddFieldsOperator AddTargetFieldsOp = new AddFieldsOperator( AddTargetAttributes );
                                OperationsToExecute.Add( AddTargetFieldsOp );
                            }
                            else
                            {
                                AttributesToRemove.Add( RootAttribute );

                                if ( IsRootMultivalued )
                                {
                                    // Use a map expression to rename attributes
                                    // within an add attributes expression
                                    Dictionary<string, JSCode> MapParams = new Dictionary<string, JSCode>();
                                    string MapAttributeAs = $"data_{RootAttribute}";

                                    foreach ( DataAttribute Attribute in Target.Element.Attributes )
                                    {
                                        string RuleValue = TargetRule.GetRuleValueForAttribute( Attribute );

                                        if ( string.IsNullOrWhiteSpace( RuleValue ) )
                                        {
                                            continue;
                                        }

                                        string[] RulePath = RuleValue.Split( new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries );

                                        MapParams.Add( $"\"{Target.GetName()}_{Attribute.Name}\"", new JSString( $"\"$${MapAttributeAs}.{string.Join( ".", RulePath.Skip( 1 ) )}\"" ) );
                                    }

                                    MapExpr MapTargetExpr = new MapExpr( RootAttribute, MapAttributeAs, MapParams );
                                    AddedTargetAttributes.Add( $"data_{Target.GetName()}", MapTargetExpr.ToJSCode() );
                                    AttributesToConcatWithRelationshipData.Add( $"data_{Target.GetName()}" );
                                }
                                else
                                {
                                    // Use add attributes
                                    foreach ( DataAttribute Attribute in Target.Element.Attributes )
                                    {
                                        string RuleValue = TargetRule.GetRuleValueForAttribute( Attribute );

                                        if ( string.IsNullOrWhiteSpace( RuleValue ) )
                                        {
                                            continue;
                                        }

                                        AddedTargetAttributes.Add( $"\"data_{Target.GetName()}.{Target.GetName()}_{Attribute.Name}\"", new JSString( $"\"${RuleValue}\"" ) );
                                    }

                                    AttributesToMergeWithRelatioshipData.Add( $"data_{Target.GetName()}" );
                                    AttributesToRemove.Add( $"data_{Target.GetName()}" );
                                }

                                AddFieldsOperator AddTargetAttributesOp = new AddFieldsOperator( AddedTargetAttributes );

                                OperationsToExecute.Add( AddTargetAttributesOp );
                            }
                        }
                        else
                        {
                            // This basicaly means a 1:N or 1:1 but the target isn't embedded
                            // Check for 1:N first
                            MapRule SourceRuleAtTarget = ModelMap.FindRule( SourceEntity.Element, MainTargetRule.Target );

                            if ( SourceRuleAtTarget == null )
                            {
                                // Then it should be a 1:1
                                // Target must have a map pointing at SourceCollection
                                MapRule TargetRuleAtSource = ModelMap.FindRule( Target.Element, MainSourceRule.Target );

                                if ( TargetRuleAtSource == null )
                                {
                                    throw new MissingMappingException( $"Target entity {Target.GetName()} must have a map rule pointing to {MainSourceRule.Target}" );
                                }

                                // This could still be a 1:N relation if the target is mapped to an array
                                // but there are several ways to represent this and we should set up some sort of standard mapping
                                // for this

                                string TargetAs = $"data_{Target.GetName()}";

                                // Assuming 1:1
                                LookupOperator TargetLookupOp = new LookupOperator()
                                {
                                    From = MainTargetRule.Target.Name,
                                    LocalField = TargetRuleAtSource.GetRuleValueForAttribute( Target.Element.GetIdentifierAttribute() ),
                                    ForeignField = MainTargetRule.GetRuleValueForAttribute( Target.Element.GetIdentifierAttribute() ),
                                    As = TargetAs
                                };

                                // Unwind joined data
                                UnwindOperator UnwindTargetOp = new UnwindOperator( TargetAs );

                                // TODO: As a 1:1 we should merge with it relationship attributes

                                // Add to execution list
                                OperationsToExecute.AddRange( new MongoDBOperator[] { TargetLookupOp, UnwindTargetOp } );
                            }
                            else
                            {
                                // This is a 1:N
                                string TargetAs = $"data_{Target.GetName()}_join";

                                LookupOperator TargetLookupOp = new LookupOperator()
                                {
                                    From = MainTargetRule.Target.Name,
                                    ForeignField = SourceRuleAtTarget.GetRuleValueForAttribute( SourceEntity.Element.GetIdentifierAttribute() ),
                                    LocalField = MainSourceRule.GetRuleValueForAttribute( SourceEntity.Element.GetIdentifierAttribute() ),
                                    As = TargetAs
                                };

                                // Add Lookup to execution list
                                OperationsToExecute.Add( TargetLookupOp );

                                // Rename attributes
                                // Use a map expression as it is an array
                                Dictionary<string, JSCode> RenamedTargetAttributes = new Dictionary<string, JSCode>();
                                string MapAttributeAs = $"data_{Target.GetName()}";

                                foreach ( DataAttribute Attribute in Target.Element.Attributes )
                                {
                                    string RuleValue = MainTargetRule.GetRuleValueForAttribute( Attribute );
                                    if ( string.IsNullOrWhiteSpace( RuleValue ) )
                                    {
                                        continue;
                                    }

                                    RenamedTargetAttributes.Add( $"\"{Target.GetName()}_{Attribute.Name}\"", new JSString( $"\"$${MapAttributeAs}.{RuleValue}\"" ) );
                                }

                                // Check if there is any relationship attribute here
                                if ( Relationship.Attributes.Count > 0 )
                                {
                                    MapRule RelationshipRuleToTarget = ModelMap.FindRule( Relationship, MainTargetRule.Target );

                                    if ( RelationshipRuleToTarget != null )
                                    {
                                        Dictionary<string, JSCode> RelationshipAttributesToAdd = new Dictionary<string, JSCode>();

                                        foreach ( DataAttribute Attribute in Relationship.Attributes )
                                        {
                                            string RuleValue = RelationshipRuleToTarget.GetRuleValueForAttribute( Attribute );

                                            if ( string.IsNullOrWhiteSpace( RuleValue ) )
                                            {
                                                continue;
                                            }

                                            RelationshipAttributesToAdd.Add( $"\"data_{Relationship.Name}_attributes.{Attribute.Name}\"",
                                                new JSString( $"$\"{TargetAs}.{RuleValue}\"" ) );
                                        }

                                        if ( RelationshipAttributesToAdd.Count > 0 )
                                        {
                                            AddFieldsOperator AddRelationshipAttributesOp = new AddFieldsOperator( RelationshipAttributesToAdd );
                                            OperationsToExecute.Add( AddRelationshipAttributesOp );
                                        }
                                    }
                                }

                                MapExpr TargetAttributeMap = new MapExpr( TargetAs, MapAttributeAs, RenamedTargetAttributes );
                                Dictionary<string, JSCode> AddFieldsDictionary = new Dictionary<string, JSCode>();
                                AddFieldsDictionary.Add( $"data_{Target.GetName()}", TargetAttributeMap.ToJSCode() );

                                AddFieldsOperator AddRenamedTargetOp = new AddFieldsOperator( AddFieldsDictionary );
                                ProjectOperator HideOldTargetOp = ProjectOperator.HideAttributesOperator( new string[] { TargetAs } );

                                // Add to merge list
                                AttributesToMergeWithRelatioshipData.Add( $"data_{Target.GetName()}" );

                                // Add operations to list
                                OperationsToExecute.AddRange( new MongoDBOperator[] { AddRenamedTargetOp, HideOldTargetOp } );
                            }
                        }
                    }
                }

                // Check if there are any attributes to be merged with relationship data
                if ( AttributesToMergeWithRelatioshipData.Count > 0 )
                {
                    // Include an add fields operator merging them
                    Dictionary<string, JSCode> AddMergedData = new Dictionary<string, JSCode>();
                    MergeObjectsOperator MergeFieldsOp = new MergeObjectsOperator( AttributesToMergeWithRelatioshipData );
                    AddMergedData.Add( $"data_{Relationship.Name}", new JSArray( new List<object> { MergeFieldsOp.ToJSCode() } ) );

                    AddFieldsOperator AddMergedDataOp = new AddFieldsOperator( AddMergedData );

                    OperationsToExecute.Add( AddMergedDataOp );
                }

                // Check if there are any attributes to remove
                if ( AttributesToRemove.Count > 0 )
                {
                    ProjectOperator HideAttributesOp = ProjectOperator.HideAttributesOperator( AttributesToRemove );
                    OperationsToExecute.Add( HideAttributesOp );
                }
            }

            // Return operations
            return new AlgebraOperatorResult( OperationsToExecute );
        }
        /// <summary>
        /// Process a computed entity
        /// </summary>
        /// <param name="TargetEntity">Target entity to process</param>
        /// <returns></returns>
        private List<MongoDBOperator> ProcessComputedEntity( ComputedEntity TargetEntity )
        {
            // Create a new RJOIN operator instance
            RelationshipJoinOperator CEJoinOperator = new RelationshipJoinOperator( TargetEntity.SourceEntity, TargetEntity.Relationship,
                TargetEntity.TargetEntities, ModelMap );

            // Run it and return the operations
            AlgebraOperatorResult CEResult = CEJoinOperator.Run();

            // Find main rule for source entity
            MapRule MainTargetRule = ModelMap.FindMainRule( TargetEntity.SourceEntity.Element );

            Dictionary<string, JSCode> RenamedAttributes = new Dictionary<string, JSCode>();
            Entity TargetSourceEntity = TargetEntity.SourceEntity.GetEntity();

            List<string> AttributesToRemove = new List<string>();

            // Rename source entity attributes
            foreach ( DataAttribute Attribute in TargetEntity.SourceEntity.Element.Attributes )
            {
                string RuleValue = MainTargetRule.GetRuleValueForAttribute( Attribute );

                if ( RuleValue == null )
                {
                    continue;
                }

                RenamedAttributes.Add( $"{TargetSourceEntity.Name}_{Attribute.Name}", new JSString( $"\"${RuleValue}\"" ) );
                AttributesToRemove.Add( RuleValue );
            }

            if ( RenamedAttributes.Count > 0 )
            {
                AddFieldsOperator AddRenamedAttributesOp = new AddFieldsOperator( RenamedAttributes );
                CEResult.Commands.Add( AddRenamedAttributesOp );

                // Hide old attributes
                ProjectOperator HideOldOp = ProjectOperator.HideAttributesOperator( AttributesToRemove );
                CEResult.Commands.Add( HideOldOp );
            }

            return CEResult.Commands;
        }
        /// <summary>
        /// Computes the virtual map after executing this instance
        /// </summary>
        /// <returns></returns>
        public override VirtualMap ComputeVirtualMap( VirtualMap ExistingVirtualMap = null )
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Compute the virtual map of a ComputedEntity
        /// </summary>
        /// <param name="RootAttribute"></param>
        /// <param name="TargetEntity"></param>
        private List<VirtualRule> ComputeCEVirtualMap( string RootAttribute, ComputedEntity TargetEntity )
        {
            // Rule list to return
            List<VirtualRule> RuleList = new List<VirtualRule>();
            // Shorten path to entity
            Entity CurrentEntity = (Entity)TargetEntity.SourceEntity.Element;
            // Create rule for the base entity
            VirtualRule VirtualEntityRule = new VirtualRule( TargetEntity.SourceEntity.Element, TargetEntity.SourceEntity.Alias );
            // Iterate it's attributes
            foreach ( DataAttribute Attribute in CurrentEntity.Attributes )
            {
                VirtualEntityRule.AddRule( Attribute.Name, $"{RootAttribute}.{CurrentEntity.Name}_{Attribute.Name}" );
            }
            // Add to list
            RuleList.Add( VirtualEntityRule );

            // Set new root attribute for relationship and joining entities
            string NewRootAttribute = $"{RootAttribute}.data_{TargetEntity.Relationship.Name}";
            // Process relationship
            VirtualRule RelationshipVirtualRule = new VirtualRule( TargetEntity.Relationship );
            foreach ( DataAttribute Attribute in TargetEntity.Relationship.Attributes )
            {
                RelationshipVirtualRule.AddRule( Attribute.Name, $"{NewRootAttribute}.{TargetEntity.Relationship.Name}_{Attribute.Name}" );
            }
            // Add to list
            RuleList.Add( RelationshipVirtualRule );

            // Process additional entities
            foreach ( QueryableEntity Target in TargetEntity.TargetEntities )
            {
                if ( Target.Element is ComputedEntity TargetAsCE )
                {
                    RuleList.AddRange( ComputeCEVirtualMap( NewRootAttribute, TargetAsCE ) );
                }
                else
                {
                    // Rules
                    VirtualRule TargetEntityVirtualRule = new VirtualRule( Target.Element, Target.Alias );
                    // Process attributes
                    foreach ( DataAttribute Attribute in Target.Element.Attributes )
                    {
                        TargetEntityVirtualRule.AddRule( Attribute.Name, $"{NewRootAttribute}.{Target.Element.Name}_{Attribute.Name}" );
                    }
                    // Add to list
                    RuleList.Add( TargetEntityVirtualRule );
                }
            }

            return RuleList;
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Initialize a new instance of RelationshipJoinOperator
        /// </summary>
        /// <param name="SourceEntity"></param>
        /// <param name="Map"></param>
        public RelationshipJoinOperator( QueryableEntity SourceEntity, Relationship Relationship, List<QueryableEntity> TargetEntities, ModelMapping Map ) : base( Map )
        {
            this.SourceEntity = SourceEntity;
            this.Relationship = Relationship;
            this.TargetEntities = TargetEntities;
        }

        public RelationshipJoinOperator( QueryableEntity SourceEntity, Relationship Relationship, string RelationshipAlias, List<QueryableEntity> TargetEntities, ModelMapping Map ) : base( Map )
        {
            this.SourceEntity = SourceEntity;
            this.Relationship = Relationship;
            this.TargetEntities = TargetEntities;
            this.RelationshipAlias = RelationshipAlias;
        }
        #endregion
    }
}
