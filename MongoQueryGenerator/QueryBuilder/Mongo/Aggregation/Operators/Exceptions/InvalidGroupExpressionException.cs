using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueryBuilder.Mongo.Aggregation.Operators.Exceptions
{
    /// <summary>
    /// Thrown when trying to use an invalid group expression
    /// </summary>
    public class InvalidGroupExpressionException : Exception
    {
        #region Constructor
        public InvalidGroupExpressionException( string inMessage ) : base( inMessage ) { }
        #endregion
    }
}
