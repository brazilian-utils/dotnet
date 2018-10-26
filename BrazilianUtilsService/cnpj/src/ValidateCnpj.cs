using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using BrazilianUtilsService.helpers;

namespace BrazilianUtilsService.cnpj.src
{
    public class ValidateCnpj
    {
        private bool IsValidLength(string cnpj) => cnpj.Length.Equals(Constants.CNPJ_LENGHT);

        private bool BelongsToBlacklist(string cnpj) => !String.IsNullOrWhiteSpace(Constants.BLACKLIST.FirstOrDefault(blacklist => blacklist.Contains(cnpj)));

        private bool IsValidChecksum(string cnpj) => 
            Constants.CHECK_DIGITS.All(predicate: verifierPos =>
            {
                string[] digits = cnpj.ToCharArray(0, verifierPos).Select(c => c.ToString()).ToArray();

                if (verifierPos == Constants.CHECK_DIGITS[Constants.CHECK_DIGITS.Length - 1])
                    Constants.WEIGHTS.Insert(0, 6);

                int mod = Checksum.GenerateChecksum(digits, Constants.WEIGHTS) % 11;

                return Convert.ToString(cnpj[verifierPos]) == Convert.ToString(mod < 2 ? 0 : 11 - mod);
            });

        public bool IsValidCnpj(string cnpj) 
        {
            if(String.IsNullOrWhiteSpace(cnpj))
                return false;

            string numericCnpj = OnlyNumbers.FetchOnlyNumbers(cnpj);

            return IsValidLength(numericCnpj) && !BelongsToBlacklist(numericCnpj) && IsValidChecksum(numericCnpj);
        }
    }
}