using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataFaker.Models.CMS
{
    /// <summary>
    /// Represents a product on the CMS database
    /// </summary>
    [Table( "products" )]
    public class Product : Model
    {
        /// <summary>
        /// Product Unique ID
        /// </summary>
        public int ProductID { get; set; }
        /// <summary>
        /// Product title
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// Product description
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// Id of the user who published this product
        /// </summary>
        public int UserID { get; set; }
        /// <summary>
        /// Id of the category in which this product belongs to
        /// </summary>
        public int CategoryID { get; set; }
        /// <summary>
        /// Id of the store in which this product belongs to
        /// </summary>
        public int StoreID { get; set; }
        /// <summary>
        /// Product price
        /// </summary>
        public int Price { get; set; }
    }
}
