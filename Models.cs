record Rule(string Id, string Name, string Pattern, string Kind, int MaxDepth, string[] Cmds);
record Cand(Rule Rule, string Path);
record Job(string Label, List<string> Lines);
record Extras(string Kind, List<string> Items, List<string> Platforms, List<string> BuildPresets);
record Opts(List<string> Archs, List<string> Base, List<string> Extra, bool Publish,
            bool Lto, bool Pgo, bool Avx2, bool Avx512, bool Fast, bool Par, string Custom, string Out,
            bool Sc, bool Single, bool Trim, bool Comp, bool R2r);
