﻿using QueryBuilder.ER;
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
    /// Defines the result of an operation
    /// </summary>
    public class OperationResult
    {
        #region Properties
        /// <summary>
        /// Available ER elements
        /// </summary>
        public List<BaseERElement> PipelineResult { get; set; }
        /// <summary>
        /// List of commands to be executed
        /// </summary>
        public List<BaseOperator> Commands { get; set; }
        #endregion

        #region Constructor
        /// <summary>
        /// Initialize a new OperationResult instance
        /// </summary>
        /// <param name="PipelineResult"></param>
        /// <param name="Commands"></param>
        public OperationResult( List<BaseERElement> PipelineResult, List<BaseOperator> Commands )
        {
            this.PipelineResult = PipelineResult;
            this.Commands = Commands;
        }
        #endregion
    }
}