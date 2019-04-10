﻿using QueryBuilder.Map;
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
    public class AddFieldsOperation : BaseOperation
    {
        #region Properties
        /// <summary>
        /// Attribute name value pairs
        /// </summary>
        public Dictionary<string, string> Attributes { get; set; }
        #endregion

        #region Methods
        /// <summary>
        /// Run the operation
        /// </summary>
        /// <param name="LastResult"></param>
        /// <returns></returns>
        public override OperationResult Run(OperationResult LastResult)
        {
            AddFields AddFieldsOperator = new AddFields( Attributes );

            if ( Attributes.Count > 0 )
            {
                LastResult.Commands.Add( AddFieldsOperator );
            }

            return LastResult;
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Initializa a new AddFieldsOperation instance
        /// </summary>
        /// <param name="Attributes"></param>
        /// <param name="Map"></param>
        public AddFieldsOperation( Dictionary<string, string> Attributes, ModelMapping Map ) : base(Map)
        {
            this.Attributes = Attributes;
        }
        #endregion
    }
}