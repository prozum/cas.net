using System;
using System.IO;
using Newtonsoft.Json;

namespace ImEx
{
	public static class Export
	{
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
	}
}

