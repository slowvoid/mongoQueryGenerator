using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using QueryBuilder.ER;
using QueryBuilder.Map;
using QueryBuilder.Operation.Exceptions;
using QueryBuilder.Query;

namespace QueryBuilder.Operation
{
    /// <summary>
    /// Represents a JOIN operation
    /// </summary>
    public class JoinOperation : BaseOperation
    {
        #region Properties
        /// <summary>
        /// Relationship startpoint
        /// </summary>
        public Entity SourceEntity { get; set; }
        /// <summary>
        /// Relationship endpoint
        /// </summary>
        public Entity TargetEntity { get; set; }
        /// <summary>
        /// Relationship connecting both entities
        /// </summary>
        public Relationship Relationship { get; set; }
        #endregion

        #region Private Data
        /// <summary>
        /// ER -> MongoDB map
        /// </summary>
        private ModelMapping ModelMap { get; set; }
        #endregion

        #region Methods
        public override QueryCommand Run()
        {
            // Get Map rule
            MapRule SourceRule = ModelMap.Rules.Find( R => R.Source == SourceEntity );
            MapRule TargetRule = ModelMap.Rules.Find( R => R.Source == TargetEntity );

            // Start a Join Command
            JoinCommand Command = new JoinCommand();

            // Set target entity
            Command.From = TargetRule.Target.Name;
            // Locate mapping for the source / target attributes
            Command.ForeignField = TargetRule.Rules.First( K => K.Key == Relationship.TargetAttribute.Name ).Value;
            Command.LocalField = SourceRule.Rules.First( K => K.Key == Relationship.SourceAttribute.Name ).Value;

            // As field (For now use relationship name)
            Command.AsField = Relationship.Name;

            return Command;
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Initialize a new instance of Join Operation class
        /// </summary>
        /// <param name="SourceEntity">Source entity</param>
        /// <param name="TargetEntity">Target entity</param>
        /// <param name="Relationship">Join through this relationship</param>
        /// <param name="ModelMap">Map rules between ER and Mongo</param>
        public JoinOperation( Entity SourceEntity, Entity TargetEntity, Relationship Relationship, ModelMapping ModelMap )
        {
            this.SourceEntity = SourceEntity;
            this.TargetEntity = TargetEntity;
            this.Relationship = Relationship;
            this.ModelMap = ModelMap;
        }
        #endregion
    }
}
