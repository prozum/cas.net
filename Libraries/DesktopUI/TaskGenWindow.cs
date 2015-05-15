﻿using System;
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

        public TaskGenWindow(TextViewList textviews)
            : base("Task Gen Window")
        {
            this.textviews = textviews;

            Table table = new Table(5, 2, true);

            Label labelMinimum = new Label("Minimum:");
            Label labelMaximum = new Label("Maximum:");
            Label labelVariables = new Label("Number of variables: ");
            Label labelNoT = new Label("Number of tasks:");

            SpinButton spinbuttonMinimum = new SpinButton(1, 100, 1);
            SpinButton spinbuttonMaximum = new SpinButton(1, 100, 1);
            SpinButton spinbuttonVariables = new SpinButton(2, 5, 1);
            SpinButton spinbuttonNumberOfTasks = new SpinButton(1, 5, 1);

            spinbuttonMaximum.Value = 20;

            Button buttonOk = new Button("Ok");
            Button buttonCancel = new Button("Cancel");

            //Sets the table
            table.Attach(labelMinimum, 0, 1, 0, 1);
            table.Attach(spinbuttonMinimum, 1, 2, 0, 1);
            table.Attach(labelMaximum, 0, 1, 1, 2);
            table.Attach(spinbuttonMaximum, 1, 2, 1, 2);
            table.Attach(labelVariables, 0, 1, 2, 3);
            table.Attach(spinbuttonVariables, 1, 2, 2, 3);
            table.Attach(labelNoT, 0, 1, 3, 4);
            table.Attach(spinbuttonNumberOfTasks, 1, 2, 3, 4);
            table.Attach(buttonCancel, 0, 1, 4, 5);
            table.Attach(buttonOk, 1, 2, 4, 5);

            buttonCancel.Clicked += (sender, e) =>
            {
                this.Destroy();
            };

            buttonOk.Clicked += (sender, e) =>
            {

                for (int generatedTaskes = 0; generatedTaskes < spinbuttonNumberOfTasks.Value; generatedTaskes++)
                {
                    TaskGenLib.Task t = TaskGenLib.TaskGen.MakeCalcTask((int)spinbuttonMinimum.Value, (int)spinbuttonMaximum.Value, (int)spinbuttonVariables.Value);
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
