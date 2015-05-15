using System;
using Gtk;
using Gdk;
using System.IO;

namespace DesktopUI
{
    public class MovableTextViewToolButton : ToolButton
    {
        TextViewList textviews;
        static Image image = new Image();

        // Constructor for movabletextviewtoolbutton
        public MovableTextViewToolButton(ref TextViewList textviews)
            : base(image, "Movable Text View")
        {
            this.textviews = textviews;

            this.TooltipText = "Write text";

            SetIcon();

            this.Clicked += delegate
            {
                OnActivated();
            };
        }

        // When clicked, inserts a new textview at the buttom of the screen.
        void OnActivated()
        {
            textviews.InsertTextView("", false, -1);
        }

        // Sets the icon
        void SetIcon()
        {
            byte[] buffer = File.ReadAllBytes("../../../Ressources/Icons/Gnome-accessories-text-editor.svg");
            Pixbuf pixbuf = new Pixbuf(buffer);
            pixbuf = pixbuf.ScaleSimple(25, 25, InterpType.Bilinear);
            image.Pixbuf = pixbuf;
        }
    }
}

