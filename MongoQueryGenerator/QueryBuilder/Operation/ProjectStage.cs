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
                // Iterate Attributes and Expressions
                foreach ( KeyValuePair<string, ProjectExpression> Attribute in Argument.Attributes )
                {
                    string AttributeMap = Map.GetRuleValue( Argument.Element.Alias ?? Argument.Element.JoinedElement.Name, Attribute.Key );
                    // If the attribute map is null, we'll ignore it
                    if ( string.IsNullOrWhiteSpace( AttributeMap ) )
                    {
                        continue;
                    }

                    // Add to attribute list
                    // Including quotation marks to prevent trouble with dot notation
                    AttributesAndExpressions.Add( $"\"{AttributeMap}\"", Attribute.Value );
                }
            }
            // Create project operator
            ProjectOperator ProjectOp = new ProjectOperator( AttributesAndExpressions );
            // Add to execution list
            // TODO: This process can be simplified to a single ProjectOperator
            OperatorsToExecute.Add( ProjectOp );
            // Return operators
            return new AlgebraOperatorResult( OperatorsToExecute );
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
