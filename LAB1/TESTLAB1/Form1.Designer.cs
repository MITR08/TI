namespace TESTLAB1
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.labelTitle = new System.Windows.Forms.Label();
            this.labelSubtitle = new System.Windows.Forms.Label();
            this.btnGrille = new System.Windows.Forms.Button();
            this.btnVigenere = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // labelTitle
            // 
            this.labelTitle.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelTitle.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
            this.labelTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(40)))), ((int)(((byte)(60)))));
            this.labelTitle.Location = new System.Drawing.Point(36, 62);
            this.labelTitle.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelTitle.Name = "labelTitle";
            this.labelTitle.Size = new System.Drawing.Size(678, 62);
            this.labelTitle.TabIndex = 0;
            this.labelTitle.Text = "Криптография";
            this.labelTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelSubtitle
            // 
            this.labelSubtitle.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelSubtitle.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.labelSubtitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(90)))), ((int)(((byte)(110)))));
            this.labelSubtitle.Location = new System.Drawing.Point(36, 184);
            this.labelSubtitle.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelSubtitle.Name = "labelSubtitle";
            this.labelSubtitle.Size = new System.Drawing.Size(678, 43);
            this.labelSubtitle.TabIndex = 1;
            this.labelSubtitle.Text = "Выберите метод:";
            this.labelSubtitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnGrille
            // 
            this.btnGrille.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnGrille.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(130)))), ((int)(((byte)(180)))));
            this.btnGrille.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnGrille.FlatAppearance.BorderSize = 0;
            this.btnGrille.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(150)))), ((int)(((byte)(200)))));
            this.btnGrille.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnGrille.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.btnGrille.ForeColor = System.Drawing.Color.White;
            this.btnGrille.Location = new System.Drawing.Point(150, 246);
            this.btnGrille.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnGrille.Name = "btnGrille";
            this.btnGrille.Size = new System.Drawing.Size(450, 86);
            this.btnGrille.TabIndex = 2;
            this.btnGrille.Text = "Поворачивающаяся решётка\r\n(английский язык)";
            this.btnGrille.UseVisualStyleBackColor = false;
            this.btnGrille.Click += new System.EventHandler(this.btnGrille_Click);
            // 
            // btnVigenere
            // 
            this.btnVigenere.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnVigenere.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(120)))), ((int)(((byte)(80)))));
            this.btnVigenere.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnVigenere.FlatAppearance.BorderSize = 0;
            this.btnVigenere.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(140)))), ((int)(((byte)(100)))));
            this.btnVigenere.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnVigenere.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.btnVigenere.ForeColor = System.Drawing.Color.White;
            this.btnVigenere.Location = new System.Drawing.Point(150, 369);
            this.btnVigenere.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnVigenere.Name = "btnVigenere";
            this.btnVigenere.Size = new System.Drawing.Size(450, 86);
            this.btnVigenere.TabIndex = 3;
            this.btnVigenere.Text = "Виженер (прогрессивный ключ)\r\n(русский язык)";
            this.btnVigenere.UseVisualStyleBackColor = false;
            this.btnVigenere.Click += new System.EventHandler(this.btnVigenere_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(248)))), ((int)(((byte)(252)))));
            this.ClientSize = new System.Drawing.Size(750, 554);
            this.Controls.Add(this.btnVigenere);
            this.Controls.Add(this.btnGrille);
            this.Controls.Add(this.labelSubtitle);
            this.Controls.Add(this.labelTitle);
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MinimumSize = new System.Drawing.Size(589, 462);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Криптография — выбор алгоритма";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label labelTitle;
        private System.Windows.Forms.Label labelSubtitle;
        private System.Windows.Forms.Button btnGrille;
        private System.Windows.Forms.Button btnVigenere;
    }
}
