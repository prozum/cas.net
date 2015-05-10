using System;
using Gtk;
using System.Text;
using System.IO;
using Gdk;

namespace DesktopUI
{
    public class UnderlineToolButton : ToolButton
    {
        TextViewList textviews;

        static Image image = new Image();

        public UnderlineToolButton(ref TextViewList textviews)
            : base(image, "Underline")
        {
            this.textviews = textviews;

            SetIcon();

            this.TooltipText = "Underline";

            Clicked += delegate
            {
                OnUnderlineClicked();
            };
        }

        void OnUnderlineClicked()
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

                    Console.WriteLine(s);

                    if (s.Contains("<attr name=\"underline\" type=\"PangoUnderline\" value=\"PANGO_UNDERLINE_SINGLE\" />"))
                    {
                        buffer.RemoveTag((item as MovableCasTextView).textview.underlineTag, startIter, endIter);
                    }
                    else
                    {
                        buffer.ApplyTag((item as MovableCasTextView).textview.underlineTag, startIter, endIter);
                    }

                }
                else if (item.GetType() == typeof(MovableLockedCasTextView))
                {
                    TextBuffer buffer = (item as MovableLockedCasTextView).textview.Buffer;
                    TextIter startIter, endIter;
                    buffer.GetSelectionBounds(out startIter, out endIter);

                    byte[] byteTextView = buffer.Serialize(buffer, buffer.RegisterSerializeTagset(null), startIter, endIter);
                    string s = Encoding.UTF8.GetString(byteTextView);

                    if (s.Contains("<attr name=\"underline\" type=\"PangoUnderline\" value=\"PANGO_UNDERLINE_LOW\" />"))
                    {
                        buffer.RemoveTag((item as MovableLockedCasTextView).textview.underlineTag, startIter, endIter);
                    }
                    else
                    {
                        buffer.ApplyTag((item as MovableLockedCasTextView).textview.underlineTag, startIter, endIter);
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
                        byte[] buffer = File.ReadAllBytes("..\\..\\..\\Ressources\\Icons\\Gnome-format-text-underline.png");
                        Pixbuf pixbuf = new Pixbuf(buffer);
                        pixbuf = pixbuf.ScaleSimple(25, 25, InterpType.Bilinear);
                        image.Pixbuf = pixbuf;

                        break;
                    }
                case PlatformID.Unix:
                case PlatformID.MacOSX:
                    {
                        byte[] buffer = File.ReadAllBytes("../../../Ressources/Icons/Gnome-format-text-underline.svg");
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

