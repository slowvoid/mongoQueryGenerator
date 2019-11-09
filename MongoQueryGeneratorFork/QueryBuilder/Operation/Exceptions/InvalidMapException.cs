using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueryBuilder.Operation.Exceptions
{
    /// <summary>
    /// Invalid map exception
    /// </summary>
    public class InvalidMapException : Exception
    {
        #region Constructors
        /// <summary>
        /// Initialize a new instance of InvalidMapException
        /// </summary>
        /// <param name="Message"></param>
        public InvalidMapException( string Message ) : base( Message )
        {
        }
        #endregion
    }
}
