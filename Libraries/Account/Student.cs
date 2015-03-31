using System;
using System.IO;
using System.Net;
using System.Text;

namespace Account
{
	public class Student
	{
		private int ID;

		public Student (string Name, string Class)
		{

		}

		public string[] GetAssignmentList(string grade, string username, string password)
		{
			var client = new WebClient ();
			client.Encoding = System.Text.Encoding.UTF8;

			string msg = "GetAssignmentList " + grade + " " + username + " " + password;
			string response = client.UploadString("http://localhost:8080/", msg);

			return response.Split(' ');
		}

        public string GetAssignment(string filename, string grade, string username, string password)
		{
			var client = new WebClient ();
			client.Encoding = System.Text.Encoding.UTF8;

			string msg = "GetAssignment " + grade + " " + username + " " + password + " " + filename;
			string response = client.UploadString("http://localhost:8080/", msg);

			return response;
		}

		public void PushTask()
		{
			throw new NotImplementedException();
		}
	}
}

