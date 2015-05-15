using System;
using Gtk;
using Gdk;
using System.IO;

namespace DesktopUI
{
    // Inserts a cascalcview when clicked
    public class MovableCalcViewToolButton : ToolButton
    {
        TextViewList textviews;
        static Image image = new Image();

        // Constructor for calcview button
        public MovableCalcViewToolButton(ref TextViewList textviews)
            : base(image, "Movable Calc View")
        {
            this.textviews = textviews;

            this.TooltipText = "Casculator";

            SetIcon();

            this.Clicked += delegate
            {
                OnActivated();
            };
        }

        // Inserts a calcview at the buttom of the workspace
        void OnActivated()
        {
            textviews.InsertCalcView(-1);
        }

        // Sets the icon for the widget
        void SetIcon()
        {
            byte[] buffer = File.ReadAllBytes("../../../Ressources/Icons/Gnome-accessories-calculator.svg");
            Pixbuf pixbuf = new Pixbuf(buffer);
            pixbuf = pixbuf.ScaleSimple(25, 25, InterpType.Bilinear);
            image.Pixbuf = pixbuf;
        }
    }
}

