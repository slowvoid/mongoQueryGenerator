using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueryBuilder.Pipeline
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
        public List<Operation> Operations { get; set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Initialize a new instance of Pipeline class
        /// </summary>
        public Pipeline( List<Operation> Operations )
        {
            this.Operations = Operations;
        }
        #endregion
    }
}
