using System;
using System.Collections.Generic;
using System.Data.SQLite;

namespace CAS.NET.Server
{
	public class Database
	{
		public const string db = "URI=file:data.db";
		public static SQLiteConnection Connection;

		public Database()
		{
			OpenDB ();
			CreateUserDB ();
			CreateTaskDB ();
		}

		private static void OpenDB()
		{
			Connection = new SQLiteConnection(db);
			Connection.Open ();
			Connection.Open ();
		}

		private static void CreateUserDB()
		{
			var cmd = new SQLiteCommand(db); 

			cmd.CommandText = @"CREATE TABLE IF NOT EXIST User(Id TEXT PRIMARY KEY, Password TEXT, Klass TEXT, Privilege INTEGER)";
			cmd.ExecuteNonQuery();
		}

		private static void CreateTaskDB()
		{
			var cmd = new SQLiteCommand (db);

			cmd.CommandText = @"CREATE TABLE IF NOT EXIST Task(Id TEXT PRIMARY KEY, TaskName TEXT, SaveFileName TEXT, Klass TEXT, Feedback INTEGER)";
			cmd.ExecuteNonQuery();
		}

		public static void AddUser(string login, string password, string klass, bool privilege)
		{
			using (var cmd = new SQLiteCommand (db)) {
				if (privilege) {
					cmd.CommandText = "INSERT INTO User VALUES(login, password, klass, 1)";
					cmd.ExecuteNonQuery();
				}
				else {
					cmd.CommandText = "INSERT INTO User VALUES(login, password, klass, 0)";
					cmd.ExecuteNonQuery();
				}
			}
		}

		public static void RemoveUser(string login)
		{
			using (var cmd = new SQLiteCommand (db)) {
				cmd.CommandText = "DELETE FROM User WHERE username = login";
				cmd.ExecuteNonQuery();
			}
		}

		public static void AddTask()
		{
			throw new NotImplementedException ();
		}
	}
}