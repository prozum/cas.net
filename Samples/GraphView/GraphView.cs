using Gtk;
using Graph;

public class MainWindow : Window
{
    static void Main(string[] args)
    {
        Application.Init ();
        new MainWindow ();
        Application.Run();
    }

    public MainWindow() : base("GraphViewTest")
    {
        DeleteEvent += delegate {
            Application.Quit ();
        };

        // Setup ui
        Add (new GraphView (1.17, -5.2, 4.23));
        ShowAll ();
    }
}