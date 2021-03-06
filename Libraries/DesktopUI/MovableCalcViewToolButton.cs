﻿using System;
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

