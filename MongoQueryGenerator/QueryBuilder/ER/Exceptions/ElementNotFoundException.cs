using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueryBuilder.ER.Exceptions
{
    /// <summary>
    /// Exception thrown when a FindByName operation fails
    /// </summary>
    public class ElementNotFoundException : Exception
    {
        #region Constructor
        /// <summary>
        /// Initializes a new instance of ElementNotFoundException
        /// </summary>
        /// <param name="Message">Exception Message</param>
        public ElementNotFoundException( string Message ) : base( Message ) { }
        #endregion
    }
}
