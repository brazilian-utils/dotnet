using Xunit;

namespace BrazilianUtils.Tests
{
    public class BoletoTest
    {
        [Theory]
        [InlineData("0019000009 01149.718601 68524.522114 6 75860000102656")]
        [InlineData("42297 03006 00002 695286 85088 140422 1 75840000044415")]
        [InlineData("34191000000000000001753980229122525005423000")]
        [InlineData("00198.10001 00030.212237 00217.236553 1 35742800321323")]
        public void ShouldBeValid(string value)
        {
            Assert.True(Boleto.IsValid(value));
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("0019000009 01149.718601 68524.522112 6 75860000102656")]
        [InlineData("42297 03006 00002 695286 85088 140422 9 75840000044415")]
        [InlineData("4191000000000000001753980229122525005423000")]
        [InlineData("00198.10005 00030.212237 00217.236553 1 35742800321323")]
        public void ShouldBeInvalid(string value)
        {
            Assert.False(Boleto.IsValid(value));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void ShouldBeValidWhenEmptyAndNotRequired(string cpfValue)
        {
            Assert.True(Boleto.IsValid(cpfValue, isRequired : false));
        }
    }
}
