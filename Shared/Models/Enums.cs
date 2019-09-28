using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParseTreeVisualizer {
    public enum TreeNodeType {
        RuleContext,
        Token,
        ErrorToken,
        WhitespaceToken,
        Placeholder
    }

    public enum FilterStates {
        NotMatched,
        Matched,
        DescendantMatched
    }
}
