using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataFaker.Models.Prograd
{
    [Table("gradegrad")]
    public class Grade : Model
    {
        [Key]
        public int gradegrad_id { get; set; }
        public string perfil_grd { get; set; }
        public int userid_grd { get; set; }
        public int discipgrad_id { get; set; }
        public int enfgrad_id { get; set; }
    }
}
