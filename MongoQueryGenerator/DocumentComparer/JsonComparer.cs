using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace DocumentComparer
{
    public class JsonComparer
    {
        #region Methods
        public void CompareProject(CompareProject inProject, string inOutput)
        {
            if ( inProject == null )
            {
                return;
            }

            StringBuilder projectLog = new StringBuilder();

            Console.WriteLine( $"Starting comparision for project {inProject.Name}" );
            projectLog.AppendLine( $"Starting comparision for project {inProject.Name}" );

            List<string> ProjectFileNames = new List<string>();
            Dictionary<string, bool> FileComparisionResult = new Dictionary<string, bool>();
            // Use first path as reference
            string ReferencePath = inProject.Paths[ 0 ];

            FileInfo[] files = ( new DirectoryInfo( ReferencePath ) ).GetFiles( "*.json" );
            foreach ( FileInfo info in files )
            {
                ProjectFileNames.Add( info.Name );
            }

            // Compare files
            foreach ( string fileName in ProjectFileNames )
            {
                projectLog.AppendLine( $"Comparing file: {fileName}" );
                // Load files
                List<JToken> filesToCompare = new List<JToken>();
                foreach ( string baseFolder in inProject.Paths )
                {
                    try
                    {
                        filesToCompare.Add( Utils.LoadJsonFromFile( $"{baseFolder}\\{fileName}" ) );
                    } catch (FileNotFoundException)
                    {
                        projectLog.AppendLine( $"File: {baseFolder}\\{fileName} does not exists." );
                    }
                }

                // use first file as base for comparision
                JToken baseFile = filesToCompare[ 0 ];
                bool isMatch = true;

                foreach ( JToken jsonFile in filesToCompare )
                {
                    if ( baseFile == jsonFile )
                    {
                        continue;
                    }

                    isMatch = JToken.DeepEquals( baseFile, jsonFile );

                    projectLog.AppendLine( $"Are files equal? {isMatch}" );
                }

                FileComparisionResult.Add( fileName, isMatch );
            }

            // Check for potential failures
            var failedFiles = FileComparisionResult.Where( f => f.Value == false );
            foreach ( var failedFile in failedFiles )
            {
                projectLog.AppendLine( $"File: {failedFile.Key} does not match reference file [comparision result: {failedFile.Value}]" );
            }

            // Write log file
            using ( StreamWriter sw = new StreamWriter( $"{inOutput}\\{inProject.Name}.log" ) )
            {
                sw.Write( projectLog.ToString() );
                sw.Close();
            }
        }
        #endregion

        #region Constructor
        public JsonComparer() { }
        #endregion
    }
}