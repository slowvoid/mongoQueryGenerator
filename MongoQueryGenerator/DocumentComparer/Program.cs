using System;

namespace DocumentComparer
{
    class Program
    {
        static string ProjectFilePath = "D:\\Projects\\mestrado\\YCSB\\results\\project.json";

        static void Main( string[] args )
        {
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
