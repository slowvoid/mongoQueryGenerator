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
        public static string ReadQueryFromFile( string File)
        {
            string FileContents = string.Empty;

            using ( StreamReader sr = new StreamReader( File ) )
            {
                FileContents = sr.ReadToEnd();
                sr.Close();
            }

            return FileContents;
        }
    }
}