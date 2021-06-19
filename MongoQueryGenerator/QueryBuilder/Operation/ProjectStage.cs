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

        override public string SummarizeToString()
        {
            return "ProjectStage (" + 
            string.Join(", ", Arguments.ToList().Select(arg => arg.SummarizeToString()))
            +")";
        }


        /// <summary>
        /// Run the operation adding MongoDB operators to the pipeline
        /// </summary>
        /// <param name="LastResult"></param>
        /// <returns></returns>
        public override AlgebraOperatorResult Run( IModelMap inMap )
        {
            if ( inMap is VirtualMap )
            {
                Map = inMap;
                RuleMap = inMap;
            }
            // Store operators to run
            List<MongoDBOperator> OperatorsToExecute = new List<MongoDBOperator>();
            // All we need to do is find the correct map for each attribute
            // Argument already provides which expression to apply
            Dictionary<string, ProjectExpression> AttributesAndExpressions = new Dictionary<string, ProjectExpression>();
            // Iterate all arguments
            foreach ( ProjectArgument Argument in Arguments )
            {
                // Each argument provides all necessary data
                string AttributeMap = Map.GetRuleValue( Argument.ParentEntity.Alias ?? Argument.ParentEntity.GetName(), Argument.Attribute.Name );

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
            // If the virtual map is not set we will use the IModelObject object
            if ( ExistingVirtualMap == null && Map is ModelMapping)
            {
                ExistingVirtualMap = VirtualMap.FromModelMap( (ModelMapping)Map );
            }

            // Store new rules
            List<VirtualRule> NewRules = new List<VirtualRule>();

            // Iterate arguments and keep rules that are either a BooleanExpr(true) or define a value to an attribute
            foreach ( ProjectArgument Argument in Arguments.Where( Arg => Arg.Expression.IsAddingOrForcingAFieldVisible ) )
            {
                // Check if rule already exists and update it
                VirtualRule ElementRule = NewRules.Find( R => R.SourceERElement.Name == Argument.ParentEntity.GetName() &&
                    R.Alias == Argument.ParentEntity.Alias );

                string RuleValue = ExistingVirtualMap.GetRuleValue( Argument.ParentEntity.GetAliasOrName(), Argument.Attribute.Name );

                if ( ElementRule == null )
                {
                    // Create new entry
                    VirtualRule ArgRule = new VirtualRule( Argument.ParentEntity.Element, Argument.ParentEntity.Alias );
                    ArgRule.Rules.Add( Argument.Attribute.Name, RuleValue );

                    NewRules.Add( ArgRule );
                }
                else
                {
                    ElementRule.Rules.Add( Argument.Attribute.Name, RuleValue );
                }
            }

            return new VirtualMap( NewRules );
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
