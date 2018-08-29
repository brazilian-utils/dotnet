using System.Text.RegularExpressions;

namespace BrazilianUtils
{
    public static class Helpers
    {
        public static string OnlyNumbers(string input) => Regex.Replace(input, @"[^\d]", string.Empty);
    }
}