﻿namespace ParseTreeVisualizer.Serialization {
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
