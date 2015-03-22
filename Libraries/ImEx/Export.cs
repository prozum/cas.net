using System;
using System.IO;
using Newtonsoft.Json;
using System.Security.Cryptography;

namespace ImEx
{
	public static class Export
	{

		public static void WriteToCasFile (string ExportString, string FileName, string FileDestination)
		{

			File.WriteAllText (FileName + ".cas", ExportString);

		}

		public static string Serialize (Object ExportObject)
		{
			string JsonString = JsonConvert.SerializeObject (ExportObject);

			return JsonString;
		}
		/*
		public static int WriteToCasFile (Object ExportObject, string FileName, string FileDestination)
		{
			string JsonFile = JsonConvert.SerializeObject (ExportObject);

			JsonSerializer serializer = new JsonSerializer ();
			serializer.NullValueHandling = NullValueHandling.Ignore;

			// bool b = Validation.ValidatePerson (JsonFile);
			// Console.WriteLine (b);

			int CSum = Checksum.BSDChecksumFromFile (JsonFile);

			File.WriteAllText (FileName + ".cas", JsonFile);

			#if DEBUG
			Console.WriteLine ("Writing to file \"" + FileName + ".cas\": " + JsonFile);
			#endif

			return CSum;
		}
		*/
	}
}

