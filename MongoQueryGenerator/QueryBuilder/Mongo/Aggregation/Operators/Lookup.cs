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
        public string From { get; set; }
        /// <summary>
        /// Local field that contains a reference to the 'From' collection
        /// </summary>
        public string LocalField { get; set; }
        /// <summary>
        /// Field in the <see cref="From"/> collection to match the value of <see cref="LocalField"/>
        /// </summary>
        public string ForeignField { get; set; }
        /// <summary>
        /// Alias of the joined data
        /// </summary>
        public string As { get; set; }
        /// <summary>
        /// Pipeline to be executed on the <see cref="From"/> collection
        /// </summary>
        public List<BaseOperator> Pipeline { get; set; }
        /// <summary>
        /// Variables to be accesible in the pipeline
        /// </summary>
        public Dictionary<string, string> Let { get; set; }
        #endregion

        #region Methods
        /// <summary>
        /// Generates a JavaScript representation of this operator
        /// </summary>
        /// <returns></returns>
        public override string ToJavaScript()
        {
            return ToJSCode().ToString();
        }

        public override JSCode ToJSCode()
        {
            Dictionary<string, object> Attrs = new Dictionary<string, object>();

            Attrs.Add( "from", From );
            Attrs.Add( "as", As );

            if ( Pipeline.Count > 0 )
            {
                Attrs.Add( "let", new JSObject( Let.ToDictionary( I => I.Key, I => (object)I.Value ) ) );
                List<object> PipelineJS = new List<object>();
                foreach ( BaseOperator Op in Pipeline )
                {
                    PipelineJS.Add( Op.ToJSCode() );
                }
                Attrs.Add( "pipeline", new JSArray( PipelineJS ) );
            }
            else
            {
                Attrs.Add( "foreignField", ForeignField );
                Attrs.Add( "localField", LocalField );
            }

            return new JSObject( "$lookup", Attrs );
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
