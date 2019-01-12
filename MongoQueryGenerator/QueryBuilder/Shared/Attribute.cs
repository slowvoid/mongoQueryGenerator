using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueryBuilder.Shared
{
    /// <summary>
    /// Represents a data attribute
    /// </summary>
    public class DataAttribute
    {
        #region Properties
        /// <summary>
        /// Type of data stored by this attribute
        /// 
        /// See: <see cref="DataType"/>
        /// </summary>
        public DataType OfType { get; set; }
        /// <summary>
        /// Gets wheter this attribute is multivalued. (Array like)
        /// </summary>
        public bool IsMultiValued
        {
            get
            {
                return OfType == DataType.ARRAY;
            }
        }
        /// <summary>
        /// Attribute name
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Attribute parent
        /// </summary>
        public IDataAttributeParent Parent { get; set; }
        #endregion
    }
}
