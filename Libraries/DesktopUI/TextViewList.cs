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
        Window window;

        public TextViewList(User user, Evaluator Eval, Window window)
            : base()
        {
            this.Eval = Eval;
            this.user = user;
            this.window = window;

            window.ResizeChecked += delegate
            {
                foreach (Widget widget in castextviews)
                {
                    if(widget is MovableCasCalcView)
                    {
                        (widget as MovableCasCalcView).calcview.input.WidthRequest = window.Window.Width - 60; 
                    }
                    else if (widget is MovableDrawCanvas)
                    {
                        (widget as MovableDrawCanvas).canvas.WidthRequest = window.Window.Width - 60;
                    }
                    else if(widget is MovableCasTextView) // <- This shall always be last as all other widgets inherit from it, but not all use it.
                    {
                        (widget as MovableCasTextView).textview.WidthRequest = window.Window.Width - 60;
                    }
                }
            };

            ShowAll();
        }

        public void InsertTextView(string serializedString, bool locked, int pos)
        {
            MovableCasTextView movableCasTextView = new MovableCasTextView(serializedString, locked);
            movableCasTextView.textview.LockTextView(locked);

            movableCasTextView = AddLockCheckButton(movableCasTextView);
            movableCasTextView = AddCommandButtons(movableCasTextView);

            if (pos == -1)
            {
                castextviews.Add(movableCasTextView);
            }
            else
            {
                castextviews.Insert(pos, movableCasTextView);
            }

            Clear();
            Redraw();
            ShowAll();
        }

        public void InsertTaskGenTextView(string TaskString)
        {
            //Button ButtonMoveUp = new Button("↑");
            //Button ButtonMoveDown = new Button("↓");
            Button ButtonDelete = new Button("X");
            Button ButtonAddNew = new Button("+");

            MovableCasTextView movableCasTextView = new MovableCasTextView(TaskString);
            movableCasTextView.textview.LockTextView(true);

            ButtonDelete.Clicked += delegate
            {
                Delete(movableCasTextView.id_);
            };

            ButtonAddNew.Clicked += delegate
            {
                AddNew(movableCasTextView);
            };

            VBox vbox = new VBox();
            //vbox.PackStart(ButtonMoveUp, false, false, 2);
            //vbox.PackEnd(ButtonMoveDown, false, false, 2);
            vbox.PackStart(ButtonDelete, false, false, 2);
            vbox.PackStart(ButtonAddNew, false, false, 2);
            movableCasTextView.Attach(vbox, 2, 1, 1, 2);

            castextviews.Add(movableCasTextView);

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
                MovCasCalcView.calcview.Eval.Locals.Clear();
                MovCasCalcView.calcview.Evaluate();
                Reevaluate();
                MovCasCalcView.ShowAll();
            };

            ButtonMoveUp.Clicked += delegate
            {
                MovCasCalcView.calcview.Eval.Locals.Clear();
                Move(MovCasCalcView.id_, -1);
            };

            ButtonMoveDown.Clicked += delegate
            {
                MovCasCalcView.calcview.Eval.Locals.Clear();
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
                MovCasCalcView.calcview.Eval.Scope.Locals.Clear();
                MovCasCalcView.calcview.Evaluate();
                MovCasCalcView.ShowAll();
            };

            ButtonMoveUp.Clicked += delegate
            {
                MovCasCalcView.calcview.Eval.Scope.Locals.Clear();
                Move(MovCasCalcView.id_, -1);
                MovCasCalcView.calcview.Eval.Scope.Locals.Clear();
            };

            ButtonMoveDown.Clicked += delegate
            {
                MovCasCalcView.calcview.Eval.Scope.Locals.Clear();
                Move(MovCasCalcView.id_, 1);
                MovCasCalcView.calcview.Eval.Scope.Locals.Clear();
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

            MovableCasResult MovableCasResult = new MovableCasResult(user, answer, facit);

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
                if(widget is MovableCasCalcView)
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

        MovableCasTextView AddLockCheckButton(MovableCasTextView mctv)
        {
            if(user.privilege == 1)
            {
                CheckButton checkbutton = new CheckButton("Lock for students");

                if (mctv.textview.locked == true)
                {
                    checkbutton.Active = true;
                }

                checkbutton.Toggled += delegate
                {
                    mctv.textview.locked = !mctv.textview.locked;
                };

                mctv.Attach(checkbutton, 1, 100, 1, 1);
                return mctv;
            }
            else
            {
                return mctv;
            }
        }

        MovableCasTextView AddCommandButtons(MovableCasTextView mctv)
        {
            Button ButtonMoveUp = new Button("↑");
            Button ButtonMoveDown = new Button("↓");
            Button ButtonDelete = new Button("X");
            Button ButtonAddNew = new Button("+");

            if (user.privilege == 1 || (user.privilege == 0 && mctv.textview.locked == false))
            {

                ButtonMoveUp.Clicked += delegate
                {
                    Move(mctv.id_, -1);
                };

                ButtonMoveDown.Clicked += delegate
                {
                    Move(mctv.id_, 1);
                };

                ButtonDelete.Clicked += delegate
                {
                    Delete(mctv.id_);
                };

                ButtonAddNew.Clicked += delegate
                {
                    AddNew(mctv);
                };

                VBox vbox = new VBox();
                HBox hbox = new HBox();
                Toolbar tb = new Toolbar();
                hbox.Add(ButtonMoveUp);
                hbox.Add(ButtonMoveDown);
                hbox.Add(ButtonDelete);
                hbox.Add(ButtonAddNew);
                vbox.PackStart(hbox, false, false, 2);
                mctv.Attach(vbox, 2, 1, 1, 2);
            }
            else if (user.privilege <= 0)
            {
                ButtonAddNew.Clicked += delegate
                {
                    AddNew(mctv);
                };

                VBox vbox = new VBox();
                vbox.PackStart(ButtonAddNew, false, false, 2);
                mctv.Attach(vbox, 2, 1, 1, 2);
            }

            return mctv;
        }
    }
}

