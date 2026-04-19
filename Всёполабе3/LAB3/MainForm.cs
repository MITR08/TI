using System.Numerics;

namespace Lab3WinForms;

public partial class MainForm : Form
{
    private string? _lastEncryptInputPath;
    private string? _lastDecryptInputPath;

    public MainForm()
    {
        InitializeComponent();
    }

    private async void BtnFindRoots_Click(object? sender, EventArgs e)
    {
        try
        {
            var p = Crypto.ParseDecimalStrict(txtP.Text);
            if (!Crypto.IsProbablePrime(p))
            {
                MessageBox.Show("p должно быть простым числом.", "Параметры", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            UseWaitCursor = true;
            btnFindRoots.Enabled = false;
            List<BigInteger> all;
            try
            {
                all = await Task.Run(() => Crypto.FindAllPrimitiveRoots(p)).ConfigureAwait(true);
            }
            finally
            {
                UseWaitCursor = false;
                btnFindRoots.Enabled = true;
            }

            cmbG.Items.Clear();
            foreach (var r in all)
                cmbG.Items.Add(r.ToString());
            cmbG.Enabled = all.Count > 0;
            if (all.Count > 0)
            {
                cmbG.SelectedIndex = 0;
                lblRoots.Text = $"Выбор первообразного корня g по модулю p — найдено корней: {all.Count}";
            }
            else
            {
                lblRoots.Text = "Выбор первообразного корня g по модулю p — корней не найдено";
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void BtnBrowseEncrypt_Click(object? sender, EventArgs e)
    {
        using var dlg = new OpenFileDialog
        {
            Title = "Файл для шифрования",
            Filter = "Все файлы|*.*",
        };
        if (dlg.ShowDialog(this) == DialogResult.OK)
        {
            _lastEncryptInputPath = dlg.FileName;
            txtEncryptPath.Text = dlg.FileName;
        }
    }

    private void BtnBrowseDecrypt_Click(object? sender, EventArgs e)
    {
        using var dlg = new OpenFileDialog
        {
            Title = "Файл шифротекста",
            Filter = "Шифротекст (*.enc)|*.enc|Все файлы|*.*",
        };
        if (dlg.ShowDialog(this) == DialogResult.OK)
        {
            _lastDecryptInputPath = dlg.FileName;
            txtDecryptPath.Text = dlg.FileName;
        }
    }

    private void BtnEncrypt_Click(object? sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(_lastEncryptInputPath) || !File.Exists(_lastEncryptInputPath))
        {
            MessageBox.Show("Выберите файл для шифрования.", "Файл", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        var gStr = cmbG.SelectedItem?.ToString()?.Trim() ?? "";
        if (string.IsNullOrEmpty(gStr))
        {
            MessageBox.Show(
                "Сначала нажмите «Найти все первообразные корни g для p» и выберите g в списке.",
                "Параметры",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
            return;
        }

        try
        {
            var (p, x, k, g) = Crypto.ValidateEncryptParams(txtP.Text, txtX.Text, txtK.Text, gStr);

            UseWaitCursor = true;
            byte[] plain;
            try
            {
                plain = File.ReadAllBytes(_lastEncryptInputPath);
            }
            finally
            {
                UseWaitCursor = false;
            }

            UseWaitCursor = true;
            byte[] enc;
            try
            {
                enc = Crypto.EncryptBytes(plain, p, x, k, g);
            }
            finally
            {
                UseWaitCursor = false;
            }

            txtPreview.Text = Crypto.CiphertextPairsDecimalPreview(enc);

            var baseName = Path.GetFileName(_lastEncryptInputPath);
            using var save = new SaveFileDialog
            {
                Title = "Сохранить шифротекст",
                Filter = "Шифротекст (*.enc)|*.enc|Все файлы|*.*",
                FileName = $"{baseName}.enc",
            };
            if (save.ShowDialog(this) == DialogResult.OK)
            {
                File.WriteAllBytes(save.FileName, enc);
                lblStatus.Text = $"Сохранено: {save.FileName}";
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Шифрование", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void BtnDecrypt_Click(object? sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(_lastDecryptInputPath) || !File.Exists(_lastDecryptInputPath))
        {
            MessageBox.Show("Выберите файл шифротекста (.enc).", "Файл", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        try
        {
            Crypto.ValidateDecryptParams(txtP.Text, txtX.Text);
            var p = Crypto.ParseDecimalStrict(txtP.Text);
            var x = Crypto.ParseDecimalStrict(txtX.Text);

            UseWaitCursor = true;
            byte[] bytes;
            try
            {
                bytes = File.ReadAllBytes(_lastDecryptInputPath);
            }
            finally
            {
                UseWaitCursor = false;
            }

            txtPreview.Text = Crypto.CiphertextPairsDecimalPreview(bytes);

            if (bytes.Length == 0)
            {
                using var save = new SaveFileDialog
                {
                    Title = "Сохранить расшифрованный файл",
                    Filter = "Все файлы|*.*",
                    FileName = Path.GetFileNameWithoutExtension(_lastDecryptInputPath) ?? "decrypted.bin",
                };
                if (save.ShowDialog(this) == DialogResult.OK)
                {
                    File.WriteAllBytes(save.FileName, []);
                    lblStatus.Text = $"Сохранено пустой файл: {save.FileName}";
                }
                return;
            }

            UseWaitCursor = true;
            byte[] plain;
            try
            {
                plain = Crypto.DecryptBytes(bytes, p, x);
            }
            finally
            {
                UseWaitCursor = false;
            }

            var suggested = Path.GetFileNameWithoutExtension(_lastDecryptInputPath);
            if (string.IsNullOrEmpty(suggested))
                suggested = "decrypted.bin";

            using var saveDlg = new SaveFileDialog
            {
                Title = "Сохранить расшифрованный файл",
                Filter = "Все файлы|*.*",
                FileName = suggested,
            };
            if (saveDlg.ShowDialog(this) == DialogResult.OK)
            {
                File.WriteAllBytes(saveDlg.FileName, plain);
                lblStatus.Text = $"Сохранено: {saveDlg.FileName}";
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Дешифрование", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
