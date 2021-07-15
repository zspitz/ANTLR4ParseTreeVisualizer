using ParseTreeVisualizer.Serialization;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System;
using ZSpitz.Util.Wpf;

namespace ParseTreeVisualizer {
    public partial class VisualizerWindow {
        public VisualizerWindow() {
            InitializeComponent();

            setAsRootNode = new(
                o => {
                    if (Config is null) { return; }
                    var newConfig = Config.Clone();
                    newConfig.RootNodePath = ((ParseTreeNodeViewModel)o).Model.Path;
                    Initialize(newConfig);
                }
            );
        }

        protected override (object windowContext, object optionsContext, Config config) GetViewState(object response, ICommand? OpenInNewWindow) => 
            response is not VisualizerData vd ? 
                throw new InvalidOperationException("Unrecognized response type; expected VisualizerData.") :
                (
                    new VisualizerDataViewModel(vd, OpenInNewWindow, Periscope.Visualizer.CopyWatchExpression, setAsRootNode),
                    new ConfigViewModel(vd),
                    vd.Config
                );

        protected override void TransformConfig(Config config, object parameter) {
            if (parameter is ParseTreeNode ptn) {
                config.RootNodePath = ptn.Path;
                return;
            }
            throw new NotImplementedException();
        }

        private readonly RelayCommand setAsRootNode;
    }
}
