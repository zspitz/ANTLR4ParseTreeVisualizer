using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using ParseTreeVisualizer.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using static System.IO.Path;

namespace ParseTreeVisualizer.ViewModels {
    [Serializable]
    [Obsolete("Use ParseTreeVisualizer.Config or ParseTreeVisualizer.ConfigViewModel")]
    public class Config {
        public static string AssemblyName { get; set; }

        private static readonly string ConfigFolder = Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "ANTLR4VisualizerParser"
        );

        private static string ConfigPath(bool globals = true) =>
            Combine(
                ConfigFolder,
                globals ? "config.json" : $"{AssemblyName}.json"
            );

        public static Config Get() {
            var ret = new JObject();
            string fileText = null;

            if (Directory.Exists(ConfigFolder)) {
                var testPath = ConfigPath(true);
                if (File.Exists(testPath)) {
                    //var globals = JObject.Parse(File.ReadAllText(testPath));

                    // TODO load globals here

                }

                testPath = ConfigPath(false);
                if (File.Exists(testPath)) {
                    try {
                        fileText = File.ReadAllText(testPath);
                    } catch { }
                }
                if (!fileText.IsNullOrWhitespace()) {
                    ret.Merge(JObject.Parse(fileText));
                }
            }

            return ret.ToObject<Config>();
        }

        private Config() { }

        public string SelectedParserName { get; set; }
        public string SelectedLexerName { get; set; }
        public bool ShowTextTokens { get; set; } = true;
        public bool ShowWhitespaceTokens { get; set; } = true;
        public bool ShowErrorTokens { get; set; } = true;

        private HashSet<int> selectedTokenTypes = new HashSet<int>();
        public HashSet<int> SelectedTokenTypes {
            get {
                if (tokenTypes != null) {
                    selectedTokenTypes.Clear();
                    tokenTypes.Where(x => x.IsSelected).Select(x => x.Index).AddRangeTo(selectedTokenTypes);
                }
                return selectedTokenTypes;
            }
        }

        public bool ShowTreeTextTokens { get; set; } = true;
        public bool ShowTreeWhitespaceTokens { get; set; } = true;
        public bool ShowTreeErrorTokens { get; set; } = true;
        public bool ShowRuleContextNodes { get; set; } = true;

        public bool HasTreeFilter() => !(ShowTreeErrorTokens && ShowTreeWhitespaceTokens && ShowTreeTextTokens && ShowRuleContextNodes && SelectedRuleContexts.None());

        private HashSet<string> selectedRuleContexts = new HashSet<string>();
        public HashSet<string> SelectedRuleContexts {
            get {
                if (ruleContextsViewModel != null) {
                    selectedRuleContexts.Clear();
                    ruleContextsViewModel.Where(x => x.IsSelected).Select(x => x.FullName).AddRangeTo(selectedRuleContexts);
                }
                return selectedRuleContexts;
            }
        }

        public string RootNodePath { get; set; }

        [NonSerialized]
        [JsonIgnore]
        private Config _originalValues;

        public void Write() {
            if (!Directory.Exists(ConfigFolder)) {
                try {
                    Directory.CreateDirectory(ConfigFolder);
                } catch {
                    // we can't create the directory, so we can't write the configuration
                    return;
                }
            }

            // write global
            var resolver = new ConfigContractResolver { ForGlobal = true };
            var settings = new JsonSerializerSettings {
                ContractResolver = resolver,
                Formatting = Formatting.Indented
            };
            var toWrite = JsonConvert.SerializeObject(this, settings);
            try {
                File.WriteAllText(ConfigPath(true), toWrite);
            } catch { }

            // write assembly-specific
            resolver = new ConfigContractResolver { ForGlobal = false };
            settings = new JsonSerializerSettings {
                ContractResolver = resolver,
                Formatting = Formatting.Indented
            };
            toWrite = JsonConvert.SerializeObject(this, settings);
            try {
                File.WriteAllText(ConfigPath(false), toWrite);
            } catch { }
        }

        public Config Clone() => new Config {
            SelectedParserName = SelectedParserName,
            SelectedLexerName = SelectedLexerName,
            selectedTokenTypes = SelectedTokenTypes.Select().ToHashSet(),
            _originalValues = this,
            ShowTextTokens = ShowTextTokens,
            ShowErrorTokens = ShowErrorTokens,
            ShowWhitespaceTokens = ShowWhitespaceTokens,
            ShowTreeTextTokens = ShowTreeTextTokens,
            ShowTreeErrorTokens = ShowTreeErrorTokens,
            ShowTreeWhitespaceTokens = ShowTreeWhitespaceTokens,
            ShowRuleContextNodes = ShowRuleContextNodes,
            TokenTypeMapping = TokenTypeMapping,
            selectedRuleContexts = SelectedRuleContexts.Select().ToHashSet(),
            UsedRuleContexts = UsedRuleContexts
        };

        public bool? ShouldTriggerReload() {
            if (_originalValues == null) { return null; }

            // force load selection changes from the viewmodel objects
            var selectedTokenTypes = SelectedTokenTypes;
            var selectedRuleContexts = SelectedRuleContexts;

            return
                !_originalValues.SelectedRuleContexts.SetEquals(selectedRuleContexts) ||
                !_originalValues.SelectedTokenTypes.SetEquals(selectedTokenTypes) ||
                _originalValues.SelectedParserName != SelectedParserName ||
                _originalValues.SelectedLexerName != SelectedLexerName ||
                _originalValues.ShowTextTokens != ShowTextTokens ||
                _originalValues.ShowErrorTokens != ShowErrorTokens ||
                _originalValues.ShowWhitespaceTokens != ShowWhitespaceTokens ||
                _originalValues.ShowTreeErrorTokens != ShowTreeErrorTokens ||
                _originalValues.ShowTreeTextTokens != ShowTreeTextTokens ||
                _originalValues.ShowTreeWhitespaceTokens != ShowTreeWhitespaceTokens ||
                _originalValues.ShowRuleContextNodes != ShowRuleContextNodes;
        }

        [JsonIgnore]
        public Dictionary<int, string> TokenTypeMapping { private get; set; }

        // TokenTypes is on the Config and not on TreeVisualizer (like other state information) because we only need it as a source for databinding
        // We also need to be able to discard the cloned config.
        // Everything else here should really be on the ViewModel, not the Model -- https://github.com/zspitz/ANTLR4ParseTreeVisualizer/issues/23#issuecomment-532818529
        [NonSerialized]
        [JsonIgnore]
        private List<TokenType> tokenTypes;

        [JsonIgnore]
        public List<TokenType> TokenTypes {
            get {
                if (tokenTypes == null) {
                    tokenTypes = TokenTypeMapping.SelectKVP((id, text) => new TokenType(id, text) {
                        IsSelected = id.In(SelectedTokenTypes)
                    }).OrderBy(x => x.Text).ToList();
                }
                return tokenTypes;
            }
        }

        [JsonIgnore]
        public List<ParseRuleContextInfo> UsedRuleContexts { private get; set; }

        [NonSerialized]
        [JsonIgnore]
        private List<ParseRuleContextInfo> ruleContextsViewModel;

        [JsonIgnore]
        public List<ParseRuleContextInfo> RuleContextsViewModel {
            get {
                if (UsedRuleContexts == null) { return null; }
                if (ruleContextsViewModel == null) {
                    ruleContextsViewModel = UsedRuleContexts.Select(x => {
                        var ret = x.Clone();
                        ret.IsSelected = ret.FullName.In(selectedRuleContexts);
                        return ret;
                    }).ToList();
                }
                return ruleContextsViewModel;
            }
        }

        [JsonIgnore]
        public string Version => GetType().Assembly.GetName().Version.ToString();
        [JsonIgnore]
        public string Location => GetType().Assembly.Location;
        [JsonIgnore]
        public string Filename => GetFileName(Location);

        [JsonIgnore]
        public string WatchBaseExpression { get; set; }
    }
}
