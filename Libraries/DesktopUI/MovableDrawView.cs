﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Draw;

namespace DesktopUI
{
    // A movable version of the drawview
    // Currently not used
    public class MovableDrawView : MovableCasTextView
    {
        DrawView drawView;

        public MovableDrawView() : base("",false)
        {
            drawView = new DrawView();

            Remove(textview);
            Attach(drawView, 1, 1, 1, 2);
        }
    }
}
