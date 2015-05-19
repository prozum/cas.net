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

            Table table = new Table(16, 2, true);

            Label labelCircle = new Label();
            labelCircle.Markup = "<b> Circle: </b>";
            labelCircle.SetAlignment (0, 2);
            Label labelCircleCircumference = new Label("Circumference (radius): ");
            Entry entryCircleCircumference = new Entry("");
            labelCircleCircumference.SetAlignment (0, 2);
            entryCircleCircumference.Activated += (o, a) =>
            {
                entryCircleCircumference.Text = Geomet.Circle.Circumference(double.Parse(entryCircleCircumference.Text)).ToString();
            };

            Label labelCircleArea = new Label("Area (radius):");
            Entry entryCircleArea = new Entry("");
            labelCircleArea.SetAlignment (0, 2);
            entryCircleArea.Activated += (o, a) =>
            {
                entryCircleArea.Text = Geomet.Circle.Area(double.Parse(entryCircleArea.Text)).ToString();
            };


            Label labelSquare = new Label();
            labelSquare.Markup = "<b> Square: </b>";
            labelSquare.SetAlignment (0, 2);
            Label labelSquareCircumference = new Label("Circumference (width, height): ");
            labelSquareCircumference.SetAlignment (0, 2);
            Entry entrySquareCircumference = new Entry("");
            entrySquareCircumference.Activated += (o, a) =>
            {
                //Skal bruge 2 inputs
                //entrySquareCircumference.Text = Geomet.Square.Circumference(double.Parse(entrySquareCircumference.Text)).ToString();
            };

            Label labelSquareArea = new Label("Area (width, height): ");
            labelSquareArea.SetAlignment (0, 2);
            Entry entrySquareArea = new Entry("");
            entrySquareArea.Activated += (o, a) =>
            {
                //skal bruge 2 inputs
                //entrySquareCircumference.Text = Geomet.Square.Area(double.Parse(entrySquareCircumference.Text)).ToString();
            };

            Label labelSphere = new Label();

            labelSphere.Markup = "<b> Sphere: </b>";
            labelSphere.SetAlignment (0, 2);
            Label labelSphereVolume = new Label("Volume (radius): ");
            labelSphereVolume.SetAlignment (0, 2);
            Entry entrySphereVolume = new Entry("");
            entrySphereVolume.Activated += (o, a) =>
            {
                entrySphereVolume.Text = Geomet.Sphere.Volume(double.Parse(entrySphereVolume.Text)).ToString();
            };

            Label labelSphereSurfaceArea = new Label("Surface area (radius):");
            labelSphereSurfaceArea.SetAlignment (0, 2);
            Entry entrySphereSurfaceArea = new Entry("");
            entrySphereSurfaceArea.Activated += (o, a) =>
            {
                entrySphereSurfaceArea.Text = Geomet.Sphere.SurfaceArea(double.Parse(entrySphereSurfaceArea.Text)).ToString();
            };

            Label labelCube = new Label();
            labelCube.Markup = "<b> Cube: </b>";
            labelCube.SetAlignment (0, 2);
            Label labelCubeVolume = new Label("Volume (length, width, height): ");
            labelCubeVolume.SetAlignment (0, 2);
            Entry entryCubeVolume = new Entry("");
            entryCubeVolume.Activated += (o, a) =>
            {
                //Skal bruge 3 inputs
                //entryCubeVolume.Text = Geomet.Cube.Volume(double.Parse(entryCubeVolume.Text)).ToString();
            };

            Label labelCubeSurfaceArea = new Label("Surface area (length, width, height):");
            Entry entryCubeSurfaceArea = new Entry("");
            entrySphereSurfaceArea.Activated += (o, a) =>
            {
                //skal bruge 3 inputs
                //entryCubeSurfaceArea.Text = Geomet.Cube.SurfaceArea(double.Parse(entryCubeSurfaceArea.Text)).ToString();
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
