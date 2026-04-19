using System;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace LAB2
{
    public partial class Lab2Form : Form
    {
        private readonly LFSR _generator;
        private byte[] _fileBytes;
        private bool _stateSanitizeInProgress;

        public Lab2Form()
        {
            InitializeComponent();
            _generator = new LFSR(29, "x^29+x^2+1");
        }

        private void txtState_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsControl(e.KeyChar)) return;
            if (e.KeyChar != '0' && e.KeyChar != '1') e.Handled = true;
        }

        private void txtState_TextChanged(object sender, EventArgs e)
        {
            if (_stateSanitizeInProgress) return;

            try
            {
                _stateSanitizeInProgress = true;

                int selectionStart = txtState.SelectionStart;
                string parsed = LFSR.ParseInput(txtState.Text);

                if (parsed.Length > _generator.RegisterLength)
                {
                    parsed = parsed.Substring(0, _generator.RegisterLength);
                }

                if (!string.Equals(parsed, txtState.Text, StringComparison.Ordinal))
                {
                    txtState.Text = parsed;
                    txtState.SelectionStart = Math.Min(selectionStart, txtState.Text.Length);
                }
            }
            finally
            {
                _stateSanitizeInProgress = false;
            }
        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            try
            {
                using (OpenFileDialog ofd = new OpenFileDialog())
                {
                    if (ofd.ShowDialog() == DialogResult.OK)
                    {
                        txtFilePath.Text = ofd.FileName;
                        _fileBytes = File.ReadAllBytes(ofd.FileName);
                        rtbInput.Text = BytesToBinaryString(_fileBytes);
                        rtbKey.Clear();
                        rtbOutput.Clear();
                        btnSave.Enabled = false;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка открытия файла", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCrypt_Click(object sender, EventArgs e)
        {
            if (_fileBytes == null || _fileBytes.Length == 0)
            {
                MessageBox.Show("Сначала выберите файл.", "Нет файла", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                string state = LFSR.ParseInput(txtState.Text);
                if (state.Length != _generator.RegisterLength)
                {
                    MessageBox.Show(
                        $"Начальное состояние должно быть строго длиной {_generator.RegisterLength} бит (только 0/1).",
                        "Неверное начальное состояние",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    txtState.Focus();
                    return;
                }

                _generator.SetRegisterState(state);

                byte[] bytesCopy = new byte[_fileBytes.Length];
                _fileBytes.CopyTo(bytesCopy, 0);

                var (keyBinary, resultBytes, resultBinary) = StreamCrypt.CryptBinary(_generator, bytesCopy);

                rtbKey.Text = keyBinary;
                rtbOutput.Text = resultBinary;


                _fileBytes = resultBytes;
                btnSave.Enabled = true;

                _generator.SetRegisterState(state);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка шифрования", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (_fileBytes == null || _fileBytes.Length == 0)
            {
                MessageBox.Show("Нет данных для сохранения. Сначала выполни шифрование/дешифрование.",
                    "Нет данных", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (SaveFileDialog sfd = new SaveFileDialog())
                {
                    if (sfd.ShowDialog() == DialogResult.OK)
                    {
                        File.WriteAllBytes(sfd.FileName, _fileBytes);
                        MessageBox.Show("Файл сохранён.", "Готово", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка сохранения", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private static string BytesToBinaryString(byte[] data)
        {
            StringBuilder sb = new StringBuilder(data.Length * 9);
            foreach (byte b in data)
            {
                sb.Append(Convert.ToString(b, 2).PadLeft(8, '0')).Append(' ');
            }
            return sb.ToString();
        }
    }
}

