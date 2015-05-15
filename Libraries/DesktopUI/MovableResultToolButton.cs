﻿using System;
using Gtk;

namespace DesktopUI
{
    public class MovableResultToolButton : ToolButton
    {
        TextViewList textviews;

        public MovableResultToolButton(ref TextViewList textviews)
            : base("Result")
        {
            this.textviews = textviews;

            this.TooltipText = "Set fixed result";

            this.Clicked += delegate
            {
                OnActivated();
            };
        }

        void OnActivated()
        {
            textviews.InsertResult("", "");
        }
    }
}

