using QueryBuilder.Map;
using QueryBuilder.Mongo.Aggregation.Operators;
using QueryBuilder.Mongo.Expressions;
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
    /// </summary>
    public class ProjectOperation : AlgebraOperator
    {
        #region Properties
        /// <summary>
        /// Attributes involved in this operation and their visibility status
        /// </summary>
        public Dictionary<string, ProjectExpression> Attributes { get; set; }
        #endregion

        #region Methods
        /// <summary>
        /// Run the operation adding MongoDB operators to the pipeline
        /// </summary>
        /// <param name="LastResult"></param>
        /// <returns></returns>
        public override void Run( ref AlgebraOperatorResult LastResult )
        {
            // For projection it is mandatory to use qualified name (Entity.Attribute)
            // Support for aliases might be done later
            Dictionary<string, ProjectExpression> AttributesToProject = new Dictionary<string, ProjectExpression>();

            // Locate each attribute mapping
            foreach ( string AttributeQualifiedName in Attributes.Keys )
            {
                string[] EntityAttributePair = AttributeQualifiedName.Split( new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries );
                if ( EntityAttributePair.Length == 0 )
                {
                    throw new InvalidOperationException( string.Format( "Attribute {0} is not a qualified name.", AttributeQualifiedName ) );
                }

                string EntityName = EntityAttributePair[ 0 ];
                string AttributeName = EntityAttributePair[ 1 ];

                MapRule EntityRule = ModelMap.Rules.First( R => R.Source.Name == EntityName );

                if ( EntityRule == null )
                {
                    throw new RuleNotFoundException( string.Format( "Entity {0} has no valid mapping to MongoDB.", EntityName ) );
                }

                KeyValuePair<string,string>? AttributeMapRule = EntityRule.Rules.First( R => R.Key == AttributeName );

                if ( !AttributeMapRule.HasValue )
                {
                    throw new RuleNotFoundException( string.Format( "Attribute {0} has no valid mapping to MongoDB.", AttributeName ) );
                }

                AttributesToProject.Add( AttributeMapRule.Value.Value, Attributes[ AttributeQualifiedName ] );
            }

            if ( AttributesToProject.Count > 0 )
            {
                // Found attributes to project, add command to pipeline
                Project ProjectCommand = new Project( AttributesToProject );
                LastResult.Commands.Add( ProjectCommand );
            }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Initialize a new instance of ProjectOperation class
        /// </summary>
        /// <param name="Map"></param>
        public ProjectOperation( Dictionary<string, ProjectExpression> Attributes, ModelMapping Map ) : base(Map)
        {
            this.Attributes = Attributes;
        }
        #endregion
    }
}
