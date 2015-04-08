using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace CAS.NET.Server
{
    public static class Server
    {
        public static void StartListen(string prefix, Database db)
        {
            if (string.IsNullOrEmpty (prefix)) {
                throw new ArgumentException ("prefix");
            }

            using (var listener = new HttpListener()) {
            
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

                    using(var memoryStream = new MemoryStream())
                    {
                        reader.CopyTo(memoryStream);
                        buffer = memoryStream.ToArray();
                        msg = Encoding.UTF8.GetString(buffer);
                    }

                    Console.WriteLine(msg);

                    var remsg = ExecuteCommand(msg, db);

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
            string command = msg.Substring(0, msg.IndexOf(" "));
            msg = msg.Substring(command.Length+1);

            Console.WriteLine (command);
            Console.WriteLine (msg);
                
            switch (command)
            {
            case "AddAssignment":
            	return TeacherAddAssignment (msg, db);
            case "GetCompleted":
                return TeacherGetCompleted (msg, db);
			case "AddFeedback":
				return TeacherAddFeedback (msg, db);
			case "TeacherGetAssignmentList":
				return TeacherGetAssignmentList (msg, db);            
            case "GetAssignment":
            	return StudentGetAssignment (msg, db);
            case "StudentGetAssignmentList":
                return StudentGetAssignmentList(msg, db);
			case "AddCompleted":
				return StudentAddCompleted (msg, db);
			case "GetFeedback":
				return StudentGetFeedback (msg, db);
            default:
            	return "Invalid command";
            }
        }

        public static string TeacherAddAssignment(string msg, Database db)
        {        
            string[] strArr = msg.Split (' ');

            string grade = strArr[0];
            string username = strArr[1];
            string password = strArr[2];
            string filename = strArr[3];
            string file = String.Empty;

            for (int i = 4; i < strArr.Length; i++) {
                file += strArr[i];
            }           

            if (db.ValidateUser(username, password) != 1)
            {
                return "Invalid teacher";
            }

            db.AddAssignment(username, filename, file, grade);

            return "Successfully added assignment";
        }

        public static string TeacherGetAssignmentList(string msg, Database db)
        {      
            string[] strArr = msg.Split(' ');

            string username = strArr[0];
            string password = strArr[1];

            if (db.ValidateUser(username, password) != 1)
            {
                return "Invalid teacher";
            }

            return string.Join(" ", db.TeacherGetAssignmentList(username));
        }

        public static string TeacherGetCompleted(string msg, Database db)
        {     
            string[] strArr = msg.Split (' ');

            string grade = strArr[0];
            string username = strArr[1];
            string password = strArr[2];
            string filename = strArr[3];           

            if (db.ValidateUser(username, password) != 1)
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

            for (int i = 4; i < strArr.Length; i++) {
                file += strArr[i];
            }

            if (db.ValidateUser(username, password) != 1)
            {
                return "Invalid teacher";
            }

            db.AddFeedback(filename, file, grade);

            return "Successfully added feedback";
        }

        public static string StudentGetAssignmentList(string msg, Database db)
        {
            string[] strArr = msg.Split (' ');

            string username = strArr[0];
            string password = strArr[1];
            string grade = db.GetGrade(username, password);

            if (db.ValidateUser(username, password) != 0)
            {
                return "Invalid student";
            }

            return string.Join(" ", db.StudentGetAssignmentList(grade));
        }

        public static string StudentGetAssignment(string msg, Database db)
        {
            string[] strArr = msg.Split (' ');
                
            string username = strArr[0];
            string password = strArr[1];
            string filename = strArr[2];
            string grade = db.GetGrade(username, password);

            if (db.ValidateUser(username, password) != 0)
            {
                return "Invalid student";
            }

            return db.GetAssignment(filename, grade);
        }

        public static string StudentAddCompleted(string msg, Database db)
        {
            string[] strArr = msg.Split (' ');         

            string username = strArr[0];
            string password = strArr[1];
            string filename = strArr[2];
            string grade = db.GetGrade(username, password);
            string file = String.Empty;

            for (int i = 3; i < strArr.Length; i++) {
                file += strArr[i];
            }

            if (db.ValidateUser(username, password) != 0)
            {
                return "Invalid student";
            }

            db.AddCompleted(username, filename, file, grade);

            return "Successfully added assignment";
        }

		public static string StudentGetFeedback(string msg, Database db)
		{
            string[] strArr = msg.Split (' ');

            string username = strArr[0];
            string password = strArr[1];
            string filename = strArr[2];
            string grade = db.GetGrade(username, password);

			if (db.ValidateUser(username, password) != 0)
			{
				return "Invalid student";
			}

			return db.GetFeedback(username, filename, grade);
		}
    }
}