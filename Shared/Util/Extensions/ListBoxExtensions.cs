using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ParseTreeVisualizer.Util {
    public static class ListBoxExtensions {
        public static List<T> SelectedItems<T>(this ListBox listbox) => listbox.SelectedItems.Cast<T>().ToList();
    }
}
