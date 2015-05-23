using System;
using Gtk;
using ImEx;
using System.Collections.Generic;
using Gdk;
using System.IO;

namespace DesktopUI
{
    // Toolbutton for saving the current workspace to a file
    public class SaveToolButton : ToolButton
    {
        static Image image = new Image();
        TextViewList textviews;

        // Constructor for savetoolbutton
        public SaveToolButton(TextViewList textviews)
            : base(image, "Save")
        {
            SetIcon();

            this.TooltipText = "Save .CAS file";

            this.textviews = textviews;

            this.Clicked += delegate
            {
                SaveFile();
            };
        }

        // Run when the button is pressed. Serializes the file, and saves it to the selected file
        public void SaveFile()
        {
            OperatingSystem os = Environment.OSVersion;
            PlatformID pid = os.Platform;

            List<MetaType> metaTypeList = new List<MetaType>();

            foreach (Widget w in textviews)
            {
                if (w is MovableCasCalcView)
                {
                    MetaType metaType = new MetaType();
                    MovableCasCalcView calcView = (MovableCasCalcView)w;
                    metaType.type = typeof(MovableCasCalcView);
                    metaType.metastring = calcView.calcview.input.Text;
                    metaType.locked = calcView.textview.locked;
                    metaTypeList.Add(metaType);
                }
                else if(w is MovableCasCalcMulitlineView)
                {
                    MetaType metaType = new MetaType();
                    MovableCasCalcMulitlineView calcview = (MovableCasCalcMulitlineView)w;
                    metaType.type = typeof(MovableCasCalcMulitlineView);
                    metaType.metastring = calcview.calcview.SerializeCasTextView();
                    metaType.locked = calcview.textview.locked;
                    metaTypeList.Add(metaType);
                }
                else if (w.GetType() == typeof(MovableCasResult))
                {
                    MetaType metaType = new MetaType();
                    MovableCasResult result = (MovableCasResult)w;
                    metaType.type = typeof(MovableCasResult);
                    metaType.metastring = Export.Serialize(result.casresult.facitContainer);
                    metaType.locked = result.casresult.correct;
                    metaTypeList.Add(metaType);
                }
                else if (w.GetType() == typeof(MovableCasTextView))
                {
                    MetaType metaType = new MetaType();
                    MovableCasTextView textView = (MovableCasTextView)w;
                    metaType.type = typeof(MovableCasTextView);
                    metaType.metastring = textView.textview.SerializeCasTextView();
                    metaType.locked = textView.textview.locked;
                    metaTypeList.Add(metaType);
                }
            }

            string s = ImEx.Export.Serialize(metaTypeList);
                
            switch (pid)
            {
                case PlatformID.Win32S:
                case PlatformID.Win32Windows:
                case PlatformID.WinCE:
                case PlatformID.Win32NT: // <- if one, this is the one we really need
                    {
                        var filechooser = new System.Windows.Forms.SaveFileDialog();

                        filechooser.InitialDirectory = "c:\\";
                        filechooser.Filter = "cas files (*.cas)|*.cas";
                        filechooser.FilterIndex = 2;
                        filechooser.RestoreDirectory = true;

                        if (filechooser.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                        {
                            System.IO.File.WriteAllText(filechooser.FileName, s);
                        }

                        break;
                    }
                case PlatformID.Unix:
                case PlatformID.MacOSX:
                    {
                        Object[] parameters = { "Cancel", ResponseType.Cancel, "Save", ResponseType.Accept };
                        FileChooserDialog filechooser = new FileChooserDialog("Save File...", null, FileChooserAction.Save, parameters);

                        filechooser.Filter = new FileFilter();
                        filechooser.Filter.AddPattern("*.cas");

                        if (filechooser.Run() == (int)ResponseType.Accept)
                        {
                            if (filechooser.Filename.ToLower().EndsWith(".cas"))
                            {
                                System.IO.File.WriteAllText(filechooser.Filename, s);
                            }
                            else
                            {
                                System.IO.File.WriteAllText(filechooser.Filename + ".cas", s);
                            }
                        }

                        filechooser.Destroy();

                        break;
                    }
                default:
                    {
                        break;
                    }
            
            }
        }

        // Sets the icon for the file
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
                        byte[] buffer = File.ReadAllBytes("..\\..\\..\\Ressources\\Icons\\Gnome-media-floppy.png");
                        Pixbuf pixbuf = new Pixbuf(buffer);
                        pixbuf = pixbuf.ScaleSimple(25, 25, InterpType.Bilinear);
                        image.Pixbuf = pixbuf;

                        break;
                    }
                case PlatformID.Unix:
                case PlatformID.MacOSX:
                    {
                        byte[] buffer = File.ReadAllBytes("../../../Ressources/Icons/Gnome-media-floppy.svg");
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

