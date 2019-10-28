using Antlr4.Runtime;
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

        public Token(IToken itoken, Dictionary<int,string> tokenTypeMapping) {
            Index = itoken.TokenIndex;
            TokenTypeID = itoken.Type;
            Line = itoken.Line;
            Col = itoken.Column;
            Text = itoken.Text.ToCSharpLiteral(false);
            Span = (itoken.StartIndex, itoken.StopIndex);
            IsWhitespace = itoken.Text.IsNullOrWhitespace();

            string tokenType = null;
            TokenType =
                tokenTypeMapping?.TryGetValue(TokenTypeID, out tokenType) ?? false ?
                tokenType :
                $"{TokenTypeID}";
        }

        public Token(TerminalNodeImpl terminalNode, Dictionary<int,string> tokenTypeMapping) : this(terminalNode.Payload, tokenTypeMapping) {
            if (terminalNode is ErrorNodeImpl) {
                IsError = true;
            }
        }

        public bool ShowToken(Config config) {
            if (!config.HasTokenListFilter()) { return true; }
            var showToken = false;

            if (!IsError && !IsWhitespace) {
                showToken = config.ShowTextTokens;
            } else {
                showToken =
                    (IsError ? config.ShowErrorTokens : true) &&
                    (IsWhitespace ? config.ShowWhitespaceTokens : true);
            }

            showToken &= config.SelectedTokenTypes.None() || TokenTypeID.In(config.SelectedTokenTypes);

            return showToken;
        }
    }
}
