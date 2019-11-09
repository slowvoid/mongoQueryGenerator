using Newtonsoft.Json;
using QueryBuilder.Mongo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueryBuilder.Query
{
    /// <summary>
    /// Defines a find command
    /// </summary>
    public class FindCommand : QueryCommand
    {
        #region Properties
        /// <summary>
        /// Target collection
        /// </summary>
        public MongoDBCollection TargetCollection { get; set; }
        /// <summary>
        /// Filters
        /// </summary>
        public Dictionary<string, object> Filters { get; set; }
        #endregion

        #region Methods
        /// <summary>
        /// Generate the corresponding MongoDB query
        /// </summary>
        /// <returns></returns>
        public override string Generate()
        {
            string FilterString = string.Empty;

            if ( Filters.Count > 0 )
            {
                FilterString = JsonConvert.SerializeObject( Filters );
            }

            return $"db.{TargetCollection.Name}.find({FilterString})";
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Initialize a new FindCommand instance
        /// </summary>
        /// <param name="TargetCollection">Target collection</param>
        /// <param name="Filters">Filters</param>
        public FindCommand( MongoDBCollection TargetCollection, Dictionary<string, object> Filters )
        {
            this.TargetCollection = TargetCollection;
            this.Filters = Filters;
        }
        #endregion
    }
}
