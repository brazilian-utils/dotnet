using System;
using BrazilianUtilsService.helpers;

namespace BrazilianUtilsService.cpf.src
{
    public class FormatCpf
    {
         public string Format(string cpf)
        {   
            if (string.IsNullOrWhiteSpace(cpf))
                return string.Empty;

            string numericCPF = OnlyNumbers.FetchOnlyNumbers(cpf);

            return  Convert.ToUInt64(numericCPF.Substring(0, Math.Min(Constants.CPF_LENGTH, numericCPF.Length))).ToString(Constants.CPF_FORMAT_PATTERN);
        }
    }
}