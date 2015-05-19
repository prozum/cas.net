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

            GeometTable gt = new GeometTable();

            Table table = gt.CreateGeometTable();

            
           
            Button buttonOk = new Button("Ok");
            Button buttonCancel = new Button("Cancel");

            
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
