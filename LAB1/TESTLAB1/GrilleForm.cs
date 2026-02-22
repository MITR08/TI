using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace TESTLAB1
{
    public class GrilleForm : Form
    {
        private int _gridSize = 4;
        private bool[,] _holes;
        private Button[,] _gridButtons;
        private TextBox _txtInput;
        private TextBox _txtOutput;
        private ComboBox _comboSize;
        private Panel _panelGrid;
        private Label _lblKeyNote;

        public GrilleForm()
        {
            InitializeComponent();
            _holes = new bool[_gridSize, _gridSize];
            SetDefaultHoles();
            BuildGrid();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            this.Text = "Поворачивающаяся решётка (английский)";
            this.ClientSize = new Size(720, 440);
            this.MinimumSize = new Size(700, 440);
            this.StartPosition = FormStartPosition.CenterParent;
            this.BackColor = Color.FromArgb(248, 250, 252);
            this.Font = new Font("Segoe UI", 9F);

            var lblSize = new Label
            {
                Text = "Размер решётки:",
                Location = new Point(16, 12),
                AutoSize = true,
                ForeColor = Color.FromArgb(50, 60, 80)
            };

            _comboSize = new ComboBox
            {
                Location = new Point(120, 10),
                Width = 50,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            for (int i = 3; i <= 10; i++)
                _comboSize.Items.Add(i.ToString());
            _comboSize.SelectedIndex = 1; // по умолчанию 4
            _comboSize.SelectedIndexChanged += ComboSize_SelectedIndexChanged;

            _lblKeyNote = new Label
            {
                Text = "Шаблон: отметьте по одному отверстию в каждой группе (клик по ячейке).",
                Location = new Point(200, 14),
                AutoSize = true,
                ForeColor = Color.FromArgb(90, 100, 120)
            };

            _panelGrid = new Panel
            {
                Location = new Point(16, 44),
                Size = new Size(260, 260),
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = Color.White
            };

            var lblInput = new Label { Text = "Исходный текст (только латиница):", Location = new Point(300, 12), AutoSize = true };
            _txtInput = new TextBox
            {
                Location = new Point(300, 36),
                Size = new Size(400, 120),
                Multiline = true,
                ScrollBars = ScrollBars.Vertical,
                Font = new Font("Consolas", 9.5f)
            };

            var lblOutput = new Label { Text = "Результат:", Location = new Point(300, 168), AutoSize = true };
            _txtOutput = new TextBox
            {
                Location = new Point(300, 192),
                Size = new Size(400, 120),
                Multiline = true,
                ReadOnly = true,
                ScrollBars = ScrollBars.Vertical,
                BackColor = Color.FromArgb(252, 252, 252),
                Font = new Font("Consolas", 9.5f)
            };

            var btnLoad = new Button
            {
                Text = "Загрузить из файла",
                Location = new Point(300, 320),
                Size = new Size(130, 32),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(230, 235, 242)
            };
            btnLoad.Click += BtnLoad_Click;

            var btnEncrypt = new Button
            {
                Text = "Зашифровать",
                Location = new Point(440, 320),
                Size = new Size(120, 32),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(70, 130, 180),
                ForeColor = Color.White
            };
            btnEncrypt.Click += BtnEncrypt_Click;

            var btnDecrypt = new Button
            {
                Text = "Расшифровать",
                Location = new Point(570, 320),
                Size = new Size(130, 32),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(80, 120, 160),
                ForeColor = Color.White
            };
            btnDecrypt.Click += BtnDecrypt_Click;

            var btnSave = new Button
            {
                Text = "Сохранить результат в файл",
                Location = new Point(300, 358),
                Size = new Size(180, 32),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(230, 235, 242)
            };
            btnSave.Click += BtnSave_Click;

            var btnStepsEncrypt = new Button
            {
                Text = "По шагам: зашифровать",
                Location = new Point(300, 396),
                Size = new Size(160, 40),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(220, 230, 245)
            };
            btnStepsEncrypt.Click += BtnStepsEncrypt_Click;

            var btnStepsDecrypt = new Button
            {
                Text = "По шагам: расшифровать",
                Location = new Point(470, 396),
                Size = new Size(160, 40),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(220, 230, 245)
            };
            btnStepsDecrypt.Click += BtnStepsDecrypt_Click;

            var btnBack = new Button
            {
                Text = "← Назад",
                Location = new Point(16, 318),
                Size = new Size(100, 32),
                FlatStyle = FlatStyle.Flat
            };
            btnBack.Click += (s, e) => Close();

            this.Controls.Add(lblSize);
            this.Controls.Add(_comboSize);
            this.Controls.Add(_lblKeyNote);
            this.Controls.Add(_panelGrid);
            this.Controls.Add(lblInput);
            this.Controls.Add(_txtInput);
            this.Controls.Add(lblOutput);
            this.Controls.Add(_txtOutput);
            this.Controls.Add(btnLoad);
            this.Controls.Add(btnEncrypt);
            this.Controls.Add(btnDecrypt);
            this.Controls.Add(btnSave);
            this.Controls.Add(btnStepsEncrypt);
            this.Controls.Add(btnStepsDecrypt);
            this.Controls.Add(btnBack);
            this.ResumeLayout(false);
        }

        private void SetDefaultHoles()
        {
            if (_gridSize == 4)
            {
                _holes[0, 0] = true;
                _holes[0, 1] = true;
                _holes[0, 2] = true;
                _holes[1, 1] = true;
            }
        }

        private void ComboSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_comboSize.SelectedItem == null) return;
            _gridSize = int.Parse(_comboSize.SelectedItem.ToString());
            _holes = new bool[_gridSize, _gridSize];
            SetDefaultHoles();
            BuildGrid();
        }

        private void BuildGrid()
        {
            _panelGrid.Controls.Clear();
            _gridButtons = new Button[_gridSize, _gridSize];
            int cellSize = Math.Max(20, Math.Min(60, (260 - _gridSize - 1) / _gridSize));
            int total = cellSize * _gridSize + _gridSize + 1;
            _panelGrid.Size = new Size(total, total);

            for (int r = 0; r < _gridSize; r++)
                for (int c = 0; c < _gridSize; c++)
                {
                    int rr = r, cc = c;
                    var btn = new Button
                    {
                        Size = new Size(cellSize, cellSize),
                        Location = new Point(1 + c * (cellSize + 1), 1 + r * (cellSize + 1)),
                        FlatStyle = FlatStyle.Flat,
                        BackColor = _holes[r, c] ? Color.FromArgb(70, 130, 180) : Color.FromArgb(240, 242, 245),
                        Text = "",
                        Tag = Tuple.Create(r, c)
                    };
                    btn.FlatAppearance.BorderSize = 1;
                    btn.FlatAppearance.BorderColor = Color.FromArgb(200, 205, 212);
                    btn.Click += GridCell_Click;
                    btn.BackColor = _holes[r, c] ? Color.FromArgb(70, 130, 180) : Color.FromArgb(240, 242, 245);
                    _panelGrid.Controls.Add(btn);
                    _gridButtons[r, c] = btn;
                }
        }

        private void GridCell_Click(object sender, EventArgs e)
        {
            var btn = (Button)sender;
            var t = (Tuple<int, int>)btn.Tag;
            int r = t.Item1, c = t.Item2;
            var orbit = GrilleCipher.GetOrbitCells(_gridSize, r, c);
            foreach (var cell in orbit)
                _holes[cell.Item1, cell.Item2] = false;
            _holes[r, c] = true;
            foreach (var cell in orbit)
            {
                bool isHole = cell.Item1 == r && cell.Item2 == c;
                _gridButtons[cell.Item1, cell.Item2].BackColor = isHole ? Color.FromArgb(70, 130, 180) : Color.FromArgb(240, 242, 245);
            }
        }

        private bool ValidateHoles()
        {
            int expected = GrilleCipher.ExpectedHoleCount(_gridSize);
            int count = 0;
            for (int r = 0; r < _gridSize; r++)
                for (int c = 0; c < _gridSize; c++)
                    if (_holes[r, c]) count++;
            return count == expected;
        }

        private void BtnEncrypt_Click(object sender, EventArgs e)
        {
            if (!ValidateHoles())
            {
                MessageBox.Show("Задайте шаблон решётки: в каждой группе из 4 ячеек должна быть ровно одна отмечена (клик по ячейке).", "Шаблон", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            try
            {
                var cipher = new GrilleCipher(_gridSize, _holes);
                _txtOutput.Text = cipher.Encrypt(_txtInput.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnDecrypt_Click(object sender, EventArgs e)
        {
            if (!ValidateHoles())
            {
                MessageBox.Show("Задайте шаблон решётки: в каждой группе из 4 ячеек должна быть ровно одна отмечена.", "Шаблон", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            try
            {
                var cipher = new GrilleCipher(_gridSize, _holes);
                string letters = Alphabets.FilterToAlphabet(_txtInput.Text, Alphabets.English);
                int need = _gridSize * _gridSize;
                if (letters.Length < need)
                {
                    MessageBox.Show($"Для расшифровки решётки {_gridSize}×{_gridSize} нужно минимум {need} букв (сейчас {letters.Length}).", "Недостаточно символов", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                _txtOutput.Text = cipher.Decrypt(_txtInput.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        private void BtnStepsEncrypt_Click(object sender, EventArgs e)
        {
            if (!ValidateHoles())
            {
                MessageBox.Show("Задайте шаблон решётки.", "Шаблон", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            try
            {
                var cipher = new GrilleCipher(_gridSize, _holes);
                var t = cipher.EncryptWithSteps(_txtInput.Text);
                _txtOutput.Text = t.Item1;
                using (var f = new GrilleStepsForm("Поворачивающаяся решётка — шифрование по шагам", t.Item2))
                    f.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnStepsDecrypt_Click(object sender, EventArgs e)
        {
            if (!ValidateHoles())
            {
                MessageBox.Show("Задайте шаблон решётки.", "Шаблон", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            try
            {
                string letters = Alphabets.FilterToAlphabet(_txtInput.Text, Alphabets.English);
                if (letters.Length < _gridSize * _gridSize)
                {
                    MessageBox.Show($"Для расшифровки нужно минимум {_gridSize * _gridSize} букв.", "Недостаточно символов", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                var cipher = new GrilleCipher(_gridSize, _holes);
                var t = cipher.DecryptWithSteps(_txtInput.Text);
                _txtOutput.Text = t.Item1;
                using (var f = new GrilleStepsForm("Поворачивающаяся решётка — расшифровка по шагам", t.Item2))
                    f.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                    File.WriteAllText(sfd.FileName, _txtOutput.Text);
                    MessageBox.Show("Файл сохранён.", "Сохранение", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка записи файла", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
