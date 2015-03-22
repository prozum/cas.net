using System;
using Account;
using System.Net;

namespace ServerTest
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			Server.StartListen("http://localhost:8080/", SendResponse);			
		}

		public static string SendResponse(HttpListenerRequest request)
		{
			return string.Format("Hello World!", DateTime.Now);
		}
	}
}
