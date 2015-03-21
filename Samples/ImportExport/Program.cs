using System;
using System.Collections.Generic;
using ImEx;

namespace ImportExport
{
	public class Program
	{
		public static void Main ()
		{
			List<Person> person = new List<Person> ();
			person.Add (new Person ("Generic", "Twat", 20));
			person.Add (new Person ("Average", "joe", 40));
			int CSumEx = Export.WriteToCasFile (person, "person", "");
			Console.WriteLine ("Exported Checksum: " + CSumEx);

			string ChecksumString = Import.OpenFileToString ("person2.cas", "");
			int CSumIm = Checksum.BSDChecksumFromFile (ChecksumString);

			Console.WriteLine ("Imported Checksum: " + CSumIm);

			if (CSumEx == CSumIm) {
				Console.WriteLine ("Identical checksum!");
			} else {
				Console.WriteLine ("Warning: The checksum for the file do not match!");
			}

			List<Person> person2 = new List<Person> ();
			person2 = Import.ReadPersonFromFile ("person", "");

			foreach (var item in person2) {
				Console.WriteLine (item.ToString ());
			}

		}
	}
}

