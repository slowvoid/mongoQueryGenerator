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
    public class ResearchCMSContext2 : DbContext
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
            
        public ResearchCMSContext2() : base("ResearchContext2") { }
        public ResearchCMSContext2(DbConnection existingConnection, bool contextOwnsConnection) : base(existingConnection, contextOwnsConnection) { }
    }
}
