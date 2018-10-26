using System.Text.RegularExpressions;

namespace BrazilianUtilsService.email.src
{
    public class ValidateEmail
    {
        private const string REGEX_PATTERN_VALID_EMAIL = "^[a-zA-Z0-9_.-]+@[a-zA-Z0-9-]+.[a-zA-Z0-9-.]+$"; 
        
        private bool IsEmailContentValid(string email) => Regex.Match(email, REGEX_PATTERN_VALID_EMAIL).Success;

        public bool IsValidEmail(string email)
        {
            if(string.IsNullOrWhiteSpace(email))
                return false;

            return IsEmailContentValid(email);
        }
    }
}