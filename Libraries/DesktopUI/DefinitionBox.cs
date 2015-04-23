using System;
using Ast;
using Gtk;

namespace DesktopUI
{
	public class DefinitionBox : Label
	{
		TextViewList textviews;
		string definitions;

		public DefinitionBox (TextViewList textviews) : base()
		{
			this.textviews = textviews;
			definitions = String.Empty;
			Show ();
		}

		public void Update()
		{
			foreach (Widget widget in textviews.castextviews)
			{
				if (widget.GetType() == typeof(CasCalcView))
				{
					definitions += (widget as CasCalcView).input + "\n";
				}
			}

			Text = definitions;
			Show ();
		}
	}
}