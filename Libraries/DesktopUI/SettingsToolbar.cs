using System;
using Gtk;
using System.Collections.Generic;
using ImEx;

namespace DesktopUI
{
	public class SettingsToolbar : Toolbar
	{
		public SettingsToolbar (ref Grid grid, ref string file)
		{
			ToolButton toolButtonNew = new ToolButton(Stock.New);
			Insert (toolButtonNew, 0);
			toolButtonNew.Clicked += delegate
			{
				//ClearWindow(grid);
			};

			ToolButton toolButtonOpen = new ToolButton(Stock.Open);
			Insert (toolButtonOpen, 1);
			toolButtonOpen.Clicked += delegate
			{
				OpenFile(ref file);
			};

			ToolButton toolButtonSave = new ToolButton(Stock.Save);
			Insert(toolButtonSave, 2);
			toolButtonSave.Clicked += delegate
			{
				//SaveFile(ref file);
			};
		}

		void OpenFile(ref string file)
		{
			OperatingSystem os = Environment.OSVersion;
			PlatformID pid = os.Platform;

			switch (pid) {
				case PlatformID.Win32S:
				case PlatformID.Win32Windows:
				case PlatformID.WinCE:
				case PlatformID.Win32NT: // <- if one, this is the one we really need
				{
					var filechooser = new System.Windows.Forms.OpenFileDialog ();

					filechooser.InitialDirectory = "c:\\";
					filechooser.Filter = "cas files (*.cas)|*.cas";
					filechooser.FilterIndex = 2;
					filechooser.RestoreDirectory = true;

					if (filechooser.ShowDialog () == System.Windows.Forms.DialogResult.OK) {
						file = System.IO.File.ReadAllText (filechooser.FileName);
					}

					break;
				}
				case PlatformID.Unix:
				case PlatformID.MacOSX:
				{
					var filechooser = new FileChooserDialog ("Open file...", this, FileChooserAction.Open, "Cancel", ResponseType.Cancel, "Open", ResponseType.Accept);

					filechooser.Filter = new FileFilter ();
					filechooser.Filter.AddPattern ("*.cas");

					if (filechooser.Run () == (int)ResponseType.Accept) {
						file = System.IO.File.ReadAllText (filechooser.Filename);
					}

					filechooser.Destroy ();

					break;
				}
				default:
				{
					break;
				}
			}
		}

		void SaveFile(ref string file, ref List<MetaType> widgets)
		{
			OperatingSystem os = Environment.OSVersion;
			PlatformID pid = os.Platform;

			Console.WriteLine(mt.Count);

			UpdateWorkspace();

			Console.WriteLine(mt.Count);

			casFile = ImEx.Export.Serialize(mt);
			Console.WriteLine("CAS FILE:::\n" + casFile);

			switch (pid)
			{

			case PlatformID.Win32S:
			case PlatformID.Win32Windows:
			case PlatformID.WinCE:
			case PlatformID.Win32NT: // <- if one, this is the one we really need
				{
					System.Windows.Forms.SaveFileDialog filechooser = new System.Windows.Forms.SaveFileDialog();

					filechooser.InitialDirectory = "c:\\";
					filechooser.Filter = "cas files (*.cas)|*.cas";
					filechooser.FilterIndex = 2;
					filechooser.RestoreDirectory = true;

					if (filechooser.ShowDialog() == System.Windows.Forms.DialogResult.OK)
					{
						System.IO.File.WriteAllText(filechooser.FileName, casFile);
					}

					break;
				}
			case PlatformID.Unix:
			case PlatformID.MacOSX:
				{
					FileChooserDialog filechooser = new FileChooserDialog("Save File...", this, FileChooserAction.Save, "Cancel", ResponseType.Cancel, "Save", ResponseType.Accept);

					filechooser.Filter = new FileFilter();
					filechooser.Filter.AddPattern("*.cas");

					if (filechooser.Run() == (int)ResponseType.Accept)
					{
						if (filechooser.Name.ToLower().EndsWith(".cas"))
						{
							System.IO.File.WriteAllText(filechooser.Filename, casFile);
						}
						else
						{
							System.IO.File.WriteAllText(filechooser.Filename + ".cas", casFile);
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

