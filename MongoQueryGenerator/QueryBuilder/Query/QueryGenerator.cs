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
        /// <summary>
        /// Whether final query should append pretty()
        /// </summary>
        public bool PrettyPrint { get; set; }
        /// <summary>
        /// Starting Model Mapping
        /// </summary>
        public ModelMapping StartMap { get; set; }
        #endregion

        #region Methods
        /// <summary>
        /// Assign a value to StartMap
        /// </summary>
        /// <param name="inMap"></param>
        public void SetStartMap( ModelMapping inMap )
        {
            StartMap = inMap;
        }
        public string SummarizeToString()
        {
            string Ret = "S: " + StartArgument.SummarizeToString()+"\n";

            int i = 1;

            foreach ( AlgebraOperator Op in PipelineOperators )
            {
                Ret += (i++)+": "+Op.SummarizeToString()+"\n";
            }

            return Ret;
        }

        /// <summary>
        /// Sets pretty print prop
        /// </summary>
        /// <param name="inValue"></param>
        public void SetPrettyPrint(bool inValue)
        {
            PrettyPrint = inValue;
        }
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

            // Get project arguments
            IEnumerable<ProjectArgument> attributesToProject = null;
            ProjectStage projStage = (ProjectStage)PipelineOperators.FirstOrDefault( Op => Op is ProjectStage );
            if ( projStage != null )
            {
                attributesToProject = projStage.Arguments;
            }

            // Move SelectStage to start of array
            AlgebraOperator MoveToTop = PipelineOperators.Find( Op => Op is SelectStage2 );

            if ( MoveToTop != null )
            {
                PipelineOperators.Remove( MoveToTop );
                PipelineOperators.Insert( 0, MoveToTop );
            }

            for ( int i = 0; i < PipelineOperators.Count; i++ )
            {
                AlgebraOperator Op = PipelineOperators[ i ];
                if ( i == 0 )
                {
                    Result.Commands.AddRange( Op.Run( StartMap, attributesToProject ).Commands );
                }
                else
                {
                    AlgebraOperator previousOp = PipelineOperators[ i - 1 ];
                    Result.Commands.AddRange( Op.Run( previousOp.ComputeVirtualMap(), attributesToProject ).Commands );
                }
            }

            // Check if there are any commands to be executed last
            List<MongoDBOperator> MoveToEnd = Result.Commands.FindAll( C => C.ShouldExecuteLast );

            if ( MoveToEnd.Count > 0 )
            {
                Result.Commands.RemoveAll( C => C.ShouldExecuteLast );
                // Add again at the end
                Result.Commands.AddRange( MoveToEnd );
            }

            // Store command objects
            List<string> AggregatePipeline = new List<string>();

            foreach ( MongoDBOperator Command in Result.Commands )
            {
                AggregatePipeline.Add( Command.ToJavaScript() );
            }

            if ( PrettyPrint )
            {
                return string.Format( "db.{0}.aggregate([{1}], {{allowDiskUse: true}}).pretty();", CollectionName, string.Join( ",", AggregatePipeline ) );
            }
            else
            {
                return string.Format( "db.{0}.aggregate([{1}], {{allowDiskUse: true}});", CollectionName, string.Join( ",", AggregatePipeline ) );
            }
        }
        /// <summary>
        /// Generates the query in explain mode
        /// </summary>
        /// <returns></returns>
        public string Explain()
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

            for ( int i = 0; i < PipelineOperators.Count; i++ )
            {
                AlgebraOperator Op = PipelineOperators[ i ];
                if ( i == 0 )
                {
                    Result.Commands.AddRange( Op.Run( StartMap ).Commands );
                }
                else
                {
                    AlgebraOperator previousOp = PipelineOperators[ i - 1 ];
                    Result.Commands.AddRange( Op.Run( previousOp.ComputeVirtualMap() ).Commands );
                }
            }

            foreach ( AlgebraOperator Op in PipelineOperators )
            {
                Result.Commands.AddRange( Op.Run(StartMap).Commands );
            }

            // Check if there are any commands to be executed last
            List<MongoDBOperator> MoveToEnd = Result.Commands.FindAll( C => C.ShouldExecuteLast );

            if ( MoveToEnd.Count > 0 )
            {
                Result.Commands.RemoveAll( C => C.ShouldExecuteLast );
                // Add again at the end
                Result.Commands.AddRange( MoveToEnd );
            }

            // Store command objects
            List<string> AggregatePipeline = new List<string>();

            foreach ( MongoDBOperator Command in Result.Commands )
            {
                AggregatePipeline.Add( Command.ToJavaScript() );
            }

            // TODO: Update this section to generate collection and aggregate
            // according to the query
            return string.Format( "db.{0}.explain('executionStats').aggregate([{1}], {{allowDiskUse: true}});", CollectionName, string.Join( ",", AggregatePipeline ) );
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

            PrettyPrint = false;
        }
        #endregion
    }
}
