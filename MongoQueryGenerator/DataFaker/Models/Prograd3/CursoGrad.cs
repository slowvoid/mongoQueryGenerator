using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataFaker.Models.Prograd3
{
    [Table("cursograd")]
    public class CursoGrad : Model
    {
        #region Properties
        [Key]
        public int codcur_cur { get; set; }

        public string sigla_cur { get; set; }
        public string nomecur_cur { get; set; }
        #endregion
    }
}
