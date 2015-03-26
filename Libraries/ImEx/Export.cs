using System;
using System.IO;
using Newtonsoft.Json;

namespace ImEx
{
	public static class Export
	{
		// Writes an string to disk
		// Currently doesnt have support for destination
		public static void WriteToCasFile (string exportString, string fileName, string fileDestination)
		{	
			File.WriteAllText (fileDestination + fileName + ".cas", exportString);
		}

		public static void WriteToCasFile (string exportString, string file)
		{
			File.WriteAllText (file, exportString);
		}

		// Takes a string, and serialize it as Json.
		public static string Serialize (Object serializeObject)
		{
			return JsonConvert.SerializeObject (serializeObject);
		}
	}
}

