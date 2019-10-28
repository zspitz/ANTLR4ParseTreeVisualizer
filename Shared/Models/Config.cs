using System;
using System.IO;
using static System.IO.Path;
using Newtonsoft.Json.Linq;
using ParseTreeVisualizer.Util;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Linq;

namespace ParseTreeVisualizer {
    [Serializable]
    public class Config {
        public static string AssemblyName { get; set; }

        private static readonly string ConfigFolder = Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "ANTLR4VisualizerParser"
        );

        private static string configPath(bool globals = true) =>
            Combine(
                ConfigFolder,
                globals ? "config.json" : $"{AssemblyName}.json"
            );

        public static Config Get() {
            var ret = new JObject();
            string fileText = null;

            if (Directory.Exists(ConfigFolder)) {
                var testPath = configPath(true);
                if (File.Exists(testPath)) {
                    //var globals = JObject.Parse(File.ReadAllText(testPath));

                    // TODO load globals here

                }

                testPath = configPath(false);
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

        public string SelectedParserName { get; set; }
        public string ParseTokensWithRule { get; set; }
        public string SelectedLexerName { get; set; }
        public bool ShowTextTokens { get; set; } = true;
        public bool ShowWhitespaceTokens { get; set; } = true;
        public bool ShowErrorTokens { get; set; } = true;
        public HashSet<int> SelectedTokenTypes { get; } = new HashSet<int>();
        public bool ShowTreeTextTokens { get; set; } = true;
        public bool ShowTreeWhitespaceTokens { get; set; } = true;
        public bool ShowTreeErrorTokens { get; set; } = true;
        public bool ShowRuleContextNodes { get; set; } = true;
        public HashSet<string> SelectedRuleContexts { get; } = new HashSet<string>();
        public string RootNodePath { get; set; }

        public bool HasTreeFilter() => !(ShowTreeErrorTokens && ShowTreeWhitespaceTokens && ShowTreeTextTokens && ShowRuleContextNodes && SelectedRuleContexts.None());
        public bool HasTokenListFilter() => !(ShowTextTokens && ShowErrorTokens && ShowWhitespaceTokens && SelectedTokenTypes.None());

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
                File.WriteAllText(configPath(true), toWrite);
            } catch { }

            // write assembly-specific
            resolver = new ConfigContractResolver { ForGlobal = false };
            settings = new JsonSerializerSettings {
                ContractResolver = resolver,
                Formatting = Formatting.Indented
            };
            toWrite = JsonConvert.SerializeObject(this, settings);
            try {
                File.WriteAllText(configPath(false), toWrite);
            } catch { }
        }

        public Config Clone() {
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
                WatchBaseExpression = WatchBaseExpression,
                ParseTokensWithRule = ParseTokensWithRule
            };
            SelectedTokenTypes.AddRangeTo(ret.SelectedTokenTypes);
            SelectedRuleContexts.AddRangeTo(ret.SelectedRuleContexts);
            return ret;
        }

        public string WatchBaseExpression { get; set; }
    }
}
