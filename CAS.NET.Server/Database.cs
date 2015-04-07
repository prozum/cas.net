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
			this.db = db;
			CreateUserDB (db);
			CreateAssignmentDB (db);
			CreateCompletedDB (db);
			CreateFeedbackDB (db);
		}

		private void CreateUserDB(string db)
		{
			try 
			{
				conn = new MySqlConnection(db);
				conn.Open();

				string stm = "SELECT VERSION()";   
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


		private void CreateAssignmentDB(string db)
		{
			try 
			{
				conn = new MySqlConnection(db);
				conn.Open();

				string stm = "SELECT VERSION()";   
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

		private void CreateCompletedDB(string db)
		{
			try 
			{
				conn = new MySqlConnection(db);
				conn.Open();

				string stm = "SELECT VERSION()";   
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

		private void CreateFeedbackDB(string db)
		{
			try 
			{
				conn = new MySqlConnection(db);
				conn.Open();

				string stm = "SELECT VERSION()";
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

		public void AddAssignment(string username, string filename, string file, string grade)
		{
			using (conn = new MySqlConnection(db)) {
				conn.Open();
				const string stm = "SELECT VERSION()";

				var cmd = new MySqlCommand (stm, conn);
				cmd.CommandText = "INSERT INTO Assignment(Username, FileName, File, Grade) VALUES(@username, @filename, @file, @grade)";
				cmd.Parameters.AddWithValue("@username", username);
				cmd.Parameters.AddWithValue("@filename", filename);
				cmd.Parameters.AddWithValue("@file", file);
				cmd.Parameters.AddWithValue("@grade", grade);
				cmd.ExecuteNonQuery();
			}
		}

		public string[] TeacherGetAssignmentList(string username)
		{
			List<string> FileList = new List<string>();
			int FileNameColumn = 1;

			using (conn = new MySqlConnection(db)) {
				conn.Open();
				const string stm = "SELECT VERSION()";

				var cmd = new MySqlCommand (stm, conn);
				cmd.CommandText = "SELECT * FROM Assignment WHERE Username = @username";
				cmd.Parameters.AddWithValue ("@Username", username);

				var rdr = cmd.ExecuteReader ();

				if (rdr.HasRows)
				{
					while(rdr.Read())
					{
						FileList.Add(rdr.GetString (FileNameColumn));
					}
				}
				else
				{
					FileList.Add("Error");
				}
			}

			return FileList.ToArray();
		}

		public string GetCompleted(string filename, string grade)
		{
			string file;
			int FileColumn = 2;

			using (conn = new MySqlConnection(db)) {
				conn.Open();
				const string stm = "SELECT VERSION()";

				var cmd = new MySqlCommand (stm, conn);
				cmd.CommandText = "SELECT * FROM Completed WHERE FileName = @filename AND Grade = @grade AND FeedbackGiven = @feedback";
				cmd.Parameters.AddWithValue ("@filename", filename);
				cmd.Parameters.AddWithValue ("@grade", grade);
				cmd.Parameters.AddWithValue("@feedback", 0);

				using (var rdr = cmd.ExecuteReader())
				{
					if (rdr.HasRows)
					{
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

		public void AddFeedback(string filename, string file, string grade)
		{
			using (conn = new MySqlConnection(db)) {
				conn.Open();
				const string stm = "SELECT VERSION()";
				int UsernameColumn = 0;
				string username;

				var cmd = new MySqlCommand (stm, conn);
				cmd.CommandText = "SELECT * FROM Completed WHERE FileName = @filename AND Grade = @grade AND FeedbackGiven = @feedback";
				cmd.Parameters.AddWithValue ("@filename", filename);
				cmd.Parameters.AddWithValue ("@grade", grade);
				cmd.Parameters.AddWithValue("@feedback", 0);

				using (var rdr = cmd.ExecuteReader())
				{
					if (rdr.HasRows)
					{
						rdr.Read();
						username = rdr.GetString(UsernameColumn);

						cmd.CommandText = "INSERT INTO Feedback WHERE Username = @username AND FileName = @filename AND File = @file AND Grade = @grade";
						cmd.Parameters.AddWithValue("@username", username);
						cmd.Parameters.AddWithValue("@filename", filename);
						cmd.Parameters.AddWithValue("@file", file);
						cmd.Parameters.AddWithValue("@grade", grade);
						cmd.ExecuteNonQuery();

						cmd.CommandText = "UPDATE Completed SET FeedbackGiven = @feedback WHERE Username = @username AND FileName = @filename AND Grade = @grade";
						cmd.Parameters.AddWithValue("@feedback", 1);
						cmd.Parameters.AddWithValue("@username", username);
						cmd.Parameters.AddWithValue("@filename", filename);
						cmd.Parameters.AddWithValue("@grade", grade);
						cmd.ExecuteNonQuery();
					}
				}
			}
		}

        public string GetAssignment(string filename, string grade)
		{
            string file;
			int FileColumn = 2;

			using (conn = new MySqlConnection(db)) {
				conn.Open();
				const string stm = "SELECT VERSION()";

				var cmd = new MySqlCommand (stm, conn);
				cmd.CommandText = "SELECT * FROM Assignment WHERE FileName = @filename AND Grade = @grade";
				cmd.Parameters.AddWithValue ("@filename", filename);
				cmd.Parameters.AddWithValue ("@grade", grade);

				var rdr = cmd.ExecuteReader ();

				if (rdr.HasRows)
				{
					rdr.Read();
					file = rdr.GetString (FileColumn);

				}
				else
				{
					file = "Error";
				}
			}

			return file;
		}

        public string[] StudentGetAssignmentList(string grade)
		{
			List<string> FileList = new List<string>();
			int FileNameColumn = 1;

			using (conn = new MySqlConnection(db)) {
				conn.Open();
				const string stm = "SELECT VERSION()";

				var cmd = new MySqlCommand (stm, conn);
				cmd.CommandText = "SELECT * FROM Assignment WHERE Grade = @grade";
				cmd.Parameters.AddWithValue ("@grade", grade);

				var rdr = cmd.ExecuteReader ();

				if (rdr.HasRows)
				{
					while(rdr.Read())
					{
						FileList.Add(rdr.GetString (FileNameColumn));
					}

				}
				else
				{
					FileList.Add("Error");
				}
			}

			return FileList.ToArray();
		}

		public void AddCompleted(string username, string filename, string file, string grade)
		{
			using (conn = new MySqlConnection(db)) {
				conn.Open();
				const string stm = "SELECT VERSION()";

				var cmd = new MySqlCommand (stm, conn);
				cmd.CommandText = "INSERT INTO Completed(Username, FileName, File, Grade, FeedbackGiven) VALUES(@username, @filename, @file, @grade, @feedback)";
				cmd.Parameters.AddWithValue("@username", username);
				cmd.Parameters.AddWithValue("@filename", filename);
				cmd.Parameters.AddWithValue("@file", file);
				cmd.Parameters.AddWithValue("@grade", grade);
				cmd.Parameters.AddWithValue("@feedback", 0);
				cmd.ExecuteNonQuery();
			}
		}

		public int ValidateUser(string username, string password)
		{
			int PrivilegeColumn = 3;

			string stm = "SELECT * FROM Account WHERE Username = @username AND Password = @password";

			var cmd = new MySqlCommand (stm, conn);
			cmd.Parameters.AddWithValue ("@username", username);
			cmd.Parameters.AddWithValue ("@password", password);

			var rdr = cmd.ExecuteReader ();

			if (rdr.HasRows)
			{
				rdr.Read();
				return rdr.GetInt32(PrivilegeColumn);
			}
			else
			{
				/* wrong username or password */
				return -1;
			}
		}

		public string GetGrade(string username, string password)
		{
			int GradeColumn = 2;

			string stm = "SELECT * FROM Account WHERE Username = @username AND Password = @password";

			var cmd = new MySqlCommand (stm, conn);
			cmd.Parameters.AddWithValue ("@username", username);
			cmd.Parameters.AddWithValue ("@password", password);

			var rdr = cmd.ExecuteReader ();

			if (rdr.HasRows)
			{
				rdr.Read();
				return rdr.GetString(GradeColumn);
			}
		}

		/*
		public void AddUser(string login, string password, string grade, int privilege)
		{
			using (var Connection = new SQLiteConnection(this.db)) {
				Connection.Open();

				using (var cmd = new SQLiteCommand(Connection)) {
					cmd.CommandText = "INSERT INTO User VALUES(login, password, grade, privilege)";
					cmd.ExecuteNonQuery();
				}

				Connection.Close();
			}
		}

		public void RemoveUser(string username)
		{
			using (var Connection = new SQLiteConnection(this.db)) {
				Connection.Open();

				using (var cmd = new SQLiteCommand(Connection)) {
					cmd.CommandText = "DELETE FROM User WHERE Username = username";
					cmd.ExecuteNonQuery();
				}

				Connection.Close();
			}
		}
		*/

		/*
		public void AddCompleted(string username, string taskname, string savefilename, string grade)
		{
			using (var Connection = new SQLiteConnection(this.db)) {
				Connection.Open();

				using (var cmd = new SQLiteCommand(Connection)) {
					cmd.CommandText = "INSERT INTO Completed VALUES(username, taskname, savefilename, grade)";
					cmd.ExecuteNonQuery();
				}

				Connection.Close();
			}
		}

		public void AddFeedback(string username, string taskname, string savefilename, string grade)
		{
			using (var Connection = new SQLiteConnection(this.db)) {
				Connection.Open();

				using (var cmd = new SQLiteCommand(Connection)) {
					cmd.CommandText = "INSERT INTO Feedback VALUES(username, taskname, savefilename, grade)";
					cmd.ExecuteNonQuery();
				}

				Connection.Close();
			}
		}
		*/
	}
}