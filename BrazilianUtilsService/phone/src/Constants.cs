using System.Collections.Generic;

namespace BrazilianUtilsService.phone.src
{
    public static class Constants
    {
        public static readonly List<int> WHITELIST_STATES = new List<int>()
        {
            11,
            12,
            13,
            14,
            15,
            16,
            17,
            18,
            19,
            21,
            22,
            24,
            27,
            28,
            31,
            32,
            33,
            34,
            35,
            37,
            38,
            41,
            42,
            43,
            44,
            45,
            46,
            47,
            48,
            49,
            51,
            53,
            54,
            55,
            61,
            62,
            63,
            64,
            65,
            66,
            67,
            68,
            69,
            71,
            73,
            74,
            75,
            77,
            79,
            81,
            82,
            83,
            84,
            85,
            86,
            87,
            88,
            89,
            91,
            92,
            93,
            94,
            95,
            96,
            97,
            98,
            99
        };
        public const int PHONE_MIN_LENGTH = 10;
        public const int PHONE_MAX_LENGTH = 11;
        public static readonly List<int> LANDLINE_NUMBERS = new List<int>() { 2, 3, 4, 5 };
        public static readonly List<int> MOBILE_NUMBERS = new List<int>() { 6, 7, 8, 9 };
    }
}