using QueryBuilder.Map;
using QueryBuilder.Mongo.Aggregation.Operators;
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
    /// Represents a Sort stage
    /// </summary>
    public class SortStage : AlgebraOperator
    {
        #region Properties
        /// <summary>
        /// List of arguments to process
        /// </summary>
        public List<SortArgument> Arguments { get; set; }
        /// <summary>
        /// List of rules
        /// </summary>
        public IModelMap MapRules { get; set; }
        #endregion

        #region Methods


        override public string SummarizeToString()
        {
            string Ret = "SortStage";

            return Ret;
        }

        /// <summary>
        /// Process stage and generates the corresponding commands
        /// </summary>
        /// <returns></returns>
        public override AlgebraOperatorResult Run( IModelMap inMap )
        {
            RuleMap = inMap;
            // Store operators to execute
            List<MongoDBOperator> OperatorsToExecute = new List<MongoDBOperator>();

            // Store attributes and their sort option
            Dictionary<string, MongoDBSort> SortByTheseFields = new Dictionary<string, MongoDBSort>();

            // Iterate arguments
            foreach ( SortArgument Argument in Arguments )
            {
                // Retrieve attribute map
                // Check if the MapRules is an instance of ModelMapping
                // If so, fetch the main mapping
                string AttributeMap = string.Empty;

                if ( MapRules is ModelMapping )
                {
                    MapRule Rule = ( MapRules as ModelMapping ).Rules.FirstOrDefault( R => R.Source.Name == Argument.Entity.GetName() && R.IsMain );
                    
                    if ( Rule == null )
                    {
                        throw new ImpossibleOperationException( $"A main mapping is required for entity {Argument.Entity.GetName()}" );
                    }
                    else
                    {
                        AttributeMap = Rule.Rules.FirstOrDefault( R => R.Key == Argument.Attribute.Name ).Value;
                    }
                }
                else
                {
                    AttributeMap = MapRules.GetRuleValue( Argument.Entity.GetAliasOrName(), Argument.Attribute.Name );
                }
                
                if ( string.IsNullOrWhiteSpace( AttributeMap ) )
                {
                    continue;
                }

                SortByTheseFields.Add( $"\"{AttributeMap}\"", Argument.SortOption );
            }

            // Create sort operator
            SortOperator SortOp = new SortOperator( SortByTheseFields );

            // Add to list
            OperatorsToExecute.Add( SortOp );

            // Return new Result instance
            return new AlgebraOperatorResult( OperatorsToExecute );
        }
        #endregion

        #region Constructor
        /// <summary>
        /// Initialize a new instance of SortStage
        /// </summary>
        /// <param name="Arguments"></param>
        /// <param name="MapRules"></param>
        public SortStage( List<SortArgument> Arguments, IModelMap MapRules )
        {
            this.Arguments = Arguments;
            this.MapRules = MapRules;
        }
        #endregion
    }
}
