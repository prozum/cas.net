using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gtk;

namespace DesktopUI
{
    public class TaskGenUnitWindow: Window
    {
        TextViewList textviews;

        // Constructor for taskgenwindow
        public TaskGenUnitWindow(TextViewList textviews)
            : base("TaskGen Unit Conversion Window")
        {
            this.textviews = textviews;

            Table table = new Table(5, 2, true);

            Label labelMinimum = new Label("Minimum:");
            Label labelMaximum = new Label("Maximum:");
            Label labelNoT = new Label("Number of tasks:");

            SpinButton spinbuttonMinimum = new SpinButton(1, 100, 1);
            SpinButton spinbuttonMaximum = new SpinButton(1, 100, 1);
            SpinButton spinbuttonNumberOfTasks = new SpinButton(1, 5, 1);

            spinbuttonMaximum.Value = 20;

            Button buttonOk = new Button("Ok");
            Button buttonCancel = new Button("Cancel");

            //Sets the table
            table.Attach(labelMinimum, 0, 1, 0, 1);
            table.Attach(spinbuttonMinimum, 1, 2, 0, 1);
            table.Attach(labelMaximum, 0, 1, 1, 2);
            table.Attach(spinbuttonMaximum, 1, 2, 1, 2);
            table.Attach(labelNoT, 0, 1, 2, 3);
            table.Attach(spinbuttonNumberOfTasks, 1, 2, 2, 3);
            table.Attach(buttonCancel, 0, 1, 3, 4);
            table.Attach(buttonOk, 1, 2, 3, 4);

            buttonCancel.Clicked += (sender, e) =>
            {
                this.Destroy();
            };

            buttonOk.Clicked += (sender, e) =>
            {

                for (int generatedTaskes = 0; generatedTaskes < spinbuttonNumberOfTasks.Value; generatedTaskes++)
                {
                    TaskGenLib.Task t = TaskGenLib.TaskGen.MakeUnitTask((int)spinbuttonMinimum.Value, (int)spinbuttonMaximum.Value);
                    textviews.InsertTaskGenTextView(t.TaskDescription);
                    //textviews.InsertResult(,);
                }


                this.Destroy();
            };

            Add(table);
            ShowAll();
        }


    }
}