using System;
using Gtk;

namespace FileOperation
{
    public class File
    {
        public File()
        {
        }

        public string Open(PlatformID pid, Window window)
        {
            string file = String.Empty;

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
                        }

                        return file;
                    }
                case PlatformID.Unix:
                case PlatformID.MacOSX:
                    {
                        Object[] parameters = { "Cancel", ResponseType.Cancel, "Open", ResponseType.Accept };
                        FileChooserDialog filechooser = new FileChooserDialog("Open file...", window, FileChooserAction.Open, parameters);

                        filechooser.Filter = new FileFilter();
                        filechooser.Filter.AddPattern("*.cas");

                        if (filechooser.Run() == (int)ResponseType.Accept)
                        {
                            file = System.IO.File.ReadAllText(filechooser.Filename);
                        }

                        filechooser.Destroy();

                        return file;
                    }
                default:
                    return null;
            }
        }

        public void Save(PlatformID pid, Window window, string file)
        {
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
                            System.IO.File.WriteAllText(filechooser.FileName, file);
                        }

                        break;
                    }
                case PlatformID.Unix:
                case PlatformID.MacOSX:
                    {
                        Object[] parameters = { "Cancel", ResponseType.Cancel, "Save", ResponseType.Accept };
                        FileChooserDialog filechooser = new FileChooserDialog("Save File...", window, FileChooserAction.Save, parameters);

                        filechooser.Filter = new FileFilter();
                        filechooser.Filter.AddPattern("*.cas");

                        if (filechooser.Run() == (int)ResponseType.Accept)
                        {
                            if (filechooser.Filename.ToLower().EndsWith(".cas"))
                            {
                                System.IO.File.WriteAllText(filechooser.Filename, file);
                            }
                            else
                            {
                                System.IO.File.WriteAllText(filechooser.Filename + ".cas", file);
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

