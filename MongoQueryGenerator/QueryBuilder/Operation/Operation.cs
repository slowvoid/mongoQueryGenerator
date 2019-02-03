using QueryBuilder.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using QueryBuilder.Map;

namespace QueryBuilder.Operation
{
    /// <summary>
    /// Defines a query operation (JOIN, PROJECT, etc)
    /// </summary>
    public abstract class BaseOperation
    {
        #region Properties
        #endregion

        #region Methods
        /// <summary>
        /// Run operation
        /// </summary>
        /// <returns></returns>
        public virtual QueryCommand Run()
        {
            return new QueryCommand();
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Initialize a new instance of Operation class
        /// </summary>
        public BaseOperation()
        {
        }
        #endregion
    }
}
