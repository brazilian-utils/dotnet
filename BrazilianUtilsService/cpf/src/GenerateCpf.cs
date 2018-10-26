using System;
using System.Collections.Generic;
using System.Linq;
using BrazilianUtilsService.helpers;

namespace BrazilianUtilsService.cpf.src
{
    public class GenerateCpf
    {
        private int RandomNumber() => new Random().Next(10000000, 99999999);

        public string GenerateNewCpf(string state = "")
        {
            string stateCode = Constants.STATES_CODE.ContainsKey(state) ? Constants.STATES_CODE[state] : RandomNumber().ToString().Substring(2, 1);
            string[] digits = Convert.ToString(String.Concat(RandomNumber(), stateCode)).ToCharArray().Select(c => c.ToString()).ToArray();

            int mod1 = Checksum.GenerateChecksum(digits, 10) % 11;
            string check1 = (mod1 < 2 ? 0 : 11 - mod1).ToString();

            string basicCpf = String.Concat(digits);
            digits = String.Concat(basicCpf, check1).ToCharArray().Select(c => c.ToString()).ToArray();

            int mod2 = Checksum.GenerateChecksum(digits, 11) % 11;
            string check2 = (mod2 < 2 ? 0 : 11 - mod2).ToString();

            return String.Concat(String.Concat(digits), check2);
        }
    }
}