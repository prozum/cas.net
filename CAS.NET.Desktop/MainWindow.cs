﻿using Gtk;
using Ast;

public class MainWindow : Window
{
    Evaluator eval;
    Expression output;

    Grid grid;
    TextView textview;
    Entry entry;


    static void Main(string[] args)
    {
        Application.Init ();
        new MainWindow ();
        Application.Run();
    }
        

    public void EvaluateEntry()
    {
        output = Ast.Parser.Parse (eval, entry.Text);

        textview.Buffer.Insert (textview.Buffer.StartIter, output.ToString () + " => " + output.Simplify().Evaluate().ToString()  +"\n");
    }

    public MainWindow() : base("MainWindow")
    {
        DeleteEvent += (o, a) => Application.Quit ();

        grid = new Grid ();
        Add (grid);

        eval = new Evaluator ();

        entry = new Entry ();
        entry.Expand = true;
        entry.Activated += (o, a) => EvaluateEntry ();
        grid.Attach (entry,0,0,1,1);
        textview = new TextView();
        textview.Expand = true;
        grid.Attach (textview,0,1,1,1);


        ShowAll ();
    }
}
