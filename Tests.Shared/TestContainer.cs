using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using ParseTreeVisualizer.Util;
using System;
using System.Linq;
using Xunit;

namespace ParseTreeVisualizer.Tests {
    public partial class TestContainer {
        static readonly string[] inputs;
        static readonly Type lexerType;
        static readonly Type parserType;
        static readonly string parseMethod;

        private static Lazy<(object, Config)[]> targetsConfigs = new Lazy<(object, Config)[]>(() => {
            return inputs
                .SelectMany(input => {
                    var stream = new AntlrInputStream(input);
                    var lexer = lexerType!.CreateInstance<Lexer>(new[] { stream });
                    var tokens = new CommonTokenStream(lexer);

                    var parser = parserType!.CreateInstance<Parser>(new[] { tokens });
                    var tree = (IParseTree)parserType.GetMethod(parseMethod!).Invoke(parser, new object[] { });
                    return new object[] { input, tokens, tree };
                })
                .SelectMany(target => {
                    // This creates new instances of Config each time the lambda expression is called
                    // The behavior is intended
                    Config[] configs = Enumerable.Range(0, 4).Select(x => new Config()).ToArray();
                    configs.Skip(1).ForEach(x => x.SelectedLexerName = lexerType.Name);
                    configs.Skip(2).ForEach(x => x.SelectedParserName = parserType.Name);
                    configs.Skip(3).ForEach(x => x.ParseTokensWithRule = parseMethod);

                    return configs.Select(config => (target, config));
                })
                .ToArray();
        });

        public static TheoryData<object, Config> GetTargetsConfigs => targetsConfigs.Value.ToTheoryData();

        [Theory]
        [MemberData(nameof(GetTargetsConfigs))]
        public void ConstructVisualizerData_(object target, Config config) {
            var vd = new VisualizerData(target, config);
        }

        private static Lazy<VisualizerData[]> visualizerDatas = new Lazy<VisualizerData[]>(() => targetsConfigs.Value.SelectT((target, config) => new VisualizerData(target, config)).ToArray());

        public static TheoryData<VisualizerData> GetVisualizerDatas => visualizerDatas.Value.ToTheoryData();

        [Theory]
        [MemberData(nameof(GetVisualizerDatas))]
        public void ConstructVisualizerDataViewModel(VisualizerData vd) {
            var vm = new VisualizerDataViewModel(vd);
        }
    }
}
