using System;

namespace CAS.NET.Server
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			string cs = @"server=localhost;userid=root;password=" + args[0] + ";database=mydb";
            Database DB = new Database(cs);
			Server.StartListen("http://localhost:8080/", DB);
			Console.WriteLine ("Hello World!");
		}
	}
}