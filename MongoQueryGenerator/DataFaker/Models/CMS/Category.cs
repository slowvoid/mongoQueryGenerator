﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataFaker.Models.CMS
{
    /// <summary>
    /// Represents a category in the cms database
    /// </summary>
    [Table("categories")]
    public class Category : Model
    {
        /// <summary>
        /// Unique category id
        /// </summary>
        public int CategoryID { get; set; }
        /// <summary>
        /// Category name
        /// </summary>
        public string CategoryName { get; set; }
    }
}
