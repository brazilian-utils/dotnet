using System;
using Xunit;

namespace BrazilianUtils.Tests
{
    public class CpfTest
    {
        [Theory]
        [InlineData("01234567890")]
        [InlineData("73615755790")]
        [InlineData("96818343481")]
        [InlineData("012.345.678-90")]
        [InlineData("308.411.120-02")]
        [InlineData("656.562.663-46")]
        public void ShouldBeValid(string cpfValue)
        {
            Assert.True(Cpf.IsValid(cpfValue));
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("123456")]
        [InlineData("11257245286")]
        [InlineData("abcabcabcde")]
        [InlineData("11111111111")]
        public void ShouldBeInvalid(string cpfValue)
        {
            Assert.False(Cpf.IsValid(cpfValue));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void ShouldBeValidWhenEmptyAndNotRequired(string cpfValue)
        {
            Assert.True(Cpf.IsValid(cpfValue, isRequired: false));
        }
    }
}
