using System;
using System.Globalization;
using System.Threading;
using Gtk;

namespace CAS.NET.Desktop
{
    public class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo("en-US");
            Application.Init();
            new MainWindow();
            Application.Run();
        }
    }
}