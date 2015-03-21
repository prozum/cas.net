using System;
using System.IO;
using System.Security.Cryptography;

namespace ImEx
{
	public static class Checksum
	{
		// BSD checksum algotithm, rewritten for C#
		public static int BSDChecksumFromFile (string s)
		{
			int Checksum = 0;

			foreach (char c in s) {
				Checksum = (Checksum >> 1) + ((Checksum & 1) << 15);
				Checksum += c;
				Checksum &= 0xffff;
			}

			return Checksum;
		}

		public static string GetMd5Hash (MD5 md5Hash, string s)
		{
			byte[] data = md5Hash.ComputeHash (System.Text.Encoding.UTF8.GetBytes (s));

			System.Text.StringBuilder SBuileder = new System.Text.StringBuilder ();

			for (int i = 0; i < data.Length; i++) {
				SBuileder.Append (data [i].ToString ("x2"));
			}

			return SBuileder.ToString ();
		}

		public static bool VerifyMd5Hash (MD5 md5Hash, string s, string hash)
		{
			string HashOfInput = GetMd5Hash (md5Hash, s);

			StringComparer comparer = StringComparer.OrdinalIgnoreCase;

			if (0 == comparer.Compare (HashOfInput, hash)) {
				return true;
			} else {
				return false;
			}
		}
	}
}

