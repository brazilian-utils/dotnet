using System.Text;

namespace BrazilianUtils
{
    public static class Helpers
    {
        public static string OnlyNumbers(string input)
        {
            var sb = new StringBuilder();

            foreach (var c in input)
            {
                if (char.IsDigit(c))
                    sb.Append(c);
            }

            return sb.ToString();
        }
    }
}
