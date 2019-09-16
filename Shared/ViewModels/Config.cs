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
        public HashSet<int> SelectedTokenTypes { get; set; } = new HashSet<int>();
        public bool ShowTokenTreeNodes { get; set; } = true;
        public HashSet<int> SelectedRuleIDs { get; set; } = new HashSet<int>();

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
            SelectedTokenTypes = SelectedTokenTypes.Select().ToHashSet(),
            _originalValues = this,
            TokenTypes = TokenTypes?.Select().ToList(),
            ShowTextTokens = ShowTextTokens,
            ShowErrorTokens = ShowErrorTokens,
            ShowWhitespaceTokens = ShowWhitespaceTokens,
            ShowTokenTreeNodes = ShowTokenTreeNodes,
            SelectedRuleIDs = SelectedRuleIDs.Select().ToHashSet()
        };

        public bool? ShouldTriggerReload() {
            if (_originalValues == null) { return null; }

            if (TokenTypes != null) {
                SelectedTokenTypes = TokenTypes.Where(x => x.IsSelected).Select(x => x.Index).ToHashSet();
            }

            return _originalValues.SelectedParserName != SelectedParserName ||
                _originalValues.SelectedLexerName != SelectedLexerName ||
                !_originalValues.SelectedTokenTypes.SetEquals(SelectedTokenTypes) ||
                _originalValues.ShowTextTokens != ShowTextTokens ||
                _originalValues.ShowErrorTokens != ShowErrorTokens ||
                _originalValues.ShowWhitespaceTokens != ShowWhitespaceTokens ||
                _originalValues.ShowTokenTreeNodes != ShowTokenTreeNodes ||
                !_originalValues.SelectedRuleIDs.SetEquals(SelectedRuleIDs);
        }

        // TokenTypes is on the Config and not on TreeVisualizer (like other state information) because TokenType also manages selection
        // We want to be able to clone the config, make changes to the clone, and then cancel by discarding the clone
        [JsonIgnore]
        public List<TokenType> TokenTypes { get; set; }
    }

    internal class ConfigContractResolver : DefaultContractResolver {
        private static readonly string[] globalNames = new[] { "" };
        public bool ForGlobal { get; set; } = true;

        protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization) {
            var ret = base.CreateProperties(type, memberSerialization);
            var predicate = ForGlobal ?
                x => x.PropertyName.In(globalNames) :
                (Func<JsonProperty, bool>)(x => x.PropertyName.NotIn(globalNames));
            ret = ret.Where(predicate).ToList();
            return ret;
        }
    }
}
