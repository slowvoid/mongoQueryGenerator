using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueryBuilder.Operation.Exceptions
{
    /// <summary>
    /// Not related entities exception
    /// </summary>
    public class NotRelatedException : Exception
    {
        public NotRelatedException( string Message ) : base( Message ) { }
    }
}
