using System;
using Ast;
using Gtk;

namespace DesktopUI
{
	public class DefinitionBox : TextView
	{
		TextViewList textviews;

		public DefinitionBox (TextViewList textviews) : base()
		{
			this.textviews = textviews;
		}
	}
}

