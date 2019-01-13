using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueryBuilder.Shared.Exceptions
{
    /// <summary>
    /// AttributeNotFoundException class
    /// </summary>
    public class AttributeNotFoundException : Exception
    {
        #region Constructor
        /// <summary>
        /// Initializes a new instance of AttributeNotFoundException
        /// </summary>
        /// <param name="Message"></param>
        public AttributeNotFoundException( string Message ) : base( Message ) { }
        #endregion
    }
}
