using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace TESTLAB1
{
    /// <summary>
    /// Форма пошагового просмотра шифрования/расшифровки Виженера.
    /// </summary>
    public class VigenereStepsForm : Form
    {
        public VigenereStepsForm(string title, List<VigenereStep> steps, bool encrypt)
        {
            this.Text = title;
            this.ClientSize = new Size(520, 380);
            this.MinimumSize = new Size(480, 320);
            this.StartPosition = FormStartPosition.CenterParent;
            this.BackColor = Color.FromArgb(248, 252, 248);
            this.Font = new Font("Segoe UI", 9F);

            string colInput = encrypt ? "Открытый текст" : "Шифротекст";
            string colOutput = encrypt ? "Шифротекст" : "Открытый текст";

            var dgv = new DataGridView
            {
                Location = new Point(12, 12),
                Size = new Size(496, 320),
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                RowHeadersVisible = false,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                Font = new Font("Consolas", 9f)
            };
            dgv.Columns.Add(new DataGridViewTextBoxColumn { Name = "Index", HeaderText = "№", Width = 40 });
            dgv.Columns.Add(new DataGridViewTextBoxColumn { Name = "Input", HeaderText = colInput, Width = 80 });
            dgv.Columns.Add(new DataGridViewTextBoxColumn { Name = "Key", HeaderText = "Ключ (прогрессивный)", Width = 120 });
            dgv.Columns.Add(new DataGridViewTextBoxColumn { Name = "Output", HeaderText = colOutput, Width = 80 });

            foreach (var s in steps ?? new List<VigenereStep>())
                dgv.Rows.Add(s.Index, s.InputChar.ToString(), s.KeyChar.ToString(), s.OutputChar.ToString());

            this.Controls.Add(dgv);
        }
    }
}
