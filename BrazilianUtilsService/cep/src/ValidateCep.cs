using System;
using BrazilianUtilsService.helpers;

namespace BrazilianUtilsService.cep.src
{
    public class ValidateCep
    {        
        private bool IsValidLength(string cep) => cep.Length.Equals(Constants.CEP_LENGTH);

        public bool IsValidCep(string cep) 
        {
            if(String.IsNullOrWhiteSpace(cep))
                return false;

            string numericCep = OnlyNumbers.FetchOnlyNumbers(cep);

            return IsValidLength(numericCep);
        }
    }
}