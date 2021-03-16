using System.IO;

namespace QueryAnalyzer
{
    /// <summary>
    /// Utility methods
    /// </summary>
    public static class Utils
    {
        /// <summary>
        /// Read a query from a file
        /// </summary>
        /// <param name="File">File to read</param>
        /// <returns></returns>
        public static string ReadQueryFromFile( string File )
        {
            string FileContents = string.Empty;

            using ( StreamReader sr = new StreamReader( File ) )
            {
                FileContents = sr.ReadToEnd();
                sr.Close();
            }

            return FileContents;
        }
        /// <summary>
        /// Export a query to a file
        /// </summary>
        /// <param name="Query"></param>
        /// <param name="File"></param>
        public static void ExportQueryToFile( string Query, string File )
        {
            using ( StreamWriter sw = new StreamWriter( File ) )
            {
                sw.Write( Query );
            }
        }
        /// <summary>
        /// Return the full path for the given relative path file
        /// </summary>
        /// <param name="inRelativePath"></param>
        /// <returns></returns>
        public static string GetFileFulPath( string inRelativePath )
        {
            FileInfo info = new FileInfo( inRelativePath );

            return info.FullName;
        }
    }
}