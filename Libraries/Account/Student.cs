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

        public string[] GetAssignmentList(string username, string password)
        {
            var client = new WebClient ();
            client.Encoding = System.Text.Encoding.UTF8;

            string msg = "GetAssignmentList " + username + " " + password;
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

        public string AddCompleted(string filename, string file, string username, string password)
        {
            var client = new WebClient ();
            client.Encoding = System.Text.Encoding.UTF8;

            string msg = "AddCompleted " +  username + " " + password + " " + filename;
            string response = client.UploadString(host, msg);

            return response;
        }

        public void PushTask()
        {
            throw new NotImplementedException();
        }
    }
}