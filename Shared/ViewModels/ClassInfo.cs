using Antlr4.Runtime.Tree;
using ParseTreeVisualizer.Util;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParseTreeVisualizer.ViewModels {
    [Serializable]
    public class ClassInfo {
        public string Name { get; }
        public string Namespace { get; }
        public string Assembly { get; }
        public string Antlr { get; private set; } // Runtime - in Antlr.Runtime, <number> - version
        public string FullName {
            get {
                if (Namespace.IsNullOrWhitespace()) { return null; }
                return $"{Namespace}.{Name}";
            }
        }

        private ClassInfo(string name) => Name = name;

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

        public static readonly ClassInfo None = new ClassInfo("(None)");
    }
}
