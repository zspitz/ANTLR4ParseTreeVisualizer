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
        public int RuleID { get; private set; }
        private ParseRuleContextInfo() { }
        public ParseRuleContextInfo(Type t, string ruleName, int ruleID) : base(t) {
            if (!ruleName.IsNullOrWhitespace()) {
                RuleName = ruleName;
            }
            RuleID = ruleID;
        }
        public override string ToString() => RuleName ?? Name;

        public ParseRuleContextInfo Clone() => new ParseRuleContextInfo {
            Antlr = Antlr,
            Assembly = Assembly,
            Name = Name,
            Namespace = Namespace,
            FullName = FullName,
            RuleName = RuleName,
            RuleID=RuleID
        };
    }
}
