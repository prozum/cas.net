using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace CAS.NET.Server
{
    public class Database
    {
        private readonly string db;
        private MySqlConnection conn;

        public Database(string db)
        {

			// checks if MySQL connection is valid
            this.db = db;

			conn = new MySqlConnection(db);
			conn.Open();
			conn.Close();

			// create tables
			CreateUserDB(db);
            CreateAssignmentDB(db);
            CreateCompletedDB(db);
            CreateFeedbackDB(db);
        }

		// creates user table if it doesn't exist
        private void CreateUserDB(string db)
        {
            try
            {
                conn = new MySqlConnection(db);
                conn.Open();

                const string stm = "SELECT VERSION()";   
                MySqlCommand cmd = new MySqlCommand(stm, conn);
                cmd.CommandText = @"CREATE TABLE IF NOT EXISTS Account(Username VARCHAR(8) PRIMARY KEY,
                                    Password TEXT CHARACTER SET binary, Grade TEXT CHARACTER SET binary, Privilege INT)";
                cmd.ExecuteNonQuery();

            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex);

            }
            finally
            {
                conn.Close();
            }
        }

		// creates assignment table if it doesn't exist
        private void CreateAssignmentDB(string db)
        {
            try
            {
                conn = new MySqlConnection(db);
                conn.Open();

                const string stm = "SELECT VERSION()";   
                MySqlCommand cmd = new MySqlCommand(stm, conn);
                cmd.CommandText = @"CREATE TABLE IF NOT EXISTS Assignment(Username VARCHAR(8), FileName TEXT CHARACTER SET binary,
                                    File TEXT CHARACTER SET binary, Grade TEXT CHARACTER SET binary)";
                cmd.ExecuteNonQuery();
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex);

            }
            finally
            {
                conn.Close();
            }
        }

		// creates table with completed assignments if it doesn't exist
        private void CreateCompletedDB(string db)
        {
            try
            {
                conn = new MySqlConnection(db);
                conn.Open();

                const string stm = "SELECT VERSION()";   
                MySqlCommand cmd = new MySqlCommand(stm, conn);
                cmd.CommandText = @"CREATE TABLE IF NOT EXISTS Completed(Username VARCHAR(8), FileName TEXT CHARACTER SET binary,
                                    File TEXT CHARACTER SET binary, Grade TEXT CHARACTER SET binary, FeedbackGiven INTEGER)";
                cmd.ExecuteNonQuery();
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex);

            }
            finally
            {
                conn.Close();
            }
        }

		// creates table with feedback if it doesn't exist
        private void CreateFeedbackDB(string db)
        {
            try
            {
                conn = new MySqlConnection(db);
                conn.Open();

                const string stm = "SELECT VERSION()";
                MySqlCommand cmd = new MySqlCommand(stm, conn);
                cmd.CommandText = @"CREATE TABLE IF NOT EXISTS Feedback(Username VARCHAR(8), FileName TEXT CHARACTER SET binary,
                                    File TEXT CHARACTER SET binary, Grade TEXT CHARACTER SET binary)";
                cmd.ExecuteNonQuery();
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex);

            }
            finally
            {
                conn.Close();
            }
        }

		// adds assignment to database if it doesn't already exist
        public string AddAssignment(string username, string filename, string file, string grade)
        {
			if (!this.CheckFilenameExists(username, filename, grade, "Assignment"))
            {
                using (conn = new MySqlConnection(db))
                {
                    conn.Open();
                    const string stm = "INSERT INTO Assignment(Username, FileName, File, Grade) VALUES(@username, @filename, @file, @grade)";
                    var cmd = new MySqlCommand(stm, conn);
                    cmd.Parameters.AddWithValue("@username", username);
                    cmd.Parameters.AddWithValue("@filename", filename);
                    cmd.Parameters.AddWithValue("@file", file);
                    cmd.Parameters.AddWithValue("@grade", grade);
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
            else
            {
				using (conn = new MySqlConnection(db))
				{
					conn.Open();
					var cmd = new MySqlCommand("UPDATE Assignment SET File = @file WHERE Username = @username AND FileName = @filename AND Grade = @grade", conn);
					cmd.Parameters.AddWithValue("@file", file);
					cmd.Parameters.AddWithValue("@username", username);
					cmd.Parameters.AddWithValue("@filename", filename);
					cmd.Parameters.AddWithValue("@grade", grade);
					cmd.ExecuteNonQuery();
				}
            }

			return "Success";
        }

		// returns the assignments a specific teacher has assigned
        public string[] TeacherGetAssignmentList(string username)
        {
            List<string> FileList = new List<string>();
            const int FileNameColumn = 1;

            using (conn = new MySqlConnection(db))
            {
                conn.Open();
                const string stm = "SELECT VERSION()";

                var cmd = new MySqlCommand(stm, conn);
                cmd.CommandText = "SELECT * FROM Assignment WHERE Username = @username";
                cmd.Parameters.AddWithValue("@Username", username);

                var rdr = cmd.ExecuteReader();

                if (rdr.HasRows)
                {
                    while (rdr.Read())
                    {
                        FileList.Add(rdr.GetString(FileNameColumn));
                    }
                }
                else
                {
                    FileList.Add("Error");
                }
            }

            return FileList.ToArray();
        }

		// returns a list of completed assignments, to a specific assignment
		public string[] GetCompletedList(string filename, string grade)
		{
			List<String> FeedbackList = new List<string>();
			List<string> TotalStudents = new List<string>();
			List<string> CompletedStudents = new List<string>();
			const int UserNameColumn = 0;
			const int FileNameColumn = 1;
			const int FeedbackColumn = 4;

			using (conn = new MySqlConnection(db))
			{
				conn.Open();
				const string stm = "SELECT * FROM Account WHERE Grade = @grade";

				var cmd = new MySqlCommand(stm, conn);
				cmd.Parameters.AddWithValue("@grade", grade);

				var rdr = cmd.ExecuteReader();

				if (rdr.HasRows)
				{
					while (rdr.Read())
					{
						TotalStudents.Add(rdr.GetString(UserNameColumn));
					}
				}
				else
				{
					TotalStudents.Add("Error");
					return TotalStudents.ToArray();
				}
			}

			using (conn = new MySqlConnection(db))
			{
				conn.Open();
				const string stm = "SELECT * FROM Completed WHERE FileName = @filename AND Grade = @grade";

				var cmd = new MySqlCommand(stm, conn);
				cmd.Parameters.AddWithValue("@filename", filename);
				cmd.Parameters.AddWithValue("@grade", grade);

				var rdr = cmd.ExecuteReader();

				if (rdr.HasRows)
				{
					while (rdr.Read())
					{
						CompletedStudents.Add(rdr.GetString(UserNameColumn));
						FeedbackList.Add(rdr.GetString(FeedbackColumn));
					}
				}
				else
				{
					TotalStudents.Add("No students have completed the assignment");
					return TotalStudents.ToArray();
				}
			}

			int StudentsCount = TotalStudents.Count;

			for (int i = 0; i < StudentsCount; i++)
			{
				int index = CompletedStudents.FindIndex(x => x.StartsWith(TotalStudents[2*i]));

				if (index == -1)
				{
					TotalStudents.Insert((2*i)+1, "NoCompleted");
				}
				else
				{
					Console.WriteLine(FeedbackList[index]);

					if (FeedbackList[index] == "1")
					{
						TotalStudents.Insert((2*i)+1, "Feedback");
					}
					else
					{
						TotalStudents.Insert((2*i)+1, "NoFeedback");
					}
				}
			}

			return TotalStudents.ToArray();
		}

		// gets a specific completed assignment
		public string GetCompleted(string filename, string username, string grade)
        {
            string file;
            const int FileColumn = 2;

            using (conn = new MySqlConnection(db))
            {
                conn.Open();
                const string stm = "SELECT VERSION()";

                var cmd = new MySqlCommand(stm, conn);
                cmd.CommandText = "SELECT * FROM Completed WHERE FileName = @filename AND UserName = @username AND Grade = @grade AND FeedbackGiven = @feedback";
                cmd.Parameters.AddWithValue("@filename", filename);
				cmd.Parameters.AddWithValue("@username", username);
                cmd.Parameters.AddWithValue("@grade", grade);
                cmd.Parameters.AddWithValue("@feedback", 0);

                using (var rdr = cmd.ExecuteReader())
                {
                    if (rdr.HasRows)
                    {
                        rdr.Read();
                        file = rdr.GetString(FileColumn);
                    }
                    else
                    {
                        file = "No more assignments to give feedback";
                    }
                }
            }

            return file;
        }

		// add feedback to a specific assignment
		public string AddFeedback(string filename, string file, string username, string grade)
        {
			if (CheckFilenameExists(username, filename, grade, "Feedback"))
			{
				using (conn = new MySqlConnection(db))
				{
					conn.Open();
					var cmd = new MySqlCommand("INSERT INTO Feedback(Username, FileName, File, Grade) Values(@username, @filename, @file, @grade)", conn);
					cmd.Parameters.AddWithValue("@username", username);
					cmd.Parameters.AddWithValue("@filename", filename);
					cmd.Parameters.AddWithValue("@file", file);
					cmd.Parameters.AddWithValue("@grade", grade);
					cmd.ExecuteNonQuery();
				}

				using (conn = new MySqlConnection(db))
				{
					conn.Open();
					var cmd = new MySqlCommand("UPDATE Completed SET FeedbackGiven = @newfeedback WHERE Username = @username AND FileName = @filename AND Grade = @grade", conn);
					cmd.Parameters.AddWithValue("@newfeedback", 1);
					cmd.Parameters.AddWithValue("@username", username);
					cmd.Parameters.AddWithValue("@filename", filename);
					cmd.Parameters.AddWithValue("@grade", grade);
					cmd.ExecuteNonQuery();
				}
			}
			else
			{
				using (conn = new MySqlConnection(db))
				{
					conn.Open();
					var cmd = new MySqlCommand("UPDATE Feedback SET File = @file WHERE Username = @username AND FileName = @filename AND Grade = @grade", conn);
					cmd.Parameters.AddWithValue("@file", file);
					cmd.Parameters.AddWithValue("@username", username);
					cmd.Parameters.AddWithValue("@filename", filename);
					cmd.Parameters.AddWithValue("@grade", grade);
					cmd.ExecuteNonQuery();
				}
			}

			return "Success";
        }

        public string GetAssignment(string filename, string grade)
        {
            string file;
            const int FileColumn = 2;

            using (conn = new MySqlConnection(db))
            {
                conn.Open();
                const string stm = "SELECT VERSION()";

                var cmd = new MySqlCommand(stm, conn);
                cmd.CommandText = "SELECT * FROM Assignment WHERE FileName = @filename AND Grade = @grade";
                cmd.Parameters.AddWithValue("@filename", filename);
                cmd.Parameters.AddWithValue("@grade", grade);

                var rdr = cmd.ExecuteReader();

                if (rdr.HasRows)
                {
                    rdr.Read();
                    file = rdr.GetString(FileColumn);

                }
                else
                {
                    file = "Error";
                }
            }

            return file;
        }

		// returns a list of the assignments available to a given grade
        public string[] StudentGetAssignmentList(string grade)
        {
            List<string> FileList = new List<string>();
            const int FileNameColumn = 1;

            using (conn = new MySqlConnection(db))
            {
                conn.Open();
                const string stm = "SELECT VERSION()";

                var cmd = new MySqlCommand(stm, conn);
                cmd.CommandText = "SELECT * FROM Assignment WHERE Grade = @grade";
                cmd.Parameters.AddWithValue("@grade", grade);

                var rdr = cmd.ExecuteReader();

                if (rdr.HasRows)
                {
                    while (rdr.Read())
                    {
                        FileList.Add(rdr.GetString(FileNameColumn));
                    }

                }
                else
                {
                    FileList.Add("Error");
                }
            }

            return FileList.ToArray();
        }

		// add a completed assignment
        public string AddCompleted(string username, string filename, string file, string grade)
        {
			if (!CheckFilenameExists(username, filename, grade, "Completed"))
			{
				using (conn = new MySqlConnection(db))
				{
					conn.Open();
					const string stm = "INSERT INTO Completed(Username, FileName, File, Grade, FeedbackGiven) VALUES(@username, @filename, @file, @grade, @feedback)";

					var cmd = new MySqlCommand(stm, conn);
					cmd.Parameters.AddWithValue("@username", username);
					cmd.Parameters.AddWithValue("@filename", filename);
					cmd.Parameters.AddWithValue("@file", file);
					cmd.Parameters.AddWithValue("@grade", grade);
					cmd.Parameters.AddWithValue("@feedback", 0);
					cmd.ExecuteNonQuery();
				}
			}
			else if (CheckCompletedOverwritable(username, filename, grade))
			{
				using (conn = new MySqlConnection(db))
				{
					conn.Open();
					const string stm = "UPDATE Completed SET File = @Newfile WHERE Username = @username AND Grade = @grade AND FileName = @filename AND FeedbackGiven = @feedback";

					var cmd = new MySqlCommand(stm, conn);
					cmd.Parameters.AddWithValue("@username", username);
					cmd.Parameters.AddWithValue("@filename", filename);
					cmd.Parameters.AddWithValue("@Newfile", file);
					cmd.Parameters.AddWithValue("@grade", grade);
					cmd.Parameters.AddWithValue("@feedback", 0);

					cmd.ExecuteNonQuery();
				}
			}

			return "Success";
        }

		// gets feedback for a specific assignment
		public string GetFeedback(string username, string filename, string grade)
		{
			string file;
			const int FileColumn = 2;

			using (conn = new MySqlConnection(db))
			{
				conn.Open();
				const string stm = "SELECT * FROM Feedback WHERE Username = @username AND FileName = @filename AND Grade = @grade";
				var cmd = new MySqlCommand(stm, conn);
				cmd.Parameters.AddWithValue ("@username", username);
				cmd.Parameters.AddWithValue("@filename", filename);
				cmd.Parameters.AddWithValue("@grade", grade);

                var rdr = cmd.ExecuteReader();

				if (rdr.HasRows)
				{
					rdr.Read();
					file = rdr.GetString(FileColumn);

				}
				else
				{
					file = "No feedback on this assignment";
				}
			}

			return file;
		}

		// checks user privilege level
        public int CheckPrivilege(string username, string password)
        {
            const int PrivilegeColumn = 3;
            const string stm = "SELECT * FROM Account WHERE Username = @username AND Password = @password";

            using (conn = new MySqlConnection (db)) {
                conn.Open ();

                var cmd = new MySqlCommand (stm, conn);
                cmd.Parameters.AddWithValue ("@username", username);
                cmd.Parameters.AddWithValue ("@password", password);

                var rdr = cmd.ExecuteReader ();

                if (rdr.HasRows) {
                    rdr.Read ();
                    return rdr.GetInt32 (PrivilegeColumn);
                } else {
                    /* wrong username or password */
                    return -1;
                }
            }
        }

		// returns the grade of a given user
        public string GetGrade(string username)
        {
            const int GradeColumn = 2;
            const string stm = "SELECT * FROM Account WHERE Username = @username";

            using (conn = new MySqlConnection (db)){
                conn.Open ();

                var cmd = new MySqlCommand(stm, conn);
                cmd.Parameters.AddWithValue("@username", username);

                var rdr = cmd.ExecuteReader();

                if (rdr.HasRows)
                {
                    rdr.Read();
                    return rdr.GetString(GradeColumn);
                }
            }

            return null;
        }

		// checks if a filename is already taken
		public bool CheckFilenameExists(string username, string filename, string grade, string table)
		{
			string stm = "SELECT * FROM " + table + " WHERE Username = @username AND Grade = @grade AND FileName = @filename";

			using (conn = new MySqlConnection (db)) {
				conn.Open ();
				var cmd = new MySqlCommand (stm, conn);
				cmd.Parameters.AddWithValue ("@username", username);
				cmd.Parameters.AddWithValue ("@filename", filename);
				cmd.Parameters.AddWithValue ("@grade", grade);
				cmd.Parameters.AddWithValue("@table", table);

				var rdr = cmd.ExecuteReader ();

				return rdr.HasRows;
			}
		}

		// checks if completed assignment is overwritable
		// completed assignment is overwritable if feedback isn't given yet
		public bool CheckCompletedOverwritable(string username, string filename, string grade)
		{
			const string stm = "SELECT * FROM Completed WHERE Username = @username AND FileName = @filename AND Grade = @grade AND FeedbackGiven = @feedback";

			using (conn = new MySqlConnection(db)) {
				conn.Open();

				var cmd = new MySqlCommand (stm, conn);
				cmd.Parameters.AddWithValue ("@username", username);
				cmd.Parameters.AddWithValue ("@filename", filename);
				cmd.Parameters.AddWithValue ("@grade", grade);
				cmd.Parameters.AddWithValue ("@feedback", 0);

				var rdr = cmd.ExecuteReader ();

				return rdr.HasRows;
			}
		}

		// clean account table
		public void CleanAccount()
		{
			try
			{
				conn = new MySqlConnection(db);
				conn.Open();

				const string stm = "SELECT VERSION()";   
				MySqlCommand cmd = new MySqlCommand(stm, conn);
				cmd.CommandText = @"DELETE FROM Account";
				cmd.ExecuteNonQuery();

			}
			catch (MySqlException ex)
			{
				Console.WriteLine(ex);

			}
			finally
			{
				conn.Close();
			}
		}

		// clean assignment table
		public void CleanAssignment()
		{
			try
			{
				conn = new MySqlConnection(db);
				conn.Open();

				const string stm = "SELECT VERSION()";   
				MySqlCommand cmd = new MySqlCommand(stm, conn);
				cmd.CommandText = @"DELETE FROM Assignment";
				cmd.ExecuteNonQuery();

			}
			catch (MySqlException ex)
			{
				Console.WriteLine(ex);

			}
			finally
			{
				conn.Close();
			}
		}

		// clean completed assignments table
		public void CleanCompleted()
		{
			try
			{
				conn = new MySqlConnection(db);
				conn.Open();

				const string stm = "SELECT VERSION()";   
				MySqlCommand cmd = new MySqlCommand(stm, conn);
				cmd.CommandText = @"DELETE FROM Completed";
				cmd.ExecuteNonQuery();

			}
			catch (MySqlException ex)
			{
				Console.WriteLine(ex);

			}
			finally
			{
				conn.Close();
			}
		}

		// clean feedback table
		public void CleanFeedback()
		{
			try
			{
				conn = new MySqlConnection(db);
				conn.Open();

				const string stm = "SELECT VERSION()";   
				MySqlCommand cmd = new MySqlCommand(stm, conn);
				cmd.CommandText = @"DELETE FROM Feedback";
				cmd.ExecuteNonQuery();

			}
			catch (MySqlException ex)
			{
				Console.WriteLine(ex);

			}
			finally
			{
				conn.Close();
			}
		}

		// add user to account table
		public void AddUser(string username, string password, string grade, int privilege)
		{
			try
			{
				conn = new MySqlConnection(db);
				conn.Open();

				const string stm = @"INSERT INTO Account(Username, Password, Grade, Privilege) Values(@username, @password, @grade, @privilege)";   
				MySqlCommand cmd = new MySqlCommand(stm, conn);
				cmd.Parameters.AddWithValue("@username", username);
				cmd.Parameters.AddWithValue("@password", password);
				cmd.Parameters.AddWithValue("@grade", grade);
				cmd.Parameters.AddWithValue("@privilege", privilege);
				cmd.ExecuteNonQuery();
			}
			catch (MySqlException ex)
			{
				Console.WriteLine(ex);

			}
			finally
			{
				conn.Close();
			}
		}

		// creates database mydb
		public void CreateDB()
		{
			try
			{
				conn = new MySqlConnection(db);
				conn.Open();

				const string stm = @"CREATE DATABASE IF NOT EXISTS mydb";   
				MySqlCommand cmd = new MySqlCommand(stm, conn);
				cmd.ExecuteNonQuery();
			}
			catch (MySqlException ex)
			{
				Console.WriteLine(ex);

			}
			finally
			{
				conn.Close();
			}
		}
    }
}