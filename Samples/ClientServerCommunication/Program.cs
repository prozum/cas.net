using System;
using Account;

namespace ClientServerCommunication
{
	class MainClass
	{
		public static void Main(string[] args)
		{
			Teacher teacher = new Teacher("kasp6378");
			teacher.AddAssignment("jsonfileextemeoverload", "9A2016", "kasp6378", "***REMOVED***");
			Console.WriteLine("Hello World!");
		}
	}
}
