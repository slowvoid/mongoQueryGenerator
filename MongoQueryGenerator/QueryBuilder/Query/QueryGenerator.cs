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
        public Pipeline Pipeline { get; set; }
        /// <summary>
        /// Collection used a start point
        /// </summary>
        public string CollectionName { get; set; }
        /// <summary>
        /// Output document model
        /// </summary>
        public ModelMapping OutputModel { get; set; }
        #endregion

        #region Methods
        /// <summary>
        /// Generates the output model based on the pipeline
        /// </summary>
        private void GenerateOutputModel()
        {

        }
        /// <summary>
        /// Run the query generator
        /// </summary>
        /// <returns></returns>
        public string Run()
        {
            // Check if collection name is set
            // otherwise throw error
            if ( string.IsNullOrWhiteSpace( CollectionName ) )
            {
                throw new InvalidOperationException( "CollectionName cannot be empty" );
            }

            // Setup results
            AlgebraOperatorResult Result = new AlgebraOperatorResult( new List<MongoDBOperator>() );

            foreach ( AlgebraOperator Op in Pipeline.Operations )
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
            return string.Format( "db.{0}.aggregate([{1}]).pretty();", CollectionName, string.Join( ",", AggregatePipeline ) );
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Initialize a new instance of QueryGenerator class
        /// </summary>
        /// <param name="Pipeline">Command pipeline</param>
        public QueryGenerator( Pipeline Pipeline )
        {
            this.Pipeline = Pipeline;
        }
        #endregion
    }
}
