using System;
using System.Collections.Generic;
using Gtk;
using ImEx;

namespace LibUI
{
    public static class GlobalVar
    {
        public static string username;

        public static string password;

        public static int privilege;

        public static string file;

        public static List<Widget> listWidget = new List<Widget>();

        public static List<MetaType> listMeta = new List<MetaType>();

        public static Grid globalGrid = new Grid();

        public static int gridNumber = 1;

        public static VBox vboxWindow = new VBox(false, 2);

    }
}

