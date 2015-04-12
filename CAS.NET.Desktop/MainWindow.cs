using Gtk;
using Ast;

public class MainWindow : Window
{
    Evaluator eval;
    Expression output, input;

    Grid grid;

    TextView textview;
    TextBuffer buffer;

    Entry entry;



    static void Main(string[] args)
    {
        Application.Init ();
        new MainWindow ();
        Application.Run();
    }
        

    public void EvaluateEntry()
    {
        input = Ast.Parser.Parse (eval, entry.Text);
        output = eval.Evaluation(entry.Text);

        buffer.Insert(buffer.StartIter, input.ToString() + " => " + output.ToString() + "\n");
    }

    public MainWindow() : base("MainWindow")
    {
        DeleteEvent += (o, a) => Application.Quit ();

        SetSizeRequest(500, 500);

        grid = new Grid ();
        Add (grid);

        eval = new Evaluator ();

        entry = new Entry ();
        entry.Activated += (o, a) => EvaluateEntry ();
        grid.Attach (entry, 0, 0, 1, 1);

        textview = new TextView();
        textview.Expand = true;
        textview.Editable = false;
        var sw = new ScrolledWindow ();
        sw.Add(textview);
        grid.Attach (sw, 0, 1, 1, 1);
        buffer = textview.Buffer;

        ShowAll ();
    }
}
