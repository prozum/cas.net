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
			string checksum = Checksum.GetMd5Hash (file);
			client.Headers.Add ("Command", "AddAssignment");
			client.Headers.Add ("Checksum", checksum);
			client.Headers.Add ("Grade", grade);
			client.Headers.Add ("Filename", filename);
			client.Headers.Add ("File", file);

			string msg = client.UploadString(host, String.Empty);

			for (int i = 0; i < client.Headers.; i++) {

			}
        }

        public string GetCompleted(string filename, string grade)
        {
            string msg = "GetCompleted " + grade + " " + filename;
            return client.UploadString(host, msg);
        }

        public string[] GetAssignmentList()
        {
            string msg = "TeacherGetAssignmentList ";
			return client.UploadString(host, msg).Split(' ');
        }

        public string AddFeedback(string file, string filename, string grade)
        {
			string checksum = Checksum.GetMd5Hash (file);
			string msg = "AddFeedback " + " " + checksum + " " + grade + " " + filename + " " + file;
            return client.UploadString(host, msg);
        }
    }
}