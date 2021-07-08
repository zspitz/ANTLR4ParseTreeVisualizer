using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls.Primitives;

namespace ParseTreeVisualizer.Util {
    public static class MultiSelectorExtensions {
        public static List<T> SelectedItems<T>(this MultiSelector multiSelector) => multiSelector.SelectedItems.Cast<T>().ToList();
    }
}
