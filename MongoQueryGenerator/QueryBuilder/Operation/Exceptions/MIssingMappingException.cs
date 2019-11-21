using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueryBuilder.Operation.Exceptions
{
    /// <summary>
    /// Thrown when a required mapping is missing
    /// </summary>
    public class MissingMappingException : Exception
    {
        #region Constructor
        public MissingMappingException( string Message ) : base ( Message )
        {

        }
        #endregion
    }
}
