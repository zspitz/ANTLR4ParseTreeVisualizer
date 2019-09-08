using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using ParseTreeVisualizer.Util;
using static System.IO.Path;
using Newtonsoft.Json;
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
                        ret.SelectedParserName = assemblyConfig[nameof(SelectedParserName)].ToString();
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
            }
            var toWrite = data.ToString();
            try {
                File.WriteAllText(ConfigPath(false), toWrite);
            } catch {}
        }


        public string SelectedParserName { get; set; }

        // TODO make this an immutable list, once we can combine everything into a single DLL https://github.com/zspitz/ANTLR4ParseTreeVisualizer/issues/17
        // the following should not be saved to disk
        public List<ClassInfo> AvailableParsers { get; private set; } = new List<ClassInfo>();

        // if we need to chose a lexer class, then duplicate the parser-choosing logic for selectors

        public void LoadAvailableParser() {
            AvailableParsers = new List<ClassInfo>();
            var types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(x => x.GetTypes())
                .Where(x => !x.IsAbstract && x != typeof(Parser) && x != typeof(Lexer) && x.InheritsFromOrImplements<Parser>());
            foreach (var t in types) {
                AvailableParsers.Add(new ClassInfo(t));
            }

            if (AvailableParsers.None(x => x.FullName == SelectedParserName)) {
                SelectedParserName = null;
            }
            if (SelectedParserName.IsNullOrWhitespace()) {
                SelectedParserName = preferredClassInfo(AvailableParsers)?.Name;
            }
        }

        // we're separating this in case we also need to choose a lexer class
        private ClassInfo preferredClassInfo(List<ClassInfo> lst) {
            var nonAntlr = lst.OneOrDefault(x => x.Antlr != "Runtime");
            if (nonAntlr != null) { return nonAntlr; }
            return lst.OneOrDefault();
        }

        private VisualizerConfig _originalValues;
        public VisualizerConfig Clone() {
            var ret = new VisualizerConfig();
            ret.SelectedParserName = SelectedParserName;
            // The Select forces creation of a new List
            // if AvailableParsers is an ImmutableList, this won't be necessary
            ret.AvailableParsers = AvailableParsers.Select().ToList();
            ret._originalValues = this;
            return ret;
        }
        public bool? ShouldTriggerReload {
            get {
                if (_originalValues == null) { return null; }
                return _originalValues.SelectedParserName != SelectedParserName;
            }
        }
    }

    [Serializable]
    public class ClassInfo {
        public string Name { get; }
        public string Namespace { get; }
        public string Assembly { get; }
        public string Antlr { get; private set; } // Runtime - in Antlr.Runtime, <number> - version
        public string FullName => $"{Namespace}.{Name}";
        public ClassInfo(Type t) {
            Name = t.Name;
            Namespace = t.Namespace;
            Assembly = t.Assembly.Location;
            if (t.Assembly == typeof(IParseTree).Assembly) {
                Antlr = "Runtime";
            } else {
                t.IfAttribute<GeneratedCodeAttribute>(attr => Antlr = attr.Version);
            }
        }
        public override string ToString() => FullName;
    }
}
