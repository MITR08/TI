using System.Windows.Forms;

namespace LAB2
{
    partial class Lab2Form
    {
        private System.ComponentModel.IContainer components = null;

        private TextBox txtState;
        private Button btnOpen;
        private Button btnCrypt;
        private Button btnSave;
        private TextBox txtFilePath;
        private RichTextBox rtbInput;
        private RichTextBox rtbKey;
        private RichTextBox rtbOutput;
        private Label lblState;
        private Label lblInput;
        private Label lblKey;
        private Label lblOutput;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.txtState = new TextBox();
            this.btnOpen = new Button();
            this.btnCrypt = new Button();
            this.btnSave = new Button();
            this.txtFilePath = new TextBox();
            this.rtbInput = new RichTextBox();
            this.rtbKey = new RichTextBox();
            this.rtbOutput = new RichTextBox();
            this.lblState = new Label();
            this.lblInput = new Label();
            this.lblKey = new Label();
            this.lblOutput = new Label();
            this.SuspendLayout();
            // 
            // txtState
            // 
            this.txtState.Location = new System.Drawing.Point(24, 42);
            this.txtState.MaxLength = 29;
            this.txtState.Name = "txtState";
            this.txtState.Size = new System.Drawing.Size(430, 25);
            this.txtState.TabIndex = 0;
            this.txtState.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtState.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtState_KeyPress);
            this.txtState.TextChanged += new System.EventHandler(this.txtState_TextChanged);
            // 
            // lblState
            // 
            this.lblState.AutoSize = true;
            this.lblState.Location = new System.Drawing.Point(12, 15);
            this.lblState.Name = "lblState";
            this.lblState.Size = new System.Drawing.Size(290, 15);
            this.lblState.TabIndex = 100;
            this.lblState.Text = "Начальное состояние регистра (ровно 29 бит 0/1):";
            // 
            // btnOpen
            // 
            this.btnOpen.Location = new System.Drawing.Point(24, 90);
            this.btnOpen.Name = "btnOpen";
            this.btnOpen.Size = new System.Drawing.Size(150, 34);
            this.btnOpen.TabIndex = 1;
            this.btnOpen.Text = "Открыть файл";
            this.btnOpen.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOpen.BackColor = System.Drawing.Color.FromArgb(52, 152, 219);
            this.btnOpen.ForeColor = System.Drawing.Color.White;
            this.btnOpen.FlatAppearance.BorderSize = 0;
            this.btnOpen.Click += new System.EventHandler(this.btnOpen_Click);
            // 
            // txtFilePath
            // 
            this.txtFilePath.Location = new System.Drawing.Point(188, 94);
            this.txtFilePath.Name = "txtFilePath";
            this.txtFilePath.ReadOnly = true;
            this.txtFilePath.Size = new System.Drawing.Size(540, 25);
            this.txtFilePath.TabIndex = 2;
            this.txtFilePath.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            // 
            // btnCrypt
            // 
            this.btnCrypt.Location = new System.Drawing.Point(744, 90);
            this.btnCrypt.Name = "btnCrypt";
            this.btnCrypt.Size = new System.Drawing.Size(170, 34);
            this.btnCrypt.TabIndex = 3;
            this.btnCrypt.Text = "Шифр / Дешифр";
            this.btnCrypt.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCrypt.BackColor = System.Drawing.Color.FromArgb(46, 204, 113);
            this.btnCrypt.ForeColor = System.Drawing.Color.White;
            this.btnCrypt.FlatAppearance.BorderSize = 0;
            this.btnCrypt.Click += new System.EventHandler(this.btnCrypt_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(928, 90);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(180, 34);
            this.btnSave.TabIndex = 4;
            this.btnSave.Text = "Сохранить файл";
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSave.BackColor = System.Drawing.Color.FromArgb(52, 73, 94);
            this.btnSave.Enabled = false;
            this.btnSave.ForeColor = System.Drawing.Color.White;
            this.btnSave.FlatAppearance.BorderSize = 0;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // rtbInput
            // 
            this.rtbInput.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.rtbInput.Location = new System.Drawing.Point(24, 170);
            this.rtbInput.Name = "rtbInput";
            this.rtbInput.ReadOnly = true;
            this.rtbInput.Size = new System.Drawing.Size(520, 390);
            this.rtbInput.TabIndex = 5;
            this.rtbInput.Text = "";
            // 
            // rtbKey
            // 
            this.rtbKey.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.rtbKey.Location = new System.Drawing.Point(568, 210);
            this.rtbKey.Name = "rtbKey";
            this.rtbKey.ReadOnly = true;
            this.rtbKey.Size = new System.Drawing.Size(540, 150);
            this.rtbKey.TabIndex = 6;
            this.rtbKey.Text = "";
            // 
            // rtbOutput
            // 
            this.rtbOutput.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.rtbOutput.Location = new System.Drawing.Point(568, 380);
            this.rtbOutput.Name = "rtbOutput";
            this.rtbOutput.ReadOnly = true;
            this.rtbOutput.Size = new System.Drawing.Size(540, 180);
            this.rtbOutput.TabIndex = 7;
            this.rtbOutput.Text = "";
            // 
            // lblInput
            // 
            this.lblInput.AutoSize = true;
            this.lblInput.Location = new System.Drawing.Point(21, 150);
            this.lblInput.Name = "lblInput";
            this.lblInput.Size = new System.Drawing.Size(209, 17);
            this.lblInput.TabIndex = 101;
            this.lblInput.Text = "Входной файл (в двоичном виде):";
            // 
            // lblKey
            // 
            this.lblKey.AutoSize = true;
            this.lblKey.Location = new System.Drawing.Point(565, 190);
            this.lblKey.Name = "lblKey";
            this.lblKey.Size = new System.Drawing.Size(206, 17);
            this.lblKey.TabIndex = 102;
            this.lblKey.Text = "Использованный ключ (битовая):";
            // 
            // lblOutput
            // 
            this.lblOutput.AutoSize = true;
            this.lblOutput.Location = new System.Drawing.Point(565, 360);
            this.lblOutput.Name = "lblOutput";
            this.lblOutput.Size = new System.Drawing.Size(221, 17);
            this.lblOutput.TabIndex = 103;
            this.lblOutput.Text = "Результат (шифр/расшифр, битовая):";
            // 
            // Lab2Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(236, 242, 255);
            this.ClientSize = new System.Drawing.Size(1120, 600);
            this.Controls.Add(this.lblOutput);
            this.Controls.Add(this.lblKey);
            this.Controls.Add(this.lblInput);
            this.Controls.Add(this.rtbOutput);
            this.Controls.Add(this.rtbKey);
            this.Controls.Add(this.rtbInput);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnCrypt);
            this.Controls.Add(this.txtFilePath);
            this.Controls.Add(this.btnOpen);
            this.Controls.Add(this.lblState);
            this.Controls.Add(this.txtState);
            this.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimumSize = new System.Drawing.Size(1136, 639);
            this.Name = "Lab2Form";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ЛР2: Потоковый шифр на LFSR (степень 29)";
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}

