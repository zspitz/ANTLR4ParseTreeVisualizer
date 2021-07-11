using System;
using System.Collections.Generic;
using ParseTreeVisualizer.Util;
using static ParseTreeVisualizer.Util.Functions;

namespace ParseTreeVisualizer {
    public class TokenTypeViewModel : Selectable<KeyValuePair<int,string>> {
        public int Index { get; }
        public string Text { get; }

        public TokenTypeViewModel(int index, string text): this(KVP(index,text)) { }

        public TokenTypeViewModel(KeyValuePair<int, string> tokenType) : base(tokenType) => 
            (Index, Text) = tokenType;
    }
}
