using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueryBuilder.Operation
{
    /// <summary>
    /// Query generator execution pipeline
    /// </summary>
    public class Pipeline
    {
        #region Properties
        /// <summary>
        /// List of Operations to perform
        /// </summary>
        public List<BaseOperation> Operations { get; set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Initialize a new instance of Pipeline class
        /// </summary>
        public Pipeline( List<BaseOperation> Operations )
        {
            this.Operations = Operations;
        }
        #endregion
    }
}
