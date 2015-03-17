using System;
using System.IO;
using Newtonsoft.Json;

namespace ImEx
{
	public static class Export
	{
		public static void WriteToCasFile (Object ExportObject, string FileName, string FileDestination)
		{
			string JsonFile = JsonConvert.SerializeObject (ExportObject);

			JsonSerializer serializer = new JsonSerializer ();
			serializer.NullValueHandling = NullValueHandling.Ignore;

			bool b = Validation.ValidatePerson (JsonFile);
			Console.WriteLine (b);

			File.WriteAllText (FileName + ".cas", JsonFile);

			#if DEBUG
			Console.WriteLine ("Writing to file \"" + FileName + ".cas\": " + JsonFile);
			#endif
		}
	}
}

