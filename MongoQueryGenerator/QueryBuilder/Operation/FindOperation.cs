using QueryBuilder.ER;
using QueryBuilder.Map;
using QueryBuilder.Mongo.Aggregation.Operators;
using QueryBuilder.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueryBuilder.Operation
{
    /// <summary>
    /// Defines a find operation
    /// </summary>
    public class FindOperation : AlgebraOperator
    {
        #region Properties
        /// <summary>
        /// Entity to look for
        /// </summary>
        public Entity TargetEntity { get; set; }
        /// <summary>
        /// Conditional filters
        /// Use dot notation for complex attributes
        /// </summary>
        public Dictionary<string, object> Filters { get; set; }
        #endregion

        #region Methods



        override public string SummarizeToString()
        {
            string Ret = "FindOperation";

            return Ret;
        }
        public override AlgebraOperatorResult Run()
        {
            return new AlgebraOperatorResult( new List<MongoDBOperator>() );
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Initialize a new FindOperation instance with no conditional filter
        /// </summary>
        /// <param name="TargetEntity">Entity to find</param>
        public FindOperation( Entity TargetEntity, ModelMapping ModelMap ) : base( ModelMap )
        {
            this.TargetEntity = TargetEntity;
            Filters = new Dictionary<string, object>();
        }
        /// <summary>
        /// Intialize a new FindOperation instance
        /// </summary>
        /// <param name="TargetEntity">Entity to find</param>
        /// <param name="Filters">Conditional parameters</param>
        public FindOperation( Entity TargetEntity, Dictionary<string, object> Filters, ModelMapping ModelMap ) : base( ModelMap )
        {
            this.TargetEntity = TargetEntity;
            this.Filters = Filters;
        }
        #endregion
    }
}
