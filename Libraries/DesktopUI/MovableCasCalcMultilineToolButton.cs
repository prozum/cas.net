using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gtk;
using System.IO;
using Gdk;

namespace DesktopUI
{
    public class MovableCasCalcMultilineToolButton : ToolButton
    {
        TextViewList textviews;
        static Image image = new Image();

        public MovableCasCalcMultilineToolButton(ref TextViewList textviews)
            : base(image, "Multiline CalcView")
        {
            this.textviews = textviews;

            TooltipText = "Multiline CalcView";

            SetIcon();

            Clicked += delegate
            {
                OnActivated();
            };
        }

        private void OnActivated()
        {
            textviews.InsertCalcMultilineView(-1);
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
                        byte[] buffer = File.ReadAllBytes("..\\..\\..\\Ressources\\Icons\\Gnome-utilities-terminal.png");
                        Pixbuf pixbuf = new Pixbuf(buffer);
                        pixbuf = pixbuf.ScaleSimple(25, 25, InterpType.Bilinear);
                        image.Pixbuf = pixbuf;

                        break;
                    }
                case PlatformID.Unix:
                case PlatformID.MacOSX:
                    {
                        byte[] buffer = File.ReadAllBytes("../../../Ressources/Icons/Gnome-utilities-terminal.svg");
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
