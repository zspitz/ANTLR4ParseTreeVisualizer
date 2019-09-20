using ParseTreeVisualizer.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParseTreeVisualizer.ViewModels {
    [Serializable]
    public class ParseRuleContextInfo : ClassInfo {
        public string RuleName { get; private set; }
        private ParseRuleContextInfo() { }
        public ParseRuleContextInfo(Type t, string ruleName) : base(t) {
            if (!ruleName.IsNullOrWhitespace()) {
                RuleName = ruleName;
            }
        }
        public override string ToString() => RuleName ?? Name;

        public ParseRuleContextInfo Clone() => new ParseRuleContextInfo {
            Antlr = Antlr,
            Assembly = Assembly,
            Name = Name,
            Namespace = Namespace,
            FullName = FullName,
            RuleName = RuleName
        };
    }
}
