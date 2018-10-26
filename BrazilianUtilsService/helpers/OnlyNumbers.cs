using System;
using System.Text.RegularExpressions;

namespace BrazilianUtilsService.helpers
{
    public static class OnlyNumbers
    {
        public static string FetchOnlyNumbers(string input)
        {
            return Regex.Replace(input, @"[^\d]", "");
        }
    }
}