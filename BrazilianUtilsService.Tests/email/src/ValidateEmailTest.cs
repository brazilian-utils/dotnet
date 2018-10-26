using BrazilianUtilsService.email.src;
using Xunit;

namespace BrazilianUtilsService.Tests.mail.src
{
    public class ValidateEmailTest
    {
         private readonly ValidateEmail _validateEmail;

        public ValidateEmailTest()
        {
            _validateEmail = new ValidateEmail();
        }

        [Theory]
        [InlineData("john.doe.teste.com.br")]
        [InlineData("jóhn.doe@yahoo.com.br")]
        [InlineData("jóhn doe@yahoo.com.br")]
        [InlineData("jóhn&doe@yahoo.com.br")]
        public void shouldReturnFalse(string value)
        {
            var result = _validateEmail.IsValidEmail(value);
            Assert.False(result);
        }

        [Theory]
        [InlineData("john.doe@hotmail.com")]
        [InlineData("john.doe@gmail.com")]
        [InlineData("john.doe@yahoo.com.br")]
        [InlineData("john_doe@myenterprise.com.br")]
        public void shouldReturnTrue(string value)
        {
            var result = _validateEmail.IsValidEmail(value);
            Assert.True(result);
        }
    }
}