using BrazilianUtilsService.cpf.src;
using Xunit;

namespace BrazilianUtilsService.Tests.cpf.src
{
    public class ValidateCpfTest
    {
        private readonly ValidateCpf _validateCpf;

        public ValidateCpfTest()
        {
            _validateCpf = new ValidateCpf();
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
        [InlineData("123456")]
        [InlineData("ababcabcabcdab")]
        [InlineData("11257245286")]
        public void shouldReturnFalse(string value)
        {
            var result = _validateCpf.IsValidCpf(value);
            Assert.False(result);
        }

        [Theory]
        [InlineData("40364478829")]
        [InlineData("962.718.458-60")]
        public void ShouldReturnTrueWhenIsCpfvalid(string value)
        {
            var result = _validateCpf.IsValidCpf(value);
            Assert.True(result);
        }
    }
}