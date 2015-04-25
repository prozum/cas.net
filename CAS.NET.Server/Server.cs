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

                    Console.WriteLine(identity.Name + " requesting " + msg);

                    // execute client message and get message for client
					string remsg = ExecuteCommand(msg, identity, request, response, db);
                    buffer = System.Text.Encoding.UTF8.GetBytes(remsg);
                    response.ContentLength64 = buffer.Length;
                    System.IO.Stream output = response.OutputStream;
                    output.Write(buffer, 0, buffer.Length);
                    output.Close();
                }
            }
        }

		private string ExecuteCommand(string msg, HttpListenerBasicIdentity identity, HttpListenerRequest request, HttpListenerResponse response, Database db)
        {
			// check user privilege level
			int Privilege = db.CheckPrivilege(identity.Name, identity.Password);

			if (msg == "Login")
			{
				return Privilege.ToString ();
			}

            // decode msg and execute serverside code for the command
            switch (Privilege)
            {
                case 0:
                    switch (msg)
                    {
                        case "GetAssignment":
                            return StudentGetAssignment(identity.Name, request, response, db);
                        case "StudentGetAssignmentList":
							return StudentGetAssignmentList(identity.Name, response, db);
                        case "AddCompleted":
							return StudentAddCompleted(identity.Name, request, db);
                        case "GetFeedback":
							return StudentGetFeedback(identity.Name, request, response, db);
                        default:
                            return "Invalid command";
                    }
                case 1:
                    switch (msg)
                    {
                        case "AddAssignment":
							return TeacherAddAssignment(identity.Name, request, db);
						case "GetCompletedList":
							return TeacherGetCompletedList(request, response, db);
                        case "GetCompleted":
							return TeacherGetCompleted(request, response, db);
                        case "AddFeedback":
							return TeacherAddFeedback(request, db);
                        case "TeacherGetAssignmentList":
							return TeacherGetAssignmentList(identity.Name, response, db);
                        default:
                            return "Invalid command";
                    }
                default:
                    return "Invalid user";
            }
        }

        private string TeacherAddAssignment(string username, HttpListenerRequest request, Database db)
        {
			if (request.Headers["Checksum"] != null && request.Headers["Grade"] != null &&
				request.Headers["Filename"] != null && request.Headers["File"] != null)
			{
				string checksum = request.Headers ["Checksum"];
				string grade = request.Headers ["Grade"];
				string filename = request.Headers ["Filename"];
				string file = request.Headers ["File"];

				// Prevents the server from saving the files if it's checksum is invalid
				string checksumNew = Checksum.GetMd5Hash(file);

				if (Checksum.VerifyMd5Hash(checksum, checksumNew) == false)
				{
					return "Corruption";
				}

				return db.AddAssignment(username, filename, file, grade);
			}

			return "Failed";
        }

		private string TeacherGetAssignmentList(string username, HttpListenerResponse response, Database db)
        {      
			string[] filelist = db.TeacherGetAssignmentList(username);

			if (filelist[0] == "Error")
			{
				return "Failed";
			}

			string[] checksumlist = new string[filelist.Length];

			for (int i = 0; i < filelist.Length; i++)
			{
				checksumlist[i] = Checksum.GetMd5Hash(filelist[i]);
				response.Headers.Add("File" + i.ToString(), filelist[i]);
				response.Headers.Add("Checksum" + i.ToString(), checksumlist[i]);
			}

			return "Success";
        }

		private string TeacherGetCompletedList(HttpListenerRequest request, HttpListenerResponse response, Database db)
		{
			response.Headers.Clear();

			if (request.Headers["Grade"] != null &&	request.Headers["Filename"] != null)
			{
				string grade = request.Headers ["Grade"];
				string filename = request.Headers ["Filename"];

				string[] StudentsAndCompleted = db.GetCompletedList(filename, grade);

				if (StudentsAndCompleted[0] == "No students have completed the assignment")
				{
					return StudentsAndCompleted[0];
				}
				else if (StudentsAndCompleted[0] == "Error")
				{
					return "Failed";
				}

				for (int i = 0; i < StudentsAndCompleted.Length/2; i++)
				{
					response.Headers.Add("Student" + i.ToString(), StudentsAndCompleted[i*2]);
					response.Headers.Add("Status" + i.ToString(), StudentsAndCompleted[(i*2)+1]);
				}

				return "Success";
			}

			return "Failed";
		}

		private string TeacherGetCompleted(HttpListenerRequest request, HttpListenerResponse response, Database db)
        {   
			/* teachers can use this to get other classes completed assignments */
			/* todo fix */
			if (request.Headers["Grade"] != null && request.Headers["Filename"] != null && request.Headers["Student"] != null)
			{
				string student = request.Headers ["Student"];
				string grade = request.Headers ["Grade"];
				string filename = request.Headers ["Filename"];

				string file = db.GetCompleted(filename, student, grade);
				string checksum = Checksum.GetMd5Hash(file);

				response.Headers.Add("File", file);
				response.Headers.Add("Checksum", checksum);

				return "Success";
			}
            
			return "Failed";
        }

		private string TeacherAddFeedback(HttpListenerRequest request, Database db)
        {
			if (request.Headers["Checksum"] != null && request.Headers["Grade"] != null &&
				request.Headers["Filename"] != null && request.Headers["File"] != null &&
				request.Headers["Student"] != null)
			{
				string checksum = request.Headers ["Checksum"];
				string grade = request.Headers ["Grade"];
				string filename = request.Headers ["Filename"];
				string file = request.Headers ["File"];
				string student = request.Headers["Student"];

				// Prevents the server from saving the files if it's checksum is invalid
				string checksumNew = Checksum.GetMd5Hash(file);

				Console.WriteLine(checksum);
				Console.WriteLine(checksumNew);

				if (Checksum.VerifyMd5Hash(checksum, checksumNew) == false)
				{
					return "Corruption";
				}

				return db.AddFeedback(filename, file, student, grade);
			}

			return "Failed";
        }

		private string StudentGetAssignmentList(string username, HttpListenerResponse response, Database db)
        {
            string grade = db.GetGrade(username);
            string[] filelist = db.StudentGetAssignmentList(grade);

			if (filelist[0] == "Error")
			{
				return "Failed";
			}

			string[] checksumlist = new string[filelist.Length];

			for (int i = 0; i < filelist.Length; i++)
			{
				checksumlist[i] = Checksum.GetMd5Hash(filelist[i]);
				response.Headers.Add("File" + i.ToString(), filelist[i]);
				response.Headers.Add("Checksum" + i.ToString(), checksumlist[i]);
			}

			return "Success";
        }

		private string StudentGetAssignment(string username, HttpListenerRequest request, HttpListenerResponse response, Database db)
        {
			if (request.Headers["Filename"] != null)
			{
				string grade = db.GetGrade(username);
				string filename = request.Headers ["Filename"];

				string file = db.GetAssignment(filename, grade);
				string checksum = Checksum.GetMd5Hash(file);

				response.Headers.Add("File", file);
				response.Headers.Add("Checksum", checksum);

				return "Success";
			}

			return "Failed";            
        }

		private string StudentAddCompleted(string username, HttpListenerRequest request, Database db)
        {
			if (request.Headers["Checksum"] != null && request.Headers["Filename"] != null &&
															request.Headers["File"] != null)
			{
				string grade = db.GetGrade(username);
				string checksum = request.Headers ["Checksum"];
				string filename = request.Headers ["Filename"];
				string file = request.Headers ["File"];

				// Prevents the server from saving the files if it's checksum is invalid
				string checksumNew = Checksum.GetMd5Hash(file);

				if (Checksum.VerifyMd5Hash(checksum, checksumNew) == false)
				{
					return "Corruption";
				}

				return db.AddCompleted(username, filename, file, grade);
			}

			return "Failed";
        }

		private string StudentGetFeedback(string username, HttpListenerRequest request, HttpListenerResponse response, Database db)
        {
			if (request.Headers["Filename"] != null)
			{
				string grade = db.GetGrade(username);
				string filename = request.Headers ["Filename"];

				string file = db.GetFeedback(username, filename, grade);
				string checksum = Checksum.GetMd5Hash(file);

				response.Headers.Add("File", file);
				response.Headers.Add("Checksum", checksum);

				return "Success";
			}

			return "Failed";
        }
    }
}