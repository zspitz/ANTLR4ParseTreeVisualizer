using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using Microsoft.VisualStudio.DebuggerVisualizers;
using System.IO;

namespace ParseTreeVisualizer {
    public class ObjectSource : VisualizerObjectSource {
        public override void TransferData(object target, Stream incomingData, Stream outgoingData) {
            var config = (VisualizerConfig)Deserialize(incomingData);
            config.LoadAvailableParser();
            var visualizerData = new VisualizerData((IParseTree)target, config);
            Serialize(outgoingData, visualizerData);
        }
    }
}
