using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ParseTreeVisualizer.Util {
    public static class FrameworkElementExtensions {
        public static T DataContext<T>(this FrameworkElement element) => (T)element.DataContext;
    }
}
