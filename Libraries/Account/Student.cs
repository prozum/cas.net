using System;
using System.IO;
using System.Net;
using System.Text;
using ImEx;

namespace Account
{
    public class Student
    {
        private readonly string host;
        private WebClient client;

        // http://localhost/:8080

        public Student(string username, string password, string host)
        {
            this.host = host;
            client = new WebClient();
            client.Encoding = System.Text.Encoding.UTF8;
            client.Credentials = new NetworkCredential(username, password);
        }

        public string AddCompleted(string file, string filename)
        {
            client.Headers.Add("Checksum", Checksum.GetMd5Hash(file));
            client.Headers.Add("Filename", filename);
            client.Headers.Add("File", file);

            string response = client.UploadString(host, "AddCompleted");

            client.Headers.Clear();

            return response;
        }

        public string[] GetAssignmentList()
        {
            string response = client.UploadString(host, "StudentGetAssignmentList");

            client.Headers.Clear();

            if (response == "Success")
            {
                string[] filelist = new string[client.ResponseHeaders.Count / 2];
                string[] checksumlist = new string[client.ResponseHeaders.Count / 2];

                for (int i = 0; i < client.ResponseHeaders.Count / 2; i++)
                {
                    filelist[i] = client.ResponseHeaders["File" + i.ToString()];
                    checksumlist[i] = client.ResponseHeaders["Checksum" + i.ToString()];

                    if (Checksum.GetMd5Hash(filelist[i]) != checksumlist[i])
                    {
                        return this.GetAssignmentList();
                    }
                }
            }

            return null;
        }

        public string GetAssignment(string filename)
        {
            client.Headers.Add("Filename", filename);

            string response = client.UploadString(host, "GetAssignment");

            client.Headers.Clear();

            if (response == "Success")
            {
                string file = client.ResponseHeaders["File"];
                string checksum = client.ResponseHeaders["Checksum"];

                if (checksum == Checksum.GetMd5Hash(file))
                {
                    return file;
                }
                else
                {
                    return this.GetAssignment(filename);
                }
            }
            Console.WriteLine("noice");
            return null;
        }

        public string GetFeedback(string filename)
        {
            client.Headers.Add("Filename", filename);
            string response = client.UploadString(host, "GetFeedback");

            client.Headers.Clear();

            if (response == "Success")
            {
                string file = client.ResponseHeaders["File"];
                string checksum = client.ResponseHeaders["Checksum"];

                if (checksum == Checksum.GetMd5Hash(file))
                {
                    return file;
                }
                else
                {
                    return this.GetFeedback(filename);
                }
            }

            return null;
        }
    }
}