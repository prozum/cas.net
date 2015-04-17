using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using ImEx;

namespace CAS.NET.Server
{
    public class Server
    {
		Database db;
		string prefix;

		public Server(string prefix, Database db)
		{
			this.db = db;
			this.prefix = prefix;
		}

        public void StartListen()
        {
            if (string.IsNullOrEmpty(prefix))
            {
                throw new ArgumentException("prefix");
            }

            // start listening for HTTP requests
            using (var listener = new HttpListener())
            {
            
                listener.Prefixes.Add(prefix);
                listener.AuthenticationSchemes = AuthenticationSchemes.Basic;
                listener.Start();

                while (true)
                {
                    Console.WriteLine("Listening...");
                    var context = listener.GetContext();
                    var request = context.Request;
                    var response = context.Response;
                    var identity = (HttpListenerBasicIdentity)context.User.Identity;

                    Stream reader = request.InputStream;
                    byte[] buffer;
                    string msg;

                    // copy client message to a buffer
                    using (var memoryStream = new MemoryStream())
                    {
                        reader.CopyTo(memoryStream);
                        buffer = memoryStream.ToArray();
                        msg = Encoding.UTF8.GetString(buffer);
                    }

                    Console.WriteLine(msg);

                    // execute client message and get message for client
                    string remsg = ExecuteCommand(identity.Name, identity.Password, msg, db);
                    buffer = System.Text.Encoding.UTF8.GetBytes(remsg);
                    response.ContentLength64 = buffer.Length;
                    System.IO.Stream output = response.OutputStream;
                    output.Write(buffer, 0, buffer.Length);
                    output.Close();
                }
            }
        }

		private string ExecuteCommand(string username, string password, string msg, HttpListenerRequest request, Database db)
        {
            // decode command from client message and remove it from msg
			int index = msg.IndexOf (" ");

			if (index == -1)
			{
				return "Invalid string format";
			}

            string command = msg.Substring(0, index);
            msg = msg.Substring(command.Length + 1);

			// check user privilege level
            int Privilege = db.CheckPrivilege(username, password);

			if (command == "Login")
			{
				return Privilege.ToString ();
			}

            // decode the command and run serverside code for the command
            switch (Privilege)
            {
                case 0:
                    switch (command)
                    {
                        case "GetAssignment":
                            return StudentGetAssignment(username, msg, db);
                        case "StudentGetAssignmentList":
                            return StudentGetAssignmentList(username, db);
                        case "AddCompleted":
                            return StudentAddCompleted(username, msg, db);
                        case "GetFeedback":
                            return StudentGetFeedback(username, msg, db);
                        default:
                            return "Invalid command";
                    }
                case 1:
                    switch (command)
                    {
                        case "AddAssignment":
                            //return TeacherAddAssignment(username, msg, db);
							return TeacherAddAssignment(request, db);
                        case "GetCompleted":
                            return TeacherGetCompleted(username, db);
                        case "AddFeedback":
                            return TeacherAddFeedback(username, db);
                        case "TeacherGetAssignmentList":
                            return TeacherGetAssignmentList(username, db);
                        default:
                            return "Invalid command";
                    }
                default:
                    return "Invalid user";
            }
        }

        private string TeacherAddAssignment(string username, HttpListenerRequest request, Database db)
        {
			/*
            string[] strArr = msg.Split(' ');

			if (strArr.Length < 4)
			{
				return "Invalid string format";
			}

            string checksum = strArr[0];
            string grade = strArr[1];
            string filename = strArr[2];
            string file = String.Empty;

            for (int i = 3; i < strArr.Length; i++)
            {
                file += strArr[i];
            }

            // generate checksum for file
            string checksumNew = Checksum.GetMd5Hash(file);

            // Writes the checksums
            Console.WriteLine(checksum + " <=> " + checksumNew);

            // Prevents the server from saving the files if it's checksum is invalid
            if (Checksum.VerifyMd5Hash(checksum, checksumNew) == false)
            {
                return "Failed adding assignment - Please try again.";
            }
            */

			string checksum = request.Headers ["Checksum"];
			string grade = request.Headers ["Grade"];
			string filename = request.Headers ["Filename"];
			string file = request.Headers ["File"];

            return db.AddAssignment(username, filename, file, grade);  
        }

		private string TeacherGetAssignmentList(string username, Database db)
        {      
            return string.Join(" ", db.TeacherGetAssignmentList(username));
        }

		private string TeacherGetCompleted(string msg, Database db)
        {     
            string[] strArr = msg.Split(' ');

			if (strArr.Length < 2)
			{
				return "Invalid string format";
			}

            string grade = strArr[0];
            string filename = strArr[1];

            /* teachers can use this to get other classes completed assignments */
            /* todo fix */
            return db.GetCompleted(filename, grade);
        }

        private string TeacherAddFeedback(string msg, Database db)
        {
            string[] strArr = msg.Split(' ');

			if (strArr.Length < 3)
			{
				return "Invalid string format";
			}

            string grade = strArr[0];
            string filename = strArr[1];
            string file = String.Empty;

            for (int i = 2; i < strArr.Length; i++)
            {
                file += strArr[i];
            }

			return db.AddFeedback(filename, file, grade);
        }

        private string StudentGetAssignmentList(string username, Database db)
        {
            string grade = db.GetGrade(username);
            return string.Join(" ", db.StudentGetAssignmentList(grade));
        }

        private string StudentGetAssignment(string username, string msg, Database db)
        {
            string[] strArr = msg.Split(' ');

			if (strArr.Length < 1)
			{
				return "Invalid string format";
			}

            string filename = strArr[0];
            string grade = db.GetGrade(username);

            return db.GetAssignment(filename, grade);
        }

        private string StudentAddCompleted(string username, string msg, Database db)
        {
            string[] strArr = msg.Split(' ');         

			if (strArr.Length < 2)
			{
				return "Invalid string format";
			}

            string filename = strArr[0];
            string grade = db.GetGrade(username);
            string file = String.Empty;

            for (int i = 1; i < strArr.Length; i++)
            {
                file += strArr[i];
            }

            return db.AddCompleted(username, filename, file, grade);
        }

        private string StudentGetFeedback(string username, string msg, Database db)
        {
            string[] strArr = msg.Split(' ');

			if (strArr.Length < 1)
			{
				return "Invalid string format";
			}

            string filename = strArr[0];
            string grade = db.GetGrade(username);

            return db.GetFeedback(username, filename, grade);
        }
    }
}