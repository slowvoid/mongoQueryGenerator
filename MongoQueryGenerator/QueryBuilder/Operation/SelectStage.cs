using QueryBuilder.Map;
using QueryBuilder.Mongo.Aggregation.Operators;
using QueryBuilder.Mongo.Expressions;
using QueryBuilder.Operation.Arguments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueryBuilder.Operation
{
    /// <summary>
    /// Defines a selection operation
    /// aka filters over a collection
    /// </summary>
    public class SelectStage : AlgebraOperator
    {
        #region Properties
        /// <summary>
        /// List of arguments
        /// </summary>
        public IEnumerable<SelectArgument> Arguments { get; set; }
        /// <summary>
        /// Mapping between ER model and Mongo schema
        /// </summary>
        public IModelMap Map { get; set; }
        #endregion

        #region Methods
        /// <summary>
        /// Generate stage code
        /// </summary>
        /// <returns></returns>
        public override AlgebraOperatorResult Run()
        {
            // Iterate arguments and build the matching queries
            foreach ( SelectArgument Argument in Arguments )
            {
                // Iterate attributes and expressions
                foreach ( KeyValuePair<string, BaseExpression> Attribute in Argument.Attributes )
                {
                    string AttributeMap = Map.GetRuleValue( Argument.Element.Alias ?? Argument.Element.Element.Name, Attribute.Key );
                    // If the attribute map is null, we'll ignore it
                    if ( string.IsNullOrWhiteSpace( AttributeMap ) )
                    {
                        continue;
                    }
                }
            }
            return new AlgebraOperatorResult( new List<MongoDBOperator>() );
        }
        #endregion

        #region Constructor
        /// <summary>
        /// Initialize a new instance of SelectStage class
        /// </summary>
        /// <param name="Arguments"></param>
        /// <param name="Map"></param>
        public SelectStage( IEnumerable<SelectArgument> Arguments, IModelMap Map )
        {
            this.Arguments = Arguments;
            this.Map = Map;
        }
        #endregion
    }
}
