using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using System;
using System.CodeDom.Compiler;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using ZSpitz.Util;

namespace ParseTreeVisualizer.Serialization {
    [Serializable]
    public class ClassInfo {
        public static readonly ClassInfo None = new("(None)");

        public string Name { get; }
        public string? Namespace { get; }
        public string? Assembly { get; }
        public string? Antlr { get; } // Runtime - in Antlr.Runtime, <number> - version
        public string? FullName { get; }
        public string? RuleName { get; }
        public int? RuleID { get; }
        public bool HasRelevantConstructor { get; } = false;

        public ReadOnlyCollection<string>? MethodNames { get; }

        private ClassInfo(string name) => Name = name;
        public ClassInfo(Type t, string? ruleName = null, int? ruleID = null, bool loadMethodNames = false) {
            if (t is null) { throw new ArgumentNullException(nameof(t)); }

            Name = t.Name;
            Namespace = t.Namespace;
            Assembly = t.Assembly.Location;
            if (t.Assembly == typeof(IParseTree).Assembly) {
                Antlr = "Runtime";
            } else if (t.GetCustomAttribute<GeneratedCodeAttribute>() is GeneratedCodeAttribute attr) {
                Antlr = attr.Version;
            }
            FullName = t.FullName;
            RuleName = ruleName.IsNullOrWhitespace() ? null : ruleName;
            RuleID = ruleID;
            HasRelevantConstructor =
                t.InheritsFromOrImplements<Lexer>() ? t.GetConstructor(new[] { typeof(AntlrInputStream) }) is not null :
                t.InheritsFromOrImplements<Parser>() ? t.GetConstructor(new[] { typeof(CommonTokenStream) }) is not null :
                false;

            if (loadMethodNames) {
                MethodNames = t.GetMethods()
                    .Where(x => !x.IsSpecialName && x.ReturnType.InheritsFromOrImplements<ParserRuleContext>())
                    .Select(x => x.Name)
                    .Where(x => x != "GetInvokingContext")
                    .Ordered()
                    .ToList()
                    .AsReadOnly();
            }
        }

        public override string ToString() => FullName ?? Name;
    }
}
