using System;
using System.Net;
using System.Text;
using System.IO;
using Account;

namespace ClientTest
{
	class MainClass
	{
		public class Test
		{
			public static void Main (string[] args)
			{
				Student student = new Student("John Smithee", "9a");
				Teacher teacher = new Teacher("Mr. Lundin");

				/* server stop executing after 1 login, this is on purpose right now */
				student.Login("http://localhost:8080/");
				//teacher.Login("http://localhost:8080/");
			}
		}
	}
}
