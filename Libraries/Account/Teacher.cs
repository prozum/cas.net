using System;
using System.Net;
using System.Text;
using System.IO;

namespace Account
{
    public class Teacher
    {
        private string username;
        private string password;
        static string host = "http://localhost:8080/";

        public Teacher(string username, string password)
        {
            this.username = username;
            this.password = password;
        }

        public string AddAssignment(string checksum, string file, string filename, string grade)
        {
            var client = new WebClient();
            client.Encoding = System.Text.Encoding.UTF8;
            client.Credentials = new NetworkCredential(username, password);

            string msg = "AddAssignment " + checksum + " " + grade + " " + username + " " + password + " " + filename + " " + file;
            string response = client.UploadString(host, msg);

            return response;
        }

        public string GetCompleted(string filename, string grade)
        {
            var client = new WebClient();
            client.Encoding = System.Text.Encoding.UTF8;
            client.Credentials = new NetworkCredential(username, password);

            string msg = "GetCompleted " + grade + " " + username + " " + password + " " + filename;
            string response = client.UploadString(host, msg);

            return response;
        }

        public string[] GetAssignmentList()
        {
            var client = new WebClient();
            client.Encoding = System.Text.Encoding.UTF8;
            client.Credentials = new NetworkCredential(username, password);

            string msg = "TeacherGetAssignmentList " + username + " " + password;
            string response = client.UploadString(host, msg);

            return response.Split(' ');
        }

        public string AddFeedback(string file, string filename, string grade)
        {
            var client = new WebClient();
            client.Encoding = System.Text.Encoding.UTF8;
            client.Credentials = new NetworkCredential(username, password);

            string msg = "AddFeedback " + grade + " " + username + " " + password + " " + filename + " " + file;
            string response = client.UploadString(host, msg);

            return response;
        }
    }
}