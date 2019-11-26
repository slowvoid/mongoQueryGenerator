﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using QueryBuilder.Map;
using QueryBuilder.Operation;
using QueryBuilder.ER;
using QueryBuilder.Mongo.Aggregation.Operators;
using QueryBuilder.Operation.Arguments;

namespace QueryBuilder.Query
{
    /// <summary>
    /// Class responsible for running a pipeline and generating
    /// MongoDB queries
    /// </summary>
    public class QueryGenerator
    {
        #region Properties
        /// <summary>
        /// Command pipeline
        /// </summary>
        public List<AlgebraOperator> PipelineOperators { get; set; }
        /// <summary>
        /// Data acting as start point
        /// </summary>
        public FromArgument StartArgument { get; set; }
        #endregion

        #region Methods
        /// <summary>
        /// Run the query generator
        /// </summary>
        /// <returns></returns>
        public string Run()
        {
            string CollectionName = StartArgument.GetCollectionName();
            // Check if collection name is set
            // otherwise throw error
            if ( string.IsNullOrWhiteSpace( CollectionName ) )
            {
                throw new InvalidOperationException( "CollectionName cannot be empty" );
            }

            // Setup results
            AlgebraOperatorResult Result = new AlgebraOperatorResult( new List<MongoDBOperator>() );

            foreach ( AlgebraOperator Op in PipelineOperators )
            {
                Result.Commands.AddRange( Op.Run().Commands );
            }

            // Store command objects
            List<string> AggregatePipeline = new List<string>();

            foreach ( MongoDBOperator Command in Result.Commands )
            {
                AggregatePipeline.Add( Command.ToJavaScript() );
            }

            // TODO: Update this section to generate collection and aggregate
            // according to the query
            return string.Format( "db.{0}.aggregate([{1}], {{allowDiskUse: true}}).pretty();", CollectionName, string.Join( ",", AggregatePipeline ) );
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Initialize a new instance of QueryGenerator class
        /// </summary>
        /// <param name="Pipeline">Command pipeline</param>
        public QueryGenerator( FromArgument StartArgument, List<AlgebraOperator> PipelineOperators )
        {
            this.StartArgument = StartArgument;
            this.PipelineOperators = PipelineOperators;
        }
        #endregion
    }
}