using System;

namespace CAS.NET.Server
{
    class MainClass
    {
        public static void Main (string[] args)
        {
            string cs = @"server=localhost;userid=root;password=" + args[0] + ";database=mydb";
            Database db = new Database(cs);
			Server server = new Server("http://localhost:8080/", db);
			server.StartListen();
            Console.WriteLine ("Hello World!");
        }
    }
}