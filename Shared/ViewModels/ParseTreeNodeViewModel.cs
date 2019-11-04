using ParseTreeVisualizer.Util;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParseTreeVisualizer {
    public class ParseTreeNodeViewModel : Selectable<ParseTreeNode> {
        public static ParseTreeNodeViewModel Create(ParseTreeNode model) =>
            model is null ?
                null :
                new ParseTreeNodeViewModel(model);

        public ParseTreeNodeViewModel(ParseTreeNode model) : base(model) => 
            Children = (model?.Children.Select(x => new ParseTreeNodeViewModel(x)) ?? Enumerable.Empty<ParseTreeNodeViewModel>()).ToList().AsReadOnly();

        public ReadOnlyCollection<ParseTreeNodeViewModel> Children { get; }

        public void ClearSelection() {
            IsSelected = false;
            foreach (var child in Children) {
                child.ClearSelection();
            }
        }

        private bool isExpanded;
        public bool IsExpanded {
            get => isExpanded;
            set => NotifyChanged(ref isExpanded, value);
        }
        public void SetSubtreeExpanded(bool expand) {
            IsExpanded = expand;
            Children.ForEach(x => x.SetSubtreeExpanded(expand));
        }
    }
}
