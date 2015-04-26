using Gtk;
using Ast;
using System.Collections.Generic;

public class MainWindow : Window
{
    Evaluator eval;
    EvalData output;
    List<EvalData> DataList;

    Grid grid;

    ListStore defStore;
    TreeView defTree;

    TextView textView;
    TextBuffer buffer;

    TextView input;
    Button evalButton;


    static void Main(string[] args)
    {
        Application.Init ();
        new MainWindow ();
        Application.Run();
    }


    public void EvaluateInput()
    {
        eval.Parse(input.Buffer.Text);

        do
        {
            output = eval.Step();
            DataList.Add(output);

        } while (!(output is DoneData));

        TextIter insertIter = buffer.StartIter;

        foreach(var data in DataList)
        {
            if (data is MsgData)
            {
                switch ((data as MsgData).type)
                {
                    case MsgType.Print:
                        buffer.Insert(ref insertIter, (data as MsgData).msg + "\n");
                        break;
                    case MsgType.Error:
                        buffer.InsertWithTagsByName(ref insertIter, (data as MsgData).msg + "\n", "error");
                        break;
                    case MsgType.Info:
                        buffer.InsertWithTagsByName(ref insertIter, (data as MsgData).msg + "\n", "info");
                        break;
                }
            }
            else if (data is PlotData)
            {

            }
        }

        DataList.Clear();
    }

    public void CreateDefTree()
    {
        CellRenderer renderer;

        defStore = new ListStore(typeof(string), typeof(string));
        defTree = new TreeView(defStore);
        defTree.Expand = true;

        renderer = new CellRendererText();
        defTree.AppendColumn("Variable", renderer, "text",0);
        renderer = new CellRendererText();
        defTree.AppendColumn("Value", renderer, "text",1);
    }

    public void UpdateDefinitions()
    {
        defStore.Clear();

        foreach (var def in eval.scope.locals)
        {
            if (def.Value is SysFunc)
            {
                defStore.AppendValues(def.Value.ToString(), "System Magic");
            }
            else if (def.Value is UsrFunc)
            {
                defStore.AppendValues(def.Value.ToString(), (def.Value as UsrFunc).expr.ToString());
            }
            else
            {
                defStore.AppendValues(def.Key, def.Value.ToString());
            }
        }
    }

    public MainWindow() : base("MainWindow")
    {
        DeleteEvent += (o, a) => Application.Quit ();

        SetSizeRequest(500, 500);

        grid = new Grid ();
        Add (grid);

        input = new TextView ();
        grid.Attach (input, 0, 0, 1, 1);

        evalButton = new Button("Evaluate");
        evalButton.Clicked += (o, a) => EvaluateInput();
        evalButton.Clicked += (o, a) => UpdateDefinitions();
        grid.Attach(evalButton, 0, 1, 1, 1); 

        textView = new TextView();
        textView.Expand = true;
        textView.Editable = false;
        var sw = new ScrolledWindow ();
        sw.Add(textView);
        grid.Attach (sw, 0, 2, 1, 1);
        buffer = textView.Buffer;


        var infoTag = new TextTag ("info");
        infoTag.Foreground = "blue";
        var errorTag = new TextTag ("error");
        errorTag.Foreground = "red";
        buffer.TagTable.Add(infoTag);
        buffer.TagTable.Add(errorTag);


        CreateDefTree();
        grid.Attach(defTree, 1, 0, 1, 3);

        eval = new Evaluator ();
        DataList = new List<EvalData>();
        UpdateDefinitions();

        ShowAll ();
    }
}
