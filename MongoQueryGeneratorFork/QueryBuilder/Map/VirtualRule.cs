using QueryBuilder.ER;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueryBuilder.Map
{
    /// <summary>
    /// Represents the virtual mapping of the output document
    /// </summary>
    public class VirtualRule
    {
        #region Properties
        /// <summary>
        /// Entity represent in this ruleset
        /// </summary>
        public BaseERElement SourceERElement { get; set; }
        /// <summary>
        /// Entity alias
        /// </summary>
        public string Alias { get; set; }
        /// <summary>
        /// Map rules
        /// </summary>
        public Dictionary<string, string> Rules { get; set; }
        #endregion

        #region Methods
        /// <summary>
        /// Add a new rule
        /// </summary>
        /// <param name="Key"></param>
        /// <param name="Value"></param>
        public void AddRule(string Key, string Value)
        {
            Rules.Add( Key, Value );
        }
        /// <summary>
        /// Converts this instance to string
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            foreach ( KeyValuePair<string,string> Rule in Rules )
            {
                sb.Append( $"{Rule.Value}{Environment.NewLine}" );
            }

            return sb.ToString();
        }
        #endregion

        #region Constructor
        /// <summary>
        /// Initialize a new instance of VirtualRule
        /// </summary>
        /// <param name="SourceEntity"></param>
        /// <param name=""></param>
        public VirtualRule(BaseERElement SourceEntity, string Alias = null)
        {
            this.SourceERElement = SourceEntity;
            this.Alias = Alias;
            Rules = new Dictionary<string, string>();
        }
        #endregion
    }
}
