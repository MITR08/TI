using System;
using System.Drawing;
using System.Windows.Forms;

namespace TESTLAB1
{
    /// <summary>
    /// Таблица подстановки Виженера (для примера). Значения соответствуют формуле: (строка + столбец) mod N.
    /// Шифрование в программе выполняется по формуле, таблица — только наглядная.
    /// </summary>
    public class VigenereTableForm : Form
    {
        public VigenereTableForm()
        {
            this.Text = "Таблица Виженера (русский алфавит) — для примера, расчёт по формуле";
            this.ClientSize = new Size(720, 420);
            this.MinimumSize = new Size(400, 300);
            this.StartPosition = FormStartPosition.CenterParent;
            this.BackColor = Color.FromArgb(250, 252, 250);
            this.Font = new Font("Segoe UI", 8F);

            var lblNote = new Label
            {
                Text = "Строка = ключ, столбец = открытый текст. Ячейка = (ключ + открытый) mod 33. В программе всё считается по этой формуле.",
                Location = new Point(12, 8),
                AutoSize = true,
                MaximumSize = new Size(680, 0),
                ForeColor = Color.FromArgb(80, 100, 80)
            };

            var dgv = new DataGridView
            {
                Location = new Point(12, 32),
                Size = new Size(696, 360),
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                RowHeadersVisible = true,
                ColumnHeadersVisible = true,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                Font = new Font("Consolas", 8F),
                DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleCenter },
                RowHeadersDefaultCellStyle = new DataGridViewCellStyle { BackColor = Color.FromArgb(230, 242, 230) },
                ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle { BackColor = Color.FromArgb(220, 238, 220) }
            };

            string abc = Alphabets.Russian;
            int N = abc.Length;

            dgv.Columns.Add(new DataGridViewTextBoxColumn { Name = "Key", HeaderText = "Ключ \\ Откр.", Width = 52 });
            for (int c = 0; c < N; c++)
                dgv.Columns.Add(new DataGridViewTextBoxColumn { Name = "C" + c, HeaderText = abc[c].ToString(), Width = 18 });

            for (int r = 0; r < N; r++)
            {
                var cells = new object[N + 1];
                cells[0] = abc[r].ToString();
                for (int c = 0; c < N; c++)
                    cells[c + 1] = abc[(r + c) % N].ToString(); // формула: (строка + столбец) mod N
                dgv.Rows.Add(cells);
                dgv.Rows[r].HeaderCell.Value = abc[r].ToString();
            }

            this.Controls.Add(lblNote);
            this.Controls.Add(dgv);
        }
    }
}
