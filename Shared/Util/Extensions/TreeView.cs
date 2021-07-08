﻿using System.Windows.Controls;

namespace ParseTreeVisualizer.Util {
    public static class TreeViewExtensions {
        public static T SelectedItem<T>(this TreeView treeview) => (T)treeview.SelectedItem;
    }
}
