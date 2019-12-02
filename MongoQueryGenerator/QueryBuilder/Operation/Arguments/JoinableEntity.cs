using QueryBuilder.ER;
using QueryBuilder.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueryBuilder.Operation.Arguments
{
    /// <summary>
    /// Represents an Entity in a queryable context
    /// </summary>
    public class QueryableEntity
    {
        #region Properties
        /// <summary>
        /// Entity alias
        /// </summary>
        public string Alias { get; set; }
        /// <summary>
        /// Entity to be joined
        /// </summary>
        public BaseERElement Element { get; set; }
        #endregion

        #region Methods
        /// <summary>
        /// Returns the Element name
        /// </summary>
        /// <returns></returns>
        public string GetName()
        {
            if ( Element != null )
            {
                return Element.Name;
            }

            return string.Empty;
        }
        /// <summary>
        /// Return an attribute from the Element
        /// </summary>
        /// <returns></returns>
        public DataAttribute GetAttribute( string AttributeName )
        {
            if ( Element != null )
            {
                return Element.GetAttribute( AttributeName );
            }

            return null;
        }
        /// <summary>
        /// Return the alias or name if the first is null
        /// </summary>
        /// <returns></returns>
        public string GetAliasOrName()
        {
            if ( Element != null )
            {
                return Alias ?? GetName();
            }

            return string.Empty;
        }
        /// <summary>
        /// Return Element as a computed entity
        /// If it is not, return null
        /// </summary>
        /// <returns></returns>
        public ComputedEntity GetComputedEntity()
        {
            if ( Element is ComputedEntity )
            {
                return (ComputedEntity)Element;
            }

            return null;
        }
        /// <summary>
        /// Return Element as an Entity
        /// If Element is a ComputedEntity this will return ComputedEntity.SourceEntity
        /// </summary>
        /// <returns></returns>
        public Entity GetEntity()
        {
            if ( Element is Entity )
            {
                return (Entity)Element;
            }
            else if ( Element is ComputedEntity )
            {
                return GetComputedEntity().SourceEntity.GetEntity();
            }

            return null;
        }
        #endregion

        #region Constructor
        /// <summary>
        /// Initialize a new instance of JoinableEntity
        /// </summary>
        /// <param name="Alias"></param>
        /// <param name="Element"></param>
        public QueryableEntity( BaseERElement Element, string Alias = null )
        {
            this.Element = Element;
            this.Alias = Alias;
        }
        #endregion
    }
}
