using QueryBuilder.ER;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueryBuilder.Map
{
    /// <summary>
    /// Represents a direct map between an ER Element and its location in the
    /// current pipeline stage (attribute location)
    /// </summary>
    public class VirtualRule
    {
        #region Properties
        /// <summary>
        /// ER Element
        /// </summary>
        public BaseERElement Element { get; set; }
        /// <summary>
        /// Rule value
        /// Root attribute in which the element is mapped to
        /// </summary>
        public string Value { get; set; }
        #endregion

        #region Constructor
        /// <summary>
        /// Initialize a new instance of VirtualRule class
        /// </summary>
        /// <param name="Element"></param>
        /// <param name="Value"></param>
        public VirtualRule( BaseERElement Element, string Value )
        {
            this.Element = Element;
            this.Value = Value;
        }
        #endregion
    }
}
