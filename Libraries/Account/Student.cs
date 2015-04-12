using System;
using System.IO;
using System.Net;
using System.Text;

namespace Account
{
    public class Student
    {
        private string username;
        private string password;
        static string host = "http://localhost:8080/";

        public Student (string username, string password)
        {
            this.username = username;
            this.password = password;
        }

        public string AddCompleted(string file, string filename)
        {
            var client = new WebClient ();
            client.Encoding = System.Text.Encoding.UTF8;
            client.Credentials = new NetworkCredential(username, password);

            string msg = "AddCompleted " + username + " " + password + " " + filename + " " + file;
            string response = client.UploadString(host, msg);

            return response;
        }

        public string[] GetAssignmentList()
        {
            var client = new WebClient ();
            client.Encoding = System.Text.Encoding.UTF8;
            client.Credentials = new NetworkCredential(username, password);

            string msg = "StudentGetAssignmentList " + username + " " + password;
            string response = client.UploadString(host, msg);

            return response.Split(' ');
        }

        public string GetAssignment(string filename)
        {
            var client = new WebClient ();
            client.Encoding = System.Text.Encoding.UTF8;
            client.Credentials = new NetworkCredential(username, password);

            string msg = "GetAssignment " + username + " " + password + " " + filename;
            string response = client.UploadString(host, msg);

            return response;
        }

        public string GetFeedback(string filename)
        {
            var client = new WebClient ();
            client.Encoding = System.Text.Encoding.UTF8;
            client.Credentials = new NetworkCredential(username, password);

            string msg = "GetFeedback " + username + " " + password + " " + filename;
            string response = client.UploadString(host, msg);

            return response;
        }
    }
}