using System.Diagnostics;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;

// Lógica da tela. O visual (controles e layout) fica em MainForm.Designer.cs e pode ser editado no Designer do Visual Studio.
// Convenção nos comandos do script: § = aspas duplas
partial class MainForm : Form
{
    static readonly string[] None = Array.Empty<string>();
    static readonly Regex NoChk = new(@"^(set|where|for|findstr|echo|pause|robocopy|mkdir|if not defined|if errorlevel 8)\b");
    // pastas ignoradas na busca (dependências de terceiros costumam ter seus próprios projetos)
    static readonly Regex SkipDir = new(@"^(\.git|node_modules|bin|obj|vendor|third[_-]?party|external|extern|deps|packages)$", RegexOptions.IgnoreCase);

    // MaxDepth = profundidade máxima da busca (1 = só a raiz)
    static readonly Rule[] Rules =
    {
        new("cmake", "CMake (C/C++)", @"^CMakeLists\.txt$", "msvc", 4, None),
        new("meson", "Meson (C/C++)", @"^meson\.build$", "msvc", 3, None),
        new("sln", "Visual Studio (sln / vcxproj)", @"\.(sln|vcxproj)$", "msvc", 5, None),
        new("dotnet", ".NET / C# (csproj)", @"\.csproj$", "dotnet", 4, None),
        new("rust", "Rust (Cargo)", @"^Cargo\.toml$", "rust", 3, None),
        new("go", "Go", @"^go\.mod$", "go", 3, None),
        new("gradle", "Java (Gradle)", @"^(build\.gradle(\.kts)?|gradlew\.bat)$", "plain", 3, new[] {
            "if exist gradlew.bat (call gradlew.bat build -x test) else (call gradle build -x test)" }),
        new("maven", "Java (Maven)", @"^pom\.xml$", "plain", 3, new[] {
            "if exist mvnw.cmd (call mvnw.cmd -B clean package -DskipTests) else (call mvn -B clean package -DskipTests)" }),
        new("node", "Node.js", @"^package\.json$", "plain", 2, new[] {
            "if exist pnpm-lock.yaml (call pnpm install --frozen-lockfile) else if exist yarn.lock (call yarn install --frozen-lockfile) else if exist package-lock.json (call npm ci) else (call npm install)",
            "set NODE_ENV=production",
            @"findstr /c:§\§build\§§ package.json >nul && call npm run build" }),
        new("python", "Python", @"^(pyproject\.toml|setup\.py|requirements\.txt)$", "plain", 2, new[] {
            "python -m venv .venv", @"call .venv\Scripts\activate.bat",
            "python -m pip install -U pip wheel build",
            "if exist requirements.txt python -m pip install -r requirements.txt",
            "if exist pyproject.toml (python -m build --wheel) else if exist setup.py (python -m build --wheel)" }),
    };

    static readonly Dictionary<string, (string Vc, string Plat, string Rid, string Rust, string Go)> Arch = new()
    {
        ["x64"] = ("x64", "x64", "win-x64", "x86_64-pc-windows-msvc", "amd64"),
        ["x86"] = ("x86", "Win32", "win-x86", "i686-pc-windows-msvc", "386"),
    };

    // ---------- estado ----------
    string script = "", repo = "repo", curUrl = "", tmpDir = "", kind = "full";
    int njobs;
    bool loading, hasVcpkg;
    List<string> paths = new();
    Func<string, string?>? read;
    List<Cand> cands = new();
    Cand? sel;
    Extras ex = new("", new(), new(), new());

    public MainForm()
    {
        InitializeComponent();
        cardEx.Visible = false;   // só aparece quando há presets / configurações do .sln
        try { Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath) ?? Icon; } catch { /* sem ícone: mantém o padrão */ }
        bScript.EnabledChanged += (_, _) => bScript.BackColor = bScript.Enabled ? Color.FromArgb(37, 99, 235) : Color.FromArgb(165, 190, 240);
        bScript.Enabled = bBuild.Enabled = bPub.Enabled = false;

        foreach (var c in new[] { a86, a64, cRel, cDbg, oLto, oPgo, oAvx2, oAvx512, oFast, oPar, pSc, pSingle, pTrim, pComp, pR2r }) c.CheckedChanged += (_, _) => Render();
        oExtra.TextChanged += (_, _) => Render();
        oOut.TextChanged += (_, _) => Render();
        clb.ItemCheck += (_, _) => BeginInvoke(Render);
        cmb.SelectedIndexChanged += (_, _) => { if (!loading && cmb.SelectedIndex >= 0) Pick(cands[cmb.SelectedIndex]); };

        bGit.Click += async (_, _) => await FromGit();
        bDir.Click += async (_, _) => await FromFolder();
        bScript.Click += (_, _) => Save("full");
        bBuild.Click += (_, _) => Save("build");
        bPub.Click += (_, _) => Save("publish");
        FormClosed += (_, _) => Cleanup();
    }

    // ---------- util ----------
    static string Q(string s) => s.Replace('§', '"');
    static string Slug(string s) => Regex.Replace(s, @"\W+", "_");
    static string Win(string p) => p.Replace('/', '\\');
    static string DirOf(string p) => p.Contains('/') ? p[..p.LastIndexOf('/')] : "";
    static int Depth(string p) => p.Split('/').Length;
    static string BaseName(string p) => p[(p.LastIndexOf('/') + 1)..];

    void Log(string s, Color? c = null)
    {
        log.SelectionColor = c ?? Color.Gainsboro;
        log.AppendText(s + "\n");
        log.ScrollToCaret();
    }

    void Reset()
    {
        log.Clear(); preview.Clear(); bScript.Enabled = bBuild.Enabled = bPub.Enabled = false;
        result.Text = "Analisando…"; resultSub.Text = ""; cardEx.Visible = false;
        loading = true; clb.Items.Clear(); cmb.Items.Clear(); loading = false; sel = null;
    }

    void Cleanup()
    {
        if (tmpDir == "") return;
        try
        {
            foreach (var f in Directory.EnumerateFiles(tmpDir, "*", SearchOption.AllDirectories)) File.SetAttributes(f, FileAttributes.Normal);
            Directory.Delete(tmpDir, true);
        }
        catch { /* best-effort */ }
        tmpDir = "";
    }

    static string NameOf(string u) =>
        Regex.Split(Regex.Replace(u.Trim(), @"\.git$", ""), @"[/:\\]").LastOrDefault(s => s != "") ?? "repo";

    // lista os arquivos em até 5 níveis, com caminhos relativos separados por "/"
    static List<string> ListFiles(string root)
    {
        var res = new List<string>();
        void Walk(string dir, string rel, int depth)
        {
            try
            {
                foreach (var f in Directory.EnumerateFiles(dir)) res.Add(rel + Path.GetFileName(f));
                if (depth >= 5) return;
                foreach (var d in Directory.EnumerateDirectories(dir))
                {
                    var n = Path.GetFileName(d);
                    if (!SkipDir.IsMatch(n)) Walk(d, rel + n + "/", depth + 1);
                }
            }
            catch { /* pasta sem acesso: ignora */ }
        }
        Walk(root, "", 1);
        return res;
    }

    // ---------- análise ----------
    async Task FromGit()
    {
        var u = url.Text.Trim();
        if (u == "") return;
        Cleanup(); Reset(); repo = NameOf(u);
        tmpDir = Path.Combine(Path.GetTempPath(), "gab_" + Guid.NewGuid().ToString("N")[..8]);
        Log($"> git clone --depth 1 {u}", Color.Gray);
        int code = await Run("git", $"clone --depth 1 \"{u}\" \"{tmpDir}\"");
        if (code != 0) { Log("[ERRO] clone falhou (URL invalida, repositorio privado ou git ausente).", Color.IndianRed); result.Text = "Falha ao clonar"; return; }
        var dir = tmpDir;
        await Analyze(ListFiles(dir), u, f => File.ReadAllText(Path.Combine(dir, f)));
    }

    async Task FromFolder()
    {
        using var dlg = new FolderBrowserDialog();
        if (dlg.ShowDialog() != DialogResult.OK) return;
        Cleanup(); Reset();
        var dir = dlg.SelectedPath;
        repo = url.Text.Trim() != "" ? NameOf(url.Text) : Path.GetFileName(dir);
        Log("Pasta local: " + dir, Color.Gray);
        await Analyze(ListFiles(dir), url.Text.Trim(), f => File.ReadAllText(Path.Combine(dir, f)));
    }

    // procura cada tipo em até MaxDepth níveis; os arquivos mais rasos vêm primeiro
    List<Cand> Candidates()
    {
        var res = new List<Cand>();
        foreach (var r in Rules)
            res.AddRange(paths
                .Where(p => Depth(p) <= r.MaxDepth && Regex.IsMatch(BaseName(p), r.Pattern, RegexOptions.IgnoreCase))
                .OrderBy(p => r.Id == "sln" && !p.EndsWith(".sln", StringComparison.OrdinalIgnoreCase) ? 1 : 0)
                .ThenBy(Depth).ThenBy(p => p, StringComparer.Ordinal)
                .Take(6).Select(p => new Cand(r, p)));
        return res;
    }

    async Task Analyze(List<string> list, string u, Func<string, string?> rd)
    {
        paths = list; read = rd; curUrl = u; hasVcpkg = paths.Contains("vcpkg.json");
        Log($"{paths.Count} arquivos listados (busca em até 5 níveis).", Color.Gray);
        cands = Candidates();
        foreach (var r in Rules)
        {
            Log($"? {r.Name}...");
            await Task.Delay(100);
            var m = cands.Where(c => c.Rule == r).ToList();
            if (m.Count > 0) Log($"   ENCONTRADO: {m[0].Path}{(m.Count > 1 ? $"  (+{m.Count - 1})" : "")}", Color.Gold);
            else Log("   nao encontrado", Color.Gray);
        }
        if (cands.Count == 0) { result.Text = "Tipo de projeto nao reconhecido"; Log("[ERRO] nenhuma regra casou.", Color.IndianRed); return; }
        loading = true; foreach (var c in cands) cmb.Items.Add($"{c.Rule.Name} — {c.Path}"); cmb.SelectedIndex = 0; loading = false;
        Pick(cands[0]);
        Log("[OK] Script gerado. Ajuste as opcoes acima; o script e atualizado na hora.", Color.LightGreen);
    }

    void Pick(Cand c)
    {
        sel = c; ex = DetectExtras(c);
        loading = true; clb.Items.Clear(); foreach (var i in ex.Items) clb.Items.Add(i); loading = false;
        cardEx.Visible = ex.Items.Count > 0;
        lblEx.Text = ex.Kind == "preset" ? "PRESETS CMAKE" : "CONFIGURAÇÕES DO .SLN";
        if (ex.Items.Count > 0) Log($"Definicoes encontradas ({ex.Kind}): {string.Join(", ", ex.Items)}", Color.Gold);
        Render();
    }

    Extras DetectExtras(Cand c)
    {
        var e = new Extras("", new(), new(), new());
        try
        {
            if (c.Rule.Id == "sln" && c.Path.EndsWith(".sln", StringComparison.OrdinalIgnoreCase))
            {
                var t = read!(c.Path);
                if (t != null)
                {
                    var cs = new SortedSet<string>(); var ps = new SortedSet<string>();
                    foreach (Match m in Regex.Matches(t, @"^\s*([^|=\r\n{]+)\|([^=\r\n]+?)\s*=\s*\1\|\2\s*$", RegexOptions.Multiline))
                    { cs.Add(m.Groups[1].Value.Trim()); ps.Add(m.Groups[2].Value.Trim()); }
                    e = new("sln", cs.Where(x => !Regex.IsMatch(x, @"^(release|debug)$", RegexOptions.IgnoreCase)).ToList(), ps.ToList(), new());
                }
            }
            if (c.Rule.Id == "cmake" && DirOf(c.Path) == "" && paths.Contains("CMakePresets.json"))
            {
                using var doc = JsonDocument.Parse(read!("CMakePresets.json")!,
                    new JsonDocumentOptions { AllowTrailingCommas = true, CommentHandling = JsonCommentHandling.Skip });
                var items = new List<string>(); var bps = new List<string>();
                if (doc.RootElement.TryGetProperty("configurePresets", out var cp))
                    foreach (var p in cp.EnumerateArray())
                    {
                        var n = p.GetProperty("name").GetString()!;
                        bool hidden = p.TryGetProperty("hidden", out var h) && h.ValueKind == JsonValueKind.True;
                        if (!hidden && !Regex.IsMatch(n, "mac|linux|ubuntu|android|ios|bsd", RegexOptions.IgnoreCase)) items.Add(n);
                    }
                if (doc.RootElement.TryGetProperty("buildPresets", out var bp))
                    foreach (var p in bp.EnumerateArray()) bps.Add(p.GetProperty("name").GetString()!);
                e = new("preset", items, new(), bps);
            }
        }
        catch (Exception x) { Log("Aviso: nao foi possivel ler as definicoes do projeto (" + x.Message + ")", Color.Gray); }
        return e;
    }

    // ---------- geração do script ----------
    Opts GetOpts(string k) => new(
        new[] { "x86", "x64" }.Where(a => (a == "x86" ? a86 : a64).Checked).ToList(),
        new[] { "Release", "Debug" }.Where(c => (c == "Release" ? cRel : cDbg).Checked).ToList(),
        clb.CheckedItems.Cast<string>().ToList(),
        k == "publish", oLto.Checked, oPgo.Checked, oAvx2.Checked, oAvx512.Checked, oFast.Checked, oPar.Checked, oExtra.Text.Trim(),
        oOut.Text.Trim().Replace("\"", "").TrimEnd('\\', '/'),
        pSc.Checked, pSingle.Checked, pTrim.Checked, pComp.Checked, pR2r.Checked);

    // Flags MSVC via _CL_ / _LINK_ (valem para CMake, MSBuild e Meson; _CL_ é aplicada por último)
    static (string cl, string lk) Flags(Opts o, bool dbg)
    {
        if (dbg) return ("", "");
        var cl = "/O2 /Ob2 /Oi /Ot /Gy /DNDEBUG"; var lk = "/OPT:REF /OPT:ICF";
        if (o.Lto || o.Pgo) cl += " /GL";
        if (o.Pgo) lk = "/LTCG:PGI"; else if (o.Lto) lk += " /LTCG";
        if (o.Avx512) cl += " /arch:AVX512"; else if (o.Avx2) cl += " /arch:AVX2";
        if (o.Fast) cl += " /fp:fast";
        if (o.Par) cl += " /Qpar";
        if (o.Custom != "") cl += " " + o.Custom;
        return (cl, lk);
    }

    List<Job> Jobs(Opts o, Cand cand)
    {
        var r = cand.Rule; var J = new List<Job>();
        var dir = Win(DirOf(cand.Path)); var src = dir == "" ? "." : dir; var file = Win(cand.Path);
        var cd = dir == "" ? new List<string>() : new List<string> { Q($"cd /d §{dir}§") };
        if (r.Kind == "plain") { J.Add(new(r.Name, cd.Concat(r.Cmds.Select(Q)).ToList())); return J; }

        var cfgs = o.Base.Concat(ex.Kind == "sln" ? o.Extra : Enumerable.Empty<string>()).ToList();
        var presets = r.Id == "cmake" && ex.Kind == "preset" ? o.Extra : new List<string>();
        int total = o.Archs.Count * cfgs.Count + presets.Count;
        // com pasta de saída: uma combinação vai direto nela; várias ganham uma subpasta cada
        string? OutDir(string t) => o.Out == "" ? null : total > 1 ? @"%OUTROOT%\" + t : "%OUTROOT%";

        foreach (var a in o.Archs)
            foreach (var cfg in cfgs)
            {
                var A = Arch[a];
                bool dbg = Regex.IsMatch(cfg, "debug", RegexOptions.IgnoreCase), rel = !dbg, pgo = o.Pgo && rel;
                var t = Slug(a + "-" + cfg); var bd = @"build\" + t; var f = Flags(o, dbg); var L = new List<string>();
                if (r.Kind == "msvc")
                {
                    L.Add(Q($@"call §%VSP%\VC\Auxiliary\Build\vcvarsall.bat§ {A.Vc} >nul"));
                    if (f.cl != "") { L.Add(Q($"set §_CL_={f.cl}§")); L.Add(Q($"set §_LINK_={f.lk}§")); }
                }
                switch (r.Id)
                {
                    case "cmake":
                        {
                            var od = OutDir(t) ?? (o.Publish ? @"publish\" + t : "");
                            var oa = o.Out != "" && !o.Publish ? Q($" -DCMAKE_RUNTIME_OUTPUT_DIRECTORY=§{od}§ -DCMAKE_LIBRARY_OUTPUT_DIRECTORY=§{od}§") : "";
                            L.Add(Q($"set §GEN=-A {A.Plat}§")); L.Add(Q("where ninja >nul 2>&1 && set §GEN=-G Ninja§"));
                            L.Add(Q($"cmake -S §{src}§ -B §{bd}§ %GEN% %VCPKG_ARGS% -DCMAKE_BUILD_TYPE={cfg}{oa}"));
                            L.Add(Q($"cmake --build §{bd}§ --config {cfg} --parallel"));
                            if (pgo)
                            {
                                L.Add($"echo [PGO] Fase 1 pronta. Execute o programa instrumentado em {bd} e feche-o."); L.Add("pause");
                                L.Add(Q("set §_LINK_=/LTCG:PGO§")); L.Add(Q($"cmake --build §{bd}§ --config {cfg} --parallel --clean-first"));
                            }
                            if (o.Publish) L.Add(Q($"cmake --install §{bd}§ --config {cfg} --prefix §{od}§"));
                            break;
                        }
                    case "sln":
                        {
                            var plat = a == "x86" ? (ex.Platforms.Contains("Win32") || !ex.Platforms.Contains("x86") ? "Win32" : "x86") : "x64";
                            var P = Q($" -p:Configuration=§{cfg}§ -p:Platform={plat} -p:WindowsTargetPlatformVersion=10.0 -p:PlatformToolset=%TS%");
                            var od = OutDir(t) ?? (o.Publish ? @"%CD%\publish\" + t : null);
                            if (od != null) { L.Add(Q($"set §OD={od}§")); P += Q(@" -p:OutDir=§%OD%\\§"); }
                            L.Add(Q("msbuild §%SLN%§ -restore -m") + P);
                            if (pgo)
                            {
                                L.Add("echo [PGO] Fase 1 pronta. Execute o programa instrumentado e feche-o."); L.Add("pause");
                                L.Add(Q("set §_LINK_=/LTCG:PGO§")); L.Add(Q("msbuild §%SLN%§ -m -t:Rebuild") + P);
                            }
                            break;
                        }
                    case "meson":
                        {
                            var od = OutDir(t); var mb = o.Out != "" && !o.Publish ? od! : bd;
                            L.Add(Q($"meson setup §{mb}§ §{src}§ --buildtype={(dbg ? "debug" : "release")}{(o.Lto && rel ? " -Db_lto=true" : "")}"));
                            L.Add(Q($"meson compile -C §{mb}§"));
                            var dest = od ?? @"%CD%\publish\" + t;
                            if (o.Publish) L.Add(Q($"meson install -C §{mb}§ --destdir §{dest}§"));
                            break;
                        }
                    case "dotnet":
                        {
                            var od = OutDir(t) ?? (o.Publish ? @"publish\" + t : @"out\" + t);
                            bool sc = o.Sc || o.Trim || o.Comp, single = o.Single || o.Comp;   // trimmed/compressão exigem self-contained
                            var pa = $"--self-contained {(sc ? "true" : "false")}" + (single ? " -p:PublishSingleFile=true" : "") + (single && sc ? " -p:IncludeNativeLibrariesForSelfExtract=true" : "")
                                + (o.Trim ? " -p:PublishTrimmed=true -p:_SuppressWinFormsTrimError=true" : "") + (o.Comp ? " -p:EnableCompressionInSingleFile=true" : "") + (o.R2r && rel ? " -p:PublishReadyToRun=true" : "");
                            L.Add(o.Publish
                                ? Q($"dotnet publish §%PROJ%§ -c {cfg} -r {A.Rid} {pa} -p:DebugType={(rel ? "none" : "embedded")} -o §{od}§")
                                : Q($"dotnet build §%PROJ%§ -c {cfg} -r {A.Rid} --self-contained false -o §{od}§"));
                            break;
                        }
                    case "rust":
                        {
                            var od = OutDir(t); var td = !o.Publish && od != null ? od : @"target\" + t;
                            L.Add($"where rustup >nul 2>&1 && rustup target add {A.Rust}");
                            if (rel)
                            {
                                if (o.Lto) { L.Add("set CARGO_PROFILE_RELEASE_LTO=fat"); L.Add("set CARGO_PROFILE_RELEASE_CODEGEN_UNITS=1"); }
                                var ft = o.Avx512 ? "+avx512f" : o.Avx2 ? "+avx2" : "";
                                if (ft != "") L.Add(Q($"set §RUSTFLAGS=-C target-feature={ft}§"));
                            }
                            L.Add(Q($"cargo build{(rel ? " --release" : "")} --manifest-path §{file}§ --target {A.Rust} --target-dir §{td}§"));
                            if (o.Publish)
                            {
                                var dest = od ?? @"publish\" + t;
                                L.Add(Q($"robocopy §{td}\\{A.Rust}\\{(rel ? "release" : "debug")}§ §{dest}§ *.exe *.dll /NJH /NJS /NP /NFL /NDL"));
                                L.Add("if errorlevel 8 exit /b 1");
                            }
                            break;
                        }
                    case "go":
                        {
                            var od = OutDir(t) ?? (o.Publish ? @"%ROOT%\publish\" + t : @"%ROOT%\bin\" + t);
                            L.AddRange(cd); L.Add(Q($"mkdir §{od}§ 2>nul")); L.Add($"set GOARCH={A.Go}"); L.Add("set CGO_ENABLED=0");
                            if (a == "x64" && rel && (o.Avx2 || o.Avx512)) L.Add($"set GOAMD64={(o.Avx512 ? "v4" : "v3")}");
                            L.Add(Q($"go build -trimpath {(rel ? (o.Pgo ? "-pgo=auto " : "") + "-ldflags=§-s -w§" : "-gcflags=§all=-N -l§")} -o §{od}\\\\§ ./..."));
                            break;
                        }
                }
                J.Add(new($"{a} - {cfg}", L));
            }

        foreach (var p in presets)   // presets do CMake: eles já definem arquitetura e configuração
        {
            bool dbg = Regex.IsMatch(p, "debug", RegexOptions.IgnoreCase), hb = ex.BuildPresets.Contains(p);
            var f = Flags(o with { Pgo = false }, dbg); var t = Slug("preset-" + p);
            var L = new List<string> { Q(@"call §%VSP%\VC\Auxiliary\Build\vcvarsall.bat§ x64 >nul") };
            if (f.cl != "") { L.Add(Q($"set §_CL_={f.cl}§")); L.Add(Q($"set §_LINK_={f.lk}§")); }
            var extra = hb ? "" : Q($" -B §build\\{t}§");
            L.Add(Q($"cmake --preset §{p}§{extra}"));
            L.Add(hb ? Q($"cmake --build --preset §{p}§") : Q($"cmake --build §build\\{t}§ --config Release --parallel"));
            J.Add(new("preset " + p, L));
        }
        return J;
    }

    // Preparação comum: pasta de saída, vcpkg, Visual Studio, arquivo de projeto escolhido
    IEnumerable<string> Pre(Cand c, Opts o, bool full)
    {
        var r = c.Rule;
        yield return "set \"ROOT=%CD%\"";
        if (o.Out != "") yield return Q($"for %%I in (§{o.Out}§) do set §OUTROOT=%%~fI§");
        if (hasVcpkg && r.Id == "cmake")
        {
            if (full)
            {
                yield return Q(@"if not exist §.vcpkg\vcpkg.exe§ git clone https://github.com/microsoft/vcpkg §.vcpkg§");
                yield return Q(@"if not exist §.vcpkg\vcpkg.exe§ call §.vcpkg\bootstrap-vcpkg.bat§ -disableMetrics");
            }
            yield return Q(@"if exist §.vcpkg\scripts\buildsystems\vcpkg.cmake§ set VCPKG_ARGS=-DCMAKE_TOOLCHAIN_FILE=§%CD%\.vcpkg\scripts\buildsystems\vcpkg.cmake§");
        }
        if (r.Kind == "msvc")
        {
            yield return @"set ""VSW=%ProgramFiles(x86)%\Microsoft Visual Studio\Installer\vswhere.exe""";
            yield return @"for /f ""usebackq delims="" %%I in (`""%VSW%"" -latest -products * -property installationPath`) do set ""VSP=%%I""";
        }
        if (r.Id == "sln")
        {
            yield return Q($"set §SLN={Win(c.Path)}§");
            yield return Q(@"for /d %%D in (§%VSP%\MSBuild\Microsoft\VC\v*§) do for /f %%T in ('dir /b /ad /o-n §%%D\Platforms\x64\PlatformToolsets\v*§ 2^>nul') do if not defined TS set §TS=%%T§");
            yield return Q("if not defined TS set §TS=v143§");
        }
        if (r.Id == "dotnet") yield return Q($"set §PROJ={Win(c.Path)}§");
    }

    // Clona, atualiza ou reaproveita a pasta (o script pode ser rodado várias vezes)
    IEnumerable<string> Fetch()
    {
        var u = curUrl == "" ? "<URL>" : curUrl;
        return new[] {
            "if exist \".git\" goto :here",
            $"if exist \"{repo}\\.git\" goto :upd",
            $"if exist \"{repo}\\\" goto :enter",
            $"git clone --recursive \"{u}\" \"{repo}\"",
            "if errorlevel 1 exit /b 1",
            "goto :enter",
            ":upd",
            "echo [INFO] Pasta ja existe, atualizando...",
            $"git -C \"{repo}\" pull --ff-only",
            ":enter",
            $"cd /d \"{repo}\"",
            "goto :ready",
            ":here",
            "echo [INFO] Usando o repositorio da pasta atual...",
            "git pull --ff-only",
            ":ready",
            "git submodule update --init --recursive" };
    }

    // k: "full" = clona + dependências + compila | "build" = só compila | "publish" = só publica
    string Build(string k)
    {
        var c = sel!; bool full = k == "full";
        var o = GetOpts(k); var J = Jobs(o, c); njobs = J.Count;
        var L = new List<string> { "@echo off", "setlocal EnableExtensions" };
        if (!full) L.Add("cd /d \"%~dp0\"");
        L.Add("set \"OKS=\""); L.Add("set \"FAILS=\"");
        if (full) L.AddRange(Fetch());
        L.AddRange(Pre(c, o, full));
        for (int i = 0; i < J.Count; i++)
        {
            var tag = Slug(J[i].Label);
            L.Add($"call :j{i}");
            L.Add($"if errorlevel 1 (set \"FAILS=%FAILS% {tag}\") else set \"OKS=%OKS% {tag}\"");
        }
        L.AddRange(new[] { "echo.", "echo ===== RESUMO =====", "echo OK:%OKS%", "echo FALHAS:%FAILS%", "pause", "if defined FAILS exit /b 1", "exit /b 0" });
        for (int i = 0; i < J.Count; i++)
        {
            L.Add($":j{i}"); L.Add("setlocal"); L.Add($"echo ===== [{i + 1}/{J.Count}] {J[i].Label} =====");
            foreach (var line in J[i].Lines) { L.Add(line); if (!NoChk.IsMatch(line)) L.Add("if errorlevel 1 exit /b 1"); }
            L.Add("exit /b 0");
        }
        return string.Join("\r\n", L);
    }

    void Render()
    {
        if (sel == null || loading) return;
        script = Build(kind);
        preview.Text = script; bScript.Enabled = bBuild.Enabled = bPub.Enabled = true;
        lblScript.Text = "SCRIPT .BAT GERADO (" + (kind == "full" ? "CLONAR + COMPILAR" : kind == "build" ? "SÓ BUILD" : "SÓ PUBLISH") + ")";
        result.Text = sel.Rule.Name;
        resultSub.Text = $"{sel.Path}  ·  {njobs} compilação(ões) no lote" + (cands.Count > 1 ? "  ·  outros arquivos encontrados: escolha em \"Sistema de build\"" : "");
    }

    // ---------- processos / salvar ----------
    Task<int> Run(string exe, string args)
    {
        var p = new Process
        {
            StartInfo = new ProcessStartInfo(exe, args) { RedirectStandardOutput = true, RedirectStandardError = true, UseShellExecute = false, CreateNoWindow = true },
            EnableRaisingEvents = true
        };
        var tcs = new TaskCompletionSource<int>();
        DataReceivedEventHandler h = (_, e) => { if (e.Data != null) BeginInvoke(() => Log("  " + e.Data, Color.Gray)); };
        p.OutputDataReceived += h; p.ErrorDataReceived += h;
        p.Exited += (_, _) => tcs.TrySetResult(p.ExitCode);
        try { p.Start(); p.BeginOutputReadLine(); p.BeginErrorReadLine(); }
        catch (Exception x) { Log("[ERRO] " + x.Message, Color.IndianRed); tcs.TrySetResult(-1); }
        return tcs.Task;
    }

    void Save(string k)
    {
        if (sel == null) return;
        kind = k; Render();   // o preview mostra exatamente o que será salvo
        using var dlg = new SaveFileDialog { FileName = k == "full" ? $"script-{repo}.bat" : $"{k}.bat", Filter = "Batch (*.bat)|*.bat" };
        if (dlg.ShowDialog() == DialogResult.OK) File.WriteAllText(dlg.FileName, script, Encoding.ASCII);
    }

    private void cardEx_Paint(object sender, PaintEventArgs e)
    {

    }
}
