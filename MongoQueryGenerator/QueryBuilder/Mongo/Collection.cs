using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueryBuilder.Mongo
{
    /// <summary>
    /// Represents a MongoDB collection
    /// </summary>
    public class Collection
    {
        #region Properties
        /// <summary>
        /// Collection name
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Document schema
        /// </summary>
        public Document DocumentSchema { get; set; }
        #endregion
    }
}
