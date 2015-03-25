using System;
using System.IO;
using Newtonsoft.Json;
using System.Collections.Generic;

//using Ast;

namespace ImEx
{

	public static class Import
	{
		// Returns a deserialised string from file to object T
		public static T ReadDeserializedFromCasFile<T> (string fileName, string fileDestination)
		{
			string serializedString = OpenFileToString (fileName + ".cas", "");
			T deserialisedObject = JsonConvert.DeserializeObject<T> (serializedString);
			return deserialisedObject;
		}

		// Returns a serialized string. Useful for creating checksums.
		public static string ReadSerializedFromCasFile (string fileName, string fileDestination)
		{
			return OpenFileToString (fileName + ".cas", "");
		}

		public static T DeserializeString<T> (string serializedString)
		{
			T deserializedObject = JsonConvert.DeserializeObject<T> (serializedString);
			return deserializedObject;
		}

		// Loads serialized filecontent into strings.
		private static string OpenFileToString (string fileName, string fileDestination)
		{
			string s;
			using (StreamReader sr = new StreamReader (fileName)) {
				s = sr.ReadToEnd ();
			}
			// Catch exception in case file cant be read or doesn't exist.

			return s;
		}
	}
}

