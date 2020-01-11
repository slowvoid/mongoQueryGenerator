using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataFaker.Models.Prograd
{
    [Table("matriculagrad")]
    public class Matricula : Model
    {
        [Key]
        public int codmatr_matr { get; set; }
        public int anoini_matr { get; set; }
        public int semiini_matr { get; set; }
        public int codalu_matr { get; set; }
        public int codenf_matr { get; set; }
    }
}
