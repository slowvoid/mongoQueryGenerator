using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QueryBuilder.Javascript;

namespace QueryBuilder.Mongo.Aggregation.Operators
{
    /// <summary>
    /// Represents a $limit operator
    /// </summary>
    public class LimitOperator : MongoDBOperator
    {
        #region Properties
        /// <summary>
        /// Limit threshold
        /// </summary>
        public int Count { get; set; }
        #endregion

        #region Methods
        /// <summary>
        /// Generates a javascript representation of this instnace
        /// </summary>
        /// <returns></returns>
        public override string ToJavaScript()
        {
            return ToJSCode().ToString();
        }
        /// <summary>
        /// Generates a javascript representation of this instnace
        /// </summary>
        /// <returns></returns>
        public override JSCode ToJSCode()
        {
            Dictionary<string, object> LimitObj = new Dictionary<string, object>();
            LimitObj.Add( "$limit", Count );

            return new JSObject( LimitObj );
        }
        #endregion

        #region Constructor
        /// <summary>
        /// Initialize a new instance o LimitOperator
        /// </summary>
        /// <param name="Count"></param>
        public LimitOperator(int Count)
        {
            this.Count = Count;
        }
        #endregion
    }
}
