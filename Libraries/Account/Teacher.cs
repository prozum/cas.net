using System;
using System.Net;
using System.Text;
using System.IO;

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

        public string AddAssignment(string checksum, string file, string filename, string grade)
        {
            string msg = "AddAssignment " + checksum + " " + grade + " " + filename + " " + file;
			return client.UploadString(host, msg);
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
            string msg = "AddFeedback " + grade + " " + filename + " " + file;
            return client.UploadString(host, msg);
        }
    }
}