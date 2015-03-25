using System;
using System.Net;
using System.Text;
using System.IO;

namespace Account
{
	public abstract class Client
	{
		public abstract void GetTask();

		public abstract void PushTask();
	}
}

