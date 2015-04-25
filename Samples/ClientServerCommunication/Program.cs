using System;
using Account;
using ImEx;

namespace ClientServerCommunication
{
    class MainClass
    {
        public static void Main()
        {
			const string host = "http://localhost:8080/";

			Teacher teacher = new Teacher("teacher", "passwd0", host);
			Student student1 = new Student("student1", "passwd1", host);
			Student student2 = new Student("student2", "passwd2", host);
			Student student3 = new Student("student3", "passwd3", host);
			Student student4 = new Student("student4", "passwd4", host);
			Student student5 = new Student("student5", "passwd5", host);

			string assignment = "AssignmentFilename";
			string completed = "2+2=4_completed";

			/*
			student1.AddCompleted(completed + 1, assignment);
			student2.AddCompleted(completed + 2, assignment);
			student3.AddCompleted(completed + 3, assignment);
			student4.AddCompleted(completed + 4, assignment);
			student5.AddCompleted(completed + 5, assignment);
			*/

			string[] StudentList = teacher.GetCompletedList(assignment, "9A2016");

			foreach (var item in StudentList)
			{
				Console.WriteLine(item);
			}

			teacher.AddFeedback(completed + 1 + "_feedback", assignment, "student1", "9A2016" );

			StudentList = teacher.GetCompletedList(assignment, "9A2016");

			foreach (var item in StudentList)
			{
				Console.WriteLine(item);
			}

			/*
            string file = "jsonfileextremeoverload";
            string fileName = "testfile";

            Console.WriteLine(teacher.AddAssignment(file, fileName, "9A2016"));
            
            string[] list = student.GetAssignmentList();

            foreach (var item in list)
            {
                Console.WriteLine(item);
            }
                     
            Console.WriteLine(student.GetAssignment("testfile"));

            Console.WriteLine(student.AddCompleted("jsonfileextremeoverload_completed", "testfile"));

            Console.WriteLine(teacher.GetCompleted("testfile", "9A2016"));

            Console.WriteLine(teacher.AddFeedback("jsonfileextremeoverload_completed_feedback", "testfile", "9A2016"));

            Console.WriteLine(student.GetFeedback("testfile"));
			*/
        }
    }
}
