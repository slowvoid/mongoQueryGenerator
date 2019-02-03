using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueryBuilder.Operation.Exceptions
{
    /// <summary>
    /// Rule not found exception
    /// </summary>
    class RuleNotFoundException : Exception
    {
        /// <summary>
        /// Initialize a new RuleNotFoundException
        /// </summary>
        /// <param name="Message">Exception message</param>
        public RuleNotFoundException( string Message ) : base( Message ) { }
    }
}
