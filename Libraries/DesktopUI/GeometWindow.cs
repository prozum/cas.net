using System;
using Gtk;

namespace DesktopUI
{
    public class GeometWindow : Window
    {
        TextViewList textviews;

        // Constructor for Geometwindow
        public GeometWindow(TextViewList textviews)
            : base("Geometry Tools")
        {
            this.textviews = textviews;
            double radius, length, width, height;
            Table table = new Table(16, 2, true);

            string[] sInput;

            Label labelCircle = new Label();
            labelCircle.Markup = "<b> Circle: </b>";
            labelCircle.SetAlignment(0, 2);
            Label labelCircleCircumference = new Label("Circumference (radius): ");
            Entry entryCircleCircumference = new Entry("");
            labelCircleCircumference.SetAlignment(0, 2);
            entryCircleCircumference.Activated += (o, a) =>
            {
                radius = 0;
                double.TryParse(entryCircleCircumference.Text, out radius);
                entryCircleCircumference.Text = Geomet.Circle.Circumference(radius).ToString();
            };

            Label labelCircleArea = new Label("Area (radius):");
            Entry entryCircleArea = new Entry("");
            labelCircleArea.SetAlignment(0, 2);
            entryCircleArea.Activated += (o, a) =>
            {
                radius = 0;
                double.TryParse(entryCircleArea.Text, out radius);
                entryCircleArea.Text = Geomet.Circle.Area(radius).ToString();
            };


            Label labelSquare = new Label();
            labelSquare.Markup = "<b> Square: </b>";
            labelSquare.SetAlignment(0, 2);
            Label labelSquareCircumference = new Label("Circumference (width; length): ");
            labelSquareCircumference.SetAlignment(0, 2);
            Entry entrySquareCircumference = new Entry("");
            entrySquareCircumference.Activated += (o, a) =>
            {
                sInput = entrySquareCircumference.Text.Split(';');

                if (sInput.Length == 2 && double.TryParse(sInput[0], out width) && double.TryParse(sInput[1], out length))
                {

                    entrySquareCircumference.Text = Geomet.Square.Circumference(width, length).ToString();

                }
                else entrySquareCircumference.Text = "0";
            };

            Label labelSquareArea = new Label("Area (width; length): ");
            labelSquareArea.SetAlignment(0, 2);
            Entry entrySquareArea = new Entry("");
            entrySquareArea.Activated += (o, a) =>
            {
                sInput = entrySquareArea.Text.Split(';');

                if (sInput.Length == 2 && double.TryParse(sInput[0], out width) && double.TryParse(sInput[1], out length))
                {

                    entrySquareArea.Text = Geomet.Square.Area(width, length).ToString();

                }
                else entrySquareArea.Text = "0";
            };

            Label labelSphere = new Label();

            labelSphere.Markup = "<b> Sphere: </b>";
            labelSphere.SetAlignment(0, 2);
            Label labelSphereVolume = new Label("Volume (radius): ");
            labelSphereVolume.SetAlignment(0, 2);
            Entry entrySphereVolume = new Entry("");
            entrySphereVolume.Activated += (o, a) =>
            {
                radius = 0;
                double.TryParse(entrySphereVolume.Text, out radius);
                entrySphereVolume.Text = Geomet.Sphere.Volume(radius).ToString();
            };

            Label labelSphereSurfaceArea = new Label("Surface area (radius):");
            labelSphereSurfaceArea.SetAlignment(0, 2);
            Entry entrySphereSurfaceArea = new Entry("");
            entrySphereSurfaceArea.Activated += (o, a) =>
            {
                radius = 0;
                double.TryParse(entrySphereSurfaceArea.Text, out radius);
                entrySphereSurfaceArea.Text = Geomet.Sphere.SurfaceArea(radius).ToString();
            };

            Label labelCube = new Label();
            labelCube.Markup = "<b> Cube: </b>";
            labelCube.SetAlignment(0, 2);
            Label labelCubeVolume = new Label("Volume (length; width; height): ");
            labelCubeVolume.SetAlignment(0, 2);
            Entry entryCubeVolume = new Entry("");
            entryCubeVolume.Activated += (o, a) =>
            {
                sInput = entryCubeVolume.Text.Split(';');

                if (sInput.Length == 3 && double.TryParse(sInput[0], out length) && double.TryParse(sInput[1], out width) && double.TryParse(sInput[2], out height))
                {

                    entryCubeVolume.Text = Geomet.Cube.Volume(length, width, height).ToString();

                }
                else entryCubeVolume.Text = "0";
            };

            Label labelCubeSurfaceArea = new Label("Surface area (length; width; height):");
            Entry entryCubeSurfaceArea = new Entry("");
            entryCubeSurfaceArea.Activated += (o, a) =>
            {
                sInput = entryCubeSurfaceArea.Text.Split(';');

                if (sInput.Length == 3 && double.TryParse(sInput[0], out length) && double.TryParse(sInput[1], out width) && double.TryParse(sInput[2], out height))
                {

                    entryCubeSurfaceArea.Text = Geomet.Cube.SurfaceArea(length, width, height).ToString();

                }
                else entryCubeSurfaceArea.Text = "0";
            };

            //Sets the table
            table.Attach(labelCircle, 0, 1, 0, 1);
            table.Attach(labelCircleCircumference, 0, 1, 1, 2);
            table.Attach(entryCircleCircumference, 1, 2, 1, 2);
            table.Attach(labelCircleArea, 0, 1, 2, 3);
            table.Attach(entryCircleArea, 1, 2, 2, 3);

            table.Attach(labelSquare, 0, 1, 4, 5);
            table.Attach(labelSquareCircumference, 0, 1, 5, 6);
            table.Attach(entrySquareCircumference, 1, 2, 5, 6);
            table.Attach(labelSquareArea, 0, 1, 6, 7);
            table.Attach(entrySquareArea, 1, 2, 6, 7);

            table.Attach(labelSphere, 0, 1, 8, 9);
            table.Attach(labelSphereSurfaceArea, 0, 1, 9, 10);
            table.Attach(entrySphereSurfaceArea, 1, 2, 9, 10);
            table.Attach(labelSphereVolume, 0, 1, 10, 11);
            table.Attach(entrySphereVolume, 1, 2, 10, 11);

            table.Attach(labelCube, 0, 1, 12, 13);
            table.Attach(labelCubeSurfaceArea, 0, 1, 13, 14);
            table.Attach(entryCubeSurfaceArea, 1, 2, 13, 14);
            table.Attach(labelCubeVolume, 0, 1, 14, 15);
            table.Attach(entryCubeVolume, 1, 2, 14, 15);

            Add(table);
            ShowAll();
        }
    }
}
