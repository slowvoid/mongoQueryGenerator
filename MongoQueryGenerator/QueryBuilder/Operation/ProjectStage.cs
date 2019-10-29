using QueryBuilder.Map;
using QueryBuilder.Mongo.Aggregation.Operators;
using QueryBuilder.Mongo.Expressions;
using QueryBuilder.Operation.Arguments;
using QueryBuilder.Operation.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueryBuilder.Operation
{
    /// <summary>
    /// Represents a PROJECT operation
    /// 
    /// CAVEATS:
    ///     _id MUST be explicit removed otherwhise it will remain in the result regardless of the existence of a project (attr:true) stage
    /// </summary>
    public class ProjectStage : AlgebraOperator
    {
        #region Properties
        /// <summary>
        /// List of arguments for this stage
        /// </summary>
        public IEnumerable<ProjectArgument> Arguments { get; set; }
        /// <summary>
        /// Map between ER model and current output
        /// </summary>
        public IModelMap Map { get; set; }
        #endregion

        #region Methods
        /// <summary>
        /// Run the operation adding MongoDB operators to the pipeline
        /// </summary>
        /// <param name="LastResult"></param>
        /// <returns></returns>
        public override AlgebraOperatorResult Run()
        {
            // Store operators to run
            List<MongoDBOperator> OperatorsToExecute = new List<MongoDBOperator>();
            // All we need to do is find the correct map for each attribute
            // Argument already provides which expression to apply
            Dictionary<string, ProjectExpression> AttributesAndExpressions = new Dictionary<string, ProjectExpression>();
            // Iterate all arguments
            foreach ( ProjectArgument Argument in Arguments )
            {
                // Each argument provides all necessary data
                string AttributeMap = Map.GetRuleValue( Argument.ParentEntity.Alias ?? Argument.ParentEntity.Name(), Argument.Attribute.Name );

                if ( string.IsNullOrWhiteSpace( AttributeMap ) )
                {
                    continue;
                }

                // Add to attribute list
                // Including quotation marks to prevent trouble with dot notation
                AttributesAndExpressions.Add( $"\"{AttributeMap}\"", Argument.Expression );
            }
            // Create project operator
            ProjectOperator ProjectOp = new ProjectOperator( AttributesAndExpressions );
            // Add to execution list
            // TODO: This process can be simplified to a single ProjectOperator
            OperatorsToExecute.Add( ProjectOp );
            // Return operators
            return new AlgebraOperatorResult( OperatorsToExecute );
        }
        /// <summary>
        /// Generates a virtual map for this operator
        /// </summary>
        /// <param name="ExistingVirtualMap"></param>
        /// <returns></returns>
        public override VirtualMap ComputeVirtualMap(VirtualMap ExistingVirtualMap = null)
        {
            // Create a list to store the new rules
            List<VirtualRule> VirtualRules = new List<VirtualRule>();

            // Store value wheter this stage is adding or forcing a field to be visible
            bool? IsAddingOrForcingAFieldToBeVisible = null;

            // This operation will fetch the attributes from arguments
            // which can cause a smaller or bigger list than the previous operator
            foreach ( ProjectArgument Argument in Arguments )
            {
                // Create new Virtual Rule
                VirtualRule AttributeRule = new VirtualRule( Argument.ParentEntity.Element, Argument.ParentEntity.Alias );

                // Check if the expression is adding or forcing a field to be visible
                if ( !IsAddingOrForcingAFieldToBeVisible.HasValue && Argument.Expression.IsAddingOrForcingAFieldVisible )
                {
                    IsAddingOrForcingAFieldToBeVisible = true;
                }

                string AttributeMap = Map.GetRuleValue( Argument.ParentEntity.Alias ?? Argument.ParentEntity.Name(), Argument.Attribute.Name );
                if ( string.IsNullOrWhiteSpace( AttributeMap ) )
                {
                    continue;
                }

                AttributeRule.AddRule( Argument.Attribute.Name, AttributeMap );

                // Add to attribute list
                VirtualRules.Add( AttributeRule );
            }

            // This stage cannot increment a previous VirtualMap if it is adding new fields / forcing fields to be visible [or have a value]
            // otherwise the whole document is defined within the arguments
            if ( ( IsAddingOrForcingAFieldToBeVisible.HasValue && IsAddingOrForcingAFieldToBeVisible.Value ) || ExistingVirtualMap == null )
            {
                // Create a new Virtual Map instance to receive the new rules
                return new VirtualMap( VirtualRules );            
            }

            List<VirtualRule> RulesToReturn = new List<VirtualRule>();

            foreach ( VirtualRule Rule in ExistingVirtualMap.Rules )
            {
                foreach ( VirtualRule RuleToRemove in VirtualRules )
                {
                    VirtualRule ReturnRule = new VirtualRule( Rule.SourceERElement, Rule.Alias );

                    if ( Rule.SourceERElement.Name == RuleToRemove.SourceERElement.Name &&
                            Rule.Alias == RuleToRemove.Alias )
                    {
                        foreach ( KeyValuePair<string, string> InternalRules in Rule.Rules )
                        {
                            if ( !RuleToRemove.Rules.ContainsKey( InternalRules.Key ) )
                            {
                                ReturnRule.AddRule( InternalRules.Key, InternalRules.Value );
                            }
                        }

                        RulesToReturn.Add( ReturnRule );
                    }
                    else
                    {
                        RulesToReturn.Add( Rule );
                    }
                }
            }

            return new VirtualMap( RulesToReturn );
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Initialize a new instance of ProjectStage class
        /// </summary>
        /// <param name="Arguments"></param>
        /// <param name="Map"></param>
        public ProjectStage( IEnumerable<ProjectArgument> Arguments, IModelMap Map )
        {
            this.Arguments = Arguments;
            this.Map = Map;
        }
        #endregion
    }
}
