using DataFaker.Models.Prograd;
using MySql.Data.Entity;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataFaker
{
    [DbConfigurationType( typeof( MySqlEFConfiguration ) )]
    public class ProgradContext3 : DbContext
    {
        #region Models
        public DbSet<Models.Prograd3.CursoGrad> Cursos { get; set; }
        public DbSet<Models.Prograd3.AlunoGrad> Alunos { get; set; }
        public DbSet<Models.Prograd3.DisciplinaGrad> Disciplinas { get; set; }
        public DbSet<Models.Prograd3.Endereco> Enderecos { get; set; }
        public DbSet<Models.Prograd3.Enfase> Enfases { get; set; }
        public DbSet<Models.Prograd3.Grade> Grades { get; set; }
        public DbSet<Models.Prograd3.Matricula> Matriculas { get; set; }
        public DbSet<Models.Prograd3.Matriculado> Matriculados { get; set; }
        #endregion

        #region Constructors
        public ProgradContext3() : base( "ProgradContext3" ) { }
        public ProgradContext3( DbConnection existingConnection, bool contextOwnsConnection ) : base( existingConnection, contextOwnsConnection ) { }
        #endregion
    }
}
