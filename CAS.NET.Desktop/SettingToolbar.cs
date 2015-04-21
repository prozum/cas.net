using System;
using Gtk;

namespace CAS.NET.Desktop
{
    public class SettingsToolbar : Toolbar
    {
        public SettingsToolbar(string file)
        {
            ToolButton toolButtonNew = new ToolButton(Stock.New);
            this.Insert(toolButtonNew, 0);
            toolButtonNew.Clicked += delegate
            {
                ClearWindow();
            };

            ToolButton toolButtonOpen = new ToolButton(Stock.Open);
            this.Insert(toolButtonOpen, 1);
            toolButtonOpen.Clicked += delegate
            {
                this.OpenFile();
            };

            ToolButton toolButtonSave = new ToolButton(Stock.Save);
            this.Insert(toolButtonSave, 2);
            toolButtonSave.Clicked += delegate
            {
                SaveFile();
            };

            SeparatorToolItem toolSeparater1 = new SeparatorToolItem();
            this.Insert(toolSeparater1, 3);

            ToolButton toolButtonBold = new ToolButton(Stock.Bold);
            this.Insert(toolButtonBold, 4);

            ToolButton toolButtonItalic = new ToolButton(Stock.Italic);
            this.Insert(toolButtonItalic, 5);

            ToolButton toolButtonUnderline = new ToolButton(Stock.Underline);
            this.Insert(toolButtonUnderline, 6);
        }

        void OpenFile()
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
                        System.Windows.Forms.OpenFileDialog filechooser = new System.Windows.Forms.OpenFileDialog();

                        filechooser.InitialDirectory = "c:\\";
                        filechooser.Filter = "cas files (*.cas)|*.cas";
                        filechooser.FilterIndex = 2;
                        filechooser.RestoreDirectory = true;

                        if (filechooser.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                        {
                            casFile = System.IO.File.ReadAllText(filechooser.FileName);
                        }

                        break;
                    }
                case PlatformID.Unix:
                case PlatformID.MacOSX:
                    {
                        FileChooserDialog filechooser = new FileChooserDialog("Open file...", this, FileChooserAction.Open, "Cancel", ResponseType.Cancel, "Open", ResponseType.Accept);

                        filechooser.Filter = new FileFilter();
                        filechooser.Filter.AddPattern("*.cas");

                        if (filechooser.Run() == (int)ResponseType.Accept)
                        {
                            casFile = System.IO.File.ReadAllText(filechooser.Filename);
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