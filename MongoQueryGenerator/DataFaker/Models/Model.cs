using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataFaker.Models
{
    /// <summary>
    /// Base class for models
    /// </summary>
    public abstract class Model
    {
        /// <summary>
        /// Generates a JSON representation
        /// </summary>
        /// <returns></returns>
        public string ToJson()
        {
            return JsonConvert.SerializeObject( this, Formatting.Indented );
        }
    }
}
