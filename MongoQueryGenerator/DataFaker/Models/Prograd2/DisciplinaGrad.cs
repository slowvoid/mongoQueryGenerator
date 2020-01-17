using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataFaker.Models.Prograd2
{
    [Table("disciplinagrad")]
    public class DisciplinaGrad : Model
    {
        [Key]
        public int coddiscip_discip { get; set; }
        public string nome_discip { get; set; }
    }
}
