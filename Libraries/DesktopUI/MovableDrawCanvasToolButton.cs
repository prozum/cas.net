using System;
using Gtk;
using Gdk;
using System.IO;

namespace DesktopUI
{
    public class MovableDrawCanvasToolButton : ToolButton
    {
        TextViewList textviews;
        static Image image = new Image();

        // Constructor for movabledrawcanvastoolbutton
        public MovableDrawCanvasToolButton(ref TextViewList textviews)
            : base(image, "Movable Draw Canvas")
        {
            this.textviews = textviews;

            this.TooltipText = "Draw";

            SetIcon();

            this.Clicked += delegate
            {
                OnActivated();
            };
        }

        // Method is run when the button is clicked, inserting a new drawcanvas at the button of the workspace
        void OnActivated()
        {
            textviews.InsertDrawCanvas(-1);
        }

        // Sets icon for button
        void SetIcon()
        {
            byte[] buffer = File.ReadAllBytes("../../../Ressources/Icons/Gnome-preferences-desktop-wallpaper.svg");
            Pixbuf pixbuf = new Pixbuf(buffer);
            pixbuf = pixbuf.ScaleSimple(25, 25, InterpType.Bilinear);
            image.Pixbuf = pixbuf;
        }
    }
}

