using Gtk;

public class MainWindow : Window
{
	static void Main(string[] args)
	{
		Application.Init ();
		new MainWindow ();
		Application.Run();
	}

	public MainWindow() : base("MainWindow")
	{
		DeleteEvent += OnDeleteEvent;

		// Setup ui
		var textview = new TextView();
		Add (textview);

		// Setup tag
		var tag = new TextTag ("helloworld-tag");
		tag.Scale = Pango.Scale.XXLarge;
		tag.Style = Pango.Style.Italic;
		tag.Underline = Pango.Underline.Double;
		tag.Foreground = "blue";
		tag.Background = "pink";
		tag.Justification = Justification.Center;
		var buffer = textview.Buffer;
		buffer.TagTable.Add (tag);

		var insertIter = buffer.StartIter;
		buffer.InsertWithTagsByName (ref insertIter, "Hello CAS.NET!\n", "helloworld-tag");
		ShowAll ();
	}

	protected void OnDeleteEvent (object sender, DeleteEventArgs a)
	{
		Application.Quit ();
	}
}