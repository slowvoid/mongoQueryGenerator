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
        public SelectArgument Argument { get; set; }
        /// <summary>
        /// Mapping between ER model and Mongo schema
        /// </summary>
        public IModelMap Map { get; set; }
        /// <summary>
        /// Matching key pairs
        /// </summary>
        public Dictionary<string, object> KeyPairs { get; set; }
        /// <summary>
        /// Get/Sets whether to use simpler match stage
        /// </summary>
        public bool UseSimplerMatch { get; set; }
        #endregion

        #region Methods
        /// <summary>
        /// Generate stage code
        /// </summary>
        /// <returns></returns>
        public override AlgebraOperatorResult Run()
        {
            MatchOperator MatchOp;

            if ( UseSimplerMatch )
            {
                MatchOp = new MatchOperator( KeyPairs );
            }
            else
            {
                MatchOp = new MatchOperator( new Expr( Argument.Expression ) );
            }

            return new AlgebraOperatorResult( new List<MongoDBOperator>() { MatchOp } );
        }
        #endregion

        #region Constructor
        /// <summary>
        /// Initialize a new instance of SelectStage class
        /// </summary>
        /// <param name="Arguments"></param>
        /// <param name="Map"></param>
        public SelectStage( SelectArgument Argument, IModelMap Map )
        {
            this.Argument = Argument;
            this.Map = Map;

            KeyPairs = new Dictionary<string, object>();
            UseSimplerMatch = false;
        }
        /// <summary>
        /// Initialize a new instance of SelectStage class
        /// This resuls in a simpler $match stage using direct attribute matching $match: { key: value }
        /// </summary>
        /// <param name="inKeyPairs"></param>
        public SelectStage( Dictionary<string, object> inKeyPairs )
        {
            KeyPairs = inKeyPairs;

            UseSimplerMatch = true;
        }
        #endregion
    }
}
