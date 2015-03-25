using System;

namespace CAS.NET.Server
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			string cs = @"server=localhost;userid=root;password=***REMOVED***;database=mydb";
            Database DB = new Database(cs);
			Console.WriteLine ("Hello World!");
		}
	}
}
