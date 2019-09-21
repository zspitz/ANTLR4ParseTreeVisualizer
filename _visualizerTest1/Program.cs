using Antlr4.Runtime;
using Antlr4.Runtime.Atn;
using Rubberduck.Parsing.Grammar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _visualizerTest1 {
    class Program {
        static void Main(string[] args) {
            var code = @"
Public Sub DoSomething()
    Debug.Print ""hi from path 1""
    If True Then
        MsgBox ""hello from path 2""
    End If
    Debug.Print ""still in path 1""
End Sub".Trim();
            var stream = new AntlrInputStream(code);
            var lexer = new VBALexer(stream);
            var tokens = new CommonTokenStream(lexer);
            var parser = new VBAParser(tokens);
            parser.Interpreter.PredictionMode = PredictionMode.Sll;
            var tree = parser.subStmt();

            Console.ReadKey(true);
        }
    }
}
