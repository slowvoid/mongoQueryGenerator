using QueryBuilder.Javascript;
using QueryBuilder.Mongo.Expressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueryBuilder.Mongo.Aggregation.Operators
{
    /// <summary>
    /// Represents the $project aggregation stage
    /// </summary>
    public class Project : BaseOperator
    {
        #region Properties
        /// <summary>
        /// Attributes and their respective visibility status
        /// </summary>
        public Dictionary<string, ProjectExpression> Attributes { get; set; }
        #endregion

        #region Methods
        /// <summary>
        /// Creates a javascript compatible object
        /// </summary>
        /// <returns></returns>
        public override string ToJavaScript()
        {
            return ToJSCode().ToString();
        }
        /// <summary> 
        /// Generates a Javascript code object representing this instance
        /// </summary>
        /// <returns></returns>
        public override JSCode ToJSCode()
        {
            return new JSObject( "$project", Attributes.ToDictionary( I => I.Key, I => (object)I.Value.ToJSCode() ) );
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Initialize a new instance of Project class
        /// </summary>
        public Project( Dictionary<string, ProjectExpression> Attributes )
        {
            this.Attributes = Attributes;
        }
        #endregion
    }
}
