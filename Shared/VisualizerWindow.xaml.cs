using ParseTreeVisualizer.Util;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using static System.Windows.SystemColors;

namespace ParseTreeVisualizer {
    public partial class VisualizerWindow : Window {
        public VisualizerWindow() {
            InitializeComponent();

            // When a control loses focus, it should look no different from when it had the focus (e.g. selection color)
            Resources[InactiveSelectionHighlightBrushKey] = HighlightBrush;
            Resources[InactiveSelectionHighlightTextBrushKey] = HighlightTextBrush;

            // if we could find out which is the current monitor, that would be better
            var workingAreas = Monitor.AllMonitors.Select(x => x.WorkingArea).ToList();
            MaxWidth = workingAreas.Min(x => x.Width) * .90;
            MaxHeight = workingAreas.Min(x => x.Height) * .90;

            PreviewKeyDown += (s, e) => {
                if (e.Key == Key.Escape) { Close(); }
            };
        }
    }
}
