using Antlr4.Runtime;
using Antlr4.Runtime.Atn;
using Antlr4.Runtime.Tree;
using Microsoft.VisualStudio.DebuggerVisualizers;
using ParseTreeVisualizer;
using ParseTreeVisualizer.ViewModels;
using Rubberduck.Parsing.Grammar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _visualizerTest {
    class Program {
        [STAThread]
        static void Main(string[] args) {
            var code = @"Public Sub DoSomething()
    Debug.Print ""hi from path 1""
    If True Then
        MsgBox ""hello from path 2""
    End If
    Debug.Print ""still in path 1""
End Sub";
            var stream = new AntlrInputStream(code);
            var lexer = new VBALexer(stream);
            var tokens = new CommonTokenStream(lexer);
            var parser = new VBAParser(tokens);
            parser.Interpreter.PredictionMode = PredictionMode.Sll;
            var tree = parser.subStmt();
            
            var visualizerHost = new VisualizerDevelopmentHost(tree, typeof(Visualizer), typeof(ObjectSource));
            visualizerHost.ShowVisualizer();
        }
    }
}
