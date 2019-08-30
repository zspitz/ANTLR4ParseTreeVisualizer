using Antlr4.Runtime.Tree;
using Microsoft.VisualStudio.DebuggerVisualizers;
using ParseTreeVisualizer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _visualizerTest {
    class Program {
        [STAThread]
        static void Main(string[] args) {
            IParseTree tree = null;

            var visualizerHost = new VisualizerDevelopmentHost(tree, typeof(Visualizer), typeof(ObjectSource));
            visualizerHost.ShowVisualizer();
        }
    }
}
