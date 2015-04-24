using System;
using Gtk;
using ImEx;
using System.Collections.Generic;

namespace DesktopUI
{
    public class SaveToolButton : ToolButton
    {
        TextViewList textviews;

        public SaveToolButton(TextViewList textviews)
            : base(Stock.Save)
        {
            this.textviews = textviews;

            this.Clicked += delegate
            {
                SaveFile();
            };
        }

        public void SaveFile()
        {
            OperatingSystem os = Environment.OSVersion;
            PlatformID pid = os.Platform;

            List<MetaType> metaTypeList = new List<MetaType>();

            foreach (Widget w in textviews)
            {
                if (w.GetType() == typeof(MovableCasCalcView))
                {
                    MetaType metaType = new MetaType();
                    MovableCasCalcView calcView = (MovableCasCalcView)w;
                    metaType.type = typeof(MovableCasCalcView);
                    metaType.metastring = calcView.calcview.input.Text;
                    metaTypeList.Add(metaType);
                }
                else if (w.GetType() == typeof(MovableCasTextView))
                {
                    MetaType metaType = new MetaType();
                    MovableCasTextView textView = (MovableCasTextView)w;
                    metaType.type = typeof(MovableCasTextView);
                    metaType.metastring = textView.textview.SerializeCasTextView();
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
                            System.IO.File.WriteAllText(filechooser.FileName, "file");
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

    }
}

