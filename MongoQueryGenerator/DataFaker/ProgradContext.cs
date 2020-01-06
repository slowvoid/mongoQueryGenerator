using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataFaker
{
    public class ProgradContext : DbContext
    {
        #region Constructors
        public ProgradContext() : base( "ProgradContext") { }
        public ProgradContext( DbConnection existingConnection, bool contextOwnsConnection ) : base( existingConnection, contextOwnsConnection ) { }
        #endregion
    }
}
