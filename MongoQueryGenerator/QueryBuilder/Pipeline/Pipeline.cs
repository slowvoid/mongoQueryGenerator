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
        /// Available Datasets
        /// </summary>
        public List<Dataset> Datasets { get; set; }
        #endregion
    }
}
