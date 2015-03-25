using System;
using System.Net;
using System.Text;
using System.IO;
using System.Collections.Generic;

namespace CAS.NET.Server
{
	public static class Server
	{
		public static void StartListen(string prefix, Database db)
		{
			if (prefix == null || prefix.Length == 0)
			{
				throw new ArgumentException("prefixes");
			}

			using (HttpListener listener = new HttpListener()) {
			
				listener.Prefixes.Add(prefix);
				listener.Start();

				while (true)
				{
					Console.WriteLine("Listening...");

					var request = listener.GetContext().Request;
					var response = listener.GetContext().Response;

					Stream reader = request.InputStream;
					byte[] msg = new byte[reader.Length];

					for (int i = 0; i < reader.Length; i++) {
						msg[i] = (byte)reader.ReadByte();
					}

					string remsg = ExecuteCommand(msg, db);

					byte[] buffer = System.Text.Encoding.UTF8.GetBytes(remsg);
					response.ContentLength64 = buffer.Length;
					System.IO.Stream output = response.OutputStream;
					output.Write(buffer, 0, buffer.Length);
				}
			}
		}

		public static string GetCommand(byte[] msg)
		{
			return GetStringFromPosition(msg, 0);
		}

		public static string ExecuteCommand(byte[] msg, Database db)
		{
			string command = GetCommand(msg);

			if (command == null)
			{
				return "Error";
			}
				
			switch (command)
			{
				case "AddAssignment":
					return TeacherAddAssignment(msg, db);
				case "GetCompleted":
					break;
				case "AddFeedback":
					break;
				default:
					break;
			}

			return "Success";
		}

		public static string TeacherAddAssignment(byte[] msg, Database db)
		{
			int n = 0;
			string  grade = "", username = "", password = "", file = "";

			/* find length of command by output parameter n */
			GetStringFromPosition(msg, ref n);

			grade = GetStringFromPosition(msg, ref n);
			username = GetStringFromPosition(msg, ref n);
			password = GetStringFromPosition(msg, ref n);

			/* file can contain spaces, GetStringFromPosition doesn't work then */
			for (int i = n; i < msg.Length; i++) {
				file = file + msg[i];
			}

			db.AddAssignment(username, file, grade);

			return "Successfully added assignment";
		}

		public static string GetStringFromPosition(byte[] msg, int pos)
		{
			string str = "";

			while (msg[pos] != ' ')
			{
				str = str + msg[pos];
				pos++;
			}

			return str;
		}

		public static string GetStringFromPosition(byte[] msg, ref int pos)
		{
			string str = "";

			while (msg[pos] != ' ')
			{
				str = str + msg[pos];
				pos++;
			}

			/* skip the space found */
			pos++;

			return str;
		}
	}
}

