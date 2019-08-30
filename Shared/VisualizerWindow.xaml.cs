using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ParseTreeVisualizer {
    public partial class VisualizerWindow : Window {
        public VisualizerWindow() {
            InitializeComponent();

            PreviewKeyDown += (s, e) => {
                if (e.Key == Key.Escape) { Close(); }
            };
        }
    }
}
