using System;
using Gtk;

namespace CAS.NET.Desktop
{
    public class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
            Application.Init();
            new MainWindow();
            Application.Run();
        }
    }
}