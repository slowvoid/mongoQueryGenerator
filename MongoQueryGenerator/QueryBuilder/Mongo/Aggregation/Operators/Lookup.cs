using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using QueryBuilder.Javascript;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueryBuilder.Mongo.Aggregation.Operators
{
    /// <summary>
    /// Represents the Lookup Operator
    /// </summary>
    public class LookupOperator : BaseOperator
    {
        #region Properties
        /// <summary>
        /// Collection to lookup
        /// </summary>
        [BsonElement("from")]
        public string From { get; set; }
        /// <summary>
        /// Local field that contains a reference to the 'From' collection
        /// </summary>
        [BsonElement("localField")]
        [BsonIgnoreIfNull]
        public string LocalField { get; set; }
        /// <summary>
        /// Field in the <see cref="From"/> collection to match the value of <see cref="LocalField"/>
        /// </summary>
        [BsonElement("foreignField")]
        [BsonIgnoreIfNull]
        public string ForeignField { get; set; }
        /// <summary>
        /// Alias of the joined data
        /// </summary>
        [BsonElement("as")]
        public string As { get; set; }
        /// <summary>
        /// Pipeline to be executed on the <see cref="From"/> collection
        /// </summary>
        [BsonElement("pipeline")]
        [BsonIgnoreIfNull]
        public List<BaseOperator> Pipeline { get; set; }
        /// <summary>
        /// Variables to be accesible in the pipeline
        /// </summary>
        [BsonElement("let")]
        [BsonIgnoreIfNull]
        public Dictionary<string, string> Let { get; set; }
        #endregion

        #region Methods
        /// <summary>
        /// Generates a JavaScript representation of this operator
        /// </summary>
        /// <returns></returns>
        public override string ToJavaScript()
        {
            BsonElement FromElement = new BsonElement( "from", From );
            BsonElement AsElement = new BsonElement( "as", As );

            if ( Pipeline.Count > 0 )
            {
                ForeignField = null;
                LocalField = null;

                List<string> PipelineOperations = new List<string>();
                foreach ( BaseOperator PipelineOp in Pipeline )
                {
                    PipelineOperations.Add( PipelineOp.ToJavaScript() );
                }


                BsonElement LetElement = new BsonElement( "let", new BsonDocument( Let ) );
                BsonElement PipelineElement = new BsonElement( "pipeline", $"[{string.Join( ",", PipelineOperations )}]" );

                List<BsonElement> Elements = new List<BsonElement> { FromElement, AsElement, LetElement, PipelineElement };

                return new BsonDocument( new BsonElement( "$lookup", new BsonDocument( Elements ) ) ).ToString();
            }
            else
            {
                Pipeline = null;
                Let = null;

                BsonElement ForeignElement = new BsonElement( "foreignField", ForeignField );
                BsonElement LocalElement = new BsonElement( "localField", LocalField );

                List<BsonElement> Elements = new List<BsonElement> { FromElement, AsElement, ForeignElement, LocalElement };

                return new BsonDocument( new BsonElement( "$lookup", new BsonDocument( Elements ) ) ).ToString();
            }
        }

        public override JSCode ToJSCode()
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Constructor
        /// <summary>
        /// Initialize a new LookupOperator instance
        /// </summary>
        public LookupOperator()
        {
            Pipeline = new List<BaseOperator>();
            Let = new Dictionary<string, string>();
        }
        #endregion
    }
}
