using System;
using System.Net;
using System.Text;
using System.IO;

namespace Account
{
	public class Teacher
	{
		private int ID;

		public Teacher (string Name)
		{

		}

		public void AddStudent()
		{
			throw new NotImplementedException();
		}

        public string AddAssignment(string file, string filename, string grade, string username, string password)
		{
			var client = new WebClient ();
			client.Encoding = System.Text.Encoding.UTF8;

            string msg = "AddAssignment " + grade + " " + username + " " + password + " " + filename + " " + file;
			string response = client.UploadString("http://localhost:8080/", msg);

			return response;
		}

		public string GetCompleted(string filename, string grade, string username, string password)
		{
			var client = new WebClient ();
			client.Encoding = System.Text.Encoding.UTF8;

			string msg = "GetCompleted " + grade + " " + username + " " + password + " " + filename;
			string response = client.UploadString("http://localhost:8080/", msg);

			return response;
		}

		public void GetCompleted()
		{
			throw new NotImplementedException();
		}

		public void AddFeedback()
		{
			throw new NotImplementedException();
		}
	}
}