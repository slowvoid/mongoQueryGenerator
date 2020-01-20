using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataFaker.Models.Prograd3
{
    [Table("endereco")]
    public class Endereco : Model
    {
        [Key]
        public int codend_end { get; set; }
        public string logradouro_end { get; set; }
        public string bairro_end { get; set; }
        public string compl_end { get; set; }
        public string codcidade_end { get; set; }
        public string cep_end { get; set; }
        public int aluno_id { get; set; }
    }
}
