using Microsoft.VisualStudio.DebuggerVisualizers;
using System.IO;
using System.Reflection;

namespace ParseTreeVisualizer {
    public class ObjectSource : VisualizerObjectSource {
        public override void TransferData(object target, Stream incomingData, Stream outgoingData) {
            var config = (Config)Deserialize(incomingData);
            var visualizerData = new VisualizerData(target, config);
            Serialize(outgoingData, visualizerData);
        }

        public override void GetData(object target, Stream outgoingData) =>
            Serialize(outgoingData, Assembly.GetAssembly(target.GetType()).GetName().Name);
    }
}
