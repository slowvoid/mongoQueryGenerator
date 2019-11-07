using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QueryBuilder.Javascript;

namespace QueryBuilder.Mongo.Aggregation.Operators
{
    /// <summary>
    /// Sort options
    /// </summary>
    public enum MongoDBSort
    {
        Descending = -1,
        Ascending = 1
    }
    /// <summary>
    /// Represents the $sort operator
    /// </summary>
    public class SortOperator : MongoDBOperator
    {
        #region Properties
        /// <summary>
        /// Sort by the given fields and options
        /// </summary>
        public Dictionary<string, MongoDBSort> SortAttributes { get; set; }
        #endregion

        #region Methods
        /// <summary>
        /// Generates a javascript representation
        /// </summary>
        /// <returns></returns>
        public override string ToJavaScript()
        {
            return ToJSCode().ToString();
        }
        /// <summary>
        /// Generates a javascript representation
        /// </summary>
        /// <returns></returns>
        public override JSCode ToJSCode()
        {
            Dictionary<string, object> ConvertedOptions = SortAttributes.ToDictionary( A => A.Key, V => (object)Convert.ToInt32( V.Value ) );
            return new JSObject( "$sort", ConvertedOptions );
        }
        #endregion

        #region Constructor
        /// <summary>
        /// Initialize a new instance of SortOperator
        /// </summary>
        /// <param name="SortAttributes"></param>
        public SortOperator( Dictionary<string, MongoDBSort> SortAttributes )
        {
            this.SortAttributes = SortAttributes;
        }
        #endregion
    }
}
