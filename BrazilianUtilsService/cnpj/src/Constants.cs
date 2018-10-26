using System.Collections.Generic;

namespace BrazilianUtilsService.cnpj.src
{
    public static class Constants
    {
        public const int CNPJ_LENGHT = 14;
        public const string CNPJ_FORMAT_PATTERN = @"00\.000\.000\/0000\-00";
        public static readonly string[] BLACKLIST = 
        {
            "00000000000000", 
            "11111111111111",
            "22222222222222",
            "33333333333333",
            "44444444444444",
            "55555555555555",
            "66666666666666",
            "77777777777777",
            "88888888888888",
            "99999999999999"
        };
        public static List<int> WEIGHTS = new List<int> { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
        public static readonly int[] CHECK_DIGITS = { 12, 13 };
    }
}