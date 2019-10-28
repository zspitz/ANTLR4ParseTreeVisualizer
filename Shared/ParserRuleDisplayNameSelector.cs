using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;


namespace ParseTreeVisualizer {
    public class ParserRuleDisplayNameSelector : DataTemplateSelector {
        public DataTemplate RuleNameTemplate { get; set; }
        public DataTemplate TypeNameTemplate { get; set; }
        public override DataTemplate SelectTemplate(object item, DependencyObject container) => 
            (item as ClassInfo)?.RuleName == null ?
                TypeNameTemplate :
                RuleNameTemplate;
    }
}
