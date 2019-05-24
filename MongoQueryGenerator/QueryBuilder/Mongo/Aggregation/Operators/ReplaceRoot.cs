using MongoDB.Bson;
using QueryBuilder.Javascript;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueryBuilder.Mongo.Aggregation.Operators
{
    /// <summary>
    /// Represents the $replaceRoot operator
    /// </summary>
    public class ReplaceRoot : MongoDBOperator
    {
        #region Properties
        /// <summary>
        /// Path to the attribute containg the replacement document
        /// </summary>
        public string NewRoot { get; set; }
        #endregion

        #region Methods
        /// <summary>
        /// Generate a JavaScript Object representing this Operator
        /// </summary>
        /// <returns></returns>
        public override string ToJavaScript()
        {
            return ToJSCode().ToString();
        }
        /// <summary>
        /// Generates a javascript representation of this instance
        /// </summary>
        /// <returns></returns>
        public override JSCode ToJSCode()
        {
            Dictionary<string, object> Attrs = new Dictionary<string, object>();
            Attrs.Add( "newRoot", NewRoot );

            return new JSObject( "$replaceRoot", Attrs );
        }
        #endregion
    }
}
