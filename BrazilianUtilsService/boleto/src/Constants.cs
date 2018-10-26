using System.Collections.Generic;

namespace BrazilianUtilsService.boleto.src
{
    public static class Constants
    {
        public static readonly Dictionary<int, int> PARTIALS_TO_VERIFY_MOD_10 = new Dictionary<int, int> 
        {
            {0, 9},
            {10, 10},
            {21, 10}
        };
        public const int DIGITABLE_LINE_LENGTH = 47;
        public const int CHECK_DIGIT_MOD_11_POSITION = 4;
        public static readonly Dictionary<string, int> MOD_11_WEIGHTS = new Dictionary<string, int> 
        {
            {"Initial", 2},
            {"End", 9},
            {"Increment", 1}
        };
        public static readonly int[] MOD_10_WEIGHTS = { 2, 1 };
        public static readonly Dictionary<string, List<int>> DIGITABLE_LINE_TO_BOLETO_CONVERT_POSITIONS = new Dictionary<string, List<int>>
        {
            {"FirstPosition", new List<int>() {0, 4}},
            {"SecondPosition", new List<int>() {32, 15}},
            {"ThirdPosition", new List<int>() {4, 5}},
            {"FourthPosition", new List<int>() {10, 10}},
            {"FifthPosition", new List<int>() {21, 10}}
        };
        public const int DigitableLinePartialFirstDigitIndex = 9;
        public const int DigitableLinePartialSecondDigitIndex = 20;
        public const int DigitableLinePartialThirdDigitIndex = 31;
    }
}