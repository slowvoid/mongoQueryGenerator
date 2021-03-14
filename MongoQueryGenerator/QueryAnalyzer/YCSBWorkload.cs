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
        /// <summary>
        /// Workload file path
        /// </summary>
        public string FileName { get; set; }
        #endregion

        #region Methods
        /// <summary>
        /// Write workload file
        /// </summary>
        public void ExportToFile()
        {
            using ( StreamWriter sw = new StreamWriter( FileName, false ) )
            {
                foreach ( KeyValuePair<string, object> prop in Props )
                {
                    sw.WriteLine( $"{prop.Key}={prop.Value}" );
                }

                sw.Close();
            }
        }

        /// <summary>
        /// Set/Override value for property
        /// </summary>
        /// <param name="PropertyName"></param>
        /// <param name="PropertyValue"></param>
        public void SetProperty(string PropertyName, object PropertyValue)
        {
            if ( Props == null )
            {
                return;
            }

            if ( Props.ContainsKey( PropertyName ) )
            {
                Props[ PropertyName ] = PropertyValue;
            }
            else
            {
                Props.Add( PropertyName, PropertyValue );
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
            FileName = $@"D:\\Projects\\mestrado\\YCSB\\workloads\\{Name}_workload";
            Props = new Dictionary<string, object>();

            // Default values
            Props.Add( "workload", "site.ycsb.db.MongoDBWorkload" );
            Props.Add( "readproportion", 1 );
            Props.Add( "mongodb.url", $"mongodb://localhost:27017/{inDatabaseName}" );
            Props.Add( "operationcount", 1000 );
            Props.Add( "queryfile", $"{FileName.Replace("_workload", "")}.mongo" );
        }
        #endregion
    }
}