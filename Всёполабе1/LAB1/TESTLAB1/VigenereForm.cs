using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace TESTLAB1
{
    public class VigenereForm : Form
    {
        private TextBox _txtKey;
        private TextBox _txtInput;
        private TextBox _txtOutput;
        private Label _lblKeySanitized;
        private TextBox _txtProgressiveKey;

        public VigenereForm()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            this.Text = "Виженер — прогрессивный ключ (русский)";
            this.ClientSize = new Size(620, 500);
            this.MinimumSize = new Size(580, 500);
            this.StartPosition = FormStartPosition.CenterParent;
            this.BackColor = System.Drawing.Color.FromArgb(248, 252, 248);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);

            var lblKey = new Label
            {
                Text = "Ключ (только русские буквы, ввод с клавиатуры):",
                Location = new System.Drawing.Point(16, 14),
                AutoSize = true
            };

            _txtKey = new TextBox
            {
                Location = new System.Drawing.Point(16, 38),
                Size = new System.Drawing.Size(400, 24),
                Font = new System.Drawing.Font("Segoe UI", 10F)
            };
            _txtKey.TextChanged += TxtKey_TextChanged;

            _lblKeySanitized = new Label
            {
                Text = "Используемый ключ: —",
                Location = new System.Drawing.Point(16, 66),
                AutoSize = true,
                ForeColor = System.Drawing.Color.FromArgb(80, 100, 120)
            };

            var lblProgressive = new Label
            {
                Text = "В какой ключ преобразуется (прогрессивный, на длину текста):",
                Location = new System.Drawing.Point(16, 88),
                AutoSize = true,
                ForeColor = System.Drawing.Color.FromArgb(60, 80, 100)
            };
            _txtProgressiveKey = new TextBox
            {
                Location = new System.Drawing.Point(16, 110),
                Size = new System.Drawing.Size(580, 22),
                ReadOnly = true,
                BackColor = System.Drawing.Color.FromArgb(252, 254, 252),
                Font = new System.Drawing.Font("Consolas", 9.5f)
            };

            var lblInput = new Label { Text = "Текст (русский): из файла или вручную", Location = new System.Drawing.Point(16, 138), AutoSize = true };
            _txtInput = new TextBox
            {
                Location = new System.Drawing.Point(16, 158),
                Size = new System.Drawing.Size(580, 100),
                Multiline = true,
                ScrollBars = ScrollBars.Vertical,
                Font = new System.Drawing.Font("Consolas", 9.5f)
            };
            _txtInput.TextChanged += (s, e) => UpdateProgressiveKey();

            var lblOutput = new Label { Text = "Результат:", Location = new System.Drawing.Point(16, 268), AutoSize = true };
            _txtOutput = new TextBox
            {
                Location = new System.Drawing.Point(16, 292),
                Size = new System.Drawing.Size(580, 80),
                Multiline = true,
                ReadOnly = true,
                ScrollBars = ScrollBars.Vertical,
                BackColor = System.Drawing.Color.FromArgb(250, 252, 250),
                Font = new System.Drawing.Font("Consolas", 9.5f)
            };

            var btnStepsEncrypt = new Button
            {
                Text = "Зашифровать(шаги)",
                Location = new System.Drawing.Point(16, 384),
                Size = new System.Drawing.Size(160, 28),
                FlatStyle = FlatStyle.Flat,
                BackColor = System.Drawing.Color.FromArgb(220, 238, 220)
            };
            btnStepsEncrypt.Click += BtnStepsEncrypt_Click;

            var btnStepsDecrypt = new Button
            {
                Text = "Расшифровать(шаги)",
                Location = new System.Drawing.Point(186, 384),
                Size = new System.Drawing.Size(160, 28),
                FlatStyle = FlatStyle.Flat,
                BackColor = System.Drawing.Color.FromArgb(220, 238, 220)
            };
            btnStepsDecrypt.Click += BtnStepsDecrypt_Click;

            var btnLoad = new Button
            {
                Text = "Загрузить из файла",
                Location = new System.Drawing.Point(16, 418),
                Size = new System.Drawing.Size(140, 32),
                FlatStyle = FlatStyle.Flat,
                BackColor = System.Drawing.Color.FromArgb(230, 240, 230)
            };
            btnLoad.Click += BtnLoad_Click;

            var btnEncrypt = new Button
            {
                Text = "Зашифровать",
                Location = new System.Drawing.Point(170, 418),
                Size = new System.Drawing.Size(120, 32),
                FlatStyle = FlatStyle.Flat,
                BackColor = System.Drawing.Color.FromArgb(60, 120, 80),
                ForeColor = System.Drawing.Color.White
            };
            btnEncrypt.Click += BtnEncrypt_Click;

            var btnDecrypt = new Button
            {
                Text = "Расшифровать",
                Location = new System.Drawing.Point(304, 418),
                Size = new System.Drawing.Size(120, 32),
                FlatStyle = FlatStyle.Flat,
                BackColor = System.Drawing.Color.FromArgb(70, 130, 90),
                ForeColor = System.Drawing.Color.White
            };
            btnDecrypt.Click += BtnDecrypt_Click;

            var btnSave = new Button
            {
                Text = "Сохранить в файл",
                Location = new System.Drawing.Point(438, 418),
                Size = new System.Drawing.Size(158, 32),
                FlatStyle = FlatStyle.Flat,
                BackColor = System.Drawing.Color.FromArgb(230, 240, 230)
            };
            btnSave.Click += BtnSave_Click;

            var btnBack = new Button
            {
                Text = "← Назад",
                Location = new System.Drawing.Point(16, 458),
                Size = new System.Drawing.Size(90, 28),
                FlatStyle = FlatStyle.Flat
            };
            btnBack.Click += (s, e) => Close();

            var btnTable = new Button
            {
                Text = "Таблица подстановки (пример)",
                Location = new System.Drawing.Point(116, 458),
                Size = new System.Drawing.Size(200, 28),
                FlatStyle = FlatStyle.Flat,
                BackColor = System.Drawing.Color.FromArgb(230, 242, 230)
            };
            btnTable.Click += (s, e) => { using (var f = new VigenereTableForm()) f.ShowDialog(); };

            this.Controls.Add(lblKey);
            this.Controls.Add(_txtKey);
            this.Controls.Add(_lblKeySanitized);
            this.Controls.Add(lblProgressive);
            this.Controls.Add(_txtProgressiveKey);
            this.Controls.Add(lblInput);
            this.Controls.Add(_txtInput);
            this.Controls.Add(lblOutput);
            this.Controls.Add(_txtOutput);
            this.Controls.Add(btnStepsEncrypt);
            this.Controls.Add(btnStepsDecrypt);
            this.Controls.Add(btnLoad);
            this.Controls.Add(btnEncrypt);
            this.Controls.Add(btnDecrypt);
            this.Controls.Add(btnSave);
            this.Controls.Add(btnBack);
            this.Controls.Add(btnTable);
            this.ResumeLayout(false);
        }

        private void TxtKey_TextChanged(object sender, EventArgs e)
        {
            string sanitized = Alphabets.SanitizeKey(_txtKey.Text, Alphabets.Russian);
            _lblKeySanitized.Text = string.IsNullOrEmpty(sanitized)
                ? "Используемый ключ: — (введите русские буквы)"
                : "Используемый ключ: " + sanitized;
            UpdateProgressiveKey();
        }

        private void UpdateProgressiveKey()
        {
            try
            {
                string key = Alphabets.SanitizeKey(_txtKey.Text, Alphabets.Russian);
                if (string.IsNullOrEmpty(key))
                {
                    _txtProgressiveKey.Text = "";
                    return;
                }
                string letters = Alphabets.FilterToAlphabet(_txtInput.Text, Alphabets.Russian);
                if (letters.Length == 0)
                {
                    _txtProgressiveKey.Text = "(введите текст — ключ покажется на его длину)";
                    return;
                }
                var cipher = new VigenereCipher(_txtKey.Text);
                _txtProgressiveKey.Text = cipher.GetProgressiveKeyExpansion(letters.Length);
            }
            catch
            {
                _txtProgressiveKey.Text = "";
            }
        }

        private void BtnEncrypt_Click(object sender, EventArgs e)
        {
            try
            {
                var cipher = new VigenereCipher(_txtKey.Text);
                _txtOutput.Text = cipher.Encrypt(_txtInput.Text);
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message, "Ключ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void BtnDecrypt_Click(object sender, EventArgs e)
        {
            try
            {
                var cipher = new VigenereCipher(_txtKey.Text);
                _txtOutput.Text = cipher.Decrypt(_txtInput.Text);
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message, "Ключ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void BtnStepsEncrypt_Click(object sender, EventArgs e)
        {
            try
            {
                var cipher = new VigenereCipher(_txtKey.Text);
                var steps = cipher.GetSteps(_txtInput.Text, encrypt: true);
                _txtOutput.Text = cipher.Encrypt(_txtInput.Text);
                if (steps.Count > 0)
                    using (var f = new VigenereStepsForm("Виженер — шифрование по шагам", steps, encrypt: true))
                        f.ShowDialog();
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message, "Ключ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void BtnStepsDecrypt_Click(object sender, EventArgs e)
        {
            try
            {
                var cipher = new VigenereCipher(_txtKey.Text);
                var steps = cipher.GetSteps(_txtInput.Text, encrypt: false);
                _txtOutput.Text = cipher.Decrypt(_txtInput.Text);
                if (steps.Count > 0)
                    using (var f = new VigenereStepsForm("Виженер — расшифровка по шагам", steps, encrypt: false))
                        f.ShowDialog();
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message, "Ключ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void BtnLoad_Click(object sender, EventArgs e)
        {
            using (var ofd = new OpenFileDialog())
            {
                if (ofd.ShowDialog() != DialogResult.OK) return;
                try
                {
                    _txtInput.Text = File.ReadAllText(ofd.FileName);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка чтения файла", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(_txtOutput.Text))
            {
                MessageBox.Show("Нет результата для сохранения.", "Сохранение", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            using (var sfd = new SaveFileDialog())
            {
                if (sfd.ShowDialog() != DialogResult.OK) return;
                try
                {
                    string key = Alphabets.SanitizeKey(_txtKey.Text, Alphabets.Russian);
                    string content = "Ключ: " + (string.IsNullOrEmpty(key) ? "" : key) + Environment.NewLine
                        + Environment.NewLine
                        + "Результат:" + Environment.NewLine
                        + _txtOutput.Text;
                    File.WriteAllText(sfd.FileName, content);
                    MessageBox.Show("Файл сохранён (ключ и результат).", "Сохранение", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка записи файла", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
