﻿using System;
using System.Threading;
using Ast;
using System.Collections.Generic;
using Gtk;

namespace DesktopUI
{
    // The core of the user interface, containing all widgets, and a lot of tools to manipulate them.
    public class TextViewList : Grid
    {
        public List<MovableCasTextView> castextviews = new List<MovableCasTextView>();
        Evaluator Eval;
        User user;
        Window window;

        // Constructor for textviewlist
        public TextViewList(User user, Evaluator Eval, Window window)
            : base()
        {
            this.Eval = Eval;
            this.user = user;
            this.window = window;

            ColumnSpacing = 10;
            RowSpacing = 10;

            // Resizes all widgets when the window is resized
            window.ResizeChecked += delegate
            {
                const int buttonbarwidth = 200;

                foreach (Widget widget in castextviews)
                {
                    if (widget is MovableCasCalcView)
                    {
                        (widget as MovableCasCalcView).calcview.input.WidthRequest = window.Window.Width - buttonbarwidth;
                        (widget as MovableCasCalcView).calcview.drawView.WidthRequest = window.Window.Width - buttonbarwidth;
                    }
                    else if (widget is MovableDrawCanvas)
                    {
                        (widget as MovableDrawCanvas).canvas.WidthRequest = window.Window.Width - buttonbarwidth;
                    }
                    else if (widget is MovableCasCalcMulitlineView)
                    {
                        (widget as MovableCasCalcMulitlineView).calcview.input.WidthRequest = window.Window.Width - buttonbarwidth;
                        (widget as MovableCasCalcMulitlineView).calcview.output.WidthRequest = window.Window.Width - buttonbarwidth;
                        (widget as MovableCasCalcMulitlineView).calcview.evaluateButton.WidthRequest = window.Window.Width - buttonbarwidth;
                        (widget as MovableCasCalcMulitlineView).calcview.drawView.WidthRequest = window.Window.Width - buttonbarwidth;
                    }
                    else if (widget is MovableCasResult)
                    {
                        (widget as MovableCasResult).casresult.entryFasitGet.WidthRequest = (window.Window.Width - buttonbarwidth - (widget as MovableCasResult).casresult.labelFacitGet.AllocatedWidth);
                    }
                    else if (widget is MovableCasTextView) // <- This shall always be last as all other widgets inherit from it, but not all use it.
                    {
                        (widget as MovableCasTextView).textview.WidthRequest = window.Window.Width - buttonbarwidth;
                    }
                }
            };

            ShowAll();
        }

        // Inserts a textview
        public void InsertTextView(string serializedString, bool locked, int pos)
        {
            MovableCasTextView movableCasTextView = new MovableCasTextView(serializedString, locked);
            movableCasTextView.textview.LockTextView(locked);

            movableCasTextView.Attach(AddLockCheckButton(movableCasTextView), 1, 100, 1, 1);
            movableCasTextView.Attach(AddCommandButtons(movableCasTextView), 100, 1, 1, 1);

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

        // Inserts a modified taskgenwidget, made for autogenerated tasks
        public void InsertTaskGenTextView(string TaskString)
        {
            MovableCasTextView movableCasTextView = new MovableCasTextView(TaskString);
            movableCasTextView.textview.LockTextView(true);

            movableCasTextView.Attach(AddLockCheckButton(movableCasTextView), 1, 100, 1, 1);
            movableCasTextView.Attach(AddCommandButtons(movableCasTextView), 100, 1, 1, 1);

            castextviews.Add(movableCasTextView);

            Clear();
            Redraw();
            ShowAll();
        }

        // Inserts an empty calcview
        public void InsertCalcView(int pos)
        {
            MovableCasCalcView MovCasCalcView = new MovableCasCalcView(Eval);
            MovCasCalcView.calcview.input.Activated += delegate
            {
                MovCasCalcView.calcview.Evaluate();
                Reevaluate();
                MovCasCalcView.ShowAll();
            };

            MovCasCalcView.Attach(AddLockCheckButton(MovCasCalcView), 1, 100, 1, 1);
            MovCasCalcView.Attach(AddCommandButtons(MovCasCalcView), 100, 1, 1, 1);

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

        // Inserts a calcview that has some kind of input.
        public void InsertCalcView(string input, bool locked)
        {
            MovableCasCalcView MovCasCalcView = new MovableCasCalcView(Eval);
            MovCasCalcView.calcview.input.Text = input;
            //MovCasCalcView.calcview.input.IsEditable = !locked;
            MovCasCalcView.calcview.input.Activated += delegate
            {
                MovCasCalcView.calcview.Evaluate();
                MovCasCalcView.ShowAll();
            };

            //MovCasCalcView.Attach(AddLockCheckButton(MovCasCalcView), 1, 100, 1, 1);
            MovCasCalcView.Attach(AddCommandButtons(MovCasCalcView), 100, 1, 1, 1);

            //if (user.privilege <= 0 && locked == true)
            //{
            //    MovCasCalcView.calcview.input.IsEditable = false;
            //}

            castextviews.Add(MovCasCalcView);

            Clear();
            Redraw();
            ShowAll();
        }

        public void InsertCalcMultilineView(int pos)
        {
            MovableCasCalcMulitlineView movCasCalcMultiView = new MovableCasCalcMulitlineView("",Eval);
            
            movCasCalcMultiView.calcview.evaluateButton.Clicked += delegate
            {
                movCasCalcMultiView.calcview.Evaluate();
                Reevaluate();
                movCasCalcMultiView.ShowAll();
            };

            movCasCalcMultiView.Attach(AddLockCheckButton(movCasCalcMultiView), 1, 100, 1, 1);
            movCasCalcMultiView.Attach(AddCommandButtons(movCasCalcMultiView), 100, 1, 1, 1);

            if (pos == -1)
            {
                castextviews.Add(movCasCalcMultiView);
            }
            else
            {
                castextviews.Insert(pos, movCasCalcMultiView);
            }

            Clear();
            Redraw();
            ShowAll();
        }

        public void InsertCalcMultilineView(string serializedString, bool locked)
        {
            MovableCasCalcMulitlineView movCasCalcMultiView = new MovableCasCalcMulitlineView(serializedString, Eval);

            movCasCalcMultiView.calcview.evaluateButton.Clicked += delegate
            {
                movCasCalcMultiView.calcview.Evaluate();
                movCasCalcMultiView.ShowAll();
            };

            movCasCalcMultiView.Attach(AddCommandButtons(movCasCalcMultiView), 100, 1, 1, 1);
            castextviews.Add(movCasCalcMultiView);
            
            Clear();
            Redraw();
            ShowAll();
        }

        // Inserts a drawcanvas
        public void InsertDrawCanvas(int pos)
        {
            MovableDrawCanvas movableDrawCanvas = new MovableDrawCanvas();

            //movableDrawCanvas.Attach(AddLockCheckButton(movableDrawCanvas), 1, 100, 1, 1);
            movableDrawCanvas.Attach(AddCommandButtons(movableDrawCanvas), 100, 1, 1, 1);

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

        // Inserts a result widget
        public void InsertResult(string answer, string facit)
        {
            MovableCasResult MovableCasResult = new MovableCasResult(user, answer, facit);

            MovableCasResult.Attach(AddLockCheckButton(MovableCasResult), 1, 100, 1, 1);
            MovableCasResult.Attach(AddCommandButtons(MovableCasResult), 100, 1, 1, 1);

            castextviews.Add(MovableCasResult);

            Clear();
            Redraw();
            ShowAll();
        }

        public void InsertResult(int pos)
        {
            MovableCasResult movableCasResult = new MovableCasResult(user, "", "");

            movableCasResult.Attach(AddLockCheckButton(movableCasResult), 1, 100, 1, 1);
            movableCasResult.Attach(AddCommandButtons(movableCasResult), 100, 1, 1, 1);

            castextviews.Insert(pos, movableCasResult);

            Clear();
            Redraw();
            ShowAll();
        }

        // Moves a widget, and switches it's position with one of it's neighbors.
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

        // Deletes the widget with ID
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

        // Clears the workspace
        public void Clear()
        {
            foreach (Widget widget in this)
            {
                Remove(widget);
            }
        }

        // Redraws the workspace
        public void Redraw()
        {
            foreach (Widget widget in castextviews)
            {
                Attach(widget, 1, Children.Length, 1, 1);
            }
        }

        // Reevaluates all cascalcviews
        public void Reevaluate()
        {
            foreach (Widget widget in castextviews)
            {
                if (widget is MovableCasCalcView)
                {
                    (widget as MovableCasCalcView).calcview.Evaluate();
                }
                else if (widget is MovableCasCalcMulitlineView)
                {
                    (widget as MovableCasCalcMulitlineView).calcview.Evaluate();
                }
            }
        }

        // Adds a new widget after the current widget.
        // Opens as a popup, where the user can select what widget to insert.
        public void AddNew(Widget w)
        {
            int _id = castextviews.IndexOf((MovableCasTextView)w);

            Button buttonCalcel = new Button("Cancel");
            Button buttonTextView = new Button("TextView");
            Button buttonCalcView = new Button("CalcView");
            Button buttonMultiline = new Button("Multiline CalcView");
            Button buttonResultView = new Button("ResultView");
            //Button buttonDrawCanvas = new Button("DrawCanvas");

            Window window = new Window("Insert new widget");

            window.SetSizeRequest(300, 300);

            VBox vbox = new VBox();

            vbox.PackStart(buttonTextView, false, false, 2);
            vbox.PackStart(buttonCalcView, false, false, 2);
            vbox.PackStart(buttonMultiline, false, false, 2);
            vbox.PackStart(buttonResultView, false, false, 2);
            //vbox.PackStart(buttonDrawCanvas, false, false, 2);

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
                buttonCalcel.Click();
            };

            buttonCalcView.Clicked += delegate
            {
                InsertCalcView(_id + 1);
                Clear();
                Reevaluate();
                Redraw();

                _id++;
                buttonCalcel.Click();
            };

            buttonMultiline.Clicked += delegate
            {
                InsertCalcMultilineView(_id + 1);
                Clear();
                Reevaluate();
                Redraw();

                _id++;
                buttonCalcel.Click();
            };

            buttonResultView.Clicked += delegate
            {
                InsertResult(_id + 1);
                Clear();
                Reevaluate();
                Redraw();

                _id++;
                buttonCalcel.Click();
            };

            //buttonDrawCanvas.Clicked += delegate
            //{
            //    InsertDrawCanvas(_id + 1);
            //    Clear();
            //    Reevaluate();
            //    Redraw();

            //    _id++;
            //    buttonCalcel.Click();
            //};
        }

        // Adds a lock button for teachers, so that they can set id the student can edit the content of the widget 
        CheckButton AddLockCheckButton(MovableCasTextView movableCasTextView)
        {
            if (user.privilege == 1)
            {
                CheckButton checkbutton = new CheckButton("Lock for students");

                if (movableCasTextView.textview.locked == true)
                {
                    checkbutton.Active = true;
                }

                checkbutton.Toggled += delegate
                {
                    movableCasTextView.textview.locked = !movableCasTextView.textview.locked;
                };

                return checkbutton;
            }
            else
            {
                return null;
            }
        }

        // Adds command buttons to the widget, allowing it to be moved and removed.
        VBox AddCommandButtons(MovableCasTextView movableCasTextView)
        {
            Button ButtonMoveUp = new Button("↑");
            Button ButtonMoveDown = new Button("↓");
            Button ButtonDelete = new Button("X");
            Button ButtonAddNew = new Button("+");

            VBox vbox = new VBox();

            if (user.privilege == 1 || (user.privilege <= 0 && movableCasTextView.textview.locked == false))
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

                HBox hbox = new HBox();
                Toolbar tb = new Toolbar();
                hbox.Add(ButtonMoveUp);
                hbox.Add(ButtonMoveDown);
                hbox.Add(ButtonDelete);
                hbox.Add(ButtonAddNew);
                vbox.PackStart(hbox, false, false, 2);
            }
            else if (user.privilege <= 0)
            {
                ButtonAddNew.Clicked += delegate
                {
                    AddNew(movableCasTextView);
                };

                vbox.PackStart(ButtonAddNew, false, false, 2);
            }

            return vbox;
        }
    }
}

