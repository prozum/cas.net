using System;
using Gtk;
using System.Threading;

namespace ThreadTest
{
    public class SomeCounter : TextView
    {
        private int count = 0;

        public SomeCounter()
            : base()
        {
            Thread thread = new Thread(new ThreadStart(IncreaseCount));
            thread.Start();
        }

        public void IncreaseCount()
        {
            while (true)
            {
                Thread.Sleep(1000);
                count++;
                Buffer.Text = count.ToString();
                this.QueueDraw();
            }
        }
    }
}

