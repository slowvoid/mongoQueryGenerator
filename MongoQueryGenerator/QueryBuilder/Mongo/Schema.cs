﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueryBuilder.Mongo
{
    public class MongoSchema
    {
        #region Properties
        /// <summary>
        /// Schema name
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Collections
        /// </summary>
        public List<Collection> Collections { get; set; }
        #endregion

        

        #region Constructors
        /// <summary>
        /// Initialize a new Instance of Schema
        /// </summary>
        /// <param name="Name">Schema name</param>
        /// <param name="Collections">Attached collections</param>
        public MongoSchema( string Name, List<Collection> Collections )
        {
            this.Name = Name;
            this.Collections = Collections;
        }
        #endregion
    }
}