using BrazilianUtilsService.phone.src;
using Xunit;

namespace BrazilianUtilsService.Tests.phone.src
{
    public class ValidatePhoneTest
    {
        private readonly ValidatePhone _validatePhone;

        public ValidatePhoneTest()
        {
            _validatePhone = new ValidatePhone();
        }

        [Theory]
        [InlineData("(00) 3 0000-0000")]
        [InlineData("(11) 9000-0000")]
        [InlineData("(11) 3 0000-0000")]
        [InlineData("11")]
        [InlineData("11300000001130000000")]
        public void shouldReturnFalse(string value)
        {
            var result = _validatePhone.IsValidPhone(value);
            Assert.False(result);
        }

        [Theory]
        [InlineData("11 9 0000-0000")]
        [InlineData("(11) 9 0000-0000")]
        [InlineData("(11) 3000-0000")]
        [InlineData("11900000000")]
        [InlineData("1130000000")]
        public void shouldReturnTrue(string value)
        {
            var result = _validatePhone.IsValidPhone(value);
            Assert.True(result);
        }
    }
}