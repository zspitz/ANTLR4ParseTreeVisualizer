using System;
using System.Diagnostics;
using Microsoft.VisualStudio.DebuggerVisualizers;
using ParseTreeVisualizer.Util;
using Microsoft.Xaml.Behaviors;

[assembly: DebuggerVisualizer(
    visualizer: typeof(ParseTreeVisualizer.Visualizer),
    visualizerObjectSource: typeof(ParseTreeVisualizer.ObjectSource),
    Target = typeof(Antlr4.Runtime.RuleContext),
    Description = "ANTLR4 Parse Tree Visualizer")]

[assembly: DebuggerVisualizer(
    visualizer: typeof(ParseTreeVisualizer.Visualizer),
    visualizerObjectSource: typeof(ParseTreeVisualizer.ObjectSource),
    Target = typeof(string),
    Description = "ANTLR4 Parse Tree Visualizer")]

[assembly: DebuggerVisualizer(
    visualizer: typeof(ParseTreeVisualizer.Visualizer),
    visualizerObjectSource: typeof(ParseTreeVisualizer.ObjectSource),
    Target = typeof(Antlr4.Runtime.BufferedTokenStream),
    Description = "ANTLR4 Parse Tree Visualizer")]


namespace ParseTreeVisualizer {
    public class Visualizer : DialogDebuggerVisualizer {
        protected override void Show(IDialogVisualizerService windowService, IVisualizerObjectProvider objectProvider) {
            if (windowService == null) { throw new ArgumentNullException(nameof(windowService)); }

            // HACK we need this to force the Microsoft.Xaml.Behaviors dll to be loaded
            // https://github.com/zspitz/ANTLR4ParseTreeVisualizer/issues/35
            var t = new EventTrigger();

            PresentationTraceSources.DataBindingSource.Listeners.Add(new DebugTraceListener());

            Config.AssemblyName = objectProvider.GetObject() as string;

            var window = new VisualizerWindow();
            var content = window.Content as VisualizerControl;
            content.Config = Config.Get();
            content.objectProvider = objectProvider;

            window.ShowDialog();
        }
    }
}
