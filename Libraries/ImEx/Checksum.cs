using System;
using System.Security.Cryptography;
using System.Text;

/* Usage:
 * TODO: BRB in 100 commits;
 */

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
        public static string GetMd5Hash (string hashableString)
        {
            using (MD5 md5Hash = MD5.Create ()) {
                byte[] data = md5Hash.ComputeHash (Encoding.UTF8.GetBytes (hashableString));
                StringBuilder sBuileder = new StringBuilder ();
                for (int i = 0; i < data.Length; i++) {
                    sBuileder.Append (data [i].ToString ("x2"));
                }
                return sBuileder.ToString ();
            }
        }

        #region BSD Compare

        // Takes two strings, generates their BSD Hashes, and compare them.
        // Returns true if they are identical
        public static bool VerifyBSDString (string verStringA, string verStringB)
        {
            int verHashA = GetBSDChecksum (verStringA);
            int verHashB = GetBSDChecksum (verStringB);

            if (verHashA == verHashB) {
                return true;
            } else {
                return false;
            }
        }

        // Takes two BSD Hashes and compare them.
        // Returns true if they are identical
        public static bool VerifyBSDHash (int verHashA, int verHashB)
        {
            if (verHashA == verHashB) {
                return true;
            } else {
                return false;
            }
        }

        // Takes one BSD Hash and one string to generate BSD Hash on, and compare them.
        // Returns true if they are identical
        public static bool VerifyBSDHashString (int verHashA, string verString)
        {
            int verHashB = GetBSDChecksum (verString);

            if (verHashA == verHashB) {
                return true;
            } else {
                return false;
            }
        }

        #endregion

        #region MD5 Compare

        // Takes two strings, generates their MD5 hashes, and compare them.
        // Returns true if they are identical
        public static bool VerifyMd5String (string verStringA, string verStringB)
        {
            using (MD5 md5Hash = MD5.Create ()) {
                string hashOfVerStringA = GetMd5Hash (verStringA);
                string hashOfVerStringB = GetMd5Hash (verStringB);
                StringComparer comparer = StringComparer.OrdinalIgnoreCase;
                return 0 == comparer.Compare (hashOfVerStringA, hashOfVerStringB);
            }
        }

        // Takes two MD5 hashes, and compare them.
        // Returns true if they are identical
        public static bool VerifyMd5Hash (string verHashA, string verHashB)
        {
            StringComparer comparer = StringComparer.OrdinalIgnoreCase;

            return 0 == comparer.Compare (verHashA, verHashB);
        }

        // Takes one MD5 hash, and one string that need to generate a hash.
        // Returns true if they are identical
        public static bool VerifyMd5HashString (string verHash, string verString)
        {
            string hashOfVerString = GetMd5Hash (verString);

            StringComparer comparer = StringComparer.OrdinalIgnoreCase;

            return 0 == comparer.Compare (hashOfVerString, verHash);
        }

        #endregion
    }
}

