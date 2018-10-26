using BrazilianUtilsService.boleto.src;
using Xunit;

namespace BrazilianUtilsService.Tests.boleto.src
{
    public class ValidateBoletoTest
    {
        private readonly ValidateBoleto _validateBoleto;

        public ValidateBoletoTest()
        {
            _validateBoleto = new ValidateBoleto();
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("00190000020114971860168524522114675860000102656")]
        [InlineData("00190000090114971860168524522114975860000102656")]
        public void shouldReturnFalse(string value)
        {
            var result = _validateBoleto.IsValidBoleto(value);
            Assert.False(result);
        }

        [Theory]
        [InlineData("00190000090114971860168524522114675860000102656")]
        [InlineData("0019000009 01149.718601 68524.522114 6 75860000102656")]
        public void shouldReturnTrue(string value)
        {
            var result = _validateBoleto.IsValidBoleto(value);
            Assert.True(result);
        }
    }
}