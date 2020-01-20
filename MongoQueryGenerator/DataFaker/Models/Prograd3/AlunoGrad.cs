using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataFaker.Models.Prograd3
{
    [Table( "alunograd" )]
    public class AlunoGrad : Model
    {
        [Key]
        public int codalu_alug { get; set; }

        public string nomealu_alug { get; set; }
        public string cpf_alug { get; set; }
    }
}
