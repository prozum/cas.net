using System;
using Gtk;
using System.Threading;

namespace CAS.NET.Desktop
{
    public class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
            Application.Init();
            new MainWindow();

            Thread thread = new Thread(new ThreadStart(ThreadRoutine));
            thread.Start();

            Application.Run();
        }

        static void ThreadRoutine()
        {
            while (Application.EventsPending())
            {
                Gtk.Application.RunIteration();
            }
        }

    }
}