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
			if (string.IsNullOrEmpty (prefix)) {
				throw new ArgumentException ("prefix");
			}

			using (HttpListener listener = new HttpListener()) {
			
				listener.Prefixes.Add(prefix);
				listener.Start();

				while (true)
				{
					Console.WriteLine("Listening...");
					var context = listener.GetContext();
					var request = context.Request;
					var response = context.Response;

					Stream reader = request.InputStream;
					byte[] buffer;
					string msg;

					using(var memoryStream = new MemoryStream())
					{
						reader.CopyTo(memoryStream);
						buffer = memoryStream.ToArray();
						msg = Encoding.UTF8.GetString(buffer);
					}

					Console.WriteLine(msg);

					var remsg = ExecuteCommand(msg, db);

					buffer = System.Text.Encoding.UTF8.GetBytes(remsg);
					response.ContentLength64 = buffer.Length;
					System.IO.Stream output = response.OutputStream;
					output.Write(buffer, 0, buffer.Length);
				}
			}
		}

		public static string GetCommand(string msg)
		{
			return GetStringFromPosition(msg, 0);
		}

		public static string ExecuteCommand(string msg, Database db)
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

		public static string TeacherAddAssignment(string msg, Database db)
		{
			int n = 0;
			string grade = "";
			string username = "";
			string password = "";
			string file = "";

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

		public static string StudentGetAssignment(string msg, Database db)
		{
			throw new NotImplementedException ();
		}

		public static string GetStringFromPosition(string msg, int pos)
		{
			string str = "";

			while (msg[pos] != ' ')
			{
				str = str + msg[pos];
				pos++;
			}

			return str;
		}

		public static string GetStringFromPosition(string msg, ref int pos)
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

