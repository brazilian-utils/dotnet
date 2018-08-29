using System;
using System.Text.RegularExpressions;
using Xunit;

namespace BrazilianUtils.Tests
{
    public class HelpersTest
    {
        [Theory]
        [InlineData("abc89320100^7")]
        [InlineData("021.154.020-06")]
        public void ShouldFormattedStringMatchRegex(string value)
        {
            Assert.Matches(@"^[0-9]*$", Helpers.OnlyNumbers(value));
        }
    }
}
