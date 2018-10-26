using BrazilianUtilsService.cep.src;
using Xunit;

namespace BrazilianUtilsService.Tests.cep.src
{
    public class ValidateCepTest
    {
        private readonly ValidateCep _validateCep;

        public ValidateCepTest()
        {
            _validateCep = new ValidateCep();
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("123456789")]
        public void shouldReturnFalse(string value)
        {
            var result = _validateCep.IsValidCep(value);
            Assert.False(result);
        }

        [Theory]
        [InlineData("92500-000")]
        [InlineData("92500000")]
        public void shouldReturnTrue(string value)
        {
            var result = _validateCep.IsValidCep(value);
            Assert.True(result);
        }
    }
}