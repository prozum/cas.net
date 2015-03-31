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
				cmd.CommandText = @"CREATE TABLE IF NOT EXISTS User(Username VARCHAR(8) PRIMARY KEY,
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
				cmd.CommandText = @"CREATE TABLE IF NOT EXISTS Completed(Username VARCHAR(8), TaskName TEXT CHARACTER SET binary,
									SaveFileName TEXT CHARACTER SET binary, Grade TEXT CHARACTER SET binary)";
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
				cmd.CommandText = @"CREATE TABLE IF NOT EXISTS Feedback(Username VARCHAR(8), TaskName TEXT CHARACTER SET binary,
									SaveFileName TEXT CHARACTER SET binary, Grade TEXT CHARACTER SET binary)";
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

            conn.Close();
		}

        public string GetAssignment(string filename, string grade)
		{
            string file;
			int filecolumn = 2;

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
					file = rdr.GetString (filecolumn);

				}
				else
				{
					file = "Error";
				}
			}

            conn.Close();

			return file;
		}

        public string[] GetAssignmentList(string grade)
        {
            return null;
        }

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