using Antlr4.Runtime;
using System;
using System.IO;

[assembly: System.CLSCompliant(false)]

namespace _visualizerTestCore1 {
    public class ErrorListener<S> : ConsoleErrorListener<S> {
        public bool hasError = false;

        public override void SyntaxError(TextWriter output, IRecognizer recognizer, S offendingSymbol, int line, int charPositionInLine, string msg, RecognitionException e) {
            hasError = true;
            base.SyntaxError(output, recognizer, offendingSymbol, line, charPositionInLine, msg, e);
        }
    }

    class Program {
        static void Main(string[] args) {
            var input = new AntlrInputStream("hello world");
            var lexer = new HelloLexer(input);
            var tokens = new CommonTokenStream(lexer);
            var parser = new HelloParser(tokens);
            var listener = new ErrorListener<IToken>();
            parser.AddErrorListener(listener);
            var tree = parser.r();


        }
    }
}
