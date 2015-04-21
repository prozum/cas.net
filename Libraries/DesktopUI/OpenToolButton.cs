using System;
using Gtk;

namespace DesktopUI
{
	public class OpenToolButton : ToolButton
	{
		public OpenToolButton() : base(Stock.Open)
		{
			this.Clicked += delegate
			{
				OpenFile();
			};
		}

		void OpenFile()
		{
			throw new NotImplementedException();
		}
	}
}

