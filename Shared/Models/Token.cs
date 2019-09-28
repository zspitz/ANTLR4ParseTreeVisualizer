using Antlr4.Runtime.Tree;
using ParseTreeVisualizer.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParseTreeVisualizer {
    [Serializable]
    public class Token {
        public int Index { get; }
        public string TokenType { get; }
        public int TokenTypeID { get; }
        public int Line { get; }
        public int Col { get; }
        public string Text { get; }
        public bool IsError { get; }
        public (int start, int stop) Span { get; }
        public bool IsWhitespace { get; }
        public Token(TerminalNodeImpl terminalNode, Dictionary<int,string> tokenTypeMapping) {
            Index = terminalNode.Payload.TokenIndex;

            TokenTypeID = terminalNode.Payload.Type;

            TokenType =
                tokenTypeMapping?[TokenTypeID] ??
                TokenTypeID.ToString();

            Line = terminalNode.Payload.Line;
            Col = terminalNode.Payload.Column;
            Text = terminalNode.Payload.Text.ToCSharpLiteral(false);
            if (terminalNode is ErrorNodeImpl) {
                IsError = true;
            }

            Span = (terminalNode.Payload.StartIndex, terminalNode.Payload.StopIndex);

            IsWhitespace = terminalNode.Payload.Text.IsNullOrWhitespace();
        }
    }
}
