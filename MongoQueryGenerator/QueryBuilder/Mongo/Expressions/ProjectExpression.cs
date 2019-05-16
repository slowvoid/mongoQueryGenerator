using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueryBuilder.Mongo.Expressions
{
    /// <summary>
    /// Represents an expression to be used in a project operation
    /// </summary>
    public abstract class ProjectExpression
    {
        #region Methods
        /// <summary>
        /// Create a JavaScript compatible representation
        /// </summary>
        /// <returns></returns>
        public abstract BsonValue ToJavaScript();
        #endregion
    }
}
