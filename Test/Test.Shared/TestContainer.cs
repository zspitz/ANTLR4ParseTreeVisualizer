using System;
using System.Linq;
using Xunit;
using static ZSpitz.Util.Functions;
using Antlr4.Runtime;
using ZSpitz.Util;
using Antlr4.Runtime.Tree;
using ParseTreeVisualizer.Serialization;
using static System.IO.File;

namespace ParseTreeVisualizer.Test {
    public class TestContainer {
        public static readonly (string lexer, string parser, string rule) InvalidSelection = (
            "Invalid lexer",
            "Invalid parser",
            "Invalid rule"
        );

        public static readonly TheoryData<object, Config> TestData = IIFE(() => {
            var java = (
                lexer: typeof(Java8Lexer),
                parser: typeof(Java8Parser),
                parseMethod: typeof(Java8Parser).GetMethod(nameof(Java8Parser.compilationUnit))!
            );

            var filesLexersParsers = new[] {
                (
                        "WindowsFunctionsForSqLite.sql",
                        typeof(SQLiteLexer),
                        typeof(SQLiteParser),
                        typeof(SQLiteParser).GetMethod(nameof(SQLiteParser.parse))!
                ),
                (
                    "Simple.java",
                    java.lexer,
                    java.parser,
                    java.parseMethod
                )
            };

            var sources = filesLexersParsers.SelectManyT((filename, lexerType, parserType, parseMethod) => {
                var source = ReadAllText($"Files/{filename}");
                var input = new AntlrInputStream(source);
                var lexer = lexerType.CreateInstance<Lexer>(new[] { input });
                var tokens = new CommonTokenStream(lexer);
                var parser = parserType.CreateInstance<Parser>(new[] { tokens });
                var tree = (IParseTree)parseMethod.Invoke(parser, Array.Empty<object>())!;
                return new object[] {
                    source, tokens, tree
                }.Select(x => (
                    source: x,
                    lexerName: lexerType.Name,
                    parserName: parserType.Name,
                    parseMethodName: parseMethod.Name
                ));
            }).ToList();

            return sources.SelectManyT((source, lexerName, parserName, parseMethodName) => {
                var configs = Enumerable.Range(0, 5).Select(x => new Config()).ToArray();
                configs.Skip(1).ForEach(x => x.SelectedLexerName = lexerName);
                configs.Skip(2).ForEach(x => x.SelectedParserName = parserName);
                configs.Skip(3).ForEach(x => x.ParseTokensWithRule = parseMethodName);

                var invalid = configs[3];
                invalid.SelectedLexerName = InvalidSelection.lexer;
                invalid.SelectedParserName = InvalidSelection.parser;
                invalid.ParseTokensWithRule = InvalidSelection.rule;

                return configs.Select(config => (source, config));
            }).ToTheoryData();
        });

        [Theory]
        [MemberData(nameof(TestData))]
        public void TestMethod(object source, Config config) {
            var vd = new VisualizerData(source, config);
            _ = new ConfigViewModel(vd);
            _ = new VisualizerDataViewModel(vd, null, null, null);

            if (config.SelectedLexerName == InvalidSelection.lexer) {
                Assert.NotEqual(InvalidSelection.lexer, vd.Config.SelectedLexerName);
                Assert.NotEqual(InvalidSelection.parser, vd.Config.SelectedParserName);
                Assert.NotEqual(InvalidSelection.rule, vd.Config.ParseTokensWithRule);
            }
        }
    }
}
