using System;

namespace ImEx
{
	public static class Open
	{
		public static void OpenFile ()
		{
			try {
				Console.WriteLine ("\n\nInside tryblock!");
				Gtk.FileChooserDialog filechooser = new Gtk.FileChooserDialog ("Open file...", null, Gtk.FileChooserAction.Open);

			} catch (Exception ex) {
				Console.WriteLine (ex);
			}
		}
	}
}

