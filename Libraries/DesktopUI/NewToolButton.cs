using System;
using Gtk;

namespace DesktopUI
{
	public class NewToolButton : ToolButton
	{
		public NewToolButton(TextViewList textviews) : base(Stock.New)
		{
			this.Clicked += delegate
			{
				textviews.Clear();
			};
		}
	}
}

