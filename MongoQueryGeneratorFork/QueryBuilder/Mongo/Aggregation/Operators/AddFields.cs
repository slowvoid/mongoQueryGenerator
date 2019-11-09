using QueryBuilder.Javascript;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueryBuilder.Mongo.Aggregation.Operators
{
    /// <summary>
    /// Represents the $addFields aggregation stage
    /// </summary>
    public class AddFieldsOperator : MongoDBOperator
    {
        #region Properties
        /// <summary>
        /// Attribute name <> value pairs to be added
        /// Use dot notation to add depth to attributes
        /// </summary>
        public Dictionary<string, JSCode> Attributes { get; set; }
        #endregion

        #region Methods
        /// <summary>
        /// Generate a javascript compatible representation of this instance
        /// </summary>
        /// <returns></returns>
        public override string ToJavaScript()
        {
            return ToJSCode().ToString();
        }
        /// <summary>
        /// Generates a Javascript code object representing this instance
        /// </summary>
        /// <returns></returns>
        public override JSCode ToJSCode()
        {
            Dictionary<string, object> AddFieldsAttrs = new Dictionary<string, object>();
            AddFieldsAttrs.Add( "$addFields", new JSObject( Attributes.ToDictionary( I => I.Key, I => (object)I.Value ) ) );

            return new JSObject( AddFieldsAttrs );
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Initialize a new instance of AddFields class
        /// </summary>
        public AddFieldsOperator( Dictionary<string, JSCode> Attributes )
        {
            this.Attributes = Attributes;
        }
        #endregion
    }
}
