using System;
using Gtk;
using System.Collections.Generic;
using ImEx;
using System.Text;

namespace DesktopUI
{
	public class SettingsToolbar : Toolbar
	{
		string file;
		Grid grid;
		int GridNumber;
		List<MetaType> meta;
		List<Widget> widgets;
		Window window;

		public SettingsToolbar (ref string file, ref Grid grid, ref int GridNumber, ref List<MetaType> meta, ref List<Widget> widgets, Window window)
		{
			this.file = file;
			this.grid = grid;
			this.GridNumber = GridNumber;
			this.meta = meta;
			this.widgets = widgets;
			this.window = window;

			ToolButton toolButtonNew = new ToolButton(Stock.New);
			Insert (toolButtonNew, 0);
			toolButtonNew.Clicked += delegate
			{
				ClearWindow();
			};

			ToolButton toolButtonOpen = new ToolButton(Stock.Open);
			Insert (toolButtonOpen, 1);
			toolButtonOpen.Clicked += delegate
			{
				OpenFile();
			};

			ToolButton toolButtonSave = new ToolButton(Stock.Save);
			Insert(toolButtonSave, 2);
			toolButtonSave.Clicked += delegate
			{
				SaveFile();
			};

			toolButtonNew.Show ();
			toolButtonOpen.Show ();
			toolButtonSave.Show ();
		}

		void OpenFile()
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
					Object[] parameters = { "Cancel", ResponseType.Cancel, "Open", ResponseType.Accept };
					FileChooserDialog filechooser = new FileChooserDialog ("Open file...", window, FileChooserAction.Open, parameters);

					filechooser.Filter = new FileFilter ();
					filechooser.Filter.AddPattern ("*.cas");

					if (filechooser.Run () == (int)ResponseType.Accept) {
						file = System.IO.File.ReadAllText (filechooser.Filename);
					}

					filechooser.Destroy ();

					break;
				}
			}

			ClearWindow ();

			meta.Clear ();
			List<MetaType> mtlmt = new List<MetaType> ();
			mtlmt = Import.DeserializeString<List<MetaType>> (file);
			meta = mtlmt;
			widgets.Clear ();

			RecreateListWidget ();

			foreach (Widget item in widgets)
			{
				grid.Attach (((CasTextView)item).GetMovableWidget(), 1, GridNumber, 1, 1);
				GridNumber++;
			}

			ShowAll ();
		}

		void SaveFile()
		{
			OperatingSystem os = Environment.OSVersion;
			PlatformID pid = os.Platform;

			Console.WriteLine(meta.Count);

			UpdateWorkspace();

			Console.WriteLine(meta.Count);

			file = Export.Serialize(meta);
			Console.WriteLine("CAS FILE:::\n" + file);

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
					if (filechooser.Name.ToLower().EndsWith(".cas"))
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

		void UpdateWorkspace()
		{
			meta.Clear();

			foreach (Widget w in widgets)
			{
				if (w.GetType() == typeof(Entry))
				{
					Entry en = (Entry)w;
					MetaType mtlmt = new MetaType();
					mtlmt.type = en.GetType();
					mtlmt.metastring = en.Text;

					meta.Add(mtlmt);
				}
				if (w.GetType() == typeof(TextView))
				{

					TextView tv = (TextView)w;
					MetaType mtlmt = new MetaType();
					TextBuffer buffer = tv.Buffer;
					TextIter startIter, endIter;
					buffer.GetBounds(out startIter, out endIter);
					mtlmt.type = tv.GetType();
					byte[] byteTextView = buffer.Serialize(buffer, buffer.RegisterSerializeTagset(null), startIter, endIter);
					mtlmt.metastring = Encoding.UTF8.GetString(byteTextView);

					meta.Add(mtlmt);
				}
				if (w.GetType() == typeof(CasTextView))
				{
					CasTextView ctv = (CasTextView)w;
					MetaType mtlmt = new MetaType();
					mtlmt.type = ctv.GetType();
					mtlmt.metastring = ctv.SerializeCasTextView();

					meta.Add(mtlmt);
				}
			}
		}

		void ClearWindow()
		{
			widgets.Clear();

			foreach (Widget item in grid)
			{
				grid.Remove(item);
			}

			GridNumber = 1;
		}

		void RecreateListWidget()
		{
			foreach (var item in meta)
			{
				if (item.type == typeof(Entry))
				{
					Entry entry = new Entry();
					entry.Text = item.metastring;

					widgets.Add(entry);
				}
				if (item.type == typeof(TextView))
				{
					Byte[] byteTextView = Encoding.UTF8.GetBytes(item.metastring);
					TextBuffer buffer = new TextView().Buffer;
					TextIter textIter = buffer.StartIter;

					buffer.Deserialize(buffer, buffer.RegisterDeserializeTagset(null), ref textIter, byteTextView, (ulong)byteTextView.Length);

					TextView textView = new TextView();
					textView.Buffer = buffer;

					widgets.Add(textView);
				}
				if (item.type == typeof(CasTextView))
				{
					CasTextView ctv = new CasTextView("", true, widgets);
					Console.WriteLine("Meta: " + item.metastring + " :End Meta");
					ctv.DeserializeCasTextView(item.metastring);

					widgets.Add(ctv);
				}
			}
		}
	}
}

