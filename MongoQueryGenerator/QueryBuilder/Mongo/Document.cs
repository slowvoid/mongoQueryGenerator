using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using QueryBuilder.Shared;

namespace QueryBuilder.Mongo
{
    /// <summary>
    /// Represents a MongoDB Document
    /// </summary>
    public class Document
    {
        #region Properties
        /// <summary>
        /// ID Attribute (_id)
        /// </summary>
        public object ID { get; set; }
        /// <summary>
        /// Other attributes
        /// </summary>
        public List<DataAttribute> Attributes { get; set; }
        #endregion
    }
}
