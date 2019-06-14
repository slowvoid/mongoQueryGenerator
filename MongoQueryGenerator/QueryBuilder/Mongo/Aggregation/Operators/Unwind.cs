using QueryBuilder.Javascript;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueryBuilder.Mongo.Aggregation.Operators
{
    /// <summary>
    /// Represents the $unwind operator
    /// </summary>
    public class UnwindOperator : MongoDBOperator
    {
        #region Properties
        /// <summary>
        /// Path to field that should be unwinded
        /// </summary>
        public string Field { get; set; }
        /// <summary>
        /// Sets wheter empty results should be preserved
        /// </summary>
        public bool PreserveNullOrEmpty { get; set; }
        #endregion

        #region Methods
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
            Dictionary<string, object> UnwindAttrs = new Dictionary<string, object>();
            UnwindAttrs.Add( "path", Field );
            UnwindAttrs.Add( "preserveNullAndEmptyArrays", PreserveNullOrEmpty );

            return new JSObject( "$unwind", UnwindAttrs );
        }
        #endregion

        #region Constructor
        /// <summary>
        /// Initialize a new Unwind instance
        /// </summary>
        public UnwindOperator(string Field, bool PreserveNullOrEmpty = true)
        {
            this.Field = $"${Field}";
            this.PreserveNullOrEmpty = PreserveNullOrEmpty;
        }
        #endregion
    }
}
