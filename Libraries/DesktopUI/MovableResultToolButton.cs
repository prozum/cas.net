using System;
using Gtk;
using System.IO;
using Gdk;

namespace DesktopUI
{
    // Inserts the result widget
    public class MovableResultToolButton : ToolButton
    {
        TextViewList textviews;
        static Image image = new Image();

        public MovableResultToolButton(ref TextViewList textviews)
            : base(image, "Result")
        {
            this.textviews = textviews;

            this.TooltipText = "Set fixed result";

            SetIcon();

            this.Clicked += delegate
            {
                OnActivated();
            };
        }

        // Inserts the resultview when clicked
        void OnActivated()
        {
            textviews.InsertResult("", "");
        }

        // Sets the icon for the widget
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
                        byte[] buffer = File.ReadAllBytes("..\\..\\..\\Ressources\\Icons\\Gnome-address-book-new.png");
                        Pixbuf pixbuf = new Pixbuf(buffer);
                        pixbuf = pixbuf.ScaleSimple(25, 25, InterpType.Bilinear);
                        image.Pixbuf = pixbuf;

                        break;
                    }
                case PlatformID.Unix:
                case PlatformID.MacOSX:
                    {
                        byte[] buffer = File.ReadAllBytes("../../../Ressources/Icons/Gnome-address-book-new.svg");
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

