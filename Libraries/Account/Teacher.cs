using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using ImEx;

namespace Account
{
    public class Teacher
    {
		private readonly string host;
		private WebClient client;

		public Teacher(string username, string password, string host)
        {
			this.host = host;
			client = new WebClient();
			client.Encoding = System.Text.Encoding.UTF8;
			client.Credentials = new NetworkCredential(username, password);
        }

        public string AddAssignment(string file, string filename, string grade)
        {
			client.Headers.Add ("Checksum", Checksum.GetMd5Hash (file));
			client.Headers.Add ("Grade", grade);
			client.Headers.Add ("Filename", filename);
			client.Headers.Add ("File", file);

			string response = client.UploadString(host, "AddAssignment");

			client.Headers.Clear();

			if (response == "Failed")
			{
				return null;
			}
			else if (response == "Corruption")
			{
				return null;
			}

			return response;
        }

        public string GetCompleted(string student, string filename, string grade)
        {
			client.Headers.Add ("Student", student);
			client.Headers.Add ("Grade", grade);
			client.Headers.Add ("Filename", filename);

			string response = client.UploadString(host, "GetCompleted");

			client.Headers.Clear();

			if (response == "Success")
			{
				string file = client.ResponseHeaders["File"];
				string checksum = client.ResponseHeaders["Checksum"];

				if (checksum == Checksum.GetMd5Hash(file))
				{
					return file;
				}
				else
				{
					return this.GetCompleted(filename, student, grade);
				}
			}

			return null;
        }

        public string[] GetAssignmentList()
        {
			string response = client.UploadString(host, "TeacherGetAssignmentList");

			if (response == "Success")
			{
				List<string> filelist = new List<string>();
				List<string> checksumlist = new List<string>();
				int i = 0;

				while (client.ResponseHeaders["File" + i.ToString()] != null)
				{
					filelist.Add(client.ResponseHeaders["File" + i.ToString()]);
					checksumlist.Add(client.ResponseHeaders["Checksum" + i.ToString()]);

					if (Checksum.GetMd5Hash(filelist[i]) != checksumlist[i])
					{
						return this.GetAssignmentList();
					}

					i++;
				}

				return filelist.ToArray();
			}

			return null;
        }

		public string[] GetCompletedList(string filename, string grade)
		{
			client.Headers.Add("Filename", filename);
			client.Headers.Add("Grade", grade);

			string response = client.UploadString(host, "GetCompletedList");

			client.Headers.Clear();

			if (response == "Success")
			{
				List<string> studentlist = new List<string>();
				int i = 0;

				while (client.ResponseHeaders["Student" + i.ToString()] != null)
				{
					studentlist.Add(client.ResponseHeaders["Student" + i.ToString()]);
					studentlist.Add(client.ResponseHeaders["Status" + i.ToString()]);

					i++;
				}

				return studentlist.ToArray();
			}

			return null;
		}

		public string AddFeedback(string file, string filename, string username, string grade)
        {
			client.Headers.Add ("Checksum", Checksum.GetMd5Hash (file));
			client.Headers.Add ("Student", username);
			client.Headers.Add ("Grade", grade);
			client.Headers.Add ("Filename", filename);
			client.Headers.Add ("File", file);
	
            string response = client.UploadString(host, "AddFeedback");

			client.Headers.Clear();

			if (response == "Failed")
			{
				return null;
			}
			else if (response == "Corruption")
			{
				return null;
			}

			return response;
        }
    }
}