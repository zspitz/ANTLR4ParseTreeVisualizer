using System.Reflection;
using ParseTreeVisualizer.Serialization;
using Periscope.Debuggee;

namespace ParseTreeVisualizer {
    public class VisualizerDataObjectSource : VisualizerObjectSourceBase<object, Config> {
        public override object GetResponse(object target, Config config) => new VisualizerData(target, config);
        public override string GetConfigKey(object target) => Assembly.GetAssembly(target.GetType()).GetName().Name;
    }
}
