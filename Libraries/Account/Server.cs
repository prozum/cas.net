using System;
using System.Net;
using System.Text;
using System.IO;

namespace Account
{
	public class Server
	{
		public Server ()
		{
		}

		public static void StartListen(string prefixes, Func<HttpListenerRequest, string> method)
		{
			if (prefixes == null || prefixes.Length == 0)
			{
				throw new ArgumentException("prefixes");
			}

			HttpListener listener = new HttpListener();

			listener.Prefixes.Add(prefixes);

			listener.Start();
			Console.WriteLine("Listening...");

			HttpListenerContext context = listener.GetContext();
			HttpListenerRequest request = context.Request;

			HttpListenerResponse response = context.Response;

			string responseString = method(request);
			byte[] buffer = System.Text.Encoding.UTF8.GetBytes(responseString);

			response.ContentLength64 = buffer.Length;
			System.IO.Stream output = response.OutputStream;
			output.Write(buffer,0,buffer.Length);

			output.Close();
			listener.Stop();
		}

	}
}

