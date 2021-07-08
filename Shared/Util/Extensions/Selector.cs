using System.Windows.Controls.Primitives;

namespace ParseTreeVisualizer.Util {
    public static class SelectorExtensions {
        public static T SelectedItem<T>(this Selector selector) => (T)selector.SelectedItem;
    }
}
