using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueryBuilder.Javascript
{
    /// <summary>
    /// Represents a Javascript object
    /// </summary>
    public class JSObject : JSCode
    {
        #region Properties
        /// <summary>
        /// Object key:value pairs
        /// </summary>
        public Dictionary<string, object> KeyValuePairs { get; set; }
        #endregion

        #region Methods
        /// <summary>
        /// Returns a string representing this instance
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            List<string> KeyPairs = new List<string>();

            foreach ( KeyValuePair<string, object> Item in KeyValuePairs )
            {
                if ( Item.Value is string )
                {
                    KeyPairs.Add( $"{Item.Key}: \"{Item.Value}\"" );
                }
                else if ( Item.Value is bool )
                {
                    KeyPairs.Add( $"{Item.Key}: {( ( (bool)Item.Value ) ? "true" : "false" )}" );
                }
                else if ( Item.Value is JSArray )
                {
                    KeyPairs.Add( $"{Item.Key}: {Item.Value.ToString()}" );
                }
                else
                {
                    KeyPairs.Add( $"{Item.Key}: {Item.Value}" );
                }
            }

            return string.Format( "{{{0}}}", string.Join( ",", KeyPairs ) );
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Initialize a new JSObject instance
        /// </summary>
        public JSObject()
        {
            this.KeyValuePairs = new Dictionary<string, object>();
        }
        /// <summary>
        /// Initialize a new JSObject instance
        /// </summary>
        /// <param name="KeyValuePairs">Key:Value pairs</param>
        public JSObject( Dictionary<string, object> KeyValuePairs )
        {
            this.KeyValuePairs = KeyValuePairs;
        }
        /// <summary>
        /// Initialize a new JSObject instance
        /// </summary>
        /// <param name="Key"></param>
        /// <param name="KeyValuePairs"></param>
        public JSObject( string Key, Dictionary<string, object> KeyValuePairs )
        {
            Dictionary<string, object> Attrs = new Dictionary<string, object>();
            Attrs.Add( Key, KeyValuePairs );

            this.KeyValuePairs = Attrs;
        }
        #endregion
    }
}
