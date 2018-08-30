using System;
using System.Text.RegularExpressions;
using Xunit;

namespace BrazilianUtils.Tests
{
    public class CnpjTest
    {
        [Theory]
        [InlineData("81.202.136/0001-86")]
        [InlineData("46.238.497/0001-81")]
        [InlineData("18240603000126")]
        [InlineData("77973689000165")]
        public void ShouldBeValid(string value)
        {
            Assert.True(Cnpj.IsValid(value));
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("123456")]
        [InlineData("11257245286")]
        [InlineData("abcabcabcde")]
        [InlineData("11111111111")]
        [InlineData("77973689000163")]
        [InlineData("77173389000163")]
        public void ShouldBeInvalid(string value)
        {
            Assert.False(Cnpj.IsValid(value));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void ShouldBeValidWhenEmptyAndNotRequired(string value)
        {
            Assert.True(Cnpj.IsValid(value, isRequired: false));
        }

        [Theory]
        [InlineData("22938962000129")]
        [InlineData("42.999.072/0001-34")]
        public void ShouldFormattedStringMatchRegex(string value)
        {
            Assert.Matches(@"^[0-9]{2}(\.[0-9]{3}){2}\/[0-9]{4}\-[0-9]{2}$", Cnpj.Format(value));
        }
    }
}
