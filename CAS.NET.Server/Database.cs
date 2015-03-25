using System;
using System.Collections.Generic;
using System.Data.SQLite;

namespace CAS.NET.Server
{
	public class Database
	{
		//public string db; // = "URI=file:data.db"

		private SQLiteConnection conn;
		private SQLiteCommand cmd;

		public Database(string db)
		{
			SQLiteConnection.CreateFile(db);
			conn = new SQLiteConnection(db);
			conn.Open();

			/*
			CreateUserDB ();
			CreateAssignmentDB ();
			CreateCompletedDB();
			CreateFeedbackDB();
			*/
			//cmd = new SQLiteCommand (Connection);
		}

		private void CreateUserDB()
		{
			cmd.CommandText = @"CREATE TABLE IF NOT EXIST User(Username TEXT PRIMARY KEY, Password TEXT, Grade TEXT, Privilege INTEGER)";
			cmd.ExecuteNonQuery();
		}

		private void CreateAssignmentDB()
		{
			cmd.CommandText = @"CREATE TABLE IF NOT EXIST Assignment(Username TEXT, TaskName TEXT, SaveFileName TEXT, Grade TEXT)";
			cmd.ExecuteNonQuery();
		}

		private void CreateCompletedDB()
		{
			cmd.CommandText = @"CREATE TABLE IF NOT EXIST Completed(Username TEXT, TaskName TEXT, SaveFileName TEXT, Grade TEXT)";
			cmd.ExecuteNonQuery();
		}

		private void CreateFeedbackDB()
		{
			/* teachers need to give the student username here */
			cmd.CommandText = @"CREATE TABLE IF NOT EXIST Feedback(Username TEXT, TaskName TEXT, SaveFileName TEXT, Grade TEXT)";
			cmd.ExecuteNonQuery();
		}

		public void AddUser(string login, string password, string grade, int privilege)
		{
			cmd.CommandText = "INSERT INTO User VALUES(login, password, grade, privilege)";
			cmd.ExecuteNonQuery();
		}

		public void RemoveUser(string username)
		{
			cmd.CommandText = "DELETE FROM User WHERE Username = username";
			cmd.ExecuteNonQuery();
		}

		public void AddAssignment(string username, string taskname, string savefilename, string grade)
		{
			cmd.CommandText = "INSERT INTO Assignment VALUES(username, taskname, savefilename, grade)";
			cmd.ExecuteNonQuery();
		}

		public void AddCompleted(string username, string taskname, string savefilename, string grade)
		{
			cmd.CommandText = "INSERT INTO Completed VALUES(username, taskname, savefilename, grade)";
			cmd.ExecuteNonQuery();
		}

		public void AddFeedback(string username, string taskname, string savefilename, string grade)
		{
			cmd.CommandText = "INSERT INTO Feedback VALUES(username, taskname, savefilename, grade)";
			cmd.ExecuteNonQuery();
		}
	}
}