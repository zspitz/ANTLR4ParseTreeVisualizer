using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ParseTreeVisualizer {
    public partial class VisualizerControl {
        public VisualizerControl() {
            InitializeComponent();

            tokens.AutoGeneratingColumn += (s, e) => {
                if (e.PropertyName == "IsError") { e.Cancel = true; }
            };
        }
    }
}
