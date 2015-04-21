using System;
using System.IO;
using Newtonsoft.Json;
using System.Collections.Generic;

/* Usage:
 * Import methods are used for reading from files, and for
 * deserialising strings back into objects.
 */


namespace ImEx
{

    public static class Import
    {
        // Returns a deserialised string from file to object T
        public static T ReadDeserializedFromCasFile<T>(string fileName, string fileDestination)
        {
            string serializedString = OpenFileToString(fileName, fileDestination);
            T deserialisedObject = JsonConvert.DeserializeObject<T>(serializedString);
            return deserialisedObject;
        }

        // Returns a serialized string. Useful for creating checksums.
        public static string ReadSerializedFromCasFile(string fileName, string fileDestination)
        {
            return OpenFileToString(fileName, fileDestination);
        }

        // Used for deserializing an object of type T.
        public static T DeserializeString<T>(string serializedString)
        {
            T deserializedObject = JsonConvert.DeserializeObject<T>(serializedString);
            return deserializedObject;
        }

        // Loads serialized filecontent into strings.
        // TBH: This is the same as ReadSerializedFromCasFile, except it needs the filetype as well.
        private static string OpenFileToString(string fileName, string fileDestination)
        {
            string s;
            using (StreamReader sr = new StreamReader(fileName))
            {
                s = sr.ReadToEnd();
            }
            // Catch exception in case file cant be read or doesn't exist.

            return s;
        }
    }
}

