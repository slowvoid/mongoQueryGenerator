﻿using QueryBuilder.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using QueryBuilder.Map;
using QueryBuilder.ER;

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
        #endregion

        #region Methods
        /// <summary>
        /// Run operation
        /// </summary>
        /// <returns></returns>
        public virtual void Run( ref AlgebraOperatorResult LastResult )
        {
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Initialize a new instance of Operation class
        /// </summary>
        public AlgebraOperator( ModelMapping ModelMap )
        {
            this.ModelMap = ModelMap;
        }
        #endregion
    }
}