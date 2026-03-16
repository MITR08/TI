using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TESTLAB1
{
    /// <summary>Один шаг шифрования/расшифровки решётки.</summary>
    public class GrilleStep
    {
        public int RotationDegrees { get; set; }
        public string Description { get; set; }
        public char[,] Matrix { get; set; }
        public string LettersThisRound { get; set; }
        /// <summary>Ячейки, заполненные на этом шаге (подсветка). null = не подсвечивать.</summary>
        public bool[,] HighlightCells { get; set; }
    }

    public class GrilleCipher
    {
        private readonly int _n;
        private readonly bool[,] _holes;

        public int Size => _n;
        public bool[,] Holes => (bool[,])_holes.Clone();

        public static List<Tuple<int, int>> GetOrbitCells(int n, int r, int c)
        {
            var set = new HashSet<Tuple<int, int>>();
            int rr = r, cc = c;
            for (int k = 0; k < 4; k++)
            {
                set.Add(Tuple.Create(rr, cc));
                int nextR = cc;
                int nextC = n - 1 - rr;
                rr = nextR;
                cc = nextC;
            }
            return new List<Tuple<int, int>>(set);
        }

        public static int ExpectedHoleCount(int n)
        {
            if (n % 2 == 0) return n * n / 4;
            return (n * n - 1) / 4 + 1;
        }

        private static bool IsCenter(int n, int r, int c)
        {
            return n % 2 == 1 && r == n / 2 && c == n / 2;
        }

        public GrilleCipher(int n, bool[,] holes)
        {
            if (n < 2 || n > 10) throw new ArgumentOutOfRangeException(nameof(n), "Размер решётки 2..10.");
            _n = n;
            _holes = (bool[,])holes.Clone();
        }

        public string Encrypt(string plainText)
        {
            string letters = Alphabets.FilterToAlphabet(plainText ?? "", Alphabets.English);
            int cells = _n * _n;
            int holeCount = CountHoles();
            int lettersPerRound = holeCount;
            int rounds = 4;
            int totalSlots = rounds * lettersPerRound;
            if (totalSlots == 0) return "";

            var random = new Random();
            var matrix = new char[_n, _n];
            for (int r = 0; r < _n; r++)
                for (int c = 0; c < _n; c++)
                    matrix[r, c] = Alphabets.English[random.Next(Alphabets.English.Length)];

            int pos = 0;
            for (int rot = 0; rot < 4; rot++)
            {
                bool[,] h = RotateHoles(rot);
                for (int r = 0; r < _n; r++)
                    for (int c = 0; c < _n; c++)
                    {
                        if (!h[r, c]) continue;
                        if (IsCenter(_n, r, c) && rot > 0) continue;
                        matrix[r, c] = pos < letters.Length ? letters[pos] : Alphabets.English[random.Next(Alphabets.English.Length)];
                        pos++;
                    }
            }

            var sb = new StringBuilder(cells);
            for (int r = 0; r < _n; r++)
                for (int c = 0; c < _n; c++)
                    sb.Append(matrix[r, c]);
            return sb.ToString();
        }

        public string Decrypt(string cipherText)
        {
            string letters = Alphabets.FilterToAlphabet(cipherText ?? "", Alphabets.English);
            int cells = _n * _n;
            if (letters.Length < cells) throw new ArgumentException("Недостаточно символов для расшифровки решётки этого размера.");

            var matrix = new char[_n, _n];
            int idx = 0;
            for (int r = 0; r < _n; r++)
                for (int c = 0; c < _n; c++)
                    matrix[r, c] = letters[idx++];

            var sb = new StringBuilder();
            for (int rot = 0; rot < 4; rot++)
            {
                bool[,] h = RotateHoles(rot);
                for (int r = 0; r < _n; r++)
                    for (int c = 0; c < _n; c++)
                    {
                        if (!h[r, c]) continue;
                        if (IsCenter(_n, r, c) && rot > 0) continue;
                        sb.Append(matrix[r, c]);
                    }
            }
            return sb.ToString();
        }

        private int CountHoles()
        {
            int k = 0;
            for (int r = 0; r < _n; r++)
                for (int c = 0; c < _n; c++)
                    if (_holes[r, c]) k++;
            return k;
        }

        private bool[,] RotateHoles(int rot)
        {
            bool[,] h = _holes;
            for (int k = 0; k < rot; k++)
                h = Rotate90CW(h);
            return h;
        }

        private bool[,] Rotate90CW(bool[,] a)
        {
            int n = a.GetLength(0);
            var b = new bool[n, n];
            for (int r = 0; r < n; r++)
                for (int c = 0; c < n; c++)
                    b[c, n - 1 - r] = a[r, c];
            return b;
        }

        public Tuple<string, List<GrilleStep>> EncryptWithSteps(string plainText)
        {
            string letters = Alphabets.FilterToAlphabet(plainText ?? "", Alphabets.English);
            int holeCount = CountHoles();
            int lettersPerRound = holeCount;
            if (lettersPerRound == 0) return Tuple.Create("", new List<GrilleStep>());

            var random = new Random();
            var steps = new List<GrilleStep>();

            // Шаг 0: только исходные буквы (все нужные буквы по порядку)
            steps.Add(new GrilleStep
            {
                RotationDegrees = -3,
                Description = "Исходный текст",
                Matrix = null,
                LettersThisRound = letters,
                HighlightCells = null
            });

            // Матрица: пока заполняем только отверстия, остальные ячейки = '\0'
            var matrix = new char[_n, _n];
            int pos = 0;

            for (int rot = 0; rot < 4; rot++)
            {
                bool[,] h = RotateHoles(rot);
                var highlightThisStep = new bool[_n, _n];
                var roundLetters = new StringBuilder();
                for (int r = 0; r < _n; r++)
                    for (int c = 0; c < _n; c++)
                    {
                        if (!h[r, c]) continue;
                        if (IsCenter(_n, r, c) && rot > 0) continue;
                        char ch = pos < letters.Length ? letters[pos] : Alphabets.English[random.Next(Alphabets.English.Length)];
                        matrix[r, c] = ch;
                        highlightThisStep[r, c] = true;
                        roundLetters.Append(ch);
                        pos++;
                    }
                steps.Add(new GrilleStep
                {
                    RotationDegrees = rot * 90,
                    Description = rot == 0 ? "Поворот 0°" : $"Поворот {rot * 90}°.",
                    Matrix = (char[,])matrix.Clone(),
                    LettersThisRound = roundLetters.ToString(),
                    HighlightCells = highlightThisStep
                });
            }

            // Шаг: заполняем оставшиеся ячейки случайными буквами (хаотичное заполнение)
            var randomHighlight = new bool[_n, _n];
            for (int r = 0; r < _n; r++)
                for (int c = 0; c < _n; c++)
                    if (matrix[r, c] == '\0')
                    {
                        matrix[r, c] = Alphabets.English[random.Next(Alphabets.English.Length)];
                        randomHighlight[r, c] = true;
                    }
            steps.Add(new GrilleStep
            {
                RotationDegrees = -4,
                Description = "Заполняем оставшиеся ячейки случайными буквами (в хаотичном порядке).",
                Matrix = (char[,])matrix.Clone(),
                LettersThisRound = "",
                HighlightCells = randomHighlight
            });

            var sb = new StringBuilder(_n * _n);
            for (int r = 0; r < _n; r++)
                for (int c = 0; c < _n; c++)
                    sb.Append(matrix[r, c]);
            steps.Add(new GrilleStep
            {
                RotationDegrees = -1,
                Description = "Читаем матрицу построчно (слева направо, сверху вниз) → шифротекст.",
                Matrix = (char[,])matrix.Clone(),
                LettersThisRound = sb.ToString(),
                HighlightCells = null
            });
            return Tuple.Create(sb.ToString(), steps);
        }

        public Tuple<string, List<GrilleStep>> DecryptWithSteps(string cipherText)
        {
            string letters = Alphabets.FilterToAlphabet(cipherText ?? "", Alphabets.English);
            if (letters.Length < _n * _n) throw new ArgumentException("Недостаточно символов для расшифровки.");

            var matrix = new char[_n, _n];
            int idx = 0;
            for (int r = 0; r < _n; r++)
                for (int c = 0; c < _n; c++)
                    matrix[r, c] = letters[idx++];

            var steps = new List<GrilleStep>();
            steps.Add(new GrilleStep
            {
                RotationDegrees = -2,
                Description = "Заполняем матрицу шифротекстом построчно.",
                Matrix = (char[,])matrix.Clone(),
                LettersThisRound = ""
            });

            var plainBuilder = new StringBuilder();
            for (int rot = 0; rot < 4; rot++)
            {
                bool[,] h = RotateHoles(rot);
                var roundLetters = new StringBuilder();
                for (int r = 0; r < _n; r++)
                    for (int c = 0; c < _n; c++)
                    {
                        if (!h[r, c]) continue;
                        if (IsCenter(_n, r, c) && rot > 0) continue;
                        roundLetters.Append(matrix[r, c]);
                        plainBuilder.Append(matrix[r, c]);
                    }
                steps.Add(new GrilleStep
                {
                    RotationDegrees = rot * 90,
                    Description = rot == 0 ? "Поворот 0°. Читаем буквы из отверстий." : $"Поворот {rot * 90}°. Читаем следующие буквы из отверстий.",
                    Matrix = (char[,])matrix.Clone(),
                    LettersThisRound = roundLetters.ToString()
                });
            }
            steps.Add(new GrilleStep
            {
                RotationDegrees = -1,
                Description = "Прочитанная последовательность — открытый текст.",
                Matrix = null,
                LettersThisRound = plainBuilder.ToString()
            });
            return Tuple.Create(plainBuilder.ToString(), steps);
        }
    }
}
