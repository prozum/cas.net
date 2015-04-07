using System;
using System.Net;
using System.Text;
using System.IO;

namespace Account
{
    public class Teacher
    {
        private int ID;
        static string host = "http://localhost:8080/";

        public Teacher (string Name)
        {

        }

        public string AddAssignment(string file, string filename, string grade, string username, string password)
        {
            var client = new WebClient ();
            client.Encoding = System.Text.Encoding.UTF8;

            string msg = "AddAssignment " + grade + " " + username + " " + password + " " + filename + " " + file;
            string response = client.UploadString(host, msg);

            return response;
        }

        public string GetCompleted(string filename, string grade, string username, string password)
        {
            var client = new WebClient ();
            client.Encoding = System.Text.Encoding.UTF8;

            string msg = "GetCompleted " + grade + " " + username + " " + password + " " + filename;
            string response = client.UploadString(host, msg);

            return response;
        }

        public string[] GetAssignmentList(string username, string password)
        {
            var client = new WebClient ();
            client.Encoding = System.Text.Encoding.UTF8;

            string msg = "TeacherGetAssignmentList " + username + " " + password;
            string response = client.UploadString(host, msg);

            return response.Split(' ');
        }

        public string AddFeedback(string file, string filename, string grade, string username, string password)
        {
            var client = new WebClient ();
            client.Encoding = System.Text.Encoding.UTF8;

            string msg = "AddAssignment " + grade + " " + username + " " + password + " " + filename + " " + file;
            string response = client.UploadString(host, msg);

            return response;
        }
    }
}