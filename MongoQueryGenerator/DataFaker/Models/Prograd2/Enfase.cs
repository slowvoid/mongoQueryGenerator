using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataFaker.Models.Prograd2
{
    [Table("enfasegrad")]
    public class Enfase : Model
    {
        [Key]
        public int codenf_enf { get; set; }
        public string nomeenf_enf { get; set; }
        public string siglaenf_enf { get; set; }
        public int codcur_enf { get; set; }
    }
}
