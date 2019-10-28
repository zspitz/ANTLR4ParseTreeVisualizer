using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using ParseTreeVisualizer.Util;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParseTreeVisualizer {
    [Serializable]
    public class ClassInfo {
        public static readonly ClassInfo None = new ClassInfo("(None)");

        public string Name { get; }
        public string Namespace { get; }
        public string Assembly { get; }
        public string Antlr { get; private set; } // Runtime - in Antlr.Runtime, <number> - version
        public string FullName { get; }
        public string RuleName { get; }
        public int? RuleID { get; }

        public ReadOnlyCollection<string> MethodNames { get; }

        private ClassInfo(string name) => Name = name;
        public ClassInfo(Type t, string ruleName = null, int? ruleID = null, bool loadMethodNames = false) {
            if (t is null) { throw new ArgumentNullException(nameof(t)); }

            Name = t.Name;
            Namespace = t.Namespace;
            Assembly = t.Assembly.Location;
            if (t.Assembly == typeof(IParseTree).Assembly) {
                Antlr = "Runtime";
            } else {
                t.IfAttribute<GeneratedCodeAttribute>(attr => Antlr = attr.Version);
            }
            FullName = t.FullName;
            if (!ruleName.IsNullOrWhitespace()) { RuleName = ruleName; }
            RuleID = ruleID;

            if (loadMethodNames) {
                MethodNames = t.GetMethods()
                    .Where(x => x.ReturnType.InheritsFromOrImplements<ParserRuleContext>())
                    .Select(x => x.Name)
                    .ToList().AsReadOnly();
            }
        }

        public override string ToString() => FullName ?? Name;
    }
}
