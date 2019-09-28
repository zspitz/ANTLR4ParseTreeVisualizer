using ParseTreeVisualizer.Util;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using static System.IO.Path;

namespace ParseTreeVisualizer {
    public class ConfigViewModel : ViewModelBase<Config> {
        readonly Config _originalValues;

        public ConfigViewModel(Config config, Dictionary<int, string> tokenTypeMapping, List<ClassInfo> usedRuleContexts) : base(config.Clone()) {
            this.tokenTypeMapping = tokenTypeMapping;
            this.usedRuleContexts = usedRuleContexts;
            _originalValues = config;
        }

        private Dictionary<int, string> tokenTypeMapping;
        private List<ClassInfo> usedRuleContexts;

        private ReadOnlyCollection<TokenTypeViewModel> tokenTypes;
        public ReadOnlyCollection<TokenTypeViewModel> TokenTypes {
            get {
                if (tokenTypeMapping == null) { return null; }
                if (tokenTypes == null) {
                    tokenTypes = tokenTypeMapping.SelectKVP((index, text) => {
                        var ret = new TokenTypeViewModel(index, text) {
                            IsSelected = index.In(Model.SelectedTokenTypes)
                        };
                        ret.PropertyChanged += (s,e) => {
                            if (e.PropertyName == "IsSelected") {
                                Model.SelectedTokenTypes.AddRemove(ret.IsSelected, ret.Index);
                            }
                        };
                        return ret;
                    }).OrderBy(x => x.Text).ToList().AsReadOnly();
                }
                return tokenTypes;
            }
        }

        private ReadOnlyCollection<Selectable<ClassInfo>> ruleContexts;
        public ReadOnlyCollection<Selectable<ClassInfo>> RuleContexts {
            get {
                if (usedRuleContexts == null) { return null; }
                if (ruleContexts == null) {
                    ruleContexts = usedRuleContexts.Select(x => {
                        var ret = new Selectable<ClassInfo>(x) {
                            IsSelected = x.FullName.In(Model.SelectedRuleContexts)
                        };
                        ret.PropertyChanged += (s, e) => {
                            if (e.PropertyName == "IsSelected") {
                                Model.SelectedRuleContexts.AddRemove(ret.IsSelected, x.FullName);
                            }
                        };
                        return ret;
                    }).ToList().AsReadOnly();
                }
                return ruleContexts;
            }
        }

        public string Version => GetType().Assembly.GetName().Version.ToString();
        public string Location => GetType().Assembly.Location;
        public string Filename => GetFileName(Location);

        public bool IsDirty {
            get {
                var m = Model;
                var o = _originalValues;
                return
                    o.SelectedParserName != m.SelectedParserName ||
                    o.SelectedLexerName != m.SelectedLexerName ||
                    o.ShowTextTokens != m.ShowTextTokens ||
                    o.ShowErrorTokens != m.ShowErrorTokens ||
                    o.ShowWhitespaceTokens != m.ShowWhitespaceTokens ||
                    o.ShowTreeErrorTokens != m.ShowTreeErrorTokens ||
                    o.ShowTreeTextTokens != m.ShowTreeTextTokens ||
                    o.ShowTreeWhitespaceTokens != m.ShowTreeWhitespaceTokens ||
                    o.ShowRuleContextNodes != m.ShowRuleContextNodes ||

                    !o.SelectedTokenTypes.SetEquals(m.SelectedTokenTypes) ||
                    !o.SelectedRuleContexts.SetEquals(m.SelectedRuleContexts);
            }
        }

        public string WatchBaseExpression { get; set; }
    }
}
