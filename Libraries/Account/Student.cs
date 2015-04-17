using System;
using System.IO;
using System.Net;
using System.Text;


namespace Account
{
    public class Student
    {
		private readonly string host;
		private WebClient client;

		public Student (string username, string password, string host)
        {
			this.host = host;
			client = new WebClient();
			client.Encoding = System.Text.Encoding.UTF8;
			client.Credentials = new NetworkCredential(username, password);
        }

        public string AddCompleted(string file, string filename)
        {
            string msg = "AddCompleted " + filename + " " + file;
            return client.UploadString(host, msg);
        }

        public string[] GetAssignmentList()
        {
            string msg = "StudentGetAssignmentList ";
			return client.UploadString(host, msg).Split(' ');
        }

        public string GetAssignment(string filename)
        {
            string msg = "GetAssignment " + filename;
			return client.UploadString(host, msg);
        }

		public string GetFeedback(string filename)
        {
            string msg = "GetFeedback " + filename;
			return client.UploadString(host, msg);
        }
    }
}