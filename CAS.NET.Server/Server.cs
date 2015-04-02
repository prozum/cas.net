using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

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
                    output.Close();
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
				
			switch (command)
			{
				case "AddAssignment":
					return TeacherAddAssignment (msg, db);
				case "AddFeedback":
					break;
				case "GetCompleted":
					break;
				case "GetAssignmentList":
					return StudentGetAssignmentList(msg, db);
				case "GetAssignment":
					return StudentGetAssignment (msg, db);
				case "AddCompleted":
					break;
				default:
					return "Invalid command";
			}

			return "";
		}

		public static string TeacherAddAssignment(string msg, Database db)
		{
			int n = 0;
			string grade = "";
			string username = "";
			string password = "";
			string filename = "";
			string file = "";

			/* find length of command by output parameter n */
			GetStringFromPosition(msg, ref n);

			grade = GetStringFromPosition(msg, ref n);
			username = GetStringFromPosition(msg, ref n);
			password = GetStringFromPosition(msg, ref n);
			filename = GetStringFromPosition(msg, ref n);

			/* because a file can contain spaces, GetStringFromPosition doesn't work then */
			for (int i = n; i < msg.Length; i++) {
				file = file + msg[i];
			}

			db.AddAssignment(username, filename, file, grade);

			return "Successfully added assignment";
		}

		public static string TeacherGetCompleted(string msg, Database db)
		{
			int n = 0;
			string grade = "";
			string username = "";
			string password = "";
			string filename = "";

			/* find length of command by output parameter n */
			GetStringFromPosition(msg, ref n);

			grade = GetStringFromPosition(msg, ref n);
			username = GetStringFromPosition(msg, ref n);
			password = GetStringFromPosition(msg, ref n);
			//filename = GetStringFromPosition(msg, ref n);

			/* because a file can contain spaces, GetStringFromPosition doesn't work then */
			for (int i = n; i < msg.Length; i++) {
				filename = filename + msg[i];
			}

			return db.GetCompleted(username, filename, grade);
		}

		/*
		public static string TeacherAddFeedback(string msg, Database db)
		{
			int n = 0;
			string grade = "";
			string username = "";
			string password = "";
			string filename = "";
			string file = "";

			//find length of command by output parameter n
			GetStringFromPosition(msg, ref n);

			grade = GetStringFromPosition(msg, ref n);
			username = GetStringFromPosition(msg, ref n);
			password = GetStringFromPosition(msg, ref n);
			filename = GetStringFromPosition(msg, ref n);

			//because a file can contain spaces, GetStringFromPosition doesn't work then
			for (int i = n; i < msg.Length; i++) {
				file = file + msg[i];
			}

			db.AddAssignment(username, filename, file, grade);

			return "Successfully added assignment";
		}
		*/

		public static string StudentGetAssignmentList(string msg, Database db)
		{
			int n = 0;
			string grade = "";
			string username = "";
			string password = "";

			GetStringFromPosition(msg, ref n);

			grade = GetStringFromPosition(msg, ref n);
			username = GetStringFromPosition(msg, ref n);

			for (int i = n; i < msg.Length; i++)
			{
				password = password + msg[i];
			}
			//password = GetStringFromPosition(msg, ref n);

			return string.Join(" ", db.GetAssignmentList(grade));
		}

		public static string StudentGetAssignment(string msg, Database db)
		{
			int n = 0;
			string grade = "";
			string username = "";
			string password = "";
            string filename = "";

			/* find length of command by output parameter n */
			GetStringFromPosition(msg, ref n);

			grade = GetStringFromPosition(msg, ref n);
           	username = GetStringFromPosition(msg, ref n);
            password = GetStringFromPosition(msg, ref n);
			//filename = GetStringFromPosition(msg, ref n);


			for (int i = n; i < msg.Length; i++) {
				filename = filename + msg[i];
			}
				
            return db.GetAssignment(filename, grade);
		}

		public static string StudentAddCompleted(string msg, Database db)
		{
			int n = 0;
			string grade = "";
			string username = "";
			string password = "";
			string filename = "";
			string file = "";

			/* find length of command by output parameter n */
			GetStringFromPosition(msg, ref n);

			grade = GetStringFromPosition(msg, ref n);
			username = GetStringFromPosition(msg, ref n);
			password = GetStringFromPosition(msg, ref n);
			filename = GetStringFromPosition(msg, ref n);

			/* because a file can contain spaces, GetStringFromPosition doesn't work then */
			for (int i = n; i < msg.Length; i++) {
				file = file + msg[i];
			}

			db.AddCompleted(username, filename, file, grade);

			return "Successfully added assignment";
		}

		public static string GetStringFromPosition(string msg, int pos)
		{
			string str = "";

			while (msg[pos] != ' ' && pos < msg.Length)
			{
				str = str + msg[pos];
				pos++;
			}

			return str;
		}

		public static string GetStringFromPosition(string msg, ref int pos)
		{
			string str = "";

			while (msg[pos] != ' ' && pos < msg.Length)
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

