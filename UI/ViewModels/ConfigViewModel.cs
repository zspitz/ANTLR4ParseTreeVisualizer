using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using ZSpitz.Util.Wpf;
using ParseTreeVisualizer.Serialization;
using ZSpitz.Util;

namespace ParseTreeVisualizer {
    public class ConfigViewModel : ViewModelBase<Config> {
        public ConfigViewModel(VisualizerData vd) : 
            this(vd.Config, vd.TokenTypeMapping, vd.UsedRuleContexts, vd.AvailableLexers, vd.AvailableParsers, vd.CanSelectLexer, vd.CanSelectParser) { }

        public ConfigViewModel(
                    Config config, 
                    Dictionary<int, string>? tokenTypeMapping, 
                    List<ClassInfo>? ruleContexts, 
                    List<ClassInfo> lexers, 
                    List<ClassInfo> parsers,
                    bool canSelectLexer,
                    bool canSelectParser
                ) : base(config) {

            TokenTypes = tokenTypeMapping?.SelectKVP((index, text) => {
                var ret = new TokenTypeViewModel(index, text) {
                    IsSelected = index.In(Model.SelectedTokenTypes)
                };
                ret.PropertyChanged += (s, e) => {
                    if (e.PropertyName == "IsSelected") {
                        Model.SelectedTokenTypes.AddRemove(ret.IsSelected, ret.Index);
                    }
                };
                return ret;
            }).OrderBy(x => x.Text).ToList().AsReadOnly();

            RuleContexts = ruleContexts?.Select(x => {
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

            AvailableLexers = getVMList(lexers);
            AvailableParsers = getVMList(parsers);
            CanSelectLexer = canSelectLexer;
            CanSelectParser = canSelectParser;
        }

        private ReadOnlyCollection<ClassInfo> getVMList(List<ClassInfo> models) {
            var lst = models.OrderBy(x => x.Name).ToList();
            lst.Insert(0, ClassInfo.None);
            return lst.AsReadOnly();
        }

        public ReadOnlyCollection<TokenTypeViewModel>? TokenTypes { get; }
        public ReadOnlyCollection<Selectable<ClassInfo>>? RuleContexts { get; }
        public ReadOnlyCollection<ClassInfo> AvailableParsers { get; }
        public ReadOnlyCollection<ClassInfo> AvailableLexers { get; }

        public bool CanSelectLexer { get; private set; }
        public bool CanSelectParser { get; private set; }
    }
}
