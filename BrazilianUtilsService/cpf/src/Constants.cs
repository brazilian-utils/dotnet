using System.Collections.Generic;

namespace BrazilianUtilsService.cpf.src
{
    public static class Constants
    {
        public const int CPF_LENGTH = 11;
        public const string CPF_FORMAT_PATTERN = @"000\.000\.000\-00";
        public static readonly string[] BLACKLIST = 
        {
            "00000000000", 
            "11111111111",
            "22222222222",
            "33333333333",
            "44444444444",
            "55555555555",
            "66666666666",
            "77777777777",
            "88888888888",
            "99999999999"
        };
        public static readonly int[] CHECK_DIGITS = { 9, 10 };
        public static readonly Dictionary<string, string> STATES_CODE = new Dictionary<string, string>
        {
            {"RS", "0"},
            {"DF", "1"},
            {"GO", "1"},
            {"MT", "1"},
            {"MS", "1"},
            {"TO", "1"},
            {"AM", "2"},
            {"PA", "2"},
            {"RR", "2"},
            {"AP", "2"},
            {"AC", "2"},
            {"RO", "2"},
            {"CE", "3"},
            {"MA", "3"},
            {"PI", "3"},
            {"PB", "4"},
            {"PE", "4"},
            {"AL", "4"},
            {"RN", "4"},
            {"BA", "5"},
            {"SE", "5"},
            {"MG", "6"},
            {"RJ", "7"},
            {"ES", "7"},
            {"SP", "8"},
            {"PR", "9"},
            {"SC", "9"}
        };
    }
}