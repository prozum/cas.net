using System;
using System.IO;
using Newtonsoft.Json;

/* Usage:
 * The Export methods are used for serializing an object and save the object to a file.
 */

namespace ImEx
{
    public static class Export
    {
        // Writes a string to disk
        public static void WriteToCasFile (string exportString, string fileName, string fileDestination)
        {    
            File.WriteAllText (fileDestination + fileName + ".cas", exportString);
        }

        // Writes a string to disk
        // Takes both file and destination as one argument
        public static void WriteToCasFile (string exportString, string file)
        {
            File.WriteAllText (file, exportString);
        }

        // Writes an object to disk
        public static void WriteToCasFile (object exportObject, string file)
        {
            string serializedObject = Export.Serialize (exportObject);
            File.WriteAllText (file, serializedObject);
        }

        // Takes a string, and serialize it as Json.
        public static string Serialize (Object serializeObject)
        {
            return JsonConvert.SerializeObject (serializeObject);
        }
    }
}

