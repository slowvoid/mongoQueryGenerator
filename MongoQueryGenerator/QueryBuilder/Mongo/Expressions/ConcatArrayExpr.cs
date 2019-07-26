using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QueryBuilder.Javascript;

namespace QueryBuilder.Mongo.Expressions
{
    /// <summary>
    /// References a $contactArrays project expression
    /// </summary>
    public class ConcatArrayExpr : ProjectExpression
    {
        #region Properties
        /// <summary>
        /// Fields to merge
        /// </summary>
        public IEnumerable<object> Fields { get; set; }
        #endregion

        #region Methods
        /// <summary>
        /// Generate javascript representation of this instance
        /// </summary>
        /// <returns></returns>
        public override JSCode ToJSCode()
        {
            JSArray FieldArray = new JSArray( Fields.ToList() );
            Dictionary<string, object> CodeObject = new Dictionary<string, object>();
            CodeObject.Add( "$concatArrays", FieldArray );

            return new JSObject( CodeObject );
        }
        #endregion

        #region Constructor
        public ConcatArrayExpr( IEnumerable<object> Fields )
        {
            // Iterate items and add '$' before each name
            List<Object> FieldList = new List<object>();
            foreach ( object Field in Fields )
            {
                // Ignore everything that is not a string
                if ( Field is string FieldName )
                {
                    FieldList.Add( $"${FieldName}" );
                }
            }

            this.Fields = FieldList;
        }
        #endregion
    }
}
