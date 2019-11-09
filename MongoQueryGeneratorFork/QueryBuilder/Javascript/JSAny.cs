using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueryBuilder.Javascript
{
    /// <summary>
    /// Represents any value
    /// </summary>
    public class JSAny : JSCode
    {
        #region Properties
        /// <summary>
        /// Value
        /// </summary>
        public object Value { get; set; }
        #endregion

        #region Methods
        public override string ToString()
        {
            return Value.ToString();
        }
        #endregion

        #region Constructor
        /// <summary>
        /// Initialize a new instance of JSAny
        /// </summary>
        /// <param name="Value"></param>
        public JSAny( object Value )
        {
            this.Value = Value;
        }
        #endregion
    }
}
