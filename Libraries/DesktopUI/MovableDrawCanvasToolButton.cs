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
            OperatingSystem os = Environment.OSVersion;
            PlatformID pid = os.Platform;

            switch (pid)
            {
                case PlatformID.Win32S:
                case PlatformID.Win32Windows:
                case PlatformID.WinCE:
                case PlatformID.Win32NT: // <- if one, this is the one we really need
                    {
                        byte[] buffer = File.ReadAllBytes("..\\..\\..\\Ressources\\Icons\\Gnome-preferences-desktop-wallpaper.png");
                        Pixbuf pixbuf = new Pixbuf(buffer);
                        pixbuf = pixbuf.ScaleSimple(25, 25, InterpType.Bilinear);
                        image.Pixbuf = pixbuf;

                        break;
                    }
                case PlatformID.Unix:
                case PlatformID.MacOSX:
                    {
                        byte[] buffer = File.ReadAllBytes("../../../Ressources/Icons/Gnome-preferences-desktop-wallpaper.svg");
                        Pixbuf pixbuf = new Pixbuf(buffer);
                        pixbuf = pixbuf.ScaleSimple(25, 25, InterpType.Bilinear);
                        image.Pixbuf = pixbuf;

                        break;
                    }
                default:
                    {
                        break;
                    }
            }
        }
    }
}

