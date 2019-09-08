﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Microsoft.VisualStudio.DebuggerVisualizers;
using System.Windows;
using static System.Windows.SystemColors;
using ParseTreeVisualizer.Util;

[assembly: DebuggerVisualizer(
    visualizer: typeof(ParseTreeVisualizer.Visualizer),
    visualizerObjectSource: typeof(ParseTreeVisualizer.ObjectSource),
    Target = typeof(Antlr4.Runtime.RuleContext),
    Description = "ANTLR4 Parse Tree Visualizer")]

namespace ParseTreeVisualizer {
    public class Visualizer : DialogDebuggerVisualizer {
        protected override void Show(IDialogVisualizerService windowService, IVisualizerObjectProvider objectProvider) {
            if (windowService == null) { throw new ArgumentNullException(nameof(windowService)); }

            var window = new VisualizerWindow();
            // When a control loses focus, it should look no different from when it had the focus (e.g. selection color)
            window.Resources[InactiveSelectionHighlightBrushKey] = HighlightBrush;
            window.Resources[InactiveSelectionHighlightTextBrushKey] = HighlightTextBrush;

            VisualizerConfig.AssemblyName = objectProvider.GetObject() as string;

            var content = window.Content as VisualizerControl;
            content.Config = VisualizerConfig.Get();
            content.objectProvider = objectProvider;

            window.ShowDialog();
        }
    }
}
