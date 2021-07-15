using ParseTreeVisualizer.Serialization;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using ZSpitz.Util;
using ZSpitz.Util.Wpf;

namespace ParseTreeVisualizer {
    public class ParseTreeNodeViewModel : Selectable<ParseTreeNode> {
        private static readonly RelayCommand subtreeExpand = new(
            prm => ((ParseTreeNodeViewModel)prm).SetSubtreeExpanded(true)
        );
        private static readonly RelayCommand subtreeCollapse = new(
            prm => ((ParseTreeNodeViewModel)prm).SetSubtreeExpanded(false)
        );

        public ParseTreeNodeViewModel(
                    ParseTreeNode model, 
                    ICommand? openInNewWindow = null, 
                    RelayCommand? copyWatchExpression = null,
                    RelayCommand? setAsRootNode = null
                ) : base(model) {
            Children = (
                model?.Children.Select(x => new ParseTreeNodeViewModel(x,openInNewWindow, copyWatchExpression, setAsRootNode)) ?? 
                Enumerable.Empty<ParseTreeNodeViewModel>()
            ).ToList().AsReadOnly();

            OpenInNewWindow = openInNewWindow;
            CopyWatchExpression = copyWatchExpression;
            SetAsRootNode = setAsRootNode;
        }

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

        public ICommand? OpenInNewWindow { get; private set; }
        public RelayCommand? CopyWatchExpression { get; private set; }

        public string? WatchFormatString => "{0}" + Model.Path?.Split('.').Joined("", x => $".GetChild({x})");

        public RelayCommand SubtreeExpand => subtreeExpand;
        public RelayCommand SubtreeCollapse => subtreeCollapse;
        public RelayCommand? SetAsRootNode { get; }
    }
}
