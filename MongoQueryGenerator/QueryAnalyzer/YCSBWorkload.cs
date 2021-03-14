using System.Collections.Generic;
using System.IO;

namespace QueryAnalyzer
{
    /// <summary>
    /// Represents a YCSB Workload file
    /// </summary>
    public class YCSBWorkloadFile
    {
        #region Properties
        /// <summary>
        /// Workload props
        /// </summary>
        public Dictionary<string, object> Props { get; set; }
        /// <summary>
        /// Workload name
        /// </summary>
        public string Name { get; set; }
        #endregion

        #region Methods
        /// <summary>
        /// Write workload file
        /// </summary>
        public void ExportToFile()
        {
            string FileDestination = $@"D:\Projects\mestrado\test-queries\{Name}_workload";

            using ( StreamWriter sw = new StreamWriter( FileDestination, false ) )
            {
                foreach ( KeyValuePair<string, object> prop in Props )
                {
                    sw.WriteLine( $"{prop.Key}={prop.Value}" );
                }

                sw.Close();
            }
        }
        #endregion

        #region Constructor
        /// <summary>
        /// Initialize a new YCSBWorkloadFile instance
        /// </summary>
        public YCSBWorkloadFile( string inName, string inDatabaseName )
        {
            Name = inName;
            Props = new Dictionary<string, object>();

            // Default values
            Props.Add( "workload", "site.ycsb.db.MongoDBWorkload" );
            Props.Add( "readproportion", 1 );
            Props.Add( "mongodb.url", $"mongodb://localhost:27017/{inDatabaseName}" );
            Props.Add( "operationcount", 1000 );
        }
        #endregion
    }
}