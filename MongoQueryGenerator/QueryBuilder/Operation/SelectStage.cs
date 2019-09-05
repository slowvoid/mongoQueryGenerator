using QueryBuilder.Map;
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
            return base.Run();
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
