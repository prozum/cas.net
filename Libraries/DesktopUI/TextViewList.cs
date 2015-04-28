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
        Evaluator Eval = new Evaluator();
        User user;


        public TextViewList(ref User user)
            : base()
        {
            this.user = user;

            ShowAll();
        }

        public void InsertTextView(string serializedString, bool locked)
        {
            Button ButtonMoveUp = new Button("↑");
            Button ButtonMoveDown = new Button("↓");

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

                movableLockedCasTextView.Attach(ButtonMoveUp, 2, 1, 1, 1);
                movableLockedCasTextView.Attach(ButtonMoveDown, 2, 2, 1, 1);

                castextviews.Add(movableLockedCasTextView);
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

                    movableCasTextView.Attach(ButtonMoveUp, 2, 1, 1, 1);
                    movableCasTextView.Attach(ButtonMoveDown, 2, 2, 1, 1);
                }

                castextviews.Add(movableCasTextView);
            }

            Clear();
            Redraw();
            ShowAll();
        }

        public void InsertCalcView()
        {
            Button ButtonMoveUp = new Button("↑");
            Button ButtonMoveDown = new Button("↓");

            MovableCasCalcView MovCasCalcView = new MovableCasCalcView(Eval);
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

            MovCasCalcView.Attach(ButtonMoveUp, 2, 1, 1, 1);
            MovCasCalcView.Attach(ButtonMoveDown, 2, 2, 1, 1);

            castextviews.Add(MovCasCalcView);

            Clear();
            Redraw();
            ShowAll();
        }

        public void InsertDrawCanvas()
        {
            Button ButtonMoveUp = new Button("↑");
            Button ButtonMoveDown = new Button("↓");

            MovableDrawCanvas movableDrawCanvas = new MovableDrawCanvas();

            ButtonMoveUp.Clicked += delegate
            {
                Move(movableDrawCanvas.id_, -1);
            };

            ButtonMoveDown.Clicked += delegate
            {
                Move(movableDrawCanvas.id_, 1);
            };

            VBox vbox = new VBox();
            vbox.PackStart(ButtonMoveUp, false, false, 2);
            vbox.PackEnd(ButtonMoveDown, false, false, 2);

            movableDrawCanvas.Attach(vbox, 2, 1, 1, 2);

            castextviews.Add(movableDrawCanvas);

            Clear();
            Redraw();
            ShowAll();
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
    }
}

