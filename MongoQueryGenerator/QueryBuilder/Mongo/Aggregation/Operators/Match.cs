using QueryBuilder.Javascript;
using QueryBuilder.Mongo.Expressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueryBuilder.Mongo.Aggregation.Operators
{
    /// <summary>
    /// Represents the Match aggregation stage
    /// </summary>
    public class MatchOperator : MongoDBOperator
    {
        #region Properties
        /// <summary>
        /// Expression to execute
        /// </summary>
        public BaseExpression Expression { get; set; }
        /// <summary>
        /// Fields to match against
        /// </summary>
        public Dictionary<string, object> FieldsToMatch { get; set; }
        #endregion

        #region Overrides
        /// <summary>
        /// Generates a JavaScript compatible version of this instance
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
            Dictionary<string, object> MatchAttrs = new Dictionary<string, object>();

            if ( Expression != null )
            {
                MatchAttrs.Add( "$match", Expression.ToJSCode() );
            }
            else
            {
                MatchAttrs.Add( "$match", new JSObject( FieldsToMatch ).ToString() );
            }

            return new JSObject( MatchAttrs );
        }
        #endregion

        #region Methods
        /// <summary>
        /// Creates a MatchOperator with inner Expr and EqExpr for the given fields
        /// </summary>
        /// <param name="SourceField"></param>
        /// <param name="TargetField"></param>
        /// <returns></returns>
        public static MatchOperator CreateLookupMatch( string SourceField, string TargetField )
        {
            EqExpr EqOp = new EqExpr( $"${SourceField}", $"$${TargetField}" );
            return new MatchOperator( new Expr( EqOp ) );
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Initialize a new instance of Match
        /// </summary>
        public MatchOperator()
        {
            FieldsToMatch = new Dictionary<string, object>();
        }
        /// <summary>
        /// Initialize a new instance of Match
        /// </summary>
        /// <param name="FieldsToMatch">Fields to match against</param>
        public MatchOperator( Dictionary<string, object> FieldsToMatch )
        {
        this.FieldsToMatch = FieldsToMatch;
        }
        /// <summary>
        /// Initialize a new instance of Match
        /// </summary>
        /// <param name="Expression">Expression to be evalueted</param>
        public MatchOperator( BaseExpression Expression )
        {
            this.Expression = Expression;
            FieldsToMatch = new Dictionary<string, object>();
        }
        #endregion
    }
}
