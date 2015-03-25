using System;
using System.Net;
using System.Text;
using System.IO;
using System.Collections.Generic;

namespace CAS.NET.Server
{
	public static class Server
	{
		public static void StartListen(string prefixes)
		{
			if (prefixes == null || prefixes.Length == 0)
			{
				throw new ArgumentException("prefixes");
			}

			using (var listener = new HttpListener()) {

			listener.Start();

				while (true)
				{
					Console.WriteLine("Listening...");

					HttpListenerContext context = listener.GetContext();
					HttpListenerRequest request = context.Request;
					HttpListenerResponse response = context.Response;

					string msg = request.ToString();
					ExecuteCommand(msg);
				}
			}
		}

		public static string GetCommand(string msg)
		{
			int i = 0;
			string command = null;

			while (msg[i] != ' ')
			{
				command = command + msg[i];
				i++;
			}
			
			return command;
		}

		public static string ExecuteCommand(string msg)
		{
			string command = GetCommand(msg);

			if (command == null)
			{
				return "Error";
			}

			/*
			switch (command)
			{
				case "AddAssignment":
					break;
				case "GetCompleted":
					break;
				case "AddFeedback":
					break;
				default:
					break;
			}
			*/

			return "Success";
		}
	}
}

