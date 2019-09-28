using ParseTreeVisualizer.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParseTreeVisualizer {
    public class ParseTreeNodeViewModel : Selectable<ParseTreeNode> {
        public ParseTreeNodeViewModel(ParseTreeNode model) : base(model) { }

        public IReadOnlyCollection<ParseTreeNodeViewModel> Children { get; }

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
