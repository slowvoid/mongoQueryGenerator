using Newtonsoft.Json.Linq;
using System;
using System.IO;

namespace DocumentComparer
{
    public static class Utils
    {
        public static JToken LoadJsonFromFile(string inFile)
        {
            if ( string.IsNullOrWhiteSpace( inFile ) )
            {
                throw new ArgumentNullException("Argument inFile is null");
            }

            using ( StreamReader sr = new StreamReader( inFile ) )
            {
                string fileContents = sr.ReadToEnd();
                sr.Close();

                return JToken.Parse( fileContents );
            }
        }
    }
}