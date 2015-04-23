using System;
using System.Threading;
using Gtk;

namespace ThreadTest
{
    public class MyClass : Window
    {
        static Label l;
        static TextView tv;
        static int numClick = 0;

        public static void Main()
        {
            Application.Init();
            new MyClass();

            Thread thread = new Thread(new ThreadStart(ThreadRoutine));
            thread.Start();
            Application.Run();
        }

        static void ThreadRoutine()
        {
            // As long as this is running, DrawRequests will be updated 
            // without the need to update the entire workspace
            while (Application.EventsPending())
            {
                Gtk.Application.RunIteration();
            }
        }

        public MyClass()
            : base("Threading window")
        {
            SetDefaultSize(300, 200);

            Grid grid = new Grid();

            l = new Label(numClick.ToString());
            grid.Attach(l, 1, 1, 1, 1);

            Button b = new Button("ClickMe!");
            grid.Attach(b, 1, 2, 1, 1);
            b.Clicked += delegate
            {
                numClick++;
                l.Text = numClick.ToString();
                l.QueueDraw(); // <- Adds the label to the quoue.
            };

            tv = new TextView();
            grid.Attach(tv, 1, 3, 1, 1);

            Button b2 = new Button("ClickMe2!");
            grid.Attach(b2, 1, 4, 1, 1);
            b2.Clicked += delegate
            {
                tv.Buffer.Text += numClick + "\n";
                tv.QueueDraw();
            };

            SomeCounter sc = new SomeCounter();
            grid.Attach(sc, 1, 5, 1, 1);

            Add(grid);

            ShowAll();
        }
    }
}

