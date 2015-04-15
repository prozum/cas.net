using Gtk;
using Graph;
using Ast;

public class MainWindow : Window
{
    GraphView graphView;

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
        Evaluator eval = new Evaluator();
        eval.Evaluation("x:=range(100)");

        var plot = eval.Evaluation("plot(x*2,x)");
        graphView = new GraphView((plot as PlotData));
        Add (graphView);
        ShowAll ();
    }
}