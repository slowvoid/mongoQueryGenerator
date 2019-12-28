using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataFaker.Models.CMS
{
    /// <summary>
    /// Represents a store in the CMS database
    /// </summary>
    public class Store : Model
    {
        /// <summary>
        /// Unique store id
        /// </summary>
        public int StoreID { get; set; }
        /// <summary>
        /// Store name
        /// </summary>
        public string StoreName { get; set; }
    }
}
