using System;
using System.IO;
using Gtk;
using Gdk;

namespace DesktopUI
{
    // This button is used to clear the screen, allowing the user to start on a new project.
    public class NewToolButton : ToolButton
    {
        static Image image = new Image();

        // Constructor for newtoolbutton
        public NewToolButton(TextViewList textviews)
            : base(image, "New")
        {
            SetIcon();

            this.TooltipText = "New .CAS file";

            this.Clicked += delegate
            {
                textviews.castextviews.Clear();
                textviews.Clear();
                textviews.Redraw();
                textviews.ShowAll();
            };
        }

        // Sets the icon
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
                        byte[] buffer = File.ReadAllBytes("..\\..\\..\\Ressources\\Icons\\Gnome-document-new.png");
                        Pixbuf pixbuf = new Pixbuf(buffer);
                        pixbuf = pixbuf.ScaleSimple(25, 25, InterpType.Bilinear);
                        image.Pixbuf = pixbuf;

                        break;
                    }
                case PlatformID.Unix:
                case PlatformID.MacOSX:
                    {
                        byte[] buffer = File.ReadAllBytes("../../../Ressources/Icons/Gnome-document-new.svg");
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

