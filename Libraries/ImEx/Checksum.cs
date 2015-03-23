using System;
using System.Security.Cryptography;
using System.Text;

namespace ImEx
{
	public static class Checksum
	{
		// BSD checksum algotithm, rewritten for C#
		// Computes 16 bit checksum
		public static int GetBSDChecksum (string checksumString)
		{
			int checksum = 0;

			foreach (char c in checksumString) {
				checksum = (checksum >> 1) + ((checksum & 1) << 15);
				checksum += c;
				checksum &= 0xffff;
			}
			return checksum;
		}

		// Generates a 128 bit (16 byte) hash in hexadecimal form
		public static string GetMd5Hash (MD5 md5Hash, string hashableString)
		{
			byte[] data = md5Hash.ComputeHash (Encoding.UTF8.GetBytes (hashableString));

			StringBuilder sBuileder = new StringBuilder ();

			for (int i = 0; i < data.Length; i++) {
				sBuileder.Append (data [i].ToString ("x2"));
			}
			return sBuileder.ToString ();
		}

		// Takes two strings, generates their MD5 hashes, and compare them.
		// Returns true if they are identical
		public static bool VerifyMd5String (MD5 md5Hash, string verStringA, string verStringB)
		{
			string hashOfVerStringA = GetMd5Hash (md5Hash, verStringA);
			string hashOfVerStringB = GetMd5Hash (md5Hash, verStringB);

			StringComparer comparer = StringComparer.OrdinalIgnoreCase;

			return 0 == comparer.Compare (hashOfVerStringA, hashOfVerStringB);
		}

		// Takes two MD5 hashes, and compare them.
		// Returns true if they are identical
		public static bool VerifyMd5Hash (MD5 md5Hash, string verHashA, string verHashB)
		{
			StringComparer comparer = StringComparer.OrdinalIgnoreCase;

			return 0 == comparer.Compare (verHashA, verHashB);
		}

		// Takes one MD5 hash, and one string that need to generate a hash.
		// Returns true if they are identical
		public static bool VerifyMd5HashString (MD5 md5Hash, string verHash, string verString)
		{
			string hashOfVerString = GetMd5Hash (md5Hash, verString);

			StringComparer comparer = StringComparer.OrdinalIgnoreCase;

			return 0 == comparer.Compare (hashOfVerString, verHash);
		}

	}
}

