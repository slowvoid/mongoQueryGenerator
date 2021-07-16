using Newtonsoft.Json;
using System.Collections.Generic;

namespace DocumentComparer
{
    [JsonObject]
    public class CompareProject
    {
        #region Properties
        /// <summary>
        /// Compare name (project name / query name)
        /// </summary>
        [JsonProperty]
        public string Name { get; set; }
        /// <summary>
        /// Paths where result files are located
        /// </summary>
        [JsonProperty]
        public List<string> Paths { get; set; }
        #endregion

        #region Constructor
        public CompareProject()
        {
            Paths = new List<string>();        
        }
        #endregion
    }
}