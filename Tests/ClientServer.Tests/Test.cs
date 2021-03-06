﻿/*
using System;
using System.Threading;
using Account;
using CAS.NET.Server;
using ImEx;
using NUnit.Framework;

namespace ClientServer
{
    [TestFixture()]
    public class Test
    {
		const string host = "http://localhost:8080/";
		const string database = @"server=localhost;userid=travis2;database=mydb";

		Teacher teacher = new Teacher("teacher", "passwd0", host);
		Student student1 = new Student("student1", "passwd1", host);
		Student student2 = new Student("student2", "passwd2", host);
		Student student3 = new Student("student3", "passwd3", host);
		Student student4 = new Student("student4", "passwd4", host);
		Student student5 = new Student("student5", "passwd5", host);

		static Database db = new Database(database);
		Server server = new Server(host, db);

		void run()
		{
			server.StartListen();
		}

        [Test()]
        public void TestCase()
        {
			db.CleanAssignment ();
			db.CleanCompleted ();
			db.CleanFeedback ();
			db.CleanAccount ();

			Thread thread = new Thread(run);
			thread.Start ();

			db.AddUser ("student1", "passwd1", "9A2016", 0);
			db.AddUser ("student2", "passwd2", "9A2016", 0);
			db.AddUser ("student3", "passwd3", "9A2016", 0);
			db.AddUser ("student4", "passwd4", "9A2016", 0);
			db.AddUser ("student5", "passwd5", "9A2016", 0);
			db.AddUser ("teacher", "passwd0", "teacher", 1);

			string assignment = "2+2=4";
			string assignmentName = "AssignmentFilename";
			string grade = "9A2016";
			string response = teacher.AddAssignment (assignment, assignmentName, grade);
			Assert.AreEqual (response, "Success");

			string getAssignment1 = student1.GetAssignment ("AssignmentFilename");
			string getAssignment2 = student2.GetAssignment ("AssignmentFilename");
			string getAssignment3 = student3.GetAssignment ("AssignmentFilename");
			string getAssignment4 = student4.GetAssignment ("AssignmentFilename");
			string getAssignment5 = student5.GetAssignment ("AssignmentFilename");
			Assert.AreEqual (getAssignment1, assignment);
			Assert.AreEqual (getAssignment2, assignment);
			Assert.AreEqual (getAssignment3, assignment);
			Assert.AreEqual (getAssignment4, assignment);
			Assert.AreEqual (getAssignment5, assignment);

			getAssignment1 += "_completed";
			getAssignment2 += "_completed";
			getAssignment3 += "_completed";
			getAssignment4 += "_completed";
			getAssignment5 += "_completed";
			assignment += "_completed";

			string addCompleted1 = student1.AddCompleted (getAssignment1, "AssignmentFilename");
			string addCompleted2 = student2.AddCompleted (getAssignment2, "AssignmentFilename");
			string addCompleted3 = student3.AddCompleted (getAssignment3, "AssignmentFilename");
			string addCompleted4 = student4.AddCompleted (getAssignment4, "AssignmentFilename");
			string addCompleted5 = student5.AddCompleted (getAssignment5, "AssignmentFilename");
			Assert.AreEqual (addCompleted1, "Success");
			Assert.AreEqual (addCompleted2, "Success");
			Assert.AreEqual (addCompleted3, "Success");
			Assert.AreEqual (addCompleted4, "Success");
			Assert.AreEqual (addCompleted5, "Success");

			assignment += "_feedback";

			string getCompleted1 = teacher.GetCompleted ("AssignmentFilename", grade);
			string addFeedback1 = teacher.AddFeedback (assignment, assignmentName, grade);
			string getFeedback1 = student1.GetFeedback (assignmentName);

			string getCompleted2 = teacher.GetCompleted ("AssignmentFilename", grade);
			string addFeedback2 = teacher.AddFeedback (assignment, assignmentName, grade);
			string getFeedback2 = student1.GetFeedback (assignmentName);

			string getCompleted3 = teacher.GetCompleted ("AssignmentFilename", grade);
			string addFeedback3 = teacher.AddFeedback (assignment, assignmentName, grade);
			string getFeedback3 = student1.GetFeedback (assignmentName);

			string getCompleted4 = teacher.GetCompleted ("AssignmentFilename", grade);
			string addFeedback4 = teacher.AddFeedback (assignment, assignmentName, grade);
			string getFeedback4 = student1.GetFeedback (assignmentName);

			string getCompleted5 = teacher.GetCompleted ("AssignmentFilename", grade);
			string addFeedback5 = teacher.AddFeedback (assignment, assignmentName, grade);
			string getFeedback5 = student1.GetFeedback (assignmentName);

			Assert.AreEqual (teacher.GetCompleted(assignmentName, grade), "No more assignments to give feedback");

			Assert.AreEqual (assignment, student1.GetFeedback(assignmentName));
			Assert.AreEqual (assignment, student2.GetFeedback(assignmentName));
			Assert.AreEqual (assignment, student3.GetFeedback(assignmentName));
			Assert.AreEqual (assignment, student4.GetFeedback(assignmentName));
			Assert.AreEqual (assignment, student5.GetFeedback(assignmentName));

			thread.Abort ();
        }
    }
}
*/