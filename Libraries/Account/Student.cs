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

		public string GetAssignment(string file, string grade, string username, string password)
		{
			WebClient client = new WebClient ();
			client.Encoding = System.Text.Encoding.UTF8;

			string msg = "AddAssignment " + grade + " " + username + " " + password + " " + file;
			string response = client.DownloadString("http://localhost:8080/", msg);

			return response;
		}

		public override void PushTask()
		{
			throw new NotImplementedException();
		}
	}
}

