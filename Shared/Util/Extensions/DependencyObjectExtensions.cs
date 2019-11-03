using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace ParseTreeVisualizer.Util {
    public static class DependencyObjectExtensions {
        public static IEnumerable<DependencyObject> GetVisualAncestors(this DependencyObject dependencyObject) {
            while (true) {
                if (dependencyObject is null) { yield break; }
                yield return dependencyObject;
                dependencyObject = VisualTreeHelper.GetParent(dependencyObject);
            }
        }

        public static T GetVisualAncestor<T>(this DependencyObject dependencyObject) where T : DependencyObject => 
            dependencyObject.GetVisualAncestors().OfType<T>().FirstOrDefault();
    }
}
