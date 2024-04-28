using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataFaker.Models.CMS
{
    /// <summary>
    /// Represents a store in the CMS database
    /// </summary>
    [Table("stores")]
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
