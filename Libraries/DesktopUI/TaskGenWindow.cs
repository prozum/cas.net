using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gtk;

namespace DesktopUI
{
    class TaskGenWindow : Window
    {
        TextViewList textviews;

        public TaskGenWindow(TextViewList textviews) : base("Task Gen Window")
        {
            this.textviews = textviews;

            SpinButton spinbuttonMinimum = new SpinButton(1, 100, 1);
            SpinButton spinbuttonMaximum = new SpinButton(1, 100, 1);
            SpinButton spinbuttonVariables = new SpinButton(2, 5, 1);
            VBox vbox = new VBox(false,5);

            vbox.Add(spinbuttonMinimum);
            vbox.Add(spinbuttonMaximum);
            vbox.Add(spinbuttonVariables);
            Add(vbox);

            ShowAll();
        }
    }
}
