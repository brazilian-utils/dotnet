using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BrazilianUtilsService.helpers;

namespace BrazilianUtilsService.boleto.src {
    public class ValidateBoleto {
        private bool IsValidLength (string digitableLine) => digitableLine.Length.Equals(Constants.DIGITABLE_LINE_LENGTH);

        private int Mod10(string input)
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

        private int Mod11(string input)
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

        private bool validateDigitableLinePartials(string input) =>
            Constants.PARTIALS_TO_VERIFY_MOD_10.All
            (partial => 
                input[Constants.DigitableLinePartialFirstDigitIndex] - '0'
                    == Mod10(input: input.Substring(0, 9)) &&
                input[Constants.DigitableLinePartialSecondDigitIndex] - '0'
                    == Mod10(input: input.Substring(10, 10)) &&
                input[Constants.DigitableLinePartialThirdDigitIndex] - '0'
                    == Mod10(input.Substring(21, 10))
            );
       
        private string ParseDigitableLine(string input)
        {
            string parsedDigitableLine = string.Empty;

            Constants.DIGITABLE_LINE_TO_BOLETO_CONVERT_POSITIONS.ToList().ForEach(position =>
            {
                parsedDigitableLine += input.Substring(position.Value[0], position.Value[1]);
            });
            
            return parsedDigitableLine;
        }

        public bool IsValidBoleto(string digitableLine)
        {
            if (string.IsNullOrWhiteSpace(digitableLine))
                return false;

            digitableLine = OnlyNumbers.FetchOnlyNumbers(digitableLine);

            return IsValidLength(digitableLine) && 
            validateDigitableLinePartials(digitableLine) &&
            Mod11(ParseDigitableLine(digitableLine).Remove(4, 1)).ToString().Equals(Convert.ToString(ParseDigitableLine(digitableLine)[4]));
        }
    }
}