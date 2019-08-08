using QueryBuilder.ER;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueryBuilder.Map
{
    /// <summary>
    /// Provides common behavior for model maps
    /// </summary>
    public interface IModelMap
    {
        /// <summary>
        /// Get the rule value for an attribute in the given ER element
        /// </summary>
        /// <param name="NameOrAlias"></param>
        /// <param name="AttributeName"></param>
        /// <returns></returns>
        string GetRuleValue( string NameOrAlias, string AttributeName );
    }
}
