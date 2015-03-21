using System;
using System.Collections.Generic;
using ImEx;
using System.Security.Cryptography;

namespace ImportExport
{
	public class Program
	{
		public static void Main ()
		{
			List<Person> person = new List<Person> ();
			person.Add (new Person ("Generic", "Twat", 20));
			person.Add (new Person ("Average", "joe", 40));
			Export.WriteToCasFile (Export.Serialize (person), "person", "");

			Console.WriteLine ("BSD:");

			int I_CSumEx = Checksum.BSDChecksumFromFile (Export.Serialize (person));
			int I_CSumIm;
			string ChecksumString;

			Console.WriteLine ("person == person");

			ChecksumString = Import.OpenFileToString ("person.cas", "");
			I_CSumIm = Checksum.BSDChecksumFromFile (ChecksumString);

			Console.WriteLine ("CSumEx: " + I_CSumEx);
			Console.WriteLine ("CSumIm: " + I_CSumIm);

			if (I_CSumEx == I_CSumIm) {
				Console.WriteLine ("Identical checksum!");
			} else {
				Console.WriteLine ("Warning: The checksum for the file do not match!");
			}

			Console.WriteLine ("person == person2");

			// person2 have generic twat's age changed to 25
			ChecksumString = Import.OpenFileToString ("person2.cas", "");
			I_CSumIm = Checksum.BSDChecksumFromFile (ChecksumString);

			Console.WriteLine ("CSumEx: " + I_CSumEx);
			Console.WriteLine ("CSumIm: " + I_CSumIm);

			if (I_CSumEx == I_CSumIm) {
				Console.WriteLine ("Identical checksum!");
			} else {
				Console.WriteLine ("Warning: The checksum for the file do not match!");
			}

			Console.WriteLine ("\n\nMD5:");

			Console.WriteLine ("person == person");

			string S_CSumEx;
			string S_CSumIm;
			bool Valid;

			ChecksumString = Import.OpenFileToString ("person.cas", "");

			using (MD5 md5Hash = MD5.Create ()) {
				S_CSumEx = Checksum.GetMd5Hash (md5Hash, Export.Serialize (person));
				S_CSumIm = Checksum.GetMd5Hash (md5Hash, ChecksumString);

				Valid = Checksum.VerifyMd5Hash (md5Hash, Export.Serialize (person), S_CSumIm);
			}

			Console.WriteLine ("CsumEx: " + S_CSumEx);
			Console.WriteLine ("CsumIm: " + S_CSumIm);

			if (Valid == true) {
				Console.WriteLine ("Identical checksum!");
			} else {
				Console.WriteLine ("Warning: The checksum for the file do not match!");
			}


			Console.WriteLine ("person == person2");

			ChecksumString = Import.OpenFileToString ("person2.cas", "");


			using (MD5 md5Hash = MD5.Create ()) {
				S_CSumEx = Checksum.GetMd5Hash (md5Hash, Export.Serialize (person));
				S_CSumIm = Checksum.GetMd5Hash (md5Hash, ChecksumString);

				Valid = Checksum.VerifyMd5Hash (md5Hash, Export.Serialize (person), S_CSumIm);
			}

			Console.WriteLine ("CsumEx: " + S_CSumEx);
			Console.WriteLine ("CsumIm: " + S_CSumIm);

			if (Valid == true) {
				Console.WriteLine ("Identical checksum!");
			} else {
				Console.WriteLine ("Warning: The checksum for the file do not match!");
			}

		}
	}
}
