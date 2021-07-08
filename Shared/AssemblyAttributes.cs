﻿using System.Diagnostics;

[assembly: DebuggerVisualizer(
    visualizer: typeof(ParseTreeVisualizer.Visualizer),
    visualizerObjectSource: typeof(ParseTreeVisualizer.ObjectSource),
    Target = typeof(Antlr4.Runtime.RuleContext),
#if ANTLR_RUNTIME
    Description = "ANTLR4 Parse Tree Visualizer")]
#else
    Description = "ANTLR4 Parse Tree Visualizer (Standard)")]
#endif 

[assembly: DebuggerVisualizer(
    visualizer: typeof(ParseTreeVisualizer.Visualizer),
    visualizerObjectSource: typeof(ParseTreeVisualizer.ObjectSource),
    Target = typeof(string),
#if ANTLR_RUNTIME
    Description = "ANTLR4 Parse Tree Visualizer")]
#else
    Description = "ANTLR4 Parse Tree Visualizer (Standard)")]
#endif 

[assembly: DebuggerVisualizer(
    visualizer: typeof(ParseTreeVisualizer.Visualizer),
    visualizerObjectSource: typeof(ParseTreeVisualizer.ObjectSource),
    Target = typeof(Antlr4.Runtime.BufferedTokenStream),
#if ANTLR_RUNTIME
    Description = "ANTLR4 Parse Tree Visualizer")]
#else
    Description = "ANTLR4 Parse Tree Visualizer (Standard)")]
#endif