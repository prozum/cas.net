using System;
using System.IO;
using System.Net;
using System.Text;

namespace Account
{
    public class Student
    {
        private int ID;
        static string host = "http://localhost:8080/";

        public Student (string Name, string Class)
        {

        }

        public string AddCompleted(string file, string filename, string username, string password)
        {
            var client = new WebClient ();
            client.Encoding = System.Text.Encoding.UTF8;

            string msg = "AddCompleted " + username + " " + password + " " + filename + " " + file;
            string response = client.UploadString(host, msg);

            return response;
        }

        public string[] GetAssignmentList(string username, string password)
        {
            var client = new WebClient ();
            client.Encoding = System.Text.Encoding.UTF8;

            string msg = "StudentGetAssignmentList " + username + " " + password;
            string response = client.UploadString(host, msg);

            return response.Split(' ');
        }

        public string GetAssignment(string filename, string username, string password)
        {
            var client = new WebClient ();
            client.Encoding = System.Text.Encoding.UTF8;

            string msg = "GetAssignment " + username + " " + password + " " + filename;
            string response = client.UploadString(host, msg);

            return response;
        }

        public string GetFeedback(string filename, string username, string password)
        {
            var client = new WebClient ();
            client.Encoding = System.Text.Encoding.UTF8;

            string msg = "GetFeedback " + username + " " + password + " " + filename;
            string response = client.UploadString(host, msg);

            return response;
        }
    }
}