using System.Windows;
using System.Windows.Controls;
using ParseTreeVisualizer.Serialization;

namespace ParseTreeVisualizer {
    public class ParserRuleDisplayNameSelector : DataTemplateSelector {
        public DataTemplate? RuleNameTemplate { get; set; }
        public DataTemplate? TypeNameTemplate { get; set; }
        public override DataTemplate? SelectTemplate(object item, DependencyObject container) => 
            (item as ClassInfo)?.RuleName == null ?
                TypeNameTemplate :
                RuleNameTemplate;
    }
}
