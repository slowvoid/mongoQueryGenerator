using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueryBuilder.Javascript
{
    /// <summary>
    /// Represents a Javascript array
    /// </summary>
    public class JSArray : JSCode
    {
        #region Properties
        /// <summary>
        /// Array items
        /// </summary>
        public List<object> Items { get; set; }
        #endregion

        #region Methods
        /// <summary>
        /// Returns a string representing this instance
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            List<string> ItemsAsString = new List<string>();

            foreach ( object Item in Items )
            {
                if ( Item is string )
                {
                    // Surround with quotes
                    ItemsAsString.Add( $"\"{Item}\"" );
                }
                else if ( Item is bool )
                {
                    // Add raw value
                    ItemsAsString.Add( string.Format( "{0}", (bool)Item ? "true" : "false" ) );
                }
                else if ( Item is JSCode )
                {
                    ItemsAsString.Add( Item.ToString() );
                }
                else if ( Item is int )
                {
                    ItemsAsString.Add( Item.ToString() );
                }
                else
                {
                    ItemsAsString.Add( (string)Item );
                }
            }

            return $"[{string.Join( ",", ItemsAsString )}]";
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Initialize a new instance of JSArray
        /// </summary>
        /// <param name="Items">Array items</param>
        public JSArray( List<object> Items )
        {
            this.Items = Items;
        }
        /// <summary>
        /// Initialize a new instance of JSArray with no items
        /// </summary>
        public JSArray()
        {
            this.Items = new List<object>();
        }
        #endregion
    }
}
