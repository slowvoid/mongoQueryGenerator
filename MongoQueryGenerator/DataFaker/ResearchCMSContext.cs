using DataFaker.Models.CMS;
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
    public class ResearchCMSContext : DbContext
    {
        #region Context Tables
        /// <summary>
        /// Users
        /// </summary>
        public DbSet<User> Users { get; set; }
        /// <summary>
        /// Products
        /// </summary>
        public DbSet<Product> Products { get; set; }
        /// <summary>
        /// Stores
        /// </summary>
        public DbSet<Store> Stores { get; set; }
        /// <summary>
        /// Categories
        /// </summary>
        public DbSet<Category> Categories { get; set; }
        #endregion

        public ResearchCMSContext() : base("ResearchContext") { }
        public ResearchCMSContext(DbConnection existingConnection, bool contextOwnsConnection) : base(existingConnection, contextOwnsConnection) { }
    }
}
