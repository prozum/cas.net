using System;
using System.IO;
using System.IO.Compression;
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
			File.WriteAllText (fileDestination + fileName+"_temp" + ".cas", exportString);
			CompressToFile (fileName);
        }

        // Writes a string to disk
        // Takes both file and destination as one argument
        public static void WriteToCasFile (string exportString, string file)
        {
            File.WriteAllText (file+"_temp", exportString);
			CompressToFile (file);
        }

        // Writes an object to disk
        public static void WriteToCasFile (object exportObject, string file)
        {
            string serializedObject = Export.Serialize (exportObject);
			File.WriteAllText (file+"_temp", serializedObject);
			CompressToFile (file);
        }

		public static void CompressToFile(string fileName) {
			FileStream sourceFileStream = File.OpenRead(fileName+"_temp");
			FileStream destFileStream = File.Create(fileName);

			GZipStream compressingStream = new GZipStream(destFileStream, 
				CompressionMode.Compress);

			byte[] bytes = new byte[2048];
			int bytesRead;
			while ((bytesRead = sourceFileStream.Read(bytes, 0, bytes.Length)) != 0)
			{
				compressingStream.Write(bytes, 0, bytesRead);
			}

			sourceFileStream.Close();
			compressingStream.Close();
			destFileStream.Close();
			System.IO.File.Delete (fileName + "_temp");
		}


        // Takes a string, and serialize it as Json.
        public static string Serialize (Object serializeObject)
        {
            return JsonConvert.SerializeObject (serializeObject);
        }
    }
}

