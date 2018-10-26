using System;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using BrazilianUtilsService.helpers;

namespace BrazilianUtilsService.cnpj.src
{
    public class FormatCnpj
    {
        public string Format(string cnpj)
        {   
            if (string.IsNullOrWhiteSpace(cnpj))
                return string.Empty;

            string numericCNPJ = OnlyNumbers.FetchOnlyNumbers(cnpj);

            return  Convert.ToUInt64(numericCNPJ.Substring(0, Math.Min(Constants.CNPJ_LENGHT, numericCNPJ.Length))).ToString(Constants.CNPJ_FORMAT_PATTERN);
        }
    }
}