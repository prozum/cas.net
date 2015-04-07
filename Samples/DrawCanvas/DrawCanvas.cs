using Gtk;
using Canvas;

public class MainWindow : Window
{
    public DrawCanvas canvas;

    static void Main(string[] args)
    {
        Application.Init ();
        new MainWindow();
        Application.Run();
    }

    public MainWindow() : base("Canvas Test")
    {
        canvas = new DrawCanvas();
        Add(canvas);

        /* Calls RedrawCanvas() every 15 ms */
        GLib.Timeout.Add (15, new GLib.TimeoutHandler (RedrawCanvas));

        /* Show all widgets (recursively) */
        ShowAll();
    }

    /* Redraws canvas and subwidgets */
    public bool RedrawCanvas()
    {
        canvas.QueueDraw();
        return true;
    }
}
