using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataFaker.Models.Prograd
{
    [Table("cursograd")]
    public class CursoGrad : Model
    {
        #region Properties
        public int codcur_cur { get; set; }
        public string sigla_cur { get; set; }
        public string nomecur_cur { get; set; }
        public int codperiodo_cur { get; set; }
        public int prazomin_cur { get; set; }
        public int prazomax_cur { get; set; }
        public int prazomed_cur { get; set; }
        public DateTime datacriacao_cur { get; set; }
        public char ativo_cur { get; set; }
        public int userid_cur { get; set; }
        public DateTime datatu_cur { get; set; }
        public string codigoenade_cur { get; set; }
        public DateTime datainicio_cur { get; set; }
        public DateTime datafim_cur { get; set; }
        public char regime_cur { get; set; }
        public string coordenacao_cur { get; set; }
        public int nrovagas_cur { get; set; }
        #endregion
    }
}
