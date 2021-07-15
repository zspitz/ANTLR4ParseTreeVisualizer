using System.Reflection;
using ParseTreeVisualizer.Serialization;
using Periscope.Debuggee;

namespace ParseTreeVisualizer {
    public class VisualizerDataObjectSource : VisualizerObjectSourceBase<object, Config> {
        static VisualizerDataObjectSource() => SubfolderAssemblyResolver.Hook(
#if ANTLR_LEGACY
            "ParseTreeVisualizer.Legacy"
#else
            "ParseTreeVisualizer.Standard"
#endif
        );
        public override object GetResponse(object target, Config config) => new VisualizerData(target, config);
        public override string GetConfigKey(object target) =>
            target is string ?
                "" :
                target.GetType().Assembly.GetName().Name;
    }
}
