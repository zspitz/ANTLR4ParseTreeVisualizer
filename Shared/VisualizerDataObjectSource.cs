using Antlr4.Runtime;
using Microsoft.VisualStudio.DebuggerVisualizers;
using System.IO;

namespace ParseTreeVisualizer {
    public class ObjectSource : VisualizerObjectSource {
        public override void GetData(object target, Stream outgoingData) {
            var visualizerData = new VisualizerData((RuleContext)target);
            Serialize(outgoingData, visualizerData);
        }
    }
}
