using System;
using Gtk;

namespace DesktopUI
{
	public class SaveToolButton : ToolButton
	{
		public SaveToolButton() : base(Stock.Save)
		{
			this.Clicked += delegate
			{
				SaveFile();
			};
		}

		void SaveFile()
		{
			throw new NotImplementedException();
		}
	}
}

