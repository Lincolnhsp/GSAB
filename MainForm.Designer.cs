#nullable disable
// Arquivo gerenciado pelo Designer do Visual Studio (abra MainForm.cs e clique em "Design").
// Só usa controles padrão do WinForms, para o Designer conseguir ler e regravar sem perder nada.

partial class MainForm
{
    private System.ComponentModel.IContainer components = null;

    protected override void Dispose(bool disposing)
    {
        if (disposing && components != null) components.Dispose();
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
        root = new TableLayoutPanel();
        pnlTop = new TableLayoutPanel();
        url = new TextBox();
        bGit = new Button();
        bDir = new Button();
        cardResult = new FlowLayoutPanel();
        result = new Label();
        resultSub = new Label();
        pnlOpts = new FlowLayoutPanel();
        cardSys = new FlowLayoutPanel();
        lblSys = new Label();
        cmb = new ComboBox();
        cardCfg = new FlowLayoutPanel();
        lblCfg = new Label();
        cRel = new CheckBox();
        cDbg = new CheckBox();
        cardArch = new FlowLayoutPanel();
        lblArch = new Label();
        a86 = new CheckBox();
        a64 = new CheckBox();
        cardOut = new FlowLayoutPanel();
        lblOut = new Label();
        oOut = new TextBox();
        cardOpt = new FlowLayoutPanel();
        lblOpt = new Label();
        oLto = new CheckBox();
        oPgo = new CheckBox();
        oAvx2 = new CheckBox();
        oAvx512 = new CheckBox();
        oFast = new CheckBox();
        oPar = new CheckBox();
        oExtra = new TextBox();
        cardEx = new FlowLayoutPanel();
        lblEx = new Label();
        clb = new CheckedListBox();
        cardPub = new FlowLayoutPanel();
        lblPub = new Label();
        pSc = new CheckBox();
        pSingle = new CheckBox();
        pTrim = new CheckBox();
        pComp = new CheckBox();
        pR2r = new CheckBox();
        pnlBody = new TableLayoutPanel();
        lblLog = new Label();
        lblScript = new Label();
        pnlLog = new Panel();
        log = new RichTextBox();
        preview = new TextBox();
        pnlButtons = new FlowLayoutPanel();
        bScript = new Button();
        bBuild = new Button();
        bPub = new Button();
        root.SuspendLayout();
        pnlTop.SuspendLayout();
        cardResult.SuspendLayout();
        pnlOpts.SuspendLayout();
        cardSys.SuspendLayout();
        cardCfg.SuspendLayout();
        cardArch.SuspendLayout();
        cardOut.SuspendLayout();
        cardOpt.SuspendLayout();
        cardEx.SuspendLayout();
        cardPub.SuspendLayout();
        pnlBody.SuspendLayout();
        pnlLog.SuspendLayout();
        pnlButtons.SuspendLayout();
        SuspendLayout();
        // 
        // root
        // 
        root.BackColor = Color.FromArgb(246, 247, 249);
        root.ColumnCount = 1;
        root.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        root.Controls.Add(pnlTop, 0, 0);
        root.Controls.Add(cardResult, 0, 1);
        root.Controls.Add(pnlOpts, 0, 2);
        root.Controls.Add(pnlBody, 0, 3);
        root.Dock = DockStyle.Fill;
        root.Location = new Point(0, 0);
        root.Name = "root";
        root.Padding = new Padding(16);
        root.RowCount = 4;
        root.RowStyles.Add(new RowStyle());
        root.RowStyles.Add(new RowStyle());
        root.RowStyles.Add(new RowStyle());
        root.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        root.Size = new Size(1377, 800);
        root.TabIndex = 0;
        // 
        // pnlTop
        // 
        pnlTop.AutoSize = true;
        pnlTop.ColumnCount = 3;
        pnlTop.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        pnlTop.ColumnStyles.Add(new ColumnStyle());
        pnlTop.ColumnStyles.Add(new ColumnStyle());
        pnlTop.Controls.Add(url, 0, 0);
        pnlTop.Controls.Add(bGit, 1, 0);
        pnlTop.Controls.Add(bDir, 2, 0);
        pnlTop.Dock = DockStyle.Top;
        pnlTop.Location = new Point(16, 16);
        pnlTop.Margin = new Padding(0, 0, 0, 10);
        pnlTop.Name = "pnlTop";
        pnlTop.RowCount = 1;
        pnlTop.RowStyles.Add(new RowStyle());
        pnlTop.Size = new Size(1345, 35);
        pnlTop.TabIndex = 0;
        // 
        // url
        // 
        url.Anchor = AnchorStyles.Left | AnchorStyles.Right;
        url.BorderStyle = BorderStyle.FixedSingle;
        url.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
        url.Location = new Point(0, 3);
        url.Margin = new Padding(0, 1, 0, 0);
        url.Name = "url";
        url.PlaceholderText = "https://github.com/usuario/repositorio.git";
        url.Size = new Size(1107, 29);
        url.TabIndex = 0;
        // 
        // bGit
        // 
        bGit.AutoSize = true;
        bGit.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        bGit.BackColor = Color.FromArgb(37, 99, 235);
        bGit.Cursor = Cursors.Hand;
        bGit.FlatAppearance.BorderSize = 0;
        bGit.FlatStyle = FlatStyle.Flat;
        bGit.ForeColor = Color.White;
        bGit.Location = new Point(1115, 0);
        bGit.Margin = new Padding(8, 0, 0, 0);
        bGit.Name = "bGit";
        bGit.Padding = new Padding(10, 3, 10, 3);
        bGit.Size = new Size(111, 33);
        bGit.TabIndex = 1;
        bGit.Text = "Analisar URL";
        bGit.UseVisualStyleBackColor = false;
        // 
        // bDir
        // 
        bDir.AutoSize = true;
        bDir.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        bDir.BackColor = Color.White;
        bDir.Cursor = Cursors.Hand;
        bDir.FlatAppearance.BorderColor = Color.FromArgb(37, 99, 235);
        bDir.FlatStyle = FlatStyle.Flat;
        bDir.ForeColor = Color.FromArgb(37, 99, 235);
        bDir.Location = new Point(1234, 0);
        bDir.Margin = new Padding(8, 0, 0, 0);
        bDir.Name = "bDir";
        bDir.Padding = new Padding(10, 3, 10, 3);
        bDir.Size = new Size(111, 35);
        bDir.TabIndex = 2;
        bDir.Text = "Pasta local…";
        bDir.UseVisualStyleBackColor = false;
        // 
        // cardResult
        // 
        cardResult.AutoSize = true;
        cardResult.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        cardResult.BackColor = Color.White;
        cardResult.BorderStyle = BorderStyle.FixedSingle;
        cardResult.Controls.Add(result);
        cardResult.Controls.Add(resultSub);
        cardResult.Dock = DockStyle.Top;
        cardResult.FlowDirection = FlowDirection.TopDown;
        cardResult.Location = new Point(16, 61);
        cardResult.Margin = new Padding(0, 0, 0, 10);
        cardResult.Name = "cardResult";
        cardResult.Padding = new Padding(14, 10, 14, 10);
        cardResult.Size = new Size(1345, 64);
        cardResult.TabIndex = 1;
        cardResult.WrapContents = false;
        // 
        // result
        // 
        result.AutoSize = true;
        result.Font = new Font("Segoe UI", 13F, FontStyle.Bold);
        result.ForeColor = Color.FromArgb(28, 30, 33);
        result.Location = new Point(17, 10);
        result.Name = "result";
        result.Size = new Size(198, 25);
        result.TabIndex = 0;
        result.Text = "Aguardando análise…";
        // 
        // resultSub
        // 
        resultSub.AutoSize = true;
        resultSub.ForeColor = Color.FromArgb(110, 115, 125);
        resultSub.Location = new Point(17, 35);
        resultSub.Name = "resultSub";
        resultSub.Size = new Size(0, 17);
        resultSub.TabIndex = 1;
        // 
        // pnlOpts
        // 
        pnlOpts.AutoSize = true;
        pnlOpts.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        pnlOpts.Controls.Add(cardSys);
        pnlOpts.Controls.Add(cardCfg);
        pnlOpts.Controls.Add(cardArch);
        pnlOpts.Controls.Add(cardOut);
        pnlOpts.Controls.Add(cardOpt);
        pnlOpts.Controls.Add(cardEx);
        pnlOpts.Controls.Add(cardPub);
        pnlOpts.Dock = DockStyle.Top;
        pnlOpts.Location = new Point(16, 135);
        pnlOpts.Margin = new Padding(0);
        pnlOpts.Name = "pnlOpts";
        pnlOpts.Size = new Size(1345, 235);
        pnlOpts.TabIndex = 2;
        // 
        // cardSys
        // 
        cardSys.AutoSize = true;
        cardSys.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        cardSys.BackColor = Color.White;
        cardSys.BorderStyle = BorderStyle.FixedSingle;
        cardSys.Controls.Add(lblSys);
        cardSys.Controls.Add(cmb);
        cardSys.FlowDirection = FlowDirection.TopDown;
        cardSys.Location = new Point(0, 0);
        cardSys.Margin = new Padding(0, 0, 10, 10);
        cardSys.Name = "cardSys";
        cardSys.Padding = new Padding(10, 6, 10, 8);
        cardSys.Size = new Size(248, 64);
        cardSys.TabIndex = 0;
        cardSys.WrapContents = false;
        // 
        // lblSys
        // 
        lblSys.AutoSize = true;
        lblSys.Font = new Font("Segoe UI", 8F, FontStyle.Bold);
        lblSys.ForeColor = Color.FromArgb(110, 115, 125);
        lblSys.Location = new Point(10, 6);
        lblSys.Margin = new Padding(0, 0, 0, 4);
        lblSys.Name = "lblSys";
        lblSys.Size = new Size(105, 13);
        lblSys.TabIndex = 0;
        lblSys.Text = "SISTEMA DE BUILD";
        // 
        // cmb
        // 
        cmb.DropDownStyle = ComboBoxStyle.DropDownList;
        cmb.Location = new Point(13, 26);
        cmb.Name = "cmb";
        cmb.Size = new Size(220, 25);
        cmb.TabIndex = 1;
        // 
        // cardCfg
        // 
        cardCfg.AutoSize = true;
        cardCfg.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        cardCfg.BackColor = Color.White;
        cardCfg.BorderStyle = BorderStyle.FixedSingle;
        cardCfg.Controls.Add(lblCfg);
        cardCfg.Controls.Add(cRel);
        cardCfg.Controls.Add(cDbg);
        cardCfg.FlowDirection = FlowDirection.TopDown;
        cardCfg.Location = new Point(258, 0);
        cardCfg.Margin = new Padding(0, 0, 10, 10);
        cardCfg.Name = "cardCfg";
        cardCfg.Padding = new Padding(10, 6, 10, 8);
        cardCfg.Size = new Size(116, 87);
        cardCfg.TabIndex = 2;
        cardCfg.WrapContents = false;
        // 
        // lblCfg
        // 
        lblCfg.AutoSize = true;
        lblCfg.Font = new Font("Segoe UI", 8F, FontStyle.Bold);
        lblCfg.ForeColor = Color.FromArgb(110, 115, 125);
        lblCfg.Location = new Point(10, 6);
        lblCfg.Margin = new Padding(0, 0, 0, 4);
        lblCfg.Name = "lblCfg";
        lblCfg.Size = new Size(94, 13);
        lblCfg.TabIndex = 0;
        lblCfg.Text = "CONFIGURAÇÃO";
        // 
        // cRel
        // 
        cRel.AutoSize = true;
        cRel.Checked = true;
        cRel.CheckState = CheckState.Checked;
        cRel.Location = new Point(13, 26);
        cRel.Name = "cRel";
        cRel.Size = new Size(72, 21);
        cRel.TabIndex = 1;
        cRel.Text = "Release";
        // 
        // cDbg
        // 
        cDbg.AutoSize = true;
        cDbg.Location = new Point(13, 53);
        cDbg.Name = "cDbg";
        cDbg.Size = new Size(66, 21);
        cDbg.TabIndex = 2;
        cDbg.Text = "Debug";
        // 
        // cardArch
        // 
        cardArch.AutoSize = true;
        cardArch.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        cardArch.BackColor = Color.White;
        cardArch.BorderStyle = BorderStyle.FixedSingle;
        cardArch.Controls.Add(lblArch);
        cardArch.Controls.Add(a86);
        cardArch.Controls.Add(a64);
        cardArch.FlowDirection = FlowDirection.TopDown;
        cardArch.Location = new Point(384, 0);
        cardArch.Margin = new Padding(0, 0, 10, 10);
        cardArch.Name = "cardArch";
        cardArch.Padding = new Padding(10, 6, 10, 8);
        cardArch.Size = new Size(126, 87);
        cardArch.TabIndex = 1;
        cardArch.WrapContents = false;
        // 
        // lblArch
        // 
        lblArch.AutoSize = true;
        lblArch.Font = new Font("Segoe UI", 8F, FontStyle.Bold);
        lblArch.ForeColor = Color.FromArgb(110, 115, 125);
        lblArch.Location = new Point(10, 6);
        lblArch.Margin = new Padding(0, 0, 0, 4);
        lblArch.Name = "lblArch";
        lblArch.Size = new Size(82, 13);
        lblArch.TabIndex = 0;
        lblArch.Text = "ARQUITETURA";
        // 
        // a86
        // 
        a86.AutoSize = true;
        a86.Location = new Point(13, 26);
        a86.Name = "a86";
        a86.Size = new Size(98, 21);
        a86.TabIndex = 1;
        a86.Text = "x86 (32 bits)";
        // 
        // a64
        // 
        a64.AutoSize = true;
        a64.Checked = true;
        a64.CheckState = CheckState.Checked;
        a64.Location = new Point(13, 53);
        a64.Name = "a64";
        a64.Size = new Size(98, 21);
        a64.TabIndex = 2;
        a64.Text = "x64 (64 bits)";
        // 
        // cardOut
        // 
        cardOut.AutoSize = true;
        cardOut.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        cardOut.BackColor = Color.White;
        cardOut.BorderStyle = BorderStyle.FixedSingle;
        cardOut.Controls.Add(lblOut);
        cardOut.Controls.Add(oOut);
        cardOut.FlowDirection = FlowDirection.TopDown;
        cardOut.Location = new Point(520, 0);
        cardOut.Margin = new Padding(0, 0, 10, 10);
        cardOut.Name = "cardOut";
        cardOut.Padding = new Padding(10, 6, 10, 8);
        cardOut.Size = new Size(185, 63);
        cardOut.TabIndex = 6;
        cardOut.WrapContents = false;
        // 
        // lblOut
        // 
        lblOut.AutoSize = true;
        lblOut.Font = new Font("Segoe UI", 8F, FontStyle.Bold);
        lblOut.ForeColor = Color.FromArgb(110, 115, 125);
        lblOut.Location = new Point(10, 6);
        lblOut.Margin = new Padding(0, 0, 0, 4);
        lblOut.Name = "lblOut";
        lblOut.Size = new Size(160, 13);
        lblOut.TabIndex = 0;
        lblOut.Text = "PASTA DE SAÍDA (OPCIONAL)";
        // 
        // oOut
        // 
        oOut.Location = new Point(13, 26);
        oOut.Name = "oOut";
        oOut.PlaceholderText = ".\\BIN  ou  .\\APP";
        oOut.Size = new Size(157, 24);
        oOut.TabIndex = 1;
        // 
        // cardOpt
        // 
        cardOpt.AutoSize = true;
        cardOpt.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        cardOpt.BackColor = Color.White;
        cardOpt.BorderStyle = BorderStyle.FixedSingle;
        cardOpt.Controls.Add(lblOpt);
        cardOpt.Controls.Add(oLto);
        cardOpt.Controls.Add(oPgo);
        cardOpt.Controls.Add(oAvx2);
        cardOpt.Controls.Add(oAvx512);
        cardOpt.Controls.Add(oFast);
        cardOpt.Controls.Add(oPar);
        cardOpt.Controls.Add(oExtra);
        cardOpt.FlowDirection = FlowDirection.TopDown;
        cardOpt.Location = new Point(715, 0);
        cardOpt.Margin = new Padding(0, 0, 10, 10);
        cardOpt.Name = "cardOpt";
        cardOpt.Padding = new Padding(10, 6, 10, 8);
        cardOpt.Size = new Size(204, 225);
        cardOpt.TabIndex = 4;
        cardOpt.WrapContents = false;
        // 
        // lblOpt
        // 
        lblOpt.AutoSize = true;
        lblOpt.Font = new Font("Segoe UI", 8F, FontStyle.Bold);
        lblOpt.ForeColor = Color.FromArgb(110, 115, 125);
        lblOpt.Location = new Point(10, 6);
        lblOpt.Margin = new Padding(0, 0, 0, 4);
        lblOpt.Name = "lblOpt";
        lblOpt.Size = new Size(136, 13);
        lblOpt.TabIndex = 0;
        lblOpt.Text = "OTIMIZAÇÕES (RELEASE)";
        // 
        // oLto
        // 
        oLto.AutoSize = true;
        oLto.Location = new Point(13, 26);
        oLto.Name = "oLto";
        oLto.Size = new Size(89, 21);
        oLto.TabIndex = 1;
        oLto.Text = "LTCG / LTO";
        // 
        // oPgo
        // 
        oPgo.AutoSize = true;
        oPgo.Location = new Point(13, 53);
        oPgo.Name = "oPgo";
        oPgo.Size = new Size(176, 21);
        oPgo.TabIndex = 2;
        oPgo.Text = "PGO (2 fases, com treino)";
        // 
        // oAvx2
        // 
        oAvx2.AutoSize = true;
        oAvx2.Location = new Point(13, 80);
        oAvx2.Name = "oAvx2";
        oAvx2.Size = new Size(57, 21);
        oAvx2.TabIndex = 3;
        oAvx2.Text = "AVX2";
        // 
        // oAvx512
        // 
        oAvx512.AutoSize = true;
        oAvx512.Location = new Point(13, 107);
        oAvx512.Name = "oAvx512";
        oAvx512.Size = new Size(76, 21);
        oAvx512.TabIndex = 4;
        oAvx512.Text = "AVX-512";
        // 
        // oFast
        // 
        oFast.AutoSize = true;
        oFast.Location = new Point(13, 134);
        oFast.Name = "oFast";
        oFast.Size = new Size(68, 21);
        oFast.TabIndex = 5;
        oFast.Text = "/fp:fast";
        // 
        // oPar
        // 
        oPar.AutoSize = true;
        oPar.Location = new Point(13, 161);
        oPar.Name = "oPar";
        oPar.Size = new Size(62, 21);
        oPar.TabIndex = 6;
        oPar.Text = "/Qpar";
        // 
        // oExtra
        // 
        oExtra.Location = new Point(13, 188);
        oExtra.Name = "oExtra";
        oExtra.PlaceholderText = "flags extras (ex: /Ob3)";
        oExtra.Size = new Size(176, 24);
        oExtra.TabIndex = 7;
        // 
        // cardEx
        // 
        cardEx.AutoSize = true;
        cardEx.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        cardEx.BackColor = Color.White;
        cardEx.BorderStyle = BorderStyle.FixedSingle;
        cardEx.Controls.Add(lblEx);
        cardEx.Controls.Add(clb);
        cardEx.FlowDirection = FlowDirection.TopDown;
        cardEx.Location = new Point(929, 0);
        cardEx.Margin = new Padding(0, 0, 10, 10);
        cardEx.Name = "cardEx";
        cardEx.Padding = new Padding(10, 6, 10, 8);
        cardEx.Size = new Size(163, 212);
        cardEx.TabIndex = 3;
        cardEx.WrapContents = false;
        cardEx.Paint += cardEx_Paint;
        // 
        // lblEx
        // 
        lblEx.AutoSize = true;
        lblEx.Font = new Font("Segoe UI", 8F, FontStyle.Bold);
        lblEx.ForeColor = Color.FromArgb(110, 115, 125);
        lblEx.Location = new Point(10, 6);
        lblEx.Margin = new Padding(0, 0, 0, 4);
        lblEx.Name = "lblEx";
        lblEx.Size = new Size(138, 13);
        lblEx.TabIndex = 0;
        lblEx.Text = "DEFINIÇÕES DO PROJETO";
        // 
        // clb
        // 
        clb.BorderStyle = BorderStyle.FixedSingle;
        clb.CheckOnClick = true;
        clb.Location = new Point(13, 26);
        clb.Name = "clb";
        clb.Size = new Size(135, 173);
        clb.TabIndex = 1;
        // 
        // cardPub
        // 
        cardPub.AutoSize = true;
        cardPub.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        cardPub.BackColor = Color.White;
        cardPub.BorderStyle = BorderStyle.FixedSingle;
        cardPub.Controls.Add(lblPub);
        cardPub.Controls.Add(pSc);
        cardPub.Controls.Add(pSingle);
        cardPub.Controls.Add(pTrim);
        cardPub.Controls.Add(pComp);
        cardPub.Controls.Add(pR2r);
        pnlOpts.SetFlowBreak(cardPub, true);
        cardPub.FlowDirection = FlowDirection.TopDown;
        cardPub.Location = new Point(1102, 0);
        cardPub.Margin = new Padding(0, 0, 10, 10);
        cardPub.Name = "cardPub";
        cardPub.Padding = new Padding(10, 6, 10, 8);
        cardPub.Size = new Size(231, 168);
        cardPub.TabIndex = 5;
        cardPub.WrapContents = false;
        // 
        // lblPub
        // 
        lblPub.AutoSize = true;
        lblPub.Font = new Font("Segoe UI", 8F, FontStyle.Bold);
        lblPub.ForeColor = Color.FromArgb(110, 115, 125);
        lblPub.Location = new Point(10, 6);
        lblPub.Margin = new Padding(0, 0, 0, 4);
        lblPub.Name = "lblPub";
        lblPub.Size = new Size(149, 13);
        lblPub.TabIndex = 0;
        lblPub.Text = "OPÇÕES DE PUBLISH (.NET)";
        // 
        // pSc
        // 
        pSc.AutoSize = true;
        pSc.Location = new Point(13, 26);
        pSc.Name = "pSc";
        pSc.Size = new Size(110, 21);
        pSc.TabIndex = 1;
        pSc.Text = "Self-contained";
        // 
        // pSingle
        // 
        pSingle.AutoSize = true;
        pSingle.Location = new Point(13, 53);
        pSingle.Name = "pSingle";
        pSingle.Size = new Size(173, 21);
        pSingle.TabIndex = 2;
        pSingle.Text = "Arquivo único (SingleFile)";
        // 
        // pTrim
        // 
        pTrim.AutoSize = true;
        pTrim.Location = new Point(13, 80);
        pTrim.Name = "pTrim";
        pTrim.Size = new Size(190, 21);
        pTrim.TabIndex = 3;
        pTrim.Text = "Trimmed (reduz o tamanho)";
        // 
        // pComp
        // 
        pComp.AutoSize = true;
        pComp.Location = new Point(13, 107);
        pComp.Name = "pComp";
        pComp.Size = new Size(203, 21);
        pComp.TabIndex = 4;
        pComp.Text = "Compressão no arquivo único";
        // 
        // pR2r
        // 
        pR2r.AutoSize = true;
        pR2r.Checked = true;
        pR2r.CheckState = CheckState.Checked;
        pR2r.Location = new Point(13, 134);
        pR2r.Name = "pR2r";
        pR2r.Size = new Size(99, 21);
        pR2r.TabIndex = 5;
        pR2r.Text = "ReadyToRun";
        // 
        // pnlBody
        // 
        pnlBody.ColumnCount = 2;
        pnlBody.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
        pnlBody.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
        pnlBody.Controls.Add(lblLog, 0, 0);
        pnlBody.Controls.Add(lblScript, 1, 0);
        pnlBody.Controls.Add(pnlLog, 0, 1);
        pnlBody.Controls.Add(preview, 1, 1);
        pnlBody.Controls.Add(pnlButtons, 1, 2);
        pnlBody.Dock = DockStyle.Fill;
        pnlBody.Location = new Point(16, 370);
        pnlBody.Margin = new Padding(0);
        pnlBody.Name = "pnlBody";
        pnlBody.RowCount = 3;
        pnlBody.RowStyles.Add(new RowStyle());
        pnlBody.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        pnlBody.RowStyles.Add(new RowStyle());
        pnlBody.Size = new Size(1345, 414);
        pnlBody.TabIndex = 3;
        // 
        // lblLog
        // 
        lblLog.AutoSize = true;
        lblLog.Font = new Font("Segoe UI", 8F, FontStyle.Bold);
        lblLog.ForeColor = Color.FromArgb(110, 115, 125);
        lblLog.Location = new Point(0, 0);
        lblLog.Margin = new Padding(0, 0, 0, 4);
        lblLog.Name = "lblLog";
        lblLog.Size = new Size(62, 13);
        lblLog.TabIndex = 0;
        lblLog.Text = "PROCESSO";
        // 
        // lblScript
        // 
        lblScript.AutoSize = true;
        lblScript.Font = new Font("Segoe UI", 8F, FontStyle.Bold);
        lblScript.ForeColor = Color.FromArgb(110, 115, 125);
        lblScript.Location = new Point(678, 0);
        lblScript.Margin = new Padding(6, 0, 0, 4);
        lblScript.Name = "lblScript";
        lblScript.Size = new Size(117, 13);
        lblScript.TabIndex = 1;
        lblScript.Text = "SCRIPT .BAT GERADO";
        // 
        // pnlLog
        // 
        pnlLog.BackColor = Color.FromArgb(16, 18, 20);
        pnlLog.Controls.Add(log);
        pnlLog.Dock = DockStyle.Fill;
        pnlLog.Location = new Point(0, 17);
        pnlLog.Margin = new Padding(0, 0, 6, 0);
        pnlLog.Name = "pnlLog";
        pnlLog.Padding = new Padding(10);
        pnlLog.Size = new Size(666, 354);
        pnlLog.TabIndex = 2;
        // 
        // log
        // 
        log.BackColor = Color.FromArgb(16, 18, 20);
        log.BorderStyle = BorderStyle.None;
        log.Dock = DockStyle.Fill;
        log.Font = new Font("Consolas", 10F);
        log.ForeColor = Color.Gainsboro;
        log.Location = new Point(10, 10);
        log.Name = "log";
        log.ReadOnly = true;
        log.Size = new Size(646, 334);
        log.TabIndex = 0;
        log.Text = "";
        // 
        // preview
        // 
        preview.BackColor = Color.White;
        preview.BorderStyle = BorderStyle.FixedSingle;
        preview.Dock = DockStyle.Fill;
        preview.Font = new Font("Consolas", 9.5F);
        preview.Location = new Point(678, 17);
        preview.Margin = new Padding(6, 0, 0, 0);
        preview.Multiline = true;
        preview.Name = "preview";
        preview.ReadOnly = true;
        preview.ScrollBars = ScrollBars.Both;
        preview.Size = new Size(667, 354);
        preview.TabIndex = 3;
        preview.WordWrap = false;
        // 
        // pnlButtons
        // 
        pnlButtons.AutoSize = true;
        pnlButtons.Controls.Add(bScript);
        pnlButtons.Controls.Add(bBuild);
        pnlButtons.Controls.Add(bPub);
        pnlButtons.Dock = DockStyle.Fill;
        pnlButtons.Location = new Point(678, 379);
        pnlButtons.Margin = new Padding(6, 8, 0, 0);
        pnlButtons.Name = "pnlButtons";
        pnlButtons.Size = new Size(667, 35);
        pnlButtons.TabIndex = 4;
        // 
        // bScript
        // 
        bScript.AutoSize = true;
        bScript.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        bScript.BackColor = Color.FromArgb(37, 99, 235);
        bScript.Cursor = Cursors.Hand;
        bScript.FlatAppearance.BorderSize = 0;
        bScript.FlatStyle = FlatStyle.Flat;
        bScript.ForeColor = Color.White;
        bScript.Location = new Point(0, 0);
        bScript.Margin = new Padding(0, 0, 8, 0);
        bScript.Name = "bScript";
        bScript.Padding = new Padding(10, 3, 10, 3);
        bScript.Size = new Size(135, 33);
        bScript.TabIndex = 0;
        bScript.Text = "Salvar Script BAT";
        bScript.UseVisualStyleBackColor = false;
        // 
        // bBuild
        // 
        bBuild.AutoSize = true;
        bBuild.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        bBuild.BackColor = Color.White;
        bBuild.Cursor = Cursors.Hand;
        bBuild.FlatAppearance.BorderColor = Color.FromArgb(37, 99, 235);
        bBuild.FlatStyle = FlatStyle.Flat;
        bBuild.ForeColor = Color.FromArgb(37, 99, 235);
        bBuild.Location = new Point(143, 0);
        bBuild.Margin = new Padding(0, 0, 8, 0);
        bBuild.Name = "bBuild";
        bBuild.Padding = new Padding(10, 3, 10, 3);
        bBuild.Size = new Size(107, 35);
        bBuild.TabIndex = 1;
        bBuild.Text = "Salvar Build";
        bBuild.UseVisualStyleBackColor = false;
        // 
        // bPub
        // 
        bPub.AutoSize = true;
        bPub.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        bPub.BackColor = Color.White;
        bPub.Cursor = Cursors.Hand;
        bPub.FlatAppearance.BorderColor = Color.FromArgb(37, 99, 235);
        bPub.FlatStyle = FlatStyle.Flat;
        bPub.ForeColor = Color.FromArgb(37, 99, 235);
        bPub.Location = new Point(258, 0);
        bPub.Margin = new Padding(0);
        bPub.Name = "bPub";
        bPub.Padding = new Padding(10, 3, 10, 3);
        bPub.Size = new Size(120, 35);
        bPub.TabIndex = 2;
        bPub.Text = "Salvar Publish";
        bPub.UseVisualStyleBackColor = false;
        // 
        // MainForm
        // 
        AutoScaleDimensions = new SizeF(96F, 96F);
        AutoScaleMode = AutoScaleMode.Dpi;
        BackColor = Color.FromArgb(246, 247, 249);
        ClientSize = new Size(1377, 800);
        Controls.Add(root);
        Font = new Font("Segoe UI", 9.5F);
        Icon = (Icon)resources.GetObject("$this.Icon");
        MinimumSize = new Size(960, 640);
        Name = "MainForm";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "Git Auto Builder";
        root.ResumeLayout(false);
        root.PerformLayout();
        pnlTop.ResumeLayout(false);
        pnlTop.PerformLayout();
        cardResult.ResumeLayout(false);
        cardResult.PerformLayout();
        pnlOpts.ResumeLayout(false);
        pnlOpts.PerformLayout();
        cardSys.ResumeLayout(false);
        cardSys.PerformLayout();
        cardCfg.ResumeLayout(false);
        cardCfg.PerformLayout();
        cardArch.ResumeLayout(false);
        cardArch.PerformLayout();
        cardOut.ResumeLayout(false);
        cardOut.PerformLayout();
        cardOpt.ResumeLayout(false);
        cardOpt.PerformLayout();
        cardEx.ResumeLayout(false);
        cardEx.PerformLayout();
        cardPub.ResumeLayout(false);
        cardPub.PerformLayout();
        pnlBody.ResumeLayout(false);
        pnlBody.PerformLayout();
        pnlLog.ResumeLayout(false);
        pnlButtons.ResumeLayout(false);
        pnlButtons.PerformLayout();
        ResumeLayout(false);
    }

    private TableLayoutPanel root;
    private TableLayoutPanel pnlTop;
    private TextBox url;
    private Button bGit;
    private Button bDir;
    private FlowLayoutPanel cardResult;
    private Label result;
    private Label resultSub;
    private FlowLayoutPanel pnlOpts;
    private FlowLayoutPanel cardSys;
    private Label lblSys;
    private ComboBox cmb;
    private FlowLayoutPanel cardArch;
    private Label lblArch;
    private CheckBox a86;
    private CheckBox a64;
    private FlowLayoutPanel cardCfg;
    private Label lblCfg;
    private CheckBox cRel;
    private CheckBox cDbg;
    private FlowLayoutPanel cardEx;
    private Label lblEx;
    private CheckedListBox clb;
    private FlowLayoutPanel cardOpt;
    private Label lblOpt;
    private CheckBox oLto;
    private CheckBox oPgo;
    private CheckBox oAvx2;
    private CheckBox oAvx512;
    private CheckBox oFast;
    private CheckBox oPar;
    private TextBox oExtra;
    private TableLayoutPanel pnlBody;
    private Label lblLog;
    private Label lblScript;
    private Panel pnlLog;
    private RichTextBox log;
    private TextBox preview;
    private FlowLayoutPanel pnlButtons;
    private Button bBuild;
    private Button bPub;
    private FlowLayoutPanel cardOut;
    private Label lblOut;
    private TextBox oOut;
    private Button bScript;
    private FlowLayoutPanel cardPub;
    private Label lblPub;
    private CheckBox pSc;
    private CheckBox pSingle;
    private CheckBox pTrim;
    private CheckBox pComp;
    private CheckBox pR2r;
}
