using ParseTreeVisualizer.Serialization;
using Periscope;
using Periscope.Debuggee;
using System.Diagnostics;

[assembly: DebuggerVisualizer(
    visualizer: typeof(ParseTreeVisualizer.Visualizer),
    visualizerObjectSource: typeof(ParseTreeVisualizer.VisualizerDataObjectSource),
    Target = typeof(Antlr4.Runtime.RuleContext),
#if ANTLR_LEGACY
    Description = "ANTLR4 ParseTree Visualizer (Legacy)")]
#else
    Description = "ANTLR4 ParseTree Visualizer")]
#endif 

[assembly: DebuggerVisualizer(
    visualizer: typeof(ParseTreeVisualizer.Visualizer),
    visualizerObjectSource: typeof(ParseTreeVisualizer.VisualizerDataObjectSource),
    Target = typeof(string),
#if ANTLR_LEGACY
    Description = "ANTLR4 ParseTree Visualizer (Legacy)")]
#else
    Description = "ANTLR4 ParseTree Visualizer")]
#endif 

[assembly: DebuggerVisualizer(
    visualizer: typeof(ParseTreeVisualizer.Visualizer),
    visualizerObjectSource: typeof(ParseTreeVisualizer.VisualizerDataObjectSource),
    Target = typeof(Antlr4.Runtime.BufferedTokenStream),
#if ANTLR_LEGACY
    Description = "ANTLR4 ParseTree Visualizer (Legacy)")]
#else
    Description = "ANTLR4 ParseTree Visualizer")]
#endif 

namespace ParseTreeVisualizer {
    public abstract class VisualizerWindowBase : VisualizerWindowBase<VisualizerWindow, Config> { };

    public class Visualizer : VisualizerBase<VisualizerWindow, Config> {
        static Visualizer() => SubfolderAssemblyResolver.Hook(
#if ANTLR_LEGACY
            "ParseTreeVisualizer.Legacy"
#else
            "ParseTreeVisualizer.Standard"
#endif
        );

        public Visualizer() : base(new GithubProjectInfo("zspitz", "antlr4parsetreevisualizer")) { }
    }
}
