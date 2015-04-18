using System;
using Account;
using ImEx;

namespace ClientServerCommunication
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            Teacher teacher = new Teacher("kasp6378", "password", args[0]);
            Student student = new Student("kasp1234", "password", args[0]);

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
        }
    }
}
