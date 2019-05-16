using MongoDB.Bson;
using QueryBuilder.Mongo.Expressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueryBuilder.Mongo.Aggregation.Operators
{
    /// <summary>
    /// Represents the $project aggregation stage
    /// </summary>
    public class Project : BaseOperator
    {
        #region Properties
        /// <summary>
        /// Attributes and their respective visibility status
        /// </summary>
        public Dictionary<string, ProjectExpression> Attributes { get; set; }
        #endregion

        #region Methods
        /// <summary>
        /// Creates a javascript compatible object
        /// </summary>
        /// <returns></returns>
        public override string ToJavaScript()
        {
            List<BsonElement> ProjectAttributes = new List<BsonElement>();

            foreach ( KeyValuePair<string, ProjectExpression> Attribute in Attributes )
            {
                ProjectAttributes.Add( new BsonElement( Attribute.Key, Attribute.Value.ToJavaScript() ) );
            }

            BsonDocument ProjectAttr = new BsonDocument( ProjectAttributes );
            BsonDocument ProjectDoc = new BsonDocument( new BsonElement( "$project", ProjectAttr ) );

            return ProjectDoc.ToString();
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Initialize a new instance of Project class
        /// </summary>
        public Project( Dictionary<string, ProjectExpression> Attributes )
        {
            this.Attributes = Attributes;
        }
        #endregion
    }
}
