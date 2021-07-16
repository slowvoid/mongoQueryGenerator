using QueryBuilder.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using QueryBuilder.Map;
using QueryBuilder.ER;
using QueryBuilder.Mongo.Aggregation.Operators;
using QueryBuilder.Operation.Arguments;

namespace QueryBuilder.Operation
{
    /// <summary>
    /// Defines a query operation (JOIN, PROJECT, etc)
    /// </summary>
    public abstract class AlgebraOperator
    {
        #region Properties
        /// <summary>
        /// ER -> MongoDB map
        /// </summary>
        protected ModelMapping ModelMap { get; set; }
        /// <summary>
        /// Virtual Map between ER model and Output document
        /// </summary>
        protected IModelMap RuleMap { get; set; }
        #endregion

        #region Methods

        public abstract string SummarizeToString();

        /// <summary>
        /// Run operation
        /// </summary>
        /// <returns></returns>
        public virtual AlgebraOperatorResult Run( IModelMap inMap, IEnumerable<ProjectArgument> inAttributesToProject = null )
        {
            return new AlgebraOperatorResult( new List<MongoDBOperator>() );
        }
        /// <summary>
        /// Computes the resulting virtual map after processing this operator
        /// </summary>
        /// <returns></returns>
        public virtual VirtualMap ComputeVirtualMap( VirtualMap ExistingVirtualMap = null )
        {
            if ( !RuleMap.IsEmpty() )
            {
                if ( RuleMap is VirtualMap )
                {
                    return RuleMap as VirtualMap;
                }
                else
                {
                    return VirtualMap.FromModelMap( RuleMap as ModelMapping );
                }
            }

            return new VirtualMap( new List<VirtualRule>() );
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Initialize a new instance of AlgebraOperator class
        /// </summary>
        public AlgebraOperator( ModelMapping ModelMap )
        {
            this.ModelMap = ModelMap;
        }
        /// <summary>
        /// Initialize a new instance of AlgebraOperator class
        /// </summary>
        public AlgebraOperator() { }
        #endregion
    }
}
