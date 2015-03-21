using System;
using System.IO;

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


	}
}

