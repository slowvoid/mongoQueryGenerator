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
    public class ProgradContext2 : DbContext
    {
        #region Models
        public DbSet<Models.Prograd2.CursoGrad> Cursos { get; set; }
        public DbSet<Models.Prograd2.AlunoGrad> Alunos { get; set; }
        public DbSet<Models.Prograd2.DisciplinaGrad> Disciplinas { get; set; }
        public DbSet<Models.Prograd2.Endereco> Enderecos { get; set; }
        public DbSet<Models.Prograd2.Enfase> Enfases { get; set; }
        public DbSet<Models.Prograd2.Grade> Grades { get; set; }
        public DbSet<Models.Prograd2.Matricula> Matriculas { get; set; }
        #endregion

        #region Constructors
        public ProgradContext2() : base( "ProgradContext2" ) { }
        public ProgradContext2( DbConnection existingConnection, bool contextOwnsConnection ) : base( existingConnection, contextOwnsConnection ) { }
        #endregion
    }
}
