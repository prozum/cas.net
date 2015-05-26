using System;
using System.IO;
using Newtonsoft.Json;
using System.IO.Compression;
using System.Collections.Generic;

/* Usage:
 * Import methods are used for reading from files, and for
 * deserialising strings back into objects.
 */

/*
 * Not all elements are implementes in the final product
 */

namespace ImEx
{
    public static class Import
    {
        // Returns a deserialised string from file to object T
        public static T ReadDeserializedFromCasFile<T>(string fileName, string fileDestination)
        {
            try
            {
                string serializedString = OpenFileToString(fileName, fileDestination);
                T deserialisedObject = JsonConvert.DeserializeObject<T>(serializedString);
                return deserialisedObject;
            }
            catch (Exception)
            {
                return default(T);
            }
        }

        // Returns a serialized string. Useful for creating checksums.
        public static string ReadSerializedFromCasFile(string fileName, string fileDestination)
        {
            try
            {
                return OpenFileToString(fileName, fileDestination);
            }
            catch (Exception)
            {
                return String.Empty;
            }
        }

        // Used for deserializing an object of type T.
        public static T DeserializeString<T>(string serializedString)
        {
            try
            {
                T deserializedObject = JsonConvert.DeserializeObject<T>(serializedString);
                return deserializedObject;
            }
            catch (Exception)
            {
                return default(T);
            }
        }

        // Loads serialized filecontent into strings.
        // TBH: This is the same as ReadSerializedFromCasFile, except it needs the filetype as well.
        private static string OpenFileToString(string fileName, string fileDestination)
        {
            string s;
			DeCompressToFile (fileName);
            using (StreamReader sr = new StreamReader(fileName))
            {
                s = sr.ReadToEnd();
            }
            // Catch exception in case file cant be read or doesn't exist.

            return s;
        }
		public static void DeCompressToFile(string fileName) {
			FileStream sourceFileStream = File.OpenRead(fileName);
			FileStream destFileStream = File.Create(fileName+"un");

			GZipStream decompressingStream = new GZipStream(sourceFileStream, 
				CompressionMode.Decompress);
			int byteRead;
			while((byteRead = decompressingStream.ReadByte()) != -1) 
			{
				destFileStream.WriteByte((byte)byteRead);
			}
				
			decompressingStream.Close();
			sourceFileStream.Close();
			destFileStream.Close();
			System.IO.File.Move(fileName, fileName+"_del");
			System.IO.File.Move(fileName+"un", fileName);
			System.IO.File.Delete (fileName + "_del");
		}
    }
}

