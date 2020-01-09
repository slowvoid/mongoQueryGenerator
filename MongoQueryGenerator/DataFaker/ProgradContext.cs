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
    public class ProgradContext : DbContext
    {
        #region Models
        public DbSet<CursoGrad> Cursos { get; set; }
        #endregion

        #region Constructors
        public ProgradContext() : base( "ProgradContext") { }
        public ProgradContext( DbConnection existingConnection, bool contextOwnsConnection ) : base( existingConnection, contextOwnsConnection ) { }
        #endregion
    }
}
