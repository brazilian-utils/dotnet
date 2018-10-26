using System;
using System.Collections.Generic;
using System.Linq;
using BrazilianUtilsService.helpers;

namespace BrazilianUtilsService.phone.src
{
    public class ValidatePhone
    {
        private bool IsValidLength(string phone) => phone.Length >= Constants.PHONE_MIN_LENGTH && phone.Length <= Constants.PHONE_MAX_LENGTH;

        private bool IsValidCode(string phone) => Constants.WHITELIST_STATES.Contains(Convert.ToInt32(phone.Substring(0, 2)));

        private bool IsValidNumberStart(string phone) 
        {
            if(phone.Length == Constants.PHONE_MIN_LENGTH)
                return Constants.LANDLINE_NUMBERS.Contains(Convert.ToInt32(phone[2].ToString()));

            return Constants.MOBILE_NUMBERS.Contains(Convert.ToInt32(phone[2].ToString()));
        }
            
        public bool IsValidPhone(string phone) 
        {
            if(String.IsNullOrWhiteSpace(phone))
                return false;

            string numericPhone = OnlyNumbers.FetchOnlyNumbers(phone);

            return IsValidLength(numericPhone) && IsValidCode(numericPhone) && IsValidNumberStart(numericPhone);
        }
    }
}