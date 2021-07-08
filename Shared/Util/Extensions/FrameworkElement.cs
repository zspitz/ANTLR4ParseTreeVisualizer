using System.Windows;

namespace ParseTreeVisualizer.Util {
    public static class FrameworkElementExtensions {
        public static T DataContext<T>(this FrameworkElement element) => (T)element.DataContext;
    }
}
