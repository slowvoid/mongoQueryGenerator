using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QueryBuilder.Javascript;

namespace QueryBuilder.Mongo.Expressions
{
    /// <summary>
    /// Represents a $map operation
    /// </summary>
    public class MapExpr : ProjectExpression
    {
        #region Properties
        /// <summary>
        /// Input field
        /// </summary>
        public string Input { get; set; }
        /// <summary>
        /// Alias to access each memeber of the array
        /// </summary>
        public string As { get; set; } 
        /// <summary>
        /// Attribute map
        /// </summary>
        public Dictionary<string, JSCode> In { get; set; }
        #endregion

        #region Methods
        /// <summary>
        /// Generates a Javascript code object representing this instance
        /// </summary>
        /// <returns></returns>
        public override JSCode ToJSCode()
        {
            Dictionary<string, object> Attrs = new Dictionary<string, object>();
            Attrs.Add( "input", Input );
            Attrs.Add( "as", As );
            Attrs.Add( "in", new JSObject( In.ToDictionary( I => I.Key, I => (object)I.Value ) ) );

            return new JSObject( "$map", Attrs );
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of MapExpr
        /// </summary>
        /// <param name="Input"></param>
        /// <param name="As"></param>
        /// <param name="In"></param>
        public MapExpr(string Input, string As, Dictionary<string, JSCode> In)
        {
            if ( !Input.StartsWith("$") )
            {
                Input = $"${Input}";
            }

            this.Input = Input;
            this.As = As;
            this.In = In;
        }
        #endregion
    }
}
