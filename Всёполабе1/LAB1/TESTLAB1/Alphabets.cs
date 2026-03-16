using System.Linq;
using System.Text;

namespace TESTLAB1
{
    /// <summary>
    /// Алфавиты и фильтрация символов.
    /// </summary>
    public static class Alphabets
    {
        public const string English = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        public const string Russian = "АБВГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯ";

        public static string FilterToAlphabet(string input, string alphabet)
        {
            if (string.IsNullOrEmpty(input) || string.IsNullOrEmpty(alphabet)) return "";
            var upper = input.ToUpperInvariant();
            var set = new System.Collections.Generic.HashSet<char>(alphabet);
            return new string(upper.Where(c => set.Contains(c)).ToArray());
        }

        public static string SanitizeKey(string key, string alphabet)
        {
            return FilterToAlphabet(key ?? "", alphabet);
        }

        public static int IndexInAlphabet(char c, string alphabet)
        {
            int i = alphabet.IndexOf(char.ToUpperInvariant(c));
            return i >= 0 ? i : -1;
        }
    }
}
