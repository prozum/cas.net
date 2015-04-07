using System;
using Account;

namespace ClientServerCommunication
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            Teacher teacher = new Teacher("kasp6378");
            Student student = new Student("kasp6378", "9A2016");
            Console.WriteLine(teacher.AddAssignment("jsonfileextremeoverload", "testfile", "9A2016", "kasp6378", args[0]));
            string[] list = student.GetAssignmentList( "kasp6378", args[0]);

            foreach (var item in list) {
                Console.WriteLine (item);
            }

            Console.WriteLine(student.GetAssignment("testfile", "kasp6378", args[0]));
        }
    }
}
