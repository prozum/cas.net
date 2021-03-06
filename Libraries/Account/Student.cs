﻿using System;
using System.Collections.Generic;
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

        public Student(string username, string password, string host)
        {
            this.host = host;
            client = new WebClient();
            client.Encoding = System.Text.Encoding.UTF8;
            client.Credentials = new NetworkCredential(username, password);
        }

		// add completed assignment
        public string AddCompleted(string file, string filename)
        {
            client.Headers.Add("Checksum", Checksum.GetMd5Hash(file));
            client.Headers.Add("Filename", filename);
            client.Headers.Add("File", file);

            string response = client.UploadString(host, "AddCompleted");

            client.Headers.Clear();

			if (response == "Failed")
			{
				return null;
			}
			else if (response == "Corruption")
			{
				return null;
			}

            return response;
        }

		// get assignment list
        public string[] GetAssignmentList()
        {
            string response = client.UploadString(host, "StudentGetAssignmentList");

            client.Headers.Clear();

            if (response == "Success")
            {
				List<string> filelist = new List<string>();
				List<string> checksumlist = new List<string>();
				int i = 0;
            
				while (client.ResponseHeaders["File" + i.ToString()] != null)
				{
					filelist.Add(client.ResponseHeaders["File" + i.ToString()]);
					checksumlist.Add(client.ResponseHeaders["Checksum" + i.ToString()]);

					if (Checksum.GetMd5Hash(filelist[i]) != checksumlist[i])
					{
						return this.GetAssignmentList();
					}

					i++;
				}

				return filelist.ToArray();
            }

			return null;
        }

		// get a specific assignment
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