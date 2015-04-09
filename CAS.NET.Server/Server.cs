using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using ImEx;

namespace CAS.NET.Server
{
    public static class Server
    {
        public static void StartListen(string prefix, Database db)
        {
            if (string.IsNullOrEmpty(prefix))
            {
                throw new ArgumentException("prefix");
            }

            // start listening for HTTP requests
            using (var listener = new HttpListener())
            {
            
                listener.Prefixes.Add(prefix);
                listener.Start();

                while (true)
                {
                    Console.WriteLine("Listening...");
                    var context = listener.GetContext();
                    var request = context.Request;
                    var response = context.Response;

                    Stream reader = request.InputStream;
                    byte[] buffer;
                    string msg;

                    // copy client message to a buffer
                    using (var memoryStream = new MemoryStream())
                    {
                        reader.CopyTo(memoryStream);
                        buffer = memoryStream.ToArray();
                        msg = Encoding.UTF8.GetString(buffer);
                    }

                    Console.WriteLine(msg);

                    // execute client message and get message for client
                    string remsg = ExecuteCommand(msg, db);
                    buffer = System.Text.Encoding.UTF8.GetBytes(remsg);
                    response.ContentLength64 = buffer.Length;
                    System.IO.Stream output = response.OutputStream;
                    output.Write(buffer, 0, buffer.Length);
                    output.Close();
                }
            }
        }

        public static string ExecuteCommand(string msg, Database db)
        {
            // decode command from client message and remove it from msg
            string command = msg.Substring(0, msg.IndexOf(" "));
            msg = msg.Substring(command.Length + 1);

            Console.WriteLine(command);
            Console.WriteLine(msg);

            // decode the command and run serverside code for the command
            switch (command)
            {
                case "AddAssignment":
                    return TeacherAddAssignment(msg, db);
                case "GetCompleted":
                    return TeacherGetCompleted(msg, db);
                case "AddFeedback":
                    return TeacherAddFeedback(msg, db);
                case "TeacherGetAssignmentList":
                    return TeacherGetAssignmentList(msg, db);            
                case "GetAssignment":
                    return StudentGetAssignment(msg, db);
                case "StudentGetAssignmentList":
                    return StudentGetAssignmentList(msg, db);
                case "AddCompleted":
                    return StudentAddCompleted(msg, db);
                case "GetFeedback":
                    return StudentGetFeedback(msg, db);
                default:
                    return "Invalid command";
            }
        }

        public static string TeacherAddAssignment(string msg, Database db)
        {        
            string[] strArr = msg.Split(' ');

            string checksum = strArr[0];
            string grade = strArr[1];
            string username = strArr[2];
            string password = strArr[3];
            string filename = strArr[4];
            string fileContent = strArr[5]; //Used for checksum only
            string file = String.Empty;

            // Generates checksum for the file
            string checksumNew = Checksum.GetMd5Hash(fileContent);

            // string[] strArr = { grade, username, password, filename };

            for (int i = 4; i < strArr.Length; i++)
            {
                file += strArr[i];
            }           

            if (db.CheckPrivilege(username, password) != 1)
            {
                return "Invalid teacher";
            }

            // Writes the checksums
            Console.WriteLine(checksum + " <=> " + checksumNew);

            // Prevents the server from saving the files if it's checksum is invalid
            if (Checksum.VerifyMd5Hash(checksum, checksumNew) == false)
            {
                return "Failed adding assignment - Please try again.";
            }

            return db.AddAssignment(username, filename, file, grade);  
        }

        public static string TeacherGetAssignmentList(string msg, Database db)
        {      
            string[] strArr = msg.Split(' ');

            string username = strArr[0];
            string password = strArr[1];

            if (db.CheckPrivilege(username, password) != 1)
            {
                return "Invalid teacher";
            }

            return string.Join(" ", db.TeacherGetAssignmentList(username));
        }

        public static string TeacherGetCompleted(string msg, Database db)
        {     
            string[] strArr = msg.Split(' ');

            string grade = strArr[0];
            string username = strArr[1];
            string password = strArr[2];
            string filename = strArr[3];           

            if (db.CheckPrivilege(username, password) != 1)
            {
                return "Invalid teacher";
            }

            /* teachers can use this to get other classes completed assignments */
            /* todo fix */
            return db.GetCompleted(filename, grade);
        }

        public static string TeacherAddFeedback(string msg, Database db)
        {
            string[] strArr = msg.Split(' ');

            string grade = strArr[0];
            string username = strArr[1];
            string password = strArr[2];
            string filename = strArr[3];
            string file = String.Empty;

            for (int i = 4; i < strArr.Length; i++)
            {
                file += strArr[i];
            }

            if (db.CheckPrivilege(username, password) != 1)
            {
                return "Invalid teacher";
            }

            db.AddFeedback(filename, file, grade);

            return "Successfully added feedback";
        }

        public static string StudentGetAssignmentList(string msg, Database db)
        {
            string[] strArr = msg.Split(' ');

            string username = strArr[0];
            string password = strArr[1];
            string grade = db.GetGrade(username, password);

            Console.WriteLine(username + "end");
            Console.WriteLine(password + "end");

            if (db.CheckPrivilege(username, password) != 0)
            {
                return "Invalid student";
            }

            return string.Join(" ", db.StudentGetAssignmentList(grade));
        }

        public static string StudentGetAssignment(string msg, Database db)
        {
            string[] strArr = msg.Split(' ');
                
            string username = strArr[0];
            string password = strArr[1];
            string filename = strArr[2];
            string grade = db.GetGrade(username, password);

            if (db.CheckPrivilege(username, password) != 0)
            {
                return "Invalid student";
            }

            return db.GetAssignment(filename, grade);
        }

        public static string StudentAddCompleted(string msg, Database db)
        {
            string[] strArr = msg.Split(' ');         

            string username = strArr[0];
            string password = strArr[1];
            string filename = strArr[2];
            string grade = db.GetGrade(username, password);
            string file = String.Empty;

            for (int i = 3; i < strArr.Length; i++)
            {
                file += strArr[i];
            }

            if (db.CheckPrivilege(username, password) != 0)
            {
                return "Invalid student";
            }

            db.AddCompleted(username, filename, file, grade);

            return "Successfully added completed assignment";
        }

        public static string StudentGetFeedback(string msg, Database db)
        {
            string[] strArr = msg.Split(' ');

            string username = strArr[0];
            string password = strArr[1];
            string filename = strArr[2];
            string grade = db.GetGrade(username, password);

            if (db.CheckPrivilege(username, password) != 0)
            {
                return "Invalid student";
            }

            return db.GetFeedback(username, filename, grade);
        }
    }
}