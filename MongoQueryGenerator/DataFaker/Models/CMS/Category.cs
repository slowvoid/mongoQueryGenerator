using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataFaker.Models.CMS
{
    /// <summary>
    /// Represents a category in the cms database
    /// </summary>
    public class Category : Model
    {
        /// <summary>
        /// Unique category id
        /// </summary>
        public int CategoryID { get; set; }
        /// <summary>
        /// Category name
        /// </summary>
        public string Name { get; set; }
    }
}
