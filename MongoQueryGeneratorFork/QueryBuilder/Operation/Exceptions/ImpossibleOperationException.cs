using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueryBuilder.Operation.Exceptions
{
    /// <summary>
    /// Impossible Operation Exception
    /// </summary>
    public class ImpossibleOperationException : Exception
    {
        #region Constructors
        /// <summary>
        /// Initialize a new ImpossibleOperationException exception
        /// </summary>
        /// <param name="Message"></param>
        public ImpossibleOperationException( string Message ) : base( Message ) { }
        #endregion
    }
}
