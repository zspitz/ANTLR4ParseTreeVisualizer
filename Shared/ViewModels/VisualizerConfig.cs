using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using ParseTreeVisualizer.Util;
using static System.IO.Path;
using Newtonsoft.Json.Linq;
using System.IO;

namespace ParseTreeVisualizer {
    [Serializable]
    public class VisualizerConfig {
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

        public static VisualizerConfig Get() {
            var ret = new VisualizerConfig();

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
                    if (!fileText.IsNullOrWhitespace()) {
                        var assemblyConfig = JObject.Parse(fileText);
                        ret.SelectedParserName = (string)assemblyConfig[nameof(SelectedParserName)];
                        ret.SelectedLexerName = (string)assemblyConfig[nameof(SelectedLexerName)];
                    }
                }
            }

            return ret;
        }

        private VisualizerConfig() { }

        public void Write() {
            if (!Directory.Exists(ConfigFolder)) {
                try {
                    Directory.CreateDirectory(ConfigFolder);
                } catch {
                    return;
                }
            }

            var data = new JObject();

            // filll globals here
            // write globals to file

            // fill assembly-specific here
            data = new JObject();
            if (!SelectedParserName.IsNullOrWhitespace()) {
                data[nameof(SelectedParserName)] = SelectedParserName;
                data[nameof(SelectedLexerName)] = SelectedLexerName;
            }
            var toWrite = data.ToString();
            try {
                File.WriteAllText(ConfigPath(false), toWrite);
            } catch { }
        }

        private string selectedParserName;
        public string SelectedParserName {
            get => selectedParserName;
            set => selectedParserName = value;
        }
        public string SelectedLexerName { get; set; }

        private VisualizerConfig _originalValues;

        public VisualizerConfig Clone() => new VisualizerConfig {
            SelectedParserName = SelectedParserName,
            SelectedLexerName = SelectedLexerName,
            _originalValues = this
        };

        public bool? ShouldTriggerReload {
            get {
                if (_originalValues == null) { return null; }
                return _originalValues.SelectedParserName != SelectedParserName ||
                    _originalValues.SelectedLexerName != SelectedLexerName;
            }
        }
    }
}
