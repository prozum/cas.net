using System;
using System.Security.Cryptography;
using System.Text;

/* Usage:
 * Used for generating checksum for comparing files when sending them between server and client;
 */

namespace ImEx
{
    public static class Checksum
    {
        // MD5 Checksum
        // Generates a 128 bit hash in hexadecimal form
        public static string GetMd5Hash(string hashableString)
        {
            using (MD5 md5Hash = MD5.Create())
            {
                byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(hashableString));
                StringBuilder sBuileder = new StringBuilder();
                for (int i = 0; i < data.Length; i++)
                {
                    sBuileder.Append(data[i].ToString("x2"));
                }
                return sBuileder.ToString();
            }
        }

        #region MD5 Compare

        // Takes two strings, generates their MD5 hashes, and compare them.
        // Returns true if they are identical
        public static bool VerifyMd5String(string verStringA, string verStringB)
        {
            using (MD5 md5Hash = MD5.Create())
            {
                string hashOfVerStringA = GetMd5Hash(verStringA);
                string hashOfVerStringB = GetMd5Hash(verStringB);
                StringComparer comparer = StringComparer.OrdinalIgnoreCase;
                return 0 == comparer.Compare(hashOfVerStringA, hashOfVerStringB);
            }
        }

        // Takes two MD5 hashes, and compare them.
        // Returns true if they are identical
        public static bool VerifyMd5Hash(string verHashA, string verHashB)
        {
            StringComparer comparer = StringComparer.OrdinalIgnoreCase;

            return 0 == comparer.Compare(verHashA, verHashB);
        }

        // Takes one MD5 hash, and one string that need to generate a hash.
        // Returns true if they are identical
        public static bool VerifyMd5HashString(string verHash, string verString)
        {
            string hashOfVerString = GetMd5Hash(verString);

            StringComparer comparer = StringComparer.OrdinalIgnoreCase;

            return 0 == comparer.Compare(hashOfVerString, verHash);
        }

        #endregion
    }
}

