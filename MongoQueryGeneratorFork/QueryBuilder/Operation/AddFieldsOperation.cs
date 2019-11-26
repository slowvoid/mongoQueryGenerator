﻿using QueryBuilder.Javascript;
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
    /// Represents an Add Fields operation
    /// </summary>
    public class AddFieldsOperation : AlgebraOperator
    {
        #region Properties
        /// <summary>
        /// Attribute name value pairs
        /// </summary>
        public Dictionary<string, JSCode> Attributes { get; set; }
        #endregion

        #region Methods
        /// <summary>
        /// Run the operation
        /// </summary>
        /// <param name="LastResult"></param>
        /// <returns></returns>
        public override AlgebraOperatorResult Run()
        {
            AddFieldsOperator AddFieldsOperator = new AddFieldsOperator( Attributes );
            List<MongoDBOperator> OperationsToExecute = new List<MongoDBOperator>();
            
            if ( Attributes.Count > 0 )
            {
                OperationsToExecute.Add( AddFieldsOperator );
            }

            return new AlgebraOperatorResult( OperationsToExecute );
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Initializa a new AddFieldsOperation instance
        /// </summary>
        /// <param name="Attributes"></param>
        /// <param name="Map"></param>
        public AddFieldsOperation( Dictionary<string, JSCode> Attributes, ModelMapping Map ) : base(Map)
        {
            this.Attributes = Attributes;
        }
        #endregion
    }
}