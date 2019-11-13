using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ParseTreeVisualizer.Util {
    // https://stackoverflow.com/a/33421573/111794
    public class ComboBoxTemplateSelector : DataTemplateSelector {
        public DataTemplate? SelectedItemTemplate { get; set; }
        public DataTemplateSelector? SelectedItemTemplateSelector { get; set; }
        public DataTemplate? DropdownItemsTemplate { get; set; }
        public DataTemplateSelector? DropdownItemsTemplateSelector { get; set; }

        public override DataTemplate? SelectTemplate(object item, DependencyObject container) {
            var itemToCheck = container;

            // Search up the visual tree, stopping at either a ComboBox or
            // a ComboBoxItem (or null). This will determine which template to use
            while (itemToCheck != null && !(itemToCheck is ComboBoxItem) && !(itemToCheck is ComboBox))
                itemToCheck = VisualTreeHelper.GetParent(itemToCheck);

            // If you stopped at a ComboBoxItem, you're in the dropdown
            var inDropDown = (itemToCheck is ComboBoxItem);

            return inDropDown
                ? DropdownItemsTemplate ?? DropdownItemsTemplateSelector?.SelectTemplate(item, container)
                : SelectedItemTemplate ?? SelectedItemTemplateSelector?.SelectTemplate(item, container);
        }
    }
}
