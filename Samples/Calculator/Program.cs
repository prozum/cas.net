using Gtk;
using Ast;
using Draw;
using System.Collections.Generic;
using System.Threading;
using System.Globalization;

public class Calculator : Window
{
    Evaluator eval;

    Grid grid;

    TreeStore defStore;
    TreeView defTree;

    TextView textView;
    TextBuffer buffer;

    TextView input;
    Button evalButton;

    DrawView draw;

    static void Main(string[] args)
    {
        Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo("en-US");
        Application.Init ();
        new Calculator ();
        Application.Run();
    }
        

    public void EvaluateInput()
    {
        TextIter insertIter = buffer.StartIter;

        if (input.Buffer.Text.Length == 0)
        {
            buffer.InsertWithTagsByName(ref insertIter, "No input\n", "error");
            return;
        }

        eval.Parse(input.Buffer.Text);

        var res = eval.Evaluate();

        if (!(res is Null || res is Error))
            buffer.Insert(ref insertIter, "ret: " + res.ToString() + "\n");

        foreach(var data in eval.SideEffects)
        {

            if (data is PrintData)
                buffer.Insert(ref insertIter, data.ToString() + "\n");
            else if (data is ErrorData)
                buffer.InsertWithTagsByName(ref insertIter, data.ToString() + "\n", "error");
            else if (data is DebugData && eval.GetBool("debug"))
                buffer.InsertWithTagsByName(ref insertIter, data.ToString() + "\n", "debug");
            else if (data is PlotData)
            {
                draw.Plot(data as PlotData);
                draw.Show();
            }
        }
    }

    public void CreateDefTree()
    {
        CellRenderer renderer;

        defStore = new TreeStore(typeof(string), typeof(string));
        defTree = new TreeView(defStore);
        defTree.Expand = true;

        renderer = new CellRendererText();
        defTree.AppendColumn("Variable", renderer, "text", 0);
        renderer = new CellRendererText();
        defTree.AppendColumn("Value", renderer, "text", 1);
    }

    public void UpdateDefinitions()
    {
        defStore.Clear();

        foreach (var def in eval.Locals)
        {
            if (def.Value is VariableFunc)
            {
                defStore.AppendValues(def.Value.ToString(), def.Value.Value.ToString());
            }
            else
            {
                var iter = defStore.AppendValues(def.Key, def.Value.Value.ToString());
                UpdateScope(def.Value, iter);
            }
        }
    }

    public void UpdateScope(Variable scope, TreeIter iter)
    {
        foreach (var def in scope.Locals)
        {
            if (def.Value is VariableFunc)
            {
                defStore.AppendValues(iter, def.Value.ToString(), def.Value.Value.ToString());
            }
            else
            {
                var subIter = defStore.AppendValues(iter, def.Key, def.Value.Value.ToString());
                UpdateScope(def.Value, subIter);
            }
        }
    }

    public Calculator() : base("MainWindow")
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

        draw = new DrawView();
        grid.Attach(draw, 0, 3, 1, 1);

        var infoTag = new TextTag ("debug");
        infoTag.Foreground = "blue";
        var errorTag = new TextTag ("error");
        errorTag.Foreground = "red";
        buffer.TagTable.Add(infoTag);
        buffer.TagTable.Add(errorTag);


        CreateDefTree();
        grid.Attach(defTree, 1, 0, 1, 3);

        eval = new Evaluator ();
        UpdateDefinitions();

        ShowAll ();
    }
}
