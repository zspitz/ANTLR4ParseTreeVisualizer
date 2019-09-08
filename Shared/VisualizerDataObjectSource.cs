using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using Microsoft.VisualStudio.DebuggerVisualizers;
using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace ParseTreeVisualizer {
    public class ObjectSource : VisualizerObjectSource {
        public override void TransferData(object target, Stream incomingData, Stream outgoingData) {
            var config = (VisualizerConfig)Deserialize(incomingData);
            config.LoadAvailableParser();
            var visualizerData = new VisualizerData((IParseTree)target, config);
            Serialize(outgoingData, visualizerData);
        }

        public override void GetData(object target, Stream outgoingData) => 
            Serialize(outgoingData, Assembly.GetAssembly(target.GetType()).GetName().Name);
    }
}
