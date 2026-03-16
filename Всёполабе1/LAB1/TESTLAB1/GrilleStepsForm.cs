using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace TESTLAB1
{
    /// <summary>
    /// Форма пошагового просмотра шифрования/расшифровки поворачивающейся решётки.
    /// </summary>
    public class GrilleStepsForm : Form
    {
        private readonly ListBox _listSteps;
        private readonly Label _lblDescription;
        private readonly Label _lblLetters;
        private readonly Panel _panelMatrix;
        private readonly List<GrilleStep> _steps;

        public GrilleStepsForm(string title, List<GrilleStep> steps)
        {
            _steps = steps ?? new List<GrilleStep>();

            this.Text = title;
            this.ClientSize = new Size(520, 420);
            this.MinimumSize = new Size(480, 380);
            this.StartPosition = FormStartPosition.CenterParent;
            this.BackColor = Color.FromArgb(250, 252, 254);
            this.Font = new Font("Segoe UI", 9F);

            var lblStep = new Label { Text = "Шаг:", Location = new Point(12, 10), AutoSize = true };
            _listSteps = new ListBox
            {
                Location = new Point(12, 32),
                Size = new Size(180, 220),
                Font = new Font("Segoe UI", 9F),
                DrawMode = DrawMode.OwnerDrawFixed,
                ItemHeight = 22
            };
            _listSteps.DrawItem += ListSteps_DrawItem;
            _listSteps.SelectedIndexChanged += ListSteps_SelectedIndexChanged;

            _lblDescription = new Label
            {
                Location = new Point(208, 10),
                Size = new Size(300, 40),
                AutoSize = true,
                MaximumSize = new Size(300, 0),
                ForeColor = Color.FromArgb(50, 60, 80),
                Font = new Font("Segoe UI", 9.5f)
            };

            _lblLetters = new Label
            {
                Location = new Point(208, 54),
                Size = new Size(300, 24),
                AutoSize = true,
                ForeColor = Color.FromArgb(70, 130, 180),
                Font = new Font("Consolas", 10f)
            };

            _panelMatrix = new Panel
            {
                Location = new Point(208, 84),
                Size = new Size(280, 280),
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = Color.White
            };

            this.Controls.Add(lblStep);
            this.Controls.Add(_listSteps);
            this.Controls.Add(_lblDescription);
            this.Controls.Add(_lblLetters);
            this.Controls.Add(_panelMatrix);

            for (int i = 0; i < _steps.Count; i++)
                _listSteps.Items.Add($"Шаг {i + 1}: {GetStepShortName(_steps[i])}");
            if (_listSteps.Items.Count > 0)
                _listSteps.SelectedIndex = 0;
        }

        private static string GetStepShortName(GrilleStep s)
        {
            if (s.RotationDegrees == -3) return "Исходные буквы";
            if (s.RotationDegrees == -2) return "Заполнение матрицы";
            if (s.RotationDegrees == -4) return "Случайные буквы";
            if (s.RotationDegrees == -1) return "Итог (читаем построчно)";
            return $"Поворот {s.RotationDegrees}°";
        }

        private void ListSteps_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index < 0) return;
            bool selected = (e.State & DrawItemState.Selected) != 0;
            e.DrawBackground();
            Color back = selected ? Color.FromArgb(220, 235, 255) : (e.Index % 2 == 0 ? Color.White : Color.FromArgb(252, 252, 254));
            using (var brush = new SolidBrush(back))
                e.Graphics.FillRectangle(brush, e.Bounds);
            using (var brush = new SolidBrush(selected ? Color.FromArgb(40, 80, 140) : e.ForeColor))
                e.Graphics.DrawString(_listSteps.Items[e.Index].ToString(), e.Font, brush, e.Bounds.X + 2, e.Bounds.Y + 2);
            e.DrawFocusRectangle();
        }

        private void ListSteps_SelectedIndexChanged(object sender, EventArgs e)
        {
            int idx = _listSteps.SelectedIndex;
            if (idx < 0 || idx >= _steps.Count) return;
            var step = _steps[idx];
            _lblDescription.Text = step.Description;
            _lblLetters.Text = string.IsNullOrEmpty(step.LettersThisRound)
                ? ""
                : (step.RotationDegrees == -3 ? "Буквы: " : "Буквы этого шага: ") + step.LettersThisRound;

            _panelMatrix.Controls.Clear();
            if (step.Matrix == null)
            {
                // Шаг "исходные буквы" — показываем буквы в одну строку с подсветкой каждой
                if (!string.IsNullOrEmpty(step.LettersThisRound))
                {
                    int len = Math.Min(step.LettersThisRound.Length, 16);
                    int cellW = 24;
                    for (int i = 0; i < len; i++)
                    {
                        var lbl = new Label
                        {
                            Text = step.LettersThisRound[i].ToString(),
                            Size = new Size(cellW - 2, cellW - 2),
                            Location = new Point(4 + i * cellW, 4),
                            TextAlign = ContentAlignment.MiddleCenter,
                            BackColor = Color.FromArgb(200, 230, 255),
                            BorderStyle = BorderStyle.FixedSingle,
                            Font = new Font("Consolas", 10f)
                        };
                        _panelMatrix.Controls.Add(lbl);
                    }
                    if (step.LettersThisRound.Length > 16)
                    {
                        var more = new Label
                        {
                            Text = "... ещё " + (step.LettersThisRound.Length - 16),
                            Location = new Point(4 + len * cellW, 6),
                            AutoSize = true,
                            ForeColor = Color.Gray
                        };
                        _panelMatrix.Controls.Add(more);
                    }
                }
                return;
            }

            int n = step.Matrix.GetLength(0);
            bool[,] highlight = step.HighlightCells;
            int cellSize = Math.Max(24, Math.Min(48, (270 - n - 1) / n));
            for (int r = 0; r < n; r++)
                for (int c = 0; c < n; c++)
                {
                    char ch = step.Matrix[r, c];
                    bool isEmpty = ch == '\0';
                    bool isHighlight = highlight != null && r < highlight.GetLength(0) && c < highlight.GetLength(1) && highlight[r, c];
                    Color back;
                    if (isEmpty)
                        back = Color.FromArgb(240, 242, 245);
                    else if (isHighlight)
                        back = Color.FromArgb(180, 220, 180);
                    else
                        back = Color.FromArgb(248, 250, 252);
                    var lbl = new Label
                    {
                        Text = isEmpty ? "—" : ch.ToString(),
                        Size = new Size(cellSize, cellSize),
                        Location = new Point(2 + c * (cellSize + 2), 2 + r * (cellSize + 2)),
                        TextAlign = ContentAlignment.MiddleCenter,
                        BackColor = back,
                        BorderStyle = BorderStyle.FixedSingle,
                        Font = new Font("Consolas", isEmpty ? 8f : 9f),
                        ForeColor = isEmpty ? Color.Gray : Color.Black
                    };
                    _panelMatrix.Controls.Add(lbl);
                }
        }
    }
}
