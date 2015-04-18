using Gtk;
using Ast;

public class MainWindow : Window
{
    Evaluator eval;
    EvalData output;

    Grid grid;

    ListStore defStore;
    TreeView defTree;

    TextView textView;
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
        output = eval.Evaluation(entry.Text);

        TextIter insertIter = buffer.StartIter;

        if (output is MsgData)
        {
            switch ((output as MsgData).type)
            {
                case MsgType.Print:
                    buffer.Insert(ref insertIter, (output as MsgData).msg + "\n");
                    break;
                case MsgType.Error:
                    buffer.InsertWithTagsByName(ref insertIter, (output as MsgData).msg + "\n", "error");
                    break;
                case MsgType.Info:
                    buffer.InsertWithTagsByName(ref insertIter, (output as MsgData).msg + "\n", "info");
                    break;
            }
        }
        else if (output is PlotData)
        {

        }
    }

    public void CreateDefTree()
    {
        CellRenderer renderer;

        defStore = new ListStore(typeof(string), typeof(string));
        defTree = new TreeView(defStore);
        defTree.Expand = true;

        renderer = new CellRendererText();
        defTree.AppendColumn("Name", renderer, "text",0);
        renderer = new CellRendererText();
        defTree.AppendColumn("Value", renderer, "text",1);
    }

    public void UpdateDefinitions()
    {
        defStore.Clear();

        foreach (var def  in eval.variableDefinitions)
        {
            defStore.AppendValues(def.Key, def.Value.ToString());
        }

        foreach (var def  in eval.functionDefinitions)
        {
            defStore.AppendValues(def.Key, def.Value.ToString());
        }
    }

    public MainWindow() : base("MainWindow")
    {
        DeleteEvent += (o, a) => Application.Quit ();

        eval = new Evaluator ();

        SetSizeRequest(500, 500);

        grid = new Grid ();
        Add (grid);

        entry = new Entry ();
        entry.Activated += (o, a) => EvaluateEntry ();
        entry.Activated += (o, a) => UpdateDefinitions();
        grid.Attach (entry, 0, 0, 1, 1);

        textView = new TextView();
        textView.Expand = true;
        textView.Editable = false;
        var sw = new ScrolledWindow ();
        sw.Add(textView);
        grid.Attach (sw, 0, 1, 1, 1);
        buffer = textView.Buffer;

        var infoTag = new TextTag ("info");
        infoTag.Foreground = "blue";
        var errorTag = new TextTag ("error");
        errorTag.Foreground = "red";
        buffer.TagTable.Add(infoTag);
        buffer.TagTable.Add(errorTag);


        CreateDefTree();
        grid.Attach(defTree, 1, 0, 1, 2);



        ShowAll ();
    }
}
