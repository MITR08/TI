namespace Lab3WinForms;

partial class MainForm
{
    private System.ComponentModel.IContainer components = null!;

    private Panel panelHeader;
    private Label lblSubtitle;
    private Panel panelMain;
    private Panel panelPreview;
    private Label lblTitle;
    private Label lblParams;
    private TextBox txtP;
    private TextBox txtX;
    private TextBox txtK;
    private Button btnFindRoots;
    private Label lblRoots;
    private ComboBox cmbG;
    private Label lblEncrypt;
    private TextBox txtEncryptPath;
    private Button btnBrowseEncrypt;
    private Button btnEncrypt;
    private Label lblDecrypt;
    private TextBox txtDecryptPath;
    private Button btnBrowseDecrypt;
    private Button btnDecrypt;
    private Label lblPreview;
    private TextBox txtPreview;
    private Label lblStatus;

    protected override void Dispose(bool disposing)
    {
        if (disposing && components != null)
            components.Dispose();
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        panelHeader = new Panel();
        lblSubtitle = new Label();
        panelMain = new Panel();
        panelPreview = new Panel();
        lblTitle = new Label();
        lblParams = new Label();
        txtP = new TextBox();
        txtX = new TextBox();
        txtK = new TextBox();
        btnFindRoots = new Button();
        lblRoots = new Label();
        cmbG = new ComboBox();
        lblEncrypt = new Label();
        txtEncryptPath = new TextBox();
        btnBrowseEncrypt = new Button();
        btnEncrypt = new Button();
        lblDecrypt = new Label();
        txtDecryptPath = new TextBox();
        btnBrowseDecrypt = new Button();
        btnDecrypt = new Button();
        lblPreview = new Label();
        txtPreview = new TextBox();
        lblStatus = new Label();
        panelHeader.SuspendLayout();
        panelMain.SuspendLayout();
        panelPreview.SuspendLayout();
        SuspendLayout();

        panelHeader.BackColor = Color.FromArgb(236, 240, 248);
        panelHeader.Controls.Add(lblSubtitle);
        panelHeader.Controls.Add(lblTitle);
        panelHeader.Location = new Point(16, 8);
        panelHeader.Name = "panelHeader";
        panelHeader.Size = new Size(1060, 1);
        panelHeader.TabIndex = 0;
        panelHeader.Visible = false;

        lblTitle.AutoSize = true;
        lblTitle.Font = new Font("Segoe UI Semibold", 18F, FontStyle.Bold, GraphicsUnit.Point, 204);
        lblTitle.ForeColor = Color.FromArgb(102, 204, 255);
        lblTitle.Location = new Point(20, 12);
        lblTitle.Name = "lblTitle";
        lblTitle.Size = new Size(300, 41);
        lblTitle.TabIndex = 0;
        lblTitle.Text = "Эль-Гамаль";

        lblSubtitle.AutoSize = true;
        lblSubtitle.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point, 204);
        lblSubtitle.ForeColor = Color.FromArgb(200, 212, 230);
        lblSubtitle.Location = new Point(22, 55);
        lblSubtitle.Name = "lblSubtitle";
        lblSubtitle.Size = new Size(453, 23);
        lblSubtitle.TabIndex = 1;
        lblSubtitle.Text = "";

        panelMain.BackColor = Color.FromArgb(248, 250, 252);
        panelMain.Controls.Add(lblParams);
        panelMain.Controls.Add(txtP);
        panelMain.Controls.Add(txtX);
        panelMain.Controls.Add(txtK);
        panelMain.Controls.Add(btnFindRoots);
        panelMain.Controls.Add(lblRoots);
        panelMain.Controls.Add(cmbG);
        panelMain.Controls.Add(lblEncrypt);
        panelMain.Controls.Add(txtEncryptPath);
        panelMain.Controls.Add(btnBrowseEncrypt);
        panelMain.Controls.Add(btnEncrypt);
        panelMain.Controls.Add(lblDecrypt);
        panelMain.Controls.Add(txtDecryptPath);
        panelMain.Controls.Add(btnBrowseDecrypt);
        panelMain.Controls.Add(btnDecrypt);
        panelMain.Controls.Add(lblStatus);
        panelMain.Location = new Point(16, 16);
        panelMain.Name = "panelMain";
        panelMain.Size = new Size(1060, 460);
        panelMain.TabIndex = 1;
        panelMain.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;

        panelPreview.BackColor = Color.FromArgb(248, 250, 252);
        panelPreview.Controls.Add(lblPreview);
        panelPreview.Controls.Add(txtPreview);
        panelPreview.Location = new Point(16, 490);
        panelPreview.Name = "panelPreview";
        panelPreview.Size = new Size(1060, 230);
        panelPreview.TabIndex = 2;
        panelPreview.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom;

        lblParams.AutoSize = true;
        lblParams.Font = new Font("Segoe UI Semibold", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 204);
        lblParams.ForeColor = Color.FromArgb(49, 61, 89);
        lblParams.Location = new Point(20, 16);
        lblParams.Name = "lblParams";
        lblParams.Size = new Size(296, 23);
        lblParams.Text = "Параметры p, x, k (10-я система)";

        txtP.BackColor = Color.White;
        txtP.BorderStyle = BorderStyle.FixedSingle;
        txtP.ForeColor = Color.FromArgb(32, 40, 56);
        txtP.Location = new Point(20, 50);
        txtP.Name = "txtP";
        txtP.PlaceholderText = "p (простое, 258..65536)";
        txtP.Size = new Size(220, 27);
        txtP.TabIndex = 1;

        txtX.BackColor = Color.White;
        txtX.BorderStyle = BorderStyle.FixedSingle;
        txtX.ForeColor = Color.FromArgb(32, 40, 56);
        txtX.Location = new Point(252, 50);
        txtX.Name = "txtX";
        txtX.PlaceholderText = "x (секретный ключ, 2..p−2)";
        txtX.Size = new Size(240, 27);
        txtX.TabIndex = 2;

        txtK.BackColor = Color.White;
        txtK.BorderStyle = BorderStyle.FixedSingle;
        txtK.ForeColor = Color.FromArgb(32, 40, 56);
        txtK.Location = new Point(504, 50);
        txtK.Name = "txtK";
        txtK.PlaceholderText = "k для 1-го байта (1..p−2, gcd(k,p−1)=1)";
        txtK.Size = new Size(534, 27);
        txtK.TabIndex = 3;

        btnFindRoots.BackColor = Color.FromArgb(76, 102, 246);
        btnFindRoots.FlatAppearance.BorderSize = 0;
        btnFindRoots.FlatStyle = FlatStyle.Flat;
        btnFindRoots.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 204);
        btnFindRoots.ForeColor = Color.White;
        btnFindRoots.Location = new Point(20, 98);
        btnFindRoots.Name = "btnFindRoots";
        btnFindRoots.Size = new Size(360, 38);
        btnFindRoots.TabIndex = 4;
        btnFindRoots.Text = "Найти первообразные корни";
        btnFindRoots.UseVisualStyleBackColor = false;
        btnFindRoots.Click += BtnFindRoots_Click;

        lblRoots.AutoSize = true;
        lblRoots.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point, 204);
        lblRoots.ForeColor = Color.FromArgb(49, 61, 89);
        lblRoots.Location = new Point(20, 154);
        lblRoots.Name = "lblRoots";
        lblRoots.Size = new Size(355, 20);
        lblRoots.Text = "Выбор первообразного корня g по модулю p";

        cmbG.BackColor = Color.White;
        cmbG.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbG.ForeColor = Color.FromArgb(32, 40, 56);
        cmbG.FormattingEnabled = true;
        cmbG.Location = new Point(20, 180);
        cmbG.Name = "cmbG";
        cmbG.Size = new Size(360, 28);
        cmbG.TabIndex = 5;
        cmbG.Enabled = false;

        lblEncrypt.AutoSize = true;
        lblEncrypt.Font = new Font("Segoe UI Semibold", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 204);
        lblEncrypt.ForeColor = Color.FromArgb(49, 61, 89);
        lblEncrypt.Location = new Point(20, 228);
        lblEncrypt.Name = "lblEncrypt";
        lblEncrypt.Size = new Size(161, 23);
        lblEncrypt.Text = "Шифрование файла";

        txtEncryptPath.BackColor = Color.White;
        txtEncryptPath.BorderStyle = BorderStyle.FixedSingle;
        txtEncryptPath.ForeColor = Color.FromArgb(32, 40, 56);
        txtEncryptPath.Location = new Point(20, 258);
        txtEncryptPath.Name = "txtEncryptPath";
        txtEncryptPath.ReadOnly = true;
        txtEncryptPath.Size = new Size(790, 27);
        txtEncryptPath.TabIndex = 6;

        btnBrowseEncrypt.BackColor = Color.FromArgb(230, 235, 245);
        btnBrowseEncrypt.FlatAppearance.BorderColor = Color.FromArgb(210, 217, 232);
        btnBrowseEncrypt.FlatStyle = FlatStyle.Flat;
        btnBrowseEncrypt.ForeColor = Color.FromArgb(49, 61, 89);
        btnBrowseEncrypt.Location = new Point(824, 256);
        btnBrowseEncrypt.Name = "btnBrowseEncrypt";
        btnBrowseEncrypt.Size = new Size(214, 32);
        btnBrowseEncrypt.TabIndex = 7;
        btnBrowseEncrypt.Text = "Выбрать файл";
        btnBrowseEncrypt.UseVisualStyleBackColor = false;
        btnBrowseEncrypt.Click += BtnBrowseEncrypt_Click;

        btnEncrypt.BackColor = Color.FromArgb(39, 174, 96);
        btnEncrypt.FlatAppearance.BorderSize = 0;
        btnEncrypt.FlatStyle = FlatStyle.Flat;
        btnEncrypt.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 204);
        btnEncrypt.ForeColor = Color.White;
        btnEncrypt.Location = new Point(20, 300);
        btnEncrypt.Name = "btnEncrypt";
        btnEncrypt.Size = new Size(320, 40);
        btnEncrypt.TabIndex = 8;
        btnEncrypt.Text = "Зашифровать в .enc";
        btnEncrypt.UseVisualStyleBackColor = false;
        btnEncrypt.Click += BtnEncrypt_Click;

        lblDecrypt.AutoSize = true;
        lblDecrypt.Font = new Font("Segoe UI Semibold", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 204);
        lblDecrypt.ForeColor = Color.FromArgb(49, 61, 89);
        lblDecrypt.Location = new Point(20, 356);
        lblDecrypt.Name = "lblDecrypt";
        lblDecrypt.Size = new Size(186, 23);
        lblDecrypt.Text = "Дешифрование файла";

        txtDecryptPath.BackColor = Color.White;
        txtDecryptPath.BorderStyle = BorderStyle.FixedSingle;
        txtDecryptPath.ForeColor = Color.FromArgb(32, 40, 56);
        txtDecryptPath.Location = new Point(20, 386);
        txtDecryptPath.Name = "txtDecryptPath";
        txtDecryptPath.ReadOnly = true;
        txtDecryptPath.Size = new Size(790, 27);
        txtDecryptPath.TabIndex = 9;

        btnBrowseDecrypt.BackColor = Color.FromArgb(230, 235, 245);
        btnBrowseDecrypt.FlatAppearance.BorderColor = Color.FromArgb(210, 217, 232);
        btnBrowseDecrypt.FlatStyle = FlatStyle.Flat;
        btnBrowseDecrypt.ForeColor = Color.FromArgb(49, 61, 89);
        btnBrowseDecrypt.Location = new Point(824, 384);
        btnBrowseDecrypt.Name = "btnBrowseDecrypt";
        btnBrowseDecrypt.Size = new Size(214, 32);
        btnBrowseDecrypt.TabIndex = 10;
        btnBrowseDecrypt.Text = "Выбрать .enc файл";
        btnBrowseDecrypt.UseVisualStyleBackColor = false;
        btnBrowseDecrypt.Click += BtnBrowseDecrypt_Click;

        btnDecrypt.BackColor = Color.FromArgb(237, 108, 2);
        btnDecrypt.FlatAppearance.BorderSize = 0;
        btnDecrypt.FlatStyle = FlatStyle.Flat;
        btnDecrypt.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 204);
        btnDecrypt.ForeColor = Color.White;
        btnDecrypt.Location = new Point(20, 416);
        btnDecrypt.Name = "btnDecrypt";
        btnDecrypt.Size = new Size(320, 40);
        btnDecrypt.TabIndex = 11;
        btnDecrypt.Text = "Расшифровать файл";
        btnDecrypt.UseVisualStyleBackColor = false;
        btnDecrypt.Click += BtnDecrypt_Click;

        lblPreview.AutoSize = true;
        lblPreview.Font = new Font("Segoe UI Semibold", 9.5F, FontStyle.Bold, GraphicsUnit.Point, 204);
        lblPreview.ForeColor = Color.FromArgb(49, 61, 89);
        lblPreview.Location = new Point(20, 14);
        lblPreview.Name = "lblPreview";
        lblPreview.Size = new Size(521, 21);
        lblPreview.Text = "Содержимое шифротекста в 10-й системе (пары a₁ b₁, a₂ b₂, …)";

        txtPreview.BackColor = Color.White;
        txtPreview.BorderStyle = BorderStyle.FixedSingle;
        txtPreview.ForeColor = Color.FromArgb(70, 82, 102);
        txtPreview.Location = new Point(20, 42);
        txtPreview.Multiline = true;
        txtPreview.ReadOnly = true;
        txtPreview.ScrollBars = ScrollBars.Vertical;
        txtPreview.Size = new Size(1020, 170);
        txtPreview.TabIndex = 12;
        txtPreview.TabStop = false;
        txtPreview.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;

        lblStatus.AutoSize = true;
        lblStatus.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 204);
        lblStatus.ForeColor = Color.FromArgb(39, 174, 96);
        lblStatus.Location = new Point(360, 426);
        lblStatus.Name = "lblStatus";
        lblStatus.Size = new Size(0, 20);
        lblStatus.Text = "";

        AutoScaleDimensions = new SizeF(8F, 20F);
        AutoScaleMode = AutoScaleMode.Font;
        BackColor = Color.FromArgb(236, 240, 248);
        ClientSize = new Size(1092, 736);
        Controls.Add(panelPreview);
        Controls.Add(panelMain);
        Controls.Add(panelHeader);
        Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 204);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimumSize = new Size(1110, 783);
        Name = "MainForm";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "ЛР3 Эль-Гамаль";
        panelHeader.ResumeLayout(false);
        panelHeader.PerformLayout();
        panelMain.ResumeLayout(false);
        panelMain.PerformLayout();
        panelPreview.ResumeLayout(false);
        panelPreview.PerformLayout();
        ResumeLayout(false);
    }
}
