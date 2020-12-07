using System.IO;

namespace QueryBuilder.Tests
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
        /// Returns a FileStream for the given file path
        /// </summary>
        /// <param name="FilePath"></param>
        /// <returns></returns>
        public static FileStream ReadMappingFile( string FilePath )
        {
            return new FileStream(FilePath, FileMode.Open);
        }
    }
}