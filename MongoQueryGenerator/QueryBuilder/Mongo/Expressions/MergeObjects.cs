﻿using QueryBuilder.Javascript;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueryBuilder.Mongo.Expressions
{
    /// <summary>
    /// Represents the $mergeObjects expression
    /// </summary>
    public class MergeObjects : BaseExpression
    {
        #region Properties
        /// <summary>
        /// Objects to merge
        /// </summary>
        public List<string> Objects { get; set; }
        #endregion

        #region Methods
        /// <summary>
        /// Generates a JavaScript object representing this expression
        /// </summary>
        /// <returns></returns>
        public override string ToJavaScript()
        {
            return ToJSCode().ToString();
        }

        public override JSCode ToJSCode()
        {
            Dictionary<string, object> MergeObj = new Dictionary<string, object>();
            MergeObj.Add( "$mergeObjects", new JSArray( Objects.ToList<object>() ) );

            return new JSObject( MergeObj );
        }
        #endregion

        #region Constructor
        /// <summary>
        /// Initializes a new instance of MergeObjects class
        /// </summary>
        public MergeObjects()
        {
            Objects = new List<string>();
        }
        #endregion
    }
}
