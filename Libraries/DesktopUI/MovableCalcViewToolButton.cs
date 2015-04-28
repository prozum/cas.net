using System;
using Gtk;
using Gdk;
using System.IO;

namespace DesktopUI
{
    public class MovableCalcViewToolButton : ToolButton
    {
        TextViewList textviews;
        static Image image = new Image();

        public MovableCalcViewToolButton(ref TextViewList textviews)
            : base(image, "Movable Calc View")
        {
            this.textviews = textviews;

            SetIcon();

            this.Clicked += delegate
            {
                OnActivated();
            };
        }

        void OnActivated()
        {
            textviews.InsertCalcView();
        }

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
                        byte[] buffer = File.ReadAllBytes("..\\..\\..\\Ressources\\Icons\\Gnome-accessories-calculator.png");
                        Pixbuf pixbuf = new Pixbuf(buffer);
                        pixbuf = pixbuf.ScaleSimple(25, 25, InterpType.Bilinear);
                        image.Pixbuf = pixbuf;

                        break;
                    }
                case PlatformID.Unix:
                case PlatformID.MacOSX:
                    {
                        byte[] buffer = File.ReadAllBytes("../../../Ressources/Icons/Gnome-accessories-calculator.svg");
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

