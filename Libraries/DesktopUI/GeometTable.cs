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
            Table table = new Table(5, 2, true);

            Label labelCircle = new Label();
            labelCircle.Markup = "<b> Circle: </b>";
            Label labelCircleCircumference = new Label("Circumference: ");
            Entry entryCircleCircumference = new Entry("");
            entryCircleCircumference.Activated += (o, a) =>
            {
                entryCircleCircumference.Text = Geomet.Circle.Circumference(double.Parse(entryCircleCircumference.Text)).ToString();
            };
            Label labelCircleArea = new Label("Area: ");
            Entry entryCircleArea = new Entry("Area");
            entryCircleArea.Activated += (o, a) =>
                {
                    entryCircleArea.Text = Geomet.Circle.Area(double.Parse(entryCircleArea.Text)).ToString();
                };




            Label labelSquare = new Label();
            labelSquare.Markup = "<b> Square: </b>";

            Label labelSquareCircumference = new Label("Circumference: 2 * l + 2 * h");
            Label labelSquareArea = new Label("Area: l * h");



            Button buttonOk = new Button("Ok");
            Button buttonCancel = new Button("Cancel");

            //Sets the table
            table.Attach(labelCircle, 0, 1, 0, 1);
            table.Attach(labelCircleCircumference, 0, 1, 1, 2);
            table.Attach(entryCircleCircumference, 1, 2, 1, 2);
            table.Attach(labelCircleArea, 0, 1, 2, 3);
            table.Attach(entryCircleArea, 1, 1, 2, 3);

            table.Attach(labelSquare, 0, 1, 3, 4);
            table.Attach(labelSquareCircumference, 0, 1, 4, 4);
            table.Attach(labelSquareArea, 1, 2, 3, 4);

            table.SetRowSpacing(1, 1);
            table.SetRowSpacing(2, 1);


            return table;
        }
    }
}
