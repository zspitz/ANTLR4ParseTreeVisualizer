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
        public TerminalNodeImplVM(TerminalNodeImpl terminalNode) {
            Index = terminalNode.Payload.TokenIndex;
            TokenType = terminalNode.Payload.Type.ToString();
            Line = terminalNode.Payload.Line;
            Col = terminalNode.Payload.Column;
            Text = terminalNode.Payload.Text;
            if (terminalNode is ErrorNodeImpl) {
                IsError = true;
            }
        }
    }
}
