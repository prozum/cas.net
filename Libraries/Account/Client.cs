using System;
using System.Net;
using System.Text;
using System.IO;

namespace Account
{
	public abstract class Client
	{
		public virtual void Login(string URI)
		{
			WebClient client = new WebClient ();

			Stream data = client.OpenRead (URI);
			StreamReader reader = new StreamReader (data);

			string s = reader.ReadToEnd ();
			Console.WriteLine (s);

			data.Close ();
			reader.Close ();
		}

		public abstract void GetTask();

		public abstract void PushTask();
	}
}

