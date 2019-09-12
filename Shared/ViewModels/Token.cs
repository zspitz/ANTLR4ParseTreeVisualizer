using Antlr4.Runtime.Tree;
using ParseTreeVisualizer.Util;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace ParseTreeVisualizer.ViewModels {
    [Serializable]
    public class Token : INotifyPropertyChanged {
        public int Index { get; }
        public string TokenType { get; }
        public int TokenTypeID { get; }
        public int Line { get; }
        public int Col { get; }
        public string Text { get; }
        public bool IsError { get; }
        public (int start, int stop) Span { get; }

        private bool isSelected;
        public bool IsSelected {
            get => isSelected;
            set => this.NotifyChanged(ref isSelected, value, args => PropertyChanged?.Invoke(this, args));
        }

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
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
