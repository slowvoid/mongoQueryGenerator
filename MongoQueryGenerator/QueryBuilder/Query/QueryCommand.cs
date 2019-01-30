using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueryBuilder.Query
{
    /// <summary>
    /// Represents a query command to be generated
    /// </summary>
    public class QueryCommand
    {
        #region Methods
        /// <summary>
        /// Generates the corresponding MongoDB query
        /// </summary>
        /// <returns></returns>
        public virtual string Generate()
        {
            return string.Empty;
        }
        #endregion
    }
}
