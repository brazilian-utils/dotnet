using System.Text;

namespace BrazilianUtils
{
    public class Boleto
    {
        const int DIGITABLE_LINE_LENGTH = 47;

        // Layout - Código de Barras - Versão 5 - 01.08.2016
        // https://bit.ly/2rksYOC
        public static bool IsValid(string input, bool isRequired = true)

        {
            if (isRequired && string.IsNullOrEmpty(input))
                return false;

            if (!isRequired && string.IsNullOrEmpty(input))
                return true;

            var clearValue = Helpers.OnlyNumbers(input);

            if (clearValue.Length != 44 && clearValue.Length != 47)
                return false;

            if (IsDigitableLine(clearValue))
            {
                if (!validateDigitableLinePartials(clearValue))
                    return false;

                clearValue = ParseDigitableLine(clearValue);
            }

            var checkDigit = clearValue[4] - '0';

            return checkDigit == Mod11(clearValue.Remove(4, 1));
        }

        private static bool IsDigitableLine(string input) => input.Length == DIGITABLE_LINE_LENGTH;

        private static int Mod10(string input)
        {
            var sum = 0;
            var weights = new int[2] { 2, 1 };

            for (int i = input.Length - 1, pos = 0; i > -1; i--, pos++)
            {
                var digit = input[i] - '0';
                var mult = digit * weights[pos % 2];

                sum += mult > 9 ? (1 + mult % 10) : mult;
            }

            var mod = 0;

            if (sum % 10 > 0)
                mod = 10 - (sum % 10);

            return mod;
        }

        private static int Mod11(string input)
        {
            var sum = 0;
            var weight = 2;

            for (int i = input.Length - 1; i > -1; i--)
            {
                var digit = input[i] - '0';
                sum += digit * weight;

                weight = weight < 9 ? weight + 1 : 2;
            }

            var mod = sum % 11;

            if (mod == 0 || mod == 1)
                return 1;

            return 11 - mod;
        }

        private static string ParseDigitableLine(string input)
        {
            var sb = new StringBuilder();

            sb.Append(input.Substring(0, 4))
                .Append(input.Substring(32, 15))
                .Append(input.Substring(4, 5))
                .Append(input.Substring(10, 10))
                .Append(input.Substring(21, 10));

            return sb.ToString();
        }

        private static bool validateDigitableLinePartials(string input)
        {
            int digit1 = input[9] - '0',
                digit2 = input[20] - '0',
                digit3 = input[31] - '0';

            return digit1 == Mod10(input.Substring(0, 9)) &&
                digit2 == Mod10(input.Substring(10, 10)) &&
                digit3 == Mod10(input.Substring(21, 10));
        }
    }
}
