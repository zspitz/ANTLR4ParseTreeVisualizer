using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ParseTreeVisualizer.Util;

namespace ParseTreeVisualizer {
    [Serializable]
    public class TerminalNodeImplVM {
        public int Index { get; }
        public string TokenType { get; }
        public int Line { get; }
        public int Col { get; }
        public string Text { get; }
        public bool IsError { get; }
        public (int start, int stop) Span { get; }
        public TerminalNodeImplVM(TerminalNodeImpl terminalNode, Dictionary<int,string> tokenTypeMapping) {
            Index = terminalNode.Payload.TokenIndex;

            TokenType = 
                tokenTypeMapping?[terminalNode.Payload.Type] ??
                terminalNode.Payload.Type.ToString();

            Line = terminalNode.Payload.Line;
            Col = terminalNode.Payload.Column;
            Text = terminalNode.Payload.Text.ToCSharpLiteral(false);
            if (terminalNode is ErrorNodeImpl) {
                IsError = true;
            }

            Span = (terminalNode.Payload.StartIndex, terminalNode.Payload.StopIndex);
        }
    }
}
