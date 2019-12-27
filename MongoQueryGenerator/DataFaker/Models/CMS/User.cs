using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataFaker.Models.CMS
{
    /// <summary>
    /// Represents an user on the CMS database
    /// </summary>
    [Table("users")]
    public class User : Model
    {
        /// <summary>
        /// User unique id
        /// </summary>
        public int UserID { get; set; }
        /// <summary>
        /// User name
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// User email
        /// </summary>
        public string Email { get; set; }
    }
}
