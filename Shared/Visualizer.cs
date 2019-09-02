using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Microsoft.VisualStudio.DebuggerVisualizers;
using System.Windows;

[assembly: DebuggerVisualizer(
    visualizer: typeof(ParseTreeVisualizer.Visualizer),
    visualizerObjectSource: typeof(ParseTreeVisualizer.ObjectSource),
    Target = typeof(Antlr4.Runtime.RuleContext),
    Description = "ANTLR4 Parse Tree Visualizer")]

namespace ParseTreeVisualizer {
    public class Visualizer : DialogDebuggerVisualizer {
        protected override void Show(IDialogVisualizerService windowService, IVisualizerObjectProvider objectProvider) {
            if (windowService == null) { throw new ArgumentNullException(nameof(windowService)); }

            var window = new VisualizerWindow {
                DataContext = objectProvider.GetObject()
            };

            var t = typeof(SystemColors);
            var inactive = t.GetProperties().Where(x => x.Name.StartsWith("Inactive") && !x.Name.EndsWith("Key")).Select(x => new {
                key = t.GetProperty(x.Name + "Key").GetValue(null),
                activePropertyValue = (t.GetProperty(x.Name.Replace("Inactive", "Active")) ?? t.GetProperty(x.Name.Replace("InactiveSelection", ""))).GetValue(null)
            }).ToList();
            foreach (var x in inactive) {
                window.Resources[x.key] = x.activePropertyValue;
            }

            window.ShowDialog();
        }
    }
}
