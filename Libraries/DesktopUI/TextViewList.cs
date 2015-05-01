using System;
using System.Threading;
using Ast;
using System.Collections.Generic;
using Gtk;

namespace DesktopUI
{
    public class TextViewList : Grid
    {
        public List<MovableCasTextView> castextviews = new List<MovableCasTextView>();
        Grid ButtonGrid = new Grid();
        Evaluator Eval;
        User user;


        public TextViewList(ref User user, Evaluator Eval)
            : base()
        {
            this.Eval = Eval;
            this.user = user;

            ShowAll();
        }

        public void InsertTextView(string serializedString, bool locked, int pos)
        {
            Button ButtonMoveUp = new Button("↑");
            Button ButtonMoveDown = new Button("↓");
            Button ButtonDelete = new Button("X");
            Button ButtonAddNew = new Button("+");

            if (user.privilege == 1)
            {
                MovableLockedCasTextView movableLockedCasTextView = new MovableLockedCasTextView(serializedString, locked);
                movableLockedCasTextView.textview.LockTextView(false);

                ButtonMoveUp.Clicked += delegate
                {
                    Move(movableLockedCasTextView.id_, -1);
                };

                ButtonMoveDown.Clicked += delegate
                {
                    Move(movableLockedCasTextView.id_, 1);
                };

                ButtonDelete.Clicked += delegate
                {
                    Delete(movableLockedCasTextView.id_);
                };

                ButtonAddNew.Clicked += delegate
                {
                    AddNew(movableLockedCasTextView);
                };

                VBox vbox = new VBox();
                vbox.PackStart(ButtonMoveUp, false, false, 2);
                vbox.PackEnd(ButtonMoveDown, false, false, 2);
                vbox.PackStart(ButtonDelete, false, false, 2);
                vbox.PackStart(ButtonAddNew, false, false, 2);
                movableLockedCasTextView.Attach(vbox, 2, 1, 1, 2);

                if (pos == -1)
                {
                    castextviews.Add(movableLockedCasTextView);
                }
                else
                {
                    castextviews.Insert(pos, movableLockedCasTextView);
                }
            }
            else if (user.privilege == 0)
            {
                MovableCasTextView movableCasTextView = new MovableCasTextView(serializedString, locked);
                movableCasTextView.textview.LockTextView(locked);

                if (locked == false)
                {
                    ButtonMoveUp.Clicked += delegate
                    {
                        Move(movableCasTextView.id_, -1);
                    };

                    ButtonMoveDown.Clicked += delegate
                    {
                        Move(movableCasTextView.id_, 1);
                    };

                    ButtonDelete.Clicked += delegate
                    {
                        Delete(movableCasTextView.id_);
                    };

                    ButtonAddNew.Clicked += delegate
                    {
                        AddNew(movableCasTextView);
                    };

                    VBox vbox = new VBox();
                    vbox.PackStart(ButtonMoveUp, false, false, 2);
                    vbox.PackEnd(ButtonMoveDown, false, false, 2);
                    vbox.PackStart(ButtonDelete, false, false, 2);
                    vbox.PackStart(ButtonAddNew, false, false, 2);
                    movableCasTextView.Attach(vbox, 2, 1, 1, 2);
                }

                if (pos == -1)
                {
                    castextviews.Add(movableCasTextView);
                }
                else
                {
                    castextviews.Insert(pos, movableCasTextView);
                }
            }

            Clear();
            Redraw();
            ShowAll();
        }

        public void InsertCalcView(int pos)
        {
            Button ButtonMoveUp = new Button("↑");
            Button ButtonMoveDown = new Button("↓");
            Button ButtonDelete = new Button("X");
            Button ButtonAddNew = new Button("+");

            MovableCasCalcView MovCasCalcView = new MovableCasCalcView(Eval);
            MovCasCalcView.calcview.input.Activated += delegate
            {
                MovCasCalcView.calcview.Eval.locals.Clear();
                MovCasCalcView.calcview.Evaluate();
                Reevaluate();
                MovCasCalcView.ShowAll();
            };

            ButtonMoveUp.Clicked += delegate
            {
                MovCasCalcView.calcview.Eval.locals.Clear();
                Move(MovCasCalcView.id_, -1);
            };

            ButtonMoveDown.Clicked += delegate
            {
                MovCasCalcView.calcview.Eval.locals.Clear();
                Move(MovCasCalcView.id_, 1);
            };

            ButtonDelete.Clicked += delegate
            {
                Delete(MovCasCalcView.id_);
            };

            ButtonAddNew.Clicked += delegate
            {
                AddNew(MovCasCalcView);
            };

            VBox vbox = new VBox();
            vbox.PackStart(ButtonMoveUp, false, false, 2);
            vbox.PackEnd(ButtonMoveDown, false, false, 2);
            vbox.PackStart(ButtonDelete, false, false, 2);
            vbox.PackStart(ButtonAddNew, false, false, 2);
            MovCasCalcView.Attach(vbox, 2, 1, 1, 2);

            if (pos == -1)
            {
                castextviews.Add(MovCasCalcView);
            }
            else
            {
                castextviews.Insert(pos, MovCasCalcView);
            }

            Clear();
            Redraw();
            ShowAll();
        }

        public void InsertCalcView(string input)
        {
            Button ButtonMoveUp = new Button("↑");
            Button ButtonMoveDown = new Button("↓");
            Button ButtonDelete = new Button("X");
            Button ButtonAddNew = new Button("+");

            MovableCasCalcView MovCasCalcView = new MovableCasCalcView(Eval);

            MovCasCalcView.calcview.input.Text = input;

            MovCasCalcView.calcview.input.Activated += delegate
            {
                MovCasCalcView.calcview.Eval.scope.locals.Clear();
                MovCasCalcView.calcview.Evaluate();
                MovCasCalcView.ShowAll();
            };

            ButtonMoveUp.Clicked += delegate
            {
                MovCasCalcView.calcview.Eval.scope.locals.Clear();
                Move(MovCasCalcView.id_, -1);
                MovCasCalcView.calcview.Eval.scope.locals.Clear();
            };

            ButtonMoveDown.Clicked += delegate
            {
                MovCasCalcView.calcview.Eval.scope.locals.Clear();
                Move(MovCasCalcView.id_, 1);
                MovCasCalcView.calcview.Eval.scope.locals.Clear();
            };

            ButtonDelete.Clicked += delegate
            {
                Delete(MovCasCalcView.id_);
            };

            ButtonAddNew.Clicked += delegate
            {
                AddNew(MovCasCalcView);
            };

            VBox vbox = new VBox();
            vbox.PackStart(ButtonMoveUp, false, false, 2);
            vbox.PackEnd(ButtonMoveDown, false, false, 2);
            vbox.PackStart(ButtonDelete, false, false, 2);
            vbox.PackStart(ButtonAddNew, false, false, 2);
            MovCasCalcView.Attach(vbox, 2, 1, 1, 2);

            castextviews.Add(MovCasCalcView);

            Clear();
            Redraw();
            ShowAll();
        }

        public void InsertDrawCanvas(int pos)
        {
            Button ButtonMoveUp = new Button("↑");
            Button ButtonMoveDown = new Button("↓");
            Button ButtonDelete = new Button("X");
            Button ButtonAddNew = new Button("+");

            MovableDrawCanvas movableDrawCanvas = new MovableDrawCanvas();

            ButtonMoveUp.Clicked += delegate
            {
                Move(movableDrawCanvas.id_, -1);
            };

            ButtonMoveDown.Clicked += delegate
            {
                Move(movableDrawCanvas.id_, 1);
            };

            ButtonDelete.Clicked += delegate
            {
                Delete(movableDrawCanvas.id_);
            };

            ButtonAddNew.Clicked += delegate
            {
                AddNew(movableDrawCanvas);
            };

            VBox vbox = new VBox();
            vbox.PackStart(ButtonMoveUp, false, false, 2);
            vbox.PackEnd(ButtonMoveDown, false, false, 2);
            vbox.PackStart(ButtonDelete, false, false, 2);
            vbox.PackStart(ButtonAddNew, false, false, 2);

            movableDrawCanvas.Attach(vbox, 2, 1, 1, 2);

            if (pos == -1)
            {
                castextviews.Add(movableDrawCanvas);
            }
            else
            {
                castextviews.Insert(pos, movableDrawCanvas);
            }

            Clear();
            Redraw();
            ShowAll();
        }

        public void InsertResult(string answer, string facit)
        {
            Button ButtonMoveUp = new Button("↑");
            Button ButtonMoveDown = new Button("↓");
            Button ButtonDelete = new Button("X");
            Button ButtonAddNew = new Button("+");

            MovableCasResult MovableCasResult = new MovableCasResult(ref user, answer, facit);

            ButtonMoveUp.Clicked += delegate
            {
                Move(MovableCasResult.id_, -1);
            };

            ButtonMoveDown.Clicked += delegate
            {
                Move(MovableCasResult.id_, 1);
            };

            ButtonDelete.Clicked += delegate
            {
                Delete(MovableCasResult.id_);
            };

            ButtonAddNew.Clicked += delegate
            {
                AddNew(MovableCasResult);
            };

            VBox vbox = new VBox();
            vbox.PackStart(ButtonMoveUp, false, false, 2);
            vbox.PackEnd(ButtonMoveDown, false, false, 2);
            vbox.PackStart(ButtonDelete, false, false, 2);
            vbox.PackStart(ButtonAddNew, false, false, 2);

            MovableCasResult.Attach(vbox, 2, 1, 1, 2);

            castextviews.Add(MovableCasResult);

            Clear();
            Redraw();
            ShowAll();

            Console.WriteLine("Inserted");
        }

        public void Move(int ID, int UpOrDown)
        {
            int i = 0;

            while (ID != castextviews[i].id_)
            {
                i++;
            }

            // move down
            if (UpOrDown == 1 && i + 1 < castextviews.Count)
            {
                castextviews.Insert(i + 2, castextviews[i]);
                castextviews.RemoveAt(i);
            }
			// move up
			else if (UpOrDown == -1 && i - 1 >= 0)
            {
                castextviews.Insert(i - 1, castextviews[i]);
                castextviews.RemoveAt(i + 1);
            }

            Clear();
            Reevaluate();
            Redraw();
        }

        public void Delete(int ID)
        {
            int i = 0;

            while (ID != castextviews[i].id_)
            {
                i++;
            }

            castextviews.RemoveAt(i);

            Clear();
            Reevaluate();
            Redraw();
        }

        public void Clear()
        {
            foreach (Widget widget in this)
            {
                Remove(widget);
            }
        }

        public void Redraw()
        {
            foreach (Widget widget in castextviews)
            {
                Attach(widget, 1, Children.Length, 1, 1);
            }

            Attach(ButtonGrid, 1, Children.Length, 1, 1);
        }

        public void Reevaluate()
        {
            foreach (Widget widget in castextviews)
            {
                if (widget.GetType() == typeof(MovableCasCalcView))
                {
                    (widget as MovableCasCalcView).calcview.Evaluate();
                }
            }
        }

        public void AddNew(Widget w)
        {
            int _id = castextviews.IndexOf((MovableCasTextView)w);

            Button buttonCalcel = new Button("Cancel");
            Button buttonTextView = new Button("TextView");
            Button buttonCalcView = new Button("CalcView");
            Button buttonDrawCanvas = new Button("DrawCanvas");

            Window window = new Window("Insert new widget");

            window.SetSizeRequest(300, 300);

            VBox vbox = new VBox();

            vbox.PackStart(buttonTextView, false, false, 2);
            vbox.PackStart(buttonCalcView, false, false, 2);
            vbox.PackStart(buttonDrawCanvas, false, false, 2);

            vbox.PackEnd(buttonCalcel, false, false, 2);

            window.Add(vbox);

            window.ShowAll();

            buttonCalcel.Clicked += delegate
            {
                window.Destroy();
            };

            buttonTextView.Clicked += delegate
            {
                InsertTextView("", false, _id + 1);
                Clear();
                Reevaluate();
                Redraw();

                _id++;
            };

            buttonCalcView.Clicked += delegate
            {
                InsertCalcView(_id + 1);
                Clear();
                Reevaluate();
                Redraw();

                _id++;
            };

            buttonDrawCanvas.Clicked += delegate
            {
                InsertDrawCanvas(_id + 1);
                Clear();
                Reevaluate();
                Redraw();

                _id++;
            };
        }
    }
}

