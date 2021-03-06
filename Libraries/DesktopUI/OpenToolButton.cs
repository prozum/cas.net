﻿using System;
using Gtk;
using System.Collections.Generic;
using ImEx;
using Ast;
using Gdk;
using System.IO;

namespace DesktopUI
{
    // Opens a saved file on the computer
    public class OpenToolButton : ToolButton
    {
        static Image image = new Image();
        TextViewList textviews;
        User user;

        public OpenToolButton(TextViewList textviews, ref User user)
            : base(image, "open")
        {

            SetIcon();
            this.TooltipText = "Open .CAS file";
            this.textviews = textviews;
            this.user = user;

            this.Clicked += delegate
            {
                OpenFile();
            };
        }

        // Opens the file when clicked
        public void OpenFile()
        {
            OperatingSystem os = Environment.OSVersion;
            PlatformID pid = os.Platform;

            string file = String.Empty;

            bool hasOpenedAnything = false;

            switch (pid)
            {
                case PlatformID.Win32S:
                case PlatformID.Win32Windows:
                case PlatformID.WinCE:
                case PlatformID.Win32NT: // <- if one, this is the one we really need
                    {
                        var filechooser = new System.Windows.Forms.OpenFileDialog();

                        filechooser.InitialDirectory = "c:\\";
                        filechooser.Filter = "cas files (*.cas)|*.cas";
                        filechooser.FilterIndex = 2;
                        filechooser.RestoreDirectory = true;

                        if (filechooser.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                        {
                            file = System.IO.File.ReadAllText(filechooser.FileName);
                            hasOpenedAnything = true;
                        }

                        break;
                    }
                case PlatformID.Unix:
                case PlatformID.MacOSX:
                    {
                        Object[] parameters = { "Cancel", ResponseType.Cancel, "Open", ResponseType.Accept };
                        FileChooserDialog filechooser = new FileChooserDialog("Open file...", null, FileChooserAction.Open, parameters);

                        filechooser.Filter = new FileFilter();
                        filechooser.Filter.AddPattern("*.cas");

                        if (filechooser.Run() == (int)ResponseType.Accept)
                        {
                            file = System.IO.File.ReadAllText(filechooser.Filename);
                            hasOpenedAnything = true;
                        }

                        filechooser.Destroy();

                        break;
                    }
                default:
                    break;
            }

            // If anything has been opened, deserializes the content, and rebuilds the workspace with the opened widgets
            if (hasOpenedAnything == true)
            {
                List<MetaType> metaTypeList = new List<MetaType>();

                metaTypeList = ImEx.Import.DeserializeString<List<MetaType>>(file);

                textviews.castextviews.Clear();

                foreach (var item in metaTypeList)
                {
                    if (item.type == typeof(MovableCasTextView) && user.privilege == 1)
                    {
                        textviews.InsertTextView(item.metastring, item.locked, -1);
                    }
                    else if (item.type == typeof(MovableCasCalcView))
                    {
                        textviews.InsertCalcView(item.metastring, item.locked);
                    }
                    else if (item.type == typeof(MovableCasResult))
                    {
                        CasResult.FacitContainer container = new CasResult.FacitContainer();
                        container = Import.DeserializeString<CasResult.FacitContainer>(item.metastring);

                        textviews.InsertResult(container.answer, container.facit);
                    }
                    else if(item.type == typeof(MovableCasCalcMulitlineView))
                    {
                        textviews.InsertCalcMultilineView(item.metastring, item.locked);
                    }
                    else if (item.type == typeof(MovableCasTextView))
                    {
                        textviews.InsertTextView(item.metastring, item.locked, -1);
                    }
                }

                textviews.castextviews.Reverse();

                textviews.Clear();
                textviews.Redraw();
                textviews.Reevaluate();
                textviews.ShowAll();

            }
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
                        byte[] buffer = File.ReadAllBytes("..\\..\\..\\Ressources\\Icons\\Gnome-document-open.png");
                        Pixbuf pixbuf = new Pixbuf(buffer);
                        pixbuf = pixbuf.ScaleSimple(25, 25, InterpType.Bilinear);
                        image.Pixbuf = pixbuf;

                        break;
                    }
                case PlatformID.Unix:
                case PlatformID.MacOSX:
                    {
                        byte[] buffer = File.ReadAllBytes("../../../Ressources/Icons/Gnome-document-open.svg");
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

