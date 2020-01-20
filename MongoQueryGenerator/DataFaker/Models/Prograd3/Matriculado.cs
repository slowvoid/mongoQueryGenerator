using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataFaker.Models.Prograd3
{
    [Table("matriculas")]
    public class Matriculado : Model
    {
        #region Properties
        [Key]
        public int codigo_pk { get; set; }
        public int cod_matricula { get; set; }
        public int cod_aluno { get; set; }
        public int cod_enfase { get; set; }
        #endregion
    }
}
