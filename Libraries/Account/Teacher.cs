using System;
using System.Net;
using System.Text;
using System.IO;
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

			return response;
        }

        public string GetCompleted(string filename, string grade)
        {
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
					return this.GetCompleted(filename, grade);
				}
			}

			return null;
        }

        public string[] GetAssignmentList()
        {
			string response = client.UploadString(host, "TeacherGetAssignmentList");

			if (response == "Success")
			{
				string[] filelist = new string[client.ResponseHeaders.Count/2];
				string[] checksumlist = new string[client.ResponseHeaders.Count/2];

				for (int i = 0; i < client.ResponseHeaders.Count/2; i++)
				{
					filelist[i] = client.ResponseHeaders["File" + i.ToString()];
					checksumlist[i] = client.ResponseHeaders["Checksum" + i.ToString()];

					if (Checksum.GetMd5Hash(filelist[i]) != checksumlist[i])
					{
						return this.GetAssignmentList();
					}
				}
			}

			return null;
        }

        public string AddFeedback(string file, string filename, string grade)
        {
			client.Headers.Add ("Checksum", Checksum.GetMd5Hash (file));
			client.Headers.Add ("Grade", grade);
			client.Headers.Add ("Filename", filename);
			client.Headers.Add ("File", file);
	
            return client.UploadString(host, "AddFeedback");
        }
    }
}