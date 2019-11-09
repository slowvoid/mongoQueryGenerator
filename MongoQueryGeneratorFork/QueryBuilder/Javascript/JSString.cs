using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueryBuilder.Javascript
{
    /// <summary>
    /// Represents a string
    /// </summary>
    public class JSString : JSCode
    {
        #region Properties
        /// <summary>
        /// String value
        /// </summary>
        public string Value { get; set; }
        #endregion

        #region Methods
        /// <summary>
        /// Generates a string representation
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Value;
        }
        #endregion

        #region Operators
        /// <summary>
        /// Implicit conversion from string
        /// </summary>
        /// <param name="str"></param>
        public static implicit operator JSString( string str )
        {
            return new JSString( str );
        }
        #endregion

        #region Constructor
        /// <summary>
        /// Initializes a new instance of JSString class
        /// </summary>
        /// <param name="Value">The value</param>
        public JSString( string Value )
        {
            this.Value = Value;
        }
        #endregion
    }
}
