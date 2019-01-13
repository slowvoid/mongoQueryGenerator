using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using QueryBuilder.ER;

namespace QueryBuilder.Pipeline
{
    /// <summary>
    /// Defines a pipeline dataset
    /// </summary>
    public class Dataset
    {
        #region Properties
        /// <summary>
        /// ER Element associated with this document
        /// </summary>
        public BaseERElement Element { get; set; }
        /// <summary>
        /// Attributes associated with this document and their visibility status
        /// </summary>
        public Dictionary<string, bool> Attributes { get; set; }
        #endregion

        #region Methods
        /// <summary>
        /// Return a list containing all visible attributes
        /// </summary>
        /// <returns></returns>
        public List<string> GetVisibleAttributes()
        {
            IEnumerable<string> VisibleElements = from Attr in Attributes
                                                  where Attr.Value
                                                  select Attr.Key;

            return VisibleElements.ToList();
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Initialize a new Dataset instance
        /// </summary>
        /// <param name="Element"></param>
        public Dataset( BaseERElement Element )
        {
            this.Element = Element;
            Attributes = new Dictionary<string, bool>();
        }
        /// <summary>
        /// Initializa a new Dataset instance
        /// </summary>
        /// <param name="Element"></param>
        /// <param name="Attributes"></param>
        public Dataset( BaseERElement Element, Dictionary<string, bool> Attributes )
        {
            this.Element = Element;
            this.Attributes = Attributes;
        }
        #endregion
    }
}
