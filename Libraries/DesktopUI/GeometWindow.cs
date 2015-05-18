using System;
using Gtk;

namespace DesktopUI
{
    public class GeometWindow : Window
    {
        TextViewList textviews;

        // Constructor for taskgenwindow
        public GeometWindow(TextViewList textviews)
            : base("Geomet Window")
        {
            this.textviews = textviews;

            Table table = new Table(5, 2, true);
           

            Label labelCircle = new Label("Circle:");
            Label labelCircleCircumferance = new Label("Circumferance: 2 * Pi * r");
            Label labelCicleArea = new Label("Area: Pi * r * r");

            Label labelSquare = new Label("Square");
            Label labelSquareCircumferance = new Label("Circumferance: 2 * l + 2 * h");
            Label labelSquareArea = new Label("Area: l * h");


           
            Button buttonOk = new Button("Ok");
            Button buttonCancel = new Button("Cancel");

            //Sets the table
            table.Attach(labelCircle, 0, 1, 0, 1);
            table.Attach(labelCircleCircumferance, 0, 1, 1, 2);
            table.Attach(labelCicleArea, 1, 2, 1, 2);
            table.Attach(labelSquare, 0, 1, 2, 3);
            table.Attach(labelSquareCircumferance, 0, 1, 3, 4);
            table.Attach(labelSquareArea, 1, 2, 3, 4);

            buttonCancel.Clicked += (sender, e) =>
            {
                this.Destroy();
            };

            buttonOk.Clicked += (sender, e) =>
            {
                this.Destroy();
            };

            Add(table);
            ShowAll();
        }


    }
}
