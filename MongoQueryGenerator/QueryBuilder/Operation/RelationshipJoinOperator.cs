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


        override public string SummarizeToString()
        {
            string Ret = "RelationshipJoinOperator ";

            Ret += "<"+Relationship.Name+"> ";
            Ret += "Source="+SourceEntity.SummarizeToString()+" ";
            Ret += "Target={ "+ string.Join(", ", TargetEntities.Select(te => te.SummarizeToString())) + " }";

            return Ret;
        }

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

                    // Add to pipeline
                    PipelineOperators.Add( AddRelationshipFieldsOp );
                }

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

                        // Try to hide attributes that are related to the relationship (usually entity identifiers)
                        // Add them to removal list

                        foreach ( DataAttribute Attribute in MainRelationshipRule.Target.DocumentSchema.Attributes )
                        {
                            RelationshipAttributesToHide.Add( Attribute.Name );
                        }

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
                            // Check if the target entity is embedded to the intermediate collection
                            MapRule TargetRelationshipRule = ModelMap.FindRule( Target.Element, MainRelationshipRule.Target );

                            if ( TargetRelationshipRule == null )
                            {
                                throw new MissingMappingException( $"Target entity {Target.GetName()} must have either a main mapping or be embedded into the intermediate collection" );
                            }

                            Dictionary<string, JSCode> RenamedTargetAttributes = new Dictionary<string, JSCode>();
                            List<string> TargetAttributesToRemove = new List<string>();

                            // This is a simple thing, just rename the attributes
                            foreach ( DataAttribute Attribute in Target.Element.Attributes )
                            {
                                string RuleValue = TargetRelationshipRule.GetRuleValueForAttribute( Attribute );
                                if ( RuleValue == null )
                                {
                                    continue;
                                }

                                string[] RulePath = RuleValue.Split( new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries );
                                if ( RulePath.Length > 1 )
                                {
                                    // Add root to removal list
                                    TargetAttributesToRemove.Add( RulePath.First() );
                                }
                                else
                                {
                                    TargetAttributesToRemove.Add( RuleValue );
                                }

                                RenamedTargetAttributes.Add( $"{Target.GetName()}_{Attribute.Name}", new JSString( $"\"${RuleValue}\"" ) );
                            }

                            if ( RenamedTargetAttributes.Count > 0 )
                            {
                                // Hide unwanted attributes from relationship (or already used and renamed)
                                TargetAttributesToRemove.Add( SourceRule.GetRuleValueForAttribute( SourceEntity.Element.GetIdentifierAttribute() ) );

                                AddFieldsOperator AddTargetFieldsOp = new AddFieldsOperator( RenamedTargetAttributes );
                                ProjectOperator HideTargetFieldsOp = ProjectOperator.HideAttributesOperator( TargetAttributesToRemove.Distinct() );

                                PipelineOperators.AddRange( new MongoDBOperator[] { AddTargetFieldsOp, HideTargetFieldsOp } );
                            }
                        }
                        else
                        {
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

                // Hide relationship attributes last
                if ( RelationshipAttributesToAdd.Count > 0 )
                {
                    // hide original fields
                    ProjectOperator HideRelationshipAttributesOp = ProjectOperator.HideAttributesOperator( RelationshipAttributesToHide );

                    PipelineOperators.Add( HideRelationshipAttributesOp );
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
                bool AreRelationshipAttributesReady = false;

                // Iterate targets
                foreach ( QueryableEntity Target in TargetEntities )
                {
                    if ( Target.Element is ComputedEntity )
                    {
                        ComputedEntity TargetComputedEntity = Target.GetComputedEntity();

                        // As this is a computed entity, the ce source entity cannot be embedded
                        MapRule MainTargetRule = ModelMap.FindMainRule( TargetComputedEntity.SourceEntity.Element );

                        if ( MainTargetRule == null )
                        {
                            throw new MissingMappingException( $"Entity {TargetComputedEntity.SourceEntity.GetName()} must have a main mapping to be the source entity of a computed entity" );
                        }

                        // This could be a 1:1 or 1:N find

                        // Check for 1:N first
                        MapRule SourceRuleAtTarget = ModelMap.FindRule( SourceEntity.Element, MainTargetRule.Target );

                        if ( SourceRuleAtTarget == null )
                        {
                            DataAttribute TargetIdentifier = TargetComputedEntity.SourceEntity.Element.GetIdentifierAttribute();
                            // It should be a 1:1 relation
                            MapRule TargetRuleAtSource = ModelMap.FindRule( TargetComputedEntity.SourceEntity.Element, MainSourceRule.Target );

                            if ( TargetRuleAtSource == null )
                            {
                                throw new MissingMappingException( $"No suitable mapping found for {TargetComputedEntity.SourceEntity.GetName()}" );
                            }

                            // As this is a computed entity we need a custom pipeline to properly continue the operation
                            List<MongoDBOperator> CELookupPipeline = new List<MongoDBOperator>();

                            // Setup pipeline variables
                            Dictionary<string, string> CELookupVariables = new Dictionary<string, string>();
                            string VariableMatchValue = TargetRuleAtSource.GetRuleValueForAttribute( TargetIdentifier );
                            string VariableMatchKey = $"source_{TargetIdentifier.Name}";
                            CELookupVariables.Add( VariableMatchKey, $"${VariableMatchValue}" );

                            // Create a match operator for the target entity (actually the target ce source entity)
                            MatchOperator MatchTargetOp = MatchOperator.CreateLookupMatch( MainTargetRule.GetRuleValueForAttribute( TargetIdentifier ), VariableMatchKey );
                            // Add to pipeline
                            CELookupPipeline.Add( MatchTargetOp );

                            LookupOperator TargetLookupOp = new LookupOperator()
                            {
                                From = MainTargetRule.Target.Name,
                                Let = CELookupVariables,
                                Pipeline = CELookupPipeline,
                                As = $"data_{TargetComputedEntity.SourceEntity.GetName()}"
                            };

                            UnwindOperator UnwindCELookup = new UnwindOperator( TargetLookupOp.As );

                            // TODO: fetch relationship attributes (if any)

                            // Process ce targets
                            List<MongoDBOperator> CEOperators = ProcessComputedEntity( TargetComputedEntity );
                            
                            CELookupPipeline.AddRange( CEOperators );
                            OperationsToExecute.AddRange( new MongoDBOperator[] { TargetLookupOp, UnwindCELookup } );

                            AttributesToMergeWithRelatioshipData.Add( TargetLookupOp.As );
                            AttributesToRemove.Add( TargetLookupOp.As );
                        }
                        else
                        {
                            DataAttribute SourceIdentifier = SourceEntity.Element.GetIdentifierAttribute();
                            // This is a 1:N
                            // As this is a computed entity we need a custom pipeline to properly continue the operation
                            List<MongoDBOperator> CELookupPipeline = new List<MongoDBOperator>();

                            // Setup pipeline variables
                            Dictionary<string, string> CELookupVariables = new Dictionary<string, string>();
                            string VariableMatchValue = MainSourceRule.GetRuleValueForAttribute( SourceIdentifier );
                            string VariableMatchKey = $"source_{SourceIdentifier.Name}";
                            CELookupVariables.Add( VariableMatchKey, $"${VariableMatchValue}" );

                            // Create a match operator for the target entity (actually the target ce source entity)
                            MatchOperator MatchTargetOp = MatchOperator.CreateLookupMatch( SourceRuleAtTarget.GetRuleValueForAttribute( SourceIdentifier ), VariableMatchKey );
                            // Add to pipeline
                            CELookupPipeline.Add( MatchTargetOp );

                            LookupOperator TargetLookupOp = new LookupOperator()
                            {
                                From = MainTargetRule.Target.Name,
                                Let = CELookupVariables,
                                Pipeline = CELookupPipeline,
                                As = $"data_{TargetComputedEntity.SourceEntity.GetName()}"
                            };

                            UnwindOperator UnwindCELookup = new UnwindOperator( TargetLookupOp.As );

                            // TODO: fetch relationship attributes (if any)


                            // Hide source attributes mapped to target
                            // And inject it to incoming CEOperators
                            List<string> HideSourceAttributesAtTarget = new List<string>();
                            
                            foreach ( KeyValuePair<string,string> AttributeRule in SourceRuleAtTarget.Rules )
                            {
                                HideSourceAttributesAtTarget.Add( AttributeRule.Value );
                            }

                            ProjectOperator HideSourceAttributesAtTargetOp = ProjectOperator.HideAttributesOperator( HideSourceAttributesAtTarget );

                            // Process ce targets
                            List<MongoDBOperator> CEOperators = ProcessComputedEntity( TargetComputedEntity );

                            CEOperators.Add( HideSourceAttributesAtTargetOp );

                            CELookupPipeline.AddRange( CEOperators );
                            OperationsToExecute.AddRange( new MongoDBOperator[] { TargetLookupOp } );

                            // Computed entities are a complex matter, mixing them up with relationships with cardinality such as 1:1 and 1:N require
                            // some special rules
                            // Set data_RelationshipName now, if trying to join multiple computed entities at the same time, only the last one will appear
                            Dictionary<string, JSCode> FinalAttributes = new Dictionary<string, JSCode>();
                            FinalAttributes.Add( $"data_{Relationship.Name}", new JSString( $"\"${TargetLookupOp.As}\"" ) );

                            AddFieldsOperator FinalCEAttributesOp = new AddFieldsOperator( FinalAttributes );
                            OperationsToExecute.Add( FinalCEAttributesOp );

                            AttributesToRemove.Add( TargetLookupOp.As );
                        }
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
                            string RelationshipRootAttribute = null;
                            if ( RelationshipRule != null )
                            {
                                RelationshipRootAttribute = RelationshipRule.GetRootAttribute();
                            }

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

                                    // Check if relationship attributes are within the target data
                                    if ( RelationshipRootAttribute == RootAttribute )
                                    {
                                        // Then relationship data is located within target attributes
                                        // Add them to the rename process
                                        foreach ( DataAttribute Attribute in Relationship.Attributes )
                                        {
                                            string RuleValue = RelationshipRule.GetRuleValueForAttribute( Attribute );

                                            if ( string.IsNullOrEmpty( RuleValue ) )
                                            {
                                                continue;
                                            }

                                            string[] RulePath = RuleValue.Split( new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries );

                                            MapParams.Add( $"\"{Relationship.Name}_{Attribute.Name}\"", new JSString( $"\"$${MapAttributeAs}.{string.Join( ".", RulePath.Skip( 1 ) )}\"" ) );
                                        }

                                        AreRelationshipAttributesReady = true;
                                    }

                                    MapExpr MapTargetExpr = new MapExpr( RootAttribute, MapAttributeAs, MapParams );
                                    // Using data_RelationshipName because we cant match multiple arrays, if this is a OneToMany relationship
                                    // we cant combine multiple entities together
                                    AddedTargetAttributes.Add( $"data_{Relationship.Name}", MapTargetExpr.ToJSCode() );
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
                                    throw new MissingMappingException( $"Target entity {Target.GetName()} must have a map rule pointing to {MainSourceRule.Target.Name}" );
                                }

                                // This could still be a 1:N relation if the target is mapped to an array
                                // but there are several ways to represent this and we should set up some sort of standard mapping
                                // for this

                                string TargetAs = $"data_{Target.GetName()}_join";
                                string TargetData = $"data_{Target.GetName()}";

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

                                Dictionary<string, JSCode> AddTargetAttributes = new Dictionary<string, JSCode>();

                                // Rename attributes
                                foreach ( DataAttribute Attribute in Target.Element.Attributes )
                                {
                                    string RuleValue = MainTargetRule.GetRuleValueForAttribute( Attribute );

                                    if ( string.IsNullOrWhiteSpace( RuleValue ) )
                                    {
                                        continue;
                                    }

                                    AddTargetAttributes.Add( $"\"{TargetData}.{Target.GetName()}_{Attribute.Name}\"", new JSString( $"\"${TargetAs}.{RuleValue}\"" ) );
                                }

                                // Check if relationship attributes are mapped within the target attributes
                                MapRule RelationshipRuleAtTarget = ModelMap.FindRule( Relationship, MainTargetRule.Target );

                                if ( RelationshipRuleAtTarget != null )
                                {
                                    // Iterate attributes and add them to the proper place
                                    foreach ( DataAttribute Attribute in Relationship.Attributes )
                                    {
                                        string RuleValue = RelationshipRuleAtTarget.GetRuleValueForAttribute( Attribute );
                                        if ( string.IsNullOrWhiteSpace( RuleValue ) )
                                        {
                                            continue;
                                        }

                                        AddTargetAttributes.Add( $"\"data_{Relationship.Name}.{Relationship.Name}_{Attribute.Name}\"", new JSString( $"\"${TargetAs}.{RuleValue}\"" ) );
                                    }

                                    // Add to merge list
                                    AttributesToMergeWithRelatioshipData.Add( $"data_{Relationship.Name}" );
                                    AreRelationshipAttributesReady = true;
                                }
                                

                                // Add joined data to a proper attribute
                                AddFieldsOperator AddTargetAtttibutesOp = new AddFieldsOperator( AddTargetAttributes );

                                // As a 1:1 we should merge with it relationship attributes
                                AttributesToMergeWithRelatioshipData.Add( TargetData );
                                AttributesToRemove.AddRange( new string[] { TargetData, TargetAs } );

                                // Add to execution list
                                OperationsToExecute.AddRange( new MongoDBOperator[] { TargetLookupOp, UnwindTargetOp, AddTargetAtttibutesOp } );
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
                                        foreach ( DataAttribute Attribute in Relationship.Attributes )
                                        {
                                            string RuleValue = RelationshipRuleToTarget.GetRuleValueForAttribute( Attribute );

                                            if ( string.IsNullOrWhiteSpace( RuleValue ) )
                                            {
                                                continue;
                                            }

                                            RenamedTargetAttributes.Add( $"\"{Relationship.Name}_{Attribute.Name}\"", new JSString( $"\"$${MapAttributeAs}.{RuleValue}\"" ) );
                                        }

                                        AreRelationshipAttributesReady = true;
                                    }
                                }

                                MapExpr TargetAttributeMap = new MapExpr( TargetAs, MapAttributeAs, RenamedTargetAttributes );
                                Dictionary<string, JSCode> AddFieldsDictionary = new Dictionary<string, JSCode>();
                                AddFieldsDictionary.Add( $"data_{Target.GetName()}", TargetAttributeMap.ToJSCode() );

                                AddFieldsOperator AddRenamedTargetOp = new AddFieldsOperator( AddFieldsDictionary );
                                ProjectOperator HideOldTargetOp = ProjectOperator.HideAttributesOperator( new string[] { TargetAs } );

                                // Add to concat list
                                AttributesToConcatWithRelationshipData.Add( $"data_{Target.GetName()}" );

                                // Add to removal list
                                AttributesToRemove.Add( $"data_{Target.GetName()}" );

                                // Add operations to list
                                OperationsToExecute.AddRange( new MongoDBOperator[] { AddRenamedTargetOp, HideOldTargetOp } );
                            }
                        }
                    }
                }

                // Check if relationship data was already joined
                if ( !AreRelationshipAttributesReady )
                {
                    // Check if there are any
                    if ( Relationship.Attributes.Count > 0 )
                    {
                        // Fetch them, at this point they must be embedded into the source entity
                        MapRule RelationshipRuleAtSource = ModelMap.FindRule( Relationship, MainSourceRule.Target );
                        if ( RelationshipRuleAtSource == null )
                        {
                            throw new MissingMappingException( $"Relationship {Relationship.Name} has attributes but they are not mapped to a reachable collection." );
                        }

                        Dictionary<string, JSCode> RelationshipAttributesToAdd = new Dictionary<string, JSCode>();
                        List<string> RelationshipFieldsToHide = new List<string>();

                        // Iterate attributes
                        foreach ( DataAttribute Attribute in Relationship.Attributes )
                        {
                            string RuleValue = RelationshipRuleAtSource.GetRuleValueForAttribute( Attribute );
                            if ( string.IsNullOrWhiteSpace( RuleValue ) )
                            {
                                continue;
                            }

                            RelationshipAttributesToAdd.Add( $"\"data_{Relationship.Name}.{Relationship.Name}_{Attribute.Name}\"", new JSString( $"\"${RuleValue}\"" ) );
                            RelationshipFieldsToHide.Add( $"\"{RuleValue}\"" );
                        }

                        // Create add fields operator
                        AddFieldsOperator AddRelationshipFieldsOp = new AddFieldsOperator( RelationshipAttributesToAdd );

                        // Hide unmapped fields
                        ProjectOperator HideRelationshipFieldsOp = ProjectOperator.HideAttributesOperator( RelationshipFieldsToHide );

                        // Add to Execution list
                        OperationsToExecute.AddRange( new MongoDBOperator[] { AddRelationshipFieldsOp, HideRelationshipFieldsOp } );

                        // Add to merge list
                        AttributesToMergeWithRelatioshipData.Add( $"data_{Relationship.Name}" );
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

                // Check if there are any attributes to concat with relationship data
                if ( AttributesToConcatWithRelationshipData.Count > 0 )
                {
                    // Note that this actually build a weird array of data
                    // 1:N relationships cannot join multiple entities properly
                    Dictionary<string, JSCode> ConcatAttributes = new Dictionary<string, JSCode>();

                    ConcatArrayExpr ConcatExpr = new ConcatArrayExpr( AttributesToConcatWithRelationshipData );
                    ConcatAttributes.Add( $"data_{Relationship.Name}", ConcatExpr.ToJSCode() );

                    AddFieldsOperator ConcatAttributesIntoRelationshipOp = new AddFieldsOperator( ConcatAttributes );

                    OperationsToExecute.Add( ConcatAttributesIntoRelationshipOp );
                }

                // Check if there are any attributes to remove
                if ( AttributesToRemove.Count > 0 )
                {
                    ProjectOperator HideAttributesOp = ProjectOperator.HideAttributesOperator( AttributesToRemove );
                    OperationsToExecute.Add( HideAttributesOp );
                }
            }

            // Try to hide all attributes related to other entities that are mapped to the source entity
            List<MapRule> UnrelatedRules = ModelMap.Rules.FindAll( R => R.Target == MainSourceRule.Target && !R.IsMain );

            if ( UnrelatedRules.Count > 0 )
            {
                List<string> AttributesToBeHidden = new List<string>();
                
                foreach ( MapRule UnrelatedRule in UnrelatedRules )
                {
                    foreach ( string RuleValue in UnrelatedRule.Rules.Values )
                    {
                        // Check if it is an embedded attribute and add only the root
                        string[] RulePath = RuleValue.Split( new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries );

                        if ( RulePath.Count() > 1 )
                        {
                            AttributesToBeHidden.Add( $"\"{RulePath.First()}\"" );
                        }
                        else
                        {
                            AttributesToBeHidden.Add( $"\"{RuleValue}\"" );
                        }
                    }
                }

                if ( AttributesToBeHidden.Count > 0 )
                {
                    ProjectOperator HideUnrelatedAttributesOp = ProjectOperator.HideAttributesOperator( AttributesToBeHidden.Distinct() );
                    HideUnrelatedAttributesOp.ShouldExecuteLast = true;
                    OperationsToExecute.Add( HideUnrelatedAttributesOp );
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

                string AttributeKey = $"{TargetSourceEntity.Name}_{Attribute.Name}";
                // Check if key is already present
                if ( RenamedAttributes.ContainsKey( AttributeKey ) )
                {
                    continue;
                }

                RenamedAttributes.Add( AttributeKey, new JSString( $"\"${RuleValue}\"" ) );
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
            // The virtal map represents the output document after this operator is executed
            // To generate it we basically need to iterate over all targetted entities
            // but without the need to dive deep into how the operation is executed.
            List<VirtualRule> OperatorRules = new List<VirtualRule>();
            // Only the first entity requires to fetch data from ModelMap
            MapRule SourceRule = ModelMap.Rules.FirstOrDefault( Rule => Rule.Source.Name == SourceEntity.Element.Name && Rule.IsMain);
            // The source entity is not renamed, so we add it as is.
            VirtualRule SourceVirtualRule = new VirtualRule( SourceEntity.Element, SourceEntity.Alias );
            if ( SourceRule != null )
            {
                foreach ( DataAttribute Attribute in SourceEntity.Element.Attributes )
                {
                    // We only need the ModelMap for the origin entity
                    // all other entities are renamed and bound to this one (through relationships)
                    string AttributeRuleValue = SourceRule.Rules.FirstOrDefault( Rule => Rule.Key == Attribute.Name ).Value;

                    SourceVirtualRule.AddRule( Attribute.Name, AttributeRuleValue );
                }
            }

            // Add to rule list
            OperatorRules.Add( SourceVirtualRule );

            // Map relationship attributes
            // Attributes here are accesible through 'data_Relationship.Entity_Attribute'
            // Dot notation is mandatory as MongoDB accepts it as path to an attribute
            string RootAttribute = $"data_{Relationship.Name}";

            VirtualRule RelationshipVirtualRule = new VirtualRule( Relationship );
            // Process relationship attributes
            foreach ( DataAttribute Attribute in Relationship.Attributes )
            {
                RelationshipVirtualRule.AddRule( Attribute.Name, $"{RootAttribute}.{Relationship.Name}_{Attribute.Name}" );
            }

            // Add to list
            OperatorRules.Add( RelationshipVirtualRule );

            // Iterate each target entity
            foreach ( QueryableEntity Target in TargetEntities )
            {
                // Check if the target entity is computed
                if ( Target.Element is ComputedEntity TargetAsCE )
                {
                    OperatorRules.AddRange( ComputeCEVirtualMap( RootAttribute, TargetAsCE ) );
                }
                else
                {
                    // Create rule
                    VirtualRule VirtualEntityRule = new VirtualRule( Target.Element, Target.Alias );
                    // Parse attributes
                    foreach ( DataAttribute Attribute in Target.Element.Attributes )
                    {
                        VirtualEntityRule.AddRule( Attribute.Name, $"{RootAttribute}.{Target.Element.Name}_{Attribute.Name}" );
                    }

                    // Add to list
                    OperatorRules.Add( VirtualEntityRule );
                }
            }

            // If ExistingVirtualMap is not null
            // Append data
            if ( ExistingVirtualMap != null )
            {
                OperatorRules.AddRange( ExistingVirtualMap.Rules );
            }

            // When done
            VirtualMap OperatorMap = new VirtualMap( OperatorRules );
            return OperatorMap;
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
