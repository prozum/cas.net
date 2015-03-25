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

		// Takes a string, and serialize it as Json.
		public static string Serialize (Object serializeObject)
		{
			return JsonConvert.SerializeObject (serializeObject);
		}
	}

	/*
	public class TypeManager
	{
		public Type _type;
		public string _JsonString;

		public TypeManager (Object o)
		{
			_type = o.GetType ();
			Console.WriteLine ("Type written: " + o.GetType ());
			_JsonString = Export.Serialize (o);
		}

		public TypeManager ()
		{

		}

		public Type GetTMType ()
		{
			return _type;
		}

		public string GetTMString ()
		{
			return _JsonString;
		}

		public string ToString ()
		{
			return _type.ToString () + " " + _JsonString;
		}
	}
	*/
}

