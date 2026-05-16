using System.Globalization;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.Win32;

namespace RsaSignatureLab;

public partial class MainWindow : Window
{
    private readonly UTF8Encoding _utf8Write = new(false);

    private bool _muteDigits;
    private string? _signFilePath;
    private string? _verifyFilePath;
    private string _signedPayloadUtf8 = string.Empty;
    private string _signedSuggestedFilename = "message_signed.txt";

    public MainWindow()
    {
        InitializeComponent();
    }

    private void DigitsField_OnTextChanged(object sender, TextChangedEventArgs e)
    {
        if (_muteDigits)
            return;

        if (sender is not TextBox box)
            return;

        var raw = box.Text;
        var sanitized = FilterDigitsAscii(raw);

        if (sanitized == raw)
            return;

        var caretBefore = Math.Min(box.SelectionStart, raw.Length);
        var leftPart = caretBefore <= 0 ? string.Empty : raw[..caretBefore];
        var caretAfterSanitize = FilterDigitsAscii(leftPart).Length;

        _muteDigits = true;
        try
        {
            box.Text = sanitized;
            box.SelectionStart = Math.Min(Math.Max(caretAfterSanitize, 0), sanitized.Length);
        }
        finally
        {
            _muteDigits = false;
        }
    }

    private static string FilterDigitsAscii(string s)
    {
        if (string.IsNullOrEmpty(s))
            return string.Empty;

        var sb = new StringBuilder(s.Length);
        foreach (var ch in s)
        {
            if (char.IsAsciiDigit(ch))
                sb.Append(ch);
        }

        return sb.ToString();
    }

    private void BtnBrowseSign_OnClick(object sender, RoutedEventArgs e)
    {
        var dlg = new OpenFileDialog
        {
            Filter = "Текстовые файлы (*.txt)|*.txt|Все файлы (*.*)|*.*",
            RestoreDirectory = true,
        };

        if (dlg.ShowDialog(this) != true)
            return;

        _signFilePath = dlg.FileName;
        TxtSignPath.Text = dlg.FileName;
    }

    private void BtnBrowseVerify_OnClick(object sender, RoutedEventArgs e)
    {
        var dlg = new OpenFileDialog
        {
            Filter = "Текстовые файлы (*.txt)|*.txt|Все файлы (*.*)|*.*",
            RestoreDirectory = true,
        };

        if (dlg.ShowDialog(this) != true)
            return;

        _verifyFilePath = dlg.FileName;
        TxtVerifyPath.Text = dlg.FileName;
    }

    private void BtnSign_OnClick(object sender, RoutedEventArgs e)
    {
        if (_signFilePath == null || !File.Exists(_signFilePath))
        {
            MessageBox.Show(this,
                "Выберите текстовый файл для подписи.",
                Title,
                MessageBoxButton.OK,
                MessageBoxImage.Information);
            return;
        }

        RsaValidatedParams parameters;
        try
        {
            parameters = RsaCrypto.ValidateParams(TxtP.Text, TxtQ.Text, TxtD.Text);
        }
        catch (Exception ex)
        {
            MessageBox.Show(this,
                string.IsNullOrEmpty(ex.Message) ? "Ошибка параметров." : ex.Message,
                Title,
                MessageBoxButton.OK,
                MessageBoxImage.Warning);
            return;
        }

        string text;
        try
        {
            text = File.ReadAllText(_signFilePath, Encoding.UTF8);
        }
        catch (Exception)
        {
            MessageBox.Show(this,
                "Ошибка чтения файла.",
                Title,
                MessageBoxButton.OK,
                MessageBoxImage.Error);
            return;
        }

        try
        {
            var (signature, hash, mCanonical, recovered) = RsaCrypto.ComputeSignature(text, parameters);
            if (recovered != mCanonical)
            {
                MessageBox.Show(this,
                    "Внутренняя проверка: S^e mod r не совпало с h(M).",
                    Title,
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
            }

            TxtE.Text = parameters.E.ToString(CultureInfo.InvariantCulture);
            TxtHash.Text = hash.ToString(CultureInfo.InvariantCulture);
            TxtSignature.Text = signature.ToString(CultureInfo.InvariantCulture);

            var builder = new StringBuilder(text.Length + 1 + signature.ToString(CultureInfo.InvariantCulture).Length)
                .Append(text)
                .Append('\n')
                .Append(signature.ToString(CultureInfo.InvariantCulture));
            _signedPayloadUtf8 = builder.ToString();

            var baseName = Path.GetFileNameWithoutExtension(_signFilePath);
            _signedSuggestedFilename = string.IsNullOrEmpty(baseName) ? "message_signed.txt" : $"{baseName}_signed.txt";

            SigningOutputPanel.Visibility = Visibility.Visible;
            BtnSaveSigned.IsEnabled = true;
        }
        catch (Exception ex)
        {
            SigningOutputPanel.Visibility = Visibility.Collapsed;
            BtnSaveSigned.IsEnabled = false;
            _signedPayloadUtf8 = string.Empty;

            MessageBox.Show(this,
                string.IsNullOrEmpty(ex.Message) ? "Ошибка подписи." : ex.Message,
                Title,
                MessageBoxButton.OK,
                MessageBoxImage.Warning);
        }
    }

    private void BtnSaveSigned_OnClick(object sender, RoutedEventArgs e)
    {
        if (string.IsNullOrEmpty(_signedPayloadUtf8))
        {
            MessageBox.Show(this,
                "Сначала вычислите подпись.",
                Title,
                MessageBoxButton.OK,
                MessageBoxImage.Information);
            return;
        }

        var dlg = new SaveFileDialog
        {
            Filter = "Текстовые файлы (*.txt)|*.txt|Все файлы (*.*)|*.*",
            FileName = _signedSuggestedFilename,
            DefaultExt = ".txt",
            AddExtension = true,
            OverwritePrompt = true,
            RestoreDirectory = true,
        };

        if (dlg.ShowDialog(this) != true)
            return;

        try
        {
            File.WriteAllText(dlg.FileName, _signedPayloadUtf8, _utf8Write);
        }
        catch (Exception ex)
        {
            MessageBox.Show(this,
                string.IsNullOrEmpty(ex.Message) ? "Не удалось сохранить файл." : ex.Message,
                Title,
                MessageBoxButton.OK,
                MessageBoxImage.Error);
        }
    }

    private void BtnVerify_OnClick(object sender, RoutedEventArgs e)
    {
        if (_verifyFilePath == null || !File.Exists(_verifyFilePath))
        {
            MessageBox.Show(this,
                "Выберите файл с подписью.",
                Title,
                MessageBoxButton.OK,
                MessageBoxImage.Information);
            return;
        }

        RsaValidatedParams parameters;
        try
        {
            parameters = RsaCrypto.ValidateParams(TxtP.Text, TxtQ.Text, TxtD.Text);
        }
        catch (Exception ex)
        {
            MessageBox.Show(this,
                string.IsNullOrEmpty(ex.Message) ? "Ошибка параметров." : ex.Message,
                Title,
                MessageBoxButton.OK,
                MessageBoxImage.Warning);
            return;
        }

        string full;
        try
        {
            full = File.ReadAllText(_verifyFilePath, Encoding.UTF8);
        }
        catch (Exception)
        {
            MessageBox.Show(this,
                "Ошибка чтения файла.",
                Title,
                MessageBoxButton.OK,
                MessageBoxImage.Error);
            return;
        }

        VerifiedBundle? bundle;
        try
        {
            bundle = RsaCrypto.VerifySignedFile(full, parameters);
        }
        catch (Exception ex)
        {
            VerificationPanel.Visibility = Visibility.Visible;
            VerifyFigures.Visibility = Visibility.Collapsed;

            VerifyTitle.Text = "Ошибка проверки";
            VerifyTitle.Foreground = (Brush?)TryFindResource("AccentDanger") ?? Brushes.DeepPink;
            TxtVerifyExplanation.Text = string.IsNullOrEmpty(ex.Message)
                ? "Ошибка проверки."
                : ex.Message;

            return;
        }

        VerificationPanel.Visibility = Visibility.Visible;
        VerifyFigures.Visibility = Visibility.Visible;

        VerifyTitle.Text = bundle.Ok ? "Подпись верна." : "Подпись неверна.";
        VerifyTitle.Foreground = bundle.Ok
            ? (Brush?)TryFindResource("AccentSuccess") ?? Brushes.LightGreen
            : (Brush?)TryFindResource("AccentDanger") ?? Brushes.DeepPink;

        TxtVerifyExplanation.Text = bundle.Reason;

        TxtVerifyR.Text = bundle.R;
        TxtVerifyMPrime.Text = bundle.MPrime;
        TxtVerifyFromSig.Text = bundle.MFromSig;
        TxtVerifyS.Text = bundle.S;
    }
}
