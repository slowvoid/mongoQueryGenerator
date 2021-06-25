using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace DocumentComparer
{
    class Program
    {
        static string ProjectFilePath = "D:\\Projects\\mestrado\\YCSB\\results\\project.json";
        static string ProjectFileDir = "D:\\Projects\\mestrado\\YCSB\\results";
        static string DefaultLogDir = "D:\\Projects\\mestrado\\resultados-benchmark"; 

        static bool ContainsArg(string inArg, string[] args)
        {
            foreach ( string arg in args )
            {
                if ( arg == inArg )
                {
                    return true;
                }
            }

            return false;
        }

        static void Main( string[] args )
        {
            if ( ContainsArg( "--generateProject", args ) )
            {
                bool skipHandcrafted = ContainsArg( "--skipHandcrafted", args );

                if ( skipHandcrafted )
                {
                    Console.WriteLine( "Skipping handcrafted queries" );
                }

                Dictionary<string, List<string>> queryAndPaths = new Dictionary<string, List<string>>();

                DirectoryInfo projectDir = Directory.CreateDirectory( ProjectFileDir ); ;
                    
                foreach ( DirectoryInfo queryDir in projectDir.EnumerateDirectories() )
                {
                    if ( queryDir.Name.Contains( "handcrafted" ) && skipHandcrafted )
                    {
                        continue;
                    }
                    string dirName = queryDir.Name.Replace( "_handcrafted", "" );
                    string queryName = dirName.Substring( 0, dirName.LastIndexOf( "_" ) );

                    if ( queryAndPaths.ContainsKey( queryName ) )
                    {
                        List<string> queryPaths = queryAndPaths[ queryName ];
                        queryPaths.Add( queryDir.FullName );
                    }
                    else
                    {
                        queryAndPaths.Add( queryName, new List<string>() { queryDir.FullName } );
                    }
                }

                ProjectFile proj = new ProjectFile();
                proj.Output = DefaultLogDir;

                foreach ( KeyValuePair<string, List<string>> queryAndPath in queryAndPaths )
                {
                    CompareProject compareProj = new CompareProject();
                    compareProj.Name = queryAndPath.Key;
                    compareProj.Paths = queryAndPath.Value;

                    proj.CompareProjects.Add( compareProj );
                }

                using ( StreamWriter sw = new StreamWriter( ProjectFilePath, false ) )
                {
                    sw.Write( JsonConvert.SerializeObject( proj ) );
                    sw.Close();
                }

                Console.WriteLine( "Project file created." );
            }

            // Load project file
            ProjectFile project = ProjectFile.FromFile( ProjectFilePath );
            JsonComparer comparer = new JsonComparer();

            foreach ( CompareProject cp in project.CompareProjects )
            {
                comparer.CompareProject( cp, project.Output );
            }
        }
    }
}
