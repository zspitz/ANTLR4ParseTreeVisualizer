using System;
using System.IO;
using Antlr4.Runtime;
using Microsoft.VisualStudio.DebuggerVisualizers;
using ParseTreeVisualizer;

[assembly: System.CLSCompliant(false)]

namespace _visualizerTestCore {
    class Program {
        [STAThread]
        static void Main(string[] args) {
            var code = "hello world";

            var visualizerHost = new VisualizerDevelopmentHost(code, typeof(Visualizer), typeof(ObjectSource));
            visualizerHost.ShowVisualizer();

            var input = new AntlrInputStream(code);
            var lexer = new HelloLexer(input);
            var tokens = new CommonTokenStream(lexer);

            visualizerHost = new VisualizerDevelopmentHost(tokens, typeof(Visualizer), typeof(ObjectSource));
            visualizerHost.ShowVisualizer();

            var parser = new HelloParser(tokens);
            var tree = parser.r();

            visualizerHost = new VisualizerDevelopmentHost(tree, typeof(Visualizer), typeof(ObjectSource));
            visualizerHost.ShowVisualizer();
        }
    }
}
