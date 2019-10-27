using System;
using System.IO;
using Antlr4.Runtime;
using Microsoft.VisualStudio.DebuggerVisualizers;
using ParseTreeVisualizer;

namespace _visualizerTestCore {
    class Program {
        [STAThread]
        static void Main(string[] args) {
            var input = new AntlrInputStream("hello world");
            var lexer = new HelloLexer(input);
            var tokens = new CommonTokenStream(lexer);
            var parser = new HelloParser(tokens);
            var tree = parser.r();

            var visualizerHost = new VisualizerDevelopmentHost(tree, typeof(Visualizer), typeof(ObjectSource));
            visualizerHost.ShowVisualizer();
        }
    }
}
