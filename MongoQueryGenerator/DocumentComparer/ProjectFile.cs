using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace DocumentComparer
{
    [JsonObject]
    public class ProjectFile
    {
        #region Properties
        /// <summary>
        /// List with a list of folders to compare.
        /// Each item compares all files of the same name within the given paths
        /// </summary>
        [JsonProperty]
        public List<CompareProject> CompareProjects { get; set; }
        /// <summary>
        /// Output directory for logs and results
        /// </summary>
        [JsonProperty]
        public string Output { get; set; }
        #endregion

        #region Methods
        public static ProjectFile FromFile( string inFilePath )
        {
            if ( string.IsNullOrWhiteSpace( inFilePath ) )
            {
                throw new ArgumentNullException( $"Argument inFilePath is null" );
            }

            using ( StreamReader sr = new StreamReader( inFilePath ) )
            {
                string contents = sr.ReadToEnd();
                sr.Close();

                ProjectFile Project = JsonConvert.DeserializeObject<ProjectFile>( contents );

                return Project;
            }
        }
        #endregion

        #region Constructor
        public ProjectFile()
        {
            CompareProjects = new List<CompareProject>();
        }
        #endregion
    }
}