using System.Text;

namespace AdminApp.Extensions
{
    public static class LuceneQueryEscaper
    {
        private static readonly char[] SpecialChars =
        {
            '+', '-', '&', '|', '!', '(', ')', '{', '}', '[', ']', '^',
            '"', '~', '*', '?', ':', '\\', '/'
        };

        public static string Escape(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return input;
            }

            var sb = new StringBuilder(input.Length);
            foreach (var c in input)
            {
                if (Array.IndexOf(SpecialChars, c) >= 0)
                {
                    sb.Append('\\');
                }
                sb.Append(c);
            }

            return sb.ToString();
        }
    }
}
