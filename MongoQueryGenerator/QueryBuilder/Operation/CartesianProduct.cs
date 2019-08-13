using QueryBuilder.ER;
using QueryBuilder.Map;
using QueryBuilder.Mongo.Aggregation.Operators;
using QueryBuilder.Operation.Arguments;
using QueryBuilder.Shared;
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
        public JoinableEntity SourceEntity { get; set; }
        /// <summary>
        /// Entity to fetch
        /// </summary>
        public JoinableEntity TargetEntity { get; set; }
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
            MapRule TargetRule = ModelMap.Rules.First( Rule => Rule.Source.Name == TargetEntity.JoinedElement.Name && Rule.IsMain );

            // Create operator
            LookupOperator LookupOp = new LookupOperator(true)
            {
                From = TargetRule.Target.Name,
                Pipeline = new List<MongoDBOperator>(),
                As = $"data_{TargetEntity.JoinedElement.Name}"
            };

            // Add to list
            OperatorsToExecute.Add( LookupOp );

            return new AlgebraOperatorResult( OperatorsToExecute );
        }
        /// <summary>
        /// Computes the virtual map after the execution of this operator
        /// Increments any existing virtual maps (if given)
        /// </summary>
        /// <param name="ExistingVirtualMap"></param>
        /// <returns></returns>
        public override VirtualMap ComputeVirtualMap( VirtualMap ExistingVirtualMap = null )
        {
            // Adds the prefix 'data_Entity' to attributes
            VirtualRule TargetVRule = new VirtualRule( TargetEntity.JoinedElement, TargetEntity.Alias );

            // Fetch original map for target entity
            MapRule TargetRule = ModelMap.Rules.First( Rule => Rule.Source.Name == TargetEntity.JoinedElement.Name && Rule.IsMain );

            foreach ( DataAttribute Attribute in TargetEntity.JoinedElement.Attributes )
            {
                string AttributeRule = TargetRule.Rules.First( Rule => Rule.Key == Attribute.Name ).Value;
                TargetVRule.AddRule( Attribute.Name, $"data_{TargetEntity.JoinedElement.Name}.{AttributeRule}" );
            }

            // Also process source entity rule
            VirtualRule SourceVRule = new VirtualRule( SourceEntity.JoinedElement, SourceEntity.Alias );
            // Fetch original map for source entity
            MapRule SourceRule = ModelMap.Rules.First( Rule => Rule.Source.Name == SourceEntity.JoinedElement.Name && Rule.IsMain );

            foreach ( DataAttribute Attribute in SourceEntity.JoinedElement.Attributes )
            {
                string AttributeRule = SourceRule.Rules.First( Rule => Rule.Key == Attribute.Name ).Value;
                SourceVRule.AddRule( Attribute.Name, AttributeRule );
            }

            VirtualMap VMap = new VirtualMap( new List<VirtualRule>() );
            VMap.Rules.AddRange( new VirtualRule[] { SourceVRule, TargetVRule } );

            if ( ExistingVirtualMap != null )
            {
                // Check if source rule already exist
                if ( ExistingVirtualMap.Rules.Exists( Rule => Rule.SourceERElement.Name == SourceEntity.JoinedElement.Name && Rule.Alias == SourceEntity.Alias ) )
                {
                    VMap.Rules.Remove( SourceVRule );
                }
                // Check if the new rules already exists
                if ( ExistingVirtualMap.Rules.Exists( Rule => Rule.SourceERElement.Name == TargetEntity.JoinedElement.Name && Rule.Alias == TargetEntity.Alias ) )
                {
                    VMap.Rules.Remove( TargetVRule );
                }

                VMap.Rules.AddRange( ExistingVirtualMap.Rules );
            }

            return VMap;
        }
        #endregion

        #region Constructor
        /// <summary>
        /// Initialize a new instance of CartesianProductOperator
        /// </summary>
        /// <param name="SourceEntity"></param>
        /// <param name="TargetEntity"></param>
        public CartesianProductOperator( JoinableEntity SourceEntity, JoinableEntity TargetEntity, ModelMapping Map )
        {
            this.SourceEntity = SourceEntity;
            this.TargetEntity = TargetEntity;
            this.ModelMap = Map;
        }
        #endregion
    }
}
