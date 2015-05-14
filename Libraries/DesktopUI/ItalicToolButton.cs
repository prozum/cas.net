using System;
using Gtk;
using System.Text;
using System.IO;
using Gdk;

namespace DesktopUI
{
    public class ItalicToolButton : ToolButton
    {
        TextViewList textviews;

        static Image image = new Image();

        public ItalicToolButton(ref TextViewList textviews)
            : base(image, "Italic")
        {
            this.textviews = textviews;

            this.TooltipText = "Italic";

            SetIcon();

            Clicked += delegate
            {
                OnItalicClicked();
            };
        }

        void OnItalicClicked()
        {
            foreach (var item in textviews)
            {
                if (item.GetType() == typeof(MovableCasTextView))
                {
                    TextBuffer buffer = (item as MovableCasTextView).textview.Buffer;
                    TextIter startIter, endIter;
                    buffer.GetSelectionBounds(out startIter, out endIter);

                    byte[] byteTextView = buffer.Serialize(buffer, buffer.RegisterSerializeTagset(null), startIter, endIter);
                    string s = Encoding.UTF8.GetString(byteTextView);

                    if (s.Contains("<attr name=\"style\" type=\"PangoStyle\" value=\"PANGO_STYLE_ITALIC\" />"))
                    {
                        buffer.RemoveTag((item as MovableCasTextView).textview.italicTag, startIter, endIter);
                    }
                    else
                    {
                        buffer.ApplyTag((item as MovableCasTextView).textview.italicTag, startIter, endIter);
                    }

                }
            }
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
                        byte[] buffer = File.ReadAllBytes("..\\..\\..\\Ressources\\Icons\\Gnome-format-text-italic.png");
                        Pixbuf pixbuf = new Pixbuf(buffer);
                        pixbuf = pixbuf.ScaleSimple(25, 25, InterpType.Bilinear);
                        image.Pixbuf = pixbuf;

                        break;
                    }
                case PlatformID.Unix:
                case PlatformID.MacOSX:
                    {
                        byte[] buffer = File.ReadAllBytes("../../../Ressources/Icons/Gnome-format-text-italic.svg");
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

