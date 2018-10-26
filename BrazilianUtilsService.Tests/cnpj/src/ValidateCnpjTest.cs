using BrazilianUtilsService.cnpj.src;
using Xunit;

namespace BrazilianUtilsService.Tests.cnpj.src
{
    public class ValidateCnpjTest
    {
        private readonly ValidateCnpj _validateCnpj;

        public ValidateCnpjTest()
        {
            _validateCnpj = new ValidateCnpj();
        }

        [Theory]
        [InlineData("00000000000000")]
        [InlineData("11111111111111")]
        [InlineData("22222222222222")]
        [InlineData("33333333333333")]
        [InlineData("44444444444444")]
        [InlineData("55555555555555")]
        [InlineData("66666666666666")]
        [InlineData("77777777777777")]
        [InlineData("88888888888888")]
        [InlineData("99999999999999")]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("12312312312")]
        [InlineData("ababcabcabcdab")]
        [InlineData("11257245286531")]
        public void shouldReturnFalse(string value)
        {
            var result = _validateCnpj.IsValidCnpj(value);
            Assert.False(result);
        }

        [Fact]
        public void ShouldReturnTrueWhenIsCnpjvalid()
        {
            var result = _validateCnpj.IsValidCnpj("13723705000189");
            Assert.True(result);
        }
    }
}