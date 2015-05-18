using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gtk;
using Geomet;

namespace DesktopUI
{
    public class GeometTable
    {
        public Table CreateGeometTable()
        {
            Table table = new Table(12, 2, false);

            Label labelCircle = new Label("Circle:");
            Label labelCircleCircumferance = new Label("Circumferance: ");
            Entry entryCicleCircumferance = new Entry("");
            entryCicleCircumferance.Activated += (o, a) =>
            {
                entryCicleCircumferance.Text = Geomet.Circle.Circumference(double.Parse(entryCicleCircumferance.Text)).ToString();
            };


            Label labelCicleArea = new Label("Area: ");
            Entry entryCicleArea = new Entry("");

            Label labelSquare = new Label("Square");
            Label labelSquareCircumferance = new Label("Circumferance: 2 * l + 2 * h");
            Label labelSquareArea = new Label("Area: l * h");



            Button buttonOk = new Button("Ok");
            Button buttonCancel = new Button("Cancel");

            //Sets the table
            table.Attach(labelCircle, 0, 1, 0, 1);
            table.Attach(labelCircleCircumferance, 0, 1, 1, 2);
            table.Attach(entryCicleCircumferance, 1, 2, 1, 2);
            table.Attach(labelSquare, 0, 1, 2, 3);
            table.Attach(labelSquareCircumferance, 0, 1, 3, 4);
            table.Attach(labelSquareArea, 1, 2, 3, 4);


            return table;
        }
    }
}
