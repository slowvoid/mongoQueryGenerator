using QueryBuilder.ER;
using QueryBuilder.Map;
using QueryBuilder.Mongo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueryAnalyzer
{
    /// <summary>
    /// Provides required data for query testing
    /// </summary>
    public class RequiredDataContainer
    {
        #region Properties
        /// <summary>
        /// The ER Model
        /// </summary>
        public ERModel EntityRelationshipModel { get; set; }
        /// <summary>
        /// MongoDB Schema
        /// </summary>
        public MongoSchema MongoDBSchema { get; set; }
        /// <summary>
        /// Mapping between ER and MongoDB
        /// </summary>
        public ModelMapping ERMongoMapping { get; set; }
        #endregion

        #region Constructor
        /// <summary>
        /// Initializes a new RequiredDataContainer instance
        /// </summary>
        /// <param name="EntityRelationshipModel"></param>
        /// <param name="MongoDBSchema"></param>
        /// <param name="ERMongoMapping"></param>
        public RequiredDataContainer(ERModel EntityRelationshipModel, MongoSchema MongoDBSchema, ModelMapping ERMongoMapping)
        {
            this.EntityRelationshipModel = EntityRelationshipModel;
            this.MongoDBSchema = MongoDBSchema;
            this.ERMongoMapping = ERMongoMapping;
        }
        #endregion
    }
}
