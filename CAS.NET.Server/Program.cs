using System;

namespace CAS.NET.Server
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			Database DB = new Database("URI=file:data.db");
			Console.WriteLine ("Hello World!");
		}
	}
}
