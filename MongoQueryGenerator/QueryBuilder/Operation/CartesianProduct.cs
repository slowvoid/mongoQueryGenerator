using QueryBuilder.ER;
using QueryBuilder.Map;
using QueryBuilder.Mongo.Aggregation.Operators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueryBuilder.Operation
{
    /// <summary>
    /// Represents the Cartesian Product operator
    /// This includes all ocurrences of an entity regardless of relationship
    /// </summary>
    public class CartesianProductOperator : AlgebraOperator
    {
        #region Properties
        /// <summary>
        /// Source entity (start point)
        /// </summary>
        public Entity SouceEntity { get; set; }
        /// <summary>
        /// Entity to fetch
        /// </summary>
        public Entity TargetEntity { get; set; }
        #endregion

        #region Methods
        /// <summary>
        /// Run operator
        /// </summary>
        /// <returns></returns>
        public override AlgebraOperatorResult Run()
        {
            // This operator is quite simple
            // basically a lookup with an empty pipeline (no join condition)
            // No support for embedded entities
            List<MongoDBOperator> OperatorsToExecute = new List<MongoDBOperator>();
            // Fetch rules
            MapRule TargetRule = ModelMap.Rules.First( Rule => Rule.Source.Name == TargetEntity.Name && Rule.IsMain );

            // Create operator
            LookupOperator LookupOp = new LookupOperator(true)
            {
                From = TargetRule.Target.Name,
                Pipeline = new List<MongoDBOperator>(),
                As = $"data_{TargetEntity.Name}"
            };

            // Add to list
            OperatorsToExecute.Add( LookupOp );

            return new AlgebraOperatorResult( OperatorsToExecute );
        }
        #endregion

        #region Constructor
        /// <summary>
        /// Initialize a new instance of CartesianProductOperator
        /// </summary>
        /// <param name="SourceEntity"></param>
        /// <param name="TargetEntity"></param>
        public CartesianProductOperator( Entity SourceEntity, Entity TargetEntity, ModelMapping Map )
        {
            this.SouceEntity = SourceEntity;
            this.TargetEntity = TargetEntity;
            this.ModelMap = Map;
        }
        #endregion
    }
}
