using System;
using System.Linq;
using BrazilianUtilsService.helpers;

namespace BrazilianUtilsService.cpf.src
{
    public class ValidateCpf
    {
        private bool IsValidLength(string cpf) => cpf.Length.Equals(Constants.CPF_LENGTH);

        private bool BelongsToBlacklist(string cpf) => !String.IsNullOrWhiteSpace(Constants.BLACKLIST.FirstOrDefault(blacklist => blacklist.Contains(cpf)));

        private bool IsValidChecksum(string cpf) => 
            Constants.CHECK_DIGITS.All(predicate: verifierPos =>
            {
                string[] digits = cpf.ToCharArray(0, verifierPos).Select(c => c.ToString()).ToArray();
                int mod = Checksum.GenerateChecksum(digits, verifierPos + 1) % 11;

                return Convert.ToString(cpf[verifierPos]) == Convert.ToString(mod < 2 ? 0 : 11 - mod);
            });

        public bool IsValidCpf(string cpf) 
        {
            if(String.IsNullOrWhiteSpace(cpf))
                return false;

            string numericCpf = OnlyNumbers.FetchOnlyNumbers(cpf);

            return IsValidLength(numericCpf) && !BelongsToBlacklist(numericCpf) && IsValidChecksum(numericCpf);
        }
    }
}