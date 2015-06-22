using Gtk;
using Ast;
using Draw;
using System.Collections.Generic;
using System.Threading;
using System.Globalization;

public class Calculator : Window
{
    Evaluator Eval;

    Grid Grid;

    TreeStore DefinitionStore;
    TreeView DefinitionTree;

    TextView OutputView;
    TextBuffer Buffer;

    TextView InputView;
    Button EvalButton;

    DrawView DrawView;

    static void Main(string[] args)
    {
        Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo("en-US");
        Application.Init ();
        new Calculator ();
        Application.Run();
    }
        

    public void EvaluateInput()
    {
        Expression res;
        TextIter insertIter = Buffer.StartIter;

        if (InputView.Buffer.Text.Length == 0)
        {
            Buffer.InsertWithTagsByName(ref insertIter, "No input\n", "error");
            return;
        }

        Eval.Parse(InputView.Buffer.Text);

        if (Eval.Error == null)
        {
            res = Eval.Evaluate();

            if (res is Error)
                Eval.SideEffects.Add(new ErrorData(res as Error));
            else if (!(res is Null))
                Buffer.Insert(ref insertIter, "ret: " + res.ToString() + "\n");
        }

        foreach(var data in Eval.SideEffects)
        {
            if (data is PrintData)
                Buffer.Insert(ref insertIter, data.ToString() + "\n");
            else if (data is ErrorData)
                Buffer.InsertWithTagsByName(ref insertIter, data.ToString() + "\n", "error");
            else if (data is DebugData && Eval.GetBool("debug"))
                Buffer.InsertWithTagsByName(ref insertIter, data.ToString() + "\n", "debug");
            else if (data is PlotData)
            {
                DrawView.Plot(data as PlotData);
                DrawView.Show();
            }
        }
    }

    public void CreateDefTree()
    {
        CellRenderer renderer;

        DefinitionStore = new TreeStore(typeof(string), typeof(string));
        DefinitionTree = new TreeView(DefinitionStore);
        DefinitionTree.Expand = true;

        renderer = new CellRendererText();
        DefinitionTree.AppendColumn("Variable", renderer, "text", 0);
        renderer = new CellRendererText();
        DefinitionTree.AppendColumn("Value", renderer, "text", 1);
    }

    public void UpdateDefinitions()
    {
        TreeIter iter;
        DefinitionStore.Clear();

        foreach (var @var in Eval.Locals)
        {
            if (@var.Value is SysFunc)
            {
                iter = DefinitionStore.AppendValues(@var.Value.ToString(), "System Magic");
                UpdateScope(@var.Value as Scope, iter);
            }
            else if (@var.Value is VarFunc)
            {
                iter = DefinitionStore.AppendValues(@var.Value.ToString(), (@var.Value as VarFunc).Definition.ToString());
                UpdateScope(@var.Value as Scope, iter);
            }
            else if (@var.Value is Scope)
            {
                iter = DefinitionStore.AppendValues(@var.Key, @var.Value.ToString());
                UpdateScope(@var.Value as Scope, iter);
            }
            else
            {
                DefinitionStore.AppendValues(@var.Key, @var.Value.ToString());
            }
        }
    }

    public void UpdateScope(Scope scope, TreeIter lastIter)
    {
        TreeIter iter;

        foreach (var @var in scope.Locals)
        {
            if (@var.Value is SysFunc)
            {
                iter = DefinitionStore.AppendValues(lastIter, @var.Value.ToString(), "System Magic");
                UpdateScope(@var.Value as Scope, iter);
            }
            else if (@var.Value is VarFunc)
            {
                iter = DefinitionStore.AppendValues(lastIter, @var.Value.ToString(), (@var.Value as VarFunc).Definition.ToString(), lastIter);
                UpdateScope(@var.Value as Scope, iter);
            }
            else if (@var.Value is Scope)
            {
                iter = DefinitionStore.AppendValues(lastIter, @var.Key, @var.Value.ToString());
                UpdateScope(@var.Value as Scope, iter);
            }
            else
            {
                DefinitionStore.AppendValues(lastIter, @var.Key, @var.Value.ToString());
            }
        }
    }

    public Calculator() : base("MainWindow")
    {
        DeleteEvent += (o, a) => Application.Quit ();

        SetSizeRequest(500, 500);

        Grid = new Grid ();
        Add (Grid);

        InputView = new TextView ();
        Grid.Attach (InputView, 0, 0, 1, 1);

        EvalButton = new Button("Evaluate");
        EvalButton.Clicked += (o, a) => EvaluateInput();
        EvalButton.Clicked += (o, a) => UpdateDefinitions();
        Grid.Attach(EvalButton, 0, 1, 1, 1); 


        OutputView = new TextView();
        OutputView.Expand = true;
        OutputView.Editable = false;
        var sw = new ScrolledWindow ();
        sw.Add(OutputView);
        Grid.Attach (sw, 0, 2, 1, 1);
        Buffer = OutputView.Buffer;

        DrawView = new DrawView();
        Grid.Attach(DrawView, 0, 3, 1, 1);

        var infoTag = new TextTag ("debug");
        infoTag.Foreground = "blue";
        var errorTag = new TextTag ("error");
        errorTag.Foreground = "red";
        Buffer.TagTable.Add(infoTag);
        Buffer.TagTable.Add(errorTag);


        CreateDefTree();
        Grid.Attach(DefinitionTree, 1, 0, 1, 3);

        Eval = new Evaluator ();
        UpdateDefinitions();

        ShowAll ();
    }
}
