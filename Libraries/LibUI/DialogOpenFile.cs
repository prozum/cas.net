using System;
using Gtk;
using System.Collections.Generic;
using ImEx;

namespace LibUI
{
    public static class DialogOpenFile
    {
        public static void OpenFile()
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
                        System.IO.Stream myStream = null;
                        System.Windows.Forms.OpenFileDialog filechooser = new System.Windows.Forms.OpenFileDialog();

                        filechooser.InitialDirectory = "c:\\";
                        filechooser.Filter = "cas files (*.cas)|*.cas";
                        filechooser.FilterIndex = 2;
                        filechooser.RestoreDirectory = true;

                        if (filechooser.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                        {
                            GlobalVar.file = System.IO.File.ReadAllText(filechooser.FileName);
                        }

                        break;
                    }
                case PlatformID.Unix:
                case PlatformID.MacOSX:
                    {
                        FileChooserDialog filechooser = new FileChooserDialog("Open file...", null, FileChooserAction.Open, "Cancel", ResponseType.Cancel, "Open", ResponseType.Accept);

                        filechooser.Filter = new FileFilter();
                        filechooser.Filter.AddPattern("*.cas");

                        if (filechooser.Run() == (int)ResponseType.Accept)
                        {
                            GlobalVar.file = System.IO.File.ReadAllText(filechooser.Filename);
                        }

                        filechooser.Destroy();

                        break;
                    }
                default:
                    {
                        break;
                    }
            }

            UpdateHandling.ClearWindow();

            GlobalVar.listMeta.Clear();
            List<MetaType> mtlmt = new List<MetaType>();
            mtlmt = Import.DeserializeString<List<MetaType>>(GlobalVar.file);
            GlobalVar.listMeta = mtlmt;
            GlobalVar.listWidget.Clear();

            foreach (var item in GlobalVar.listMeta)
            {
                if (item.type == typeof(Entry))
                {
                    Entry entry = new Entry();
                    entry.Text = item.metastring;
                    GlobalVar.listWidget.Add(entry);
                }
                if (item.type == typeof(TextView))
                {
                    TextView textView = new TextView();
                    textView.Buffer.Text = item.metastring;
                    GlobalVar.listWidget.Add(textView);
                }
            }

            foreach (Widget item in GlobalVar.listWidget)
            {
                GlobalVar.globalGrid.Attach(Widgets.MovableWidget(item), 1, GlobalVar.gridNumber, 1, 1);
                GlobalVar.gridNumber++;
            }
        }
    }
}

