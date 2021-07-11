using Periscope.Debuggee;
using System;
using System.Collections.Generic;
using ZSpitz.Util;

namespace ParseTreeVisualizer.Serialization {
    [Serializable]
#if VISUALIZER_DEBUGGEE
    public class Config : Periscope.Debuggee.ConfigBase<Config> {
#else
    public class Config {
#endif

        public string? SelectedParserName { get; set; }
        public string? ParseTokensWithRule { get; set; }
        public string? SelectedLexerName { get; set; }
        public bool ShowTextTokens { get; set; } = true;
        public bool ShowWhitespaceTokens { get; set; } = true;
        public bool ShowErrorTokens { get; set; } = true;
        public HashSet<int> SelectedTokenTypes { get; } = new HashSet<int>();
        public bool ShowTreeTextTokens { get; set; } = true;
        public bool ShowTreeWhitespaceTokens { get; set; } = true;
        public bool ShowTreeErrorTokens { get; set; } = true;
        public bool ShowRuleContextNodes { get; set; } = true;
        public HashSet<string?> SelectedRuleContexts { get; } = new HashSet<string?>();
        public string? RootNodePath { get; set; }

        public bool HasTreeFilter() => !(ShowTreeErrorTokens && ShowTreeWhitespaceTokens && ShowTreeTextTokens && ShowRuleContextNodes && SelectedRuleContexts.None());
        public bool HasTokenListFilter() => !(ShowTextTokens && ShowErrorTokens && ShowWhitespaceTokens && SelectedTokenTypes.None());

#if VISUALIZER_DEBUGGEE
        // TODO should any parts of the config return ConfigDiffStates.NeedsWrite?
        public override ConfigDiffStates Diff(Config baseline) =>
            (
                baseline.SelectedParserName == SelectedParserName &&
                baseline.SelectedLexerName == SelectedLexerName &&
                baseline.ShowTextTokens == ShowTextTokens &&
                baseline.ShowErrorTokens == ShowErrorTokens &&
                baseline.ShowWhitespaceTokens == ShowWhitespaceTokens &&
                baseline.ShowTreeTextTokens == ShowTreeTextTokens &&
                baseline.ShowTreeErrorTokens == ShowTreeErrorTokens &&
                baseline.ShowTreeWhitespaceTokens == ShowTreeWhitespaceTokens &&
                baseline.ShowRuleContextNodes == ShowRuleContextNodes &&
                baseline.ParseTokensWithRule == ParseTokensWithRule &&
                baseline.SelectedTokenTypes.SetEquals(SelectedTokenTypes) &&
                baseline.SelectedRuleContexts.SetEquals(SelectedRuleContexts)
            ) ? ConfigDiffStates.NoAction : ConfigDiffStates.NeedsTransfer;

        public override Config Clone() {
#else
        public Config Clone() {
#endif
            var ret = new Config {
                SelectedParserName = SelectedParserName,
                SelectedLexerName = SelectedLexerName,
                ShowTextTokens = ShowTextTokens,
                ShowErrorTokens = ShowErrorTokens,
                ShowWhitespaceTokens = ShowWhitespaceTokens,
                ShowTreeTextTokens = ShowTreeTextTokens,
                ShowTreeErrorTokens = ShowTreeErrorTokens,
                ShowTreeWhitespaceTokens = ShowTreeWhitespaceTokens,
                ShowRuleContextNodes = ShowRuleContextNodes,
                ParseTokensWithRule = ParseTokensWithRule
            };
            SelectedTokenTypes.AddRangeTo(ret.SelectedTokenTypes);
            SelectedRuleContexts.AddRangeTo(ret.SelectedRuleContexts);
            return ret;
        }
    }
}
