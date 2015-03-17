using System;
using System.IO;
using Newtonsoft.Json;

//using Ast;

namespace ImEx
{
	public static class Import
	{
		public static Object ReadFromCasFile (string FileName, string FileDestination)
		{
			string s = OpenFileToString (FileName + ".cas", "");
			Object deserializedObject = JsonConvert.DeserializeObject<Object> (s);
			return deserializedObject;
		}

		public static string OpenFileToString (string FileName, string FileDestination)
		{
			string s;
			using (StreamReader sr = new StreamReader (FileName)) {
				s = sr.ReadToEnd ();
			}
			// Catch exception in case file cant be read or doesn't exist.

			return s;
		}

		// This is a test purpose only object
		public static Person ReadPersonFromFile (string FileName, string FileDestination)
		{
			string s = OpenFileToString (FileName + ".cas", FileDestination);
			Person DeserializedPerson = JsonConvert.DeserializeObject<Person> (s);
			return DeserializedPerson;
		}
	}

	public class Person
	{
		public string FirstName { get; set; }

		public string LastName { get; set; }

		public int Age{ get; set; }


		public Person ()
		{

		}

		public Person (string FirstName, string LastName, int Age)
		{
			this.FirstName = FirstName;
			this.LastName = LastName;
			this.Age = Age;
		}


	}
}

