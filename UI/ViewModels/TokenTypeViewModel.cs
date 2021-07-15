using System.Collections.Generic;
using ZSpitz.Util.Wpf;
using static ZSpitz.Util.Functions;

namespace ParseTreeVisualizer {
    public class TokenTypeViewModel : Selectable<KeyValuePair<int,string>> {
        public int Index { get; }
        public string Text { get; }

        public TokenTypeViewModel(int index, string text): this(KVP(index,text)) { }

        public TokenTypeViewModel(KeyValuePair<int, string> tokenType) : base(tokenType) => 
            (Index, Text) = (tokenType.Key, tokenType.Value);
    }
}
