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
            Table table = new Table(16, 2, true);

            Label labelCircle = new Label();
            labelCircle.Markup = "<b> Circle: </b>";
            Label labelCircleCircumference = new Label("Circumference: ");
            Entry entryCircleCircumference = new Entry("");
            entryCircleCircumference.Activated += (o, a) =>
            {
                entryCircleCircumference.Text = Geomet.Circle.Circumference(double.Parse(entryCircleCircumference.Text)).ToString();
            };
           
            Label labelCircleArea = new Label("Area: ");
            Entry entryCircleArea = new Entry("");
            entryCircleArea.Activated += (o, a) =>
            {
                entryCircleArea.Text = Geomet.Circle.Area(double.Parse(entryCircleArea.Text)).ToString();
            };




            Label labelSquare = new Label();
            labelSquare.Markup = "<b> Square: </b>";

            Label labelSquareCircumference = new Label("Circumference: ");
            Entry entrySquareCircumference = new Entry("");
            entrySquareCircumference.Activated += (o, a) =>
            {
                //Skal bruge 2 inputs
                //entrySquareCircumference.Text = Geomet.Square.Circumference(double.Parse(entrySquareCircumference.Text)).ToString();
            };

            Label labelSquareArea = new Label("Area: ");
            Entry entrySquareArea = new Entry("");
            entrySquareArea.Activated += (o, a) =>
            {
                //skal bruge 2 inputs
                //entrySquareCircumference.Text = Geomet.Square.Area(double.Parse(entrySquareCircumference.Text)).ToString();
            };

            Label labelSphere = new Label();
            labelSphere.Markup = "<b> Sphere: </b>";
            Label labelSphereVolume = new Label("Volume: ");
            Entry entrySphereVolume = new Entry("");
            entrySphereVolume.Activated += (o, a) =>
            {
                entrySphereVolume.Text = Geomet.Sphere.Volume(double.Parse(entrySphereVolume.Text)).ToString();
            };

            Label labelSphereSurfaceArea = new Label("Surface area");
            Entry entrySphereSurfaceArea = new Entry("");
            entrySphereSurfaceArea.Activated += (o, a) =>
            {
                entrySphereSurfaceArea.Text = Geomet.Sphere.SurfaceArea(double.Parse(entrySphereSurfaceArea.Text)).ToString();
            };

            Label labelCube = new Label();
            labelCube.Markup = "<b> Cube: </b>";
            Label labelCubeVolume = new Label("Volume: ");
            Entry entryCubeVolume = new Entry("");
            entryCubeVolume.Activated += (o, a) =>
            {
                //Skal bruge 3 inputs
                //entryCubeVolume.Text = Geomet.Cube.Volume(double.Parse(entryCubeVolume.Text)).ToString();
            };

            Label labelCubeSurfaceArea = new Label("Surface area");
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

            return table;
        }
    }
}
