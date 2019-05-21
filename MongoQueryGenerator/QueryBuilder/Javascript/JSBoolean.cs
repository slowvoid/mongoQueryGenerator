using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueryBuilder.Javascript
{
    /// <summary>
    /// Represents a boolean in javascript
    /// </summary>
    public class JSBoolean : JSCode
    {
        #region Properties
        /// <summary>
        /// The value
        /// </summary>
        public bool Value { get; set; }
        #endregion

        #region Methods
        /// <summary>
        /// Return a string representing this value
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Value ? "true" : "false";
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Initialize a new JSBoolean instance
        /// </summary>
        /// <param name="Value"></param>
        public JSBoolean( bool Value )
        {
            this.Value = Value;
        }
        #endregion
    }
}
