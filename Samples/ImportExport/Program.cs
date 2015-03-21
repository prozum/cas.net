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
			Export.WriteToCasFile (person, "person", "");
		}
	}
}

